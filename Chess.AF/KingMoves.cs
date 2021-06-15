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

namespace Chess.AF
{
    public class KingMoves : Moves
    {
        private static KingMoves instance = null;
        private KingMoves() { }

        public static KingMoves Get()
        {
            if (instance == null)
                instance = new KingMoves();
            return instance;
        }

        public PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, IPositionImpl position, PieceEnum pieceEnum = PieceEnum.King)
            => new PiecesIterator<PieceEnum>(new PieceMap<PieceEnum>(pieceEnum, position.IncludeRokade(position.ExcludeOpponentKing(position.ExcludeOwnPieces(MovesDictionaries.KingMovesDictionary[square])))));
    }
}
