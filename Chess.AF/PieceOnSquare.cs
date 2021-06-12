using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF
{
    public class PieceOnSquare<T>
             where T : Enum
    {
        public T Piece { get; set; }
        public SquareEnum Square { get; set; }
        public bool IsSelected { get; set; }
    }
}
