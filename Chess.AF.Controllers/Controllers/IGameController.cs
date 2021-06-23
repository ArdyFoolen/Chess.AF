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

namespace Chess.AF.Controllers
{
    public interface IGameController
    {
        void Register(IBoardView view);
        void UnRegister(IBoardView view);
        Option<PieceOnSquare<PiecesEnum>> this[int index]
        {
            get;
        }

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
        Move LastMove { get; }
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
        void Select(int square);
        void Promote(int moveSquare, int piece);
        string ToFenString();
    }
}
