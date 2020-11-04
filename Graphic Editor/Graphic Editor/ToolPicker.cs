using System.Windows.Forms;
using System.Drawing;
using System;

namespace Graphic_Editor
{
    class ToolPicker : Form1
    {
        private int x1, y1;
        private PictureBox pic, picture;

        internal static ToolPicker PenDraw(MouseEventArgs e, Color paintCol, PictureBox pic, int x, int y, PictureBox picture, Pen pencil)
        {
            Graphics g;
            g = Graphics.FromImage(pic.Image);
            pencil.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            pencil.StartCap = System.Drawing.Drawing2D.LineCap.Round;

            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                g.DrawLine(pencil, x, y, e.X, e.Y);
                picture.Image = pic.Image;
            }
            ToolPicker t = new ToolPicker();

            t.pic = pic;
            t.picture = picture;
            t.x1 = e.X;
            t.y1 = e.Y;
            return t;
        }
    }

    class A : ToolPicker
    {
        
    }
}
