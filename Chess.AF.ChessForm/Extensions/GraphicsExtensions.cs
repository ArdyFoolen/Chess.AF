using Chess.AF.ChessForm.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.ChessForm.Extensions
{
    public static class GraphicsExtensions
    {
        public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            //arc.X = (bounds.Right - bounds.Left) / 2 + diameter;
            //arc.Y = bounds.Bottom;
            //path.AddArc(arc, 270, -90);

            //arc.Y = bounds.Bottom + diameter;
            //path.AddArc(arc, 90, -90);

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        //public static GraphicsPath RoundedRect(Rectangle bounds, int radius, CornerEnum corner)
        //{
        //    int diameter = radius * 2;
        //    Size size = new Size(diameter, diameter);
        //    Rectangle arc = new Rectangle(bounds.Location, size);
        //    GraphicsPath path = new GraphicsPath();

        //    if (radius == 0)
        //    {
        //        path.AddRectangle(bounds);
        //        return path;
        //    }

        //    // top left arc  
        //    if (corner == CornerEnum.Left || corner == CornerEnum.Top || corner == CornerEnum.LeftTop)
        //        path.AddArc(arc, 180, 90);

        //    // top right arc  
        //    if (corner == CornerEnum.Right || corner == CornerEnum.Top || corner == CornerEnum.RightTop)
        //    {
        //        arc.X = bounds.Right - diameter;
        //        path.AddArc(arc, 270, 90);
        //    }

        //    // bottom right arc  
        //    if (corner == CornerEnum.Right || corner == CornerEnum.Bottom || corner == CornerEnum.RightBottom)
        //    {
        //        arc.Y = bounds.Bottom - diameter;
        //        path.AddArc(arc, 0, 90);
        //    }

        //    // bottom left arc 
        //    if (corner == CornerEnum.Left || corner == CornerEnum.Bottom || corner == CornerEnum.LeftBottom)
        //    {
        //        arc.X = bounds.Left;
        //        path.AddArc(arc, 90, 90);
        //    }

        //    path.CloseFigure();
        //    return path;
        //}

        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle bounds, int cornerRadius)
        {
            if (graphics == null)
                throw new ArgumentNullException("graphics");
            if (pen == null)
                throw new ArgumentNullException("pen");

            using (GraphicsPath path = RoundedRect(bounds, cornerRadius))
            {
                graphics.DrawPath(pen, path);
            }
        }

        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle bounds, int cornerRadius)
        {
            if (graphics == null)
                throw new ArgumentNullException("graphics");
            if (brush == null)
                throw new ArgumentNullException("brush");

            using (GraphicsPath path = RoundedRect(bounds, cornerRadius))
            {
                graphics.FillPath(brush, path);
            }
        }
    }
}
