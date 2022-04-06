using Chess.AF.ChessForm.Controls;
using Chess.AF.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.ChessForm.Factories
{
    public class BaseSquareControlFactory : ISquareControlFactory
    {
        private ISquareController squareController;

        public BaseSquareControlFactory(ISquareController squareController)
        {
            this.squareController = squareController;
        }

        public BaseSquareControl Create(int id)
            => new BaseSquareControl(id, squareController);
    }
}
