using Chess.AF.ChessForm.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess.AF.ChessForm.Controls
{
    public partial class DrawLabel : UserControl
    {
        private int Id { get; set; }
        public bool IsReverse { get; set; } = false;
        public bool IsFile { get; set; } = false;
        public string DrawText { get; set; }

        const int GWL_EXSTYLE = -20;
        const int WS_EX_TRANSPARENT = 0x20;

        public DrawLabel(int id)
        {
            InitializeComponent();

            this.BorderStyle = BorderStyle.None;

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.SupportsTransparentBackColor, true);
            //base.BackColor = Color.FromArgb(0, 0, 0, 0);//Added this because image wasnt redrawn when resizing form

            Opacity = 100;
            Id = id;
            SetBackColor();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if ((IsVisibleFile() && IsFile) ||
                (IsVisibleRow() && !IsFile))
                DrawString(e.Graphics);
        }

        private void DrawString(Graphics g)
        {
            Font font = new Font(FontFamily.Families[0], 9.5f, FontStyle.Regular);
            SolidBrush brush = GetSolidBrush();
            float x = 0.0F;
            float y = 0.0F;
            StringFormat format = new StringFormat();

            g.DrawString(DrawText, font, brush, x, y, format);
            font.Dispose();
            brush.Dispose();
        }

        private bool IsVisibleFile()
            => Id.File() == 0 && !IsReverse || Id.File() == 7 && IsReverse;

        private bool IsVisibleRow()
            => Id.Row() == 0 && IsReverse || Id.Row() == 7 && !IsReverse;

        private SolidBrush GetSolidBrush()
            => this.BackColor == Color.White ? new SolidBrush(Color.Brown) : new SolidBrush(Color.White);

        private void SetBackColor()
            => this.BackColor = (Id % 2 == 0 && (Id / 8) % 2 == 0 ||
                    Id % 2 == 1 && (Id / 8) % 2 == 1)
                   ? Color.White : Color.Brown;

        /// <summary>
        /// Tranparent
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_TRANSPARENT;
                return cp;
            }
        }

        private int opacity;
        public int Opacity
        {
            get { return opacity; }
            set
            {
                opacity = value;
                this.InvalidateEx();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Color bk = Color.FromArgb(Opacity, this.BackColor);
            //Color bk = this.BackColor;
            //label.Parent = newParent;
            //label.BackColor = Color.Transparent;
            //Color bk = Color.Transparent;
            e.Graphics.FillRectangle(new SolidBrush(bk), e.ClipRectangle);
        }

        protected void InvalidateEx()
        {
            if (Parent == null)
                return;
            Rectangle rc = new Rectangle(this.Location, this.Size);
            Parent.Invalidate(rc, true);
        }
    }
}
