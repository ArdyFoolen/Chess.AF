using AF.Bootstrapper;
using AF.Factories;
using Chess.AF.ChessForm.Extensions;
using Chess.AF.ChessForm.Helpers;
using Chess.AF.Controllers;
using Chess.AF.Controllers.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Container.Instance.Register<IGameFactory, GameFactory>();
            Container.Instance.Register<IGameController, GameController>();
            Container.Instance.Register<IGame, IGameFactory>(f => f.MakeGame());
            Container.Instance.Register<IPgnController, PgnController>();
            var gameController = Container.Instance.GetInstanceOf<IGameController>();
            var pgnController = Container.Instance.GetInstanceOf<IPgnController>();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ChessFrm(gameController, pgnController));
        }
    }
}
