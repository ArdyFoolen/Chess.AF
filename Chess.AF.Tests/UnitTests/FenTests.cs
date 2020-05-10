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
        internal static readonly FenString[] FenArray = new FenString[]
        {
            new FenString(@"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", true),
            new FenString(null, false),
            new FenString(@"rnbqkbnr/pppppppp/8/8/8/4P3/PPPP1PPP/RNBQKBNR b KQkq - 0 1", true),
            new FenString(@"rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1", true)
        };

        internal static readonly FenString[] FenInCheckArray = new FenString[]
        {
            new FenString(@"4k3/8/8/8/8/8/6n1/4K3 w KQkq - 0 1", true),
            new FenString(@"4k3/6N1/8/8/8/8/8/4K3 b KQkq - 0 1", true),
            new FenString(@"4k3/8/8/8/7b/8/8/4K3 w KQkq - 0 1", true),
            new FenString(@"4k3/8/8/7B/8/8/8/4K3 b KQkq - 0 1", true),
            new FenString(@"4r3/8/8/8/8/8/8/4K3 w KQkq - 0 1", true),
            new FenString(@"4k3/8/8/8/8/8/8/4R3 b KQkq - 0 1", true),
            new FenString(@"4k3/8/8/8/7q/8/8/4K3 w KQkq - 0 1", true),
            new FenString(@"4k3/8/8/7Q/8/8/8/4K3 b KQkq - 0 1", true),
            new FenString(@"4q3/8/8/8/8/8/8/4K3 w KQkq - 0 1", true),
            new FenString(@"4k3/8/8/8/8/8/8/4Q3 b KQkq - 0 1", true),
            new FenString(@"4k3/8/8/8/8/8/3p4/4K3 w KQkq - 0 1", true),
            new FenString(@"4k3/3P4/8/8/8/8/8/4K3 b KQkq - 0 1", true)
        };

        internal static readonly FenString[] FenOpponentInCheckArray = new FenString[]
        {
            new FenString(@"4k3/6N1/8/8/8/8/8/4K3 w KQkq - 0 1", true),
            new FenString(@"4k3/8/8/8/8/8/6n1/4K3 b KQkq - 0 1", true),
            new FenString(@"4k3/8/8/7B/8/8/8/4K3 w KQkq - 0 1", true),
            new FenString(@"4k3/8/8/8/7b/8/8/4K3 b KQkq - 0 1", true),
            new FenString(@"4k3/8/8/8/8/8/8/4R3 w KQkq - 0 1", true),
            new FenString(@"4r3/8/8/8/8/8/8/4K3 b KQkq - 0 1", true),
            new FenString(@"4k3/8/8/7Q/8/8/8/4K3 w KQkq - 0 1", true),
            new FenString(@"4k3/8/8/8/7q/8/8/4K3 b KQkq - 0 1", true),
            new FenString(@"4k3/8/8/8/8/8/8/4Q3 w KQkq - 0 1", true),
            new FenString(@"4q3/8/8/8/8/8/8/4K3 b KQkq - 0 1", true),
            new FenString(@"4k3/3P4/8/8/8/8/8/4K3 w KQkq - 0 1", true),
            new FenString(@"4k3/8/8/8/8/8/3p4/4K3 b KQkq - 0 1", true)
        };

        [Test]
        public void Of_IsValid()
        {
            // Act
            foreach (FenString fenString in FenArray)
                Fen.Of(fenString.Fen).Match(
                    None: () => { Assert.IsFalse(fenString.IsValid); return true; },
                    Some: s => { Assert.IsTrue(fenString.IsValid); return true; });
        }

    }
}
