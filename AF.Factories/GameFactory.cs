using Chess.AF;
using Chess.AF.Controllers.Interfaces;

namespace AF.Factories
{
    public class GameFactory : IGameFactory
    {
        public IGame MakeGame()
            => new Game();
    }
}
