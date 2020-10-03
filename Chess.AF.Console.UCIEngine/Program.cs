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

namespace Chess.AF.Console.UCIEngine
{
    // Link UCI protocol: https://gist.github.com/aliostad/f4470274f39d29b788c1b09519e67372

    class Program
    {
        static void Main(string[] args)
        {
            Debugger.Launch();

            Using(new StreamWriter("ArenaCommands.log", true), MainLoop);
        }

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
                        bestMove = CreateBestMoves(command);

                if (command.StartsWith("go"))
                {
                    Send(bestMove);
                    bestMove = "";
                }

            }
        }

        private static string GetMovesFrom(string command)
            => command.Split(' ').Last();

        private static string CreateBestMoves(string command)
        {
            var moves = GetMovesFrom(command);
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
