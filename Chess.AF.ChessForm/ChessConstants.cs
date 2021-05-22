using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.ChessForm
{
    public static class ChessConstants
    {
        public static readonly int SquareWidth = 70;
        public static readonly int BoardWidth = SquareWidth * 8;
        public static readonly int FormHeight = BoardWidth + 73; // 633
        public static readonly int PieceWidth = SquareWidth - 10;
    }
}
