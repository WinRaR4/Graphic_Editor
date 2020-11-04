using System;
using System.Drawing;
using System.Windows.Forms;

namespace Graphic_Editor
{
    public partial class Form1 : Form
    {
        private enum Tool { Pen, Ellipse, Rectangle, Line, Zoom }
        Tool ToolPick = new Tool();
        Color paintCol = Color.Black;
        Color paintColSecond = Color.White;

        Pen pencil;
        Image[] MemoryPicture = new Image[10];
        int memoryPosition = 0;

        bool filled = false;
        PictureBox pic, pic1;
        private readonly Bitmap p, a, z;
        int sizeZoom;
        SByte zoomLimit = 0;

        public int x, y, x1, y1;

        public Form1()
        {
            InitializeComponent();
            pic = new PictureBox(); pic1 = new PictureBox();
            p = new Bitmap(Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown2.Value));
            a = new Bitmap(Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown2.Value));
            z = new Bitmap(Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown2.Value));
            pic1.Image = z; pic.Image = a; picture.Image = p;
            x = y = x1 = y1 = 0;
            this.picture.MouseWheel += picture_MouseWheel;
            sizeZoom = Math.Min(picture.Width / pic.Width, picture.Height / pic.Height);
        }

        private void picture_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                picture.Width += 50;
                picture.Height += 50;
            } else
            {
                picture.Width -= 50;
                picture.Height -= 50;
            }

        }

        private void Pen(object sender, EventArgs e)
        {
            ToolPick = Tool.Pen;
            picture.Cursor = Cursors.Cross;

        }

        private void Ellipse(object sender, EventArgs e)
        {
            ToolPick = Tool.Ellipse;
            picture.Cursor = Cursors.Cross;
        }

        private void Rectangle(object sender, EventArgs e)
        {
            ToolPick = Tool.Rectangle;
            picture.Cursor = Cursors.Cross;
        }

        private void Line(object sender, EventArgs e)
        {
            ToolPick = Tool.Line;
            picture.Cursor = Cursors.Cross;

        }
        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolPick = Tool.Zoom;
            picture.Cursor = Cursors.SizeAll;
        }

        private void LineDraw(MouseEventArgs e, Pen pencil)
        {

            Graphics g, g1;

            g = Graphics.FromImage(pic1.Image);

            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                {
                    g.Clear(Color.White);
                    g.DrawLine(pencil, x, y, e.X, e.Y);
                }
                g.DrawImage(pic.Image, 0, 0);
                picture.Image = pic1.Image;
            }
        }

        private void DrawRect(MouseEventArgs e, Graphics g)
        {
            g.Clear(Color.White);
            SolidBrush c = new SolidBrush(paintColSecond);

            if (x < e.X && y < e.Y)
            {
                g.DrawRectangle(pencil, x, y, e.X - x, e.Y - y);
                if (filled)
                {
                    g.FillRectangle(c, Rect(g, x, y, e.X, e.Y));
                }
            }
            else
            if (x > e.X && y < e.Y)
            {
                g.DrawRectangle(pencil, e.X, y, x - e.X, e.Y - y);
                if (filled)
                {
                    g.FillRectangle(c, Rect(g, e.X, y, x, e.Y));
                }
            }
            else
            if (x < e.X && y > e.Y)
            {
                g.DrawRectangle(pencil, x, e.Y, e.X - x, y - e.Y);
                if (filled)
                {
                    g.FillRectangle(c, Rect(g, x, e.Y, e.X, y));
                }
            }
            else
            if (x > e.X && y > e.Y)
            {
                g.DrawRectangle(pencil, e.X, e.Y, x - e.X, y - e.Y);
                if (filled)
                {
                    g.FillRectangle(c, Rect(g, e.X, e.Y, x, y));
                }
            }
        }

        private void RectangleDraw(MouseEventArgs e, Pen pencil)
        {
            {
                Graphics g, g1;
                g = Graphics.FromImage(pic1.Image);

                if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
                {
                    {
                        DrawRect(e, g);
                    }
                    g.DrawImage(pic.Image, 0, 0);
                    picture.Image = pic1.Image;
                }
            }
        }

        private void openImage(object sender, EventArgs e)
        {
            using (OpenFileDialog file = new OpenFileDialog()
            {
                Multiselect = false,
                ValidateNames = true,
                Filter = "JPEG|*.jpg"
            })
            {
                if (file.ShowDialog() == DialogResult.OK)
                {
                    picture.Image = Image.FromFile(file.FileName);
                    pic.Image = picture.Image;
                }
            }
            numericUpDown1.Value = pic.Image.Width;
            numericUpDown2.Value = pic.Image.Height;
        }

        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {
            picture.Width += Convert.ToInt32(numericUpDown1.Value) - picture.Width;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            picture.Height += Convert.ToInt32(numericUpDown1.Value) - picture.Height;

        }

        private void filledEllipse(object sender, EventArgs e)
        {
            ToolPick = Tool.Ellipse;
            filled = true;
        }

        private void emptyEllipse(object sender, EventArgs e)
        {
            ToolPick = Tool.Ellipse;
            filled = false;
        }

        private void chooseColor(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            pictureBox1.BackColor = paintCol = colorDialog1.Color;
        }

        private void chooseColorSecond(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            pictureBoxSecond.BackColor = paintColSecond = colorDialog1.Color;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void filledToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ToolPick = Tool.Rectangle;
            filled = true;
        }

        private void saveImg(object sender, EventArgs e)
        {
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "JPeg Image|*.jpg";
                saveDialog.Title = "Save an Image File";
                saveDialog.ShowDialog();
                if (saveDialog.FileName != "")  // Разобраться с этой редкостной фигнёй !!!
                {
                    System.IO.FileStream fileSave = (System.IO.FileStream)saveDialog.OpenFile();
                    picture.Image.Save(fileSave, System.Drawing.Imaging.ImageFormat.Jpeg);
                    fileSave.Close();
                }
            }
        }

        private void backStep(object sender, EventArgs e)
        {
            if (memoryPosition > 0)
            {
                memoryPosition--;
                picture.Image = MemoryPicture[memoryPosition];
            }
        }

        private void nextStep(object sender, EventArgs e)
        {
            if (memoryPosition < 9 && MemoryPicture[memoryPosition + 1] != null)
            {
                memoryPosition++;
                picture.Image = MemoryPicture[memoryPosition];
            }
        }

        private Pen rightLeft(MouseEventArgs e, Pen pencil)
        {
            if (e.Button == MouseButtons.Left) pencil.Color = paintCol;
            else if (e.Button == MouseButtons.Right) pencil.Color = paintColSecond;
            return pencil;
        }

        private void picture_MouseClick(object sender, MouseEventArgs e)
        {
            if (ToolPick == Tool.Zoom)
            {
                var brush = new SolidBrush(Color.White);
                var s_pic = new Bitmap(picture.Width, picture.Height);
                var s_g = Graphics.FromImage(s_pic);
                if ((e.Button == MouseButtons.Right) && (zoomLimit > -5))
                {
                    sizeZoom /= 2;
                    zoomLimit -= 1;
                }
                if ((e.Button == MouseButtons.Left) && (zoomLimit < 5))
                {
                    sizeZoom *= 2;
                    zoomLimit += 1;
                }
                if ((zoomLimit > -5) && (zoomLimit < 5))
                {
                    var scaleWidth = (int)(pic.Width * sizeZoom);
                    var scaleHeight = (int)(pic.Height * sizeZoom);
                    s_g.FillRectangle(brush, new RectangleF(e.X, e.Y, (float)picture.Width, (float)picture.Height));
                    s_g.DrawImage(pic.Image, ((int)pic.Width - scaleWidth) / 2, ((int)pic.Height - scaleHeight) / 2, scaleWidth, scaleHeight);
                    picture.Image = s_pic;
                }
            }
        }

        private void Clear(object sender, EventArgs e)
        {
            pic1.Image = z; pic.Image = a; picture.Image = p;
        }

        private void emptyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            filled = false;
            ToolPick = Tool.Rectangle;
        }

        private void picture_MouseMove(object sender, MouseEventArgs e)
        {
            pencil = new Pen(paintCol, widthSlider.Value);
            pencil = rightLeft(e, pencil);
            pencil.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            pencil.StartCap = System.Drawing.Drawing2D.LineCap.Round;

            switch (ToolPick)
            {
                case Tool.Pen:
                    //ToolPicker Tools = new ToolPicker();
                    PenDraw(e, pencil);
                    //x = Tools.x;
                    //y = Tools.y;
                    //picture.Image = Tools.picture.Image;
                    //pic.Image = Tools.pic.Image;
                    break;
                case Tool.Ellipse:
                    EllipseDraw(e, pencil);
                    break;
                case Tool.Rectangle:
                    RectangleDraw(e, pencil);
                    break;
                case Tool.Line:
                    LineDraw(e, pencil);
                    break;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;
            if (memoryPosition < 8)
            {
                memoryPosition++;

                for (int i = 8; i >= 0; i--)
                {
                    Image buf;
                    buf = MemoryPicture[i];
                    MemoryPicture[i + 1] = buf;
                }
                MemoryPicture[0] = picture.Image;
            }
        }

        private void picture_MouseUp(object sender, MouseEventArgs e)
        {
            Pen p;
            p = new Pen(paintCol, widthSlider.Value);
            p = rightLeft(e, p);
            p.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            p.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            Graphics g;
            g = Graphics.FromImage(pic.Image);
            switch (ToolPick)
            {
                case Tool.Ellipse:
                    g.DrawEllipse(p, x, y, e.X - x, e.Y - y);
                    if (filled)
                    {
                        SolidBrush b = new SolidBrush(paintColSecond);
                        g.FillEllipse(b, Rect(g, x, y, e.X, e.Y));
                    }
                    break;
                case Tool.Rectangle:
                    {
                        SolidBrush c = new SolidBrush(paintColSecond);

                        if (x < e.X && y < e.Y)
                        {
                            g.DrawRectangle(pencil, x, y, e.X - x, e.Y - y);
                            if (filled)
                            {
                                g.FillRectangle(c, Rect(g, x, y, e.X, e.Y));
                            }
                        }
                        else
                        if (x > e.X && y < e.Y)
                        {
                            g.DrawRectangle(pencil, e.X, y, x - e.X, e.Y - y);
                            if (filled)
                            {
                                g.FillRectangle(c, Rect(g, e.X, y, x, e.Y));
                            }
                        }
                        else
                        if (x < e.X && y > e.Y)
                        {
                            g.DrawRectangle(pencil, x, e.Y, e.X - x, y - e.Y);
                            if (filled)
                            {
                                g.FillRectangle(c, Rect(g, x, e.Y, e.X, y));
                            }
                        }
                        else
                        if (x > e.X && y > e.Y)
                        {
                            g.DrawRectangle(pencil, e.X, e.Y, x - e.X, y - e.Y);
                            if (filled)
                            {
                                g.FillRectangle(c, Rect(g, e.X, e.Y, x, y));
                            }
                        }
                    }
                    break;
                case Tool.Line:
                    g.DrawLine(p, x, y, e.X, e.Y);

                    break;
            }
        }

        public void PenDraw(MouseEventArgs e, Pen pencil)
        {

            pencil.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            pencil.StartCap = System.Drawing.Drawing2D.LineCap.Round;

            Graphics g;
            g = Graphics.FromImage(pic.Image);

            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                g.DrawLine(pencil, x, y, e.X, e.Y);
                picture.Image = pic.Image;
            }
            x = e.X; 
            y = e.Y;
        }

        private void EllipseDraw(MouseEventArgs e, Pen pencil)
        {
            Graphics g;
            g = Graphics.FromImage(pic1.Image);
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                {
                    g.Clear(Color.White);
                    g.DrawEllipse(pencil, x, y, e.X - x, e.Y - y);
                }
                if (filled)
                {
                    SolidBrush b = new SolidBrush(paintColSecond);
                    g.FillEllipse(b, Rect(g, x, y, e.X, e.Y));
                }
                g.DrawImage(pic.Image, 0, 0);
                picture.Image = pic1.Image;
            }
        }

        private RectangleF Rect(Graphics g, int x, int y, int x1, int y1)
        {
            PointF p = new PointF(x, y);
            SizeF s = new SizeF(x1 - x, y1 - y);
            RectangleF fill = new RectangleF(p, s);
            return fill;
        }
    }
}
