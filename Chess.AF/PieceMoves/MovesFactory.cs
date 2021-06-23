using AF.Functional;
using Chess.AF.Enums;
using Chess.AF.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.PieceMoves
{
    internal class MovesFactory
    {
        public static IEnumerable<(PieceEnum Piece, SquareEnum Square)> Create(PieceEnum piece, SquareEnum square, IBoardMap boardMap)
        {
            var moves = Create(piece);
            foreach (var m in moves.GetIteratorFor(square, boardMap, piece))
                yield return (m.Piece, m.Square);
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
