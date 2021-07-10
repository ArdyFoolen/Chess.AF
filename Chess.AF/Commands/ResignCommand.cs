using AF.Functional;
using Chess.AF.Domain;
using Chess.AF.Dto;
using Chess.AF.ImportExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Commands
{
    public class ResignCommand : Command, IMoveCommand
    {
        public Option<IBoard> Previous { get => Command.Previous; }
        public Move Move { get => Command.Move; }

        public IMoveCommand Command { get; private set; }

        public ResignCommand(IMoveCommand command) : base(command.Board)
        {
            this.Command = command;
        }

        public override void Execute()
            => Board = Board.Bind(p => p.Resign());

        public override void Accept(ICommandVisitor visitor)
            => visitor.Visit(this);
    }
}
