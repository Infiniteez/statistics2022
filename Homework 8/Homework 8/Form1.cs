using Microsoft.VisualBasic.Devices;
using System.Globalization;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.Xml;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Homework_8
{
    public partial class Form1 : Form
    {
        private Bitmap b;
        private Graphics g;
        private Random r = new Random();

        private int x_mouse, y_mouse;
        private int x_down, y_down;
        private int r_width, r_height;
        private bool drag = false, dragX = false, dragY = false;
        private bool resizing = false, resizingX = false, resizingY = false;
        private bool zoom = false, zoomX = false, zoomY = false;

        private Rectangle virtualWindow;
        private Rectangle virtualWindowX;
        private Rectangle virtualWindowY;
        private Pen penRectangle = new Pen(Color.Green, 0.2f);

        private bool vertical = false;

        private SortedDictionary<double, double> d = new SortedDictionary<double, double>();
        private SortedDictionary<double, double> dx = new SortedDictionary<double, double>();
        private SortedDictionary<double, double> dy = new SortedDictionary<double, double>();

        List<PointF> points;

        private int trialCount = 15000;

        private bool ex1 = false;

        public Form1()
        {
            InitializeComponent();
            //comboBox1.SelectedIndex = 0;
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            pictureBox1.Image = b;

            virtualWindow = Rectangle.FromLTRB(0, 320, 600, 920);
            virtualWindowX = Rectangle.FromLTRB(0, 20, 600, 320);
            virtualWindowY = Rectangle.FromLTRB(600, 320, 900, 920);

            timer1.Enabled = true;
            timer1.Interval = 16;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ex1 = true;
            dx.Clear();
            dy.Clear();

            points = new List<PointF>();
            for (int m = 0; m < trialCount; m++)
            {
                // Box-Muller transform
                double ro = r.NextDouble();
                double theta = r.NextDouble();

                double x = ro * Math.Cos(2 * Math.PI * theta);
                double y = ro * Math.Sin(2 * Math.PI * theta);

                double xApprox = Math.Round(x, 1);
                double yApprox = Math.Round(y, 1);

                dx[xApprox] = dx.GetValueOrDefault(xApprox, 0) + 1;
                dy[yApprox] = dy.GetValueOrDefault(yApprox, 0) + 1;

                points.Add(new PointF((float)x, (float)y));
                //g.DrawLine(penRectangle, xDevice, yDevice, xDevice, yDevice);
            }
            foreach (double key in dx.Keys.ToList())
            {
                dx[key] = Math.Round(dx[key] / trialCount, 4);
            }
            foreach (double key in dy.Keys.ToList())
            {
                dy[key] = Math.Round(dy[key] / trialCount, 4);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ex1 = false;
            d.Clear();
            for (int m = 0; m < trialCount; m++)
            {
                // Box-Muller transform
                double u1 = 1.0 - r.NextDouble(); // random uniform(0,1]
                double u2 = 1.0 - r.NextDouble(); // random uniform(0,1]

                double x = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); // random normal(mean=0,var=1)
                double y = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2); // random normal(mean=0,var=1)

                switch (comboBox1.SelectedIndex)
                {
                    case 0: // normal
                        x = Math.Round(x, 1);
                        break;
                    case 1: // chi-squared
                        x = Math.Pow(x, 2);
                        x = Math.Round(x, 1);
                        break;
                    case 2: // Cauchy distribution
                        x = x / y;
                        x = Math.Round(x, 0);
                        break;
                    case 3: // fisher's f
                        x = Math.Pow(x, 2) / Math.Pow(y, 2);
                        x = Math.Round(x, 0);
                        break;
                    case 4: // student's T
                        x = Math.Round(x / Math.Pow(y, 2));
                        // x = (int)x >> 4;
                        break;
                }
                d[x] = d.GetValueOrDefault(x, 0) + 1;
            }
            foreach (double key in d.Keys.ToList())
            {
                d[key] = Math.Round(d[key] / trialCount, 4);
            }

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (virtualWindow.Contains(e.X, e.Y))
            {

                x_mouse = e.X;
                y_mouse = e.Y;

                x_down = virtualWindow.X;
                y_down = virtualWindow.Y;

                r_width = virtualWindow.Width;
                r_height = virtualWindow.Height;

                if (e.Button == MouseButtons.Left)
                {
                    drag = true;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    resizing = true;
                }
            }
            else if (virtualWindowX.Contains(e.X, e.Y))
            {

                x_mouse = e.X;
                y_mouse = e.Y;

                x_down = virtualWindowX.X;
                y_down = virtualWindowX.Y;

                r_width = virtualWindowX.Width;
                r_height = virtualWindowX.Height;

                if (e.Button == MouseButtons.Left)
                {
                    dragX = true;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    resizingX = true;
                }
            }
            else if (virtualWindowY.Contains(e.X, e.Y))
            {

                x_mouse = e.X;
                y_mouse = e.Y;

                x_down = virtualWindowY.X;
                y_down = virtualWindowY.Y;

                r_width = virtualWindowY.Width;
                r_height = virtualWindowY.Height;

                if (e.Button == MouseButtons.Left)
                {
                    dragY = true;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    resizingY = true;
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
            resizing = false;
            dragX = false;
            resizingX = false;
            dragY = false;
            resizingY = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (b == null) return;

            int delta_x = e.X - x_mouse;
            int delta_y = e.Y - y_mouse;



            if (drag)
            {
                virtualWindow.X = x_down + delta_x;
                virtualWindow.Y = y_down + delta_y;
            }
            else if (resizing)
            {

                virtualWindow.Width = r_width + delta_x;
                virtualWindow.Height = r_height + delta_y;
            }
            else if (dragX)
            {
                virtualWindowX.X = x_down + delta_x;
                virtualWindowX.Y = y_down + delta_y;
            }
            else if (resizingX)
            {

                virtualWindowX.Width = r_width + delta_x;
                virtualWindowX.Height = r_height + delta_y;
            }
            else if (dragY)
            {
                virtualWindowY.X = x_down + delta_x;
                virtualWindowY.Y = y_down + delta_y;
            }
            else if (resizingY)
            {

                virtualWindowY.Width = r_width + delta_x;
                virtualWindowY.Height = r_height + delta_y;
            }
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!(ModifierKeys == Keys.Control)) return;
            float stepZoom;
            if (ModifierKeys == (Keys.Shift | Keys.Control))
            {
                stepZoom = 0.01F;
            }
            else
            {
                stepZoom = 0.1F;
            }
            if (!zoom && virtualWindow.Contains(e.X, e.Y))
            {
                zoom = true;

                virtualWindow.Inflate((int)(e.Delta * stepZoom), (int)(e.Delta * stepZoom));

                zoom = false;
            }
            else if (!zoomX && virtualWindowX.Contains(e.X, e.Y))
            {
                zoomX = true;

                virtualWindowX.Inflate((int)(e.Delta * stepZoom), (int)(e.Delta * stepZoom));

                zoomX = false;
            }
            else if (!zoomY && virtualWindowY.Contains(e.X, e.Y))
            {
                zoomY = true;

                virtualWindowY.Inflate((int)(e.Delta * stepZoom), (int)(e.Delta * stepZoom));

                zoomY = false;
            }
        }

        private void generateHistogram(Rectangle r, IDictionary<double, double> d, bool vertical = false)
        {
            if (d == null || d.Count == 0) return;
            int n = d.Count;

            double maxKey = d.Keys.Max();
            double maxValue = d.Values.Max();

            List<Point> points = new List<Point>();

            for (int i = 0; i < n; i++)
            {
                Rectangle rr;
                int left, top, right, bottom;
                if (vertical)
                {
                    left = fromXRealToXVirtual(0, 0, maxValue, r.Left, r.Width);
                    top = fromYRealToYVirtual(i + 1, 0, n, r.Top, r.Height);
                    right = fromXRealToXVirtual(d.ElementAt(i).Value, 0, maxValue, r.Left, r.Width);
                    bottom = fromYRealToYVirtual(i, 0, n, r.Top, r.Height);
                    points.Add(new Point(right, (top + bottom) / 2));
                }
                else
                {
                    left = fromXRealToXVirtual(i, 0, n, r.Left, r.Width);
                    top = fromYRealToYVirtual(d.ElementAt(i).Value, 0, maxValue, r.Top, r.Height);
                    right = fromXRealToXVirtual(i + 1, 0, n, r.Left, r.Width);
                    bottom = fromYRealToYVirtual(0, 0, maxValue, r.Top, r.Height);
                    points.Add(new Point((left + right) / 2, top));
                }
                rr = Rectangle.FromLTRB(left, top, right, bottom);

                g.DrawRectangle(penRectangle, rr);
                g.FillRectangle(new SolidBrush(Color.FromArgb(64, 0, 255, 0)), rr);

                //g.DrawString(vertical ? d.ElementAt(i).Key.ToString() : "" /* d.ElementAt(i).Value.ToString() */, DefaultFont, Brushes.Black, r.Left - 30, vertical ? bottom - Font.Height : top - Font.Height);
                //g.DrawString(vertical ? "" /*d.ElementAt(i).Value.ToString()*/ : d.ElementAt(i).Key.ToString(), DefaultFont, Brushes.Black, vertical ? right : left, r.Bottom);
            }
            g.DrawLines(Pens.Black, points.ToArray());
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);
        }

        private int fromXRealToXVirtual(double x, double minX, double maxX, int left, int w)
        {
            return left + (int)(w * (x - minX) / (maxX - minX));
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            vertical = checkBox1.Checked;
        }

        private int fromYRealToYVirtual(double y, double minY, double maxY, int top, int h)
        {
            return top + (int)(h - h * (y - minY) / (maxY - minY));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            redraw();
        }

        private void redraw()
        {

            g.Clear(BackColor);
            if (ex1)
            {
                generateHistogram(virtualWindowX, dx, false);
                generateHistogram(virtualWindowY, dy, true);
                foreach (PointF point in points)
                {
                    int xDevice = fromXRealToXVirtual(point.X, -1, 1, virtualWindow.Left, virtualWindow.Width);
                    int yDevice = fromYRealToYVirtual(point.Y, -1, 1, virtualWindow.Top, virtualWindow.Height);
                    g.DrawEllipse(Pens.Black, Rectangle.FromLTRB(xDevice - 1, yDevice + 1, xDevice + 1, yDevice - 1));
                }
            }
            else
            {
                generateHistogram(virtualWindow, d, vertical);
            }

            if (ex1) { g.DrawRectangle(Pens.DarkSlateGray, virtualWindow); }

            g.DrawRectangle(Pens.DarkSlateGray, virtualWindowX);
            g.DrawRectangle(Pens.DarkSlateGray, virtualWindowY);
            pictureBox1.Image = b;
        }
    }
}