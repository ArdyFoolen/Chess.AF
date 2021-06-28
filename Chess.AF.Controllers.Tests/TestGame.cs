using AF.Functional;
using Chess.AF.Domain;
using Chess.AF.Dto;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Controllers.Tests
{
    public class TestGame : IGame
    {
        public bool IsLoaded => throw new NotImplementedException();

        public bool IsWhiteToMove => throw new NotImplementedException();

        public bool IsMate => throw new NotImplementedException();

        public bool IsInCheck => throw new NotImplementedException();

        public bool IsStaleMate => throw new NotImplementedException();

        public GameResult Result => throw new NotImplementedException();

        public Option<Move> LastMove => throw new NotImplementedException();

        public int MaterialCount => throw new NotImplementedException();

        public IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> AllMoves()
        {
            throw new NotImplementedException();
        }

        public void Draw()
        {
            throw new NotImplementedException();
        }

        public void GotoFirstMove()
        {
            throw new NotImplementedException();
        }

        public void GotoLastMove()
        {
            throw new NotImplementedException();
        }

        public void GotoNextMove()
        {
            throw new NotImplementedException();
        }

        public void GotoPreviousMove()
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Load(string fenString)
        {
            throw new NotImplementedException();
        }

        public void Map(Func<IBoard, IBoard> func)
        {
            throw new NotImplementedException();
        }

        public void Move(Move move)
        {
            throw new NotImplementedException();
        }

        public void Resign()
        {
            throw new NotImplementedException();
        }

        public string ToFenString()
        {
            throw new NotImplementedException();
        }
    }
}
