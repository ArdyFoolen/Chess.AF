using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess.AF.ChessForm.Helpers
{
    public static class FontHelper
    {
        public static float NewFontSize(Graphics graphics, Size size, Font font, string str)
        {
            SizeF stringSize = graphics.MeasureString(str, font);
            float wRatio = size.Width / stringSize.Width;
            float hRatio = size.Height / stringSize.Height;
            float ratio = Math.Min(hRatio, wRatio);
            return font.Size * ratio;
        }

        public static void Label_Paint(object sender, PaintEventArgs e)
        {
            Label lbl = sender as Label;
            float fontSize = NewFontSize(e.Graphics, lbl.Bounds.Size, lbl.Font, lbl.Text);
            Font f = new Font(lbl.Font.Name, fontSize, FontStyle.Regular);
            lbl.Font = f;
        }
    }
}
