using Chess.AF.ChessForm.Controls;
using Chess.AF.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.ChessForm.Factories
{
    public class SquareControlFactory : ISquareControlFactory
    {
        private IGameController gameController;

        public SquareControlFactory(IGameController gameController)
        {
            this.gameController = gameController;
        }

        public BaseSquareControl Create(int id)
            => new SquareControl(id, gameController);
    }
}
