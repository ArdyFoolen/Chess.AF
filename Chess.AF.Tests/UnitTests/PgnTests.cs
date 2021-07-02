using AF.Functional;
using Chess.AF.Commands;
using Chess.AF.ImportExport;
using Chess.AF.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
                Some: p => AssertTagPairs(p, tagPairsFormat1));
        }

        [Test]
        public void Export_BuildPgnWithSetup_ShouldCreateTagPairs()
        {
            // Arrange
            Game game = new Game();
            game.Load("r1q4k/1P6/8/8/8/8/8/R5K1 w - - 0 1");
            var commands = game.GetType().GetField("Commands", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(game) as IList<Command>;

            // Act
            var pgn = Pgn.Export(commands);

            // Assert
            pgn.Match(
                None: () => Assert.Fail(),
                Some: p => AssertTagPairs(p, tagPairsFormat2));
        }

        private const string tagPairsFormat1 = @"[Event ""Chess.AF Game""]
[Site """"]
[Date ""{0}""]
[Round """"]
[White """"]
[Black """"]
[Result """"]

";

        private const string tagPairsFormat2 = @"[Event ""Chess.AF Game""]
[Site """"]
[Date ""{0}""]
[Round """"]
[White """"]
[Black """"]
[Result ""*""]
[Setup ""1""]
[FEN ""r1q4k/1P6/8/8/8/8/8/R5K1 w - - 0 1""]

";

        private void AssertTagPairs(Pgn pgn, string tagPairsFormat)
        {
            var tagPairs = string.Format(tagPairsFormat, DateTime.Today.ToString("yyyy.MM.dd"));
            Assert.AreEqual(tagPairs, pgn.PgnString);
        }
    }
}
