using Chess.AF.Controllers;
using Chess.AF.Dto;
using Chess.AF.Enums;
using Chess.AF.Views;
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
using static Chess.AF.ChessForm.ImageHelper;
using AF.Functional;

namespace Chess.AF.ChessForm
{
    public partial class BaseSquareControl : UserControl, IBoardView
    {
        private DrawLabel lblFileAorH;
        private DrawLabel lblRow8or1;

        private IGameController gameController;
        protected bool IsReverse { get; private set; } = false;

        public int Id { get; private set; }

        public BaseSquareControl(int id, IGameController gameController)
        {
            InitializeComponent();

            this.Id = id;
            this.gameController = gameController;
            this.gameController.Register(this);

            SetBackColor();
            DrawFileOrRowLabel();
            btnImage.Size = new Size(SquareWidth, SquareWidth);

            SetImage();
        }

        private void SetBackColor()
            => this.BackColor = (Id % 2 == 0 && (Id / 8) % 2 == 0 ||
                    Id % 2 == 1 && (Id / 8) % 2 == 1)
                   ? Color.White : Color.Brown;

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

        private DrawLabel CreateDrawLabel(Point location)
        {
            DrawLabel label = new DrawLabel(Id);
            label.Size = new Size(16, 16);
            label.Location = location;
            this.Controls.Add(label);
            label.BringToFront();

            return label;
        }

        private void btnImage_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            PaintButtonImage(sender, e);
        }

        private void btnImage_MouseLeave(object sender, EventArgs e)
           => SetBackColorToImage(false);

        private void btnImage_MouseEnter(object sender, EventArgs e)
            => SetBackColorToImage(false);

        private void btnImage_MouseClick(object sender, MouseEventArgs e)
            => gameController.Select(Id);

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
            btnImage.Image = GetPieceDict[(int)pieceOnSquare.Piece]();
            SetBackColorToImage(false);
        }

        #region Virtual methods

        protected virtual void PaintButtonImage(object sender, PaintEventArgs e) { }

        protected virtual void SetBackColorToImage(bool isSelected)
        {
            btnImage.BackColor = this.BackColor;
            if (lblFileAorH != null)
                lblFileAorH.BackColor = btnImage.BackColor;
            if (lblRow8or1 != null)
                lblRow8or1.BackColor = btnImage.BackColor;
        }

        public virtual void UpdateView()
            => SetImage();

        #endregion
    }
}
