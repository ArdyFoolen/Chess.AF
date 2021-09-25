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
        public void Build_With2BlackKings_ShouldReturnNone()
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;
            builder
                .WithBlackRokade(RokadeEnum.None)
                .WithWhiteRokade(RokadeEnum.None)
                .WithPiece(PiecesEnum.WhiteKing).On(SquareEnum.e1)
                .WithPiece(PiecesEnum.BlackKing).On(SquareEnum.e8)
                .WithPiece(PiecesEnum.BlackKing).On(SquareEnum.e6);

            // Act
            var board = build.Build();

            // Assert
            Assert.IsTrue(board.Match(
                None: () => true,
                Some: b => false
                ));
        }

        [Test]
        public void Build_With2WhiteKings_ShouldReturnNone()
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;
            builder
                .WithBlackRokade(RokadeEnum.None)
                .WithWhiteRokade(RokadeEnum.None)
                .WithPiece(PiecesEnum.WhiteKing).On(SquareEnum.e1)
                .WithPiece(PiecesEnum.BlackKing).On(SquareEnum.e8)
                .WithPiece(PiecesEnum.WhiteKing).On(SquareEnum.e3);

            // Act
            var board = build.Build();

            // Assert
            Assert.IsTrue(board.Match(
                None: () => true,
                Some: b => false
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

        [TestCase(PiecesEnum.WhitePawn, SquareEnum.a3, SquareEnum.a4)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.b3, SquareEnum.b4)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.c3, SquareEnum.c4)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.d3, SquareEnum.d4)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.e3, SquareEnum.e4)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.f3, SquareEnum.f4)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.g3, SquareEnum.g4)]
        [TestCase(PiecesEnum.WhitePawn, SquareEnum.h3, SquareEnum.h4)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.a6, SquareEnum.a5)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.b6, SquareEnum.b5)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.c6, SquareEnum.c5)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.d6, SquareEnum.d5)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.e6, SquareEnum.e5)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.f6, SquareEnum.f5)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.g6, SquareEnum.g5)]
        [TestCase(PiecesEnum.BlackPawn, SquareEnum.h6, SquareEnum.h5)]
        public void Build_WithEpSquare_ShouldReturnBoard(PiecesEnum pawn, SquareEnum rokadeSquare, SquareEnum pawnSquare)
        {
            // Arrange
            IBoardBuilder builder = Board.CreateBuilder();
            var build = builder as IBoardBuild;
            builder
                .WithBlackRokade(RokadeEnum.None)
                .WithWhiteRokade(RokadeEnum.None)
                .WithPiece(PiecesEnum.WhiteKing).On(SquareEnum.e1)
                .WithPiece(PiecesEnum.BlackKing).On(SquareEnum.e8)
                .WithEpSquare(rokadeSquare)
                .WithPiece(pawn).On(pawnSquare);

            // Act
            var board = build.Build();

            // Assert
            Assert.IsTrue(board.Match(
                None: () => false,
                Some: b => true
                ));
        }
    }
}
