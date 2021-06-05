using AF.Functional;
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
        [Test]
        public void Of_BuildPgn_Succeeds()
        {
            // Arrange
            string lines = ResourceHelper.ReadEmbeddedRessource("Chess.AF.Tests.Pgn.Pgn01.txt");
            Option<Pgn> expected = Pgn.Of(lines);

            // Act
            //Board actual = Board.Of();

            // Assert
            //Assert.IsFalse(Object.ReferenceEquals(expected, actual));
        }

    }
}
