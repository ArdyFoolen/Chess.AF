using AF.Functional;
using Chess.AF.ChessForm.Views;
using Chess.AF.Enums;
using Chess.AF.PositionBridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;
using Unit = System.ValueTuple;

namespace Chess.AF.ChessForm.Controllers
{
    public class BoardController : IBoardController
    {
        private Game game;
        private Dictionary<int, (PiecesEnum Piece, SquareEnum Square, bool IsSelected)> positionDict;
        private IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> moves;
        private List<IBoardView> views = new List<IBoardView>();
        private int? selectedSquare;

        public bool IsWhiteToMove { get => game.IsWhiteToMove; }
        public bool IsMate { get => game.IsMate; }
        public bool IsInCheck { get => game.IsInCheck; }
        public bool IsStaleMate { get => game.IsStaleMate; }

        public GameResult Result { get => game.Result; }
        public int MaterialCount { get => game.MaterialCount; }

        public IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> SelectedMoves
        {
            get
            {
                if (selectedSquare.HasValue)
                    return moves.Where(w => (int)w.Square == selectedSquare);
                else
                    return Enumerable.Empty<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>();
            }
        }

        public bool IsPromoteMove(int moveSquare)
            => (IsSelected && SelectedMovesTo(moveSquare).Count() == 4);

        private IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> SelectedMovesTo(int moveSquare)
            => SelectedMoves.Where(w => (int)w.MoveSquare == moveSquare);

        public BoardController()
        {
            game = new Game();
            game.Load();
            SetPositionDict();
            moves = game.AllMoves();
        }

        public void SetFromPgn(Option<Pgn> pgn)
            => pgn.Map(p => SetFromPgn(p));

        private Unit SetFromPgn(Pgn pgn)
        {
            game = pgn.Game;
            GotoFirstMove();
            return Unit();
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

        public Option<(PiecesEnum Piece, SquareEnum Square, bool IsSelected)> this[int index]
        {
            get
            {
                if (positionDict.ContainsKey(index))
                    return Some(positionDict[index]);
                return None;
            }
        }

        public void LoadFen()
        {
            game.Load();
            ResetController();
        }

        public void LoadFen(string fen)
        {
            game.Load(fen);
            ResetController();
        }

        private void ResetController()
        {
            selectedSquare = null;
            SetPositionDict();
            moves = game.AllMoves();
            NotifyViews();
        }

        public void Select(int square)
        {
            if (IsSelected && SelectedMovesTo(square).Any())
                if (SelectedMovesTo(square).Count() == 4)
                    throw new Exception("Promotion happens throught Promote method");
                else
                    Move(SelectedMovesTo(square).First());

            positionDict.Keys.Where(w => positionDict[w].IsSelected).ForEach(f => UnSelect(f));
            selectedSquare = null;
            if (positionDict.ContainsKey(square) && IsFromMove(square))
                SetSelectedSquare(square);
            NotifyViews();
        }

        public void Promote(int moveSquare, int piece)
        {
            if (IsSelected && SelectedMovesTo(moveSquare).Count() == 4)
                Move(SelectedMovesTo(moveSquare).Single(s => s.Promoted == (PieceEnum)(piece % 7)));

            positionDict.Keys.Where(w => positionDict[w].IsSelected).ForEach(f => UnSelect(f));
            NotifyViews();
        }

        public string ToFenString()
            => game.ToFenString();

        private void Move((PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare) move)
        {
            var toMove = AF.Move.Of(move.Piece, move.Square, move.MoveSquare, move.Promoted);
            toMove.Map(m => Move(m));
            SetPositionDict();
            moves = game.AllMoves();
        }

        private Unit Move(AF.Move move)
        {
            game.Move(move);
            return Unit();
        }

        public void Resign()
        {
            game.Resign();
            NotifyViews();
        }

        public void Draw()
        {
            game.Draw();
            NotifyViews();
        }


        private bool IsSelected
        {
            get
            {
                return selectedSquare != null;
            }
        }

        public void GotoFirstMove()
        {
            game.GotoFirstMove();
            ResetController();
        }
        
        public void GotoPreviousMove()
        {
            game.GotoPreviousMove();
            ResetController();
        }

        public void GotoNextMove()
        {
            game.GotoNextMove();
            ResetController();
        }

        public void GotoLastMove()
        {
            game.GotoLastMove();
            ResetController();
        }

        private void SetSelectedSquare(int square)
        {
            selectedSquare = square;
            positionDict[square] = (positionDict[square].Piece, positionDict[square].Square, true);
        }

        private bool IsFromMove(int square)
            => moves.Any(a => (int)a.Square == square);

        private void UnSelect(int square)
        {
            if (positionDict.ContainsKey(square))
                positionDict[square] = (positionDict[square].Piece, positionDict[square].Square, false);
        }

        private void SetPositionDict()
            => game.Map(SetPositionDict);

        //private Position SetPositionDict(Position position)
        //{
        //    positionDict = position.ToDictionary();
        //    return position;
        //}
        private IPositionAbstraction SetPositionDict(IPositionAbstraction position)
        {
            positionDict = position.ToDictionary();
            return position;
        }
    }
}
