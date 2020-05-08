using AF.Functional;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.Position;

namespace Chess.AF
{
    public class PawnMoves : Moves
    {
        private static readonly ulong b7Map = 0xa000a00000000000;
        private IDictionary<int, ulong> MovesDictionary = new Dictionary<int, ulong>();

        private static PawnMoves instance = null;
        private PawnMoves()
        {
            CreateMovesMap();
        }

        public static PawnMoves Get()
        {
            if (instance == null)
                instance = new PawnMoves();
            return instance;
        }

        private void CreateMovesMap()
        {
            for (int file = 0; file < 8; file++)
            {
                ulong cpTakeMap = b7Map;

                if (file == 0 || file == 7)
                    cpTakeMap = cpTakeMap.BitOffForFile(file);

                if (file == 0)
                    cpTakeMap <<= 1;
                else
                    cpTakeMap >>= file - 1;

                MovesDictionary[file] = cpTakeMap;
            }
        }

        public Position.PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, Option<Position> position, PieceEnum pieceEnum = PieceEnum.Pawn)
            => position.Match(
                None: () => new PiecesIterator<PieceEnum>((PieceEnum.Pawn, 0ul)),
                Some: p => new PiecesIterator<PieceEnum>((PieceEnum.Pawn, GetMapFor(p, square)))
                );

        // Make a map for black/white pawns, later set bits of either below or above square
        private ulong GetMapFor(Position position, SquareEnum square)
        {
            var mvMap = 0x0000000000000000ul;

            if (square.Row() < 7)
                mvMap = mvMap.SetBit((int)square + 8);
            if (square.Row() > 0)
                mvMap = mvMap.SetBit((int)square - 8);
            if (square.Row() == 1)
                mvMap = mvMap.SetBit((int)square + 16);
            if (square.Row() == 6)
                mvMap = mvMap.SetBit((int)square - 16);

            var tkMap = MovesDictionary[square.File()];
            if (square.Row() < 1)
                tkMap <<= 8;
            else
                tkMap >>= 8 * (square.Row() - 1);

            return position.GetPawnMapFor(square, mvMap, tkMap);
        }
    }
}
