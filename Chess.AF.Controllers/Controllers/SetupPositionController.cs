﻿using AF.Functional;
using Chess.AF.Domain;
using Chess.AF.Dto;
using Chess.AF.Enums;
using Chess.AF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF.Controllers
{
    public class SetupPositionController : ISetupPositionController
    {
        private IBoardBuilder boardBuilder;
        private List<IBoardView> views = new List<IBoardView>();
        public IEnumerable<Error> Errors { get; private set; } = Enumerable.Empty<Error>();
        public IBoard Board { get; private set; }

        public Option<PieceOnSquare<PiecesEnum>> this[int index] { get => boardBuilder.GetPieceOn((SquareEnum)index); }

        public SetupPositionController(IBoardBuilder boardBuilder)
        {
            this.boardBuilder = boardBuilder;
        }

        public void Register(IBoardView view)
        {
            if (!views.Contains(view))
                views.Add(view);
        }

        public void UnRegister(IBoardView view)
        {
            if (views.Contains(view))
                views.Remove(view);
        }

        private void NotifyViews()
        {
            foreach (var view in views)
                view.UpdateView();
        }

        public PiecesEnum CurrentPiece { get => boardBuilder.CurrentPiece; }
        public Option<SquareEnum> CurrentEpSquare { get => boardBuilder.CurrentEpSquare; }
        public bool IsWhiteToMove { get => boardBuilder.IsWhiteToMove; }
        public RokadeEnum WhiteRokade { get => boardBuilder.WhiteRokade; }
        public RokadeEnum BlackRokade { get => boardBuilder.BlackRokade; }

        public void WithWhiteToMove(bool whiteToMove)
        {
            boardBuilder.With(whiteToMove);
            NotifyViews();
        }

        public void WithPiece(PiecesEnum piece)
        {
            boardBuilder.With(piece);
            NotifyViews();
        }

        public void WithEpSquare(SquareEnum epSquare)
        {
            boardBuilder.WithEnPassant(epSquare);
            NotifyViews();
        }

        public void WithoutEpSquare()
        {
            boardBuilder.WithoutEnPassant();
            NotifyViews();
        }

        public void WithWhiteRokade(RokadeEnum rokade)
        {
            boardBuilder.WithWhite(rokade);
            NotifyViews();

        }
        public void WithBlackRokade(RokadeEnum rokade)
        {
            boardBuilder.WithBlack(rokade);
            NotifyViews();
        }

        public void ClearBoard()
        {
            boardBuilder.Clear();
            NotifyViews();
        }

        public void Select(int square)
        {
            boardBuilder.Toggle((SquareEnum)square);
            NotifyViews();
        }

        public bool TryBuild()
        {
            var board = (boardBuilder as IBoardBuild).Build();

            return board.Match(
                Invalid: ie => SetErrors(ie),
                Valid: b => SetBoard(b)
                );
        }

        private bool SetErrors(IEnumerable<Error> errors)
        {
            this.Errors = errors;
            NotifyViews();
            return false;
        }

        private bool SetBoard(IBoard board)
        {
            Errors = Enumerable.Empty<Error>(); ;
            this.Board = board;
            return true;
        }
    }
}
