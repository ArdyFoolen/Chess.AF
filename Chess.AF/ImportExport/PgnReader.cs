using AF.Functional;
using Chess.AF;
using Chess.AF.Dto;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF.ImportExport
{
    public partial class Pgn
    {
        public class PgnReader : IPgnReader, IPgnTagStateContext
        {
            #region Private

            private const string movePattern = @"([NBRQK])?([a-h])?([1-8])?([-x])?([a-h][1-8])(=)?([NBRQ])?|O-O-O|O-O";
            private static Regex moveRegex = new Regex(movePattern, RegexOptions.Compiled);

            private Dictionary<string, string> EventTags;
            private bool commentShouldBeclosed = true;
            private IGameBuilder Builder;

            #endregion

            #region Public Properties

            public IEnumerable<string> TagLines { get; private set; }
            public IEnumerable<string> MoveTextLines { get; private set; }
            public PgnTagState State { get; set; }
            public List<Error> Errors { get; private set; } = new List<Error>();
            public Pgn Pgn { get; private set; }

            #endregion

            public PgnReader(IGameBuilder builder)
            {
                this.Builder = builder;
            }

            public void Read(string pgnFile)
            {
                Pgn = new Pgn(pgnFile);
                SetTagAndMoveText(pgnFile);

                ReadTags();
                ReadMoveText();

                Pgn.Game = Builder.Game;
                Pgn.TagPairDictionary = EventTags;
            }

            #region Set Tag and MoveText

            private void SetTagAndMoveText(string pgnFile)
            {
                var iter = EnumerableTagAndMoveText(pgnFile).GetEnumerator();
                if (iter.MoveNext())
                {
                    TagLines = iter.Current.SplitLines();
                    iter.MoveNext();
                    MoveTextLines = iter.Current.SplitLines();
                }
            }

            private IEnumerable<string> EnumerableTagAndMoveText(string pgnFile)
            {
                var parts = pgnFile.Split(new string[] { "\r\n\r\n", "\r\r", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2)
                    Errors.Add(Error($"Wrong Pgn file format"));
                else
                {
                    yield return parts[0];
                    yield return parts[1];
                }
            }

            #endregion

            #region TagText

            private void ReadTags()
            {
                EventTags = new Dictionary<string, string>();
                State = PgnTagState.CreateInitialState(this, EventTags);

                foreach (string line in TagLines)
                    State.TryAddTagPair(line).Match(
                        Invalid: ie => { Errors.AddRange(ie); return Unit(); },
                        Valid: kv => { return Unit(); });

                validateSevenTagRoster();
            }

            private void validateSevenTagRoster()
            {
                if (EventTags.Keys.Count() < 7)
                    Errors.Add(Error($"Seven Tag Roster count {EventTags.Keys.Count()} not valid"));
            }

            #endregion

            #region MoveText

            private void ReadMoveText()
            {
                commentShouldBeclosed = true;
                WithLoad();

                foreach (string line in MoveTextLines)
                    ReadMoves(line);

                WithResult();
            }

            private void WithLoad()
            {
                if (EventTags.ContainsKey(nameof(FenSetupEnum.Setup).ToLowerInvariant()) && EventTags[nameof(FenSetupEnum.Setup).ToLowerInvariant()].Equals("1") && EventTags.ContainsKey(nameof(FenSetupEnum.FEN).ToLowerInvariant()))
                    Builder.WithFen(EventTags[nameof(FenSetupEnum.FEN).ToLowerInvariant()]);
                else
                    Builder.WithDefault();
            }

            private void ReadMoves(string line)
            {
                Option<Move> move = None;
                line = removeComments(line);

                MatchCollection matches = moveRegex.Matches(line);
                for (int count = 0; count < matches.Count; count++)
                    if (!tryWithRokade(matches[count]))
                        WithMove(matches[count]);
            }

            private void WithResult()
                => Builder.With(EventTags[nameof(SevenTagRosterEnum.Result).ToLowerInvariant()].ToGameResult());

            #region Move

            private bool tryWithRokade(Match match)
            {
                var group = match.Groups[0];
                var rokade = group.Value.ToRokade();
                if (!RokadeEnum.None.Equals(rokade))
                {
                    Builder.With(rokade);
                    return true;
                }

                return false;
            }

            private void WithMove(Match match)
            {
                Builder.With(GetPiece(match));
                WithFileFrom(match);
                WithRowFrom(match);
                WithPromote(match);
                WithMoveTo(match);
            }

            private PieceEnum GetPiece(Match match)
            {
                var group = match.Groups[1];
                return group.Value.FromSanToPiece().Match(
                    None: () => PieceEnum.Pawn,
                    Some: s => s);
            }

            private void WithFileFrom(Match match)
            {
                var group = match.Groups[2];
                $"{group.Value}1".ToSquare().Match(
                    None: () => Unit(),
                    Some: s => Builder.WithMoveFromFile(s.File()));
            }

            private void WithRowFrom(Match match)
            {
                var group = match.Groups[3];
                $"a{group.Value}".ToSquare().Match(
                    None: () => Unit(),
                    Some: s => Builder.WithMoveFromRow(s.Row()));
            }

            private void WithPromote(Match match)
            {
                var group = match.Groups[7];
                group.Value.FromSanToPromoted().Match(
                    None: () => Unit(),
                    Some: p => Builder.WithPromote(p));
            }

            private void WithMoveTo(Match match)
            {
                var group = match.Groups[5];
                group.Value.ToSquare().Match(
                    None: () => Errors.Add(Error($"Invalid square to move to {group.Value}")),
                    Some: s => Builder.WithMoveTo(s));
            }

            #endregion

            #region remove comments

            private string removeComments(string line)
            {
                line = removeCommentEndOfLine(line);
                return removeMultilineComments(line);
            }

            private string removeCommentEndOfLine(string line)
                => line.Substring(0, indexEndOfLineComment(line));

            private string removeMultilineComments(string line)
                => string.Join(" ", excludeMultilineComments(line));

            private int indexEndOfLineComment(string line)
            {
                int index = line.IndexOf(';');
                return (index >= 0) ? index : line.Length;
            }

            private IEnumerable<string> excludeMultilineComments(string line)
            {
                int start = 0;
                var indexes = getrMultiLineIndexes(line);
                foreach (var tuple in indexes)
                {
                    if (commentShouldBeclosed && tuple.Character.Equals('}'))
                    {
                        Errors.Add(Error($"Open Comment {{ expected"));
                        break;
                    }
                    if (!commentShouldBeclosed && tuple.Character.Equals('{'))
                    {
                        Errors.Add(Error($"Closed Comment }} expected"));
                        break;
                    }

                    if (start >= line.Length)
                        break;

                    if (commentShouldBeclosed)
                        yield return line.Substring(start, tuple.Index - start);
                    start = tuple.Index + 1;
                    commentShouldBeclosed = !commentShouldBeclosed;
                }

                if (start == 0)
                    yield return line;
                else if (commentShouldBeclosed)
                    yield return line.Substring(start);
            }

            private IEnumerable<(char Character, int Index)> getrMultiLineIndexes(string line)
                => line
                .Select((c, i) => (Character: c, Index: i))
                .Where(w => w.Character.Equals('{') || w.Character.Equals('}'));

            #endregion

            #endregion
        }
    }
}
