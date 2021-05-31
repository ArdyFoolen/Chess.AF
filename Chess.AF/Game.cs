using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF
{
    public class Game
    {
        private static readonly string DefaultFen = @"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        private Option<Position> Position = None;

        public void Load()
            => Load(DefaultFen);

        public void Load(string fenString)
            => Position = fenString.CreateFen().CreatePosition();

        public bool IsLoaded { get { return !Position.Equals(None); } }

        public void Move(Move move)
            => Position = Position.Bind(p => p.Move(move))
            .Match(None: () => Position,
                    Some: s => s);

        public Option<Selected> SelectPiece(Option<PieceEnum> piece)
            => piece.Bind(pc => Position.Bind(p => Selected.Of(pc, p.GetIteratorFor(pc), p)));

        public string ToFenString()
            => Position.Match(
                None: () => "No Game loaded or Invalid Game",
                Some: p => p.ToFenString());

        public Option<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> FindMove(
            Func<IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>,
                Option<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>> findIn)
            => Position.Map(p => p.IterateForAllMoves()).Bind(it => findIn(it));

        public void ForEachMove(Action<IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>> iterator)
            => Position.Map(p => p.IterateForAllMoves()).ForEach(iterator);

        public IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> AllMoves()
            => Position.Map(p => p.IterateForAllMoves()).Match(
                None: () => Enumerable.Empty<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>(),
                Some: s => s
                );

        public void Map(Func<Position, Position> func)
            => Position = Position.Map(func);

        public bool IsWhiteToMove
        {
            get => Position.Match(
                None: () => false,
                Some: s => s.IsWhiteToMove);
        }

        public bool IsMate
        {
            get => Position.Match(
                None: () => false,
                Some: s => s.IsMate);
        }

        public bool IsInCheck
        {
            get => Position.Match(
                None: () => false,
                Some: s => s.IsInCheck);
        }

        public bool IsStaleMate
        {
            get => Position.Match(
                None: () => false,
                Some: s => s.IsStaleMate);
        }
    }
}
