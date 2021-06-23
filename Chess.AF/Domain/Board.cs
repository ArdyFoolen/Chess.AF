using AF.Functional;
using Chess.AF.Dto;
using Chess.AF.Enums;
using Chess.AF.ImportExport;
using Chess.AF.PieceMoves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;
using static Chess.AF.Domain.BoardMap.BoardFactory;

namespace Chess.AF.Domain
{
    [DataContract]
    public partial class Board : IBoard
    {
        #region static methods

        public static Option<IBoard> Of(Option<Fen> fen)
            => fen.Bind(WhenValid);

        private static Option<IBoard> WhenValid(Fen fen)
            => Some((IBoard)new Board(fen));

        #endregion

        #region Properties

        private IBoardMap Implementor { get; set; }

        [DataMember]
        public bool IsWhiteToMove { get; private set; }
        [DataMember]
        public RokadeEnum WhiteRokade { get; private set; }
        [DataMember]
        public RokadeEnum BlackRokade { get; private set; }
        [DataMember]
        public Option<SquareEnum> EpSquare { get; private set; }

        [DataMember]
        public bool IsTake { get => Implementor.IsTake; }
        [DataMember]
        public int PlyCount { get; private set; }

        [DataMember]
        public int MoveNumber { get; private set; }

        [DataMember]
        public Move LastMove { get; private set; }

        #endregion

        #region ctors

        private Board(Fen fen)
        {
            Implementor = Create(fen, this);
            this.IsWhiteToMove = fen.IsWhiteToMove;
            this.WhiteRokade = fen.WhiteRokade;
            this.BlackRokade = fen.BlackRokade;
            this.EpSquare = fen.EnPassant;
            this.PlyCount = fen.PlyCount;
            this.MoveNumber = fen.MoveNumber;
        }

        private Board(Board board, IBoardMap implementor)
        {
            this.Implementor = implementor;
            this.IsWhiteToMove = board.IsWhiteToMove;
            this.WhiteRokade = board.WhiteRokade;
            this.BlackRokade = board.BlackRokade;
            this.EpSquare = board.EpSquare;
            this.PlyCount = board.PlyCount;
            this.MoveNumber = board.MoveNumber;
        }

        private IBoard CreateCopy()
            => new Board(this, Implementor);

        #endregion

        #region Move

        public Option<IBoard> Move(Move move)
            => RokadeEnum.None.Equals(move.Rokade) ?
                    MoveAndValidateFromTo(move) :
                    MoveAndValidateRokade(move);

        private Option<IBoard> MoveAndValidateRokade(Move move)
        {
            if (RokadeEnum.None.Equals(move.Rokade) || RokadeEnum.KingAndQueenSide.Equals(move.Rokade) ||
                RokadeEnum.None.Equals(Implementor.PossibleRokade() | move.Rokade) || !PieceEnum.King.Equals(move.Piece))
                return None;
            var om = AF.Dto.Move.Of(move.Piece, Implementor.KingSquare, move.Rokade.GetKingRokadeSquare(IsWhiteToMove), PieceEnum.King);
            return om.Bind(m => MoveAndValidateFromTo(m));
        }

        private Option<IBoard> MoveAndValidateFromTo(Move move)
            => ValidateMoveFromTo(move).Map(p => ((Board)p).MoveFromTo(move));

        private Option<IBoard> ValidateMoveFromTo(Move move)
            => IterateForAllMoves().Any(a => move.Piece.Equals(a.Piece) && move.From.Equals(a.Square) &&
            move.Promote.Equals(a.Promoted) && move.To.Equals(a.MoveSquare)) ? Some(CreateCopy()) : None;

        private IBoard Move((PieceEnum Piece, SquareEnum From, SquareEnum To, PieceEnum Promote) move)
            => AF.Dto.Move.Of(move.Piece, move.From, move.To, move.Promote).Match(
                None: () => this,
                Some: m => MoveFromTo(m));

        private IBoard MoveFromTo(Move move)
        {
            LastMove = move;
            Implementor = Implementor.SetBits(move, this);

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
            => Implementor.GetIteratorFor(piece);

        public PiecesIterator<T> GetIteratorForAll<T>()
            where T : Enum
            => Implementor.GetIteratorForAll<T>();

        public IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> IterateForAllMoves()
        {
            foreach (var pieceTuple in GetIteratorForAll<PieceEnum>())
                foreach (var move in MovesFactory.Create(pieceTuple.Piece, pieceTuple.Square, Implementor))
                {
                    var pos = (new Board(this, Implementor)).Move((pieceTuple.Piece, pieceTuple.Square, move.Square, move.Piece)) as Board;
                    pos.IsWhiteToMove = !pos.IsWhiteToMove;
                    if (!pos.IsInCheck)
                        yield return (pieceTuple.Piece, pieceTuple.Square, move.Piece, move.Square);
                }
        }

        #endregion

        #region IsInCheck

        public bool IsInCheck { get => Implementor.IsInCheck; }

        #endregion

        #region IsMate

        public bool IsMate
        {
            get
            {
                if (Implementor.IsInCheck)
                    return IterateForAllMoves().Count() == 0;
                return false;
            }
        }

        #endregion

        #region IsStaleMate

        public bool IsStaleMate
        {
            get
            {
                if (!Implementor.IsInCheck)
                    return IterateForAllMoves().Count() == 0;
                return false;
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

        public Option<IBoard> Resign()
            => (GameResult.Ongoing.Equals(result)) ? Some((new Board(this, Implementor)).resign()) : None;

        public Option<IBoard> Draw()
            => (GameResult.Ongoing.Equals(result)) ? Some((new Board(this, Implementor)).draw()) : None;

        private IBoard resign()
        {
            if (IsWhiteToMove)
                result = GameResult.BlackWins;
            else
                result = GameResult.WhiteWins;
            return this;
        }

        private IBoard draw()
        {
            result = GameResult.Draw;
            return this;
        }

        #endregion

        #region Rokade

        public RokadeEnum Rokade { get => IsWhiteToMove ? this.WhiteRokade : this.BlackRokade; }

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
            foreach (var square in iterator.OrderBy(o => o.Square))
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

        public Dictionary<int, PieceOnSquare<PiecesEnum>> ToDictionary()
        {
            Dictionary<int, PieceOnSquare<PiecesEnum>> dict = new Dictionary<int, PieceOnSquare<PiecesEnum>>();
            PiecesIterator<PiecesEnum> iterator = this.GetIteratorForAll<PiecesEnum>();
            foreach (var pieceOnSquare in iterator.OrderBy(o => o.Square))
            {
                dict.Add((int)pieceOnSquare.Square, pieceOnSquare);
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
