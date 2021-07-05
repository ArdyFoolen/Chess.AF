using AF.Functional;
using Chess.AF.Domain;
using Chess.AF.ImportExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Commands
{
    internal class DrawCommand : Command
    {
        public DrawCommand(Option<IBoard> board) : base(board) { }

        public override void Execute()
            => Board = Board.Bind(p => p.Draw());

        internal override void Accept(ICommandVisitor visitor)
            => visitor.Visit(this);
    }
}
