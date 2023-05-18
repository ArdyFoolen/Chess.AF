using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Dto
{
    public class AttackSquare
    {
        public SquareEnum Square { get; set; }
        public int Count { get; set; }
    }
}
