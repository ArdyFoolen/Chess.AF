﻿using AF.Functional;
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
        bool IsInCheck { get; }
        SquareEnum KingSquare { get; }

        IPositionImpl SetBits(Move move, IPositionAbstraction abstraction);
        PiecesIterator<PieceEnum> GetIteratorFor(PieceEnum piece);
        PiecesIterator<T> GetIteratorForAll<T>() where T : Enum;

        RokadeEnum PossibleRokade();

        ulong ExcludeOwnPieces(ulong map);
        ulong IncludeRokade(ulong map);
        ulong ExcludeOpponentKing(ulong map);
        ulong GetMinMap(ulong map);
        ulong GetMaxMap(ulong map);
        ulong GetPawnMapFor(SquareEnum square, ulong mvMap, ulong tkMap);
    }
}
