using AF.Bootstrapper;
using Chess.AF.Controllers.Interfaces;
using NUnit.Framework;
using System;

namespace Chess.AF.Controllers.Tests
{
    public class GameFactoryTests
    {
        [SetUp]
        public void Setup()
        {
            Container.Instance.Register<IGameFactory, TestGameFactory>();
            Container.Instance.Register<IGame, IGameFactory>(f => f.MakeGame());
        }

        [Test]
        public void GameFactory_Ctor_ShouldCreateBootstrappedGame()
        {
            // Arrange
            IGame game = Container.Instance.GetInstanceOf<IGame>();

            // Act
            Assert.Throws<NotImplementedException>(() => game.AllMoves());
        }
    }
}