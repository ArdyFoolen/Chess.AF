using AF.Functional;
using Chess.AF.Domain;
using Chess.AF.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Commands
{
    public interface IMoveCommand : ICommand
    {
        Option<IBoard> Previous { get; }
        Option<Move> Move { get; }
    }
}
