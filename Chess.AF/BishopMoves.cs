using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.Position;

namespace Chess.AF
{
    public class BishopMoves : Moves
    {
        private static BishopMoves instance = null;
        private BishopMoves() { }

        public static BishopMoves Get()
        {
            if (instance == null)
                instance = new BishopMoves();
            return instance;
        }

        public Position.PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, Option<Position> position, PieceEnum pieceEnum = PieceEnum.Bishop)
            => position.Match(
                None: () => new PiecesIterator<PieceEnum>((pieceEnum, 0ul)),
                Some: p => new PiecesIterator<PieceEnum>((pieceEnum, GetMapFor(p, square)))
                );

        private ulong GetMapFor(Position position, SquareEnum square)
        {
            var maps = MovesDictionaries.BishopMovesDictionary[square];
            ulong upMap = square.UpBitsOn();
            ulong dwMap = square.DownBitsOn();

            // 1
            ulong upR = maps.a1h8Map & upMap;
            upR = position.GetMinMap(upR);

            // 2
            ulong dwL = maps.a1h8Map & dwMap;
            dwL = position.GetMaxMap(dwL);

            // 3
            ulong upL = maps.a8h1Map & upMap;
            upL = position.GetMinMap(upL);

            // 4
            ulong dwR = maps.a8h1Map & dwMap;
            dwR = position.GetMaxMap(dwR);

            // 5
            return upR | upL | dwL | dwR;
        }
    }
}
