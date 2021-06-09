using AF.Functional;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF.UCIEngine
{
    public class PositionCommandDto
    {
        public bool IsStartPos { get; }
        public string Fen { get; }
        public string[] Moves { get; }

        public Option<SquareEnum> LastFrom { get; }
        public Option<SquareEnum> LastTo { get; }
        public Option<PieceEnum> Promoted { get; }

        private PositionCommandDto(string[] cmdParms)
        {
            IsStartPos = cmdParms[0].Trim().StartsWith("startpos");
            if (!IsStartPos)
                Fen = cmdParms[0].GetFenFromCommandParameter();
            
            if (cmdParms.Length > 1)
            {
                Moves = cmdParms[1].Trim().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (Moves.Length > 0)
                {
                    var move = Moves[Moves.Length - 1];
                    LastFrom = move.Substring(0, 2).ToSquare();
                    LastTo = move.Substring(2, 2).ToSquare();
                    if (move.Length == 5)
                        Promoted = move.Substring(4, 1).ToPiece();
                }
            }
        }

        public static Option<PositionCommandDto> Of(string command)
        {
            var cmdParms = command.SplitPositionsCommand();
            if (IsValid(cmdParms))
                return new PositionCommandDto(cmdParms);
            return None;
        }

        private static bool IsValid(string[] cmdParms)
            => (cmdParms.Length >= 1 && cmdParms[0].Trim().StartsWith("startpos") ||
                (cmdParms.Length >= 1 && cmdParms[0].Trim().StartsWith("fen")))
                ? true : false;
    }
}
