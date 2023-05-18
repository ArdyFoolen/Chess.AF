using AF.Functional;
using Chess.AF.Views;
using Chess.AF.Dto;
using Chess.AF.Enums;
using Chess.AF.ImportExport;
using Chess.AF.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;
using Unit = System.ValueTuple;

namespace Chess.AF.Controllers
{
    public class GameController : IGameController
    {
        private IGame game;
        private IPgnController pgnController;

        private Dictionary<int, PieceOnSquare<PiecesEnum>> boardDictionary = new Dictionary<int, PieceOnSquare<PiecesEnum>>();
        private IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> moves;
        private List<IBoardView> views = new List<IBoardView>();
        private int? selectedSquare;

        public bool IsWhiteToMove { get => game.IsWhiteToMove; }
        public bool IsMate { get => game.IsMate; }
        public bool IsInCheck { get => game.IsInCheck; }
        public bool IsStaleMate { get => game.IsStaleMate; }

        public GameResult Result { get => game.Result; }
        public int MoveNumber { get => game.MoveNumber; }
        public int PlyCount { get => game.PlyCount; }
        public Option<Move> LastMove { get => game.LastMove; }
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

        public GameController(IGame game, IPgnController pgnController)
        {
            this.pgnController = pgnController;
            this.game = game;
            this.game.Load();
            SetPositionDict();
            moves = game.AllMoves();
        }

        public bool IsPromoteMove(int moveSquare)
            => (IsSelected && SelectedMovesTo(moveSquare).Count() == 4);

        private IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> SelectedMovesTo(int moveSquare)
            => SelectedMoves.Where(w => (int)w.MoveSquare == moveSquare);

        public void SetFromPgn(Option<Pgn> pgn)
            => pgn.Map(p => SetFromPgn(p));

        private Unit SetFromPgn(Pgn pgn)
        {
            game = pgn.Game;
            GotoFirstMove();
            pgnController.SetTagPairDictionary(pgn.TagPairDictionary);
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
            RefreshIterators();

            foreach (var view in views)
                view.UpdateView();
        }

        public Option<PieceOnSquare<PiecesEnum>> this[int index]
        {
            get
            {
                if (boardDictionary.ContainsKey(index))
                    return Some(boardDictionary[index]);
                return None;
            }
        }

        public void LoadFen()
        {
            game.Load();
            pgnController.Clear();
            ResetController();
        }

        public void LoadFen(string fen)
        {
            game.Load(fen);
            pgnController.Clear();
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

            boardDictionary.Keys.Where(w => boardDictionary[w].IsSelected).ForEach(f => UnSelect(f));
            selectedSquare = null;
            if (boardDictionary.ContainsKey(square) && IsFromMove(square))
                SetSelectedSquare(square);
            NotifyViews();
        }

        public void Promote(int moveSquare, int piece)
        {
            if (IsSelected && SelectedMovesTo(moveSquare).Count() == 4)
                Move(SelectedMovesTo(moveSquare).Single(s => s.Promoted == (PieceEnum)(piece % 7)));

            boardDictionary.Keys.Where(w => boardDictionary[w].IsSelected).ForEach(f => UnSelect(f));
            NotifyViews();
        }

        public Option<Pgn> Export()
            => game.Export();

        public string ToFenString()
            => game.ToFenString();

        private void Move((PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare) move)
        {
            var toMove = AF.Dto.Move.Of(move.Piece, move.Square, move.MoveSquare, move.Promoted);
            toMove.Map(m => Move(m));
            SetPositionDict();
            moves = game.AllMoves();
        }

        private Unit Move(AF.Dto.Move move)
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

        private void RefreshIterators()
        {
            RefreshLoosePiecesIterator();
            RefreshNotAttackedIterator();
            RefreshNumberAttackedIterator();
        }

        #region LoosePiecesIterator

        public void UseLoosePiecesIterator(bool on, FilterFlags flags = FilterFlags.Both)
        {
            IsSetLoosePieceSquares = on;
            loosePiecesFlags = flags;
            NotifyViews();
        }

        private void RefreshLoosePiecesIterator()
        {
            if (IsSetLoosePieceSquares)
                game.Map(SetLoosePiecesIterator);
        }

        private IBoard SetLoosePiecesIterator(IBoard board)
        {
            board.Accept(LoosePiecesVisitor);
            return board;
        }

