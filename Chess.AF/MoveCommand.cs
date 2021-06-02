using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF
{
    internal class MoveCommand : Command
    {
        public Move Move { get; private set; }

        public MoveCommand(Option<Position> position, Move move) : base(position)
        {
            this.Move = move;
        }

        public override void Execute()
            => Position = Position.Bind(p => p.Move(Move))
            .Match(None: () => Position,
                    Some: s => s);
    }
}
