using Chess.AF.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Tests.UnitTests
{
    public class GameResultTests
    {
        [TestCase("*", GameResult.Ongoing)]
        [TestCase("1-0", GameResult.WhiteWins)]
        [TestCase("0-1", GameResult.BlackWins)]
        [TestCase("1/2-1/2", GameResult.Draw)]
        [TestCase("bla", GameResult.Invalid)]
        public void ToGameResult_FromString_ShouldSucceed(string str, GameResult expected)
        {
            // Act
            var actual = str.ToGameResult();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
