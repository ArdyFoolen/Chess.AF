using Chess.AF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Functional;
using static AF.Functional.F;
using Chess.AF.Domain;
using Chess.AF.Enums;
using Chess.AF.Dto;

namespace Chess.AF.ImportExport
{
    public partial class Pgn
    {
        private class PgnExportBuilder : PgnBuilder
        {
            private IGame Game;
            private IList<Command> Commands;
            private Dictionary<string, string> tagPairDict;
            private string PgnString { get; set; }

            public PgnExportBuilder(IGame game, IList<Command> commands)
            {
                this.Game = game;
                this.Commands = commands;
            }

            private string getResultFromLastCommand()
            {
                if (Commands == null || Commands.Count() == 0)
                    return string.Empty;

                return Commands[Commands.Count() - 1].Board.Match(
                    None: () => string.Empty,
                    Some: b => b.Result.ToDisplayString()
                    );
            }

            private void formatTagPairsToString()
                => PgnString = string.Join(Environment.NewLine, tagPairDict.Select(kv => formatTagPairsToString(kv))) +
                    Environment.NewLine + Environment.NewLine;

            private string formatTagPairsToString(KeyValuePair<string, string> tagPair)
                => $"[{tagPair.Key} \"{tagPair.Value}\"]";

            private void addSevenTagRosterTagPairs()
            {
                tagPairDict.Add(nameof(SevenTagRosterEnum.Event), "Chess.AF Game");
                tagPairDict.Add(nameof(SevenTagRosterEnum.Site), "");
                tagPairDict.Add(nameof(SevenTagRosterEnum.Date), $"{DateTime.Today.ToString("yyyy.MM.dd")}");
                tagPairDict.Add(nameof(SevenTagRosterEnum.Round), "");
                tagPairDict.Add(nameof(SevenTagRosterEnum.White), "");
                tagPairDict.Add(nameof(SevenTagRosterEnum.Black), "");
                tagPairDict.Add(nameof(SevenTagRosterEnum.Result), $"{getResultFromLastCommand()}");
            }

            private void addSetupTagPairs()
            {
                if (Commands == null || Commands.Count() == 0)
                    return;

                addSetupTagPair(Commands[0]);
            }

            private void addSetupTagPair(Command command)
            {
                LoadCommand loadCommand = command as LoadCommand;
                if (loadCommand == null || loadCommand.IsDefaultFen)
                    return;

                tagPairDict.Add(nameof(FenSetupEnum.Setup), "1");
                tagPairDict.Add(nameof(FenSetupEnum.FEN), loadCommand.Fen);
            }

            private void setIfFirstIsBlackMove(bool whiteToMove)
            {
                if (whiteToMove)
                    return;

                PgnString += "1... ";
            }

            private void CreateResultText()
                => PgnString += $"{getResultFromLastCommand()}{Environment.NewLine}{Environment.NewLine}";

            private void setMoveNumber()
                => PgnString += $"{Game.MoveNumber}. ";

            private string showTake()
                => Game.IsTake ? "x" : string.Empty;

            private void showRokadeText(Move move)
            {
                if (move.Piece.IsKingsideRokadeMove(move.From, move.To))
                    PgnString += $"O-O ";
                else
                    PgnString += $"O-O-O ";
            }

            /// <summary>
            /// AllMoves contains no more than one where Piece == Piece, Show Piece + IsTake + To
            /// </summary>
            /// <param name="move"></param>
            private void showMoveText(Move move)
                => PgnString += $"{move.Piece.ToDisplayString()}{showTake()}{move.To.ToDisplayString()}{showPromoteText(move)}{showCheckOrMate()} ";

            /// <summary>
            /// AllMoves contains no more than one where Piece == Piece AND File is equal, Show Piece + file + IsTake + To
            /// </summary>
            /// <param name="move"></param>
            private void showFileMoveText(Move move)
                => PgnString += $"{move.Piece.ToDisplayString()}{move.From.ToFileString()}{showTake()}{move.To.ToDisplayString()}{showPromoteText(move)}{showCheckOrMate()} ";

