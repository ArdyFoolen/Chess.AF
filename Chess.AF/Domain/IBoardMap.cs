using AF.Functional;
using Chess.AF.Dto;
using Chess.AF.Enums;
using Chess.AF.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.Domain.Board;

namespace Chess.AF.Domain
{
    public interface IBoardMap
    {
        bool IsTake { get; }
        bool IsInCheck { get; }
        SquareEnum KingSquare { get; }

        IBoardMap SetBits(Move move, IBoard abstraction);
        PiecesIterator<PieceEnum> GetIteratorFor(PieceEnum piece);
        PiecesIterator<T> GetIteratorForAll<T>() where T : Enum;

        RokadeEnum PossibleRokade();

        ulong ExcludeOwnPieces(ulong map);
        ulong IncludeRokade(ulong map);
        ulong ExcludeOpponentKing(ulong map);
        ulong GetMinMap(ulong map);
        ulong GetMaxMap(ulong map);
        ulong GetPawnMapFor(SquareEnum square, ulong mvMap, ulong tkMap);

        void Accept(IBoardMapVisitor visitor);
    }
}
