using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Dto
{
    internal class PieceMap<T>
           where T : Enum
    {
        public T Piece { get; }
        public ulong Map { get; }

        public PieceMap(T piece, ulong map)
        {
            Piece = piece;
            Map = map;
        }
    }
}
