using AF.Bootstrapper;
using AF.Factories;
using Chess.AF.ChessForm.Forms;
using Chess.AF.Controllers;
using Chess.AF.Controllers.Interfaces;
using Chess.AF.Domain;
using Chess.AF.ImportExport;
using System;
using System.Windows.Forms;
using static Chess.AF.Domain.Board;
using static Chess.AF.ImportExport.Pgn;

namespace Chess.AF.ChessForm
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Helpers.ConfigurationHelper.RegisterInterfaces();

            Container.Instance.Register<IGameController, GameController>();
            Container.Instance.Register<IGame, IGameFactory>(f => f.MakeGame("Chess Game"));
            Container.Instance.Register(() => Board.CreateBuilder());
            Container.Instance.Register<IGameBuilder, GameBuilder>();
            Container.Instance.Register<IPgnReader, PgnReader>();

            Container.Instance.Register<IPgnController, PgnController>();
            Container.Instance.Register<ISetupPositionController, SetupPositionController>();

            var gameController = Container.Instance.GetInstanceOf<IGameController>();
            var pgnController = Container.Instance.GetInstanceOf<IPgnController>();
            var setupPositionController = Container.Instance.GetInstanceOf<ISetupPositionController>();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ChessFrm(gameController, pgnController, setupPositionController));
        }
    }
}
