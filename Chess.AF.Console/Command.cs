using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using static AF.Functional.F;
using static Chess.AF.Console.ChessConsole;
using static Chess.AF.Console.Program;
using System.Runtime.InteropServices;

namespace Chess.AF.Console
{
    public class Command
    {
        private static readonly Game game = new Game();
        private static IDictionary<string, (string Description, Action<string[]> Action)> CmdDictionary = new Dictionary<string, (string Description, Action<string[]> Action)>()
        {
            { "default", ("Setup default initial Chess position", (parms) => game.Load()) },
            { "deselect", ("De-Select what is selected", (parms) => DeSelectPiece())},
            { "exit", ("Exit program", (parms) => WriteLine("Exit the Program")) },
            { "fen", ("Enter a valid Fen string, from which a chess Position gets created", (parms) => game.Load(Prompt("Enter FEN: "))) },
            { "fenstring", ("Create fen string from chess position", (parms) => WriteLine(game.ToFenString())) },
            { "help", ("Show this Help", (parms) => ShowHelp(CmdDictionary)) },
            { "move", ("Move {piece}{square}[-x]{square}{promote} or o-o, o-o-o", (parms) => MovePiece(game, parms)) },
            { "moves", ("Moves by selected piece, or all if no piece is selected", (parms) => Moves(game))},
            { "select", ("Select {piece} where piece is pnbrqk", (parms) => SelectPiece(game, parms))},
            { "show", ("Show the Chess board, if available", (parms) => ShowBoard(game)) }
        };

        public string Cmd { get; }
        public string[] Parameters { get; }
        private Command(string cmd, params string[] parameters) { this.Cmd = cmd; this.Parameters = parameters; }

        public static Option<Command> Of(string cmd)
            => IsValid(cmd.GetCommand().ToLowerInvariant()) ? Some(new Command(cmd.GetCommand().ToLowerInvariant(), cmd.GetParameters())) : WhenInvalid();

        private static Option<Command> WhenInvalid()
        {
            WriteError("Not a valid command entered");
            ShowHelp(CmdDictionary);
            return None;
        }

        public static bool IsValid(string cmd)
            => CmdDictionary.ContainsKey(cmd);

        public void Run()
            => CmdDictionary[Cmd].Action(Parameters);
    }
}
