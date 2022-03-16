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

        protected ISquareController squareController;
        protected bool IsReverse { get; private set; } = false;

        protected Color ImageBackColor { get { return btnImage.BackColor; } set { btnImage.BackColor = value; } }

        public int Id { get; private set; }

        public BaseSquareControl(int id, ISquareController squareController)
        {
            InitializeComponent();

            this.Id = id;
            this.squareController = squareController;
            this.squareController.Register(this);

            SetBackColor();
            DrawFileOrRowLabel();
            btnImage.Size = new Size(SquareWidth, SquareWidth);

            SetImage();
        }

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
            => PaintButtonImage(sender, e);

        private void btnImage_MouseLeave(object sender, EventArgs e)
           => MouseLeaveImage(sender, e);

        private void btnImage_MouseEnter(object sender, EventArgs e)
           => MouseEnterImage(sender, e);

        private void btnImage_MouseClick(object sender, MouseEventArgs e)
           => MouseClickImage(sender, e);

        protected void SetImage()
        {
            squareController[Id].Match(
                None: () => { btnImage.Image = null; SetImageBackColorTo(); },
                Some: s => SetImage(s)
                );
            this.Invalidate(true);
        }

        protected void SetImageBackColorTo(Color? backColor = null)
        {
            btnImage.BackColor = backColor.HasValue ? backColor.Value : this.BackColor;
            if (lblFileAorH != null)
                lblFileAorH.BackColor = btnImage.BackColor;
            if (lblRow8or1 != null)
                lblRow8or1.BackColor = btnImage.BackColor;
        }

        #region Virtual methods

        protected virtual void MouseLeaveImage(object sender, EventArgs e) { }

        protected virtual void MouseEnterImage(object sender, EventArgs e) { }

        protected virtual void MouseClickImage(object sender, EventArgs e)
            => squareController.Select(Id);

        protected virtual void SetImage(PieceOnSquare<PiecesEnum> pieceOnSquare)
        {
            btnImage.Image = GetPieceDict[(int)pieceOnSquare.Piece]();
            SetImageBackColorTo();
        }

        protected virtual void PaintButtonImage(object sender, PaintEventArgs e) { lblFileAorH?.Invalidate(); lblRow8or1?.Invalidate(); }

        public virtual void UpdateView()
            => SetImage();

        #endregion
    }
}
