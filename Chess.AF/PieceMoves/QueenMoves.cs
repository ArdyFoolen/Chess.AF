using AF.Functional;
using Chess.AF.Enums;
using Chess.AF.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.Domain.Board;

namespace Chess.AF.PieceMoves
{
    internal class QueenMoves : Moves
    {
        private static QueenMoves instance = null;
        private QueenMoves() { }

        public static QueenMoves Get()
        {
            if (instance == null)
                instance = new QueenMoves();
            return instance;
        }

        public PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, IBoardMap boardMap, PieceEnum pieceEnum = PieceEnum.Queen)
            => CreateIterator(square, boardMap, pieceEnum);

        private PiecesIterator<PieceEnum> CreateIterator(SquareEnum square, IBoardMap boardMap, PieceEnum pieceEnum)
        {
            var bIterator = BishopMoves.Get().GetIteratorFor(square, boardMap, pieceEnum);
            var rIterator = RookMoves.Get().GetIteratorFor(square, boardMap, pieceEnum);
            return new PiecesIterator<PieceEnum>(bIterator, rIterator);
        }
    }
}
