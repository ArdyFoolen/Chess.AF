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

        //public static Option<Pgn> Of(string pgnString)
        //    => IsValid(pgnString) ? Some(new Pgn(pgnString)) : None;

        public static Option<Pgn> Of(IEnumerable<string> lines)
            => Of(string.Join(Environment.NewLine, lines.ToArray()));
        public static Option<Pgn> Of(string pgnString)
        {
            PgnBuilder builder = new PortableGameNotationBuilder();
            builder.PgnString = pgnString;
            builder.BuildPrepare();
            builder.BuildTagPairs();
            builder.BuildMoveText();

            return builder.Pgn;
        }

        public static bool IsValid(string pgnString)
            => true;//FenRegex.IsValid(pgnString);

    }
}
