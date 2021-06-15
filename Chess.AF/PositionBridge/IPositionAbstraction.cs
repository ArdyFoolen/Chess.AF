using AF.Functional;
using Chess.AF.Dto;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.PositionBridge.PositionAbstraction;

namespace Chess.AF.PositionBridge
{
    public interface IPositionAbstraction
    {
        bool IsWhiteToMove { get; }
        RokadeEnum Rokade { get; }
        Option<SquareEnum> EpSquare { get; }

        GameResult Result { get; }
        bool IsMate { get; }
        bool IsStaleMate { get; }
        bool IsInCheck { get; }

        Option<IPositionAbstraction> Resign();
        Option<IPositionAbstraction> Draw();
        Option<IPositionAbstraction> Move(Move move);

        IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> IterateForAllMoves();
        PiecesIterator<T> GetIteratorForAll<T>() where T : Enum;

        string ToFenString();
        Dictionary<int, PieceOnSquare<PiecesEnum>> ToDictionary();
    }
}
