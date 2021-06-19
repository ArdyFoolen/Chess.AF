using AF.Functional;
using Chess.AF.Commands;
using Chess.AF.Dto;
using Chess.AF.Enums;
using Chess.AF.Helpers;
using Chess.AF.PositionBridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AF.Functional.F;

namespace Chess.AF
{
    public class Game
    {
        private Option<IBoard> Position
        {
            get
            {
                if (this.CurrentCommand < 0)
                    return None;
                return this.Commands[this.CurrentCommand].Position;
            }
        }
        private List<Command> Commands = new List<Command>();
        private int CurrentCommand = -1;

        public void Load()
            => LoadCommand(new LoadCommand());

        public void Load(string fenString)
            => LoadCommand(new LoadCommand(fenString));

        public bool IsLoaded { get { return !Position.Equals(None); } }

        public void Move(Move move)
            => ExecuteCommand(new MoveCommand(Position, move));

        public void Resign()
            => ReplaceCommand(new ResignCommand(Position));

        public void Draw()
            => ReplaceCommand(new DrawCommand(Position));

        private void ReplaceCommand(Command command)
        {
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

        //public Option<Selected> SelectPiece(Option<PieceEnum> piece)
        //    => piece.Bind(pc => Position.Bind(p => Selected.Of(pc, p.GetIteratorFor(pc), p)));

        public string ToFenString()
            => Position.Match(
                None: () => "No Game loaded or Invalid Game",
                Some: p => p.ToFenString());

        public Option<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> FindMove(
            Func<IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>,
                Option<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>> findIn)
            => Position.Map(p => p.IterateForAllMoves()).Bind(it => findIn(it));

        public void ForEachMove(Action<IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>> iterator)
            => Position.Map(p => p.IterateForAllMoves()).ForEach(iterator);

        public IEnumerable<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)> AllMoves()
            => Position.Map(p => p.IterateForAllMoves()).Match(
                None: () => Enumerable.Empty<(PieceEnum Piece, SquareEnum Square, PieceEnum Promoted, SquareEnum MoveSquare)>(),
                Some: s => s
                );

        //public void Map(Func<Position, Position> func)
        //    => Position.Map(func);
        public void Map(Func<IBoard, IBoard> func)
            => Position.Map(func);

        public bool IsWhiteToMove
        {
            get => Position.Match(
                None: () => false,
                Some: s => s.IsWhiteToMove);
        }

        public bool IsMate
        {
            get => Position.Match(
                None: () => false,
                Some: s => s.IsMate);
        }

        public bool IsInCheck
        {
            get => Position.Match(
                None: () => false,
                Some: s => s.IsInCheck);
        }

        public bool IsStaleMate
        {
            get => Position.Match(
                None: () => false,
                Some: s => s.IsStaleMate);
        }

        public GameResult Result
        {
            get => Position.Match(
                None: () => GameResult.Invalid,
                Some: s => s.Result);
        }

        public int MaterialCount
        {
            get => Position.Match(
                None: () => 0,
                Some: s => GetMaterialCount(s));
        }

        private int GetMaterialCount(IBoard position)
        {
            Material material = new Material(position.GetIteratorForAll<PiecesEnum>());
            return material.Count();
        }
    }
}
