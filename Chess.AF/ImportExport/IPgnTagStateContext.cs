using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.ImportExport
{
    public interface IPgnTagStateContext
    {
        PgnTagState State { get; set; }
    }
}
