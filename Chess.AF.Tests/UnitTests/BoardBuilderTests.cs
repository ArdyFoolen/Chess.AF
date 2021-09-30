using Chess.AF.Domain;
using Chess.AF.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.Tests.UnitTests
{
    public class BoardBuilderTests
    {
        [Test]
        public void Build_NoKings_ShouldReturnNone()
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;
            builder
                .WithBlackRokade(RokadeEnum.None)
                .WithWhiteRokade(RokadeEnum.None);

            // Act
            var board = build.Build();

            // Assert
            Assert.IsTrue(board.Match(
                None: () => true,
                Some: b => false
                ));
        }

        [Test]
        public void Build_NoBlackKing_ShouldReturnNone()
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;
            builder.WithPiece(PiecesEnum.WhiteKing).On(SquareEnum.e1);
            builder
                .WithBlackRokade(RokadeEnum.None)
                .WithWhiteRokade(RokadeEnum.None);

            // Act
            var board = build.Build();

            // Assert
            Assert.IsTrue(board.Match(
                None: () => true,
                Some: b => false
                ));
        }

        [Test]
        public void Build_NoWhiteking_ShouldReturnNone()
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;
            builder
                .WithBlackRokade(RokadeEnum.None)
                .WithWhiteRokade(RokadeEnum.None)
                .WithPiece(PiecesEnum.BlackKing).On(SquareEnum.e8);

            // Act
            var board = build.Build();

            // Assert
            Assert.IsTrue(board.Match(
                None: () => true,
                Some: b => false
                ));
        }

        [Test]
        public void Build_Withkings_ShouldReturnBoard()
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;
            builder
                .WithBlackRokade(RokadeEnum.None)
                .WithWhiteRokade(RokadeEnum.None)
                .WithPiece(PiecesEnum.WhiteKing).On(SquareEnum.e1)
                .WithPiece(PiecesEnum.BlackKing).On(SquareEnum.e8);

            // Act
            var board = build.Build();

            // Assert
            Assert.IsTrue(board.Match(
                None: () => false,
                Some: b => true
                ));
        }

        [Test]
        public void Build_With2BlackKings_ShouldReturnBoardWith1BlackKing()
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;
            builder
                .WithBlackRokade(RokadeEnum.None)
                .WithWhiteRokade(RokadeEnum.None)
                .WithWhiteToMove(false)
                .WithPiece(PiecesEnum.WhiteKing).On(SquareEnum.e1)
                .WithPiece(PiecesEnum.BlackKing).On(SquareEnum.e8)
                .WithPiece(PiecesEnum.BlackKing).On(SquareEnum.e6);

            // Act
            var board = build.Build();

            // Assert
            Assert.IsTrue(board.Match(
                None: () => false,
                Some: b => true
                ));
            Assert.IsTrue(board.Match(
                None: () => false,
                Some: b => b.IterateForAllMoves()
                            .Where(t => PieceEnum.King.Is(t.Piece) && (SquareEnum.e8.Equals(t.Square) || SquareEnum.e6.Equals(t.Square)))
                            .All(a => SquareEnum.e6.Equals(a.Square))
                ));
        }

        [Test]
        public void Build_With2WhiteKings_ShouldReturnBoardWith1WhiteKing()
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;
            builder
                .WithBlackRokade(RokadeEnum.None)
                .WithWhiteRokade(RokadeEnum.None)
                .WithWhiteToMove(true)
                .WithPiece(PiecesEnum.WhiteKing).On(SquareEnum.e1)
                .WithPiece(PiecesEnum.BlackKing).On(SquareEnum.e8)
                .WithPiece(PiecesEnum.WhiteKing).On(SquareEnum.e3);

            // Act
            var board = build.Build();

            // Assert
            Assert.IsTrue(board.Match(
                None: () => false,
                Some: b => true
                ));
            Assert.IsTrue(board.Match(
                None: () => false,
                Some: b => b.IterateForAllMoves()
                            .Where(t => PieceEnum.King.Is(t.Piece) && (SquareEnum.e1.Equals(t.Square) || SquareEnum.e3.Equals(t.Square)))
                            .All(a => SquareEnum.e3.Equals(a.Square))
                ));
        }

        [Test]
        public void Build_WithNoRokade_ShouldReturnBoard()
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;
            builder
                .WithBlackRokade(RokadeEnum.None)
                .WithWhiteRokade(RokadeEnum.None)
                .WithPiece(PiecesEnum.WhiteKing).On(SquareEnum.e1)
                .WithPiece(PiecesEnum.BlackKing).On(SquareEnum.e8);

            // Act
            var board = build.Build();

            // Assert
            Assert.IsTrue(board.Match(
                None: () => false,
                Some: b => true
                ));
        }

        [TestCase(RokadeEnum.KingSide)]
        [TestCase(RokadeEnum.KingAndQueenSide)]
        public void Build_WithWhiteKingsideRokade_ShouldReturnBoard(RokadeEnum rokade)
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;
            builder
                .WithBlackRokade(RokadeEnum.None)
                .WithPiece(PiecesEnum.BlackKing).On(SquareEnum.e8)
                .WithWhiteRokade(rokade)
                .WithPiece(PiecesEnum.WhiteKing).On(SquareEnum.e1)
                .WithPiece(PiecesEnum.WhiteRook).On(SquareEnum.h1);

            if (rokade.IsQueensideRokade())
                builder.WithPiece(PiecesEnum.WhiteRook).On(SquareEnum.a1);

            // Act
            var board = build.Build();

            // Assert
            Assert.IsTrue(board.Match(
                None: () => false,
                Some: b => true
                ));
        }

        [TestCase(RokadeEnum.KingSide)]
        [TestCase(RokadeEnum.KingAndQueenSide)]
        public void Build_WithBlackKingsideRokade_ShouldReturnBoard(RokadeEnum rokade)
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;
            builder
                .WithBlackRokade(rokade)
                .WithPiece(PiecesEnum.BlackKing).On(SquareEnum.e8)
                .WithPiece(PiecesEnum.BlackRook).On(SquareEnum.h8)
                .WithWhiteRokade(RokadeEnum.None)
                .WithPiece(PiecesEnum.WhiteKing).On(SquareEnum.e1);

            if (rokade.IsQueensideRokade())
                builder.WithPiece(PiecesEnum.BlackRook).On(SquareEnum.a8);

            // Act
            var board = build.Build();

            // Assert
            Assert.IsTrue(board.Match(
                None: () => false,
                Some: b => true
                ));
        }

        [TestCase(RokadeEnum.QueenSide)]
        [TestCase(RokadeEnum.KingAndQueenSide)]
        public void Build_WithWhiteQueensideRokade_ShouldReturnBoard(RokadeEnum rokade)
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;
            builder
                .WithBlackRokade(RokadeEnum.None)
                .WithPiece(PiecesEnum.BlackKing).On(SquareEnum.e8)
                .WithWhiteRokade(rokade)
                .WithPiece(PiecesEnum.WhiteKing).On(SquareEnum.e1)
                .WithPiece(PiecesEnum.WhiteRook).On(SquareEnum.a1);

            if (rokade.IsKingsideRokade())
                builder.WithPiece(PiecesEnum.WhiteRook).On(SquareEnum.h1);

            // Act
            var board = build.Build();

            // Assert
            Assert.IsTrue(board.Match(
                None: () => false,
                Some: b => true
                ));
        }

        [TestCase(RokadeEnum.QueenSide)]
        [TestCase(RokadeEnum.KingAndQueenSide)]
        public void Build_WithBlackQueensideRokade_ShouldReturnBoard(RokadeEnum rokade)
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;
            builder
                .WithBlackRokade(rokade)
                .WithPiece(PiecesEnum.BlackKing).On(SquareEnum.e8)
                .WithPiece(PiecesEnum.BlackRook).On(SquareEnum.a8)
                .WithWhiteRokade(RokadeEnum.None)
                .WithPiece(PiecesEnum.WhiteKing).On(SquareEnum.e1);

            if (rokade.IsKingsideRokade())
                builder.WithPiece(PiecesEnum.BlackRook).On(SquareEnum.h8);

            // Act
            var board = build.Build();

            // Assert
            Assert.IsTrue(board.Match(
                None: () => false,
                Some: b => true
                ));
        }

        [TestCase(PiecesEnum.WhitePawn, SquareEnum.a3, SquareEnum.a4, false)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.b3, SquareEnum.b4, false)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.c3, SquareEnum.c4, false)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.d3, SquareEnum.d4, false)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.e3, SquareEnum.e4, false)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.f3, SquareEnum.f4, false)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.g3, SquareEnum.g4, false)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.h3, SquareEnum.h4, false)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.a6, SquareEnum.a5, true)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.b6, SquareEnum.b5, true)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.c6, SquareEnum.c5, true)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.d6, SquareEnum.d5, true)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.e6, SquareEnum.e5, true)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.f6, SquareEnum.f5, true)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.g6, SquareEnum.g5, true)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.h6, SquareEnum.h5, true)]
        public void Build_WithEpSquare_ShouldReturnBoard(PiecesEnum pawn, SquareEnum epSquare, SquareEnum pawnSquare, bool isWhiteToMove)
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;
            builder
                .WithWhiteToMove(isWhiteToMove)
                .WithBlackRokade(RokadeEnum.None)
                .WithWhiteRokade(RokadeEnum.None)
                .WithPiece(PiecesEnum.WhiteKing).On(SquareEnum.e1)
                .WithPiece(PiecesEnum.BlackKing).On(SquareEnum.e8)
                .WithEpSquare(epSquare)
                .WithPiece(pawn).On(pawnSquare);

            // Act
            var board = build.Build();

            // Assert
            Assert.IsTrue(board.Match(
                None: () => false,
                Some: b => true
                ));
        }

        [TestCase(PiecesEnum.WhitePawn, SquareEnum.e3, SquareEnum.e4, true)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.e6, SquareEnum.e5, false)]
        public void Build_WithToMoveAndSameColorEpSquare_ShouldReturnNone(PiecesEnum pawn, SquareEnum epSquare, SquareEnum pawnSquare, bool isWhiteToMove)
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;
            builder
                .WithWhiteToMove(isWhiteToMove)
                .WithBlackRokade(RokadeEnum.None)
                .WithWhiteRokade(RokadeEnum.None)
                .WithPiece(PiecesEnum.WhiteKing).On(SquareEnum.e1)
                .WithPiece(PiecesEnum.BlackKing).On(SquareEnum.e8)
                .WithEpSquare(epSquare)
                .WithPiece(pawn).On(pawnSquare);

            // Act
            var board = build.Build();

            // Assert
            Assert.IsTrue(board.Match(
                None: () => true,
                Some: b => false
                ));
        }

        [Test]
        public void Build_WithPawnOnValidSquare_ShouldReturnBoard()
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;
            builder
                .WithBlackRokade(RokadeEnum.None)
                .WithWhiteRokade(RokadeEnum.None)
                .WithPiece(PiecesEnum.WhiteKing).On(SquareEnum.e1)
                .WithPiece(PiecesEnum.BlackKing).On(SquareEnum.e8)
                .WithPiece(PiecesEnum.WhitePawn).On(SquareEnum.a2)
                .WithPiece(PiecesEnum.WhitePawn).On(SquareEnum.b3)
                .WithPiece(PiecesEnum.WhitePawn).On(SquareEnum.c4)
                .WithPiece(PiecesEnum.WhitePawn).On(SquareEnum.d5)
                .WithPiece(PiecesEnum.WhitePawn).On(SquareEnum.e6)
                .WithPiece(PiecesEnum.WhitePawn).On(SquareEnum.f7)
                .WithPiece(PiecesEnum.WhitePawn).On(SquareEnum.g6)
                .WithPiece(PiecesEnum.WhitePawn).On(SquareEnum.h5)
                .WithPiece(PiecesEnum.BlackPawn).On(SquareEnum.a7)
                .WithPiece(PiecesEnum.BlackPawn).On(SquareEnum.b6)
                .WithPiece(PiecesEnum.BlackPawn).On(SquareEnum.c5)
                .WithPiece(PiecesEnum.BlackPawn).On(SquareEnum.d4)
                .WithPiece(PiecesEnum.BlackPawn).On(SquareEnum.e3)
                .WithPiece(PiecesEnum.BlackPawn).On(SquareEnum.f2)
                .WithPiece(PiecesEnum.BlackPawn).On(SquareEnum.g3)
                .WithPiece(PiecesEnum.BlackPawn).On(SquareEnum.h4);

            // Act
            var board = build.Build();

            // Assert
            Assert.IsTrue(board.Match(
                None: () => false,
                Some: b => true
                ));
        }

        [TestCase(PiecesEnum.WhitePawn, SquareEnum.a8, true)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.b8, true)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.c8, true)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.d8, true)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.e8, true)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.f8, true)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.g8, true)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.h8, true)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.a1, true)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.b1, true)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.c1, true)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.d1, true)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.e1, true)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.f1, true)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.g1, true)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.h1, true)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.a8, false)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.b8, false)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.c8, false)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.d8, false)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.e8, false)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.f8, false)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.g8, false)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.h8, false)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.a1, false)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.b1, false)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.c1, false)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.d1, false)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.e1, false)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.f1, false)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.g1, false)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.h1, false)]
        public void Build_WithPawnOnInvalidSquare_ShouldReturnBoardWithNoPawnsOnInvalidSquares(PiecesEnum pawn, SquareEnum pawnSquare, bool isWhiteToMove)
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;
            builder
                .WithBlackRokade(RokadeEnum.None)
                .WithWhiteRokade(RokadeEnum.None)
                .WithWhiteToMove(isWhiteToMove)
                .WithPiece(PiecesEnum.WhiteKing).On(SquareEnum.e1)
                .WithPiece(PiecesEnum.BlackKing).On(SquareEnum.e8)
                .WithPiece(pawn).On(pawnSquare);

            // Act
            var board = build.Build();

            // Assert
            Assert.IsTrue(board.Match(
                None: () => false,
                Some: b => true
                ));
            Assert.IsTrue(board.Match(
                None: () => false,
                Some: b => !b.IterateForAllMoves()
                            .Where(t => PieceEnum.Pawn.Is(t.Piece) && (pawnSquare.Equals(t.Square)))
                            .Any()
                ));
        }

        [Test]
        public void WithPieceOn_ReplaceBlackPieceByWhitePiece_ShouldReturnWhitePiece()
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;

            // Act
            builder
                .WithPiece(PiecesEnum.BlackPawn).On(SquareEnum.e4)
                .WithPiece(PiecesEnum.WhitePawn).On(SquareEnum.e4);

            // Assert
            var piece = builder.GetPieceOn(SquareEnum.e4);

            Assert.IsTrue(piece.Match(
                None: () => false,
                Some: p => PiecesEnum.WhitePawn.Is(p.Piece)
                ));
        }

        [Test]
        public void WithPieceOn_ReplaceBlackPawnByBlackRook_ShouldReturnBlackRook()
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;

            // Act
            builder
                .WithPiece(PiecesEnum.BlackPawn).On(SquareEnum.e4)
                .WithPiece(PiecesEnum.BlackRook).On(SquareEnum.e4);

            // Assert
            var piece = builder.GetPieceOn(SquareEnum.e4);

            Assert.IsTrue(piece.Match(
                None: () => false,
                Some: p => PiecesEnum.BlackRook.Is(p.Piece)
                ));
        }

        [Test]
        public void WithPieceOff_ReplacePiece_ShouldReturnNone()
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;

            // Act
            builder
                .WithPiece(PiecesEnum.BlackPawn).On(SquareEnum.e4)
                .WithPiece(PiecesEnum.BlackRook).Off(SquareEnum.e4);

            // Assert
            var piece = builder.GetPieceOn(SquareEnum.e4);

            Assert.IsTrue(piece.Match(
                None: () => true,
                Some: p => false
                ));
        }

        [TestCase(PiecesEnum.WhitePawn, SquareEnum.e4, SquareEnum.e3, SquareEnum.e2, false)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.e5, SquareEnum.e6, SquareEnum.e7, true)]
        public void Build_WithEpSquareAndPieceOnEmptySquare_ShouldReturnNone(PiecesEnum pawn, SquareEnum pawnSquare, SquareEnum epSquare, SquareEnum emptySquare, bool isWhiteToMove)
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;
            builder
                .WithBlackRokade(RokadeEnum.None)
                .WithWhiteRokade(RokadeEnum.None)
                .WithWhiteToMove(isWhiteToMove)
                .WithPiece(PiecesEnum.WhiteKing).On(SquareEnum.e1)
                .WithPiece(PiecesEnum.BlackKing).On(SquareEnum.e8)
                .WithPiece(pawn).On(pawnSquare)
                .WithEpSquare(epSquare)
                .WithPiece(PiecesEnum.BlackRook).On(emptySquare);

            // Act
            var board = build.Build();

            // Assert
            Assert.IsTrue(board.Match(
                None: () => true,
                Some: b => false
                ));
        }
    }
}
