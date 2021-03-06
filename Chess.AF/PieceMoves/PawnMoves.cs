﻿using AF.Functional;
using Chess.AF.Dto;
using Chess.AF.Enums;
using Chess.AF.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.AF.Domain.Board;

namespace Chess.AF.PieceMoves
{
    internal class PawnMoves : Moves
    {
        private static PawnMoves instance = null;
        private PawnMoves() { }

        public static PawnMoves Get()
        {
            if (instance == null)
                instance = new PawnMoves();
            return instance;
        }

        public PiecesIterator<PieceEnum> GetIteratorFor(SquareEnum square, IBoardMap boardMap, PieceEnum pieceEnum = PieceEnum.Pawn)
            => new PiecesIterator<PieceEnum>(new PieceMap<PieceEnum>(pieceEnum, GetMapFor(boardMap, square)));

        // Make a map for black/white pawns, later set bits of either below or above square
        private ulong GetMapFor(IBoardMap boardMap, SquareEnum square)
        {
            var mvMap = 0x0000000000000000ul;

            if (square.Row() < 7)
                mvMap = mvMap.SetBit((int)square + 8);
            if (square.Row() > 0)
                mvMap = mvMap.SetBit((int)square - 8);
            if (square.Row() == 1)
                mvMap = mvMap.SetBit((int)square + 16);
            if (square.Row() == 6)
                mvMap = mvMap.SetBit((int)square - 16);

            ulong tkMap = MovesDictionaries.GetTakeMap(square);

            return boardMap.GetPawnMapFor(square, mvMap, tkMap);
        }
    }
}
