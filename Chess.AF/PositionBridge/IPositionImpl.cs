using AF.Functional;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.PositionBridge.PositionAbstraction;

namespace Chess.AF.PositionBridge
{
    public interface IPositionImpl
    {
        bool IsTake { get; }

        IPositionImpl CreateCopy();
        SquareEnum KingSquare(bool isWhiteToMove);
        void SetBits(Move move, bool isWhiteToMove);
        PiecesIterator<PieceEnum> GetIteratorFor(PieceEnum piece, bool isWhiteToMove);
        PiecesIterator<T> GetIteratorForAll<T>(bool isWhiteToMove) where T : Enum;

        bool IsInCheck(bool isWhiteToMove);
        bool OpponentIsInCheck(bool isWhiteToMove);

        void SetRokade(Func<RokadeEnum> rokade);
        void SetEpSquare(Func<Option<SquareEnum>> epSquare);
        RokadeEnum PossibleRokade(bool isWhiteToMove);

        ulong ExcludeOwnPieces(ulong map, bool isWhiteToMove);
        ulong IncludeRokade(ulong map, bool isWhiteToMove);
        ulong ExcludeOpponentKing(ulong map, bool isWhiteToMove);
        ulong GetMinMap(ulong map, bool isWhiteToMove);
        ulong GetMaxMap(ulong map, bool isWhiteToMove);
        ulong GetPawnMapFor(SquareEnum square, ulong mvMap, ulong tkMap, bool isWhiteToMove);
    }
}
