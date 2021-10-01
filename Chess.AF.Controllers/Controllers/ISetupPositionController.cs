using AF.Functional;
using Chess.AF.Domain;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Controllers
{
    public interface ISetupPositionController : ISquareController
    {
        IBoard Board { get; }
        IEnumerable<Error> Errors { get; }
        PiecesEnum CurrentPiece { get; }
        Option<SquareEnum> CurrentEpSquare { get; }
        bool IsWhiteToMove { get; }
        RokadeEnum WhiteRokade { get; }
        RokadeEnum BlackRokade { get; }

        void ClearBoard();
        void WithWhiteToMove(bool whiteToMove);
        void WithPiece(PiecesEnum piece);
        void WithEpSquare(SquareEnum epSquare);
        void WithoutEpSquare();
        void WithWhiteRokade(RokadeEnum rokade);
        void WithBlackRokade(RokadeEnum rokade);

        bool TryBuild();
    }
}
