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
        private static readonly ulong a8h1Map = 0x8040201008040201;
        private static readonly ulong a1h8Map = 0x0102040810204080;
        private IDictionary<SquareEnum, (ulong a8h1Map, ulong a1h8Map)> MovesDictionary = new Dictionary<SquareEnum, (ulong a8h1Map, ulong a1h8Map)>();

        private static BishopMoves instance = null;
        private BishopMoves()
        {
            CreateMovesMap();
        }

        public static BishopMoves Get()
        {
            if (instance == null)
                instance = new BishopMoves();
            return instance;
        }

        private void CreateMovesMap()
        {
            foreach (SquareEnum square in Enum.GetValues(typeof(SquareEnum)))
            {
                ulong cpA8H1Map = ShiftA8H1(square, a8h1Map);
                ulong cpA1H8Map = ShiftA1H8(square, a1h8Map);
                MovesDictionary[square] = (cpA8H1Map, cpA1H8Map);
            }
        }

        private ulong ShiftA8H1(SquareEnum square, ulong a8h1)
        {
            int toShift = (int)square % 9;
            if (toShift == 0)
                return a8h1;
            if (square.Row() > square.File())
            {
                toShift = 9 - toShift;
                a8h1 = (a8h1 << (toShift * 8)) >> (toShift * 8 - toShift);
            }
            else
                a8h1 = (a8h1 >> (toShift * 8)) << (toShift * 8 - toShift);
            return a8h1 ;
        }

        private ulong ShiftA1H8(SquareEnum square, ulong a1h8)
        {
            int toShift = square == SquareEnum.a8 ? 7 : (63 - (int)square) % 7;
            if (toShift == 0 && square != SquareEnum.h1)
                return a1h8;
            if (square.Row() > 7 - square.File())
            {
                toShift = 7 - toShift;
                a1h8 = (a1h8 << (toShift * 8)) >> (toShift * 8 + toShift);
            }
            else
                a1h8 = (a1h8 >> (toShift * 8)) << (toShift * 8 + toShift);
            return a1h8;
        }

        public Position.PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, Option<Position> position, PieceEnum pieceEnum = PieceEnum.Bishop)
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
