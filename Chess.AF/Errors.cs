using System;
using AF.Functional;
using Chess.AF.ImportExport;

namespace Chess.AF
{
    public static class Errors
    {
        public static Error FenError(Fen fen)
           => new FenError(fen);
    }

    public sealed class FenError : Error
    {
        Fen Fen { get; }
        public FenError(Fen fen) { Fen = fen; }

        public override string Message
           => $"Invalid Fen string '{Fen.FenString}'";
    }
}
