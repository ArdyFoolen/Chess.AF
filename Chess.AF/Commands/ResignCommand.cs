using AF.Functional;
using Chess.AF.PositionBridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Commands
{
    internal class ResignCommand : Command
    {
        public ResignCommand(Option<IBoard> position) : base(position) { }

        public override void Execute()
            => Position = Position.Bind(p => p.Resign());
    }
}
