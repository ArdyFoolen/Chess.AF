using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.Position;

namespace Chess.AF
{
    public class KnightMoves : Moves
    {
        private static readonly ulong c6Map = 0x5088008850000000;
        private IDictionary<SquareEnum, ulong> MovesDictionary = new Dictionary<SquareEnum, ulong>();

        private static KnightMoves instance = null;
        private KnightMoves()
        {
            CreateMovesMap();
        }

        public static KnightMoves Get()
        {
            if (instance == null)
                instance = new KnightMoves();
            return instance;
        }

        private void CreateMovesMap()
        {
            foreach (SquareEnum square in Enum.GetValues(typeof(SquareEnum)))
            {
                ulong cpMap = c6Map;
                if (square.File() < 2)
                    cpMap = cpMap.BitOffForFile(0);
                if (square.File() < 1)
                    cpMap = cpMap.BitOffForFile(1);
                if (square.File() > 5)
                    cpMap = cpMap.BitOffForFile(4);
                if (square.File() > 6)
                    cpMap = cpMap.BitOffForFile(3);
                if (square.Row() < 2)
                    cpMap = cpMap.BitOffForRow(0);
                if (square.Row() < 1)
                    cpMap = cpMap.BitOffForRow(1);
                if (square.Row() > 5)
                    cpMap = cpMap.BitOffForRow(4);
                if (square.Row() > 6)
                    cpMap = cpMap.BitOffForRow(3);

                var shift = ((int)square - (int)SquareEnum.c6);
                if (shift < 0)
                    cpMap <<= (shift * -1);
                else
                    cpMap >>= shift;

                MovesDictionary[square] = cpMap;
            }
        }

        public PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, Option<Position> position, PieceEnum pieceEnum = PieceEnum.Knight)
            => position.Match(
                None: () => new PiecesIterator<PieceEnum>((PieceEnum.Knight, 0ul)),
                Some: p => new PiecesIterator<PieceEnum>((PieceEnum.Knight, p.ExcludeOwnPieces(MovesDictionary[square])))
                );

        public PiecesIterator<PieceEnum> this[SquareEnum square]
        {
            get
            {
                return new PiecesIterator<PieceEnum>((PieceEnum.Knight, MovesDictionary[square]));
            }
        }
    }
}
