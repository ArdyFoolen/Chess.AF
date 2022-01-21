using AF.Functional;
using Chess.AF.Views;
using Chess.AF.Dto;
using Chess.AF.Enums;
using Chess.AF.ImportExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.AF.Domain;

namespace Chess.AF.Controllers
{
    public interface IGameController : ISquareController
    {
        IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> SelectedMoves
        {
            get;
        }

        void SetFromPgn(Option<Pgn> pgn);

        bool IsWhiteToMove { get; }
        bool IsMate { get; }
        bool IsInCheck { get; }
        bool IsStaleMate { get; }

        GameResult Result { get; }
        int MoveNumber { get; }
        int PlyCount { get; }
        Option<Move> LastMove { get; }
        int MaterialCount { get; }

        void GotoFirstMove();
        void GotoPreviousMove();
        void GotoNextMove();
        void GotoLastMove();

        void Resign();
        void Draw();

        bool IsPromoteMove(int square);

        void LoadFen();
        void LoadFen(string fen);
        void Promote(int moveSquare, int piece);
        Option<Pgn> Export();

        void UseLoosePiecesIterator(bool on, FilterFlags flags = FilterFlags.Both);
        IEnumerable<SquareEnum> LoosePieceSquares { get; }

        string ToFenString();
    }
}
