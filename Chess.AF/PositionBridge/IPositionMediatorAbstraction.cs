using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.PositionBridge.PositionAbstraction;

namespace Chess.AF.PositionBridge
{
    public interface IPositionMediatorAbstraction
    {
        IPositionImpl PositionImpl { get; }
        bool IsTake { get; }
        bool IsInCheck { get; }

        SquareEnum KingSquare { get; }

        void SetBits(Move move);
        PiecesIterator<PieceEnum> GetIteratorFor(PieceEnum piece);
        PiecesIterator<T> GetIteratorForAll<T>() where T : Enum;
        RokadeEnum PossibleRokade();

        IPositionAbstraction CreateCopy();
    }
}
