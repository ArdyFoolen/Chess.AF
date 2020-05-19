using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        internal ulong[] Maps = new ulong[14];
        public bool IsWhiteToMove { get; internal set; }
        public RokadeEnum WhiteRokade { get; internal set; }
        public RokadeEnum BlackRokade { get; internal set; }
        public Option<SquareEnum> EpSquare { get; internal set; }

        public bool IsTake { get; internal set; } = false;

        private Position(ulong[] maps, bool isWhiteToMove, RokadeEnum whiteRokade, RokadeEnum blackRokade, Option<SquareEnum> ep)
        {
            this.Maps = maps;
            this.IsWhiteToMove = isWhiteToMove;
            this.WhiteRokade = whiteRokade;
            this.BlackRokade = blackRokade;
            this.EpSquare = ep;
        }

        private Position(Position position)
        {
            for (int i = 0; i < position.Maps.Length; i++)
                this.Maps[i] = position.Maps[i];

            this.IsWhiteToMove = position.IsWhiteToMove;
            this.WhiteRokade = position.WhiteRokade;
            this.BlackRokade = position.BlackRokade;
            this.EpSquare = position.EpSquare;
        }

        public Option<Position> Move(PieceEnum Piece, RokadeEnum Rokade)
        {
            if (RokadeEnum.None.Equals(Rokade) || RokadeEnum.KingAndQueenSide.Equals(Rokade) || RokadeEnum.None.Equals(PossibleRokade | Rokade) || !PieceEnum.King.Equals(Piece))
                return None;
            return Move(Piece, KingSquare(), PieceEnum.King, GetKingRokadeSquare(Rokade));
        }

        public Option<Position> Move(PieceEnum Piece, SquareEnum From, PieceEnum Promoted, SquareEnum To)
        {
            if (!IsValidMove(Piece, From, Promoted, To))
                return None;
            return Some(Move((Piece, From, Promoted, To)));
        }

        private bool IsValidMove(PieceEnum Piece, SquareEnum From, PieceEnum Promoted, SquareEnum To)
            => IterateForAllMoves().Any(a => Piece.Equals(a.Piece) && From.Equals(a.Square) && Promoted.Equals(a.Promoted) && To.Equals(a.MoveSquare));

        private Position Move((PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare) moveTo)
        {
            Position position = new Position(this);
            var pieces = moveTo.Piece.ToPieces(position.IsWhiteToMove);
            var promoted = moveTo.Promoted.ToPieces(position.IsWhiteToMove);
            int piecesIndex = (int)(position.IsWhiteToMove ? PositionEnum.WhitePieces : PositionEnum.BlackPieces);

            bool isRokade = moveTo.Piece.IsRokadeMove(moveTo.Square, moveTo.MoveSquare);
            if (isRokade)
            {
                var rookRokadeSquares = GetRookRokadeSquares(moveTo.MoveSquare);
                var rook = PieceEnum.Rook.ToPieces(position.IsWhiteToMove);

                position.Maps[(int)rook] = position.Maps[(int)rook].SetBitOff((int)rookRokadeSquares.From);
                position.Maps[(int)rook] = position.Maps[(int)rook].SetBit((int)rookRokadeSquares.To);

                position.Maps[piecesIndex] = position.Maps[piecesIndex].SetBitOff((int)rookRokadeSquares.From);
                position.Maps[piecesIndex] = position.Maps[piecesIndex].SetBit((int)rookRokadeSquares.To);
            }

            position.Maps[(int)pieces] = position.Maps[(int)pieces].SetBitOff((int)moveTo.Square);
            if (moveTo.Piece == moveTo.Promoted)
                position.Maps[(int)pieces] = position.Maps[(int)pieces].SetBit((int)moveTo.MoveSquare);
            else
                position.Maps[(int)promoted] = position.Maps[(int)promoted].SetBit((int)moveTo.MoveSquare);

            position.Maps[piecesIndex] = position.Maps[piecesIndex].SetBitOff((int)moveTo.Square);
            position.Maps[piecesIndex] = position.Maps[piecesIndex].SetBit((int)moveTo.MoveSquare);

            int otherIndex = (int)(!position.IsWhiteToMove ? PositionEnum.WhitePieces : PositionEnum.BlackPieces);
            if (position.Maps[otherIndex].IsBitOn((int)moveTo.MoveSquare))
            {
                position.IsTake = true;
                foreach (var i in Enumerable.Range(otherIndex, 7))
                    position.Maps[i] = position.Maps[i].SetBitOff((int)moveTo.MoveSquare);
            }

            position.EpSquare = None;
            if (PieceEnum.Pawn.Equals(moveTo.Piece) && Math.Abs((int)moveTo.MoveSquare - (int)moveTo.Square) == 16)
            {
                int ep = (int)moveTo.MoveSquare;
                if (IsWhiteToMove)
                    ep += 8;
                else
                    ep -= 8;
                position.EpSquare = Some((SquareEnum)ep);
            }

            position.UpdateRokadePossibilities(moveTo.Piece, moveTo.Square);
            position.IsWhiteToMove = !position.IsWhiteToMove;

            return position;
        }

        internal void UpdateRokadePossibilities(PieceEnum piece, SquareEnum from)
        {
            if (!PieceEnum.King.Equals(piece) && !PieceEnum.Rook.Equals(piece))
                return;
            if (PieceEnum.King.Equals(piece))
                SetColorRokade(RokadeEnum.None);
            if (PieceEnum.Rook.Equals(piece))
            {
                if (from.File() == 0)
                    SetColorRokade(GetColorRokade() & RokadeEnum.KingSide);
                if (from.File() == 7)
                    SetColorRokade(GetColorRokade() & RokadeEnum.QueenSide);
            }
        }

        private RokadeEnum GetColorRokade()
            => IsWhiteToMove ? WhiteRokade : BlackRokade;

        private void SetColorRokade(RokadeEnum rokade)
        {
            if (IsWhiteToMove)
                SetWhiteRokade(rokade);
            else
                SetBlackRokade(rokade);
        }

        private void SetWhiteRokade(RokadeEnum rokade)
            => WhiteRokade = rokade;

        private void SetBlackRokade(RokadeEnum rokade)
            => BlackRokade = rokade;

        private SquareEnum KingSquare()
            => this.Maps[(int)PieceEnum.King.ToPieces(IsWhiteToMove)].GetSquareFrom();

        private SquareEnum GetKingRokadeSquare(RokadeEnum rokade)
        {
            if (RokadeEnum.KingSide.Equals(rokade))
                return IsWhiteToMove ? SquareEnum.g1 : SquareEnum.g8;
            else
                return IsWhiteToMove ? SquareEnum.c1 : SquareEnum.c8;
        }

        private (SquareEnum From, SquareEnum To) GetRookRokadeSquares(SquareEnum kingMoveSquare)
        {
            if (kingMoveSquare.File() == 6)
                return (IsWhiteToMove ? SquareEnum.h1 : SquareEnum.h8, (SquareEnum)((int)kingMoveSquare - 1));
            else
                return (IsWhiteToMove ? SquareEnum.a1 : SquareEnum.a8, (SquareEnum)((int)kingMoveSquare + 1));
        }

        #region IsInCheck

        public bool IsInCheck
        {
            get
            {
                PiecesEnum king = PieceEnum.King.ToPieces(IsWhiteToMove);
                ulong kingMap = Maps[(int)king];
                SquareEnum kingSquare = kingMap.GetSquareFrom();

                return IsSquareAttacked(kingSquare);
            }
        }

        public bool OpponentIsInCheck
        {
            get
            {
                Position position = new Position(this);
                position.IsWhiteToMove = !IsWhiteToMove;
                return position.IsInCheck;
            }
        }

        private bool IsSquareAttacked(SquareEnum square)
        {
            PiecesEnum opponentKnight = PieceEnum.Knight.ToPieces(!IsWhiteToMove);
            ulong opponentKnightMap = Maps[(int)opponentKnight];

            bool knightChecks = (MovesDictionaries.KnightMovesDictionary[square] & opponentKnightMap) != 0x0ul;

            PiecesEnum opponentBishop = PieceEnum.Bishop.ToPieces(!IsWhiteToMove);
            ulong opponentBishopMap = Maps[(int)opponentBishop];
            PiecesEnum opponentQueen = PieceEnum.Queen.ToPieces(!IsWhiteToMove);
            ulong opponentQueenMap = Maps[(int)opponentQueen];

            bool bishopChecks = (MovesDictionaries.GetBishopMovesMapFor(this, square) & opponentBishopMap | MovesDictionaries.GetBishopMovesMapFor(this, square) & opponentQueenMap) != 0x0ul;

            PiecesEnum opponentRook = PieceEnum.Rook.ToPieces(!IsWhiteToMove);
            ulong opponentRookMap = Maps[(int)opponentRook];

            bool rookChecks = (MovesDictionaries.GetRookMovesMapFor(this, square) & opponentRookMap | MovesDictionaries.GetRookMovesMapFor(this, square) & opponentQueenMap) != 0x0ul;

            PiecesEnum opponentPawn = PieceEnum.Pawn.ToPieces(!IsWhiteToMove);
            ulong opponentPawnMap = Maps[(int)opponentPawn];

            ulong takeMap = MovesDictionaries.GetTakeMap(square);
            takeMap &= IsWhiteToMove ? square.DownBitsOff() : square.UpBitsOff();

            bool pawnChecks = (takeMap & opponentPawnMap) != 0x0ul;

            return knightChecks || bishopChecks || rookChecks || pawnChecks;
        }

        #endregion

        #region Rokade

        public RokadeEnum PossibleRokade
        {
            get
            {
                RokadeEnum rokade = IsWhiteToMove ? WhiteRokade : BlackRokade;
                if (RokadeEnum.None.Equals(rokade) || IsInCheck)
                    return RokadeEnum.None;

                RokadeEnum queenSide = RokadeEnum.None;
                if (RokadeEnum.KingAndQueenSide.Equals(rokade) || RokadeEnum.QueenSide.Equals(rokade))
                    if (!QueenSideRokadeSquaresOccupied && !QueenSideRokadeSquares.Any(a => IsSquareAttacked(a)))
                        queenSide = RokadeEnum.QueenSide;

                RokadeEnum kingSide = RokadeEnum.None;
                if (RokadeEnum.KingAndQueenSide.Equals(rokade) || RokadeEnum.KingSide.Equals(rokade))
                    if (!KingSideRokadeSquaresOccupied && !KingSideRokadeSquares.Any(a => IsSquareAttacked(a)))
                        kingSide = RokadeEnum.KingSide;

                return queenSide | kingSide;
            }
        }

        public RokadeEnum PossibleOpponentRokade
        {
            get
            {
                Position position = new Position(this);
                position.IsWhiteToMove = !IsWhiteToMove;
                return position.PossibleRokade;
            }
        }

        private IEnumerable<SquareEnum> QueenSideRokadeSquares
        {
            get
            {
                yield return IsWhiteToMove ? SquareEnum.d1 : SquareEnum.d8;
                yield return IsWhiteToMove ? SquareEnum.c1 : SquareEnum.c8;
            }
        }

        private IEnumerable<SquareEnum> KingSideRokadeSquares
        {
            get
            {
                yield return IsWhiteToMove ? SquareEnum.f1 : SquareEnum.f8;
                yield return IsWhiteToMove ? SquareEnum.g1 : SquareEnum.g8;
            }
        }

        private bool QueenSideRokadeSquaresOccupied
        {
            get
            {
                ulong map = IsWhiteToMove ? 0x0000000000000030ul : 0x3000000000000000ul;
                return (Maps[(int)(IsWhiteToMove ? PositionEnum.WhitePieces : PositionEnum.BlackPieces)] & map) != 0x0ul;
            }
        }

        private bool KingSideRokadeSquaresOccupied
        {
            get
            {
                ulong map = IsWhiteToMove ? 0x0000000000000006ul : 0x0600000000000000ul;
                return (Maps[(int)(IsWhiteToMove ? PositionEnum.WhitePieces : PositionEnum.BlackPieces)] & map) != 0x0ul;
            }
        }

        #endregion

        #region IsMate

        public bool IsMate
        {
            get
            {
                if (IsInCheck)
                    return IterateForAllMoves().Count() == 0;
                return false;
            }
        }

        public bool OpponentIsMate
        {
            get
            {
                Position position = new Position(this);
                position.IsWhiteToMove = !IsWhiteToMove;
                return position.IsMate;
            }
        }

        #endregion

        #region IsStaleMate

        public bool IsStaleMate
        {
            get
            {
                if (!IsInCheck)
                    return IterateForAllMoves().Count() == 0;
                return false;
            }
        }

        public bool OpponentIsStaleMate
        {
            get
            {
                Position position = new Position(this);
                position.IsWhiteToMove = !IsWhiteToMove;
                return position.IsStaleMate;
            }
        }

        #endregion

        public static Option<Position> Of(Option<Fen> fen)
            => fen.Bind(WhenValid);

        private static Option<Position> WhenValid(Fen fen)
            => Some(Create(fen));

        #region Iterators

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

        public IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> IterateForAllMoves()
        {
            foreach (var pieceTuple in GetIteratorForAll<PieceEnum>().Iterate())
                foreach (var move in MovesFactory.Create(pieceTuple.Piece, pieceTuple.Square, Some(this)))
                    if (!this.Move((pieceTuple.Piece, pieceTuple.Square, move.Piece, move.Square)).OpponentIsInCheck)
                        yield return (pieceTuple.Piece, pieceTuple.Square, move.Piece, move.Square);
        }

        #endregion

        #region Map methods

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

        public ulong ExcludeOpponentKing(ulong map)
        {
            PiecesEnum king = PieceEnum.King.ToPieces(!IsWhiteToMove);
            ulong kingMap = Maps[(int)king];

            if (kingMap == 0x0ul)
                return map;

            SquareEnum kingSquare = kingMap.GetSquareFrom();

            return ~MovesDictionaries.KingMovesDictionary[kingSquare] & map;
        }

        public ulong IncludeRokade(ulong map)
        {
            RokadeEnum rokade = PossibleRokade;
            if (RokadeEnum.KingAndQueenSide.Equals(rokade))
                return map | (IsWhiteToMove ? 0x0000000000000022ul : 0x2200000000000000ul);
            if (RokadeEnum.KingSide.Equals(rokade))
                return map | (IsWhiteToMove ? 0x0000000000000002ul : 0x0200000000000000ul);
            if (RokadeEnum.QueenSide.Equals(rokade))
                return map | (IsWhiteToMove ? 0x0000000000000020ul : 0x2000000000000000ul);
            return map;
        }

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

        private bool TryGetLowestSquare(ulong map, out SquareEnum piece)
        {
            int i = 63;
            ulong test = 0x0000000000000001;
            while ((test & map) == 0 && i-- > 0)
                test <<= 1;
            piece = (SquareEnum)i;
            return !((test & map) == 0);
        }

        private bool TryGetHighestSquare(ulong map, out SquareEnum piece)
        {
            int i = 0;
            ulong test = 0x8000000000000000;
            while ((test & map) == 0 && i++ < 63)
                test >>= 1;
            piece = (SquareEnum)i;
            return !((test & map) == 0);
        }

        #endregion

        #region ToStrings

        public string ToFenString()
        {
            PiecesIterator<PiecesEnum> iterator = this.GetIteratorForAll<PiecesEnum>();
            StringBuilder sb = new StringBuilder();
            int prevIndex = -1;
            int lastIndex = 0;
            foreach (var square in iterator.Iterate().OrderBy(o => o.Square))
            {
                lastIndex = EmptyFenSquares(sb, prevIndex, lastIndex, square.Square);

                sb.Append(Extensions.ConvertPieceToChar((int)square.Piece));
                prevIndex = (int)square.Square;
                lastIndex += 1;

                int remainFile = 8 - lastIndex % 8;
                if (remainFile == 8 && lastIndex < 63)
                    sb.Append("/");

            }

            lastIndex = EmptyFenSquares(sb, prevIndex, lastIndex);
            string fenString = $"{sb.ToString()} {Extensions.ConvertWhiteToMoveToChar(IsWhiteToMove)} {Extensions.ConvertRokadeToString(WhiteRokade, BlackRokade)} {Extensions.ConvertEPToString(EpSquare)} 0 1";

            return fenString;
        }

        private static int EmptyFenSquares(StringBuilder sb, int prevIndex, int lastIndex, SquareEnum? square = null)
        {
            int diff = 64 - prevIndex - 1;
            if (square.HasValue)
                diff = (int)square.Value - prevIndex - 1;
            int remainFile = 8 - lastIndex % 8;

            while (diff > remainFile)
            {
                if (lastIndex < 63)
                    sb.Append($"{remainFile}/");
                else
                    sb.Append($"{remainFile}");
                lastIndex += remainFile;
                diff -= remainFile;
                remainFile = 8 - lastIndex % 8;
            }
            if (diff > 0)
            {
                sb.Append($"{diff}");
                lastIndex += diff;
                if (diff == 8 && lastIndex < 63 || lastIndex % 8 == 0 && lastIndex < 63)
                    sb.Append("/");
            }

            return lastIndex;
        }

        #endregion
    }
}
