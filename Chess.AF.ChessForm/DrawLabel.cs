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

namespace Chess.AF.ChessForm
{
    public partial class DrawLabel : UserControl
    {
        private int Id { get; set; }
        public bool IsReverse { get; set; } = false;
        public bool IsFile { get; set; } = false;
        public string DrawText { get; set; }

        public DrawLabel(int id)
        {
            InitializeComponent();

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
            Font font = new Font(FontFamily.Families[0], 16, FontStyle.Regular);
            float fontSize = FontHelper.NewFontSize(g, new Size(16, 16), font, DrawText);
            font = new Font(font.Name, fontSize, FontStyle.Regular);
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
    }
}
