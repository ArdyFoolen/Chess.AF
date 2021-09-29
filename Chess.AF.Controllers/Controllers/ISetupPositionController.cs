using Chess.AF.Domain;
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
        bool TryBuild();
    }
}
