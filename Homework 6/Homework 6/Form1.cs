using System.Data.Common;
using System.Globalization;
using static System.Windows.Forms.LinkLabel;

namespace Homework_6
{
    public partial class Form1 : Form
    {
        Random r = new Random();
        Bitmap b;
        Graphics g;
        Rectangle virtualWindow;
        Rectangle virtualWindow2;
        List<string> lines;

        List<double> p = new List<double>();

        SortedDictionary<double, long> distOfAvgs = new SortedDictionary<double, long>();
        SortedDictionary<double, long> distOfVars = new SortedDictionary<double, long>();

        bool vertical = false;

        int x_mouse, y_mouse;
        int x_down, y_down;

        int r_width, r_height;

        bool drag = false;
        bool resizing = false;
        bool drag2 = false;
        bool resizing2 = false;

        bool zoom;
        bool zoom2;

        Pen penRectangle = new Pen(Color.Green, 0.2f);


        double avg = 0;

        int numberOfExperiments = 100000;
        int sampleSize = 5;

        public Form1()
        {
            InitializeComponent();
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);

            pictureBox1.Image = b;
            virtualWindow = new Rectangle(20, 20, b.Width - 40, b.Height - 40);
            virtualWindow2 = new Rectangle(20, 300, b.Width - 40, b.Height - 40);
            timer1.Enabled = true;
            timer1.Interval = 16;
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
            else if (virtualWindow2.Contains(e.X, e.Y))
            {

                x_mouse = e.X;
                y_mouse = e.Y;

                x_down = virtualWindow2.X;
                y_down = virtualWindow2.Y;

                r_width = virtualWindow2.Width;
                r_height = virtualWindow2.Height;

                if (e.Button == MouseButtons.Left)
                {
                    drag2 = true;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    resizing2 = true;
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
            resizing = false;
            drag2 = false;
            resizing2 = false;
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

            if (drag2)
            {
                virtualWindow2.X = x_down + delta_x;
                virtualWindow2.Y = y_down + delta_y;
            }
            else if (resizing2)
            {

                virtualWindow2.Width = r_width + delta_x;
                virtualWindow2.Height = r_height + delta_y;
            }

        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!(ModifierKeys == Keys.Control)) return;
            if (!zoom && virtualWindow.Contains(e.X, e.Y))
            {
                zoom = true;

                float stepZoom;
                if (ModifierKeys == (Keys.Shift | Keys.Control))
                {
                    stepZoom = 0.01F;
                }
                else
                {
                    stepZoom = 0.1F;
                }

                virtualWindow.Inflate((int)(e.Delta * stepZoom), (int)(e.Delta * stepZoom));

                zoom = false;
            }
            else if (!zoom2 && virtualWindow2.Contains(e.X, e.Y))
            {
                zoom2 = true;

                float stepZoom;
                if (ModifierKeys == (Keys.Shift | Keys.Control))
                {
                    stepZoom = 0.01F;
                }
                else
                {
                    stepZoom = 0.1F;
                }

                virtualWindow2.Inflate((int)(e.Delta * stepZoom), (int)(e.Delta * stepZoom));

                zoom2 = false;
            }
        }

        private void generateHistogram(Rectangle r, IDictionary<double, long> d, bool vertical = false)
        {
            if (d == null || d.Count==0) return;
            int n = d.Count;

            double maxKey = d.Keys.Max();
            double maxValue = d.Values.Max();
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
                }
                else
                {
                    left = fromXRealToXVirtual(i, 0, n, r.Left, r.Width);
                    top = fromYRealToYVirtual(d.ElementAt(i).Value, 0, maxValue, r.Top, r.Height);
                    right = fromXRealToXVirtual(i + 1, 0, n, r.Left, r.Width);
                    bottom = fromYRealToYVirtual(0, 0, maxValue, r.Top, r.Height);
                }
                rr = Rectangle.FromLTRB(left, top, right, bottom);

