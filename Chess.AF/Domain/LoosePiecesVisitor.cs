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
    public interface ILoosePiecesVisitor : IBoardMapVisitor
    {
        IEnumerable<SquareEnum> Iterator { get; }
    }

    public partial class BoardMap
    {
        public static ILoosePiecesVisitor GetLoosePiecesVisitor()
            => new LoosePiecesVisitor();

        private class LoosePiecesVisitor : BoardMapVisitor, ILoosePiecesVisitor
        {
            public IEnumerable<SquareEnum> Iterator { get; private set; }

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
                if (!map.IsWhiteToMove && pieceOnSquare.Piece.IsWhitePiece() ||
                    map.IsWhiteToMove && pieceOnSquare.Piece.IsBlackPiece())
                    return WhitePieceWhiteNotToMoveFilter(map, pieceOnSquare.Square);
                else
                    return WhitePieceWhiteToMoveFilter(map, pieceOnSquare.Square);
            }
        }
    }
}
