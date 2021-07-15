using AF.Functional;
using Chess.AF.Controllers;
using Chess.AF.ChessForm.Extensions;
using Chess.AF.Views;
using System;
using System.Drawing;
using System.Windows.Forms;
using static Chess.AF.ChessForm.ChessConstants;
using static Chess.AF.ChessForm.ImageHelper;
using System.Linq;
using Chess.AF.Enums;
using Chess.AF.Dto;
using Chess.AF.ChessForm.Helpers;
using Unit = System.ValueTuple;
using static AF.Functional.F;

namespace Chess.AF.ChessForm
{
    public partial class SquareControl : UserControl, IBoardView
    {
        private DrawLabel lblFileAorH;
        private DrawLabel lblRow8or1;

        private IGameController gameController;
        private bool isSelected;
        private PromoteControl promoteControl;
        private bool IsReverse { get; set; } = false;

        public int Id { get; private set; }
        public bool AbleToMoveTo
        {
            get
            {
                return gameController.SelectedMoves.Any(a => (int)a.MoveSquare == Id);
            }
        }

        public SquareControl(int id, IGameController gameController)
        {
            InitializeComponent();

            this.Id = id;
            this.gameController = gameController;
            this.gameController.Register(this);

            SetBackColor();
            DrawFileOrRowLabel();
            btnImage.Size = new Size(SquareWidth, SquareWidth);

            CreatePromoteControl();

            SetImage();
        }

        private void DrawFileOrRowLabel()
        {
            if (Id.File() == 0 || Id.File() == 7)
            {
                lblFileAorH = CreateDrawLabel(new Point(0, 0));
                lblFileAorH.DrawText = (8 - Id.Row()).ToString();
                lblFileAorH.IsFile = true;
            }
            if (Id.Row() == 0 || Id.Row() == 7)
            {
                lblRow8or1 = CreateDrawLabel(new Point(Size.Width - 5, Size.Height - 10));
                lblRow8or1.DrawText = Convert.ToChar(97 + Id.File()).ToString();
                lblRow8or1.IsFile = false;
            }
            SetDrawLabelsToVisible();
        }

        private DrawLabel CreateDrawLabel(Point location)
        {
            DrawLabel label = new DrawLabel(Id);
            label.Size = new Size(16, 16);
            label.Location = location;
            this.Controls.Add(label);
            label.BringToFront();

            return label;
        }

        private void CreatePromoteControl()
        {
            int r = ((SquareEnum)Id).Row();
            if (r == 0 || r == 7)
            {
                promoteControl = new PromoteControl(Id, gameController, r == 7);
                promoteControl.Location = new Point(0, 0);
                promoteControl.Size = new Size(70, 70);
                this.Controls.Add(promoteControl);
                promoteControl.Enabled = promoteControl.Visible = false;
            }
        }

        public void UpdateView()
            => SetImage();

        public void Relocate(bool isReverse)
        {
            this.IsReverse = isReverse;
            if (this.lblFileAorH != null)
                this.lblFileAorH.IsReverse = IsReverse;
            if (this.lblRow8or1 != null)
                this.lblRow8or1.IsReverse = IsReverse;
            SetDrawLabelsToVisible();
            this.Location = GetLocation(this.Id);
        }

        private Point GetLocation(int id)
            => new Point(GetX(id) * SquareWidth, GetY(id) * SquareWidth);

        private int GetX(int id)
            => GetRelativeLocation(id) % 8;

        private int GetY(int id)
            => GetRelativeLocation(id) / 8;

        private int GetRelativeLocation(int id)
            => IsReverse ? 63 - id : id;

        private void SetImage()
        {
            this.Invalidate(true);
            gameController[Id].Match(
                None: () => { btnImage.Image = null; SetBackColorToImage(false); },
                Some: s => SetImage(s)
                );
        }

        private void SetImage(PieceOnSquare<PiecesEnum> pieceOnSquare)
        {
            isSelected = pieceOnSquare.IsSelected;
            btnImage.Image = GetPieceDict[(int)pieceOnSquare.Piece]();
            SetBackColorToImage(pieceOnSquare.IsSelected);
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
            => gameController.Select(Id);

        private void SetBackColor()
            => this.BackColor = (Id % 2 == 0 && (Id / 8) % 2 == 0 ||
                    Id % 2 == 1 && (Id / 8) % 2 == 1)
                   ? Color.White : Color.Brown;

        private void SetBackColorToImage(bool isSelected)
        {
            btnImage.BackColor = isSelected ? this.BackColor.ChangeColorBrightness(GetBrightFactor()) : this.BackColor;
            if (lblFileAorH != null)
                lblFileAorH.BackColor = btnImage.BackColor;
            if (lblRow8or1 != null)
                lblRow8or1.BackColor = btnImage.BackColor;
        }

        private float GetBrightFactor()
            => this.BackColor == Color.White ? 0.8f : 1.5f;

        private void btnImage_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            if (AbleToMoveTo)
            {
                DrawCircle(e.Graphics);

                if (gameController.IsPromoteMove(Id))
                {
                    SetEnableVisible(true);
                    promoteControl?.BringToFront();
                }
                else
                    SetEnableVisible(false);
            }
            else
                SetEnableVisible(false);

            //gameController.LastMove.Map(m => DrawLastMove(e, m));
            PaintRectangles(e);
        }

        private void PaintRectangles(PaintEventArgs e)
        {
            foreach (var item in gameController.LoosePieceSquares)
            {
                if (Id == (int)item)
                {
                    var color = Color.FromArgb(255, 192, 0);
                    var alpha = Color.FromArgb(125, color);
                    e.Graphics.DrawRectangle(new Pen(color, 5), this.DisplayRectangle);
                    Brush brush = new SolidBrush(alpha);
                    e.Graphics.FillRectangle(brush, this.DisplayRectangle);
                }
            }
        }

        private Unit DrawLastMove(PaintEventArgs e, Move move)
        {
            if (Id == (int)move.From || Id == (int)move.To)
            {
                var color = Color.FromArgb(255, 192, 0);
                var alpha = Color.FromArgb(125, color);
                e.Graphics.DrawRectangle(new Pen(color, 5), this.DisplayRectangle);
                Brush brush = new SolidBrush(alpha);
                e.Graphics.FillRectangle(brush, this.DisplayRectangle);
            }
            return Unit();
        }

        private void SetDrawLabelsToVisible()
        {
            SetDrawLabelFileToVisible();
            SetDrawLabelRowToVisible();
        }

        private void SetDrawLabelFileToVisible()
        {
            if (lblFileAorH == null)
                return;
            lblFileAorH.Visible = IsVisibleFile();
        }

        private void SetDrawLabelRowToVisible()
        {
            if (lblRow8or1 == null)
                return;
            lblRow8or1.Visible = IsVisibleRow();
        }

        private bool IsVisibleFile()
            => Id.File() == 0 && !IsReverse || Id.File() == 7 && IsReverse;

        private bool IsVisibleRow()
            => Id.Row() == 0 && IsReverse || Id.Row() == 7 && !IsReverse;

        private void SetEnableVisible(bool value)
        {
            if (promoteControl != null)
                promoteControl.Enabled = promoteControl.Visible = value;
        }
    }
}
