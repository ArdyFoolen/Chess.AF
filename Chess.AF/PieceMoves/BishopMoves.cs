﻿using AF.Functional;
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
    internal class BishopMoves : Moves
    {
        private static BishopMoves instance = null;
        private BishopMoves() { }

        public static BishopMoves Get()
        {
            if (instance == null)
                instance = new BishopMoves();
            return instance;
        }

        public PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, IPositionImpl position, PieceEnum pieceEnum = PieceEnum.Bishop)
            => new PiecesIterator<PieceEnum>(new PieceMap<PieceEnum>(pieceEnum, MovesDictionaries.GetBishopMovesMapFor(position, square)));
    }
}