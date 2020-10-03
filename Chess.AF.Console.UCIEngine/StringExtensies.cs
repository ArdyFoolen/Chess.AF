using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Console.UCIEngine
{
    public static class StringExtensies
    {
        public static bool IsEqual(this string left, string right)
            => string.Compare(left, right) == 0;
    }
}
