using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF
{
    internal abstract class Command
    {
        public Option<Position> Position { get; protected set; }

        public Command()
        {
            this.Position = None;
        }

        public Command(Option<Position> position)
        {
            this.Position = position;
        }

        public abstract void Execute();
    }
}
