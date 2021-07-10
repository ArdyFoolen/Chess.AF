using Chess.AF.ImportExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Commands
{
    public class LoadCommand : Command
    {
        private static readonly string DefaultFen = @"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        public string Fen { get; private set; }
        public bool IsDefaultFen { get; } = false;

        public LoadCommand() : this(DefaultFen) { }

        public LoadCommand(string fen)
        {
            IsDefaultFen = DefaultFen.Equals(fen);
            this.Fen = fen;
        }

        public override void Execute()
            => Board = Fen.CreateFen().CreateBoard();

        public override void Accept(ICommandVisitor visitor)
            => visitor.Visit(this);
    }
}
