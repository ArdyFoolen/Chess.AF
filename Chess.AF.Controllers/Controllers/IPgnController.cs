using Chess.AF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Controllers
{
    public interface IPgnController
    {
        void Register(IPgnView view);
        void UnRegister(IPgnView view);
        Dictionary<string, string> TagPairDictionary { get; }

        void SetTagPairDictionary(Dictionary<string, string> tagPairDictionary);
        void Clear();
    }
}
