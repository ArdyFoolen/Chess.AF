using AF.Functional;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;
using static Chess.AF.PositionBridge.PositionImpl.PositionFactory;

namespace Chess.AF.PositionBridge
{
    [DataContract]
    public partial class PositionAbstraction
    {
        #region Properties

        private IPositionImpl PositionImpl { get; set; }

        [DataMember]
        public bool IsWhiteToMove { get; private set; }
        [DataMember]
        public RokadeEnum WhiteRokade { get; private set; }
        [DataMember]
        public RokadeEnum BlackRokade { get; private set; }
        [DataMember]
        public Option<SquareEnum> EpSquare { get; private set; }

        [DataMember]
        public bool IsTake { get => PositionImpl.IsTake; }
        [DataMember]
        public int PlyCount { get; private set; }

        [DataMember]
        public int MoveNumber { get; private set; }

        #endregion

        #region static methods

        public static Option<PositionAbstraction> Of(Option<Fen> fen)
            => fen.Bind(WhenValid);

        private static Option<PositionAbstraction> WhenValid(Fen fen)
            => Some(new PositionAbstraction(Create(fen), fen.IsWhiteToMove, fen.WhiteRokade, fen.BlackRokade, fen.EnPassant,
                fen.PlyCount, fen.MoveNumber));

        #endregion

        #region ctors

        public PositionAbstraction(IPositionImpl positionImpl, bool isWhiteToMove, RokadeEnum whiteRokade, RokadeEnum blackRokade,
            Option<SquareEnum> ep, int plyCount, int moveNumber)
        {
            this.PositionImpl = positionImpl;
            this.IsWhiteToMove = isWhiteToMove;
            this.WhiteRokade = whiteRokade;
            this.BlackRokade = blackRokade;
            this.EpSquare = ep;
            this.PlyCount = plyCount;
            this.MoveNumber = moveNumber;

            this.PositionImpl.SetRokade(() => IsWhiteToMove ? this.WhiteRokade : this.BlackRokade);
            this.PositionImpl.SetEpSquare(() => EpSquare);
        }

        private PositionAbstraction(PositionAbstraction position)
        {
            this.PositionImpl = position.PositionImpl.CreateCopy();
            this.IsWhiteToMove = position.IsWhiteToMove;
            this.WhiteRokade = position.WhiteRokade;
            this.BlackRokade = position.BlackRokade;
            this.EpSquare = position.EpSquare;
            this.PlyCount = position.PlyCount;
            this.MoveNumber = position.MoveNumber;
        }

        #endregion

        #region Move

        public Option<PositionAbstraction> Move(Move move)
            => RokadeEnum.None.Equals(move.Rokade) ?
                    MoveAndValidateFromTo(move) :
                    MoveAndValidateRokade(move);

        private Option<PositionAbstraction> MoveAndValidateRokade(Move move)
        {
            if (RokadeEnum.None.Equals(move.Rokade) || RokadeEnum.KingAndQueenSide.Equals(move.Rokade) ||
                RokadeEnum.None.Equals(PositionImpl.PossibleRokade(IsWhiteToMove) | move.Rokade) || !PieceEnum.King.Equals(move.Piece))
                return None;
            var om = AF.Move.Of(move.Piece, PositionImpl.KingSquare(IsWhiteToMove), move.Rokade.GetKingRokadeSquare(IsWhiteToMove), PieceEnum.King);
            return om.Bind(m => MoveAndValidateFromTo(m));
        }

        private Option<PositionAbstraction> MoveAndValidateFromTo(Move move)
            => ValidateMoveFromTo(move).Map(p => p.MoveFromTo(move));

        private Option<PositionAbstraction> ValidateMoveFromTo(Move move)
            => IterateForAllMoves().Any(a => move.Piece.Equals(a.Piece) && move.From.Equals(a.Square) &&
            move.Promote.Equals(a.Promoted) && move.To.Equals(a.MoveSquare)) ? Some(new PositionAbstraction(this)) : None;

        private PositionAbstraction Move((PieceEnum Piece, SquareEnum From, SquareEnum To, PieceEnum Promote) move)
            => AF.Move.Of(move.Piece, move.From, move.To, move.Promote).Match(
                None: () => this,
                Some: m => MoveFromTo(m));

        private PositionAbstraction MoveFromTo(Move move)
        {
            PositionImpl.SetBits(move, IsWhiteToMove);

            SetEpSquare(move);

            UpdateRokadePossibilities(move.Piece, move.From);
            IsWhiteToMove = !IsWhiteToMove;

            UpdatePlyCount(move.Piece);

            if (IsWhiteToMove)
                MoveNumber += 1;

            return this;
        }

