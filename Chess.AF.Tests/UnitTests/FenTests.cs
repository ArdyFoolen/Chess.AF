using Chess.AF.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Tests.UnitTests
{
    public class FenTests
    {
        internal static readonly FenString[] FenArrays = new FenString[]
        {
            new FenString(@"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", true),
            new FenString(null, false),
            new FenString(@"rnbqkbnr/pppppppp/8/8/8/4P3/PPPP1PPP/RNBQKBNR b KQkq - 0 1", true),
            new FenString(@"rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1", true)
        };

        [Test]
        public void Of_IsValid()
        {
            // Act
            foreach (FenString fenString in FenArrays)
                Fen.Of(fenString.Fen).Match(
                    None: () => { Assert.IsFalse(fenString.IsValid); return true; },
                    Some: s => { Assert.IsTrue(fenString.IsValid); return true; });
        }

    }
}
