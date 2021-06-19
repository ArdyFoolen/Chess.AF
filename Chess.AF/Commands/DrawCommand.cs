using AF.Functional;
using Chess.AF.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Commands
{
    internal class DrawCommand : Command
    {
        public DrawCommand(Option<IBoard> position) : base(position) { }

        public override void Execute()
            => Position = Position.Bind(p => p.Draw());
    }
}
