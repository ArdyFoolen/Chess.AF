using AF.Functional;
using Chess.AF.Dto;
using Chess.AF.Enums;
using Chess.AF.PositionBridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.PositionBridge.PositionAbstraction;

namespace Chess.AF.PieceMoves
{
    internal class KnightMoves : Moves
    {
        private static KnightMoves instance = null;
        private KnightMoves() { }

        public static KnightMoves Get()
        {
            if (instance == null)
                instance = new KnightMoves();
            return instance;
        }

        public PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, IPositionImpl position, PieceEnum pieceEnum = PieceEnum.Knight)
            => new PiecesIterator<PieceEnum>(new PieceMap<PieceEnum>(pieceEnum, position.ExcludeOwnPieces(MovesDictionaries.KnightMovesDictionary[square])));
    }
}
