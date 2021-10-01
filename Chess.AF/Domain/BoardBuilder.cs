using AF.Functional;
using Chess.AF.Dto;
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
        public static IBoardBuilder CreateBuilder()
            => new BoardBuilder(CreateValidator());

        private static IBoardValidator CreateValidator()
            => new BoardValidator();

        public class BoardBuilder : IBoardBuilder, IBoardBuild
        {
            #region Properties

            private Board board;
            private BoardMapBuilder boardMapBuilder;
            private IBoardValidator validator;

            #endregion

            #region ctor

            public BoardBuilder(IBoardValidator validator)
            {
                this.validator = validator;
                boardMapBuilder = new BoardMapBuilder();
                board = new Board();
                Default();
            }

            #endregion

            #region IBoardBuilder

            public PiecesEnum CurrentPiece { get => boardMapBuilder.CurrentPiece; }
            public Option<SquareEnum> CurrentEpSquare { get => board.EpSquare; }
            public bool IsWhiteToMove { get => board.IsWhiteToMove; }
            public RokadeEnum WhiteRokade { get => board.WhiteRokade; }
            public RokadeEnum BlackRokade { get => board.BlackRokade; }

            public Option<PieceOnSquare<PiecesEnum>> GetPieceOn(SquareEnum square)
                => boardMapBuilder.GetPieceOn(square);

            public IBoardBuilder Default()
            {
                board.IsWhiteToMove = true;
                board.WhiteRokade = RokadeEnum.None;
                board.BlackRokade = RokadeEnum.None;
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

            public IBoardBuilder WithoutEpSquare()
            {
                board.EpSquare = None;
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

            private bool IsValid()
            {
                validator.SetBoard(board);
                validator.SetBoardMap(board.Implementor);
                return validator.Validate();
            }

            #endregion

            #region Build

            public Option<IBoard> Build()
            {
                boardMapBuilder.WithBoard(board);
                board.Implementor = boardMapBuilder.Build();

                if (!IsValid())
                    return None;

                return board;
            }

            #endregion
        }
    }
}
