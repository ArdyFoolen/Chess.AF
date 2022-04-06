using Chess.AF.ChessForm.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.ChessForm.Factories
{
    public interface ISquareControlFactory
    {
        BaseSquareControl Create(int id);
    }
}