        private ILoosePiecesVisitor loosePiecesVisitor = null;
        private ILoosePiecesVisitor LoosePiecesVisitor
        {
            get
            {
                if (loosePiecesVisitor == null)
                    loosePiecesVisitor = BoardMap.GetLoosePiecesVisitor();
                loosePiecesVisitor.Flags = loosePiecesFlags;
                return loosePiecesVisitor;
            }
        }
        private bool IsSetLoosePieceSquares { get; set; } = false;
        private FilterFlags loosePiecesFlags { get; set; } = FilterFlags.Both;
        public IEnumerable<SquareEnum> LoosePieceSquares
        {
            get
            {
                if (IsSetLoosePieceSquares)
                    return loosePiecesVisitor.Iterator;

                return Enumerable.Empty<SquareEnum>();
            }
        }

        #endregion

        #region NotAttackedIterator

        public void UseNotAttackedIterator(bool on, FilterFlags flags = FilterFlags.Both)
        {
            IsSetNotAttackedSquares = on;
            notAttackedFlags = flags;
            NotifyViews();
        }

        private void RefreshNotAttackedIterator()
        {
            if (IsSetNotAttackedSquares)
                game.Map(SetNotAttackedIterator);
        }

        private IBoard SetNotAttackedIterator(IBoard board)
        {
            board.Accept(NotAttackedVisitor);
            return board;
        }

        private ISquareNotAttackedVisitor notAttackedVisitor = null;
        private ISquareNotAttackedVisitor NotAttackedVisitor
        {
            get
            {
                if (notAttackedVisitor == null)
                    notAttackedVisitor = BoardMap.GetSquareNotAttackedVisitor();
                notAttackedVisitor.Flags = notAttackedFlags;
                return notAttackedVisitor;
            }
        }
        private bool IsSetNotAttackedSquares { get; set; } = false;
        private FilterFlags notAttackedFlags { get; set; } = FilterFlags.Both;
        public IEnumerable<SquareEnum> NotAttackedSquares
        {
            get
            {
                if (IsSetNotAttackedSquares)
                    return notAttackedVisitor.Iterator;

                return Enumerable.Empty<SquareEnum>();
            }
        }

        #endregion

        #region NumberAttackedIterator

        public void UseNumberAttackedIterator(bool on, FilterFlags flags = FilterFlags.Both)
        {
            IsSetNumberAttackedSquares = on;
            numberAttackedFlags = flags;
            NotifyViews();
        }

        private void RefreshNumberAttackedIterator()
        {
            if (IsSetNumberAttackedSquares)
                game.Map(SetNumberAttackedIterator);
        }

        private IBoard SetNumberAttackedIterator(IBoard board)
        {
            board.Accept(NumberAttackedVisitor);
            return board;
        }

        private ISquareNumberAttackedVisitor numberAttackedVisitor = null;
        private ISquareNumberAttackedVisitor NumberAttackedVisitor
        {
            get
            {
                if (numberAttackedVisitor == null)
                    numberAttackedVisitor = BoardMap.GetSquareNumberAttackedVisitor();
                numberAttackedVisitor.Flags = numberAttackedFlags;
                return numberAttackedVisitor;
            }
        }
        private bool IsSetNumberAttackedSquares { get; set; } = false;
        private FilterFlags numberAttackedFlags { get; set; } = FilterFlags.Both;
        public IEnumerable<AttackSquare> NumberAttackedSquares
        {
            get
            {
                if (IsSetNumberAttackedSquares)
                    return numberAttackedVisitor.Iterator;

                return Enumerable.Empty<AttackSquare>();
            }
        }

        #endregion

        private void SetSelectedSquare(int square)
        {
            selectedSquare = square;
            boardDictionary[square] = new PieceOnSquare<PiecesEnum>(boardDictionary[square].Piece, boardDictionary[square].Square, true);
        }

        private bool IsFromMove(int square)
            => moves.Any(a => (int)a.Square == square);

        private void UnSelect(int square)
        {
            if (boardDictionary.ContainsKey(square))
                boardDictionary[square] = new PieceOnSquare<PiecesEnum>(boardDictionary[square].Piece, boardDictionary[square].Square, false);
        }

        private void SetPositionDict()
            => game.Map(SetPositionDict);

        private IBoard SetPositionDict(IBoard board)
        {
            boardDictionary = board.ToDictionary();
            return board;
        }
    }
}
