using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Console.Tests.UnitTests
{
    public class ExtensionsTests
    {
        [TestCase("", "")]
        [TestCase("fen", "fen")]
        [TestCase("select n", "select")]
        public void GetCommandTests(string cmd, string expected)
        {
            var actual = cmd.GetCommand();

            Assert.AreEqual(expected, actual);
        }

        [TestCase("", new string[0])]
        [TestCase("fen", new string[0])]
        [TestCase("select n", new string[1] { "n" })]
        [TestCase("select n b", new string[2] { "n", "b" })]
        public void GetParametersTests(string cmd, string[] expected)
        {
            var actual = cmd.GetParameters();

            Assert.AreEqual(expected.Length, actual.Length);
            Assert.IsTrue(expected.All(a => actual.Contains(a)));
        }
    }
}
