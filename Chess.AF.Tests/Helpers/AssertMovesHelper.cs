using AF.Functional;
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
            Option<Selected> selected = pieceOpt.Bind(piece => position.Bind(p => Selected.Of(piece, p.GetIteratorFor(piece))));

            selected.Match(
                None: () => Assert.Fail(),
                Some: s => position.Match(
                    None: () => Assert.Fail(),
                    Some: p => AssertSelected(s, p, expected.ToList())
                ));
        }

        private Unit AssertSelected(Selected selected, Position position, List<SquareEnum> expected)
        {
            int count = 0;
            foreach (var square in selected.Moves(position))
            {
                Assert.That(expected.Contains(square));
                count += 1;
            }
            Assert.AreEqual(expected.Count(), count);
            return Unit();
        }

    }
}
