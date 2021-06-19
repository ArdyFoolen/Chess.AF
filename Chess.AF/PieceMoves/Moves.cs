using AF.Functional;
using Chess.AF.Enums;
using Chess.AF.PositionBridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.PositionBridge.Board;

namespace Chess.AF.PieceMoves
{
    internal interface Moves
    {
        PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, IBoardMap position, PieceEnum pieceEnum);
    }
}
