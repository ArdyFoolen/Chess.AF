using Chess.AF.Dto;
using Chess.AF.Enums;
using Chess.AF.PieceMoves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chess.AF.Domain
{
    public interface ISquareNumberAttackedVisitor : IBoardMapVisitor
    {
        IEnumerable<AttackSquare> Iterator { get; }
        FilterFlags Flags { get; set; }
    }

    public partial class BoardMap
    {
        public static ISquareNumberAttackedVisitor GetSquareNumberAttackedVisitor()
            => new SquareNumberAttackedVisitor();
        public static ISquareNumberAttackedVisitor GetSquareNumberAttackedVisitor(FilterFlags flags = FilterFlags.Both)
            => new SquareNumberAttackedVisitor(flags);

        private class SquareNumberAttackedVisitor : BoardMapVisitor, ISquareNumberAttackedVisitor
        {
            public FilterFlags Flags { get; set; }
            public IEnumerable<AttackSquare> Iterator { get; private set; }

            public SquareNumberAttackedVisitor() : this(FilterFlags.Both) { }
            public SquareNumberAttackedVisitor(FilterFlags flags = FilterFlags.Both)
            {
                this.Flags = flags;
            }

            public override void Visit(BoardMap map)
                => Iterator = Enum<SquareEnum>
                .AsEnumerable()
                .Select(s => new AttackSquare() { Square = s, Count = 0 })
                .Select(a => Count(map, a));

            private AttackSquare Count(BoardMap map, AttackSquare attackSquare)
            {
                attackSquare.Count = CountAttacks(map, attackSquare.Square);
                return attackSquare;
            }

            private bool ShouldFilterWhite()
                => (Flags & FilterFlags.White) == FilterFlags.White;

            private bool ShouldFilterBlack()
                => (Flags & FilterFlags.Black) == FilterFlags.Black;


            private int CountAttacks(BoardMap map, SquareEnum square)
            {
                if (!ShouldFilterWhite() && !ShouldFilterBlack())
                    return 0;

                var pieceOnSquare = map.GetPieceOnSquare(square);
                if (pieceOnSquare == null)
                    return 0;

                int count = 0;
                if (ShouldCountOpponentMoves(map, pieceOnSquare))
                    count = CountOpponentMoves(map, square);
                else if (ShouldCountCurrentMoves(map, pieceOnSquare))
                    count = CountMoves(map, square);

                return count;
            }

            private bool ShouldCountCurrentMoves(BoardMap map, PieceOnSquare<PiecesEnum> pieceOnSquare)
                => ShouldFilterWhite() && pieceOnSquare.Piece.IsWhitePiece() && !map.IsWhiteToMove ||
                    ShouldFilterBlack() && pieceOnSquare.Piece.IsBlackPiece() && map.IsWhiteToMove;

            private bool ShouldCountOpponentMoves(BoardMap map, PieceOnSquare<PiecesEnum> pieceOnSquare)
                => ShouldFilterWhite() && pieceOnSquare.Piece.IsWhitePiece() && map.IsWhiteToMove ||
                    ShouldFilterBlack() && pieceOnSquare.Piece.IsBlackPiece() && !map.IsWhiteToMove;

            private int CountMoves(BoardMap map, SquareEnum square)
                => map.GetIteratorForAll<PieceEnum>()
                    .SelectMany(p => MovesFactory.Create(p.Piece, p.Square, map))
                    .Count(f => f.Square == square);

            private int CountOpponentMoves(BoardMap map, SquareEnum square)
            {
                var opponent = (Board)map.CreateOpponent();
                return opponent.GetIteratorForAll<PieceEnum>()
                    .SelectMany(p => MovesFactory.Create(p.Piece, p.Square, opponent.Implementor))
                    .Count(f => f.Square == square);
            }
        }
    }
}
