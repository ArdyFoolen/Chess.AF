﻿using AF.Functional;
using Chess.AF.Enums;
using System.Collections.Generic;
using Unit = System.ValueTuple;
using static AF.Functional.F;
using System.Linq;
using Chess.AF.Dto;
using System;

namespace Chess.AF.ImportExport
{
    public partial class Pgn
    {
        private class PgnImportBuilder : PgnBuilder, IPgnTagStateContext
        {
            private bool isValid = true;
            private GameResult gameResult;
            public IGame Game { get; private set; } = new Game();

            private Option<string[]> tagPairsMoveText;
            private Option<Dictionary<string, string>> tagPairDict;
            private string PgnString { get; set; }

            PgnTagState IPgnTagStateContext.State { get; set; }

            public PgnImportBuilder(string pgnString)
            {
                this.PgnString = pgnString;
            }

            private void SetInitialState(IPgnTagStateContext context)
                => context.State = new PgnTagEventState();
            public bool TryAddTagPair(IPgnTagStateContext context, Dictionary<string, string> eventTags, KeyValuePair<string, string> kv)
                => context.State.TryAddTagPair(context, eventTags, kv);

            public override void BuildPrepare()
                => tagPairsMoveText = splitPgnStringIntoTagPairsMoveText();

            public override void BuildTagPairs()
                => tagPairDict = splitTagPairs();

            public override void Build()
            {
                createGameFrom();
                if (isValid)
                {
                    var pgn = new Pgn(PgnString);
                    pgn.Game = Game;
                    tagPairDict.Map(t => pgn.TagPairDictionary = t);
                    this.Pgn = Some(pgn);
                }
            }

            private void createGameFrom()
                => tagPairsMoveText.Bind(m => createGameFrom(m[1]));

            private Option<Unit> createGameFrom(string moveText)
            {
                loadGame();
                moveText = sanitizeMoveText(moveText);
                var moves = splitMoves(moveText);
                makeMovesToGame(moves);
                return None;
            }

            private void makeMovesToGame(Option<List<string>> moves)
                => moves.Map(m => makeMovesToGame(m));

            private IGame makeMovesToGame(List<string> moves)
            {
                foreach (string move in moves)
                {
                    var halfMoves = move.Split();
                    foreach (string halfMove in halfMoves)
                        makeMoveToGame(halfMove);
                }

                setFinalResult();

                return Game;
            }

            private void setFinalResult()
            {
                if (GameResult.Draw.Equals(gameResult))
                    Game.Draw();
                else if (GameResult.BlackWins.Equals(gameResult) || GameResult.WhiteWins.Equals(gameResult))
                    Game.Resign();
            }

            private void makeMoveToGame(string halfMove)
            {
                Option<Move> toMove = None;
                toMove = GetRokadeMoveWhenValid(halfMove).Match(
                    None: () => GetNoneRokadeMoveWhenValid(halfMove),
                    Some: m => m);
                toMove.Map(m => Move(m));
            }

            private Option<Move> GetNoneRokadeMoveWhenValid(string halfMove)
            {
                var parts = dissectHalfMoveIntoParts(halfMove);
                var selectedMove = selectMove(parts).FirstOrDefault();
                return AF.Dto.Move.Of(selectedMove.Piece, selectedMove.Square, selectedMove.MoveSquare, selectedMove.Promoted);
            }

            private Option<Move> GetRokadeMoveWhenValid(string halfMove)
            {
                if ("O-O".Equals(halfMove))
                    return AF.Dto.Move.Of(RokadeEnum.KingSide);
                if ("O-O-O".Equals(halfMove))
                    return AF.Dto.Move.Of(RokadeEnum.QueenSide);
                return None;
            }

            private Unit Move(AF.Dto.Move move)
            {
                Game.Move(move);
                return Unit();
            }

            private IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> selectMove(
                (Option<PieceEnum> Piece, string from, PieceEnum Promoted, string MoveSquare) parts)
            {
                var iter = selectPieceMoves(parts.Piece);
                iter = selectMoveSquareMoves(iter, parts.MoveSquare);
                iter = selectFromSquareMoves(iter, parts.from);
                iter = selectPromotedMove(iter, parts.Promoted);
                return iter;
            }

