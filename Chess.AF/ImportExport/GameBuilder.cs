using AF.Functional;
using Chess.AF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;
using Unit = System.ValueTuple;

namespace Chess.AF.ImportExport
{
    public class GameBuilder : IGameBuilder
    {
        public IGame Game { get; private set; }
        private Func<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare), bool> fileFilter;
        private Func<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare), bool> rowFilter;
        private Func<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare), bool> promote;
        private readonly Func<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare), bool> DefaultFunc = t => true;
        public PieceEnum Piece { get; private set; }

        public GameBuilder(IGame game)
        {
            Game = game;
        }

        public GameBuilder WithDefault()
        {
            fileFilter = rowFilter = promote = DefaultFunc;
            Game.Load();
            return this;
        }

        public GameBuilder WithFen(string fen)
        {
            fileFilter = rowFilter = promote = DefaultFunc;
            Game.Load(fen);
            return this;
        }

        public GameBuilder With(PieceEnum piece)
        {
            this.Piece = piece;
            return this;
        }

        public GameBuilder WithMoveFromFile(int file)
        {
            fileFilter = t => t.Square.File() == file;
            return this;
        }

        public GameBuilder WithMoveFromRow(int row)
        {
            rowFilter = t => t.Square.Row() == row;
            return this;
        }

        public GameBuilder WithPromote(PieceEnum piece)
        {
            promote = t => t.Promoted.Is(piece);
            return this;
        }

        public GameBuilder With(RokadeEnum rokade)
        {
            var move = Dto.Move.Of(rokade);
            move.Map(m => Move(m));
            return this;
        }

        public GameBuilder WithMoveTo(SquareEnum square)
        {
            var moves = Game.AllMoves()
                .Where(w => w.Piece.Is(Piece))
                .Where(fileFilter)
                .Where(rowFilter)
                .Where(promote)
                .Where(t => t.MoveSquare.Equals(square))
                .FirstOrDefault();
            var move = Dto.Move.Of(Piece, moves.Square, moves.MoveSquare, moves.Promoted);
            move.Map(m => Move(m));
            fileFilter = rowFilter = promote = DefaultFunc;
            return this;
        }

        public GameBuilder With(GameResult result)
        {
            if (!Game.IsMate && !Game.IsStaleMate && !GameResult.Ongoing.Equals(result))
                if (GameResult.Draw.Equals(result))
                    Game.Draw();
                else if (GameResult.BlackWins.Equals(result) || GameResult.WhiteWins.Equals(result))
                    Game.Resign();
            return this;
        }

        private Unit Move(Dto.Move move)
        {
            Game.Move(move);
            return Unit();
        }

    }
}
