using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.UCIEngine
{
    public class CompareCultureInvariant : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
            => x.ToLowerInvariant().Equals(y.ToLowerInvariant());

        public int GetHashCode(string obj)
            => obj.GetHashCode();
    }
}
