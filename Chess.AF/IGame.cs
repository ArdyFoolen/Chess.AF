using AF.Functional;
using Chess.AF.Domain;
using Chess.AF.Dto;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF
{
    public interface IGame
    {
        bool IsLoaded { get; }
        bool IsWhiteToMove { get; }
        bool IsMate { get; }
        bool IsInCheck { get; }
        bool IsStaleMate { get; }
        GameResult Result { get; }
        Option<Move> LastMove { get; }
        int MaterialCount { get; }

        int MoveNumber { get; }

        void Load();
        void Load(string fenString);

        void Move(Move move);
        void Resign();
        void Draw();

        void GotoFirstMove();
        void GotoPreviousMove();
        void GotoNextMove();
        void GotoLastMove();

        IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> AllMoves();
        void Map(Func<IBoard, IBoard> func);

        string ToFenString();
    }
}
