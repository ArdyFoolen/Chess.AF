using Chess.AF.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.AF;
using AF.Functional;
using Chess.AF.Enums;
using Chess.AF.PositionBridge;
using Chess.AF.Dto;
using Chess.AF.ImportExport;

namespace Chess.AF.Tests.UnitTests
{
    public class PositionTests
    {
        [Test]
        public void Of_IsValid()
        {
            // Act
            foreach (FenString fenString in FenTests.FenArray)
                Fen.Of(fenString.Fen).CreatePositionAbstraction()
                .Match(
                    None: () => { Assert.IsFalse(fenString.IsValid); return true; },
                    Some: s => { Assert.IsTrue(fenString.IsValid); return true; });
        }

        [Test]
        public void IsInCheck_IsValid()
        {
            // Act
            foreach (FenString fenString in FenTests.FenInCheckArray)
                Fen.Of(fenString.Fen).CreatePositionAbstraction()
                .Match(
                    None: () => { Assert.Fail(); return true; },
                    Some: p => { Assert.AreEqual(fenString.IsValid, p.IsInCheck); return true; });
        }

        private static void ChangeWhoToMove(IPositionAbstraction position)
        {
            var prop = position.GetType().GetProperty("IsWhiteToMove");
            var getIsWhiteToMove = prop.GetGetMethod();
            bool isWhiteToMove = (bool)getIsWhiteToMove.Invoke(position, null);
            var setIsWhiteToMove = prop.GetSetMethod(true);
            setIsWhiteToMove.Invoke(position, new object[] { !isWhiteToMove });
        }

        private bool isOpponentIncheck(IPositionAbstraction position)
        {
            ChangeWhoToMove(position);
            return position.IsInCheck;
        }

        private bool isOpponentMate(IPositionAbstraction position)
        {
            ChangeWhoToMove(position);
            return position.IsMate;
        }

        private bool isOpponentStaleMate(IPositionAbstraction position)
        {
            ChangeWhoToMove(position);
            return position.IsStaleMate;
        }

