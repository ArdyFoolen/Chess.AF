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
    }
}
