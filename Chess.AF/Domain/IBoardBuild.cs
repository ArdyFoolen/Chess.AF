using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Domain
{
    public interface IBoardBuild
    {
        Option<IBoard> Build();
    }
}
