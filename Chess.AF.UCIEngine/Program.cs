using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using AF.Functional;
using static AF.Functional.F;
using AF.Classes.Agent;

namespace Chess.AF.UCIEngine
{
    // Link UCI protocol: https://gist.github.com/aliostad/f4470274f39d29b788c1b09519e67372

    class Program
    {
        static void Main(string[] args)
        {
            Debugger.Launch();

            Using(new StreamWriter("ArenaCommands.log", true), MainLoop);
        }

        static Game game = new Game();
        static IAgent<Action<Game>> GameAgent;

        private static void MainLoop(StreamWriter writer)
        {
            Func<string> Read = () => ReadCommand(writer);
            Action<string> Send = c => SendCommand(writer, c);
            string bestMove = "";

            var command = Read();
            if (!"uci".IsEqual(command))
                throw new Exception("Expected UCI protocol");

            Send($"id name Chess.AF");
            Send($"id author Ardy Foolen");
            Send("uciok");

            GameAgent = Agent.Start<Action<Game>>(run => run(game));
            GameAgent.Tell(g => g.Load());

            while (!"quit".IsEqual(command))
            {
                command = Read();

                switch (command)
                {
                    case "isready":
                        Send($"readyok");
                        break;
                }

                if (command.StartsWith("position startpos moves"))
                {
                    bestMove = CreateBestMoves(command);
                }

                if (command.StartsWith("go"))
                {
                    Send(bestMove);
                    bestMove = "";
                }

            }
        }

        //private static void AllMoves(Game game)
        //    => game.ForEachMove(IterateAllMoves);

        private static Option<Move> FindMove(Game game, string command)
        {
            var move = CreateMoveFrom(command);
            return Move.Of(PieceEnum.Pawn, SquareEnum.e2, SquareEnum.e4, PieceEnum.Pawn, RokadeEnum.None);
        }

        //private static void FindMove(IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> iterator, (Option<SquareEnum> From, Option<SquareEnum> To, RokadeEnum Rokade) move)
        //{
        //    iterator.FirstOrDefault(m => m.Piece.IsQueensideRokadeMove(m.Square, m.MoveSquare) && RokadeEnum.QueenSide.Equals(move.Rokade) ||
        //                                    m.Piece.IsKingsideRokadeMove(m.Square, m.MoveSquare) && RokadeEnum.KingSide.Equals(move.Rokade) ||
        //                                    move.From.Map())
        //    //var group = iterator.GroupBy(t => (t.Piece, t.Square)).Select(g => new { g.Key, Items = g.ToList() });
        //    //foreach (var item in group)
        //    //{
        //    //    List<(PieceEnum Promoted, SquareEnum Square)> moveSquares = new List<(PieceEnum Promoted, SquareEnum Square)>();
        //    //    foreach (var value in item.Items)
        //    //        moveSquares.Add((value.Promoted, value.MoveSquare));
        //    //    if (moveSquares.Count() > 0)
        //    //        ShowMoves(item.Key.Piece, item.Key.Square, moveSquares.ToArray());
        //    //}
        //}

        private static (Option<SquareEnum> From, Option<SquareEnum> To, RokadeEnum Rokade) CreateMoveFrom(string command)
        {
            var move = GetMoveFrom(command);
            switch(move)
            {
                case "o-o":
                    return (None, None, RokadeEnum.KingSide);
                case "o-o=o":
                    return (None, None, RokadeEnum.QueenSide);
                default:
                    var split = SplitMove(move);
                    return (split.From.ToSquare(), split.To.ToSquare(), RokadeEnum.None);
            }
        }

        private static (string From, string To) SplitMoveFrom(string command)
            => SplitMoveFrom(GetMoveFrom(command));

        private static (string From, string To) SplitMove(string move)
            => (move.Substring(0, 2), move.Substring(2));

        private static string GetMoveFrom(string command)
            => command.Split(' ').Last();

        private static string CreateBestMoves(string command)
        {
            var moves = GetMoveFrom(command);
            switch (moves)
            {
                case "e2e4":
                    return "bestmove e7e5";
                case "g1f3":
                    return "bestmove b8c6";
                case "d2d4":
                    return "bestmove e5d4";
                case "f3d4":
                    return "bestmove g8f6";
            }
            return "";
        }

        private static string ReadCommand(StreamWriter writer)
        {
            string command = ReadLine().ToLowerInvariant();
            writer.WriteLine($"Cmd: {command}");
            return command;
        }

        private static void SendCommand(StreamWriter writer, string command)
        {
            writer.WriteLine($"Send: {command}");
            WriteLine(command);
        }
    }
}
