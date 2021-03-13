using AF.Functional;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using static AF.Functional.F;
using AF.Classes.Agent;
using Unit = System.ValueTuple;

namespace Chess.AF.UCIEngine
{
    public class Engine
    {
        private StreamWriter writer = new StreamWriter(AppConfigHelper.LogCommandsFilePath, true);
        private static Func<StreamWriter, string> Read = (writer) => ReadCommand(writer);
        private static Action<StreamWriter, string> Send = (writer, c) => SendCommand(writer, c);
        private IAgent<Action<Game>> GameAgent;

        private static string ReadCommand(StreamWriter writer)
        {
            string command = ReadLine();
            writer.WriteLine($"Cmd: {command}");
            return command;
        }

        private static void SendCommand(StreamWriter writer, string command)
        {
            writer.WriteLine($"Send: {command}");
            WriteLine(command);
        }

        private void MainLoop(StreamWriter writer)
        {
            //string bestMove = "";
            Option<PositionCommandDto> positionCommand = None;

            var command = Read(writer);
            if (!"uci".IsEqual(command))
                throw new Exception("Expected UCI protocol");

            Send(writer, $"id name Chess.AF");
            Send(writer, $"id author Ardy Foolen");
            Send(writer, "uciok");

            var game = new Game();
            GameAgent = Agent.Start<Action<Game>>(run => run(game));
            //GameAgent.Tell(g => g.Load());

            while (!"quit".IsEqual(command))
            {
                command = Read(writer);

                switch (command)
                {
                    case "isready":
                        Send(writer, $"readyok");
                        break;
                    case "ucinewgame":
                        //GameAgent.Tell(g => g.Load());
                        break;
                    case "stop":
                        break;
                }

                if (command.StartsWith("position"))
                    positionCommand = PositionCommandDto.Of(command)
                        .Map(PositionSetup);
                //CreatePosition(command).Match(
                //    Exception: ex => throw ex,
                //    Success: _ => _);

                //if (command.StartsWith("position startpos moves"))
                //{
                //    bestMove = CreateBestMoves(command);
                //}

                if (command.StartsWith("go"))
                {
                    //Send(bestMove);
                    //bestMove = "";
                }

            }
        }

        private PositionCommandDto PositionSetup(PositionCommandDto positionCommand)
        {
            GameAgent.Tell(g => ChangePosition(g, positionCommand));

            return positionCommand;
        }

        private void ChangePosition(Game game, PositionCommandDto positionCommand)
        {
            LoadGame(game, positionCommand);
            MakeMove(game, positionCommand);
            //var myMove = CreateMove(game.AllMoves().FirstOrDefault());
            var myMove = CreateMove(game.AllMoves().Where(w => w.Piece.Is(PieceEnum.King) && w.MoveSquare.Equals(SquareEnum.c1)).FirstOrDefault());
            myMove = myMove.Match(
                None: () => CreateMove(game.AllMoves().FirstOrDefault()),
                Some: (m) => m);
            myMove.Map<Move, Unit>(m => game.Move(m));
            SendBestMove(myMove);
        }

        private static void LoadGame(Game game, PositionCommandDto positionCommand)
        {
            if (!game.IsLoaded)
                if (positionCommand.IsStartPos)
                    game.Load();
                else
                    game.Load(positionCommand.Fen);
        }

        private static void MakeMove(Game game, PositionCommandDto positionCommand)
        {
            var found = Find(game, positionCommand);
            var move = CreateMove(found);
            move.Map<Move, Unit>(m => game.Move(m));
        }

        private static Option<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> Find(Game game, PositionCommandDto positionCommand)
        {
            foreach (var tuple in game.AllMoves())
            {
                if (positionCommand.LastFrom == Some(tuple.Square) && positionCommand.LastTo == Some(tuple.MoveSquare))
                    return Some(tuple);
            }

            return None;
        }

        private static Option<Move> CreateMove(Option<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> tuple)
            => tuple.Bind(t => CreateMove(t));

        private static Option<Move> CreateMove((PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare) tuple)
        {
            PieceEnum? promote = null;
            if (!tuple.Promoted.Is(PieceEnum.Pawn))
                promote = tuple.Promoted;

            RokadeEnum rokade = RokadeEnum.None;
            if (tuple.Piece.IsQueensideRokadeMove(tuple.Square, tuple.MoveSquare))
                rokade = RokadeEnum.QueenSide;
            if (tuple.Piece.IsKingsideRokadeMove(tuple.Square, tuple.MoveSquare))
                rokade = RokadeEnum.KingSide;

            return Move.Of(tuple.Piece, tuple.Square, tuple.MoveSquare, promote, rokade);
        }

        private void SendBestMove(Option<Move> move)
        {
            move.Map<Move, Unit>(m => Send(writer, $"bestmove {m.From}{m.To}"));
        }

        public void Execute()
            => Using(writer, MainLoop);
    }
}
