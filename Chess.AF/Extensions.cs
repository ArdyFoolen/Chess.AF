using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AF.Functional;
using Chess.AF.Dto;
using Chess.AF.Enums;
using Chess.AF.Exceptions;
using Chess.AF.ImportExport;
using Chess.AF.PositionBridge;
using Microsoft.SqlServer.Server;

namespace Chess.AF
{
    using static F;

    public static class Extensions
    {
        public static Either<string, Option<Fen>> CreateFen(this Either<string, string> fenString)
            => fenString.Map(f => Fen.Of(f));

        public static Option<Fen> CreateFen(this string fenString)
            => Fen.Of(fenString);

        public static Option<IBoard> CreatePositionAbstraction(this Option<Fen> fen)
            => Board.Of(fen);

        internal static int Value(this PositionEnum position) { return (int)position; }

        public static bool IsEqual(this PiecesEnum piecesEnum, PieceEnum pieceEnum)
            => ((int)pieceEnum) == ((int)piecesEnum) || ((int)pieceEnum + 7) == ((int)piecesEnum);

        public static bool IsEqual(this PieceEnum pieceEnum, PiecesEnum piecesEnum)
            => piecesEnum.IsEqual(pieceEnum);

        public static bool IsEqual(this PieceEnum pieceEnum, PieceEnum piecesEnum)
            => piecesEnum == pieceEnum;

        public static bool IsEqual<T>(this PieceEnum pieceEnum, T piecesEnum) where T : Enum
            => (piecesEnum is PiecesEnum)
                ? pieceEnum.IsEqual((PiecesEnum)(object)piecesEnum)
                : pieceEnum.IsEqual((PieceEnum)(object)piecesEnum);
        public static PiecesEnum ToPieces(this PieceEnum pieceEnum, bool isWhiteToMove)
            => (PiecesEnum)(isWhiteToMove ? ((int)pieceEnum + 7) : (int)pieceEnum);

        public static SquareEnum GetKingRokadeSquare(this RokadeEnum rokade, bool isWhiteToMove)
        {
            if (RokadeEnum.KingSide.Equals(rokade))
                return isWhiteToMove ? SquareEnum.g1 : SquareEnum.g8;
            else
                return isWhiteToMove ? SquareEnum.c1 : SquareEnum.c8;
        }

        public static (SquareEnum From, SquareEnum To) GetRookRokadeSquares(this SquareEnum kingMoveSquare, bool isWhiteToMove)
        {
            if (kingMoveSquare.File() == 6)
                return (isWhiteToMove ? SquareEnum.h1 : SquareEnum.h8, (SquareEnum)((int)kingMoveSquare - 1));
            else
                return (isWhiteToMove ? SquareEnum.a1 : SquareEnum.a8, (SquareEnum)((int)kingMoveSquare + 1));
        }

        public static bool Is(this PieceEnum pieceEnum, PieceEnum compare)
            => pieceEnum == compare;

        public static bool Is(this PiecesEnum pieceEnum, PiecesEnum compare)
            => pieceEnum == compare;

        private static readonly Dictionary<string, PieceEnum> promoteDict = new Dictionary<string, PieceEnum>()
        {
            { "N", PieceEnum.Knight },
            { "B", PieceEnum.Bishop },
            { "R", PieceEnum.Rook },
            { "Q", PieceEnum.Queen }
        };
        private static readonly Dictionary<string, PieceEnum> pieceDict = new Dictionary<string, PieceEnum>()
        {
            { "N", PieceEnum.Knight },
            { "B", PieceEnum.Bishop },
            { "R", PieceEnum.Rook },
            { "Q", PieceEnum.Queen },
            { "K", PieceEnum.King }
        };

        public static Option<PieceEnum> ToPiece(this string piece)
            => pieceDict.ContainsKey(piece) ? Some(pieceDict[piece]) : None;

