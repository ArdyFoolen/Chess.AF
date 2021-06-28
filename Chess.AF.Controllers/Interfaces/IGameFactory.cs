using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Controllers.Interfaces
{
    public interface IGameFactory
    {
        IGame MakeGame(string gameType);
        IEnumerable<string> AvailableGameTypes();
    }
}
