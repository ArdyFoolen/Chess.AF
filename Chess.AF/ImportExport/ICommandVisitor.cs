using Chess.AF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.ImportExport
{
    internal interface ICommandVisitor
    {
        void Visit(LoadCommand command);
        void Visit(MoveCommand command);
        void Visit(DrawCommand command);
        void Visit(ResignCommand command);
    }
}
