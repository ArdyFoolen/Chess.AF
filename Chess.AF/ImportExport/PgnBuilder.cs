using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF.ImportExport
{
    public partial class Pgn
    {
        private abstract class PgnBuilder
        {
            public Option<Pgn> Pgn { get; protected set; } = None;

            public abstract void BuildPrepare();
            public abstract void BuildTagPairs();
            public abstract void Build();
        }
    }
}
