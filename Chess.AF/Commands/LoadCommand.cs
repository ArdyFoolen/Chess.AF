using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Commands
{
    internal class LoadCommand : Command
    {
        private static readonly string DefaultFen = @"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        public string Fen { get; private set; }
        public bool IsDefaultFen { get; } = false;

        public LoadCommand() : this(DefaultFen) { IsDefaultFen = true; }

        public LoadCommand(string fen)
        {
            this.Fen = fen;
        }

        public override void Execute()
            => Board = Fen.CreateFen().CreateBoard();
    }
}