                g.DrawRectangle(penRectangle, rr);
                g.FillRectangle(new SolidBrush(Color.FromArgb(64, 0, 255, 0)), rr);

                g.DrawString(vertical ? d.ElementAt(i).Key.ToString() : d.ElementAt(i).Value.ToString(), DefaultFont, Brushes.Black, r.Right, vertical ? (top + bottom) / 2 : top);
                g.DrawString(vertical ? d.ElementAt(i).Value.ToString() : d.ElementAt(i).Key.ToString(), DefaultFont, Brushes.Black, vertical ? right : (left + right) / 2, r.Bottom);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            redraw();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            vertical = checkBox1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                string filename = openFileDialog1.SafeFileName;
                try
                {
                    lines = File.ReadLines(path).ToList();
                    int n = lines.Count()-1;
                    button1.Enabled = true;
                    int i = 0;
                    foreach (var line in lines.Skip(1))
                    {
                        i++;
                        var list = line.Split(',');
                        int col = 3;

                        
                        double key = double.Parse(list[col], CultureInfo.InvariantCulture);

                        p.Add(key);

                        avg = avg + (key - avg) / i;
                    }
                }
                catch (IOException)
                {
                }
                
                List<double> listOfAvgs = new List<double>();
                List<double> listOfVars = new List<double>();

                for (int m=0; m < numberOfExperiments; m++)
                {
                    List<Double> sample = p.OrderBy(x => r.Next()).Take(sampleSize).ToList();

                    double avg = sample.Average();
                    int avgApprox = (int)Math.Truncate(avg) >> 1 << 1;

                    if (distOfAvgs.ContainsKey(avgApprox))
                    {
                        distOfAvgs[avgApprox]++;
                    }
                    else
                    {
                        distOfAvgs.Add(avgApprox, 1);
                    }

                    listOfAvgs.Add(avg);


                    double var = sample.Average(v => Math.Pow(v - avg, 2));
                    int varApprox = (int)Math.Truncate(var) >> 3 << 3;

                    if (distOfVars.ContainsKey(varApprox))
                    {
                        distOfVars[varApprox]++;
                    }
                    else
                    {
                        distOfVars.Add(varApprox, 1);
                    }

                    listOfVars.Add(var);

                }

                double avgOfAvgs = listOfAvgs.Average();
                double avgOfVars = listOfVars.Average();

                double avgOfPopulation = p.Average();
                double varOfPopulation = p.Average(v => Math.Pow(v - avgOfPopulation, 2));

                richTextBox1.Clear();
                richTextBox1.AppendText("Avg of population:" + avgOfPopulation.ToString() + Environment.NewLine);
                richTextBox1.AppendText("Var of population:" + varOfPopulation.ToString() + Environment.NewLine);
                richTextBox1.AppendText("Avg of avgs:" + avgOfAvgs.ToString() + Environment.NewLine);
                richTextBox1.AppendText("Var of avgs:" + listOfAvgs.Average(v => Math.Pow(v - avgOfAvgs, 2)).ToString() + Environment.NewLine);
                richTextBox1.AppendText("Avg of vars:" + avgOfVars.ToString() + Environment.NewLine);
                richTextBox1.AppendText("Var of vars:" + listOfVars.Average(v => Math.Pow(v - avgOfVars, 2)).ToString() + Environment.NewLine);


            }
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

        private int fromYRealToYVirtual(double y, double minY, double maxY, int top, int h)
        {
            return top + (int)(h - h * (y - minY) / (maxY - minY));
        }

        private void redraw()
        {

            g.Clear(BackColor);
            generateHistogram(virtualWindow, distOfAvgs, vertical);
            generateHistogram(virtualWindow2, distOfVars, vertical);
            g.DrawRectangle(Pens.DarkSlateGray, virtualWindow);
            g.DrawRectangle(Pens.DarkSlateGray, virtualWindow2);
            pictureBox1.Image = b;
        }
    }
}