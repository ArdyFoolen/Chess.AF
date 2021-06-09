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
    public class KingMoves : Moves
    {
        private static KingMoves instance = null;
        private KingMoves() { }

        public static KingMoves Get()
        {
            if (instance == null)
                instance = new KingMoves();
            return instance;
        }

        public PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, Option<Position> position, PieceEnum pieceEnum = PieceEnum.King)
            => position.Match(
                None: () => new PiecesIterator<PieceEnum>((pieceEnum, 0ul)),
                Some: p => new PiecesIterator<PieceEnum>((pieceEnum, p.IncludeRokade(p.ExcludeOpponentKing(p.ExcludeOwnPieces(MovesDictionaries.KingMovesDictionary[square])))))
                );

        public PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, IPositionImpl position, bool isWhiteToMove, PieceEnum pieceEnum = PieceEnum.King)
            => new PiecesIterator<PieceEnum>((pieceEnum, position.IncludeRokade(position.ExcludeOpponentKing(position.ExcludeOwnPieces(MovesDictionaries.KingMovesDictionary[square], isWhiteToMove), isWhiteToMove), isWhiteToMove)));
    }
}
