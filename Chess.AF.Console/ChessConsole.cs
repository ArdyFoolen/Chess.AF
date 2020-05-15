using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Chess.AF.Console
{
    public static class ChessConsole
    {
        #region UniCodes

        private static readonly char[] ucodesTop = new char[]
        {
                (char)0x250c,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x252c,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x252c,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x252c,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x252c,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x252c,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x252c,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x252c,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x2510
        };
        private static readonly char ucodeVertical = (char)0x2502;
        private static readonly char[] ucodesMiddle = new char[]
        {
                (char)0x251c,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x253c,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x253c,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x253c,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x253c,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x253c,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x253c,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x253c,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x2524
        };
        private static readonly char[] ucodesBottom = new char[]
        {
                (char)0x2514,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x2534,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x2534,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x2534,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x2534,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x2534,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x2534,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x2534,
                (char)0x2500,
                (char)0x2500,
                (char)0x2500,
                (char)0x2518
        };

        #endregion

        public static char[] PiecesCharEnum = new char[]
        {
            ' ', 'p', 'n', 'b', 'r', 'q', 'k',
            ' ', 'P', 'N', 'B', 'R', 'Q', 'K'
        };

        private static void WriteFrame(char[] frame)
        {
            foreach (char c in frame)
                Write(c);
            WriteLine();
        }

        public static void WriteError(string error)
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine(error);
            ForegroundColor = ConsoleColor.White;
        }

        public static void ShowHelp(IDictionary<string, (string Description, Action<string[]> Action)> cmds)
        {
            WriteLine("Help on commands");
            WriteLine("----------------");
            foreach (string key in cmds.Keys)
                WriteLine($"{key}: {cmds[key].Description}");
        }

        public static Action<SquareEnum> WriteInfo = null;

        public static void WriteBoard(IDictionary<SquareEnum, (PiecesEnum Piece, SquareEnum Square, bool IsSelected)> dictionary)
        {
            WriteFrame(ucodesTop);
            for (int i = 0; i < 64; i++)
            {
                Write($"{ucodeVertical}");
                WritePiece(ConvertPieceToChar(dictionary, (SquareEnum)i), IsSelected(dictionary, (SquareEnum)i));
                if (i % 8 == 7)
                {
                    whiteBackground = !whiteBackground;
                    Write(ucodeVertical);

                    WriteInfo?.Invoke((SquareEnum)i);

                    WriteLine();
                    if (i != 63)
                        WriteFrame(ucodesMiddle);
                }
            }
            WriteFrame(ucodesBottom);
        }

        //public static void WriteBoard(char[] fields)
        //{
        //    WriteFrame(ucodesTop);
        //    for (int i = 0; i < fields.Length; i++)
        //    {
        //        Write($"{ucodeVertical}");
        //        WritePiece(fields[i]);
        //        if (i % 8 == 7)
        //        {
        //            whiteBackground = !whiteBackground;
        //            WriteLine(ucodeVertical);
        //            if (i != 63)
        //                WriteFrame(ucodesMiddle);
        //        }
        //    }
        //    WriteFrame(ucodesBottom);
        //}

        private static bool whiteBackground = true;
        private static void WritePiece(char piece, bool isSelected = false)
        {
            if (isSelected)
                BackgroundColor = ConsoleColor.Red;
            else if (whiteBackground)
                BackgroundColor = ConsoleColor.White;

            if (char.IsUpper(piece))
                ForegroundColor = ConsoleColor.DarkYellow;
            else
                ForegroundColor = ConsoleColor.DarkCyan;

            Write($" {piece} ");
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.White;
            whiteBackground = !whiteBackground;
        }
        //private static void WritePiece(PiecesEnum piece)
        //{
        //    if (whiteBackground)
        //    {
        //        BackgroundColor = ConsoleColor.White;
        //        ForegroundColor = ConsoleColor.Black;
        //    }
        //    Write($" {PiecesCharEnum[(int)piece]} ");
        //    BackgroundColor = ConsoleColor.Black;
        //    ForegroundColor = ConsoleColor.White;
        //    whiteBackground = !whiteBackground;
        //}

        private static bool IsSelected(IDictionary<SquareEnum, (PiecesEnum Piece, SquareEnum Square, bool IsSelected)> dictionary, SquareEnum square)
            => dictionary.ContainsKey(square) ? dictionary[square].IsSelected : false;
        private static char ConvertPieceToChar(IDictionary<SquareEnum, (PiecesEnum Piece, SquareEnum Square, bool IsSelected)> dictionary, SquareEnum square)
            => ConvertPieceToChar(dictionary.ContainsKey(square) ? (int)dictionary[square].Piece : 0);
        public static char ConvertPieceToChar(int index)
            => PiecesCharEnum[index];
    }
}
