using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Tests.UnitTests
{
    public class BoardTests
    {
        [Test]
        public void Of_CreateNewReference_AreNotEqual()
        {
            // Arrange
            Board expected = Board.Of();

            // Act
            Board actual = Board.Of();

            // Assert
            Assert.IsFalse(Object.ReferenceEquals(expected, actual));
        }
    }
}
