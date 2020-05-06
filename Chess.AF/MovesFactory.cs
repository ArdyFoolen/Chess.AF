﻿using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF
{
    public class MovesFactory
    {
        public static IEnumerable<SquareEnum> Create(PieceEnum piece, SquareEnum square, Option<Position> position)
        {
            var moves = Create(piece);
            foreach (var m in moves.GetIteratorFor(square, position).Iterate())
                yield return m.Square;
        }

        private static Moves Create(PieceEnum piece)
        {
            switch (piece)
            {
                case PieceEnum.Knight:
                    return KnightMoves.Get();
                case PieceEnum.Bishop:
                    return BishopMoves.Get();
                case PieceEnum.Rook:
                    return RookMoves.Get();
                case PieceEnum.Queen:
                    return QueenMoves.Get();
                case PieceEnum.King:
                    return KingMoves.Get();
                case PieceEnum.Pawn:
                    return PawnMoves.Get();
            }
            return null;
        }
    }
}
