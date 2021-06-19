using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.PositionBridge.Board;

namespace Chess.AF.Helpers
{
    public class Material
    {
        private PiecesIterator<PiecesEnum> Iterator { get; set; }
        public Material(PiecesIterator<PiecesEnum> iterator)
        {
            Iterator = iterator;
        }

        public int Count()
        {
            int count = 0;
            foreach (var piece in Iterator)
                count += ValueOf(piece.Piece);
            return count;
        }

        private int ValueOf(PiecesEnum piece)
        {
            int multiplier = getMultiplier(piece);
            int value = 0;
            switch (piece)
            {
                case PiecesEnum.BlackPawn:
                case PiecesEnum.WhitePawn:
                    value = 1;
                    break;
                case PiecesEnum.BlackKnight:
                case PiecesEnum.WhiteKnight:
                case PiecesEnum.BlackBishop:
                case PiecesEnum.WhiteBishop:
                    value = 3;
                    break;
                case PiecesEnum.BlackRook:
                case PiecesEnum.WhiteRook:
                    value = 5;
                    break;
                case PiecesEnum.BlackQueen:
                case PiecesEnum.WhiteQueen:
                    value = 9;
                    break;
            }
            return value * multiplier;
        }

        private int getMultiplier(PiecesEnum piece)
            => ((int)piece < 8) ? -1 : 1;
    }
}
