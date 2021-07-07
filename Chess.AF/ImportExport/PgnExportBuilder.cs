using Chess.AF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Functional;
using static AF.Functional.F;
using Chess.AF.Domain;
using Chess.AF.Enums;
using Chess.AF.Dto;

namespace Chess.AF.ImportExport
{
    public partial class Pgn
    {
        private class PgnExportBuilder : PgnBuilder
        {
            private IGame Game;
            private IList<Command> Commands;
            private Dictionary<string, string> tagPairDict;
            private string PgnString { get; set; }

            public PgnExportBuilder(IGame game, IList<Command> commands)
            {
                this.Game = game;
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

            private void addSevenTagRosterTagPairs()
            {
                tagPairDict.Add(nameof(SevenTagRosterEnum.Event), "Chess.AF Game");
                tagPairDict.Add(nameof(SevenTagRosterEnum.Site), "");
                tagPairDict.Add(nameof(SevenTagRosterEnum.Date), $"{DateTime.Today.ToString("yyyy.MM.dd")}");
                tagPairDict.Add(nameof(SevenTagRosterEnum.Round), "");
                tagPairDict.Add(nameof(SevenTagRosterEnum.White), "");
                tagPairDict.Add(nameof(SevenTagRosterEnum.Black), "");
                tagPairDict.Add(nameof(SevenTagRosterEnum.Result), $"{getResultFromLastCommand()}");
            }

            private void addSetupTagPairs()
            {
                if (Commands == null || Commands.Count() == 0)
                    return;

                addSetupTagPair(Commands[0]);
            }

            private void addSetupTagPair(Command command)
            {
                LoadCommand loadCommand = command as LoadCommand;
                if (loadCommand == null || loadCommand.IsDefaultFen)
                    return;

                tagPairDict.Add(nameof(FenSetupEnum.Setup), "1");
                tagPairDict.Add(nameof(FenSetupEnum.FEN), loadCommand.Fen);
            }

            private void CreateMoveText()
            {
                if (Commands == null || Commands.Count() == 0)
                    return;

                var visitor = new PgnCommandVisitor();

                foreach (var command in Commands)
                    command.Accept(visitor);

                PgnString += visitor.PgnString;
            }

            public override void BuildPrepare()
            {
            }

            public override void BuildTagPairs()
            {
                tagPairDict = new Dictionary<string, string>();

                addSevenTagRosterTagPairs();
                addSetupTagPairs();

                formatTagPairsToString();
            }

            public override void Build()
            {
                CreateMoveText();

                var pgn = new Pgn(PgnString);
                pgn.Game = this.Game;
                pgn.TagPairDictionary = tagPairDict;
                this.Pgn = Some(pgn);
            }
        }
    }
}