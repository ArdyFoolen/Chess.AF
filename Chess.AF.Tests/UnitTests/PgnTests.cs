using AF.Functional;
using Chess.AF.Commands;
using Chess.AF.Enums;
using Chess.AF.ImportExport;
using Chess.AF.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;
using Unit = System.ValueTuple;

namespace Chess.AF.Tests.UnitTests
{
    public class PgnTests
    {
        [TestCase("Chess.AF.Tests.Pgn.Pgn01.pgn")]
        [TestCase("Chess.AF.Tests.Pgn.Pgn02.pgn")]
        public void Of_BuildPgn_Succeeds(string pgnFile)
        {
            // Arrange
            string lines = ResourceHelper.ReadEmbeddedRessource(pgnFile);

            // Act
            Option<Pgn> expected = Pgn.Import(lines);

            // Assert
            expected.Match(
                None: () => Assert.Fail(),
                Some: p => Assert.IsTrue(true));
        }

        [Test]
        public void Export_BuildPgn_ShouldCreateTagPairs()
        {
            // Act
            var pgn = Pgn.Export(null, null);

            // Assert
            pgn.Match(
                None: () => Assert.Fail(),
                Some: p => AssertTagPairs(p, tagPairsFormat0, DateTime.Today.ToString("yyyy.MM.dd")));
        }

        [Test]
        public void Export_BuildPgnWithSetup_ShouldCreateTagPairs()
        {
            // Arrange
            Game game = new Game();
            game.Load(fenString1);
            var commands = GetCommandsFrom(game);

            // Act
            var pgn = Pgn.Export(game, commands);

            // Assert
            pgn.Match(
                None: () => Assert.Fail(),
                Some: p => AssertTagPairs(p, tagPairsFormat2, DateTime.Today.ToString("yyyy.MM.dd"), fenString1, GameResult.Ongoing.ToDisplayString()));
        }

        [Test]
        public void Export_BuildPgnWithLoadedDefaultFen_ShouldCreateWithoutSetup()
        {
            // Arrange
            Game game = new Game();
            game.Load(defaultFenString);
            IList<Command> commands = GetCommandsFrom(game);

            // Act
            var pgn = Pgn.Export(game, commands);

            // Assert
            pgn.Match(
                None: () => Assert.Fail(),
                Some: p => AssertTagPairs(p, tagPairsFormat1, DateTime.Today.ToString("yyyy.MM.dd"), GameResult.Ongoing.ToDisplayString()));
        }

        [Test]
        public void Export_BuildPgnAndWhiteResigns_ShouldCreateResult()
        {
            // Arrange
            Game game = new Game();
            game.Load(fenString1);
            var commands = GetCommandsFrom(game);
            game.Resign();

            // Act
            var pgn = Pgn.Export(game, commands);

            // Assert
            pgn.Match(
                None: () => Assert.Fail(),
                Some: p => AssertTagPairs(p, moveTextFormat1, DateTime.Today.ToString("yyyy.MM.dd"), GameResult.BlackWins.ToDisplayString(), fenString1));
        }

        [Test]
        public void Export_BuildPgnAndWhiteDraws_ShouldCreateResult()
        {
            // Arrange
            Game game = new Game();
            game.Load(fenString1);
            var commands = GetCommandsFrom(game);
            game.Draw();

            // Act
            var pgn = Pgn.Export(game, commands);

            // Assert
            pgn.Match(
                None: () => Assert.Fail(),
                Some: p => AssertTagPairs(p, moveTextFormat1, DateTime.Today.ToString("yyyy.MM.dd"), GameResult.Draw.ToDisplayString(), fenString1));
        }

        [Test]
        public void Export_BuildPgnAndBlackResigns_ShouldCreateResult()
        {
            // Arrange
            Game game = new Game();
            game.Load(fenString2);
            var commands = GetCommandsFrom(game);
            game.Resign();

            // Act
            var pgn = Pgn.Export(game, commands);

            // Assert
            pgn.Match(
                None: () => Assert.Fail(),
                Some: p => AssertTagPairs(p, moveTextFormat2, DateTime.Today.ToString("yyyy.MM.dd"), GameResult.WhiteWins.ToDisplayString(), fenString2));
        }

        [Test]
        public void Export_BuildPgnAndBlackDraws_ShouldCreateResult()
        {
            // Arrange
            Game game = new Game();
            game.Load(fenString2);
            var commands = GetCommandsFrom(game);
            game.Draw();

            // Act
            var pgn = Pgn.Export(game, commands);

            // Assert
            pgn.Match(
                None: () => Assert.Fail(),
                Some: p => AssertTagPairs(p, moveTextFormat2, DateTime.Today.ToString("yyyy.MM.dd"), GameResult.Draw.ToDisplayString(), fenString2));
        }

