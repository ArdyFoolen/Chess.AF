using AF.Functional;
using AF.Functional.Option;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.Position;
using static AF.Functional.F;
using Chess.AF.Enums;

namespace Chess.AF
{
    public class Selected
    {
        public PieceEnum Piece { get; }
        public PiecesIterator<PieceEnum> Iterator { get; }
        public Option<Position> Position { get; }

        private Selected(PieceEnum piece, PiecesIterator<PieceEnum> iterator, Option<Position> position)
        {
            this.Piece = piece;
            this.Iterator = iterator;
            this.Position = position;
        }

        public static Option<Selected> Of(PieceEnum piece, PiecesIterator<PieceEnum> iterator, Option<Position> position)
            => Some(new Selected(piece, iterator, position));

        public bool Contains(PiecesEnum piece, SquareEnum square)
        {
            if (!this.Piece.IsEqual(piece)) return false;
            foreach (var sq in Iterator.Iterate())
                if (square.Equals(sq.Square))
                    return true;
            return false;
        }

        public IEnumerable<SquareEnum> Moves()
        {
            foreach (var pc in Iterator.Iterate())
                foreach (var tuple in MovesFactory.Create(Piece, pc.Square, Position))
                    yield return tuple.Square;
        }
    }
}
