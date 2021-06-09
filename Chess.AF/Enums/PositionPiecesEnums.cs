using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Enums
{
    internal enum PositionEnum
    {
        BlackPieces,    // 0
        BlackPawns,     // 1
        BlackKnights,   // 2
        BlackBishops,   // 3
        BlackRooks,     // 4
        BlackQueens,    // 5
        BlackKing,      // 6
        WhitePieces,    // 7
        WhitePawns,     // 8
        WhiteKnights,   // 9
        WhiteBishops,   // 10
        WhiteRooks,     // 11
        WhiteQueens,    // 12
        WhiteKing       // 13
    }

    public enum PiecesEnum
    {
        BlackPawn = 1,
        BlackKnight,
        BlackBishop,
        BlackRook,
        BlackQueen,
        BlackKing,
        WhitePawn = 8,
        WhiteKnight,
        WhiteBishop,
        WhiteRook,
        WhiteQueen,
        WhiteKing
    }

    [DataContract(Name = "MoveCondition")]
    public enum PieceEnum
    {
        [EnumMember]
        Pawn = 1,      // 1 8
        [EnumMember]
        Knight,        // 2 9
        [EnumMember]
        Bishop,        // 3 10
        [EnumMember]
        Rook,          // 4 11
        [EnumMember]
        Queen,         // 5 12
        [EnumMember]
        King            // 6 13
    }

    internal enum BlackPiecesEnum
    {
        BlackPawns = 1,
        BlackKnights,
        BlackBishops,
        BlackRooks,
        BlackQueens,
        BlackKing
    }

    internal enum WhitePiecesEnum
    {
        WhitePawns = 8,
        WhiteKnights,
        WhiteBishops,
        WhiteRooks,
        WhiteQueens,
        WhiteKing
    }

    [DataContract(Name = "MoveCondition")]
    public enum RokadeEnum : byte
    {
        [EnumMember]
        None,
        [EnumMember]
        KingSide,
        [EnumMember]
        QueenSide,
        [EnumMember]
        KingAndQueenSide
    }

    public enum SquareEnum
    {
        a8, b8, c8, d8, e8, f8, g8, h8, // c6 5088008850000000 a6 4020002040000000 g1 0000000000050800 f6 0a1100110a000000 h6 0204000402000000
        a7, b7, c7, d7, e7, f7, g7, h7, // c7 8800885000000000 a7 2000204000000000 g2 0000000005080008 f7 1100110a00000000 h7 0400040200000000
        a6, b6, c6, d6, e6, f6, g6, h6, // c8 0088500000000000 a8 0020400000000000 g3 0000000508000805 f8 00110a0000000000 h8 0004020000000000
        a5, b5, c5, d5, e5, f5, g5, h5, // b6 a0100010a0000000 f1 00000000000a1100 h1 0000000000020400 g6 0508000805000000
        a4, b4, c4, d4, e4, f4, g4, h4, // b7 100010a000000000 f2 0000000006110011 h2 0000000002040004 g7 0800080500000000
        a3, b3, c3, d3, e3, f3, g3, h3, // b8 0010a00000000000 f3 0000000611001106 h3 0000000204000402 g8 0008050000000000
        a2, b2, c2, d2, e2, f2, g2, h2,
        a1, b1, c1, d1, e1, f1, g1, h1
    }

    public enum GameResult
    {
        [Display(Name = "*")]
        Ongoing,
        [Display(Name = "1-0")]
        WhiteWins,
        [Display(Name = "0-1")]
        BlackWins,
        [Display(Name = "1/2-1/2")]
        Draw,
        Invalid
    }
}
