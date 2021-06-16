using Chess.AF.Enums;
using Chess.AF.PositionBridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.PieceMoves
{
    internal static class MovesDictionaries
    {
        private static readonly ulong knightC6Map = 0x5088008850000000;

        private static readonly ulong bishopA8H1Map = 0x8040201008040201;
        private static readonly ulong bishopA1H8Map = 0x0102040810204080;

        private static readonly ulong rookRow8Map = 0xff00000000000000;
        private static readonly ulong rookFileAMap = 0x8080808080808080;

        private static readonly ulong pawnB7Map = 0xa000a00000000000;

        private static readonly ulong kingB7Map = 0xe0a0e00000000000;

        internal static readonly IDictionary<SquareEnum, ulong> KnightMovesDictionary = new Dictionary<SquareEnum, ulong>();
        internal static readonly IDictionary<SquareEnum, (ulong a8h1Map, ulong a1h8Map)> BishopMovesDictionary = new Dictionary<SquareEnum, (ulong a8h1Map, ulong a1h8Map)>();
        internal static readonly IDictionary<SquareEnum, (ulong r8Map, ulong faMap)> RookMovesDictionary = new Dictionary<SquareEnum, (ulong r8Map, ulong faMap)>();
        internal static readonly IDictionary<int, ulong> PawnMovesDictionary = new Dictionary<int, ulong>();
        internal static readonly IDictionary<SquareEnum, ulong> KingMovesDictionary = new Dictionary<SquareEnum, ulong>();

        static MovesDictionaries()
        {
            CreateKnightMovesDictionary();
            CreateBishopMovesDictionary();
            CreateRookMovesDictionary();
            CreatePawnMovesDictionary();
            CreateKingMovesMap();
        }

        #region CreateKingMovesMap

        private static void CreateKingMovesMap()
        {
            foreach (SquareEnum square in Enum.GetValues(typeof(SquareEnum)))
            {
                ulong cpMap = kingB7Map;
                if (square.File() < 1)
                    cpMap = cpMap.BitOffForFile(0);
                if (square.File() > 6)
                    cpMap = cpMap.BitOffForFile(2);
                if (square.Row() < 1)
                    cpMap = cpMap.BitOffForRow(0);
                if (square.Row() > 6)
                    cpMap = cpMap.BitOffForRow(2);

                var shift = ((int)square - (int)SquareEnum.b7);
                if (shift < 0)
                    cpMap <<= (shift * -1);
                else
                    cpMap >>= shift;

                KingMovesDictionary[square] = cpMap;
            }
        }

        #endregion

        #region CreatePawnMovesDictionary

        private static void CreatePawnMovesDictionary()
        {
            for (int file = 0; file < 8; file++)
            {
                ulong cpTakeMap = pawnB7Map;

                if (file == 0 || file == 7)
                    cpTakeMap = cpTakeMap.BitOffForFile(file);

                if (file == 0)
                    cpTakeMap <<= 1;
                else
                    cpTakeMap >>= file - 1;

                PawnMovesDictionary[file] = cpTakeMap;
            }
        }

        internal static ulong GetTakeMap(SquareEnum square)
        {
            var tkMap = PawnMovesDictionary[square.File()];
            if (square.Row() < 1)
                tkMap <<= 8;
            else
                tkMap >>= 8 * (square.Row() - 1);
            return tkMap;
        }

        #endregion

        #region CreateRookMovesDictionary

        private static void CreateRookMovesDictionary()
        {
            foreach (SquareEnum square in Enum.GetValues(typeof(SquareEnum)))
            {
                ulong cpR8Map = rookRow8Map >> square.Row() * 8;
                ulong cpFaMap = rookFileAMap >> square.File();
                RookMovesDictionary[square] = (cpR8Map, cpFaMap);
            }
        }

        internal static ulong GetRookMovesMapFor(IPositionImpl position, SquareEnum square)
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

        #endregion

        #region CreateBishopMovesDictionary

        private static void CreateBishopMovesDictionary()
        {
            foreach (SquareEnum square in Enum.GetValues(typeof(SquareEnum)))
            {
                ulong cpA8H1Map = ShiftA8H1(square, bishopA8H1Map);
                ulong cpA1H8Map = ShiftA1H8(square, bishopA1H8Map);
                BishopMovesDictionary[square] = (cpA8H1Map, cpA1H8Map);
            }
        }

        private static ulong ShiftA8H1(SquareEnum square, ulong a8h1)
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
            return a8h1;
        }

        private static ulong ShiftA1H8(SquareEnum square, ulong a1h8)
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

        internal static ulong GetBishopMovesMapFor(IPositionImpl position, SquareEnum square)
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

        #endregion

        #region CreateKnightMovesDictionary

        private static void CreateKnightMovesDictionary()
        {
            foreach (SquareEnum square in Enum.GetValues(typeof(SquareEnum)))
            {
                ulong cpMap = knightC6Map;
                if (square.File() < 2)
                    cpMap = cpMap.BitOffForFile(0);
                if (square.File() < 1)
                    cpMap = cpMap.BitOffForFile(1);
                if (square.File() > 5)
                    cpMap = cpMap.BitOffForFile(4);
                if (square.File() > 6)
                    cpMap = cpMap.BitOffForFile(3);
                if (square.Row() < 2)
                    cpMap = cpMap.BitOffForRow(0);
                if (square.Row() < 1)
                    cpMap = cpMap.BitOffForRow(1);
                if (square.Row() > 5)
                    cpMap = cpMap.BitOffForRow(4);
                if (square.Row() > 6)
                    cpMap = cpMap.BitOffForRow(3);

                var shift = ((int)square - (int)SquareEnum.c6);
                if (shift < 0)
                    cpMap <<= (shift * -1);
                else
                    cpMap >>= shift;

                KnightMovesDictionary[square] = cpMap;
            }
        }

        #endregion
    }
}
