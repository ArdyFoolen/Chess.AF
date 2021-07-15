using AF.Functional;
using Chess.AF.Dto;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.Domain.Board;

namespace Chess.AF.Domain
{
    public interface IBoard
    {
        bool IsWhiteToMove { get; }
        RokadeEnum Rokade { get; }
        Option<SquareEnum> EpSquare { get; }

        GameResult Result { get; }
        bool IsMate { get; }
        bool IsStaleMate { get; }
        bool IsInCheck { get; }
        Option<Move> LastMove { get; }

        Option<IBoard> Resign();
        Option<IBoard> Draw();
        Option<IBoard> Move(Move move);

        IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> IterateForAllMoves();
        PiecesIterator<PieceEnum> GetIteratorFor(PieceEnum piece);
        PiecesIterator<T> GetIteratorForAll<T>() where T : Enum;

        int MoveNumber { get; }
        bool IsTake { get; }

        string ToFenString();
        Dictionary<int, PieceOnSquare<PiecesEnum>> ToDictionary();

        void Accept(IBoardMapVisitor visitor);
    }
}
