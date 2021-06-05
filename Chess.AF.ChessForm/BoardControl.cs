using Chess.AF.ChessForm.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Chess.AF.ChessForm.ChessConstants;

namespace Chess.AF.ChessForm
{
    public partial class BoardControl : UserControl
    {
        private IEnumerable<SquareControl> squares;
        private IBoardController boardController;

        public BoardControl(IBoardController boardController)
        {
            InitializeComponent();

            this.boardController = boardController;
            this.squares = GetSquares();

            this.Controls.AddRange(this.squares.ToArray());
        }

        private IEnumerable<SquareControl> GetSquares()
        {
            foreach (int id in Enumerable.Range(0, 64))
                yield return CreateSquareControl(id);
        }

        private SquareControl CreateSquareControl(int id)
        {
            var control = new SquareControl(id, boardController);
            control.Location = GetLocation(id);
            control.Margin = new Padding(0);
            control.Size = new Size(SquareWidth, SquareWidth);
            return control;
        }

        private Point GetLocation(int id)
            => new Point(GetX(id) * SquareWidth, GetY(id) * SquareWidth);

        private int GetX(int id)
            => GetRelativeLocation(id) % 8;

        private int GetY(int id)
            => GetRelativeLocation(id) / 8;

        private int GetRelativeLocation(int id)
            => IsReverse ? 63 - id : id;

        public void ReverseBoardView()
        {
            this.IsReverse = !this.IsReverse;
            foreach (SquareControl control in this.Controls)
                control.Location = GetLocation(control.Id);
            this.Invalidate(true);
        }

        public bool IsReverse { get; private set; } = false;

        public void LoadFen()
            => boardController.LoadFen();

        public void LoadFen(string fen)
            => boardController.LoadFen(fen);

        public string ToFenString()
            => boardController.ToFenString();

    }
}
