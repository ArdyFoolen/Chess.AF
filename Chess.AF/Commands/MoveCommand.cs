using AF.Functional;
using Chess.AF.Dto;
using Chess.AF.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.AF.ImportExport;

namespace Chess.AF.Commands
{
    public class MoveCommand : Command, IMoveCommand
    {
        public Option<IBoard> Previous { get; private set; }
        public Move Move { get; private set; }

        public MoveCommand(Option<IBoard> board, Move move) : base(board)
        {
            this.Previous = board;
            this.Move = move;
        }

        public override void Execute()
            => Board = Previous.Bind(p => p.Move(Move));

        public override void Accept(ICommandVisitor visitor)
            => visitor.Visit(this);
    }
}
