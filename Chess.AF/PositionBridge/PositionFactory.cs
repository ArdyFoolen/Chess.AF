using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.PositionBridge
{
    internal partial class PositionImpl
    {
        public static class PositionFactory
        {
            public static IPositionImpl Create(Fen fen, IPositionAbstraction abstraction)
            {
                ulong[] maps = new ulong[14];
                int i = 0;
                fen.ForEachPosition()
                    .Where(c => !'/'.Equals(c))
                    .Select(c => new { index = i += int.TryParse(c.ToString(), out int result) ? result : 1, piece = c })
                    .Where(ic => !char.IsDigit(ic.piece))
                    .ForEach(ic => ic.piece.SetToMaps(maps, ic.index - 1));

                IPositionImpl position = new PositionImpl(abstraction, maps);
                return position;
            }
        }
    }
}
