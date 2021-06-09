using AF.Functional;
using Chess.AF.Enums;
using System;
using System.Linq;

namespace Chess.AF.Console
{
    using static F;

    public static class Extensions
    {
        public static Either<string, string> AbortIf(this string s, Func<string, bool> f)
            => f(s) ? (Either<string, string>)Left(s) : Right(s);
        public static Either<string, Option<Command>> CreateCommand(this Either<string, string> cmd)
            => cmd.Map(f => Command.Of(f));

        public static string GetCommand(this string cmd)
            => cmd.Split(' ')[0];

        public static string[] GetParameters(this string cmd)
            => cmd.Split(' ').Skip(1).ToArray();

        public static Option<PieceEnum> TryPieceParse(this char piece)
            => piece.ToString().TryPieceParse();

        public static Option<PieceEnum> TryPieceParse(this string piece)
        {
            switch (piece.ToLowerInvariant())
            {
                case "p": return PieceEnum.Pawn;
                case "n": return PieceEnum.Knight;
                case "b": return PieceEnum.Bishop;
                case "r": return PieceEnum.Rook;
                case "q": return PieceEnum.Queen;
                case "k": return PieceEnum.King;
                default: return None;
            }
        }

        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
