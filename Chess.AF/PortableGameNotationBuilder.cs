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
            private GameResult gameResult;
            private Game game = new Game();

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
                loadGame();
                moveText = sanitizeMoveText(moveText);
                var moves = splitMoves(moveText);
                makeMovesToGame(moves);
                return None;
            }

            private void makeMovesToGame(Option<List<string>> moves)
                => moves.Map(m => makeMovesToGame(m));

            private Game makeMovesToGame(List<string> moves)
            {
                foreach (string move in moves)
                {
                    var halfMoves = move.Split();
                    foreach (string halfMove in halfMoves)
                        makeMoveToGame(halfMove);
                }
                return game;
            }

            private void makeMoveToGame(string halfMove)
            {
                var piece = halfMove.FromSanToPiece();
                var promoted = halfMove.FromSanToPromoted(piece);
                halfMove = sanitizeHalfMove(halfMove, piece);
            }

            private string sanitizeHalfMove(string halfMove, Option<PieceEnum> piece)
                => piece.Match(
                    None: () => halfMove,
                    Some: p => sanitizeHalfMove(halfMove, p));

            private string sanitizeHalfMove(string halfMove, PieceEnum piece)
                => piece.Equals(PieceEnum.Pawn)
                ? halfMove.Replace("x", string.Empty).Replace("-", string.Empty).Split('=')[0]
                : halfMove.Replace("x", string.Empty).Replace("-", string.Empty).Split('=')[0].Substring(1);

            private void loadGame()
                => tagPairDict.Map(d => loadGame(d));

            private Game loadGame(Dictionary<string, string> dict)
            {
                if (dict.ContainsKey("setup") && dict["setup"].Equals("1") && dict.ContainsKey("fen"))
                    game.Load(dict["fen"]);
                else
                    game.Load();
                return game;
            }

            private Option<List<string>> splitMoves(string moveText)
            {
                List<string> moves = new List<string>();
                var splits = moveText.Split(' ');
                var indexes = generateMoveIndexes(splits);
                int index = 0;
                int prevMoveNbr = 0;
                while (index < indexes.Count())
                {
                    var i1 = indexes[index];
                    var i2 = index + 1 < indexes.Count() ? indexes[index+1] : 999;
                    var take = i2 - i1 - 1;
                    if (!int.TryParse(splits[i1].Replace(".", string.Empty), out int moveNbr))
                        return None;
                    if (moveNbr == prevMoveNbr)
                        moves[moves.Count() - 1] = string.Join(" ", moves[moves.Count() - 1], string.Join(" ", splits.Skip(i1 + 1).Take(take).ToArray()));

                    else
                        moves.Add(string.Join(" ", splits.Skip(i1 + 1).Take(take).ToArray()));

                    prevMoveNbr = moveNbr;
                    index += 1;
                }

                return Some(moves);
            }

            private List<int> generateMoveIndexes(string[] splits)
                => splits.Select((s, i) => new { str = s, index = i }).Where(w => w.str.Contains(".")).Select(x => x.index).ToList();

            private string sanitizeMoveText(string moveText)
            {
                var lines = splitMoveTextIntoLines(moveText);
                moveText = string.Join(" ", removeCommentsEndToLine(lines).ToArray());
                moveText = removeCommentsMultipleLines(moveText);
                SetGameResult(moveText);
                moveText = RemoveGameResult(moveText);
                moveText = ReplaceThreeDots(moveText);
                return moveText;
            }

            private string ReplaceThreeDots(string moveText)
                => moveText.Replace("...", ".");

            private string RemoveGameResult(string moveText)
            {
                var words = moveText.Split();
                return string.Join(" ", words.Take(words.Length - 1).Where(w => !string.IsNullOrWhiteSpace(w)));
            }

            private void SetGameResult(string moveText)
                => gameResult = moveText.Split().Reverse().Take(1).FirstOrDefault().ToGameResult();

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
                    eventValues.Add(kv.Key.ToLowerInvariant(), kv.Value);
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
                    if (!dict.ContainsKey(str.ToLowerInvariant()))
                        return false;
                return true;
            }

            private bool isValidTagPairsMoveText(string[] parts)
                => isValid = isValid && parts.Length == 2;
        }
    }
}
