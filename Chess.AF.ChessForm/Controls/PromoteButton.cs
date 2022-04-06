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

namespace Chess.AF.ChessForm.Controls
{
    public partial class PromoteButton : PictureBox
    {
        public int Id { get; private set; }
        public PromoteButton(int id)
        {
            InitializeComponent();

            this.Id = id;
            this.DoubleBuffered = true;
            this.BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var rect = new Rectangle(0, 0, 35, 35);
            var path = GraphicsExtensions.RoundedRect(rect, 5);
            this.Region = new Region(path);
            path.Dispose();
            e.Graphics.DrawRoundedRectangle(new Pen(Color.Black, 2f), rect, 5);
        }

    }
}
