using Chess.AF.Dto;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.Domain.Board;

namespace Chess.AF.Domain
{
    [Flags]
    public enum FilterFlags : byte
    {
        Black = 1,
        White = 2,
        Both = 3
    }

    public interface ILoosePiecesVisitor : IBoardMapVisitor
    {
        IEnumerable<SquareEnum> Iterator { get; }
        FilterFlags Flags { get; set; }
    }

    public partial class BoardMap
    {
        public static ILoosePiecesVisitor GetLoosePiecesVisitor()
            => new LoosePiecesVisitor();
        public static ILoosePiecesVisitor GetLoosePiecesVisitor(FilterFlags flags = FilterFlags.Both)
            => new LoosePiecesVisitor(flags);

        private class LoosePiecesVisitor : BoardMapVisitor, ILoosePiecesVisitor
        {
            public FilterFlags Flags { get; set; }
            public IEnumerable<SquareEnum> Iterator { get; private set; }

            public LoosePiecesVisitor() : this(FilterFlags.Both) { }
            public LoosePiecesVisitor(FilterFlags flags = FilterFlags.Both)
            {
                this.Flags = flags;
            }

            public override void Visit(BoardMap map)
                => Iterator = map.GetIteratorForAll<PiecesEnum>().Where(p => Filter(map, p)).Select(s => s.Square);

            /// <summary>
            /// Piece == White
            /// !IsWhiteToMove
            /// map.IsDefended && !map.IsAttacked
            /// -- or --
            /// Piece == Black
            /// IsWhiteToMove
            /// map.IsDefended && !map.IsAttacked
            /// </summary>
            /// 
            private static bool WhitePieceWhiteNotToMoveFilter(BoardMap map, SquareEnum square)
                => map.IsSquareDefended(square) && !map.IsSquareAttacked(square);

            /// <summary>
            /// Piece == White
            /// IsWhiteToMove
            /// map.IsAttacked && !map.IsDefended
            /// -- or --
            /// Piece == Black
            /// !IsWhiteToMove
            /// map.IsAttacked && !map.IsDefended
            /// </summary>
            private static bool WhitePieceWhiteToMoveFilter(BoardMap map, SquareEnum square)
                => map.IsSquareAttacked(square) && !map.IsSquareDefended(square);

            private bool Filter(BoardMap map, PieceOnSquare<PiecesEnum> pieceOnSquare)
            {
                if (!shouldFilter(pieceOnSquare.Piece))
                    return false;

                if (!map.IsWhiteToMove && pieceOnSquare.Piece.IsWhitePiece() ||
                    map.IsWhiteToMove && pieceOnSquare.Piece.IsBlackPiece())
                    return WhitePieceWhiteNotToMoveFilter(map, pieceOnSquare.Square);
                else if (!PieceEnum.King.IsEqual(pieceOnSquare.Piece))
                    return WhitePieceWhiteToMoveFilter(map, pieceOnSquare.Square);
 
                return false;
            }

            private bool shouldFilter(PiecesEnum piece)
                => piece.IsWhitePiece() && ShouldFilterWhite() ||
                piece.IsBlackPiece() && ShouldFilterBlack();

            private bool ShouldFilterWhite()
                => (Flags & FilterFlags.White) == FilterFlags.White;

            private bool ShouldFilterBlack()
                => (Flags & FilterFlags.Black) == FilterFlags.Black;
        }
    }
}