        private void SetEpSquare(Move move)
        {
            EpSquare = None;
            if (PieceEnum.Pawn.Equals(move.Piece) && Math.Abs((int)move.To - (int)move.From) == 16)
            {
                int ep = (int)move.To;
                if (IsWhiteToMove)
                    ep += 8;
                else
                    ep -= 8;
                EpSquare = Some((SquareEnum)ep);
            }
        }

        private void UpdatePlyCount(PieceEnum piece)
        {
            if (PieceEnum.Pawn.Equals(piece) || IsTake)
                PlyCount = 0;
            else
                PlyCount += 1;
        }

        #endregion

        #region Iterators

        public PiecesIterator<PieceEnum> GetIteratorFor(PieceEnum piece)
            => PositionImpl.GetIteratorFor(piece, IsWhiteToMove);

        public PiecesIterator<T> GetIteratorForAll<T>()
            where T : Enum
            => PositionImpl.GetIteratorForAll<T>(IsWhiteToMove);

        public IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> IterateForAllMoves()
        {
            foreach (var pieceTuple in GetIteratorForAll<PieceEnum>().Iterate())
                foreach (var move in MovesFactory.Create(pieceTuple.Piece, pieceTuple.Square, PositionImpl, IsWhiteToMove))
                    if (!(new PositionAbstraction(this)).Move((pieceTuple.Piece, pieceTuple.Square, move.Square, move.Piece)).OpponentIsInCheck)
                        yield return (pieceTuple.Piece, pieceTuple.Square, move.Piece, move.Square);
        }

        #endregion

        #region IsInCheck

        public bool IsInCheck { get => PositionImpl.IsInCheck(IsWhiteToMove); }

        public bool OpponentIsInCheck { get => PositionImpl.OpponentIsInCheck(IsWhiteToMove); }

        #endregion

        #region IsMate

        public bool IsMate
        {
            get
            {
                if (PositionImpl.IsInCheck(IsWhiteToMove))
                    return IterateForAllMoves().Count() == 0;
                return false;
            }
        }

        public bool OpponentIsMate
        {
            get
            {
                PositionAbstraction position = new PositionAbstraction(this);
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
                if (!PositionImpl.IsInCheck(IsWhiteToMove))
                    return IterateForAllMoves().Count() == 0;
                return false;
            }
        }

        public bool OpponentIsStaleMate
        {
            get
            {
                PositionAbstraction position = new PositionAbstraction(this);
                position.IsWhiteToMove = !IsWhiteToMove;
                return position.IsStaleMate;
            }
        }

        #endregion

        #region Result

        private GameResult result = GameResult.Ongoing;
        public GameResult Result
        {
            get
            {
                if (IsStaleMate)
                    return GameResult.Draw;
                if (IsWhiteToMove && IsMate)
                    return GameResult.BlackWins;
                if (!IsWhiteToMove && IsMate)
                    return GameResult.WhiteWins;
                return this.result;
            }
        }

        #endregion

        #region Resign and Draw

        public Option<PositionAbstraction> Resign()
            => (GameResult.Ongoing.Equals(result)) ? Some(new PositionAbstraction(this).resign()) : None;

        public Option<PositionAbstraction> Draw()
            => (GameResult.Ongoing.Equals(result)) ? Some(new PositionAbstraction(this).draw()) : None;

        private PositionAbstraction resign()
        {
            if (IsWhiteToMove)
                result = GameResult.BlackWins;
            else
                result = GameResult.WhiteWins;
            return this;
        }

        private PositionAbstraction draw()
        {
            result = GameResult.Draw;
            return this;
        }

        #endregion

        #region Rokade

        private void UpdateRokadePossibilities(PieceEnum piece, SquareEnum from)
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

        public Dictionary<int, (PiecesEnum Piece, SquareEnum Square, bool IsSelected)> ToDictionary()
        {
            Dictionary<int, (PiecesEnum Piece, SquareEnum Square, bool IsSelected)> dict = new Dictionary<int, (PiecesEnum Piece, SquareEnum Square, bool IsSelected)>();
            PiecesIterator<PiecesEnum> iterator = this.GetIteratorForAll<PiecesEnum>();
            foreach (var square in iterator.Iterate().OrderBy(o => o.Square))
            {
                dict.Add((int)square.Square, square);
            }
            return dict;
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
