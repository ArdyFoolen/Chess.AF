using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.Position;

namespace Chess.AF
{
    public class KingMoves : Moves
    {
        private static readonly ulong b7Map = 0xe0a0e00000000000;
        private IDictionary<SquareEnum, ulong> MovesDictionary = new Dictionary<SquareEnum, ulong>();

        private static KingMoves instance = null;
        private KingMoves()
        {
            CreateMovesMap();
        }

        public static KingMoves Get()
        {
            if (instance == null)
                instance = new KingMoves();
            return instance;
        }

        private void CreateMovesMap()
        {
            foreach (SquareEnum square in Enum.GetValues(typeof(SquareEnum)))
            {
                ulong cpMap = b7Map;
                if (square.File() < 1)
                    cpMap = cpMap.BitOffForFile(0);
                if (square.File() > 6)
                    cpMap = cpMap.BitOffForFile(2);
                if (square.Row() < 1)
                    cpMap = cpMap.BitOffForRow(0);
                if (square.Row() > 6)
                    cpMap = cpMap.BitOffForRow(2);

                var shift = ((int)square - (int)SquareEnum.b7);
                if (shift < 0)
                    cpMap <<= (shift * -1);
                else
                    cpMap >>= shift;

                MovesDictionary[square] = cpMap;
            }
        }

        public PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, Option<Position> position, PieceEnum pieceEnum = PieceEnum.King)
            => position.Match(
                None: () => new PiecesIterator<PieceEnum>((PieceEnum.King, 0ul)),
                Some: p => new PiecesIterator<PieceEnum>((PieceEnum.King, p.ExcludeOwnPieces(MovesDictionary[square])))
                );

        public PiecesIterator<PieceEnum> this[SquareEnum square]
        {
            get
            {
                return new PiecesIterator<PieceEnum>((PieceEnum.King, MovesDictionary[square]));
            }
        }
    }
}
