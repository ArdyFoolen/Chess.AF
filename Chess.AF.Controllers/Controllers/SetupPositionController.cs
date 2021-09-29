using AF.Functional;
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
        private string error = "";
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

        public void Select(int square)
        {
            boardBuilder.On((SquareEnum)square);
            NotifyViews();
        }

        public bool TryBuild()
        {
            //boardBuilder
            //    .WithPiece(PiecesEnum.BlackKing);
            //Select(5);
            //boardBuilder
            //    .WithWhiteRokade(RokadeEnum.None)
            //    .WithBlackRokade(RokadeEnum.None)
            //    .WithPiece(PiecesEnum.WhiteKing).On(SquareEnum.e1)
            //    .WithPiece(PiecesEnum.BlackKing).On(SquareEnum.e8);
            var board = (boardBuilder as IBoardBuild).Build();

            return board.Match(
                None: () => SetError("Failed to build Board"),
                Some: b => SetBoard(b)
                );
        }

        private bool SetError(string error)
        {
            this.error = error;
            NotifyViews();
            return false;
        }

        private bool SetBoard(IBoard board)
        {
            error = "";
            this.Board = board;
            return true;
        }
    }
}