        // Tests Moves                          Ok
        // Long short rokade, black and white   Ok
        // Pawn other pieces                    Ok
        // Take or none take                    Ok
        // Promote different pieces             Ok
        // Show check and mate                  Ok

        [Test]
        public void Export_BuildPgnWithMoves_ShouldCreateMoveText()
        {
            // Arrange
            Game game = new Game();
            game.Load();
            MakeMove(game, (PieceEnum.Pawn, SquareEnum.e2, PieceEnum.Pawn, SquareEnum.e4));
            MakeMove(game, (PieceEnum.Pawn, SquareEnum.e7, PieceEnum.Pawn, SquareEnum.e5));
            MakeMove(game, (PieceEnum.Knight, SquareEnum.g1, PieceEnum.Knight, SquareEnum.f3));
            MakeMove(game, (PieceEnum.Knight, SquareEnum.b8, PieceEnum.Knight, SquareEnum.c6));
            MakeMove(game, (PieceEnum.Bishop, SquareEnum.f1, PieceEnum.Bishop, SquareEnum.c4));
            MakeMove(game, (PieceEnum.Bishop, SquareEnum.f8, PieceEnum.Bishop, SquareEnum.c5));
            MakeMove(game, (PieceEnum.King, SquareEnum.e1, PieceEnum.King, SquareEnum.g1));
            MakeMove(game, (PieceEnum.Knight, SquareEnum.g8, PieceEnum.Knight, SquareEnum.f6));
            MakeMove(game, (PieceEnum.Pawn, SquareEnum.d2, PieceEnum.Pawn, SquareEnum.d4));
            MakeMove(game, (PieceEnum.Pawn, SquareEnum.e5, PieceEnum.Pawn, SquareEnum.d4));
            MakeMove(game, (PieceEnum.Pawn, SquareEnum.c2, PieceEnum.Pawn, SquareEnum.c3));
            MakeMove(game, (PieceEnum.King, SquareEnum.e8, PieceEnum.King, SquareEnum.g8));
            MakeMove(game, (PieceEnum.Pawn, SquareEnum.c3, PieceEnum.Pawn, SquareEnum.d4));
            MakeMove(game, (PieceEnum.Bishop, SquareEnum.c5, PieceEnum.Bishop, SquareEnum.b4));
            MakeMove(game, (PieceEnum.Knight, SquareEnum.f3, PieceEnum.Knight, SquareEnum.g5));
            MakeMove(game, (PieceEnum.Pawn, SquareEnum.d7, PieceEnum.Pawn, SquareEnum.d6));
            MakeMove(game, (PieceEnum.Bishop, SquareEnum.c4, PieceEnum.Bishop, SquareEnum.f7));
            MakeMove(game, (PieceEnum.Rook, SquareEnum.f8, PieceEnum.Rook, SquareEnum.f7));
            MakeMove(game, (PieceEnum.Knight, SquareEnum.g5, PieceEnum.Knight, SquareEnum.f7));
            MakeMove(game, (PieceEnum.King, SquareEnum.g8, PieceEnum.King, SquareEnum.f7));
            MakeMove(game, (PieceEnum.Queen, SquareEnum.d1, PieceEnum.Queen, SquareEnum.b3));
            MakeMove(game, (PieceEnum.King, SquareEnum.f7, PieceEnum.King, SquareEnum.f8));
            MakeMove(game, (PieceEnum.Pawn, SquareEnum.d4, PieceEnum.Pawn, SquareEnum.d5));
            MakeMove(game, (PieceEnum.Knight, SquareEnum.c6, PieceEnum.Knight, SquareEnum.d4));
            MakeMove(game, (PieceEnum.Queen, SquareEnum.b3, PieceEnum.Queen, SquareEnum.b4));
            MakeMove(game, (PieceEnum.Knight, SquareEnum.d4, PieceEnum.Knight, SquareEnum.c2));
            MakeMove(game, (PieceEnum.Queen, SquareEnum.b4, PieceEnum.Queen, SquareEnum.e1));
            MakeMove(game, (PieceEnum.Knight, SquareEnum.c2, PieceEnum.Knight, SquareEnum.a1));
            MakeMove(game, (PieceEnum.Pawn, SquareEnum.f2, PieceEnum.Pawn, SquareEnum.f4));
            MakeMove(game, (PieceEnum.Knight, SquareEnum.a1, PieceEnum.Knight, SquareEnum.c2));
            MakeMove(game, (PieceEnum.Queen, SquareEnum.e1, PieceEnum.Queen, SquareEnum.e2));
            MakeMove(game, (PieceEnum.Knight, SquareEnum.c2, PieceEnum.Knight, SquareEnum.d4));
            MakeMove(game, (PieceEnum.Queen, SquareEnum.e2, PieceEnum.Queen, SquareEnum.d3));
            MakeMove(game, (PieceEnum.Pawn, SquareEnum.c7, PieceEnum.Pawn, SquareEnum.c5));
            MakeMove(game, (PieceEnum.Pawn, SquareEnum.d5, PieceEnum.Pawn, SquareEnum.c6));
            MakeMove(game, (PieceEnum.Knight, SquareEnum.d4, PieceEnum.Knight, SquareEnum.c6));
            MakeMove(game, (PieceEnum.Knight, SquareEnum.b1, PieceEnum.Knight, SquareEnum.c3));
            MakeMove(game, (PieceEnum.Bishop, SquareEnum.c8, PieceEnum.Bishop, SquareEnum.e6));
            MakeMove(game, (PieceEnum.Pawn, SquareEnum.f4, PieceEnum.Pawn, SquareEnum.f5));
            MakeMove(game, (PieceEnum.Bishop, SquareEnum.e6, PieceEnum.Bishop, SquareEnum.f7));
            MakeMove(game, (PieceEnum.Rook, SquareEnum.f1, PieceEnum.Rook, SquareEnum.f3));
            MakeMove(game, (PieceEnum.Queen, SquareEnum.d8, PieceEnum.Queen, SquareEnum.e7));
            MakeMove(game, (PieceEnum.Bishop, SquareEnum.c1, PieceEnum.Bishop, SquareEnum.f4));
            MakeMove(game, (PieceEnum.Knight, SquareEnum.c6, PieceEnum.Knight, SquareEnum.e5));
            MakeMove(game, (PieceEnum.Bishop, SquareEnum.f4, PieceEnum.Bishop, SquareEnum.e5));
            MakeMove(game, (PieceEnum.Queen, SquareEnum.e7, PieceEnum.Queen, SquareEnum.e5));
            MakeMove(game, (PieceEnum.Rook, SquareEnum.f3, PieceEnum.Rook, SquareEnum.f1));
            MakeMove(game, (PieceEnum.Rook, SquareEnum.a8, PieceEnum.Rook, SquareEnum.e8));
            MakeMove(game, (PieceEnum.Rook, SquareEnum.f1, PieceEnum.Rook, SquareEnum.e1));
            MakeMove(game, (PieceEnum.Knight, SquareEnum.f6, PieceEnum.Knight, SquareEnum.g4));
            MakeMove(game, (PieceEnum.Queen, SquareEnum.d3, PieceEnum.Queen, SquareEnum.f3));
            MakeMove(game, (PieceEnum.Queen, SquareEnum.e5, PieceEnum.Queen, SquareEnum.h2));
            MakeMove(game, (PieceEnum.King, SquareEnum.g1, PieceEnum.King, SquareEnum.f1));
            MakeMove(game, (PieceEnum.Bishop, SquareEnum.f7, PieceEnum.Bishop, SquareEnum.c4));
            MakeMove(game, (PieceEnum.Knight, SquareEnum.c3, PieceEnum.Knight, SquareEnum.e2));
            MakeMove(game, (PieceEnum.Queen, SquareEnum.h2, PieceEnum.Queen, SquareEnum.h1));
            var commands = GetCommandsFrom(game);

            // Act
            var pgn = Pgn.Export(game, commands);

            // Assert
            pgn.Match(
                None: () => Assert.Fail(),
                Some: p => AssertTagPairs(p, moveTextFormat3, DateTime.Today.ToString("yyyy.MM.dd"), GameResult.BlackWins.ToDisplayString()));
        }


