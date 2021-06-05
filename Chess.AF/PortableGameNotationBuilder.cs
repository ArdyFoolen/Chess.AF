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
        private class PortableGameNotationBuilder : PgnBuilder
        {
            private bool isValid = true;

            private Option<string[]> tagPairsMoveText;
            private Option<Dictionary<string, string>> tagPairDict;
            public override void BuildPrepare()
                => tagPairsMoveText = splitPgnStringIntoTagPairsMoveText();

            public override void BuildTagPairs()
                => tagPairDict = splitTagPairs();

            public override void BuildMoveText()
            {
                createGameFrom();
                //var moveText = splitMoveText();
            }

            private void createGameFrom()
                => tagPairsMoveText.Bind(m => createGameFrom(m[1]));

            private Option<string> createGameFrom(string moveText)
            {
                moveText = sanitizeMoveText(moveText);
                return None;
            }

            private string sanitizeMoveText(string moveText)
            {
                var lines = splitMoveTextIntoLines(moveText);
                moveText = string.Join(" ", removeCommentsEndToLine(lines).ToArray());
                return removeCommentsMultipleLines(moveText);
            }

            private string removeCommentsMultipleLines(string moveTest)
            {
                var splits = moveTest.Split(new char[] { '{', '}' });
                var lines = splits.Select((s, i) => new { line = s, index = i }).Where(w => w.index % 2 == 0).Select(x => x.line).ToArray();
                return string.Join(" ", lines);
            }

            private IEnumerable<string> removeCommentsEndToLine(string[] lines)
            {
                foreach (var line in lines)
                    yield return line.Substring(0, indexOfComment(line));
            }

            private int indexOfComment(string line)
            {
                int index = line.IndexOf(';');
                return (index >= 0) ? index : line.Length;
            }

            private string[] splitMoveTextIntoLines(string moveText)
                => moveText.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            private Option<Dictionary<string, string>> splitTagPairs()
                => tagPairsMoveText.Bind(m => splitTagPairs(m[0]));

            private Option<Dictionary<string, string>> splitTagPairs(string tagPair)
            {
                var splits = tagPair.Split(new string[] { "]\n[", "]\r\n[", "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
                var dict = splitTagPairs(splits);
                if (!isValidSevenTagRoster(dict))
                    return None;
                return Some(dict);
            }

            private Dictionary<string, string> splitTagPairs(string[] parts)
            {
                var eventValues = new Dictionary<string, string>();
                foreach (string tagPair in parts)
                {
                    var kv = splitTagPair(tagPair);
                    eventValues.Add(kv.Key, kv.Value);
                }
                return eventValues;
            }

            private KeyValuePair<string, string> splitTagPair(string tagPair)
            {
                int index = tagPair.IndexOf(' ');
                string tag = tagPair.Substring(0, index).Trim();
                string value = cleanupValue(tagPair.Substring(index));
                return new KeyValuePair<string, string>(tag, value);
            }

            public string cleanupValue(string value)
                => value.Substring(0, value.Length - 1).Trim().Substring(1);

            private Option<string[]> splitPgnStringIntoTagPairsMoveText()
            {
                var splits = PgnString.Split(new string[] { "\n\n", "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                return isValidTagPairsMoveText(splits) ? Some(splits) : None;
            }

            private bool isValidSevenTagRoster(Dictionary<string, string> dict)
                => isValid = isValid && dict.Keys.Count() >= 7 && AreValidSevenTagRosterKeys(dict);

            private bool AreValidSevenTagRosterKeys(Dictionary<string, string> dict)
            {
                foreach (string str in Enum.GetNames(typeof(SevenTagRosterEnum)))
                    if (!dict.ContainsKey(str))
                        return false;
                return true;
            }

            private bool isValidTagPairsMoveText(string[] parts)
                => isValid = isValid && parts.Length == 2;
        }
    }
}
