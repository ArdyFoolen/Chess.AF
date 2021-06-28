using Chess.AF.Controllers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Controllers.Tests
{
    public class TestGameFactory : IGameFactory
    {
        public IGame MakeGame()
            => new TestGame();
    }
}
