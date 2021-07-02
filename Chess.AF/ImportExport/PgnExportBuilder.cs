using Chess.AF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Functional;
using static AF.Functional.F;

namespace Chess.AF.ImportExport
{
    public partial class Pgn
    {
        private class PgnExportBuilder : PgnBuilder
        {
            private IList<Command> Commands;
            private Dictionary<string, string> tagPairDict;
            private string PgnString { get; set; }

            public PgnExportBuilder(IList<Command> commands)
            {
                this.Commands = commands;
            }

            private string getResultFromLastCommand()
            {
                if (Commands == null || Commands.Count() == 0)
                    return string.Empty;

                return Commands[Commands.Count() - 1].Board.Match(
                    None: () => string.Empty,
                    Some: b => b.Result.ToDisplayString()
                    );
            }

            private void formatTagPairsToString()
                => PgnString = string.Join(Environment.NewLine, tagPairDict.Select(kv => formatTagPairsToString(kv))) +
                    Environment.NewLine + Environment.NewLine;

            private string formatTagPairsToString(KeyValuePair<string, string> tagPair)
                => $"[{tagPair.Key} \"{tagPair.Value}\"]";

            public override void BuildPrepare()
            {
            }

            public override void BuildTagPairs()
            {
                tagPairDict = new Dictionary<string, string>();

                tagPairDict.Add(nameof(SevenTagRosterEnum.Event), "Chess.AF Game");
                tagPairDict.Add(nameof(SevenTagRosterEnum.Site), "");
                tagPairDict.Add(nameof(SevenTagRosterEnum.Date), $"{DateTime.Today.ToString("yyyy.MM.dd")}");
                tagPairDict.Add(nameof(SevenTagRosterEnum.Round), "");
                tagPairDict.Add(nameof(SevenTagRosterEnum.White), "");
                tagPairDict.Add(nameof(SevenTagRosterEnum.Black), "");
                tagPairDict.Add(nameof(SevenTagRosterEnum.Result), $"{getResultFromLastCommand()}");

                formatTagPairsToString();
            }

            public override void Build()
            {
                var pgn = new Pgn(PgnString);
                this.Pgn = Some(pgn);
            }
        }
    }
}