            private IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> selectPromotedMove(
                IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> iter, PieceEnum promoted)
                => iter.Where(w => promoted.Equals(w.Promoted));

            private IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> selectFromSquareMoves(
                IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> iter, string from)
            {
                if (string.IsNullOrWhiteSpace(from))
                    return iter;
                if (from.Length == 2)
                    return from.ToSquare().Match(
                        None: () => Enumerable.Empty<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>(),
                        Some: s => iter.Where(w => s.Equals(w.Square)));
                if (from[0] >= 'a' && from[0] <= 'h')
                    return iter.Where(w => w.Square.File() == ((int)from[0] - (int)('a')));
                if (from[0] >= '1' && from[0] <= '8')
                    return iter.Where(w => w.Square.Row() == ((int)from[0] - (int)('1')));
                return Enumerable.Empty<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>();
            }

            private IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> selectMoveSquareMoves(
                IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> iter, string moveSquare)
                => moveSquare.ToSquare().Match(
                    None: () => Enumerable.Empty<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>(),
                    Some: s => iter.Where(w => s.Equals(w.MoveSquare)));

            private IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> selectPieceMoves(Option<PieceEnum> piece)
                => piece.Match(
                    None: () => Enumerable.Empty<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>(),
                    Some: p => Game.AllMoves().Where(w => p.Equals(w.Piece)));

            private (Option<PieceEnum> Piece, string from, PieceEnum Promoted, string MoveSquare) dissectHalfMoveIntoParts(string halfMove)
            {
                halfMove = sanitizeHalfMove(halfMove);
                var piece = halfMove.FromSanToPiece();
                var promoted = halfMove.FromSanToPromoted(piece);
                halfMove = sanitizeHalfMove(halfMove, piece);
                var fromToInfo = splitFromAndToSquare(halfMove);
                return (piece, fromToInfo.from, promoted, fromToInfo.moveSquare);
            }

            private (string from, string moveSquare) splitFromAndToSquare(string halfMove)
            {
                string from = halfMove.Substring(0, halfMove.Length - 2);
                string to = halfMove.Substring(halfMove.Length - 2);
                return (from, to);
            }

            private string sanitizeHalfMove(string halfMove)
                => halfMove.Replace("x", string.Empty).Replace("-", string.Empty).Replace("+", string.Empty);

            private string sanitizeHalfMove(string halfMove, Option<PieceEnum> piece)
                => piece.Match(
                    None: () => halfMove,
                    Some: p => sanitizeHalfMove(halfMove, p));

            private string sanitizeHalfMove(string halfMove, PieceEnum piece)
            {
                return piece.Equals(PieceEnum.Pawn)
                    ? halfMove.Split('=')[0]
                    : halfMove.Split('=')[0].Substring(1);
            }

            private void loadGame()
                => tagPairDict.Map(d => loadGame(d));

            private IGame loadGame(Dictionary<string, string> dict)
            {
                if (dict.ContainsKey(nameof(FenSetupEnum.Setup).ToLowerInvariant()) && dict[nameof(FenSetupEnum.Setup).ToLowerInvariant()].Equals("1") && dict.ContainsKey(nameof(FenSetupEnum.FEN).ToLowerInvariant()))
                    Game.Load(dict[nameof(FenSetupEnum.FEN).ToLowerInvariant()]);
                else
                    Game.Load();
                return Game;
            }

            private Option<List<string>> splitMoves(string moveText)
            {
                List<string> moves = new List<string>();
                var splits = moveText.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var indexes = generateMoveIndexes(splits);
                int index = 0;
                int prevMoveNbr = 0;
                while (index < indexes.Count())
                {
                    var i1 = indexes[index];
                    var i2 = index + 1 < indexes.Count() ? indexes[index + 1] : 999;
                    var take = i2 - i1 - 1;
                    if (!int.TryParse(splits[i1].Replace(".", string.Empty), out int moveNbr))
                        return None;
                    if (moveNbr == prevMoveNbr)
                        moves[moves.Count() - 1] = string.Join(" ", moves[moves.Count() - 1], string.Join(" ", splits.Skip(i1 + 1).Take(take).ToArray()));

                    else
                        moves.Add(string.Join(" ", splits.Skip(i1 + 1).Take(take).ToArray()));

                    prevMoveNbr = moveNbr;
                    index += 1;
                }

                return Some(moves);
            }

