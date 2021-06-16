using Chess.AF.ImportExport;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Tests.UnitTests
{
    public class FenRegexTests
    {
        [TestCase("4k3/8/1P3P2/2p1p3/3B4/2p1p3/1P3P2/4K3 w KQkq - 0 1", true)]
        public void AreValid(string fenString, bool expected)
        {
            Assert.AreEqual(expected, FenRegex.IsValid(fenString));
        }
    }
}
