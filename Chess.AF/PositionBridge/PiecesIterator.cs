using Chess.AF.Dto;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.PositionBridge
{
    public partial class PositionAbstraction
    {
        public class PiecesIterator<T> : IEnumerable<PieceOnSquare<T>>
            where T : Enum
        {
            private List<PieceMap<T>> Maps { get; } = new List<PieceMap<T>>();
            internal PiecesIterator(params PieceMap<T>[] maps)
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

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
                => GetEnumerator();
        }
    }
}
