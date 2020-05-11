using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.Position;

namespace Chess.AF
{
    public class KnightMoves : Moves
    {
        private static KnightMoves instance = null;
        private KnightMoves() { }

        public static KnightMoves Get()
        {
            if (instance == null)
                instance = new KnightMoves();
            return instance;
        }

        public PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, Option<Position> position, PieceEnum pieceEnum = PieceEnum.Knight)
            => position.Match(
                None: () => new PiecesIterator<PieceEnum>((pieceEnum, 0ul)),
                Some: p => new PiecesIterator<PieceEnum>((pieceEnum, p.ExcludeOwnPieces(MovesDictionaries.KnightMovesDictionary[square])))
                );

        public PiecesIterator<PieceEnum> this[SquareEnum square]
        {
            get
            {
                return new PiecesIterator<PieceEnum>((PieceEnum.Knight, MovesDictionaries.KnightMovesDictionary[square]));
            }
        }
    }
}
