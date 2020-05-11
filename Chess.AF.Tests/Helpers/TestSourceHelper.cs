using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Tests.Helpers
{
    public class TestSourceHelper
    {
        public static IEnumerable<(string FenString, (PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)[] Expected)> MovesTestCases
        {
            get
            {
                yield return ("4k3/8/8/8/7b/8/8/4K1B1 w KQkq - 0 1",
                    new (PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)[]
                    {
                        (PieceEnum.Bishop, SquareEnum.g1, PieceEnum.Bishop, SquareEnum.f2),
                        (PieceEnum.King, SquareEnum.e1, PieceEnum.King, SquareEnum.d1),
                        (PieceEnum.King, SquareEnum.e1, PieceEnum.King, SquareEnum.e2),
                        (PieceEnum.King, SquareEnum.e1, PieceEnum.King, SquareEnum.d2),
                        (PieceEnum.King, SquareEnum.e1, PieceEnum.King, SquareEnum.f1)
                    });
            }
        }
    }
}
