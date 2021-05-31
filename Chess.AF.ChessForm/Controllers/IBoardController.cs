using AF.Functional;
using Chess.AF.ChessForm.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.ChessForm.Controllers
{
    public interface IBoardController
    {
        void Register(IBoardView view);
        void UnRegister(IBoardView view);
        Option<(PiecesEnum Piece, SquareEnum Square, bool IsSelected)> this[int index]
        {
            get;
        }

        IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> SelectedMoves
        {
            get;
        }

        bool IsPromoteMove(int square);

        void Select(int square);
        void Promote(int moveSquare, int piece);
        string ToFenString();
    }
}
