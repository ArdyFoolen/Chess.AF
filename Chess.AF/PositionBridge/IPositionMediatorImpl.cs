using AF.Functional;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.PositionBridge
{
    public interface IPositionMediatorImpl
    {
        bool IsWhiteToMove { get; }
        RokadeEnum Rokade { get; }
        Option<SquareEnum> EpSquare { get; }
    }
}
