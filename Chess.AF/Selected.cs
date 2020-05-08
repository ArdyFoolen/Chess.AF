using AF.Functional;
using AF.Functional.Option;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.Position;
using static AF.Functional.F;

namespace Chess.AF
{
    public class Selected
    {
        public PieceEnum Piece { get; }
        public PiecesIterator<PieceEnum> Iterator { get; }

        private Selected(PieceEnum piece, PiecesIterator<PieceEnum> iterator)
        {
            this.Piece = piece;
            this.Iterator = iterator;
        }

        public static Option<Selected> Of(PieceEnum piece, PiecesIterator<PieceEnum> iterator)
            => Some(new Selected(piece, iterator));

        public bool Contains(PiecesEnum piece, SquareEnum square)
        {
            if (!this.Piece.IsEqual(piece)) return false;
            foreach (var sq in Iterator.Iterate())
                if (square.Equals(sq.Square))
                    return true;
            return false;
        }

        public IEnumerable<SquareEnum> Moves(Option<Position> position)
        {
            foreach (var pc in Iterator.Iterate())
                foreach (var tuple in MovesFactory.Create(Piece, pc.Square, position))
                    yield return tuple.Square;
        }
    }
}
