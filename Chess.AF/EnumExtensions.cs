using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF
{
    /// <summary> Enum Extension Methods </summary>
    /// <typeparam name="T"> type of Enum </typeparam>
    public class Enum<TEnum> where TEnum : struct, IConvertible
    {
        public static IEnumerable<TEnum> AsEnumerable()
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("TEnum must be an enumerated type");

            foreach (TEnum elem in Enum.GetValues(typeof(TEnum)))
                yield return elem;
        }

    }
}
