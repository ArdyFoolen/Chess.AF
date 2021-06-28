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
        public const string TestChessGame = "Test Chess Game";

        public const string ChessGame = "Chess Game";
        public IEnumerable<string> AvailableGameTypes()
        {
            yield return ChessGame;
            yield return TestChessGame;
        }

        public IGame MakeGame(string gameType)
        {
            switch(gameType)
            {
                case TestChessGame:
                    return new TestGame();
                case ChessGame:
                    return new Game();
            }

            return null;
        }
    }
}
