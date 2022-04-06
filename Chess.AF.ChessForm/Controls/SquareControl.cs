using AF.Functional;
using Chess.AF.Controllers;
using Chess.AF.ChessForm.Extensions;
using Chess.AF.Views;
using System;
using System.Drawing;
using System.Windows.Forms;
using static Chess.AF.ChessForm.ChessConstants;
using static Chess.AF.ChessForm.Helpers.ImageHelper;
using System.Linq;
using Chess.AF.Enums;
using Chess.AF.Dto;
using Chess.AF.ChessForm.Helpers;
using Unit = System.ValueTuple;
using static AF.Functional.F;
using Chess.AF.ChessForm.Controls;

namespace Chess.AF.ChessForm.Controls
{
    public class SquareControl : BaseSquareControl
    {
        private bool isSelected;
        private PromoteControl promoteControl;
        private IGameController gameController;

        private bool AbleToMoveTo
        {
            get
            {
                return gameController.SelectedMoves.Any(a => (int)a.MoveSquare == Id);
            }
        }

        public SquareControl(int id, IGameController gameController) : base(id, gameController)
        {
            // No need to register here, base will do this
            this.gameController = gameController;

            CreatePromoteControl();

            SetImage();
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

        protected override void SetImage(PieceOnSquare<PiecesEnum> pieceOnSquare)
        {
            base.SetImage(pieceOnSquare);
            isSelected = pieceOnSquare.IsSelected;
            SetBackColorToImage();
        }

        private void DrawCircle(Graphics graphics)
        {
            var brush = new SolidBrush(Color.FromArgb(150, Color.Gray));
            graphics.FillEllipse(brush, new Rectangle(MoveSquareLeftTop, MoveSquareLeftTop, MoveSquareWidthHeight, MoveSquareWidthHeight));
        }

        protected override void MouseLeaveImage(object sender, EventArgs e)
            => SetBackColorToImage();

        protected override void MouseEnterImage(object sender, EventArgs e)
            => SetImageBackColorTo(GetBrightBackColor());

        protected override void MouseClickImage(object sender, EventArgs e)
            => gameController.Select(Id);

        private void SetBackColorToImage()
            => SetImageBackColorTo(GetBrightBackColorIfSelected());

        private Color GetBrightBackColorIfSelected()
            => isSelected ? GetBrightBackColor() : this.BackColor;

        private Color GetBrightBackColor()
            => this.BackColor.ChangeColorBrightness(GetBrightFactor());

        private float GetBrightFactor()
            => this.BackColor == Color.White ? 0.8f : 1.5f;

        protected override void PaintButtonImage(object sender, PaintEventArgs e)
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

            gameController.LastMove.Map(m => DrawLastMove(e, m));
            PaintRectangles(e);
        }

        private void PaintRectangles(PaintEventArgs e)
        {
            if (gameController.LoosePieceSquares.Any(a => (int)a == Id))
                DrawRectangle(e, Color.FromArgb(192, 255, 0));
        }

        private Unit DrawLastMove(PaintEventArgs e, Move move)
        {
            if (Id == (int)move.From || Id == (int)move.To)
                DrawRectangle(e);
            return Unit();
        }

        private void DrawRectangle(PaintEventArgs e, Color? color = null)
        {
            color = color == null ? Color.FromArgb(255, 192, 0) : color;
            var alpha = Color.FromArgb(125, color.Value);
            e.Graphics.DrawRectangle(new Pen(color.Value, 5), this.DisplayRectangle);
            SetImageBackColorTo(alpha);
        }

        private void SetEnableVisible(bool value)
        {
            if (promoteControl != null)
                promoteControl.Enabled = promoteControl.Visible = value;
        }
    }
}
