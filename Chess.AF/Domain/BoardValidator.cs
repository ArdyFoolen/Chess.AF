using Chess.AF.Dto;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.Domain.Board;

namespace Chess.AF.Domain
{
    public partial class BoardMap
    {
        internal class BoardValidator : IBoardValidator
        {
            #region Properties

            private Board board;
            private BoardMap boardMap;

            #endregion

            #region IBoardValidator

            public void SetBoard(IBoard board)
                => this.board = board as Board;

            public void SetBoardMap(IBoardMap boardMap)
                => this.boardMap = boardMap as BoardMap;

            public bool Validate()
                => ValidateKings() && ValidateRokade() && ValidateEpSquare() && ValidatePawns();

            #endregion

            #region ValidateKings

            private bool ValidateKings()
                => ValidateKing(PiecesEnum.BlackKing) && ValidateKing(PiecesEnum.WhiteKing);

            private bool ValidateKing(PiecesEnum piece)
                => GetIteratorFor(piece).ToArray().Length == 1;

            private PiecesIterator<PiecesEnum> GetIteratorFor(PiecesEnum piece)
            {
                var map = boardMap.Maps[(int)piece];
                var pieceMap = new PieceMap<PiecesEnum>(piece, map);
                return new PiecesIterator<PiecesEnum>(pieceMap);
            }

            #endregion

            #region ValidateRokade

            private bool ValidateRokade()
                => NoRokadePossible() || ValidateWhiteRokade() && ValidateBlackRokade();

            private bool ValidateWhiteRokade()
                => NoWhiteRokadePossible() || ValidateWhiteKingsideRokade() && ValidateWhiteQueensideRokade();

            private bool ValidateWhiteKingsideRokade()
                => !board.WhiteRokade.IsKingsideRokade() ||
                    board.WhiteRokade.IsKingsideRokade() && SquareEnum.e1.Equals(GetWhiteKingSquare()) && IsPieceOnSquare(PiecesEnum.WhiteRook, SquareEnum.h1);

            private bool ValidateWhiteQueensideRokade()
                => !board.WhiteRokade.IsQueensideRokade() ||
                    board.WhiteRokade.IsQueensideRokade() && SquareEnum.e1.Equals(GetWhiteKingSquare()) && IsPieceOnSquare(PiecesEnum.WhiteRook, SquareEnum.a1);

            private bool ValidateBlackRokade()
                => noBlackRokadePossible() || ValidateBlackKingsideRokade() && ValidateBlackQueensideRokade();

            private bool ValidateBlackKingsideRokade()
                => !board.BlackRokade.IsKingsideRokade() ||
                    board.BlackRokade.IsKingsideRokade() && SquareEnum.e8.Equals(GetBlackKingSquare()) && IsPieceOnSquare(PiecesEnum.BlackRook, SquareEnum.h8);

            private bool ValidateBlackQueensideRokade()
                => !board.BlackRokade.IsQueensideRokade() ||
                    board.BlackRokade.IsQueensideRokade() && SquareEnum.e8.Equals(GetBlackKingSquare()) && IsPieceOnSquare(PiecesEnum.BlackRook, SquareEnum.a8);

            private bool NoRokadePossible()
                => NoWhiteRokadePossible() && noBlackRokadePossible();

            private bool NoWhiteRokadePossible()
                => RokadeEnum.None.Equals(board.WhiteRokade);

            private bool noBlackRokadePossible()
                => RokadeEnum.None.Equals(board.BlackRokade);

            private SquareEnum GetWhiteKingSquare()
                => GetIteratorFor(PiecesEnum.WhiteKing).First().Square;

            private SquareEnum GetBlackKingSquare()
                => GetIteratorFor(PiecesEnum.BlackKing).First().Square;

            private bool IsPieceOnSquare(PiecesEnum piece, SquareEnum square)
                => GetIteratorFor(piece).Any(a => square.Equals(a.Square));

            #endregion

            #region ValidateEpSquare

            private bool ValidateEpSquare()
                => board.EpSquare.Match(
                    None: () => true,
                    Some: s => ValidateEpSquare(s)
                    );

            private bool ValidateEpSquare(SquareEnum square)
                => IsEpSquare(square) &&
                    ValidateWhiteToMoveEpSquare(square) &&
                    ValidateBlackToMoveEpSquare(square) &&
                    (ValidateWhitePawnOnSquare(square) || ValidateBlackPawnOnSquare(square));

            private bool ValidateWhitePawnOnSquare(SquareEnum square)
                => !IsWhiteEpSquare(square) ||
                    IsWhiteEpSquare(square) && IsPieceOnSquare(PiecesEnum.WhitePawn, GetWhitePawnSquare(square));

            private bool ValidateBlackPawnOnSquare(SquareEnum square)
                => !IsBlackEpSquare(square) ||
                    IsBlackEpSquare(square) && IsPieceOnSquare(PiecesEnum.BlackPawn, GetBlackPawnSquare(square));

            private bool ValidateWhiteToMoveEpSquare(SquareEnum square)
                => !(IsWhiteEpSquare(square) && board.IsWhiteToMove);

            private bool ValidateBlackToMoveEpSquare(SquareEnum square)
                => !(IsBlackEpSquare(square) && !board.IsWhiteToMove);

            private bool IsEpSquare(SquareEnum square)
                => IsWhiteEpSquare(square) || IsBlackEpSquare(square);

            private bool IsWhiteEpSquare(SquareEnum square)
                => square.Row() == 5;

            private bool IsBlackEpSquare(SquareEnum square)
                => square.Row() == 2;

            private SquareEnum GetWhitePawnSquare(SquareEnum square)
                => (SquareEnum)((int)square) - 8;

            private SquareEnum GetBlackPawnSquare(SquareEnum square)
                => (SquareEnum)((int)square) + 8;

            #endregion

            #region ValidatePawn

            private bool ValidatePawns()
                => !IsPawnOnRow1Or8(PiecesEnum.BlackPawn) && !IsPawnOnRow1Or8(PiecesEnum.WhitePawn);

            private bool IsPawnOnRow1Or8(PiecesEnum piece)
                => GetIteratorFor(piece).Any(ps => ps.Square.Row() == 0 || ps.Square.Row() == 7);

            #endregion
        }
    }
}
