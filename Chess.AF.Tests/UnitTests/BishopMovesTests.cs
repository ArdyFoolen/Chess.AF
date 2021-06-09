using AF.Functional;
using AF.Functional.Option;
using Chess.AF.Enums;
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
    public class BishopMovesTests
    {
        [TestCase("8/8/1P3P2/2p1p3/3B4/2p1p3/1P3P2/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.c5, SquareEnum.e5, SquareEnum.c3, SquareEnum.e3 })]
        [TestCase("8/8/1P3P2/8/3B4/8/1P3P2/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.c5, SquareEnum.e5, SquareEnum.c3, SquareEnum.e3 })]
        [TestCase("8/8/8/2p1p3/3B4/2p1p3/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.c5, SquareEnum.e5, SquareEnum.c3, SquareEnum.e3 })]
        [TestCase("8/8/1p3p2/2P1P3/3B4/2P1P3/1p3p2/8 w KQkq - 0 1", new SquareEnum[] { })]
        [TestCase("8/8/8/2P1P3/3B4/2P1P3/8/8 w KQkq - 0 1", new SquareEnum[] { })]
        [TestCase("8/8/1P3P2/2p1p3/3b4/2p1p3/1P3P2/8 b KQkq - 0 1", new SquareEnum[] { })]
        [TestCase("8/8/8/2p1p3/3b4/2p1p3/8/8 b KQkq - 0 1", new SquareEnum[] { })]
        [TestCase("8/8/1p3p2/2P1P3/3b4/2P1P3/1p3p2/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.c5, SquareEnum.e5, SquareEnum.c3, SquareEnum.e3 })]
        [TestCase("8/8/1p3p2/8/3b4/8/1p3p2/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.c5, SquareEnum.e5, SquareEnum.c3, SquareEnum.e3 })]
        [TestCase("8/8/8/2P1P3/3b4/2P1P3/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.c5, SquareEnum.e5, SquareEnum.c3, SquareEnum.e3 })]
        [TestCase("rn1qk1nr/ppp2ppp/1b4b1/3pp3/3PP3/1B4B1/PPP2PPP/RN1QK1NR w KQkq - 0 1", new SquareEnum[] { SquareEnum.a4, SquareEnum.c4, SquareEnum.d5, SquareEnum.h4, SquareEnum.f4, SquareEnum.e5 })]
        [TestCase("rn1qk1nr/ppp2ppp/1b4b1/3pp3/3PP3/1B4B1/PPP2PPP/RN1QK1NR b KQkq - 0 1", new SquareEnum[] { SquareEnum.a5, SquareEnum.c5, SquareEnum.d4, SquareEnum.h5, SquareEnum.f5, SquareEnum.e4 })]
        public void BishopMoves_AreValid(string fenString, SquareEnum[] expected)
        {
            AssertMovesHelper helper = new AssertMovesHelper();
            helper.AssertMovesFor(fenString, PieceEnum.Bishop, expected);
        }
    }
}
