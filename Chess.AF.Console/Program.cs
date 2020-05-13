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

namespace Chess.AF.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            FuncExt.WhileNotAborted(() => RunCommand());

            //WriteLine("Enter Q to Quit");
            //Option<Fen> fen = FuncExt.WhileNone(() => ReadFen()).Match(
            //    Left: l => None,
            //    Right: r => r
            //    );
            //Option<Position> position = fen.CreatePosition();

            //var iterator = MapIterator.Of(position);
            //iterator.Match(
            //    None: () => WriteLine("Nothing to Iterate"),
            //    Some: iter =>
            //    {
            //        foreach (ulong map in iter.Maps())
            //            Write($"{map}");
            //    });
            //WriteLine();

            //position.Map(ShowKnights);
            //position.Map(ShowAll<PieceEnum>);
            //position.Map(ShowAll<PiecesEnum>);
            //position.Map(ShowBoard);

            //char[] fields = new char[]
            //{
            //    'r', 'n', 'b', 'q', 'k', 'b', 'n', 'r',
            //    'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p',
            //    ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ',
            //    ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ',
            //    ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ',
            //    ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ',
            //    'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P',
            //    'R', 'N', 'B', 'Q', 'K', 'B', 'N', 'R'
            //};
            //WriteBoard(fields);

            //#if DEBUG
            //            WriteLine("Press any key to continue . . .");
            //            ReadKey();
            //#endif
        }

        //private static Unit ShowKnights(Position position)
        //{
        //    PiecesIterator<PieceEnum> iterator = position.GetIteratorFor(PieceEnum.Knight);
        //    foreach (var square in iterator.Iterate())
        //        Write($"{square.Square.ToString()} ");
        //    WriteLine();
        //    return Unit();
        //}

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
            foreach (var sq in selected.Moves(Position))
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
                    Write($"={Char.ToUpperInvariant(ChessConsole.ConvertPieceToChar((int)sq.Promoted))}");
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

        //private static Unit ShowAll<T>(Position position)
        //    where T : Enum
        //{
        //    PiecesIterator<T> iterator = position.GetIteratorForAll<T>();
        //    foreach (var square in iterator.Iterate())
        //        Write($"{square.Piece.ToString()} {square.Square.ToString()} ");
        //    WriteLine();
        //    return Unit();
        //}

        public static void ShowBoard()
            => Position.Map(ShowBoard);

        private static Unit ShowBoard(Position position)
        {
            PiecesIterator<PiecesEnum> iterator = position.GetIteratorForAll<PiecesEnum>();
            List<(PiecesEnum Piece, SquareEnum Square, bool IsSelected)> list = new List<(PiecesEnum Piece, SquareEnum Square, bool IsSelected)>();
            foreach (var square in iterator.Iterate(IsSelected))
                list.Add(square);
            var dictionary = list.ToDictionary(d => d.Square);
            WriteBoard(dictionary);
            return Unit();
        }

        private static bool IsSelected(PiecesEnum piece, SquareEnum square)
            => SelectOpt.Match(
                None: () => false,
                Some: s => s.Contains(piece, square));

        //private static Either<string, Option<Fen>> ReadFen()
        //    => Prompt("Enter FEN: ").AbortIf(s => "q".Equals(s.ToLowerInvariant())).CreateFen();

        private static string Prompt(string prompt)
        {
            Write(prompt);
            return ReadLine();
        }

        public static Option<Position> Position;
        public static void CreatePositionFromFen()
        {
            Position = Prompt("Enter FEN: ").CreateFen().CreatePosition();
        }

        private static Option<Selected> SelectOpt = None;
        public static void SelectPiece(params string[] parameters)
        {
            if (parameters.Length != 1)
                return;
            Option<PieceEnum> pieceOpt = parameters[0].TryPieceParse();
            SelectOpt = pieceOpt.Bind(piece => Position.Bind(p => Selected.Of(piece, p.GetIteratorFor(piece))));

            SelectOpt.Map(ShowSelected);
        }

        /// <summary>
        /// Parameters {piece}{square}[-x]{square}{promote} or o-o, o-o-o
        /// </summary>
        /// <param name="parameters"></param>
        public static void MovePiece(params string[] parameters)
        {
            if (parameters.Length != 1)
                return;

            Option<(PieceEnum Piece, SquareEnum From, PieceEnum Promote, SquareEnum To, RokadeEnum Rokade)> ToMove = parameters[0].ToMove();
            ToMove.Match(
                None: () => WriteLine($"Invalid Move: {parameters[0]}"),
                Some: m => Position = Position.Bind(p => RokadeEnum.None.Equals(m.Rokade) ? p.Move(m.Piece, m.From, m.Promote, m.To) : p.Move(m.Piece, m.Rokade))
                );
            ShowBoard();
            //Option<PieceEnum> pieceOpt;
            //Option<SquareEnum> from;
            //Option<SquareEnum> to;
            //Option<PieceEnum> promoted;
            //if ("o-o".Equals(parameters[0]))
            //{
            //    pieceOpt = Some(PieceEnum.King);
            //    from = Position.Map(p => p.IsWhiteToMove ? SquareEnum.e1 : SquareEnum.e8);
            //    to = Position.Map(p => p.IsWhiteToMove ? SquareEnum.g1 : SquareEnum.g8);
            //    promoted = pieceOpt;
            //}
            //else if ("o-o-o".Equals(parameters[0]))
            //{
            //    pieceOpt = Some(PieceEnum.King);
            //    from = Position.Map(p => p.IsWhiteToMove ? SquareEnum.e1 : SquareEnum.e8);
            //    to = Position.Map(p => p.IsWhiteToMove ? SquareEnum.c1 : SquareEnum.c8);
            //    promoted = pieceOpt;
            //}
            //else
            //{
            //    var splits = parameters[0].Split(new char[] { '-', 'x' });
            //    if (splits[0].Length == 2)
            //    {
            //        pieceOpt = Some(PieceEnum.Pawn);
            //        from = splits[0].Substring(0, 2).ParseEnum<SquareEnum>();
            //    }
            //    else
            //    {
            //        pieceOpt = splits[0].Substring(0, 1).TryPieceParse();
            //        from = splits[0].Substring(1, 2).ParseEnum<SquareEnum>();
            //    }
            //    to = splits[1].Substring(0, 2).ParseEnum<SquareEnum>();
            //    promoted = pieceOpt;
            //    if (PieceEnum.Pawn.Equals(pieceOpt.AsEnumerable().First()) && splits[1].Length == 3)
            //        promoted = splits[1].Substring(2, 1).TryPieceParse();
            //}

            //Position = Position.Bind(p => pieceOpt.Bind(piece => from.Bind(f => promoted.Bind(promote => to.Bind(t => p.Move(piece, f, promote, t))))));
            //ShowBoard();
        }

        public static void DeSelectPiece()
        {
            SelectOpt.Match(
                None: () => { WriteLine("-- Nothing to De-Select --"); return Unit(); },
                Some: ShowSelected);
            SelectOpt = None;
        }

        public static void Moves()
        {
            SelectOpt.Match(
                None: () => { AllMoves(Position); return Unit(); },
                Some: ShowMoves);
        }
        private static Unit AllMoves(Option<Position> position)
        {
            position.Map(p => p.IterateForAllMoves()).ForEach(IterateAllMoves);
            return Unit();
        }

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
