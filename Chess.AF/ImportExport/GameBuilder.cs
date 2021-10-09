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
        #region Filters

        private readonly Func<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare), bool> DefaultFunc = t => true;

        private Func<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare), bool> fileFilter;
        private Func<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare), bool> rowFilter;
        private Func<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare), bool> promoteFilter;

        #endregion

        #region Public properties

        public PieceEnum Piece { get; private set; }
        public IGame Game { get; private set; }


        #endregion

        public GameBuilder(IGame game)
        {
            Game = game;
        }

        #region Load

        public GameBuilder WithDefault()
        {
            fileFilter = rowFilter = promoteFilter = DefaultFunc;
            Game.Load();
            return this;
        }

        public GameBuilder WithFen(string fen)
        {
            fileFilter = rowFilter = promoteFilter = DefaultFunc;
            Game.Load(fen);
            return this;
        }

        #endregion

        #region Result

        public GameBuilder With(GameResult result)
        {
            if (!Game.IsMate && !Game.IsStaleMate && !GameResult.Ongoing.Equals(result))
                if (GameResult.Draw.Equals(result))
                    Game.Draw();
                else if (GameResult.BlackWins.Equals(result) || GameResult.WhiteWins.Equals(result))
                    Game.Resign();
            return this;
        }

        #endregion

        #region Move

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
            promoteFilter = t => t.Promoted.Is(piece);
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
                .Where(promoteFilter)
                .Where(t => t.MoveSquare.Equals(square))
                .FirstOrDefault();

            Dto.Move.Of(Piece, moves.Square, moves.MoveSquare, moves.Promoted)
                .Map(m => Move(m));

            fileFilter = rowFilter = promoteFilter = DefaultFunc;
            return this;
        }

        private Unit Move(Dto.Move move)
        {
            Game.Move(move);
            return Unit();
        }

        #endregion
    }
}
