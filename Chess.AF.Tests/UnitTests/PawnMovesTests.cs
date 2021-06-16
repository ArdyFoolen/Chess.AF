using AF.Functional;
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
        [TestCase("8/8/8/4p3/4P3/8/8/8 w KQkq e6 0 1", new SquareEnum[] { })]
        [TestCase("8/8/8/4p3/4P3/8/8/8 b KQkq e3 0 1", new SquareEnum[] { })]
        [TestCase("8/8/8/3pP3/8/8/8/8 w KQkq d6 0 1", new SquareEnum[] { SquareEnum.d6, SquareEnum.e6 })]
        [TestCase("8/8/8/8/3pP3/8/8/8 b KQkq e3 0 1", new SquareEnum[] { SquareEnum.e3, SquareEnum.d3 })]
        [TestCase("8/8/8/4Pp2/8/8/8/8 w KQkq f6 0 1", new SquareEnum[] { SquareEnum.f6, SquareEnum.e6 })]
        [TestCase("8/8/8/8/2Pp4/8/8/8 b KQkq c3 0 1", new SquareEnum[] { SquareEnum.c3, SquareEnum.d3 })]
        [TestCase("8/4P3/8/8/8/8/8/8 w KQkq - 0 1", new SquareEnum[] { SquareEnum.e8, SquareEnum.e8, SquareEnum.e8, SquareEnum.e8 })]
        [TestCase("8/8/8/8/8/8/4p3/8 b KQkq - 0 1", new SquareEnum[] { SquareEnum.e1, SquareEnum.e1, SquareEnum.e1, SquareEnum.e1 })]

        public void PawnMoves_AreValid(string fenString, SquareEnum[] expected)
        {
            AssertMovesHelper helper = new AssertMovesHelper();
            helper.AssertMovesFor(fenString, PieceEnum.Pawn, expected);
        }

        [TestCaseSource(nameof(Tuples))]
        public void PawnMoves_TakeEnPassant_AreValid((string fen, PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare, string expectedFen) tuples)
        {
            Game game = new Game();
            game.Load(tuples.fen);
            var toMove = AF.Dto.Move.Of(tuples.Piece, tuples.Square, tuples.MoveSquare, tuples.Promoted);
            toMove.Map(m => { game.Move(m); return Unit(); });

            Assert.AreEqual(tuples.expectedFen, game.ToFenString());
        }

        public static IEnumerable<(string fen, PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare, string expectedFen)[]> Tuples
        {
            get
            {
                yield return new[] { ("r1bqk2r/ppp2ppp/2n5/2bpP1N1/2Bp2n1/8/PPP2PPP/RNBQ1RK1 w kq d6 0 1", PieceEnum.Pawn, SquareEnum.e5, PieceEnum.Pawn, SquareEnum.d6, "r1bqk2r/ppp2ppp/2nP4/2b3N1/2Bp2n1/8/PPP2PPP/RNBQ1RK1 b kq - 0 1") };
                yield return new[] { ("rnbqkbnr/ppp2ppp/8/3p4/3pP3/5N2/PPP2PPP/RNBQKB1R w KQkq d6 0 1", PieceEnum.Pawn, SquareEnum.e4, PieceEnum.Pawn, SquareEnum.d5, "rnbqkbnr/ppp2ppp/8/3P4/3p4/5N2/PPP2PPP/RNBQKB1R b KQkq - 0 1") };
            }
        }
    }
}
