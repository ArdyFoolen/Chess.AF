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
    public class QueenMovesTests
    {
        [TestCase("8/8/1P1P1P2/2ppp3/1PpQpP2/2ppp3/1P1P1P2/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.c3, SquareEnum.c4, SquareEnum.c5, SquareEnum.e3, SquareEnum.e4, SquareEnum.e5, SquareEnum.d3, SquareEnum.d5 })]
        [TestCase("8/8/1P1P1P2/8/1P1Q1P2/8/1P1P1P2/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.c3, SquareEnum.c4, SquareEnum.c5, SquareEnum.e3, SquareEnum.e4, SquareEnum.e5, SquareEnum.d3, SquareEnum.d5 })]
        [TestCase("8/8/8/2ppp3/2pQp3/2ppp3/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.c3, SquareEnum.c4, SquareEnum.c5, SquareEnum.e3, SquareEnum.e4, SquareEnum.e5, SquareEnum.d3, SquareEnum.d5 })]
        [TestCase("8/8/1p1p1p2/2PPP3/1pPQPp2/2PPP3/1p1p1p2/8 w KQkq - 0 1", new SquareEnum[] { })]
        [TestCase("8/8/8/2PPP3/2PQP3/2PPP3/8/8 w KQkq - 0 1", new SquareEnum[] { })]
        [TestCase("8/8/1P1P1P2/2ppp3/1PpqpP2/2ppp3/1P1P1P2/8 b KQkq - 0 1", new SquareEnum[] { })]
        [TestCase("8/8/8/2ppp3/2pqp3/2ppp3/8/8 b KQkq - 0 1", new SquareEnum[] { })]
        [TestCase("8/8/1p1p1p2/2PPP3/1pPqPp2/2PPP3/1p1p1p2/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.c3, SquareEnum.c4, SquareEnum.c5, SquareEnum.e3, SquareEnum.e4, SquareEnum.e5, SquareEnum.d3, SquareEnum.d5 })]
        [TestCase("8/8/1p1p1p2/8/1p1q1p2/8/1p1p1p2/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.c3, SquareEnum.c4, SquareEnum.c5, SquareEnum.e3, SquareEnum.e4, SquareEnum.e5, SquareEnum.d3, SquareEnum.d5 })]
        [TestCase("8/8/8/2PPP3/2PqP3/2PPP3/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.c3, SquareEnum.c4, SquareEnum.c5, SquareEnum.e3, SquareEnum.e4, SquareEnum.e5, SquareEnum.d3, SquareEnum.d5 })]
        public void QueenMoves_AreValid(string fenString, SquareEnum[] expected)
        {
            AssertMovesHelper helper = new AssertMovesHelper();
            helper.AssertMovesFor(fenString, PieceEnum.Queen, expected);
        }
    }
}
