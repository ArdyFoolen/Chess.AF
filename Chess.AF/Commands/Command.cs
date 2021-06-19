using AF.Functional;
using Chess.AF.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF.Commands
{
    internal abstract class Command
    {
        public Option<IBoard> Position { get; protected set; }

        public Command()
        {
            this.Position = None;
        }

        public Command(Option<IBoard> position)
        {
            this.Position = position;
        }

        public abstract void Execute();
    }
}
