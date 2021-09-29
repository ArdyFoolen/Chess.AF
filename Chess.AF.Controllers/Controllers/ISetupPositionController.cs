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
        PiecesEnum CurrentPiece { get; }

        void WithPiece(PiecesEnum piece);
        bool TryBuild();
    }
}
