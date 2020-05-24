using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF
{
    public class Game
    {
        private static readonly string DefaultFen = @"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        private Option<Position> Position;

        public void Load()
            => Load(DefaultFen);

        public void Load(string fenString)
            => Position = fenString.CreateFen().CreatePosition();

        public void Move((PieceEnum Piece, SquareEnum From, PieceEnum Promote, SquareEnum To, RokadeEnum Rokade) Move)
            => Position = Position.Bind(p => RokadeEnum.None.Equals(Move.Rokade) ?
                                            p.Move(Move.Piece, Move.From, Move.Promote, Move.To) :
                                            p.Move(Move.Piece, Move.Rokade))
            .Match(None: () => Position,
                    Some: s => s);

        public Option<Selected> SelectPiece(Option<PieceEnum> piece)
            => piece.Bind(pc => Position.Bind(p => Selected.Of(pc, p.GetIteratorFor(pc), p)));

        public string ToFenString()
            => Position.Match(
                None: () => "No Game loaded or Invalid Game",
                Some: p => p.ToFenString());

        public void ForEachMove(Action<IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>> iterator)
            => Position.Map(p => p.IterateForAllMoves()).ForEach(iterator);

        public void Map(Func<Position, Position> func)
            => Position = Position.Map(func);
    }
}
