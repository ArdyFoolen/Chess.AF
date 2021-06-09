using AF.Functional;
using Chess.AF.Enums;
using Chess.AF.PositionBridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.Position;

namespace Chess.AF
{
    public class RookMoves : Moves
    {
        private static RookMoves instance = null;
        private RookMoves() { }

        public static RookMoves Get()
        {
            if (instance == null)
                instance = new RookMoves();
            return instance;
        }

        public Position.PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, Option<Position> position, PieceEnum pieceEnum = PieceEnum.Rook)
            => position.Match(
                None: () => new PiecesIterator<PieceEnum>((pieceEnum, 0ul)),
                Some: p => new PiecesIterator<PieceEnum>((pieceEnum, MovesDictionaries.GetRookMovesMapFor(p, square)))
                );

        public Position.PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, IPositionImpl position, bool isWhiteToMove, PieceEnum pieceEnum = PieceEnum.Rook)
            => new PiecesIterator<PieceEnum>((pieceEnum, MovesDictionaries.GetRookMovesMapFor(position, square, isWhiteToMove)));
    }
}
