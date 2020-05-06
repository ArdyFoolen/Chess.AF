using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Tests.Helpers
{
    internal class FenString
    {
        internal string Fen { get; }
        internal bool IsValid { get; }
        internal FenString(string fen, bool isValid)
        {
            this.Fen = fen;
            this.IsValid = isValid;
        }
    }
}
