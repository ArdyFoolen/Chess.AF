using AF.Functional;
using Chess.AF.Dto;
using Chess.AF.Enums;
using Chess.AF.PositionBridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.PositionBridge.Board;

namespace Chess.AF.PieceMoves
{
    internal class RookMoves : Moves
    {
        private static RookMoves instance = null;
        private RookMoves() { }

        public static RookMoves Get()
        {
            if (instance == null)
                instance = new RookMoves();
            return instance;
        }

        public PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, IBoardMap position, PieceEnum pieceEnum = PieceEnum.Rook)
            => new PiecesIterator<PieceEnum>(new PieceMap<PieceEnum>(pieceEnum, MovesDictionaries.GetRookMovesMapFor(position, square)));
    }
}
