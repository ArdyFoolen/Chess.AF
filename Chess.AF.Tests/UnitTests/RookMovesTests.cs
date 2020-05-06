using AF.Functional;
using AF.Functional.Option;
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
    public class RookMovesTests
    {
        [TestCase("8/8/3P4/3p4/1PpRpP2/3p4/3P4/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.c4, SquareEnum.e4, SquareEnum.d3, SquareEnum.d5 })]
        [TestCase("8/8/3P4/8/1P1R1P2/8/3P4/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.c4, SquareEnum.e4, SquareEnum.d3, SquareEnum.d5 })]
        [TestCase("8/8/8/3p4/2pRp3/3p4/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.c4, SquareEnum.e4, SquareEnum.d3, SquareEnum.d5 })]
        [TestCase("8/8/3p4/3P4/1pPRPp2/3P4/3p4/8 w KQkq - 0 1", new SquareEnum[] { })]
        [TestCase("8/8/8/3P4/2PRP3/3P4/8/8 w KQkq - 0 1", new SquareEnum[] { })]
        [TestCase("8/8/3P4/3p4/1PprpP2/3p4/3P4/8 b KQkq - 0 1", new SquareEnum[] { })]
        [TestCase("8/8/8/3p4/2prp3/3p4/8/8 b KQkq - 0 1", new SquareEnum[] { })]
        [TestCase("8/8/3p4/3P4/1pPrPp2/3P4/3p4/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.c4, SquareEnum.e4, SquareEnum.d3, SquareEnum.d5 })]
        [TestCase("8/8/3p4/8/1p1r1p2/8/3p4/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.c4, SquareEnum.e4, SquareEnum.d3, SquareEnum.d5 })]
        [TestCase("8/8/8/3P4/2PrP3/3P4/8/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.c4, SquareEnum.e4, SquareEnum.d3, SquareEnum.d5 })]
        public void RookMoves_AreValid(string fenString, SquareEnum[] expected)
        {
            AssertMovesHelper helper = new AssertMovesHelper();
            helper.AssertMovesFor(fenString, PieceEnum.Rook, expected);
        }
    }
}
