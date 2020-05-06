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
    public class KingMovesTests
    {
        [TestCase("8/1K6/8/8/8/8/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.a8, SquareEnum.b8, SquareEnum.c8, SquareEnum.a7, SquareEnum.c7, SquareEnum.a6, SquareEnum.b6, SquareEnum.c6 })]
        [TestCase("8/1k6/8/8/8/8/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.a8, SquareEnum.b8, SquareEnum.c8, SquareEnum.a7, SquareEnum.c7, SquareEnum.a6, SquareEnum.b6, SquareEnum.c6 })]
        [TestCase("8/6K1/8/8/8/8/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.f8, SquareEnum.g8, SquareEnum.h8, SquareEnum.f7, SquareEnum.h7, SquareEnum.f6, SquareEnum.g6, SquareEnum.h6 })]
        [TestCase("8/6k1/8/8/8/8/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.f8, SquareEnum.g8, SquareEnum.h8, SquareEnum.f7, SquareEnum.h7, SquareEnum.f6, SquareEnum.g6, SquareEnum.h6 })]
        [TestCase("8/8/8/8/8/8/1K6/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.a1, SquareEnum.b1, SquareEnum.c1, SquareEnum.a2, SquareEnum.c2, SquareEnum.a3, SquareEnum.b3, SquareEnum.c3 })]
        [TestCase("8/8/8/8/8/8/1k6/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.a1, SquareEnum.b1, SquareEnum.c1, SquareEnum.a2, SquareEnum.c2, SquareEnum.a3, SquareEnum.b3, SquareEnum.c3 })]
        [TestCase("8/8/8/8/8/8/6K1/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.f1, SquareEnum.g1, SquareEnum.h1, SquareEnum.f2, SquareEnum.h2, SquareEnum.f3, SquareEnum.g3, SquareEnum.h3 })]
        [TestCase("8/8/8/8/8/8/6k1/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.f1, SquareEnum.g1, SquareEnum.h1, SquareEnum.f2, SquareEnum.h2, SquareEnum.f3, SquareEnum.g3, SquareEnum.h3 })]
        [TestCase("K7/8/8/8/8/8/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.b8, SquareEnum.a7, SquareEnum.b7 })]
        [TestCase("k7/8/8/8/8/8/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.b8, SquareEnum.a7, SquareEnum.b7 })]
        [TestCase("7K/8/8/8/8/8/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.g8, SquareEnum.g7, SquareEnum.h7 })]
        [TestCase("7k/8/8/8/8/8/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.g8, SquareEnum.g7, SquareEnum.h7 })]
        [TestCase("8/8/8/8/8/8/8/K7 w KQkq - 0 1", new SquareEnum[] { SquareEnum.a2, SquareEnum.b1, SquareEnum.b2 })]
        [TestCase("8/8/8/8/8/8/8/k7 b KQkq - 0 1", new SquareEnum[] { SquareEnum.a2, SquareEnum.b1, SquareEnum.b2 })]
        [TestCase("8/8/8/8/8/8/8/7K w KQkq - 0 1", new SquareEnum[] { SquareEnum.g1, SquareEnum.g2, SquareEnum.h2 })]
        [TestCase("8/8/8/8/8/8/8/7k b KQkq - 0 1", new SquareEnum[] { SquareEnum.g1, SquareEnum.g2, SquareEnum.h2 })]
        public void KingMoves_AreValid(string fenString, SquareEnum[] expected)
        {
            AssertMovesHelper helper = new AssertMovesHelper();
            helper.AssertMovesFor(fenString, PieceEnum.King, expected);
        }
    }
}
