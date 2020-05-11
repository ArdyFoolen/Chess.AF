using AF.Functional;
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
    }
}
