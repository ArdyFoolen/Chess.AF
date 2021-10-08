using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.ImportExport
{
    public interface IPgnReader
    {
        List<Error> Errors { get; }
        Pgn Pgn { get; }
        void Read(string pgnFile);
    }
}
