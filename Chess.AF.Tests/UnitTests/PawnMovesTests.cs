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
    public class PawnMovesTests
    {
        [TestCase("8/p7/8/8/8/8/P7/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.a3, SquareEnum.a4 })]
        [TestCase("8/p7/8/8/8/8/P7/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.a6, SquareEnum.a5 })]
        [TestCase("8/1p6/8/8/8/8/1P6/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.b3, SquareEnum.b4 })]
        [TestCase("8/1p6/8/8/8/8/1P6/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.b6, SquareEnum.b5 })]
        [TestCase("8/2p5/8/8/8/8/2P5/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.c3, SquareEnum.c4 })]
        [TestCase("8/2p5/8/8/8/8/2P5/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.c6, SquareEnum.c5 })]
        [TestCase("8/3p4/8/8/8/8/3P4/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.d3, SquareEnum.d4 })]
        [TestCase("8/3p4/8/8/8/8/3P4/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.d6, SquareEnum.d5 })]
        [TestCase("8/4p3/8/8/8/8/4P3/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.e3, SquareEnum.e4 })]
        [TestCase("8/4p3/8/8/8/8/4P3/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.e6, SquareEnum.e5 })]
        [TestCase("8/5p2/8/8/8/8/5P2/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.f3, SquareEnum.f4 })]
        [TestCase("8/5p2/8/8/8/8/5P2/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.f6, SquareEnum.f5 })]
        [TestCase("8/6p1/8/8/8/8/6P1/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.g3, SquareEnum.g4 })]
        [TestCase("8/6p1/8/8/8/8/6P1/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.g6, SquareEnum.g5 })]
        [TestCase("8/7p/8/8/8/8/7P/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.h3, SquareEnum.h4 })]
        [TestCase("8/7p/8/8/8/8/7P/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.h6, SquareEnum.h5 })]
        [TestCase("8/8/p7/8/8/P7/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.a4 })]
        [TestCase("8/8/p7/8/8/P7/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.a5 })]
        [TestCase("8/8/1p6/8/8/1P6/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.b4 })]
        [TestCase("8/8/1p6/8/8/1P6/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.b5 })]
        [TestCase("8/8/2p5/8/8/2P5/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.c4 })]
        [TestCase("8/8/2p5/8/8/2P5/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.c5 })]
        [TestCase("8/8/3p4/8/8/3P4/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.d4 })]
        [TestCase("8/8/3p4/8/8/3P4/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.d5 })]
        [TestCase("8/8/4p3/8/8/4P3/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.e4 })]
        [TestCase("8/8/4p3/8/8/4P3/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.e5 })]
        [TestCase("8/8/5p2/8/8/5P2/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.f4 })]
        [TestCase("8/8/5p2/8/8/5P2/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.f5 })]
        [TestCase("8/8/6p1/8/8/6P1/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.g4 })]
        [TestCase("8/8/6p1/8/8/6P1/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.g5 })]
        [TestCase("8/8/7p/8/8/7P/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.h4 })]
        [TestCase("8/8/7p/8/8/7P/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.h5 })]
        [TestCase("8/8/8/3pp3/3PP3/8/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.d5, SquareEnum.e5 })]
        [TestCase("8/8/8/3pp3/3PP3/8/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.d4, SquareEnum.e4 })]
        [TestCase("8/8/8/3p1p2/4P3/8/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.d5, SquareEnum.e5, SquareEnum.f5 })]
        [TestCase("8/8/8/4p3/3P1P2/8/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.d4, SquareEnum.e4, SquareEnum.f4 })]
        public void PawnMoves_AreValid(string fenString, SquareEnum[] expected)
        {
            AssertMovesHelper helper = new AssertMovesHelper();
            helper.AssertMovesFor(fenString, PieceEnum.Pawn, expected);
        }
    }
}
