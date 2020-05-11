using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.Position;

namespace Chess.AF
{
    public class QueenMoves : Moves
    {
        private static QueenMoves instance = null;
        private QueenMoves() { }

        public static QueenMoves Get()
        {
            if (instance == null)
                instance = new QueenMoves();
            return instance;
        }

        public Position.PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, Option<Position> position, PieceEnum pieceEnum = PieceEnum.Queen)
            => position.Match(
                None: () => new PiecesIterator<PieceEnum>((pieceEnum, 0ul)),
                Some: p => CreateIterator(square, position, pieceEnum)
                );

        private PiecesIterator<PieceEnum> CreateIterator(SquareEnum square, Option<Position> position, PieceEnum pieceEnum)
        {
            var bIterator = BishopMoves.Get().GetIteratorFor(square, position, pieceEnum);
            var rIterator = RookMoves.Get().GetIteratorFor(square, position, pieceEnum);
            return new PiecesIterator<PieceEnum>(bIterator, rIterator);
        }
    }
}
