﻿using AF.Functional;
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
        public class PgnReader : IPgnTagStateContext
        {
            private IGameBuilder Builder = new GameBuilder(new Game());
            public IEnumerable<string> Lines { get; private set; }
            public PgnTagState State { get; set; }
            public List<Error> Errors { get; private set; } = new List<Error>();

            private Dictionary<string, string> EventTags;
            public Pgn Pgn { get; private set; }

            private readonly Action<string> TagReadAction;
            private readonly Action<string> MoveReadAction;
            private Action<string> ReadAction;
            private bool commentShouldBeclosed = true;

            private const string movePattern = @"([NBRQK])?([a-h])?([1-8])?([-x])?([a-h][1-8])(=)?([NBRQ])?|O-O-O|O-O";
            private static Regex moveRegex = new Regex(movePattern, RegexOptions.Compiled);

            public PgnReader(string pgnFile)
            {
                Pgn = new Pgn(pgnFile);

                Lines = pgnFile.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                TagReadAction = line => State.TryAddTagPair(line).Match(Invalid: ie => { Errors.AddRange(ie); return Unit(); }, Valid: kv => { return Unit(); });
                MoveReadAction = line => ReadMoves(line);
            }

            public void Read()
            {
                EventTags = new Dictionary<string, string>();
                State = PgnTagState.CreateInitialState(this, EventTags);
                ReadAction = TagReadAction;
                bool isReadingMoveText = false;

                foreach (string line in Lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        if (isReadingMoveText)
                            break;
                        validateSevenTagRoster();
                        commentShouldBeclosed = true;
                        WithLoad();
                        ReadAction = MoveReadAction;
                        isReadingMoveText = true;
                    }
                    else
                        ReadAction(line);
                }
                WithResult();

                Pgn.Game = Builder.Game;
                Pgn.TagPairDictionary = EventTags;
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

            private void validateSevenTagRoster()
            {
                if (EventTags.Keys.Count() < 7)
                    Errors.Add(Error($"Seven Tag Roster count {EventTags.Keys.Count()} not valid"));
            }

            private void WithLoad()
            {
                if (EventTags.ContainsKey(nameof(FenSetupEnum.Setup).ToLowerInvariant()) && EventTags[nameof(FenSetupEnum.Setup).ToLowerInvariant()].Equals("1") && EventTags.ContainsKey(nameof(FenSetupEnum.FEN).ToLowerInvariant()))
                    Builder.WithFen(EventTags[nameof(FenSetupEnum.FEN).ToLowerInvariant()]);
                else
                    Builder.WithDefault();
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

            private void WithResult()
                => Builder.With(EventTags[nameof(SevenTagRosterEnum.Result).ToLowerInvariant()].ToGameResult());

            private string removeComments(string line)
            {
                line = removeCommentEndOfLine(line);
                return removeMultilineComments(line);
            }

            private string removeMultilineComments(string line)
                => string.Join(" ", excludeMultilineComments(line));

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

            private string removeCommentEndOfLine(string line)
                => line.Substring(0, indexEndOfLineComment(line));

            private int indexEndOfLineComment(string line)
            {
                int index = line.IndexOf(';');
                return (index >= 0) ? index : line.Length;
            }
        }
    }
}