using AF.Bootstrapper;
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
            ConfigurationHelper.LoadFactoryTypes();
            IGameFactory gameFactory = Container.Instance.GetInstanceOf<IGameFactory>();
            IGame game = gameFactory.MakeGame();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ChessFrm(game));
        }
    }
}
