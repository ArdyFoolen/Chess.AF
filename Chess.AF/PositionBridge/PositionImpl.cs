using AF.Functional.Option;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.PositionBridge.PositionAbstraction;
using static AF.Functional.F;
using AF.Functional;
using Chess.AF.Dto;

namespace Chess.AF.PositionBridge
{
    internal partial class PositionImpl : IPositionImpl
    {
        #region Properties

        private IPositionAbstraction Abstraction { get; set; }

        private ulong[] Maps = new ulong[14];

        public bool IsTake { get; private set; } = false;

        #endregion

        #region ctors

        public PositionImpl(IPositionAbstraction abstraction, ulong[] maps)
        {
            Abstraction = abstraction;
            Maps = maps;
        }

        private PositionImpl(PositionImpl position, IPositionAbstraction abstraction)
        {
            Abstraction = abstraction;
            for (int i = 0; i < position.Maps.Length; i++)
                this.Maps[i] = position.Maps[i];
        }

        #endregion

        #region public methods

        private bool IsWhiteToMove { get => Abstraction.IsWhiteToMove; }

        public SquareEnum KingSquare { get => this.Maps[(int)PieceEnum.King.ToPieces(IsWhiteToMove)].GetSquareFrom(); }

        #region SetBits

        public IPositionImpl SetBits(Move move, IPositionAbstraction abstraction)
        {
            PositionImpl implementor = new PositionImpl(this, abstraction);
            implementor.SetBitsForRokadeRook(move);
            implementor.SetBitsForMove(move);
            implementor.SetBitsForEnPassantTake(move);
            implementor.SetBitsForTake(move);
            return implementor;
        }

        private void SetBitsForRokadeRook(Move move)
        {
            bool isRokade = move.Piece.IsRokadeMove(move.From, move.To);
            if (isRokade)
            {
                var rookRokadeSquares = move.To.GetRookRokadeSquares(IsWhiteToMove);
                var rook = (int)PieceEnum.Rook.ToPieces(IsWhiteToMove);

                Maps[rook] = Maps[rook].SetBitOff((int)rookRokadeSquares.From);
                Maps[rook] = Maps[rook].SetBit((int)rookRokadeSquares.To);

                int piecesIndex = GetIndexAllPiecesFor(IsWhiteToMove);
                Maps[piecesIndex] = Maps[piecesIndex].SetBitOff((int)rookRokadeSquares.From);
                Maps[piecesIndex] = Maps[piecesIndex].SetBit((int)rookRokadeSquares.To);
            }
        }

        private void SetBitsForMove(Move move)
        {
            var pieces = move.Piece.ToPieces(IsWhiteToMove);
            var promoted = move.Promote.ToPieces(IsWhiteToMove);

            Maps[(int)pieces] = Maps[(int)pieces].SetBitOff((int)move.From);
            if (move.Piece == move.Promote)
                Maps[(int)pieces] = Maps[(int)pieces].SetBit((int)move.To);
            else
                Maps[(int)promoted] = Maps[(int)promoted].SetBit((int)move.To);

            int piecesIndex = GetIndexAllPiecesFor(IsWhiteToMove);
            Maps[piecesIndex] = Maps[piecesIndex].SetBitOff((int)move.From);
            Maps[piecesIndex] = Maps[piecesIndex].SetBit((int)move.To);
        }

        private void SetBitsForEnPassantTake(Move move)
        {
            int oppositeColoredIndex = GetOppositeColoredIndexAllPiecesFor(IsWhiteToMove);

            if (IsEpTake(oppositeColoredIndex, move))
            {
                IsTake = true;
                int epIndex = (int)(IsWhiteToMove ? (int)move.To + 8 : (int)move.To - 8);
                Maps[oppositeColoredIndex] = Maps[oppositeColoredIndex].SetBitOff(epIndex);
                Maps[oppositeColoredIndex + 1] = Maps[oppositeColoredIndex + 1].SetBitOff(epIndex);
            }
        }

        private void SetBitsForTake(Move move)
        {
            int oppositeColoredIndex = GetOppositeColoredIndexAllPiecesFor(IsWhiteToMove);
            if (Maps[oppositeColoredIndex].IsBitOn((int)move.To))
            {
                IsTake = true;
                foreach (var i in Enumerable.Range(oppositeColoredIndex, 7))
                    Maps[i] = Maps[i].SetBitOff((int)move.To);
            }
        }

        #endregion

        public PiecesIterator<PieceEnum> GetIteratorFor(PieceEnum piece)
            => new PiecesIterator<PieceEnum>(new PieceMap<PieceEnum>(piece, GetMapFor(piece)));

        public PiecesIterator<T> GetIteratorForAll<T>()
            where T : Enum
            => new PiecesIterator<T>(
                Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(piece => new PieceMap<T>(piece, GetMapFor(piece)))
                .ToArray());

