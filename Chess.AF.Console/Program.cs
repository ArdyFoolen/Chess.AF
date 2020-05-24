using AF.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using static AF.Functional.F;
using static Chess.AF.Position;
using Unit = System.ValueTuple;
using static Chess.AF.Console.ChessConsole;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

namespace Chess.AF.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            FuncExt.WhileNotAborted(() => RunCommand());
        }

        private static Unit ShowSelected(Selected selected)
        {
            foreach (var square in selected.Iterator.Iterate())
                Write($"{square.Square.ToString()} ");
            WriteLine();
            return Unit();
        }

        private static Unit ShowMoves(Selected selected)
        {
            WriteLine($"Moves for {selected.Piece.ToString()}");
            foreach (var sq in selected.Moves())
            {
                if (selected.Piece.IsRokadeMove(selected.Iterator.Iterate().FirstOrDefault().Square, sq))
                    ShowRokade(sq);
                else
                    Write($"{sq.ToString()}");
                Write(" ");
            }
            WriteLine();
            return Unit();
        }

        private static Unit ShowMoves(PieceEnum piece, SquareEnum square, (PieceEnum Promoted, SquareEnum Square)[] moveSquares)
        {
            WriteLine($"Moves for {piece.ToString()} on {square.ToString()}");
            foreach (var sq in moveSquares)
            {
                if (piece.IsRokadeMove(square, sq.Square))
                    ShowRokade(sq.Square);
                else
                    Write($"{sq.Square.ToString()}");
                if (sq.Promoted != piece)
                    Write($"={Char.ToUpperInvariant(AF.Extensions.ConvertPieceToChar((int)sq.Promoted))}");
                Write(" ");
            }
            WriteLine();
            return Unit();
        }

        private static void ShowRokade(SquareEnum square)
        {
            if (square.File() == 6)
                Write("O-O");
            else
                Write("O-O-O");
        }

        /// <summary>
        /// Calculates the lenght in bytes of an object 
        /// and returns the size 
        /// </summary>
        /// <param name="TestObject"></param>
        /// <returns></returns>
        private static int GetObjectSize(object TestObject)
        {
            var bf = new DataContractSerializer(typeof(Position));
            MemoryStream ms = new MemoryStream();
            byte[] Array;
            bf.WriteObject(ms, TestObject);
            Array = ms.ToArray();
            return Array.Length;
        }

        public static void ShowBoard(Game game)
            => game.Map(ShowBoard);

        private static Position ShowBoard(Position position)
        {
            //int objSize = GetObjectSize(position);

            ChessConsole.WriteInfo = WriteInfo(position);
            PiecesIterator<PiecesEnum> iterator = position.GetIteratorForAll<PiecesEnum>();
            List<(PiecesEnum Piece, SquareEnum Square, bool IsSelected)> list = new List<(PiecesEnum Piece, SquareEnum Square, bool IsSelected)>();
            foreach (var square in iterator.Iterate(IsSelected))
                list.Add(square);
            var dictionary = list.ToDictionary(d => d.Square);
            WriteBoard(dictionary);
            return position;
        }

        private static bool IsSelected(PiecesEnum piece, SquareEnum square)
            => SelectOpt.Match(
                None: () => false,
                Some: s => s.Contains(piece, square));

        private static Action<SquareEnum> WriteInfo(Position position)
            => (s) => WriteInfo(position, s);

        private static void WriteInfo(Position position, SquareEnum square)
        {
            if (square.Row() == 0)
                WriteWhoToMove(position);
            if (square.Row() == 1)
                WriteCheckInfo(position);

        }
        private static void WriteWhoToMove(Position position)
        {
            if (position.IsWhiteToMove)
                Write("\tWhite to move");
            else
                Write("\tBlack to move");
        }

        private static void WriteCheckInfo(Position position)
        {
            if (position.IsMate)
                Write("\tCheck Mate");
            else if (position.IsInCheck)
                Write("\tCheck");
            else if (position.IsStaleMate)
                Write("\tStale Mate");
        }

        public static string Prompt(string prompt)
        {
            Write(prompt);
            return ReadLine();
        }

        private static Option<Selected> SelectOpt = None;
        public static void SelectPiece(Game game, params string[] parameters)
        {
            if (parameters.Length != 1)
                return;
            Option<PieceEnum> pieceOpt = parameters[0].TryPieceParse();
            SelectOpt = game.SelectPiece(pieceOpt);

            SelectOpt.Map(ShowSelected);
        }

        /// <summary>
        /// Parameters {piece}{square}[-x]{square}{promote} or o-o, o-o-o
        /// </summary>
        /// <param name="parameters"></param>
        public static void MovePiece(Game game, params string[] parameters)
        {
            if (parameters.Length != 1)
                return;

            Option<(PieceEnum Piece, SquareEnum From, PieceEnum Promote, SquareEnum To, RokadeEnum Rokade)> ToMove = parameters[0].ToMove();
            ToMove.Match(
                None: () => WriteLine($"Invalid Move: {parameters[0]}"),
                Some: m => game.Move(m));
            ShowBoard(game);
        }

        public static void DeSelectPiece()
        {
            SelectOpt.Match(
                None: () => { WriteLine("-- Nothing to De-Select --"); return Unit(); },
                Some: ShowSelected);
            SelectOpt = None;
        }

        public static void Moves(Game game)
        {
            SelectOpt.Match(
                None: () => { AllMoves(game); return Unit(); },
                Some: ShowMoves);
        }

        private static void AllMoves(Game game)
            => game.ForEachMove(IterateAllMoves);

        private static void IterateAllMoves(IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> iterator)
        {
            var group = iterator.GroupBy(t => (t.Piece, t.Square)).Select(g => new { g.Key, Items = g.ToList() });
            foreach (var item in group)
            {
                List<(PieceEnum Promoted, SquareEnum Square)> moveSquares = new List<(PieceEnum Promoted, SquareEnum Square)>();
                foreach (var value in item.Items)
                    moveSquares.Add((value.Promoted, value.MoveSquare));
                if (moveSquares.Count() > 0)
                    ShowMoves(item.Key.Piece, item.Key.Square, moveSquares.ToArray());
            }
        }

        private static Either<string, Option<Command>> RunCommand()
            => Prompt("Enter command: ")
            .AbortIf(s => "exit".Equals(s.ToLowerInvariant()))
            .CreateCommand()
            .Bind(RunIfValid);

        private static Either<string, Option<Command>> RunIfValid(Option<Command> command)
            => command.Bind<Command, Command>(
                NoneF: () => None,
                SomeF: c => { c.Run(); return c; });
    }
}
