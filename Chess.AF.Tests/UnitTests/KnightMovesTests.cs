using AF.Functional;
using Chess.AF.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;
using Unit = System.ValueTuple;

namespace Chess.AF.Tests.UnitTests
{
    public class KnightMovesTests
    {
        [TestCase("8/8/2N5/8/8/8/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.b8, SquareEnum.d8, SquareEnum.a7, SquareEnum.e7, SquareEnum.a5, SquareEnum.e5, SquareEnum.b4, SquareEnum.d4 })]
        [TestCase("8/8/2n5/8/8/8/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.b8, SquareEnum.d8, SquareEnum.a7, SquareEnum.e7, SquareEnum.a5, SquareEnum.e5, SquareEnum.b4, SquareEnum.d4 })]
        [TestCase("8/8/5N2/8/8/8/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.e8, SquareEnum.g8, SquareEnum.h7, SquareEnum.d7, SquareEnum.h5, SquareEnum.d5, SquareEnum.g4, SquareEnum.e4 })]
        [TestCase("8/8/5n2/8/8/8/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.e8, SquareEnum.g8, SquareEnum.h7, SquareEnum.d7, SquareEnum.h5, SquareEnum.d5, SquareEnum.g4, SquareEnum.e4 })]
        [TestCase("8/8/8/8/8/2N5/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.b1, SquareEnum.d1, SquareEnum.a2, SquareEnum.e2, SquareEnum.a4, SquareEnum.e4, SquareEnum.b5, SquareEnum.d5 })]
        [TestCase("8/8/8/8/8/2n5/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.b1, SquareEnum.d1, SquareEnum.a2, SquareEnum.e2, SquareEnum.a4, SquareEnum.e4, SquareEnum.b5, SquareEnum.d5 })]
        [TestCase("8/8/8/8/8/5N2/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.g1, SquareEnum.e1, SquareEnum.h2, SquareEnum.d2, SquareEnum.h4, SquareEnum.d4, SquareEnum.g5, SquareEnum.e5 })]
        [TestCase("8/8/8/8/8/5n2/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.g1, SquareEnum.e1, SquareEnum.h2, SquareEnum.d2, SquareEnum.h4, SquareEnum.d4, SquareEnum.g5, SquareEnum.e5 })]
        [TestCase("N7/8/8/8/8/8/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.b6, SquareEnum.c7 })]
        [TestCase("n7/8/8/8/8/8/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.b6, SquareEnum.c7 })]
        [TestCase("7N/8/8/8/8/8/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.g6, SquareEnum.f7 })]
        [TestCase("7n/8/8/8/8/8/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.g6, SquareEnum.f7 })]
        [TestCase("8/8/8/8/8/8/8/N7 w KQkq - 0 1", new SquareEnum[] { SquareEnum.b3, SquareEnum.c2 })]
        [TestCase("8/8/8/8/8/8/8/n7 b KQkq - 0 1", new SquareEnum[] { SquareEnum.b3, SquareEnum.c2 })]
        [TestCase("8/8/8/8/8/8/8/7N w KQkq - 0 1", new SquareEnum[] { SquareEnum.g3, SquareEnum.f2 })]
        [TestCase("8/8/8/8/8/8/8/7n b KQkq - 0 1", new SquareEnum[] { SquareEnum.g3, SquareEnum.f2 })]
        public void KnightMoves_AreValid(string fenString, SquareEnum[] expected)
        {
            AssertMovesHelper helper = new AssertMovesHelper();
            helper.AssertMovesFor(fenString, PieceEnum.Knight, expected);
        }
    }
}
