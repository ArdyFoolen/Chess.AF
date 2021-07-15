using AF.Functional;
using Chess.AF.Commands;
using Chess.AF.Domain;
using Chess.AF.Dto;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;
using Unit = System.ValueTuple;

namespace Chess.AF.ImportExport
{
    public partial class Pgn
    {
        private class PgnCommandVisitor : ICommandVisitor
        {
            private string pgnString;
            public string PgnString
            {
                get => pgnString + $"{result.ToDisplayString()}{Environment.NewLine}{Environment.NewLine}";
            }

            private GameResult result;
            private int moveNumberCount;

            #region Public Methods

            public void Visit(LoadCommand command)
            {
                moveNumberCount = 0;
                setFirstIfBlackMoveNumber(command.Board);
            }

            public void Visit(MoveCommand command)
                => CreateMoveText(command);

            public void Visit(DrawCommand command)
                => CreateMoveText(command);

            public void Visit(ResignCommand command)
                => CreateMoveText(command);

            private void CreateMoveText(IMoveCommand command)
            {
                setMoveNumber(command.Previous);
                tryShowRokadeText(command);
                setResult(command);
            }

            #endregion

            #region Private Methods

            private void setResult(ICommand command)
                => result = command.Board.Match(
                    None: () => GameResult.Ongoing,
                    Some: b => b.Result);

            private void setFirstIfBlackMoveNumber(Option<IBoard> board)
                => pgnString += board.Match(
                    None: () => string.Empty,
                    Some: b =>
                    {
                        if (!b.IsWhiteToMove)
                            return $"{b.MoveNumber}... ";
                        return string.Empty;
                    });

            private void setMoveNumber(Option<IBoard> board)
                => pgnString += board.Match(
                    None: () => string.Empty,
                    Some: b => setMoveNumberText(b));

            private string setMoveNumberText(IBoard board)
            {
                if (!board.IsWhiteToMove)
                    return string.Empty;

                string newLine = (moveNumberCount > 0 && moveNumberCount % 8 == 0) ? Environment.NewLine : string.Empty;
                moveNumberCount += 1;

                return $"{newLine}{board.MoveNumber}. ";
            }

            private string showTake(IMoveCommand command)
                => command.Board.Match(
                    None: () => string.Empty,
                    Some: b =>
                    {
                        if (b.IsTake) return "x";
                        return string.Empty;
                    });

            private void showRokadeText(Move move)
            {
                if (RokadeEnum.KingSide.Equals(move.Rokade) || move.Piece.IsKingsideRokadeMove(move.From, move.To))
                    pgnString += $"O-O ";
                else
                    pgnString += $"O-O-O ";
            }

            private void setPgnString(IMoveCommand command, Move move, string fromText = "")
                => pgnString += $"{move.Piece.ToDisplayString()}{fromText}{showTake(command)}{move.To.ToDisplayString()}{showPromoteText(move)}{showCheckOrMate(command)} ";

            /// <summary>
            /// AllMoves contains no more than one where Piece == Piece, Show Piece + IsTake + To
            /// </summary>
            /// <param name="move"></param>
            private void setMoveText(IMoveCommand command, Move move)
                => setPgnString(command, move);

            /// <summary>
            /// AllMoves contains no more than one where Piece == Piece AND File is equal, Show Piece + file + IsTake + To
            /// </summary>
            /// <param name="move"></param>
            private void setFileMoveText(IMoveCommand command, Move move)
                => setPgnString(command, move, move.From.ToFileString());

            /// <summary>
            /// AllMoves contains no more than one where Piece == Piece AND Row is equal, Show Piece + row + IsTake + To
            /// </summary>
            /// <param name="move"></param>
            private void setRowMoveText(IMoveCommand command, Move move)
                => setPgnString(command, move, move.From.ToRowString());

            /// <summary>
            /// Show Piece + from + IsTake + To
            /// </summary>
            /// <param name="move"></param>
            private void setFileRowMoveTExt(IMoveCommand command, Move move)
                => setPgnString(command, move, move.From.ToDisplayString());

            private string showCheckOrMate(IMoveCommand command)
                => command.Board.Match(
                    None: () => string.Empty,
                    Some: b =>
                    {
                        if (b.IsMate) return "#";
                        if (b.IsInCheck) return "+";
                        return string.Empty;
                    });

            private string showPromoteText(Move move)
            {
                if (PieceEnum.Pawn.Equals(move.Piece) && !PieceEnum.Pawn.Equals(move.Promote))
                    return $"={move.Promote.ToDisplayString()}";

                return string.Empty;
            }

            private void tryShowRokadeText(IMoveCommand command)
                => command.Move.Map(m => tryShowRokadeText(command, m));

            private Unit tryShowRokadeText(IMoveCommand command, Move move)
            {
                if (PieceEnum.King.Equals(move.Piece) && !RokadeEnum.None.Equals(move.Rokade) || move.Piece.IsRokadeMove(move.From, move.To))
                    showRokadeText(move);
                else
                    tryShowMoveText(command, move);
                return Unit();
            }

            private void tryShowMoveText(IMoveCommand command, Move move)
            {
                if (AllMoves(command.Previous).Where(w => FilterOnPieceAndTo(w, move)).Count() == 1)
                    setMoveText(command, move);
                else
                    tryShowFileMoveText(command, move);
            }

            private void tryShowFileMoveText(IMoveCommand command, Move move)
            {
                if (AllMoves(command.Previous).Where(w => FilterOnPieceAndTo(w, move)).Where(w => move.From.File() == w.Square.File()).Count() == 1)
                    setFileMoveText(command, move);
                else
                    tryShowRowMoveText(command, move);
            }

            private void tryShowRowMoveText(IMoveCommand command, Move move)
            {
                if (AllMoves(command.Previous).Where(w => FilterOnPieceAndTo(w, move)).Where(w => move.From.Row() == w.Square.Row()).Count() == 1)
                    setRowMoveText(command, move);
                else
                    setFileRowMoveTExt(command, move);
            }

            private static Func<(PieceEnum Piece, SquareEnum From, PieceEnum Promoted, SquareEnum To), Move, bool> FilterOnPieceAndTo =
                (tuple, move) => move.Piece.Equals(tuple.Piece) && move.To.Equals(tuple.To) && move.Promote.Equals(tuple.Promoted);

            private IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> AllMoves(Option<IBoard> board)
                => board.Map(p => p.IterateForAllMoves()).Match(
                    None: () => Enumerable.Empty<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>(),
                    Some: s => s
                    );

            #endregion
        }
    }
}
