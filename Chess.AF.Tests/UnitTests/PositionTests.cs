using Chess.AF.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.AF;

namespace Chess.AF.Tests.UnitTests
{
    public class PositionTests
    {
        [Test]
        public void Of_IsValid()
        {
            // Act
            foreach (FenString fenString in FenTests.FenArray)
                Fen.Of(fenString.Fen).CreatePosition()
                .Match(
                    None: () => { Assert.IsFalse(fenString.IsValid); return true; },
                    Some: s => { Assert.IsTrue(fenString.IsValid); return true; });
        }

        [Test]
        public void IsInCheck_IsValid()
        {
            // Act
            foreach (FenString fenString in FenTests.FenInCheckArray)
                Fen.Of(fenString.Fen).CreatePosition()
                .Match(
                    None: () => { Assert.Fail(); return true; },
                    Some: p => { Assert.AreEqual(fenString.IsValid, p.IsInCheck); return true; });
        }

        [Test]
        public void OpponentIsInCheck_IsValid()
        {
            // Act
            foreach (FenString fenString in FenTests.FenOpponentInCheckArray)
                Fen.Of(fenString.Fen).CreatePosition()
                .Match(
                    None: () => { Assert.Fail(); return true; },
                    Some: p => { Assert.AreEqual(fenString.IsValid, p.OpponentIsInCheck); return true; });
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
            Fen.Of(fenString).CreatePosition()
            .Match(
                None: () => { Assert.Fail(); return true; },
                Some: p => { Assert.AreEqual(expected, p.PossibleRokade); return true; });
        }
    }
}
