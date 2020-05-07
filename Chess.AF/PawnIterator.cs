using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF
{
    public partial class Position
    {
        public class PawnIterator<T> : PiecesIterator<T>
            where T : Enum
        {
            public PawnIterator(params (T Piece, ulong Map)[] maps) : base(maps) { }
            public PawnIterator(params PawnIterator<T>[] iterators) : base(iterators) { }

            public override IEnumerable<(T Piece, SquareEnum Square, bool IsSelected)> Iterate(Func<T, SquareEnum, bool> isSelected)
            {
                for (int m = 0; m < Maps.Count(); m++)
                    foreach (int i in Enumerable.Range(0, 64))
                        if ((Maps[m].Map & (1ul << i)) != 0)
                            if (IsPromoted((SquareEnum)(63 - i)))
                                foreach (var item in IteratePromotedPawn(Maps[m].Piece, (SquareEnum)(63 - i), isSelected(Maps[m].Piece, (SquareEnum)(63 - i))))
                                    yield return item;
                            else
                                yield return (Maps[m].Piece, (SquareEnum)(63 - i), isSelected(Maps[m].Piece, (SquareEnum)(63 - i)));
            }

            private bool IsPromoted(SquareEnum square)
                => square.Row() == 0 || square.Row() == 7;

            private IEnumerable<(T Piece, SquareEnum Square, bool IsSelected)> IteratePromotedPawn(T piece, SquareEnum square, bool IsSelected)
            {
                yield return ((T)Enum.Parse(typeof(T), (Convert.ToInt32(piece) + 4).ToString()), square, IsSelected);
                yield return ((T)Enum.Parse(typeof(T), (Convert.ToInt32(piece) + 3).ToString()), square, IsSelected);
                yield return ((T)Enum.Parse(typeof(T), (Convert.ToInt32(piece) + 2).ToString()), square, IsSelected);
                yield return ((T)Enum.Parse(typeof(T), (Convert.ToInt32(piece) + 1).ToString()), square, IsSelected);
            }
        }
    }
}