            /// <summary>
            /// AllMoves contains no more than one where Piece == Piece AND Row is equal, Show Piece + row + IsTake + To
            /// </summary>
            /// <param name="move"></param>
            private void showRowMoveText(Move move)
                => PgnString += $"{move.Piece.ToDisplayString()}{move.From.ToRowString()}{showTake()}{move.To.ToDisplayString()}{showPromoteText(move)}{showCheckOrMate()} ";

            /// <summary>
            /// Show Piece + from + IsTake + To
            /// </summary>
            /// <param name="move"></param>
            private void showFileRowMoveTExt(Move move)
                => PgnString += $"{move.Piece.ToDisplayString()}{move.From.ToDisplayString()}{showTake()}{move.To.ToDisplayString()}{showPromoteText(move)}{showCheckOrMate()} ";

            private string showCheckOrMate()
            {
                if (Game.NextIsMate)
                    return "#";
                if (Game.NextIsCheck)
                    return "+";

                return string.Empty;
            }

            private string showPromoteText(Move move)
            {
                if (PieceEnum.Pawn.Equals(move.Piece) && !PieceEnum.Pawn.Equals(move.Promote))
                    return $"={move.Promote.ToDisplayString()}";

                return string.Empty;
            }

            private void tryShowRokadeTExt(Move move)
            {
                if (move.Piece.IsRokadeMove(move.From, move.To))
                    showRokadeText(move);
                else
                    tryShowMoveTExt(move);
            }

            private void tryShowMoveTExt(Move move)
            {
                if (Game.AllMoves().Where(w => FilterOnPieceAndTo(w, move)).Count() == 1)
                    showMoveText(move);
                else
                    tryShowFileMoveText(move);
            }

            private void tryShowFileMoveText(Move move)
            {
                if (Game.AllMoves().Where(w => FilterOnPieceAndTo(w, move)).Where(w => move.From.File() == w.Square.File()).Count() == 1)
                    showFileMoveText(move);
                else
                    tryShowRowMoveText(move);
            }

            private void tryShowRowMoveText(Move move)
            {
                if (Game.AllMoves().Where(w => FilterOnPieceAndTo(w, move)).Where(w => move.From.Row() == w.Square.Row()).Count() == 1)
                    showRowMoveText(move);
                else
                    showFileRowMoveTExt(move);
            }

            private static Func<(PieceEnum Piece, SquareEnum From, PieceEnum Promoted, SquareEnum To), Move, bool> FilterOnPieceAndTo =
                (tuple, move) => move.Piece.Equals(tuple.Piece) && move.To.Equals(tuple.To) && move.Promote.Equals(tuple.Promoted);


            private void CreateMoveText(Command command)
            {
                var move = command as MoveCommand;
                if (move == null)
                    return;

                if (this.Game.IsWhiteToMove)
                    setMoveNumber();

                tryShowRokadeTExt(move.Move);
            }

            private void CreateMoveText()
            {
                if (Commands == null || Commands.Count() == 0)
                    return;

                this.Game.GotoFirstMove();
                int plyNbr = 1;
                setIfFirstIsBlackMove(this.Game.IsWhiteToMove);

                for (int i = plyNbr; i < Commands.Count(); i++)
                {
                    CreateMoveText(Commands[i]);

                    this.Game.GotoNextMove();
                    plyNbr += 1;
                }

                CreateResultText();
            }

            public override void BuildPrepare()
            {
            }

            public override void BuildTagPairs()
            {
                tagPairDict = new Dictionary<string, string>();

                addSevenTagRosterTagPairs();
                addSetupTagPairs();

                formatTagPairsToString();
            }

            public override void Build()
            {
                CreateMoveText();

                var pgn = new Pgn(PgnString);
                pgn.Game = this.Game;
                this.Pgn = Some(pgn);
            }
        }
    }
}