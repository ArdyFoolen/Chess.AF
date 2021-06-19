using AF.Functional;
using Chess.AF.Dto;
using Chess.AF.PositionBridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Commands
{
    internal class MoveCommand : Command
    {
        public Move Move { get; private set; }

        public MoveCommand(Option<IBoard> position, Move move) : base(position)
        {
            this.Move = move;
        }

        public override void Execute()
            => Position = Position.Bind(p => p.Move(Move));
    }
}
