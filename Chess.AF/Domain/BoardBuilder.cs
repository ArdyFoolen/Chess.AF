using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;
using static Chess.AF.Domain.BoardMap;

namespace Chess.AF.Domain
{
    public partial class Board
    {
        public class BoardBuilder : IBoardBuilder
        {
            #region Properties

            private Board board;
            private BoardMapBuilder boardMapBuilder;

            #endregion

            #region ctor

            public BoardBuilder()
            {
                boardMapBuilder = new BoardMapBuilder();
                board = new Board();
                Default();
            }

            #endregion

            #region IBoardBuilder

            public IBoardBuilder Default()
            {
                board.IsWhiteToMove = true;
                board.WhiteRokade = RokadeEnum.KingAndQueenSide;
                board.BlackRokade = RokadeEnum.KingAndQueenSide;
                board.EpSquare = None;
                board.PlyCount = 0;
                board.MoveNumber = 1;
                board.LastMove = None;

                return this;
            }

            public IBoardBuilder WithWhiteToMove(bool whiteToMove)
            {
                board.IsWhiteToMove = whiteToMove;
                return this;
            }

            public IBoardBuilder WithWhiteRokade(RokadeEnum rokade)
            {
                board.WhiteRokade = rokade;
                return this;
            }

            public IBoardBuilder WithBlackRokade(RokadeEnum rokade)
            {
                board.BlackRokade = rokade;
                return this;
            }

            public IBoardBuilder WithEpSquare(SquareEnum epSquare)
            {
                board.EpSquare = Some(epSquare);
                return this;
            }

            public IBoardBuilder Clear()
            {
                boardMapBuilder.Clear();
                return this;
            }
            public IBoardBuilder WithPiece(PiecesEnum piece)
            {
                boardMapBuilder.WithPiece(piece);
                return this;
            }

            public IBoardBuilder Off(SquareEnum square)
            {
                boardMapBuilder.Off(square);
                return this;
            }

            public IBoardBuilder On(SquareEnum square)
            {
                boardMapBuilder.On(square);
                return this;
            }

            public IBoardBuilder Toggle(SquareEnum square)
            {
                boardMapBuilder.Toggle(square);
                return this;
            }

            #endregion

            #region Validation

            // Validate King, only 1 for white and black
            // (Done) Validate rokade
            //      Black/White: King on king square
            //                   King Rokade Rook on Kingside rook square
            //                   Queen Rokade Rook on Queenside rook square
            // Validate EpSquare
            //      EpSquare on row 2 or 5
            //      if on 2 then pawn on row 3
            //      if on 5 then pawn on row 4

            private bool IsKingSideRokadePossible(bool isWhiteToMove)
                => isWhiteToMove ? board.WhiteRokade.IsKingsideRokade() : board.BlackRokade.IsKingsideRokade();

            private bool IsQueenSideRokadePossible(bool isWhiteToMove)
                => isWhiteToMove ? board.WhiteRokade.IsQueensideRokade() : board.BlackRokade.IsQueensideRokade();

            private bool IsValidWhiteRookRokade()
                => (!IsKingSideRokadePossible(true) ||
                    IsKingSideRokadePossible(true) && boardMapBuilder.ValidRookOnRookSquare(true, board.WhiteRokade.GetRookRokadeOnKingsideSquare(true))) &&
                   (!IsQueenSideRokadePossible(true) ||
                    IsQueenSideRokadePossible(true) && boardMapBuilder.ValidRookOnRookSquare(true, board.WhiteRokade.GetRookRokadeOnQueensideSquare(true)));

            private bool IsValidBlackRookRokade()
                => (!IsKingSideRokadePossible(false) ||
                    IsKingSideRokadePossible(false) && boardMapBuilder.ValidRookOnRookSquare(false, board.BlackRokade.GetRookRokadeOnKingsideSquare(false))) &&
                   (!IsQueenSideRokadePossible(false) ||
                    IsQueenSideRokadePossible(false) && boardMapBuilder.ValidRookOnRookSquare(false, board.WhiteRokade.GetRookRokadeOnQueensideSquare(false)));

            private bool IsRokadePossible(bool isWhiteToMove)
                => isWhiteToMove ?
                    !RokadeEnum.None.Equals(board.WhiteRokade) :
                    !RokadeEnum.None.Equals(board.BlackRokade);

            private bool IsValidWhiteKingRokade()
                => !IsRokadePossible(true) ||
                    IsRokadePossible(true) && boardMapBuilder.ValidKingOnKingSquare(true, board.WhiteRokade.GetKingRokadeSquare(true));

            private bool IsValidBlackKingRokade()
                => !IsRokadePossible(false) ||
                    IsRokadePossible(false) && boardMapBuilder.ValidKingOnKingSquare(false, board.BlackRokade.GetKingRokadeSquare(false));

            private bool IsValidRokade()
                => IsValidWhiteKingRokade() && IsValidBlackKingRokade() && IsValidWhiteRookRokade() && IsValidBlackRookRokade();

            #endregion

            #region Build

            public IBoard Build()
            {
                boardMapBuilder.WithBoard(board);
                board.Implementor = boardMapBuilder.Build();
                return board;
            }

            #endregion
        }
    }
}