        private RokadeEnum possibleRokade(IPositionAbstraction position)
        {
            var prop = position.GetType().GetProperty("Implementor", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var getImplementor = prop.GetGetMethod(true);
            IPositionImpl implementor = (IPositionImpl)getImplementor.Invoke(position, null);
            return implementor.PossibleRokade();
        }

        [Test]
        public void OpponentIsInCheck_IsValid()
        {
            // Act
            foreach (FenString fenString in FenTests.FenOpponentInCheckArray)
                Fen.Of(fenString.Fen).CreatePositionAbstraction()
                .Match(
                    None: () => { Assert.Fail(); return true; },
                    Some: p => { Assert.AreEqual(fenString.IsValid, isOpponentIncheck(p)); return true; });
        }

        [TestCase("rnbqkbnr/8/8/8/8/8/8/RNBQKBNR w KQkq - 0 1", RokadeEnum.None)]
        [TestCase("rnbqkbnr/8/8/8/8/8/8/RNBQKBNR b KQkq - 0 1", RokadeEnum.None)]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1", RokadeEnum.KingAndQueenSide)]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1", RokadeEnum.KingAndQueenSide)]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R w Kk - 0 1", RokadeEnum.KingSide)]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R b Kk - 0 1", RokadeEnum.KingSide)]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R w Qq - 0 1", RokadeEnum.QueenSide)]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R b Qq - 0 1", RokadeEnum.QueenSide)]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R w - - 0 1", RokadeEnum.None)]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R b - - 0 1", RokadeEnum.None)]
        [TestCase("r3k2r/8/8/B7/b7/8/8/R3K2R w Kk - 0 1", RokadeEnum.KingSide)]
        [TestCase("r3k2r/8/8/B7/b7/8/8/R3K2R b Kk - 0 1", RokadeEnum.KingSide)]
        [TestCase("r3k2r/8/B7/8/8/b7/8/R3K2R w Kk - 0 1", RokadeEnum.KingSide)]
        [TestCase("r3k2r/8/B7/8/8/b7/8/R3K2R b Kk - 0 1", RokadeEnum.KingSide)]
        [TestCase("r3k2r/8/7B/8/8/7b/8/R3K2R w Qq - 0 1", RokadeEnum.QueenSide)]
        [TestCase("r3k2r/8/7B/8/8/7b/8/R3K2R b Qq - 0 1", RokadeEnum.QueenSide)]
        [TestCase("r3k2r/7B/8/8/8/8/7b/R3K2R w Qq - 0 1", RokadeEnum.QueenSide)]
        [TestCase("r3k2r/7B/8/8/8/8/7b/R3K2R b Qq - 0 1", RokadeEnum.QueenSide)]
        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R w KQkq - 0 1", RokadeEnum.None)]
        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R b KQkq - 0 1", RokadeEnum.None)]
        public void CanRokade(string fenString, RokadeEnum expected)
        {
            Fen.Of(fenString).CreatePositionAbstraction()
            .Match(
                None: () => { Assert.Fail(); return true; },
                Some: p => { Assert.AreEqual(expected, possibleRokade(p)); return true; });
        }

        [TestCaseSource(typeof(TestSourceHelper), "MovesTestCases")]
        public void Moves_AreValid((string FenString, (PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)[] Expected) moveTo)
        {
            AssertMovesHelper helper = new AssertMovesHelper();

            Fen.Of(moveTo.FenString).CreatePositionAbstraction()
            .Match(
                None: () => { Assert.Fail(); return true; },
                Some: p => { helper.AssertIterateForMoves(p, moveTo.Expected); return true; });
        }

        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R w KQkq - 0 1", false)]
        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R b KQkq - 0 1", false)]
        [TestCase("8/8/8/8/8/6k1/6q1/6K1 w KQkq - 0 1", true)]
        [TestCase("6k1/6Q1/6K1/8/8/8/8/8 b KQkq - 0 1", true)]
        public void IsMate_AreValid(string fenString, bool expected)
        {
            Fen.Of(fenString).CreatePositionAbstraction()
                .Match(
                    None: () => { Assert.Fail(); return true; },
                    Some: p => { Assert.AreEqual(expected, p.IsMate); return true; });
        }

        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R w KQkq - 0 1", false)]
        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R b KQkq - 0 1", false)]
        [TestCase("6k1/6Q1/6K1/8/8/8/8/8 w KQkq - 0 1", true)]
        [TestCase("8/8/8/8/8/6k1/6q1/6K1 b KQkq - 0 1", true)]
        public void OpponentIsMate_AreValid(string fenString, bool expected)
        {
            Fen.Of(fenString).CreatePositionAbstraction()
                .Match(
                    None: () => { Assert.Fail(); return true; },
                    Some: p => { Assert.AreEqual(expected, isOpponentMate(p)); return true; });
        }

        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R w KQkq - 0 1", false)]
        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R b KQkq - 0 1", false)]
        [TestCase("8/8/8/8/8/6k1/6q1/6K1 w - - 0 1", false)]
        [TestCase("6k1/6Q1/6K1/8/8/8/8/8 b - - 0 1", false)]
        [TestCase("8/8/8/8/8/5kq1/8/7K w - - 0 1", true)]
        [TestCase("7k/8/5KQ1/8/8/8/8/8 b - - 0 1", true)]
        public void IsStaleMate_AreValid(string fenString, bool expected)
        {
            Fen.Of(fenString).CreatePositionAbstraction()
                .Match(
                    None: () => { Assert.Fail(); return true; },
                    Some: p => { Assert.AreEqual(expected, p.IsStaleMate); return true; });
        }

        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R w KQkq - 0 1", false)]
        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R b KQkq - 0 1", false)]
        [TestCase("6k1/6Q1/6K1/8/8/8/8/8 w - - 0 1", false)]
        [TestCase("8/8/8/8/8/6k1/6q1/6K1 b - - 0 1", false)]
        [TestCase("7k/8/5KQ1/8/8/8/8/8 w - - 0 1", true)]
        [TestCase("8/8/8/8/8/5kq1/8/7K b - - 0 1", true)]
        public void OpponentIsStaleMate_AreValid(string fenString, bool expected)
        {
            Fen.Of(fenString).CreatePositionAbstraction()
                .Match(
                    None: () => { Assert.Fail(); return true; },
                    Some: p => { Assert.AreEqual(expected, isOpponentStaleMate(p)); return true; });
        }

        [TestCaseSource(typeof(TestSourceHelper), "RokadeTestCases")]
        public void AfterKingOrRookMove_CanNotRokade((string fenString, RokadeEnum expected, Option<Move> moveTo) tuple)
        {
            AssertMovesHelper helper = new AssertMovesHelper();

            Fen.Of(tuple.fenString).CreatePositionAbstraction()
            .Match(
                None: () => { Assert.Fail(); return true; },
                Some: p => tuple.moveTo.Match(
                    None: () => { Assert.Fail(); return true; },
                    Some: m => { helper.AssertRokadeAfterMove(p, m, tuple.expected); return true; }
                    ));
        }

        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")]
        [TestCase("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b KQkq - 0 1")]
        [TestCase("7k/8/5KQ1/8/8/8/8/8 w - - 0 1")]
        [TestCase("8/8/8/8/8/5kq1/8/7K b - - 0 1")]
        [TestCase("4k3/8/8/8/7b/8/8/4K1B1 w - - 0 1")]
        [TestCase("4k1b1/8/8/7B/8/8/8/4K3 b - - 0 1")]
        [TestCase("rnb1kbnr/pppp1ppp/8/4p3/5P1q/8/PPPPP1PP/RNBQKBNR w KQkq - 0 1")]
        [TestCase("rnbqkbnr/ppppp1pp/8/5p1Q/4P3/8/PPPP1PPP/RNB1KBNR b KQkq - 0 1")]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1")]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1")]
        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R w KQkq - 0 1")]
        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R b KQkq - 0 1")]
        [TestCase("6k1/6Q1/6K1/8/8/8/8/8 w - - 0 1")]
        [TestCase("8/8/8/8/8/6k1/6q1/6K1 b - - 0 1")]
        [TestCase("7k/8/5KQ1/8/8/8/8/8 w - - 0 1")]
        [TestCase("8/8/8/8/8/5kq1/8/7K b - - 0 1")]
        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R w KQkq - 0 1")]
        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R b KQkq - 0 1")]
        [TestCase("8/8/8/8/8/6k1/6q1/6K1 w - - 0 1")]
        [TestCase("6k1/6Q1/6K1/8/8/8/8/8 b - - 0 1")]
        [TestCase("8/8/8/8/8/5kq1/8/7K w - - 0 1")]
        [TestCase("7k/8/5KQ1/8/8/8/8/8 b - - 0 1")]
        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R w KQkq - 0 1")]
        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R b KQkq - 0 1")]
        [TestCase("6k1/6Q1/6K1/8/8/8/8/8 w KQkq - 0 1")]
        [TestCase("8/8/8/8/8/6k1/6q1/6K1 b KQkq - 0 1")]
        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R w KQkq - 0 1")]
        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R b KQkq - 0 1")]
        [TestCase("8/8/8/8/8/6k1/6q1/6K1 w KQkq - 0 1")]
        [TestCase("6k1/6Q1/6K1/8/8/8/8/8 b KQkq - 0 1")]
        [TestCase("rnbqkbnr/8/8/8/8/8/8/RNBQKBNR w KQkq - 0 1")]
        [TestCase("rnbqkbnr/8/8/8/8/8/8/RNBQKBNR b KQkq - 0 1")]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1")]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1")]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R w Kk - 0 1")]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R b Kk - 0 1")]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R w Qq - 0 1")]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R b Qq - 0 1")]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R w - - 0 1")]
        [TestCase("r3k2r/8/8/8/8/8/8/R3K2R b - - 0 1")]
        [TestCase("r3k2r/8/8/B7/b7/8/8/R3K2R w Kk - 0 1")]
        [TestCase("r3k2r/8/8/B7/b7/8/8/R3K2R b Kk - 0 1")]
        [TestCase("r3k2r/8/B7/8/8/b7/8/R3K2R w Kk - 0 1")]
        [TestCase("r3k2r/8/B7/8/8/b7/8/R3K2R b Kk - 0 1")]
        [TestCase("r3k2r/8/7B/8/8/7b/8/R3K2R w Qq - 0 1")]
        [TestCase("r3k2r/8/7B/8/8/7b/8/R3K2R b Qq - 0 1")]
        [TestCase("r3k2r/7B/8/8/8/8/7b/R3K2R w Qq - 0 1")]
        [TestCase("r3k2r/7B/8/8/8/8/7b/R3K2R b Qq - 0 1")]
        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R w KQkq - 0 1")]
        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R b KQkq - 0 1")]
        public void ToFenString_AreValid(string fenString)
        {
            Fen.Of(fenString).CreatePositionAbstraction()
                .Match(
                    None: () => { Assert.Fail(); return true; },
                    Some: p => { Assert.AreEqual(fenString, p.ToFenString()); return true; });
        }

        [TestCase("r3k2r/8/8/b7/B7/8/8/R3K2R b KQkq - 0 1")]
        public void ToDictionary_IsValid(string fenString)
        {
            Dictionary<int, PieceOnSquare<PiecesEnum>> dict = null;
            Fen.Of(fenString).CreatePositionAbstraction()
                .Match(
                    None: () => { Assert.Fail(); return true; },
                    Some: p => { dict = p.ToDictionary(); return true; });

            AssertSquare(dict[0], PiecesEnum.BlackRook, SquareEnum.a8);
            AssertSquare(dict[4], PiecesEnum.BlackKing, SquareEnum.e8);
            AssertSquare(dict[7], PiecesEnum.BlackRook, SquareEnum.h8);
            AssertSquare(dict[24], PiecesEnum.BlackBishop, SquareEnum.a5);
            AssertSquare(dict[32], PiecesEnum.WhiteBishop, SquareEnum.a4);
            AssertSquare(dict[56], PiecesEnum.WhiteRook, SquareEnum.a1);
            AssertSquare(dict[60], PiecesEnum.WhiteKing, SquareEnum.e1);
            AssertSquare(dict[63], PiecesEnum.WhiteRook, SquareEnum.h1);
        }

        private static void AssertSquare(PieceOnSquare<PiecesEnum> item,
            PiecesEnum expectedPiece, SquareEnum expectedSquare)
        {
            Assert.AreEqual(expectedPiece, item.Piece);
            Assert.AreEqual(expectedSquare, item.Square);
        }
    }
}
