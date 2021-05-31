using Chess.AF.ChessForm.Controllers;
using Chess.AF.ChessForm.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess.AF.ChessForm
{
    public partial class PromoteControl : Panel
    {
        public int Id { get; private set; }
        private IBoardController boardController;

        public PromoteControl(int id, IBoardController boardController, bool isBlackToMove)
        {
            InitializeComponent();

            this.Id = id;
            this.boardController = boardController;
            IsBlackToMove = isBlackToMove;
            AddPromoteButtons();

            this.DoubleBuffered = true;
            this.BackColor = Color.Transparent;
        }

        public bool IsBlackToMove { get; private set; }

        private void AddPromoteButtons()
        {
            if (IsBlackToMove)
                AddBlackPromoteButtons();
            else
                AddWhitePromoteButtons();
        }

        private void AddBlackPromoteButtons()
        {
            AddPromoteButton(5, ImageHelper.BlackQueenSmall, new Point(0, 0));
            AddPromoteButton(4, ImageHelper.BlackRookSmall, new Point(35, 0));
            AddPromoteButton(3, ImageHelper.BlackBishopSmall, new Point(0, 35));
            AddPromoteButton(2, ImageHelper.BlackKnightSmall, new Point(35, 35));
        }

        private void AddWhitePromoteButtons()
        {
            AddPromoteButton(12, ImageHelper.WhiteQueenSmall, new Point(0, 0));
            AddPromoteButton(11, ImageHelper.WhiteRookSmall, new Point(35, 0));
            AddPromoteButton(10, ImageHelper.WhiteBishopSmall, new Point(0, 35));
            AddPromoteButton(9, ImageHelper.WhiteKnightSmall, new Point(35, 35));
        }

        private void AddPromoteButton(int id, Func<Image> getImage, Point location)
        {
            PromoteButton promote = new PromoteButton(id);
            promote.Image = getImage();
            promote.Location = location;
            this.Controls.Add(promote);
            promote.Click += new EventHandler(promote_Click);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var rect = new Rectangle(0, 0, 70, 70);
            var path = GraphicsExtensions.RoundedRect(rect, 5);
            this.Region = new Region(path);
            path.Dispose();
        }

        protected void promote_Click(object sender, EventArgs e)
        {
            PromoteButton panel = (PromoteButton)sender;
            boardController.Promote(Id, panel.Id);
        }
    }
}
