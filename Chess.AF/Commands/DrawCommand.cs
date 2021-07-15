using AF.Functional;
using Chess.AF.Domain;
using Chess.AF.Dto;
using Chess.AF.ImportExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF.Commands
{
    public class DrawCommand : Command, IMoveCommand
    {
        public Option<IBoard> Previous { get => Command != null ? Command.Previous : None; }
        public Option<Move> Move { get => Command != null ? Command.Move : None; }

        public IMoveCommand Command { get; private set; }

        public DrawCommand(ICommand command) : base(command.Board)
        {
            if (command is IMoveCommand)
                this.Command = command as IMoveCommand;
        }

        public override void Execute()
            => Board = Board.Bind(p => p.Draw());

        public override void Accept(ICommandVisitor visitor)
            => visitor.Visit(this);
    }
}
