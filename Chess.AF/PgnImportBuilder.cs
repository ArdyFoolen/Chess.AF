using AF.Functional;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;
using Unit = System.ValueTuple;

namespace Chess.AF
{
    public partial class Pgn
    {
        private class PgnImportBuilder : PgnBuilder
        {
            private bool isValid = true;
            private GameResult gameResult;
            public Game Game { get; private set; } = new Game();

            private Option<string[]> tagPairsMoveText;
            private Option<Dictionary<string, string>> tagPairDict;
            public override void BuildPrepare()
                => tagPairsMoveText = splitPgnStringIntoTagPairsMoveText();

            public override void BuildTagPairs()
                => tagPairDict = splitTagPairs();

            public override void BuildMoveText()
            {
                createGameFrom();
                if (isValid)
                {
                    var pgn = new Pgn(PgnString);
                    pgn.Game = Game;
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

            private Game makeMovesToGame(List<string> moves)
            {
                foreach (string move in moves)
                {
                    var halfMoves = move.Split();
                    foreach (string halfMove in halfMoves)
                        makeMoveToGame(halfMove);
                }
                return Game;
            }

            private void makeMoveToGame(string halfMove)
            {
                var parts = dissectHalfMoveIntoParts(halfMove);
                var selectedMove = selectMove(parts).FirstOrDefault();
                var toMove = AF.Move.Of(selectedMove.Piece, selectedMove.Square, selectedMove.MoveSquare, selectedMove.Promoted, RokadeEnum.None);
                toMove.Map(m => Move(m));
            }

            private Unit Move(AF.Move move)
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
            {
                if ("O-O".Equals(moveSquare))
                    return iter.Where(w => w.MoveSquare.File() == 6);
                if ("O-O-O".Equals(moveSquare))
                    return iter.Where(w => w.MoveSquare.File() == 2);
                return moveSquare.ToSquare().Match(
                    None: () => Enumerable.Empty<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>(),
                    Some: s => iter.Where(w => s.Equals(w.MoveSquare)));
            }

            private IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> selectPieceMoves(Option<PieceEnum> piece)
                => piece.Match(
                    None: () => Enumerable.Empty<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>(),
                    Some: p => Game.AllMoves().Where(w => p.Equals(w.Piece)));

            private (Option<PieceEnum> Piece, string from, PieceEnum Promoted, string MoveSquare) dissectHalfMoveIntoParts(string halfMove)
            {
                var piece = halfMove.FromSanToPiece();
                var promoted = halfMove.FromSanToPromoted(piece);
                halfMove = sanitizeHalfMove(halfMove, piece);
                var fromToInfo = splitFromAndToSquare(halfMove);
                return (piece, fromToInfo.from, promoted, fromToInfo.moveSquare);
            }

            private (string from, string moveSquare) splitFromAndToSquare(string halfMove)
            {
                if (!halfMove.StartsWith("O-O"))
                {
                    string from = halfMove.Substring(0, halfMove.Length - 2);
                    string to = halfMove.Substring(halfMove.Length - 2);
                    return (from, to);
                }
                return (string.Empty, halfMove);
            }

            private string sanitizeHalfMove(string halfMove, Option<PieceEnum> piece)
                => piece.Match(
                    None: () => halfMove,
                    Some: p => sanitizeHalfMove(halfMove, p));

            private string sanitizeHalfMove(string halfMove, PieceEnum piece)
            {
                if (!halfMove.StartsWith("O-O"))
                    return piece.Equals(PieceEnum.Pawn)
                        ? halfMove.Replace("x", string.Empty).Replace("-", string.Empty).Replace("+", string.Empty).Split('=')[0]
                        : halfMove.Replace("x", string.Empty).Replace("-", string.Empty).Replace("+", string.Empty).Split('=')[0].Substring(1);
                return halfMove;
            }

            private void loadGame()
                => tagPairDict.Map(d => loadGame(d));

            private Game loadGame(Dictionary<string, string> dict)
            {
                if (dict.ContainsKey("setup") && dict["setup"].Equals("1") && dict.ContainsKey("fen"))
                    Game.Load(dict["fen"]);
                else
                    Game.Load();
                return Game;
            }

            private Option<List<string>> splitMoves(string moveText)
            {
                List<string> moves = new List<string>();
                var splits = moveText.Split(' ');
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
                return moveText;
            }

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
                var dict = splitTagPairs(splits);
                if (!isValidSevenTagRoster(dict))
                    return None;
                return Some(dict);
            }

            private Dictionary<string, string> splitTagPairs(string[] parts)
            {
                var eventValues = new Dictionary<string, string>();
                foreach (string tagPair in parts)
                {
                    var kv = splitTagPair(tagPair);
                    eventValues.Add(kv.Key.ToLowerInvariant(), kv.Value);
                }
                return eventValues;
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

            private bool isValidSevenTagRoster(Dictionary<string, string> dict)
                => isValid = isValid && dict.Keys.Count() >= 7 && AreValidSevenTagRosterKeys(dict);

            private bool AreValidSevenTagRosterKeys(Dictionary<string, string> dict)
            {
                foreach (string str in Enum.GetNames(typeof(SevenTagRosterEnum)))
                    if (!dict.ContainsKey(str.ToLowerInvariant()))
                        return false;
                return true;
            }

            private bool isValidTagPairsMoveText(string[] parts)
                => isValid = isValid && parts.Length == 2;
        }
    }
}
