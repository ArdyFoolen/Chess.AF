using AF.Functional;
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
                Some: p => new PiecesIterator<PieceEnum>((pieceEnum, GetMapFor(p, square)))
                );

        private ulong GetMapFor(Position position, SquareEnum square)
        {
            var maps = MovesDictionaries.RookMovesDictionary[square];
            ulong upMap = square.UpBitsOn();
            ulong dwMap = square.DownBitsOn();

            // 1
            ulong upL = maps.r8Map & upMap;
            upL = position.GetMinMap(upL);

            // 2
            ulong dwR = maps.r8Map & dwMap;
            dwR = position.GetMaxMap(dwR);

            // 3
            ulong upU = maps.faMap & upMap;
            upU = position.GetMinMap(upU);

            // 4
            ulong dwD = maps.faMap & dwMap;
            dwD = position.GetMaxMap(dwD);

            // 5
            return upL | upU | dwR | dwD;
        }
    }
}
