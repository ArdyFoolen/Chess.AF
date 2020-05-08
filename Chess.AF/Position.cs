using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Functional;
using static AF.Functional.F;
using static Chess.AF.Position.PositionFactory;

namespace Chess.AF
{
    internal enum PositionEnum
    {
        BlackPieces,    // 0
        BlackPawns,     // 1
        BlackKnights,   // 2
        BlackBishops,   // 3
        BlackRooks,     // 4
        BlackQueens,    // 5
        BlackKing,      // 6
        WhitePieces,    // 7
        WhitePawns,     // 8
        WhiteKnights,   // 9
        WhiteBishops,   // 10
        WhiteRooks,     // 11
        WhiteQueens,    // 12
        WhiteKing       // 13
    }


    public enum PiecesEnum
    {
        BlackPawn = 1,
        BlackKnight,
        BlackBishop,
        BlackRook,
        BlackQueen,
        BlackKing,
        WhitePawn = 8,
        WhiteKnight,
        WhiteBishop,
        WhiteRook,
        WhiteQueen,
        WhiteKing
    }

    public enum PieceEnum
    {
        Pawn = 1,      // 1 8
        Knight,        // 2 9
        Bishop,        // 3 10
        Rook,          // 4 11
        Queen,         // 5 12
        King            // 6 13
    }

    internal enum BlackPiecesEnum
    {
        BlackPawns = 1,
        BlackKnights,
        BlackBishops,
        BlackRooks,
        BlackQueens,
        BlackKing
    }

    internal enum WhitePiecesEnum
    {
        WhitePawns = 8,
        WhiteKnights,
        WhiteBishops,
        WhiteRooks,
        WhiteQueens,
        WhiteKing
    }

    public enum RokadeEnum : byte
    {
        None,
        KingSide,
        QueenSide,
        KingAndQueenSide
    }

    public enum SquareEnum
    {
        a8, b8, c8, d8, e8, f8, g8, h8, // c6 5088008850000000 a6 4020002040000000 g1 0000000000050800 f6 0a1100110a000000 h6 0204000402000000
        a7, b7, c7, d7, e7, f7, g7, h7, // c7 8800885000000000 a7 2000204000000000 g2 0000000005080008 f7 1100110a00000000 h7 0400040200000000
        a6, b6, c6, d6, e6, f6, g6, h6, // c8 0088500000000000 a8 0020400000000000 g3 0000000508000805 f8 00110a0000000000 h8 0004020000000000
        a5, b5, c5, d5, e5, f5, g5, h5, // b6 a0100010a0000000 f1 00000000000a1100 h1 0000000000020400 g6 0508000805000000
        a4, b4, c4, d4, e4, f4, g4, h4, // b7 100010a000000000 f2 0000000006110011 h2 0000000002040004 g7 0800080500000000
        a3, b3, c3, d3, e3, f3, g3, h3, // b8 0010a00000000000 f3 0000000611001106 h3 0000000204000402 g8 0008050000000000
        a2, b2, c2, d2, e2, f2, g2, h2,
        a1, b1, c1, d1, e1, f1, g1, h1
    }

    public partial class Position
    {
        private ulong[] Maps = new ulong[14];
        public bool IsWhiteToMove { get; }
        public RokadeEnum WhiteRokade { get; }
        public RokadeEnum BlackRokade { get; }
        public Option<SquareEnum> EpSquare { get; }

        private Position(ulong[] maps, bool isWhiteToMove, RokadeEnum whiteRokade, RokadeEnum blackRokade, Option<SquareEnum> ep)
        {
            this.Maps = maps;
            this.IsWhiteToMove = isWhiteToMove;
            this.WhiteRokade = whiteRokade;
            this.BlackRokade = blackRokade;
            this.EpSquare = ep;
        }

        public static Option<Position> Of(Option<Fen> fen)
            => fen.Bind(WhenValid);

        private static Option<Position> WhenValid(Fen fen)
            => Some(Create(fen));

        public PiecesIterator<PieceEnum> GetIteratorFor(PieceEnum piece)
        {
            return new PiecesIterator<PieceEnum>((piece, GetMapFor(piece)));
        }

        public PiecesIterator<T> GetIteratorForAll<T>()
            where T : Enum
        {
            return new PiecesIterator<T>(
                Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(piece => (piece, GetMapFor(piece)))
                .ToArray());
        }

        public IEnumerable<(PieceEnum Piece, SquareEnum Square, SquareEnum MoveSquare)> IterateForAllMoves()
        {
            foreach (var pieceTuple in GetIteratorForAll<PieceEnum>().Iterate())
                foreach (var move in MovesFactory.Create(pieceTuple.Piece, pieceTuple.Square, Some(this)))
                    yield return (pieceTuple.Piece, pieceTuple.Square, move);
        }

        private ulong GetMapFor<T>(T piece)
            where T : Enum
        {
            if (piece is PieceEnum)
                return GetMapFor((PieceEnum)(object)piece);
            if (piece is PiecesEnum)
                return GetMapFor((PiecesEnum)(object)piece);
            throw new ArgumentException();
        }

        private ulong GetMapFor(PieceEnum piece)
        {
            if (IsWhiteToMove)
                return Maps[(int)piece + 7];
            return Maps[(int)piece];
        }

