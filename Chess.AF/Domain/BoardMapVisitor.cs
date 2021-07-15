using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Domain
{
    public partial class BoardMap
    {
        private abstract class BoardMapVisitor : IBoardMapVisitor
        {
            public virtual void Visit(BoardMap map) { }
        }
    }
}
