using AF.Functional;
using Chess.AF.Dto;
using Chess.AF.Enums;
using Chess.AF.ImportExport;
using Chess.AF.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;
using static Chess.AF.Domain.Board;
using Unit = System.ValueTuple;

namespace Chess.AF.Tests.Helpers
{
    public class AssertMovesHelper
    {
        public void AssertMovesFor(string fenString, PieceEnum pieceEnum, SquareEnum[] expected)
        {
            Option<Fen> fen = Fen.Of(fenString);
            Option<IBoard> board = Board.Of(fen);

            board.Match(
                None: () => Assert.Fail(),
                Some: p => AssertSelected(FilterByPiece(p.IterateForAllMoves(), pieceEnum), expected.ToList())
            );
        }

        private IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> FilterByPiece(IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> moves, PieceEnum piece)
            => moves.Where(w => piece.IsEqual(w.Piece));

        private Unit AssertSelected(IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> moves, List<SquareEnum> expected)
        {
            int count = 0;
            foreach (var tuple in moves)
            {
                Assert.That(expected.Contains(tuple.MoveSquare));
                count += 1;
            }
            Assert.AreEqual(expected.Count(), count);
            return Unit();
        }

        public Unit AssertIterateForMoves(IBoard board, (PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)[] Expected)
        {
            int count = 0;
            foreach (var tuple in board.IterateForAllMoves())
            {
                Assert.IsTrue(Expected.Any(a => tuple.Piece.Equals(a.Piece) && tuple.Square.Equals(a.Square) && tuple.Promoted.Equals(a.Promoted) && tuple.MoveSquare.Equals(a.MoveSquare)));
                count += 1;
            }
            Assert.AreEqual(Expected.Length, count);

            return Unit();
        }

        public Unit AssertRokadeAfterMove(IBoard board, Move moveTo, RokadeEnum expected)
            => board.Move(moveTo).Match(
                None: () => Assert.Fail(),
                Some: s => Assert.AreEqual(GetOpponentColorRokade(s as Board), expected));

        private RokadeEnum GetOpponentColorRokade(Board board)
            => board.IsWhiteToMove ? board.BlackRokade : board.WhiteRokade;
    }
}
