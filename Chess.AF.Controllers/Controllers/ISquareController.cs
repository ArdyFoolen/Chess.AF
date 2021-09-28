using AF.Functional;
using Chess.AF.Dto;
using Chess.AF.Enums;
using Chess.AF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Controllers
{
    public interface ISquareController
    {
        void Register(IBoardView view);
        void UnRegister(IBoardView view);
        Option<PieceOnSquare<PiecesEnum>> this[int index]
        {
            get;
        }
        void Select(int square);
    }
}
