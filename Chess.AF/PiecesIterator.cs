using Chess.AF.Dto;
using Chess.AF.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF
{
    public partial class Position
    {
        public class PiecesIterator<T> : IEnumerable<PieceOnSquare<T>>
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
            private bool IsPromoted(T piece, SquareEnum square)
                => PieceEnum.Pawn.IsEqual(piece) && (square.Row() == 0 || square.Row() == 7);
            private IEnumerable<PieceOnSquare<T>> IteratePromotedPawn(T piece, SquareEnum square)
            {
                yield return new PieceOnSquare<T>((T)Enum.Parse(typeof(T), (Convert.ToInt32(piece) + 4).ToString()), square);
                yield return new PieceOnSquare<T>((T)Enum.Parse(typeof(T), (Convert.ToInt32(piece) + 3).ToString()), square);
                yield return new PieceOnSquare<T>((T)Enum.Parse(typeof(T), (Convert.ToInt32(piece) + 2).ToString()), square);
                yield return new PieceOnSquare<T>((T)Enum.Parse(typeof(T), (Convert.ToInt32(piece) + 1).ToString()), square);
            }

            public IEnumerator<PieceOnSquare<T>> GetEnumerator()
            {
                for (int m = 0; m < Maps.Count(); m++)
                    foreach (int i in Enumerable.Range(0, 64))
                        if ((Maps[m].Map & (1ul << i)) != 0)
                            if (IsPromoted(Maps[m].Piece, (SquareEnum)(63 - i)))
                                foreach (var item in IteratePromotedPawn(Maps[m].Piece, (SquareEnum)(63 - i)))
                                    yield return item;
                            else
                                yield return new PieceOnSquare<T>(Maps[m].Piece, (SquareEnum)(63 - i));
            }

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();
        }
    }
}
