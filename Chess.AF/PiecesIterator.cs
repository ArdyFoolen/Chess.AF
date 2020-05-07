using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF
{
    public partial class Position
    {
        public class PiecesIterator<T>
            where T : Enum
        {
            public List<(T Piece, ulong Map)> Maps { get; } = new List<(T Piece, ulong Map)>();
            public PiecesIterator(params (T Piece, ulong Map)[] maps)
            {
                this.Maps.AddRange(maps);
            }
            public PiecesIterator(params PiecesIterator<T>[] iterators)
            {
                foreach (PiecesIterator<T> iterator in iterators)
                    this.Maps.AddRange(iterator.Maps);
            }
            public virtual IEnumerable<(T Piece, SquareEnum Square, bool IsSelected)> Iterate()
                => Iterate((p, s) => false);
            public virtual IEnumerable<(T Piece, SquareEnum Square, bool IsSelected)> Iterate(Func<T, SquareEnum, bool> isSelected)
            {
                for (int m = 0; m < Maps.Count(); m++)
                    foreach (int i in Enumerable.Range(0, 64))
                        if ((Maps[m].Map & (1ul << i)) != 0)
                            yield return (Maps[m].Piece, (SquareEnum)(63 - i), isSelected(Maps[m].Piece, (SquareEnum)(63 - i)));
            }

        }
    }
}
