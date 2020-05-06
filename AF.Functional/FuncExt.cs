using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.Functional
{
    using static F;

    public static class FuncExt
    {
        public static Either<L, Option<T>> WhileNone<L, T>(this Func<Either<L, Option<T>>> func)
        {
            bool toContinue = true;
            Either<L, Option<T>> either;

            do
            {
                either = func();
                either.Match(
                    Left: l => toContinue = false,
                    Right: r => r.Match(
                        None: () => toContinue = true,
                        Some: _ => toContinue = false));
            } while (toContinue);

            return either;
        }
        public static Either<L, Option<T>> WhileNotAborted<L, T>(this Func<Either<L, Option<T>>> func)
        {
            bool toContinue = true;
            Either<L, Option<T>> either;

            do
            {
                either = func();
                either.Match(
                    Left: l => toContinue = false,
                    Right: r => r.Match(
                        None: () => toContinue = true,
                        Some: _ => toContinue = true));
            } while (toContinue);

            return either;
        }
    }
}
