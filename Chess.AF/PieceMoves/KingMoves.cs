using AF.Functional;
using Chess.AF.Dto;
using Chess.AF.Enums;
using Chess.AF.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.Domain.Board;

namespace Chess.AF.PieceMoves
{
    internal class KingMoves : Moves
    {
        private static KingMoves instance = null;
        private KingMoves() { }

        public static KingMoves Get()
        {
            if (instance == null)
                instance = new KingMoves();
            return instance;
        }

        public PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, IBoardMap boardMap, PieceEnum pieceEnum = PieceEnum.King)
            => new PiecesIterator<PieceEnum>(new PieceMap<PieceEnum>(pieceEnum, boardMap.IncludeRokade(boardMap.ExcludeOpponentKing(boardMap.ExcludeOwnPieces(MovesDictionaries.KingMovesDictionary[square])))));
    }
}
