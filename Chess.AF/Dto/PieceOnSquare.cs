using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Dto
{
    public class PieceOnSquare<T>
             where T : Enum
    {
        public T Piece { get; }
        public SquareEnum Square { get; }
        public bool IsSelected { get; }

        public PieceOnSquare(T piece, SquareEnum square, bool isSelected = false)
        {
            Piece = piece;
            Square = square;
            IsSelected = isSelected;
        }
    }
}