        // A8: 0 / 8 = 0
        // H8: 7 / 8 = 0;
        // A1: 56 / 8 = 7;
        // H1: 63 / 8 = 7;
        public static int Row(this SquareEnum square)
            => (int)square / 8;

        // A8: 0 % 8 = 0
        // H8: 7 % 8 = 7;
        // A1: 56 % 8 = 0;
        // H1: 63 % 8 = 7;
        public static int File(this SquareEnum square)
            => (int)square % 8;

        internal static void SetBit(this ulong[] maps, int position, int index)
            => maps[position] = maps[position].SetBit(index);

        public static ulong SetBit(this ulong ul, int index)
        {
            ulong im = 1ul << (63 - index);
            ul |= im;
            return ul;
        }

        public static ulong SetBitOff(this ulong ul, int index)
            => ul &= ~SetBit(0x0ul, index);

        public static bool IsBitOn(this ulong ul, int index)
            => (0x0ul.SetBit(index) & ul) != 0x0ul; 

        private static readonly ulong f1Map = 0x7f7f7f7f7f7f7f7f;
        private static readonly ulong r1Map = 0x00ffffffffffffff;

        public static ulong BitOffForFile(this ulong map, int file)
            => map & ~(~f1Map >> file);

        public static ulong BitOffForRow(this ulong map, int row)
            => map & ~(~r1Map >> (row * 8));

        public static ulong UpBitsOn(this SquareEnum square, int add=0)
            => 0xfffffffffffffffeul << (63 - ((int)square + add));

        public static ulong DownBitsOn(this SquareEnum square, int sub=0)
            => 0x7ffffffffffffffful >> ((int)square - sub);

        public static ulong UpBitsOff(this SquareEnum square, int add=0)
            => ~square.UpBitsOn(add);

        public static ulong DownBitsOff(this SquareEnum square, int sub=0)
            => ~square.DownBitsOn(sub);

        public static void SetToMaps(this char piece, ulong[] maps, int index)
        {
            maps.SetBit(piece.ColorIndex(), index);
            maps.SetBit(piece.PieceIndex(), index);
        }

        public static int ColorIndex(this char piece)
            => char.IsUpper(piece) ? PositionEnum.WhitePieces.Value() : PositionEnum.BlackPieces.Value();

        private static readonly Dictionary<char, PositionEnum> directory = new Dictionary<char, PositionEnum>()
        {
            { 'p', PositionEnum.BlackPawns },
            { 'n', PositionEnum.BlackKnights },
            { 'b', PositionEnum.BlackBishops },
            { 'r', PositionEnum.BlackRooks },
            { 'q', PositionEnum.BlackQueens },
            { 'k', PositionEnum.BlackKing },
            { 'P', PositionEnum.WhitePawns },
            { 'N', PositionEnum.WhiteKnights },
            { 'B', PositionEnum.WhiteBishops },
            { 'R', PositionEnum.WhiteRooks },
            { 'Q', PositionEnum.WhiteQueens },
            { 'K', PositionEnum.WhiteKing },
        };

        public static char[] PiecesCharEnum = new char[]
        {
            ' ', 'p', 'n', 'b', 'r', 'q', 'k',
            ' ', 'P', 'N', 'B', 'R', 'Q', 'K'
        };

        public static char ConvertPieceToChar(IDictionary<SquareEnum, PieceOnSquare<PiecesEnum>> dictionary, SquareEnum square)
            => ConvertPieceToChar(dictionary.ContainsKey(square) ? (int)dictionary[square].Piece : 0);
        //public static char ConvertPieceToChar(IDictionary<SquareEnum, (PiecesEnum Piece, SquareEnum Square, bool IsSelected)> dictionary, SquareEnum square)
        //    => ConvertPieceToChar(dictionary.ContainsKey(square) ? (int)dictionary[square].Piece : 0);
        public static char ConvertPieceToChar(int index)
            => PiecesCharEnum[index];

