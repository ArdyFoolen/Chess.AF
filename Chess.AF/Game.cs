using AF.Functional;
using Chess.AF.Commands;
using Chess.AF.Dto;
using Chess.AF.Enums;
using Chess.AF.Helpers;
using Chess.AF.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF
{
    public class Game : IGame
    {
        private Option<IBoard> Board
        {
            get
            {
                if (this.CurrentCommand < 0)
                    return None;
                return this.Commands[this.CurrentCommand].Board;
            }
        }
        private List<Command> Commands = new List<Command>();
        private int CurrentCommand = -1;

        public void Load()
            => LoadCommand(new LoadCommand());

        public void Load(string fenString)
            => LoadCommand(new LoadCommand(fenString));

        public bool IsLoaded { get { return !Board.Equals(None); } }

        public void Move(Move move)
            => ExecuteCommand(new MoveCommand(Board, move));

        public void Resign()
            => ReplaceCommand(new ResignCommand(Board));

        public void Draw()
            => ReplaceCommand(new DrawCommand(Board));

        private void ReplaceCommand(Command command)
        {
            if (this.CurrentCommand > 1)
                this.CurrentCommand -= 1;
            ExecuteCommand(command);
        }

        private void LoadCommand(Command command)
        {
            this.Commands.Clear();
            this.CurrentCommand = -1;
            ExecuteCommand(command);
        }

        private void ExecuteCommand(Command command)
        {
            command.Execute();
            AddCommand(command);
        }

        private void AddCommand(Command command)
        {
            if (this.CurrentCommand + 1 < this.Commands.Count)
                this.Commands = this.Commands.Take(this.CurrentCommand + 1).ToList();

            this.Commands.Add(command);
            this.CurrentCommand += 1;
        }

        public void GotoFirstMove()
            => this.CurrentCommand = 0;

        public void GotoPreviousMove()
        {
            if (this.CurrentCommand > 0)
                this.CurrentCommand -= 1;
        }

        public void GotoNextMove()
        {
            if (this.CurrentCommand + 1 < this.Commands.Count())
                this.CurrentCommand += 1;
        }

        public void GotoLastMove()
            => this.CurrentCommand = this.Commands.Count() - 1;

        public string ToFenString()
            => Board.Match(
                None: () => "No Game loaded or Invalid Game",
                Some: p => p.ToFenString());

        public Option<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> FindMove(
            Func<IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>,
                Option<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>> findIn)
            => Board.Map(p => p.IterateForAllMoves()).Bind(it => findIn(it));

        public void ForEachMove(Action<IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>> iterator)
            => Board.Map(p => p.IterateForAllMoves()).ForEach(iterator);

        public IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> AllMoves()
            => Board.Map(p => p.IterateForAllMoves()).Match(
                None: () => Enumerable.Empty<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>(),
                Some: s => s
                );

        public void Map(Func<IBoard, IBoard> func)
            => Board.Map(func);

        public bool IsWhiteToMove
        {
            get => Board.Match(
                None: () => false,
                Some: s => s.IsWhiteToMove);
        }

        public bool IsMate
        {
            get => Board.Match(
                None: () => false,
                Some: s => s.IsMate);
        }

        public bool IsInCheck
        {
            get => Board.Match(
                None: () => false,
                Some: s => s.IsInCheck);
        }

        public bool IsStaleMate
        {
            get => Board.Match(
                None: () => false,
                Some: s => s.IsStaleMate);
        }

        public GameResult Result
        {
            get => Board.Match(
                None: () => GameResult.Invalid,
                Some: s => s.Result);
        }

        public Option<Move> LastMove
        {
            get => Board.Match(
                None: () => null,
                Some: s => s.LastMove);
        }

        public int MoveNumber
        {
            get => Board.Match(
                None: () => 0,
                Some: s => s.MoveNumber);
        }

        public int MaterialCount
        {
            get => Board.Match(
                None: () => 0,
                Some: s => GetMaterialCount(s));
        }

        private int GetMaterialCount(IBoard board)
        {
            Material material = new Material(board.GetIteratorForAll<PiecesEnum>());
            return material.Count();
        }
    }
}
