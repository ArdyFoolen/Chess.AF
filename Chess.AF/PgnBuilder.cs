using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF
{
    public partial class Pgn
    {
        private abstract class PgnBuilder
        {
            protected Option<Pgn> pgn = None;
            public Option<Pgn> Pgn { get { return pgn; } }
            public string PgnString { get; set; }

            public abstract void BuildPrepare();
            public abstract void BuildTagPairs();
            public abstract void BuildMoveText();
        }
    }
}
