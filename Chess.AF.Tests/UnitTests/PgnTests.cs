using AF.Functional;
using Chess.AF.ImportExport;
using Chess.AF.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Tests.UnitTests
{
    public class PgnTests
    {
        [TestCase("Chess.AF.Tests.Pgn.Pgn01.pgn")]
        [TestCase("Chess.AF.Tests.Pgn.Pgn02.pgn")]
        public void Of_BuildPgn_Succeeds(string pgnFile)
        {
            // Arrange
            string lines = ResourceHelper.ReadEmbeddedRessource(pgnFile);

            // Act
            Option<Pgn> expected = Pgn.Import(lines);

            // Assert
            expected.Match(
                None: () => Assert.Fail(),
                Some: p => Assert.IsTrue(true));
        }

        [Test]
        public void Export_BuildPgn_ShouldCreateTagPairs()
        {
            // Act
            var pgn = Pgn.Export(null);

            // Assert
            pgn.Match(
                None: () => Assert.Fail(),
                Some: p => AssertTagPairs(p));
        }

        private const string tagPairsFormat1 = @"[Event ""Chess.AF Game""]
[Site """"]
[Date ""{0}""]
[Round """"]
[White """"]
[Black """"]
[Result """"]

";

        private void AssertTagPairs(Pgn pgn)
        {
            var tagPairs = string.Format(tagPairsFormat1, DateTime.Today.ToString("yyyy.MM.dd"));
            Assert.AreEqual(tagPairs, pgn.PgnString);
        }
    }
}
