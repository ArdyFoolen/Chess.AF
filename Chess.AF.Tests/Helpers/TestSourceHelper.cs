﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Tests.Helpers
{
    public class TestSourceHelper
    {
        public static IEnumerable<(string FenString, (PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)[] Expected)> MovesTestCases
        {
            get
            {
                yield return ("4k3/8/8/8/7b/8/8/4K1B1 w - - 0 1",
                    new (PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)[]
                    {
                        (PieceEnum.Bishop, SquareEnum.g1, PieceEnum.Bishop, SquareEnum.f2),
                        (PieceEnum.King, SquareEnum.e1, PieceEnum.King, SquareEnum.d1),
                        (PieceEnum.King, SquareEnum.e1, PieceEnum.King, SquareEnum.e2),
                        (PieceEnum.King, SquareEnum.e1, PieceEnum.King, SquareEnum.d2),
                        (PieceEnum.King, SquareEnum.e1, PieceEnum.King, SquareEnum.f1)
                    });
                yield return ("4k1b1/8/8/7B/8/8/8/4K3 b - - 0 1",
                    new (PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)[]
                    {
                        (PieceEnum.Bishop, SquareEnum.g8, PieceEnum.Bishop, SquareEnum.f7),
                        (PieceEnum.King, SquareEnum.e8, PieceEnum.King, SquareEnum.d8),
                        (PieceEnum.King, SquareEnum.e8, PieceEnum.King, SquareEnum.e7),
                        (PieceEnum.King, SquareEnum.e8, PieceEnum.King, SquareEnum.d7),
                        (PieceEnum.King, SquareEnum.e8, PieceEnum.King, SquareEnum.f8)
                    });
                yield return ("rnb1kbnr/pppp1ppp/8/4p3/5P1q/8/PPPPP1PP/RNBQKBNR w KQkq - 0 1",
                    new (PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)[]
                    {
                        (PieceEnum.Pawn, SquareEnum.g2, PieceEnum.Pawn, SquareEnum.g3)
                    });
                yield return ("rnbqkbnr/ppppp1pp/8/5p1Q/4P3/8/PPPP1PPP/RNB1KBNR b KQkq - 0 1",
                    new (PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)[]
                    {
                        (PieceEnum.Pawn, SquareEnum.g7, PieceEnum.Pawn, SquareEnum.g6)
                    });
            }
        }

        public static IEnumerable<(string FenString, RokadeEnum Expected, (PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare) MoveTo)> RokadeTestCases
        {
            get
            {
                yield return ("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1",
                    RokadeEnum.None,
                    (PieceEnum.King, SquareEnum.e1, PieceEnum.King, SquareEnum.e2));
                yield return ("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1",
                    RokadeEnum.None,
                    (PieceEnum.King, SquareEnum.e8, PieceEnum.King, SquareEnum.e7));
                yield return ("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1",
                    RokadeEnum.KingSide,
                    (PieceEnum.Rook, SquareEnum.a1, PieceEnum.Rook, SquareEnum.a2));
                yield return ("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1",
                    RokadeEnum.KingSide,
                    (PieceEnum.Rook, SquareEnum.a8, PieceEnum.Rook, SquareEnum.a7));
                yield return ("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1",
                    RokadeEnum.QueenSide,
                    (PieceEnum.Rook, SquareEnum.h1, PieceEnum.Rook, SquareEnum.h2));
                yield return ("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1",
                    RokadeEnum.QueenSide,
                    (PieceEnum.Rook, SquareEnum.h8, PieceEnum.Rook, SquareEnum.h7));
                yield return ("r3k2r/8/8/8/8/8/8/R3K2R w Qq - 0 1",
                    RokadeEnum.None,
                    (PieceEnum.Rook, SquareEnum.a1, PieceEnum.Rook, SquareEnum.a2));
                yield return ("r3k2r/8/8/8/8/8/8/R3K2R b Qq - 0 1",
                    RokadeEnum.None,
                    (PieceEnum.Rook, SquareEnum.a8, PieceEnum.Rook, SquareEnum.a7));
                yield return ("r3k2r/8/8/8/8/8/8/R3K2R w Kk - 0 1",
                    RokadeEnum.None,
                    (PieceEnum.Rook, SquareEnum.h1, PieceEnum.Rook, SquareEnum.h2));
                yield return ("r3k2r/8/8/8/8/8/8/R3K2R b Kk - 0 1",
                    RokadeEnum.None,
                    (PieceEnum.Rook, SquareEnum.h8, PieceEnum.Rook, SquareEnum.h7));
            }
        }
    }
}
