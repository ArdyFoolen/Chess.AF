using AF.Functional;
using Chess.AF.Dto;
using Chess.AF.Domain;
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

        public MoveCommand(Option<IBoard> board, Move move) : base(board)
        {
            this.Move = move;
        }

        public override void Execute()
            => Board = Board.Bind(p => p.Move(Move));
    }
}
