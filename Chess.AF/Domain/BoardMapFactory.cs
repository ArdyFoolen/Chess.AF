using AF.Functional;
using Chess.AF.ImportExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Domain
{
    public partial class BoardMap
    {
        internal static class BoardMapFactory
        {
            public static IBoardMap Create(Fen fen, IBoard abstraction)
            {
                ulong[] maps = new ulong[14];
                int i = 0;
                fen.ForEachPosition()
                    .Where(c => !'/'.Equals(c))
                    .Select(c => new { index = i += int.TryParse(c.ToString(), out int result) ? result : 1, piece = c })
                    .Where(ic => !char.IsDigit(ic.piece))
                    .ForEach(ic => ic.piece.SetToMaps(maps, ic.index - 1));

                IBoardMap boardMap = new BoardMap(abstraction, maps);
                return boardMap;
            }
        }
    }
}
