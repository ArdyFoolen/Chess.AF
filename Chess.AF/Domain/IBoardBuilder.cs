using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Domain
{
    public interface IBoardBuilder
    {
        IBoardBuilder Default();
        IBoardBuilder WithWhiteToMove(bool whiteToMove);
        IBoardBuilder WithWhiteRokade(RokadeEnum rokade);
        IBoardBuilder WithBlackRokade(RokadeEnum rokade);
        IBoardBuilder WithEpSquare(SquareEnum epSquare);

        IBoardBuilder Clear();
        IBoardBuilder WithPiece(PiecesEnum piece);
        IBoardBuilder Off(SquareEnum square);
        IBoardBuilder On(SquareEnum square);
        IBoardBuilder Toggle(SquareEnum square);
    }
}
