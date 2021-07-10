using AF.Functional;
using Chess.AF.Domain;
using Chess.AF.ImportExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF.Commands
{
    public abstract class Command : ICommand
    {
        public Option<IBoard> Board { get; protected set; }

        public Command()
        {
            this.Board = None;
        }

        public Command(Option<IBoard> board)
        {
            this.Board = board;
        }

        public abstract void Execute();
        public abstract void Accept(ICommandVisitor visitor);
    }
}
