using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF
{
    public enum SevenTagRosterEnum
    {
        Event,
        Site,
        Date,
        Round,
        White,
        Black,
        Result
    }

    public partial class Pgn
    {
        public string PgnString { get; }
        private Pgn(string pgnString) { this.PgnString = pgnString; }

        public Game Game { get; private set; }

        public static Option<Pgn> Import(IEnumerable<string> lines)
            => Import(string.Join(Environment.NewLine, lines.ToArray()));
        public static Option<Pgn> Import(string pgnString)
        {
            PgnBuilder builder = new PgnImportBuilder();
            builder.PgnString = pgnString;
            builder.BuildPrepare();
            builder.BuildTagPairs();
            builder.BuildMoveText();

            return builder.Pgn;
        }
    }
}
