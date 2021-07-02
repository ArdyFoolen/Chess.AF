using AF.Functional;
using Chess.AF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF.ImportExport
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

    public enum FenSetupEnum
    {
        Setup,
        FEN
    }

    public partial class Pgn
    {
        public string PgnString { get; private set; }
        private Pgn(string pgnString) { this.PgnString = pgnString; }
        private Pgn(IGame game) { this.Game = game; }

        public IGame Game { get; private set; }
        public Dictionary<string, string> TagPairDictionary { get; private set; } = new Dictionary<string, string>();

        public static Option<Pgn> Import(IEnumerable<string> lines)
            => Import(string.Join(Environment.NewLine, lines.ToArray()));
        public static Option<Pgn> Import(string pgnString)
        {
            PgnBuilder builder = new PgnImportBuilder(pgnString);
            builder.BuildPrepare();
            builder.BuildTagPairs();
            builder.Build();

            return builder.Pgn;
        }

        public static Option<Pgn> Export(IGame game, IList<Command> commands)
        {
            PgnBuilder builder = new PgnExportBuilder(game, commands);
            builder.BuildPrepare();
            builder.BuildTagPairs();
            builder.Build();

            return builder.Pgn;
        }
    }
}
