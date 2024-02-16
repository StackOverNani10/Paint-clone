using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Programming_Windows_GUI_Final_Project___Paint
{
    public partial class FormPaint : Form
    {
        //Variables
        bool paint = false;
        int index = 4;
        int x, y, sX, sY, cX, cY;
        Color colorP;
        Point pointX, pointY;
        Bitmap bitmapN;
        Graphics graphics;
        Pen pen = new Pen(Color.Black, 2);
        Pen eraser = new Pen(Color.White, 2);
        ColorDialog colorDialog = new ColorDialog();

        //Funtions and Methods
        static Point SetPoint(PictureBox pictureBox, Point point)
        {
            float pX = 1f * pictureBox.Image.Width / pictureBox.Width;
            float pY = 1f * pictureBox.Image.Height / pictureBox.Height;

            return new Point((int)(point.X*pX),(int)(point.Y*pY));
        }
        private void Validate(Bitmap bitmap, Stack<Point> pointStack, int x, int y, Color colorNew, Color colorOld)
        {
            Color cx = bitmap.GetPixel(x, y);
            if (cx == colorOld)
            {
                pointStack.Push(new Point(x, y));
                bitmap.SetPixel(x, y, colorNew);
            }
        }
        private void FillUp(Bitmap bitmap, int x, int y, Color newColor)
        {
            Color oldColor = bitmap.GetPixel(x,y);
            Stack<Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x,y));
            bitmap.SetPixel(x, y, newColor);
            if (oldColor == newColor) return;

            while (pixel.Count > 0)
            {
                Point point = (Point)pixel.Pop();
                if(point.X > 0 && point.Y > 0 && point.X < bitmap.Width - 1 && point.Y < bitmap.Height - 1)
                {
                    Validate(bitmap, pixel, point.X - 1, point.Y, newColor, oldColor);
                    Validate(bitmap, pixel, point.X, point.Y - 1, newColor, oldColor);
                    Validate(bitmap, pixel, point.X + 1, point.Y, newColor, oldColor);
                    Validate(bitmap, pixel, point.X, point.Y + 1, newColor, oldColor);
                }
            }
        }

        public FormPaint()
        {
            InitializeComponent();
            bitmapN = new Bitmap(pictureBoxMain.Width, pictureBoxMain.Height);
            graphics = Graphics.FromImage(bitmapN);
            graphics.Clear(Color.White);
            pictureBoxMain.Image = bitmapN;
            btnPencil.BackColor = btnPenWidth1.BackColor = Color.LightGray; //Colorea el selecionado
        }

        private void BtnColor_Click(object sender, EventArgs e)
        {
            Button penColor = (Button)sender;
            lblPenColor.BackColor = pen.Color = colorP = penColor.BackColor;
        }
        private void btnColorSet_Click(object sender, EventArgs e)
        {
            colorDialog.ShowDialog();
            colorP = lblPenColor.BackColor = pen.Color = colorDialog.Color;
        }
        private void BtnPenWidth_Click(object sender, EventArgs e)
        {
            foreach (var btn in pnlPenWidth.Controls.OfType<Button>())
                btn.BackColor = Color.WhiteSmoke;
            Button btnPenWidth = (Button)sender;
            pen.Width = eraser.Width = Convert.ToInt32(btnPenWidth.Tag);
        }
        private void Btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            index = Convert.ToInt32(btn.Tag);
        }

        private void BtnView_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem btnFile = (ToolStripMenuItem)sender;
            index = Convert.ToInt32(btnFile.Tag);

            if (index == 1)
            {
                panelMain.Width -= 100;
                panelMain.Height -= 100;
            }
            if (index == 2)
            {
                panelMain.Width += 100;
                panelMain.Height += 100;
            }
            if (index == 3)
            {
                return;
            }
            if (index == 4)
            {
                bool isVisible = statusStrip1.Visible ? false : true;
                bool isLabelCursorVisible = lblCursorCords.Visible ? false : true;
                bool isLabelShapeVisible = lblShapeCords.Visible ? false : true;
                statusStrip1.Visible = isVisible;
                lblCursorCords.Visible = isLabelCursorVisible;
                lblShapeCords.Visible = isLabelShapeVisible;
                if (isVisible)
                {
                    statusBarToolStripMenuItem.BackColor = SystemColors.ControlLight;
                    //statusBarToolStripMenuItem.Image = Image.FromFile(@"..\..\..\..\..\..\Check.png");
                }
                else
                {
                    statusBarToolStripMenuItem.BackColor = SystemColors.Control;
                    //statusBarToolStripMenuItem.Image = null;
                }
            }
            if (index == 5)
            {
                FormPaint.ActiveForm.WindowState = FormWindowState.Maximized;
            }
        }

        private void saveIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Image(*.png)|*.png|Image(*.jpg)|*.jpg|All files (*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Bitmap btm = bitmapN.Clone(new Rectangle(0, 0, pictureBoxMain.Width, pictureBoxMain.Height), bitmapN.PixelFormat);
                if (index == 4)
                    btm.Save(sfd.FileName, ImageFormat.Png);
                if (index == 5)
                    btm.Save(sfd.FileName, ImageFormat.Jpeg);
                if (index == 6)
                    btm.Save(sfd.FileName, ImageFormat.Gif);
            }
        }

        private void BtnFile_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem btnFile = (ToolStripMenuItem)sender;
            index = Convert.ToInt32(btnFile.Tag);

            if (index == 1)
            {
                return;
            }
            if (index == 2)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Open Image";
                ofd.Filter = "Image(*.png)|*.png|Image(*.jpg)|*.jpg|All files (*.*)|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Bitmap btm = new Bitmap(ofd.FileName);
                    pictureBoxMain.Image = btm;
                }
            }
            if (index == 3 || index == 4 || index == 5 || index == 6)
            {
                var sfd = new SaveFileDialog();
                sfd.Filter = "Image(*.png)|*.png|Image(*.jpg)|*.jpg|All files (*.*)|*.*";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Bitmap btm = bitmapN.Clone(new Rectangle(0, 0, pictureBoxMain.Width, pictureBoxMain.Height), bitmapN.PixelFormat);
                    if (index == 4) 
                        btm.Save(sfd.FileName, ImageFormat.Png);
                    if (index == 5)
                        btm.Save(sfd.FileName, ImageFormat.Jpeg);
                    if (index == 6)
                        btm.Save(sfd.FileName, ImageFormat.Gif);
                }
            }
            if (index == 7)
                MessageBox.Show("All Rights Reserved with Daniel Dominguez", "About Print Application", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pictureBoxMain_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true;
            pointY = e.Location;

            cX = e.X;
            cY = e.Y;
        }
        private void pictureBoxMain_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;
            sX = x - cX;
            sY = y - cY;

            if (index == 8)
                graphics.DrawLine(pen, cX, cY, x, y);
            if (index == 9)
                graphics.DrawRectangle(pen, cX, cY, sX, sY);
            if (index == 10)
                graphics.DrawEllipse(pen, cX, cY, sX, sY);
            if (index == 11)
            {
                Point point1 = new Point(cX, cY);
                Point point2 = new Point((cX + x) / 2, ((cY + y) / 2) - (x - cX));
                Point point3 = new Point(x, y);

                Point[] trianglePoints =
                {
                 point1,
                 point2,
                 point3,
                };

                graphics.DrawPolygon(pen, trianglePoints);
            } 
        }
        private void pictureBoxMain_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = SetPoint(pictureBoxMain, e.Location);
            if (index == 6)
                FillUp(bitmapN, point.X, point.Y, colorP);
            if (index == 7)
                colorP = pen.Color = lblPenColor.BackColor = ((Bitmap)pictureBoxMain.Image).GetPixel(point.X, point.Y);
        }
        private void pictureBoxMain_MouseMove(object sender, MouseEventArgs e)
        {
            lblCursorCords.Text = e.X + ", " + e.Y + "px";
            if (paint)
            {
                if(index == 4)
                {
                    pointX = e.Location;
                    graphics.DrawLine(pen, pointX, pointY);
                    pointY = pointX;
                }
                if (index == 5)
                {
                    pointX = e.Location;
                    graphics.DrawLine(eraser, pointX, pointY);
                    pointY = pointX;
                }
                if (index == 8)
                    lblShapeCords.Text = x + ", " + y + "px";
                if (index == 9)
                    lblShapeCords.Text = e.X + ", " + e.Y + "px";
                if (index == 10)
                    lblShapeCords.Text = e.X + ", " + e.Y + "px";
                if (index == 11)
                    lblShapeCords.Text = e.X + ", " + e.Y + "px";
            }
            pictureBoxMain.Refresh();
            x = e.X;
            y = e.Y;
            sX = e.X - cX;
            sY = e.Y - cY;
        }
        private void pictureBoxMain_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphicsPaint = e.Graphics;
            if (paint)
            {
                if (index == 8)
                    graphicsPaint.DrawLine(pen, cX, cY, x, y);
                if (index == 9)
                    graphicsPaint.DrawRectangle(pen, cX, cY, sX, sY);
                if (index == 10)
                    graphicsPaint.DrawEllipse(pen, cX, cY, sX, sY);
                if (index == 11)
                {
                    Point point1 = new Point(cX, cY);
                    Point point2 = new Point((cX + x) / 2, ((cY + y) / 2) - (x - cX));
                    Point point3 = new Point(x, y);

                    Point[] trianglePoints =
                    {
                        point1,
                        point2,
                        point3,
                    };

                    graphicsPaint.DrawPolygon(pen, trianglePoints);
                }
            }
        }
    }
}
