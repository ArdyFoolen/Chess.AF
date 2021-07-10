using AF.Functional;
using Chess.AF.Domain;
using Chess.AF.ImportExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Commands
{
    public interface ICommand
    {
        Option<IBoard> Board { get; }
        void Execute();
        void Accept(ICommandVisitor visitor);
    }
}