        private ulong GetMapFor(PiecesEnum piece)
        {
            return Maps[(int)piece];
        }

        internal ulong GetMapFor(PositionEnum position)
            => Maps[(int)position];

        public ulong ExcludeOwnPieces(ulong map)
            => IsWhiteToMove ? map & ~Maps[(int)PositionEnum.WhitePieces] : map & ~Maps[(int)PositionEnum.BlackPieces];

        internal ulong GetPawnMapFor(SquareEnum square, ulong mvMap, ulong tkMap)
        {
            if (IsWhiteToMove)
                return GetWhitePawnMapFor(square, mvMap, tkMap);
            else
                return GetBlackPawnMapFor(square, mvMap, tkMap);
        }

        private ulong GetWhitePawnMapFor(SquareEnum square, ulong mvMap, ulong tkMap)
        {
            mvMap &= square.DownBitsOff();
            tkMap &= square.DownBitsOff();

            bool succesWhite = TryGetLowestSquare(Maps[(int)PositionEnum.WhitePieces] & mvMap, out SquareEnum whitePiece);
            bool succesBlack = TryGetLowestSquare(Maps[(int)PositionEnum.BlackPieces] & mvMap, out SquareEnum blackPiece);

            if (succesWhite || succesBlack)
                if (succesWhite && !succesBlack || succesWhite && succesBlack && whitePiece > blackPiece)
                    mvMap &= whitePiece.UpBitsOff(1);
                else
                    mvMap &= blackPiece.UpBitsOff(1);

            tkMap &= Maps[(int)PositionEnum.BlackPieces];

            tkMap |= GetEPMap(square);

            return mvMap | tkMap;
        }

        private ulong GetEPMap(SquareEnum square)
            => EpSquare.Match(
                None: () => 0x0ul,
                Some: s => CanTakeEP(s, square) ? 0x0ul.SetBit((int)s) : 0x0ul
                );

        private bool CanTakeEP(SquareEnum epSquare, SquareEnum square)
        {
            if (IsWhiteToMove && square.Row() == 3 ||
                !IsWhiteToMove && square.Row() == 4)
                if (Math.Abs(square.File() - epSquare.File()) == 1)
                    return true;
            return false;
        }

        private ulong GetBlackPawnMapFor(SquareEnum square, ulong mvMap, ulong tkMap)
        {
            mvMap &= square.UpBitsOff();
            tkMap &= square.UpBitsOff();

            bool succesWhite = TryGetHighestSquare(Maps[(int)PositionEnum.WhitePieces] & mvMap, out SquareEnum whitePiece);
            bool succesBlack = TryGetHighestSquare(Maps[(int)PositionEnum.BlackPieces] & mvMap, out SquareEnum blackPiece);

            if (succesWhite || succesBlack)
                if (succesWhite && !succesBlack || succesWhite && succesBlack && whitePiece < blackPiece)
                    mvMap &= whitePiece.DownBitsOff(1);
                else
                    mvMap &= blackPiece.DownBitsOff(1);

            tkMap &= Maps[(int)PositionEnum.WhitePieces];

            tkMap |= GetEPMap(square);

            return mvMap | tkMap;
        }

        public ulong GetMinMap(ulong map)
        {
            bool succesWhite = TryGetLowestSquare(Maps[(int)PositionEnum.WhitePieces] & map, out SquareEnum whitePiece);
            bool succesBlack = TryGetLowestSquare(Maps[(int)PositionEnum.BlackPieces] & map, out SquareEnum blackPiece);

            if (!succesWhite && !succesBlack)
                return map;

            if (succesWhite && !succesBlack || succesWhite && succesBlack && whitePiece > blackPiece)
                return whitePiece.UpBitsOff(IsWhiteToMove ? 1 : 0) & map;
            else
                return blackPiece.UpBitsOff(IsWhiteToMove ? 0 : 1) & map;
        }

        public ulong GetMaxMap(ulong map)
        {
            bool succesWhite = TryGetHighestSquare(Maps[(int)PositionEnum.WhitePieces] & map, out SquareEnum whitePiece);
            bool succesBlack = TryGetHighestSquare(Maps[(int)PositionEnum.BlackPieces] & map, out SquareEnum blackPiece);

            if (!succesWhite && !succesBlack)
                return map;

            if (succesWhite && !succesBlack || succesWhite && succesBlack && whitePiece < blackPiece)
                return whitePiece.DownBitsOff(IsWhiteToMove ? 1 : 0) & map;
            else
                return blackPiece.DownBitsOff(IsWhiteToMove ? 0 : 1) & map;
        }

        public bool TryGetLowestSquare(ulong map, out SquareEnum piece)
        {
            int i = 63;
            ulong test = 0x0000000000000001;
            while ((test & map) == 0 && i-- > 0)
                test <<= 1;
            piece = (SquareEnum)i;
            return !((test & map) == 0);
        }

        public bool TryGetHighestSquare(ulong map, out SquareEnum piece)
        {
            int i = 0;
            ulong test = 0x8000000000000000;
            while ((test & map) == 0 && i++ < 63)
                test >>= 1;
            piece = (SquareEnum)i;
            return !((test & map) == 0);
        }
    }
}
