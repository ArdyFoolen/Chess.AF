using Chess.AF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            public override void BuildPrepare()
            {
            }

            public override void BuildTagPairs()
            {
                tagPairDict = new Dictionary<string, string>();

                tagPairDict.Add("Event", "Chess.AF Game");
                tagPairDict.Add("Site", "");
                tagPairDict.Add("Date", $"{DateTime.Today.ToString("yyyy.MM.dd")}");
                tagPairDict.Add("Round", "");
                tagPairDict.Add("White", "");
                tagPairDict.Add("Black", "");
                tagPairDict.Add("Result", $"{getResultFromLastCommand()}");
            }

            public override void Build()
            {
            }
        }
    }
}