            private List<int> generateMoveIndexes(string[] splits)
                => splits.Select((s, i) => new { str = s, index = i }).Where(w => w.str.Contains(".")).Select(x => x.index).ToList();

            private string sanitizeMoveText(string moveText)
            {
                var lines = splitMoveTextIntoLines(moveText);
                moveText = string.Join(" ", removeCommentsEndToLine(lines).ToArray());
                moveText = removeCommentsMultipleLines(moveText);
                SetGameResult(moveText);
                moveText = RemoveGameResult(moveText);
                moveText = ReplaceThreeDots(moveText);
                moveText = ReplaceSingleDot(moveText);
                return moveText;
            }

            private string ReplaceSingleDot(string moveText)
                => moveText.Replace(".", ". ");

            private string ReplaceThreeDots(string moveText)
                => moveText.Replace("...", ".");

            private string RemoveGameResult(string moveText)
            {
                var words = moveText.Split();
                return string.Join(" ", words.Take(words.Length - 1).Where(w => !string.IsNullOrWhiteSpace(w)));
            }

            private void SetGameResult(string moveText)
                => gameResult = moveText.Split().Reverse().Take(1).FirstOrDefault().ToGameResult();

            private string removeCommentsMultipleLines(string moveTest)
            {
                var splits = moveTest.Split(new char[] { '{', '}' });
                var lines = splits.Select((s, i) => new { line = s, index = i }).Where(w => w.index % 2 == 0).Select(x => x.line).ToArray();
                return string.Join(" ", lines);
            }

            private IEnumerable<string> removeCommentsEndToLine(string[] lines)
            {
                foreach (var line in lines)
                    yield return line.Substring(0, indexOfComment(line));
            }

            private int indexOfComment(string line)
            {
                int index = line.IndexOf(';');
                return (index >= 0) ? index : line.Length;
            }

            private string[] splitMoveTextIntoLines(string moveText)
                => moveText.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            private Option<Dictionary<string, string>> splitTagPairs()
                => tagPairsMoveText.Bind(m => splitTagPairs(m[0]));

            private Option<Dictionary<string, string>> splitTagPairs(string tagPair)
            {
                var splits = tagPair.Split(new string[] { "]\n[", "]\r\n[", "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
                return splitTagPairs(splits);
            }

            private Option<Dictionary<string, string>> splitTagPairs(string[] parts)
            {
                SetInitialState(this);

                var eventTags = new Dictionary<string, string>();
                foreach (string tagPair in parts)
                {
                    var kv = splitTagPair(tagPair);
                    if (!TryAddTagPair(this, eventTags, kv))
                    {
                        isValid = false;
                        return None;
                    }
                }
                if (!isValidSevenTagRosterCount(eventTags))
                    return None;
                return Some(eventTags);
            }

            private KeyValuePair<string, string> splitTagPair(string tagPair)
            {
                int index = tagPair.IndexOf(' ');
                string tag = tagPair.Substring(0, index).Trim();
                string value = cleanupValue(tagPair.Substring(index));
                return new KeyValuePair<string, string>(tag, value);
            }

            public string cleanupValue(string value)
                => value.Substring(0, value.Length - 1).Trim().Substring(1);

            private Option<string[]> splitPgnStringIntoTagPairsMoveText()
            {
                var splits = PgnString.Split(new string[] { "\n\n", "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                return isValidTagPairsMoveText(splits) ? Some(splits) : None;
            }

            private bool isValidSevenTagRosterCount(Dictionary<string, string> dict)
                => isValid = isValid && dict.Keys.Count() >= 7;

            private bool isValidTagPairsMoveText(string[] parts)
                => isValid = isValid && parts.Length == 2;
        }
    }
}
