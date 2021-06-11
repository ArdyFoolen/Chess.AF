using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF
{
    internal class LoadCommand : Command
    {
        private static readonly string DefaultFen = @"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        private string Fen { get; set; }

        public LoadCommand()
        {
            this.Fen = DefaultFen;
        }

        public LoadCommand(string fen)
        {
            this.Fen = fen;
        }

        //public override void Execute()
        //    => Position = Fen.CreateFen().CreatePosition();
        public override void Execute()
            => Position = Fen.CreateFen().CreatePositionAbstraction();
    }
}
