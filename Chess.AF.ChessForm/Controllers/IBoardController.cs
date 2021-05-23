using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.ChessForm.Controllers
{
    public interface IBoardController
    {
        Option<(PiecesEnum Piece, SquareEnum Square, bool IsSelected)> this[int index]
        {
            get;
        }
    }
}
