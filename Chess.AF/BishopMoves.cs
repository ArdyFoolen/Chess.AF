﻿using AF.Functional;
using Chess.AF.Enums;
using Chess.AF.PositionBridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.Position;

namespace Chess.AF
{
    public class BishopMoves : Moves
    {
        private static BishopMoves instance = null;
        private BishopMoves() { }

        public static BishopMoves Get()
        {
            if (instance == null)
                instance = new BishopMoves();
            return instance;
        }

        public Position.PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, Option<Position> position, PieceEnum pieceEnum = PieceEnum.Bishop)
            => position.Match(
                None: () => new PiecesIterator<PieceEnum>((pieceEnum, 0ul)),
                Some: p => new PiecesIterator<PieceEnum>((pieceEnum, MovesDictionaries.GetBishopMovesMapFor(p, square)))
                );

        public Position.PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, IPositionImpl position, bool isWhiteToMove, PieceEnum pieceEnum = PieceEnum.Bishop)
            => new PiecesIterator<PieceEnum>((pieceEnum, MovesDictionaries.GetBishopMovesMapFor(position, square, isWhiteToMove)));
    }
}