        public ulong ExcludeOwnPieces(ulong map)
            => IsWhiteToMove ? map & ~Maps[(int)PositionEnum.WhitePieces] : map & ~Maps[(int)PositionEnum.BlackPieces];

        public ulong IncludeRokade(ulong map)
        {
            RokadeEnum rokade = PossibleRokade();
            if (RokadeEnum.KingAndQueenSide.Equals(rokade))
                return map | (IsWhiteToMove ? 0x0000000000000022ul : 0x2200000000000000ul);
            if (RokadeEnum.KingSide.Equals(rokade))
                return map | (IsWhiteToMove ? 0x0000000000000002ul : 0x0200000000000000ul);
            if (RokadeEnum.QueenSide.Equals(rokade))
                return map | (IsWhiteToMove ? 0x0000000000000020ul : 0x2000000000000000ul);
            return map;
        }

        public ulong ExcludeOpponentKing(ulong map)
        {
            PiecesEnum king = PieceEnum.King.ToPieces(!IsWhiteToMove);
            ulong kingMap = Maps[(int)king];

            if (kingMap == 0x0ul)
                return map;

            SquareEnum kingSquare = kingMap.GetSquareFrom();

            return ~MovesDictionaries.KingMovesDictionary[kingSquare] & map;
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

        public ulong GetPawnMapFor(SquareEnum square, ulong mvMap, ulong tkMap)
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
            => Abstraction.EpSquare.Match(
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

        public RokadeEnum PossibleRokade()
        {
            RokadeEnum rokade = Abstraction.Rokade;
            if (RokadeEnum.None.Equals(rokade) || IsInCheck)
                return RokadeEnum.None;

            RokadeEnum queenSide = RokadeEnum.None;
            if (RokadeEnum.KingAndQueenSide.Equals(rokade) || RokadeEnum.QueenSide.Equals(rokade))
                if (!QueenSideRokadeSquaresOccupied(IsWhiteToMove) && !QueenSideRokadeSquares(IsWhiteToMove).Any(a => IsSquareAttacked(a)))
                    queenSide = RokadeEnum.QueenSide;

            RokadeEnum kingSide = RokadeEnum.None;
            if (RokadeEnum.KingAndQueenSide.Equals(rokade) || RokadeEnum.KingSide.Equals(rokade))
                if (!KingSideRokadeSquaresOccupied(IsWhiteToMove) && !KingSideRokadeSquares(IsWhiteToMove).Any(a => IsSquareAttacked(a)))
                    kingSide = RokadeEnum.KingSide;

            return queenSide | kingSide;
        }

        #endregion

        #region private methods

        private int GetIndexAllPiecesFor(bool isWhiteToMove)
            => (int)(isWhiteToMove ? PositionEnum.WhitePieces : PositionEnum.BlackPieces);

        private int GetOppositeColoredIndexAllPiecesFor(bool isWhiteToMove)
            => GetIndexAllPiecesFor(!isWhiteToMove);

        private bool IsEpTake(int index, Move move)
            => PieceEnum.Pawn == move.Piece && move.From.File() != move.To.File() && !Maps[index].IsBitOn((int)move.To);

        private ulong GetMapFor(PieceEnum piece)
        {
            if (IsWhiteToMove)
                return Maps[(int)piece + 7];
            return Maps[(int)piece];
        }

        private ulong GetMapFor(PiecesEnum piece)
            => Maps[(int)piece];

        private ulong GetMapFor<T>(T piece)
            where T : Enum
        {
            if (piece is PieceEnum)
                return GetMapFor((PieceEnum)(object)piece);
            if (piece is PiecesEnum)
                return GetMapFor((PiecesEnum)(object)piece);
            throw new ArgumentException();
        }

        private IEnumerable<SquareEnum> QueenSideRokadeSquares(bool isWhiteToMove)
        {
            yield return isWhiteToMove ? SquareEnum.d1 : SquareEnum.d8;
            yield return isWhiteToMove ? SquareEnum.c1 : SquareEnum.c8;
        }

        private IEnumerable<SquareEnum> KingSideRokadeSquares(bool isWhiteToMove)
        {
            yield return isWhiteToMove ? SquareEnum.f1 : SquareEnum.f8;
            yield return isWhiteToMove ? SquareEnum.g1 : SquareEnum.g8;
        }

        private bool QueenSideRokadeSquaresOccupied(bool isWhiteToMove)
        {
            ulong map = isWhiteToMove ? 0x0000000000000030ul : 0x3000000000000000ul;
            return (Maps[(int)(isWhiteToMove ? PositionEnum.WhitePieces : PositionEnum.BlackPieces)] & map) != 0x0ul;
        }

        private bool KingSideRokadeSquaresOccupied(bool isWhiteToMove)
        {
            ulong map = isWhiteToMove ? 0x0000000000000006ul : 0x0600000000000000ul;
            return (Maps[(int)(isWhiteToMove ? PositionEnum.WhitePieces : PositionEnum.BlackPieces)] & map) != 0x0ul;
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
    }
}
