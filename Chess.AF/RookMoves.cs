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
        private static readonly ulong r8Map = 0xff00000000000000;
        private static readonly ulong faMap = 0x8080808080808080;
        private IDictionary<SquareEnum, (ulong r8Map, ulong faMap)> MovesDictionary = new Dictionary<SquareEnum, (ulong r8Map, ulong faMap)>();

        private static RookMoves instance = null;
        private RookMoves()
        {
            CreateMovesMap();
        }

        public static RookMoves Get()
        {
            if (instance == null)
                instance = new RookMoves();
            return instance;
        }

        private void CreateMovesMap()
        {
            foreach (SquareEnum square in Enum.GetValues(typeof(SquareEnum)))
            {
                ulong cpR8Map = r8Map >> square.Row() * 8;
                ulong cpFaMap = faMap >> square.File();
                MovesDictionary[square] = (cpR8Map, cpFaMap);
            }
        }

        public Position.PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, Option<Position> position, PieceEnum pieceEnum = PieceEnum.Rook)
            => position.Match(
                None: () => new PiecesIterator<PieceEnum>((pieceEnum, 0ul)),
                Some: p => new PiecesIterator<PieceEnum>((pieceEnum, GetMapFor(p, square)))
                );

        private ulong GetMapFor(Position position, SquareEnum square)
        {
            var maps = MovesDictionary[square];
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
