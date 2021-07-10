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
    internal interface IMoveCommand
    {
        Option<IBoard> Board { get; }
        Option<IBoard> Previous { get; }
        Move Move { get; }
    }
}
