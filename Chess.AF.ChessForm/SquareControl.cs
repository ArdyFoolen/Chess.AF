using AF.Functional;
using Chess.AF.ChessForm.Controllers;
using Chess.AF.ChessForm.Extensions;
using Chess.AF.ChessForm.Views;
using System;
using System.Drawing;
using System.Windows.Forms;
using static Chess.AF.ChessForm.ChessConstants;
using static Chess.AF.ChessForm.ImageHelper;
using System.Linq;

namespace Chess.AF.ChessForm
{
    public partial class SquareControl : UserControl, IBoardView
    {
        private IBoardController boardController;
        private bool isSelected;

        public int Id { get; private set; }
        public bool AbleToMoveTo
        {
            get
            {
                return boardController.SelectedMoves.Any(a => (int)a.MoveSquare == Id);
            }
        }

        public SquareControl(int id, IBoardController boardController)
        {
            InitializeComponent();

            this.Id = id;
            this.boardController = boardController;
            this.boardController.Register(this);

            SetBackColor();

            btnImage.Size = new Size(SquareWidth, SquareWidth);

            SetImage();
        }

        public void UpdateView()
            => SetImage();

        private void SetImage()
        {
            this.Invalidate(true);
            boardController[Id].Match(
                None: () => btnImage.Image = null,
                Some: s => SetImage(s)
                );
        }

        private void SetImage((PiecesEnum Piece, SquareEnum Square, bool IsSelected) tuple)
        {
            isSelected = tuple.IsSelected;
            btnImage.Image = GetPieceDict[(int)tuple.Piece]();
            SetBackColorToImage(tuple.IsSelected);
        }

        private void DrawCircle(Graphics graphics)
        {
            var brush = new SolidBrush(Color.FromArgb(150, Color.Gray));
            graphics.FillEllipse(brush, new Rectangle(MoveSquareLeftTop, MoveSquareLeftTop, MoveSquareWidthHeight, MoveSquareWidthHeight));
        }

        private void btnImage_MouseLeave(object sender, EventArgs e)
            => SetBackColorToImage(isSelected);

        private void btnImage_MouseEnter(object sender, EventArgs e)
        => SetBackColorToImage(true);

        private void btnImage_MouseClick(object sender, MouseEventArgs e)
            => boardController.Select(Id);

        private void SetBackColor()
            => this.BackColor = (Id % 2 == 0 && (Id / 8) % 2 == 0 ||
                    Id % 2 == 1 && (Id / 8) % 2 == 1)
                   ? Color.White : Color.Brown;

        private void SetBackColorToImage(bool isSelected)
            => btnImage.BackColor = isSelected ? this.BackColor.ChangeColorBrightness(GetBrightFactor()) : this.BackColor;

        private float GetBrightFactor()
            => this.BackColor == Color.White ? 0.8f : 1.5f;

        private void btnImage_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            if (AbleToMoveTo)
                DrawCircle(e.Graphics);
        }
    }
}