        [Test]
        public void Export_BuildPgnWithMovesLongRokadeAndPromote_ShouldCreateMoveText()
        {
            // Arrange
            Game game = new Game();
            game.Load(fenString3);
            MakeMove(game, (PieceEnum.King, SquareEnum.e8, PieceEnum.King, SquareEnum.c8));
            MakeMove(game, (PieceEnum.King, SquareEnum.e1, PieceEnum.King, SquareEnum.c1));
            MakeMove(game, (PieceEnum.Pawn, SquareEnum.g2, PieceEnum.Queen, SquareEnum.g1));
            MakeMove(game, (PieceEnum.Pawn, SquareEnum.h7, PieceEnum.Rook, SquareEnum.h8));
            MakeMove(game, (PieceEnum.Rook, SquareEnum.d8, PieceEnum.Rook, SquareEnum.h8));
            MakeMove(game, (PieceEnum.Pawn, SquareEnum.g7, PieceEnum.Knight, SquareEnum.h8));
            MakeMove(game, (PieceEnum.Queen, SquareEnum.g1, PieceEnum.Queen, SquareEnum.d1));
            MakeMove(game, (PieceEnum.King, SquareEnum.c1, PieceEnum.King, SquareEnum.d1));
            MakeMove(game, (PieceEnum.Pawn, SquareEnum.h2, PieceEnum.Bishop, SquareEnum.h1));

            var commands = GetCommandsFrom(game);

            // Act
            var pgn = Pgn.Export(game, commands);

            // Assert
            pgn.Match(
                None: () => Assert.Fail(),
                Some: p => AssertTagPairs(p, moveTextFormat4, DateTime.Today.ToString("yyyy.MM.dd"), GameResult.Ongoing.ToDisplayString(), fenString3));
        }