        public static char ConvertWhiteToMoveToChar(bool IsWhiteToMove)
            => IsWhiteToMove ? 'w' : 'b';

        public static string ConvertRokadeToString(RokadeEnum whiteRokade, RokadeEnum blackRokade)
        {
            string rokadeString = ConvertRokadeToString(whiteRokade, true) + ConvertRokadeToString(blackRokade, false);
            return "".Equals(rokadeString) ? "-" : rokadeString;
        }

        public static string ConvertRokadeToString(RokadeEnum rokade, bool IsWhite)
        {
            if (RokadeEnum.None.Equals(rokade))
                return "";
            string rokadeString = "";
            if (RokadeEnum.KingSide.Equals(RokadeEnum.KingSide & rokade))
                rokadeString = "k";
            if (RokadeEnum.QueenSide.Equals(RokadeEnum.QueenSide & rokade))
                rokadeString += "q";

            if (IsWhite)
                rokadeString = rokadeString.ToUpperInvariant();
            return rokadeString;
        }

        public static string ConvertEPToString(Option<SquareEnum> epSquare)
            => epSquare.Match(
                None: () => "-",
                Some: s => s.ToString());

        public static int PieceIndex(this char piece)
            => directory[piece].Value();

        private static readonly Dictionary<ulong, SquareEnum> MapToSquare = new Dictionary<ulong, SquareEnum>();
        static Extensions()
        {
            ulong map = 0x1ul;
            foreach (int i in Enumerable.Range(0, 64))
            {
                MapToSquare.Add(map, (SquareEnum)(63 - i));
                map *= 2;
            }
        }

        public static SquareEnum GetSquareFrom(this ulong map)
            => MapToSquare.ContainsKey(map) ? MapToSquare[map] : throw new MapNotFoundException();

        public static bool IsRokadeMove(this PieceEnum piece, SquareEnum square, SquareEnum moveSquare)
            => PieceEnum.King.Equals(piece) && Math.Abs((int)square - (int)moveSquare) == 2;

        public static bool IsQueensideRokadeMove(this PieceEnum piece, SquareEnum square, SquareEnum moveSquare)
            => piece.IsRokadeMove(square, moveSquare) && moveSquare.File() == 2;

        public static bool IsKingsideRokadeMove(this PieceEnum piece, SquareEnum square, SquareEnum moveSquare)
            => piece.IsRokadeMove(square, moveSquare) && moveSquare.File() == 6;

        public static Option<SquareEnum> ToSquare(this string square)
        {
            if (Enum.TryParse(square, out SquareEnum newSquare))
                return newSquare;
            return None;
        }

        public static GameResult ToGameResult(this string str)
        {
            foreach (GameResult gameResult in Enum.GetValues(typeof(GameResult)))
            {
                var dispAttr = gameResult.GetAttributeOfType<DisplayAttribute>();
                if (dispAttr != null && str.Equals(dispAttr.Name))
                    return gameResult;
            }
            return GameResult.Invalid;
        }

        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        /// <example><![CDATA[string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;]]></example>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        public static Option<PieceEnum> FromSanToPiece(this string str)
        {
            if (pieceDict.ContainsKey(str.FirstOrDefault().ToString()))
                return str.FirstOrDefault().ToString().ToPiece();
            else return Some(PieceEnum.Pawn);
        }

        public static PieceEnum FromSanToPromoted(this string str, Option<PieceEnum> piece)
            => piece.Match(
                None: () => PieceEnum.Pawn,
                Some: p => str.FromSanToPromoted(p));

        public static PieceEnum FromSanToPromoted(this string str, PieceEnum piece)
        {
            if (promoteDict.ContainsKey(str.LastOrDefault().ToString()))
                return str.FirstOrDefault().ToString().ToPiece().Match(
                    None: () => piece,
                    Some: s => s);
            else
                return piece;
        }
    }
}
