using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Functional;

namespace Chess.AF
{
    public partial class Position
    {
        public static class PositionFactory
        {
            public static Position Create(Fen fen)
            {
                ulong[] maps = new ulong[14];
                int i = 0;
                fen.ForEachPosition()
                    .Where(c => !'/'.Equals(c))
                    .Select(c => new { index = i += int.TryParse(c.ToString(), out int result) ? result : 1, piece = c })
                    .Where(ic => !char.IsDigit(ic.piece))
                    .ForEach(ic => ic.piece.SetToMaps(maps, ic.index - 1));

                Position position = new Position(maps, fen.IsWhiteToMove, fen.WhiteRokade, fen.BlackRokade, fen.EnPassant, fen.PlyCount, fen.MoveNumber);
                return position;
            }
        }
    }
}
