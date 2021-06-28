using Chess.AF;
using Chess.AF.Controllers.Interfaces;
using System.Collections.Generic;

namespace AF.Factories
{
    public class GameFactory : IGameFactory
    {
        public const string ChessGame = "Chess Game";
        public IEnumerable<string> AvailableGameTypes()
        {
            yield return ChessGame;
        }

        public IGame MakeGame(string gameType)
        {
            switch (gameType)
            {
                case ChessGame:
                    return new Game();
            }

            return null;
        }
    }
}
