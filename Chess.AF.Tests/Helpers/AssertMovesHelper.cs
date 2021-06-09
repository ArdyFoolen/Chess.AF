using AF.Functional;
using Chess.AF.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;
using Unit = System.ValueTuple;

namespace Chess.AF.Tests.Helpers
{
    public class AssertMovesHelper
    {
        public void AssertMovesFor(string fenString, PieceEnum pieceEnum, SquareEnum[] expected)
        {
            Option<Fen> fen = Fen.Of(fenString);
            Option<Position> position = Position.Of(fen);
            Option<PieceEnum> pieceOpt = pieceEnum;
            Option<Selected> selected = pieceOpt.Bind(piece => position.Bind(p => Selected.Of(piece, p.GetIteratorFor(piece), p)));

            selected.Match(
                None: () => Assert.Fail(),
                Some: s => position.Match(
                    None: () => Assert.Fail(),
                    Some: p => AssertSelected(s, expected.ToList())
                ));
        }

        private Unit AssertSelected(Selected selected, List<SquareEnum> expected)
        {
            int count = 0;
            foreach (var square in selected.Moves())
            {
                Assert.That(expected.Contains(square));
                count += 1;
            }
            Assert.AreEqual(expected.Count(), count);
            return Unit();
        }

        public Unit AssertIterateForMoves(Position position, (PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)[] Expected)
        {
            int count = 0;
            foreach (var tuple in position.IterateForAllMoves())
            {
                Assert.IsTrue(Expected.Any(a => tuple.Piece.Equals(a.Piece) && tuple.Square.Equals(a.Square) && tuple.Promoted.Equals(a.Promoted) && tuple.MoveSquare.Equals(a.MoveSquare)));
                count += 1;
            }
            Assert.AreEqual(Expected.Length, count);

            return Unit();
        }

        public Unit AssertRokadeAfterMove(Position position, Move moveTo, RokadeEnum expected)
            => position.Move(moveTo).Match(
                None: () => Assert.Fail(),
                Some: s => Assert.AreEqual(GetOpponentColorRokade(s), expected));

        private RokadeEnum GetOpponentColorRokade(Position position)
            => position.IsWhiteToMove ? position.BlackRokade : position.WhiteRokade;
    }
}
