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

            #region Public Methods

            public void Visit(LoadCommand command)
            {
                setFirstIfBlackMoveNumber(command.Board);
            }

            public void Visit(MoveCommand command)
            {
                setMoveNumber(command.Previous);
                tryShowRokadeTExt(command);
                setResult(command);
            }

            public void Visit(DrawCommand command)
            {
                setResult(command);
            }
            public void Visit(ResignCommand command)
            {
                setResult(command);
            }

            #endregion

            #region Private Methods

            private void setResult(Command command)
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
                    Some: b =>
                    {
                        if (b.IsWhiteToMove)
                            return $"{b.MoveNumber}. ";
                        return string.Empty;
                    });

            private string showTake(MoveCommand command)
                => command.Board.Match(
                    None: () => string.Empty,
                    Some: b =>
                    {
                        if (b.IsTake) return "x";
                        return string.Empty;
                    });

            private void showRokadeText(Move move)
            {
                if (move.Piece.IsKingsideRokadeMove(move.From, move.To))
                    pgnString += $"O-O ";
                else
                    pgnString += $"O-O-O ";
            }

            /// <summary>
            /// AllMoves contains no more than one where Piece == Piece, Show Piece + IsTake + To
            /// </summary>
            /// <param name="move"></param>
            private void showMoveText(MoveCommand command)
            {
                Move move = command.Move;
                pgnString += $"{move.Piece.ToDisplayString()}{showTake(command)}{move.To.ToDisplayString()}{showPromoteText(move)}{showCheckOrMate(command)} ";
            }

            /// <summary>
            /// AllMoves contains no more than one where Piece == Piece AND File is equal, Show Piece + file + IsTake + To
            /// </summary>
            /// <param name="move"></param>
            private void showFileMoveText(MoveCommand command)
            {
                Move move = command.Move;
                pgnString += $"{move.Piece.ToDisplayString()}{move.From.ToFileString()}{showTake(command)}{move.To.ToDisplayString()}{showPromoteText(move)}{showCheckOrMate(command)} ";
            }

            /// <summary>
            /// AllMoves contains no more than one where Piece == Piece AND Row is equal, Show Piece + row + IsTake + To
            /// </summary>
            /// <param name="move"></param>
            private void showRowMoveText(MoveCommand command)
            {
                Move move = command.Move;
                pgnString += $"{move.Piece.ToDisplayString()}{move.From.ToRowString()}{showTake(command)}{move.To.ToDisplayString()}{showPromoteText(move)}{showCheckOrMate(command)} ";
            }

            /// <summary>
            /// Show Piece + from + IsTake + To
            /// </summary>
            /// <param name="move"></param>
            private void showFileRowMoveTExt(MoveCommand command)
            {
                Move move = command.Move;
                pgnString += $"{move.Piece.ToDisplayString()}{move.From.ToDisplayString()}{showTake(command)}{move.To.ToDisplayString()}{showPromoteText(move)}{showCheckOrMate(command)} ";
            }

            private string showCheckOrMate(MoveCommand command)
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

            private void tryShowRokadeTExt(MoveCommand command)
            {
                Move move = command.Move;
                if (move.Piece.IsRokadeMove(move.From, move.To))
                    showRokadeText(move);
                else
                    tryShowMoveTExt(command);
            }

            private void tryShowMoveTExt(MoveCommand command)
            {
                Move move = command.Move;
                if (AllMoves(command.Previous).Where(w => FilterOnPieceAndTo(w, move)).Count() == 1)
                    showMoveText(command);
                else
                    tryShowFileMoveText(command);
            }

            private void tryShowFileMoveText(MoveCommand command)
            {
                Move move = command.Move;
                if (AllMoves(command.Previous).Where(w => FilterOnPieceAndTo(w, move)).Where(w => move.From.File() == w.Square.File()).Count() == 1)
                    showFileMoveText(command);
                else
                    tryShowRowMoveText(command);
            }

            private void tryShowRowMoveText(MoveCommand command)
            {
                Move move = command.Move;
                if (AllMoves(command.Previous).Where(w => FilterOnPieceAndTo(w, move)).Where(w => move.From.Row() == w.Square.Row()).Count() == 1)
                    showRowMoveText(command);
                else
                    showFileRowMoveTExt(command);
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
