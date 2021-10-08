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

    public enum StatesEnum
    {
        Event,
        Site,
        Date,
        Round,
        White,
        Black,
        Result,
        Optional
    }

    public partial class Pgn
    {
        public string PgnString { get; private set; }
        private Pgn(string pgnString) { this.PgnString = pgnString; }
        private Pgn(IGame game) { this.Game = game; }

        public IGame Game { get; private set; }
        public Dictionary<string, string> TagPairDictionary { get; private set; } = new Dictionary<string, string>();

        public static Option<Pgn> Import(IPgnReader reader, string pgnString)
        {
            reader.Read(pgnString);
            return reader.Pgn;
        }

        public static Option<Pgn> Export(IGame game, IList<Command> commands)
        {
            PgnBuilder builder = new PgnExportBuilder(game, commands);
            return Build(builder);
        }

        private static Option<Pgn> Build(PgnBuilder builder)
        {
            builder.BuildPrepare();
            builder.BuildTagPairs();
            builder.Build();

            return builder.Pgn;
        }
    }
}