        // Tests File Moves
        // Pawn other pieces
        // Take or none take
        // Promote different pieces
        // Show check and mate

        // Tests Row Moves
        // Pawn other pieces
        // Take or none take
        // Show check and mate

        // Tests FileRow Moves
        // other pieces
        // Take or none take
        // Show check and mate

        private void MakeMove(IGame game, (PieceEnum Piece, SquareEnum From, PieceEnum Promoted, SquareEnum To) move)
        {
            var toMove = AF.Dto.Move.Of(move.Piece, move.From, move.To, move.Promoted);
            toMove.Map(m => { game.Move(m); return Unit(); });
        }

        private static IList<Command> GetCommandsFrom(Game game)
            => game.GetType().GetField("Commands", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(game) as IList<Command>;

        private const string fenString1 = "r1q4k/1P6/8/8/8/8/8/R5K1 w - - 0 1";
        private const string fenString2 = "r1q4k/1P6/8/8/8/8/8/R5K1 b - - 0 1";
        private const string fenString3 = "r3k3/6PP/8/8/8/8/3P2pp/R3K3 b Qq - 0 1";
        private const string defaultFenString = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        private const string tagPairsFormat0 = @"[Event ""Chess.AF Game""]
[Site """"]
[Date ""{0}""]
[Round """"]
[White """"]
[Black """"]
[Result """"]

";

        private const string tagPairsFormat1 = @"[Event ""Chess.AF Game""]
[Site """"]
[Date ""{0}""]
[Round """"]
[White """"]
[Black """"]
[Result ""{1}""]

{1}

";

        private const string tagPairsFormat2 = @"[Event ""Chess.AF Game""]
[Site """"]
[Date ""{0}""]
[Round """"]
[White """"]
[Black """"]
[Result ""*""]
[Setup ""1""]
[FEN ""{1}""]

{2}

";

        private const string moveTextFormat1 = @"[Event ""Chess.AF Game""]
[Site """"]
[Date ""{0}""]
[Round """"]
[White """"]
[Black """"]
[Result ""{1}""]
[Setup ""1""]
[FEN ""{2}""]

{1}

";

        private const string moveTextFormat2 = @"[Event ""Chess.AF Game""]
[Site """"]
[Date ""{0}""]
[Round """"]
[White """"]
[Black """"]
[Result ""{1}""]
[Setup ""1""]
[FEN ""{2}""]

1... {1}

";

        private const string moveTextFormat3 = @"[Event ""Chess.AF Game""]
[Site """"]
[Date ""{0}""]
[Round """"]
[White """"]
[Black """"]
[Result ""{1}""]

1. e4 e5 2. Nf3 Nc6 3. Bc4 Bc5 4. O-O Nf6 5. d4 xd4 6. c3 O-O 7. xd4 Bb4 8. Ng5 d6 9. Bxf7+ Rxf7 10. Nxf7 Kxf7 11. Qb3+ Kf8 12. d5 Nd4 13. Qxb4 Nc2 14. Qe1 Nxa1 15. f4 Nc2 16. Qe2 Nd4 17. Qd3 c5 18. xc6 Nxc6 19. Nc3 Be6 20. f5 Bf7 21. Rf3 Qe7 22. Bf4 Ne5 23. Bxe5 Qxe5 24. Rf1 Re8 25. Re1 Ng4 26. Qf3 Qxh2+ 27. Kf1 Bc4+ 28. Ne2 Qh1# {1}

";

        private const string moveTextFormat4 = @"[Event ""Chess.AF Game""]
[Site """"]
[Date ""{0}""]
[Round """"]
[White """"]
[Black """"]
[Result ""{1}""]
[Setup ""1""]
[FEN ""{2}""]

1... O-O-O 2. O-O-O g1=Q 3. h8=R Rxh8 4. xh8=N Qxd1+ 5. Kxd1 h1=B {1}

";

        private void AssertTagPairs(Pgn pgn, string tagPairsFormat, params string[] parms)
        {
            var tagPairs = string.Format(tagPairsFormat, parms);
            Assert.AreEqual(tagPairs, pgn.PgnString);
        }
    }
}
