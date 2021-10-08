using Chess.AF.ImportExport;
using Chess.AF.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.ImportExport.Pgn;

namespace Chess.AF.Tests.UnitTests
{
    public class PgnReaderTests
    {
        [TestCase("Chess.AF.Tests.Pgn.Pgn02.pgn")]
        public void Read_BuildPgn_Succeeds(string pgnFile)
        {
            // Arrange
            string lines = ResourceHelper.ReadEmbeddedRessource(pgnFile);
            var reader = new PgnReader(new GameBuilder(new Game()));

            // Act
            //Option<Pgn> expected = Pgn.Import(lines);
            reader.Read(lines);

            // Assert
            //expected.Match(
            //    None: () => Assert.Fail(),
            //    Some: p => Assert.IsTrue(true));
        }
    }
}
