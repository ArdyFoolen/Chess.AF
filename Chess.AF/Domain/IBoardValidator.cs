using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Domain
{
    public interface IBoardValidator
    {
        void SetBoard(IBoard board);
        void SetBoardMap(IBoardMap boardMap);

        Validation<Board> Validate();
    }
}
