namespace Homework_4
{
    public partial class Form1 : Form
    {
        Random r;
        Bitmap b;
        Graphics g;
        Pen penAbsolute = new Pen(Color.Black, 0.2f);
        Pen penRelative = new Pen(Color.Red, 0.2f);
        Pen penNormalized = new Pen(Color.Blue, 0.2f);

        Pen penRectangle = new Pen(Color.Green, 0.2f);

        public Form1()
        {
            InitializeComponent();
            r = new Random();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //button1.Text = button1.Text == "Start" ? "Stop" : "Start";
            //if (timer1.Enabled) timer1.Stop(); else timer1.Start();

            richTextBox1.Clear();
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(Color.White);
            pictureBox1.Image = b;

            Rectangle virtualWindow = new Rectangle(20, 20, b.Width - 40, b.Height - 40);

            g.DrawRectangle(Pens.DarkSlateGray, virtualWindow);

            int trialCount = 10;
            double d;
            double y = 0;

            Point[] pointsAbsolute = new Point[trialCount];
            Point[] pointsRelative = new Point[trialCount];
            Point[] pointsNormalized = new Point[trialCount];

            int numberOfExperiments = 5000;

            double distLimit0 = 0,
                distLimit1 = (trialCount - 1) * 1 / 5d,
                distLimit2 = (trialCount - 1) * 2 / 5d,
                distLimit3 = (trialCount - 1) * 3 / 5d,
                distLimit4 = (trialCount - 1) * 4 / 5d,
                distLimit5 = trialCount - 1;

            int rectValue1 = 0,
                rectValue2 = 0,
                rectValue3 = 0,
                rectValue4 = 0,
                rectValue5 = 0;

            for (int i = 1; i <= numberOfExperiments; i++)
            {
                Array.Clear(pointsAbsolute, 0, pointsAbsolute.Length);
                Array.Clear(pointsRelative, 0, pointsRelative.Length);
                Array.Clear(pointsNormalized, 0, pointsNormalized.Length);
                y = 0;
                for (int x = 0; x < trialCount; x++)
                {
                    d = r.NextDouble();
                    if (d >= 0.5)
                    {
                        y++;

                    }
                    int xAbsolute = fromXRealToXVirtual(x, 0, trialCount - 1, virtualWindow.Left, virtualWindow.Width);
                    int yAbsolute = fromYRealToYVirtual(y, 0, trialCount, virtualWindow.Top, virtualWindow.Height);

                    int xRelative = fromXRealToXVirtual(x, 0, trialCount - 1, virtualWindow.Left, virtualWindow.Width);
                    int yRelative = fromYRealToYVirtual(y / (x + 1), 0, 1, virtualWindow.Top, virtualWindow.Height);

                    int xNormalized = fromXRealToXVirtual(x, 0, trialCount - 1, virtualWindow.Left, virtualWindow.Width);
                    int yNormalized = fromYRealToYVirtual(y / Math.Sqrt(x + 1), 0, trialCount / Math.Sqrt(trialCount), virtualWindow.Top, virtualWindow.Height);

                    pointsAbsolute[x] = new Point(xAbsolute, yAbsolute);
                    pointsRelative[x] = new Point(xRelative, yRelative);
                    pointsNormalized[x] = new Point(xNormalized, yNormalized);

                    if(!checkBox1.Checked)
                        richTextBox1.AppendText("E" + i + "T" + (x + 1) + ": " + y + Environment.NewLine);

                }
                g.DrawLines(penAbsolute, pointsAbsolute);
                g.DrawLines(penRelative, pointsRelative);
                g.DrawLines(penNormalized, pointsNormalized);
                
                double lastTrialResult = y;

               
                if(lastTrialResult <= distLimit1)
                {
                    rectValue1++;
                }
                else if (distLimit1 <= lastTrialResult && lastTrialResult < distLimit2)
                {
                    rectValue2++;
                }
                else if (distLimit2 <= lastTrialResult && lastTrialResult < distLimit3)
                {
                    rectValue3++;
                }
                else if (distLimit3 <= lastTrialResult && lastTrialResult < distLimit4)
                {
                    rectValue4++;
                }
                else
                {
                    rectValue5++;
                }


            }

            Rectangle rectangle1 = Rectangle.FromLTRB(fromXRealToXVirtual(0, 0, numberOfExperiments, virtualWindow.Left, virtualWindow.Width),
                fromYRealToYVirtual(distLimit1, 0, trialCount - 1, virtualWindow.Top, virtualWindow.Height),
                fromXRealToXVirtual(rectValue1, 0, numberOfExperiments, virtualWindow.Left, virtualWindow.Width),
                fromYRealToYVirtual(distLimit0, 0, trialCount - 1, virtualWindow.Top, virtualWindow.Height));
            g.DrawRectangle(penRectangle, rectangle1);
            g.FillRectangle(new SolidBrush(Color.FromArgb(64, 0, 255, 0)), rectangle1);

            Rectangle rectangle2 = Rectangle.FromLTRB(fromXRealToXVirtual(0, 0, numberOfExperiments, virtualWindow.Left, virtualWindow.Width),
                fromYRealToYVirtual(distLimit2, 0, trialCount - 1, virtualWindow.Top, virtualWindow.Height),
                fromXRealToXVirtual(rectValue2, 0, numberOfExperiments, virtualWindow.Left, virtualWindow.Width),
                fromYRealToYVirtual(distLimit1, 0, trialCount - 1, virtualWindow.Top, virtualWindow.Height));
            g.DrawRectangle(penRectangle, rectangle2);
            g.FillRectangle(new SolidBrush(Color.FromArgb(64, 0, 255, 0)), rectangle2);

            Rectangle rectangle3 = Rectangle.FromLTRB(fromXRealToXVirtual(0, 0, numberOfExperiments, virtualWindow.Left, virtualWindow.Width),
                fromYRealToYVirtual(distLimit3, 0, trialCount - 1, virtualWindow.Top, virtualWindow.Height),
                fromXRealToXVirtual(rectValue3, 0, numberOfExperiments, virtualWindow.Left, virtualWindow.Width),
                fromYRealToYVirtual(distLimit2, 0, trialCount - 1, virtualWindow.Top, virtualWindow.Height));
            g.DrawRectangle(penRectangle, rectangle3);
            g.FillRectangle(new SolidBrush(Color.FromArgb(64, 0, 255, 0)), rectangle3);

            Rectangle rectangle4 = Rectangle.FromLTRB(fromXRealToXVirtual(0, 0, numberOfExperiments, virtualWindow.Left, virtualWindow.Width),
                fromYRealToYVirtual(distLimit4, 0, trialCount - 1, virtualWindow.Top, virtualWindow.Height),
                fromXRealToXVirtual(rectValue4, 0, numberOfExperiments, virtualWindow.Left, virtualWindow.Width),
                fromYRealToYVirtual(distLimit3, 0, trialCount - 1, virtualWindow.Top, virtualWindow.Height));
            g.DrawRectangle(penRectangle, rectangle4);
            g.FillRectangle(new SolidBrush(Color.FromArgb(64, 0, 255, 0)), rectangle4);

            Rectangle rectangle5 = Rectangle.FromLTRB(fromXRealToXVirtual(0, 0, numberOfExperiments, virtualWindow.Left, virtualWindow.Width),
                fromYRealToYVirtual(distLimit5, 0, trialCount - 1, virtualWindow.Top, virtualWindow.Height),
                fromXRealToXVirtual(rectValue5, 0, numberOfExperiments, virtualWindow.Left, virtualWindow.Width),
                fromYRealToYVirtual(distLimit4, 0, trialCount - 1, virtualWindow.Top, virtualWindow.Height));
            g.DrawRectangle(penRectangle, rectangle5);
            g.FillRectangle(new SolidBrush(Color.FromArgb(64, 0, 255, 0)), rectangle5);

            richTextBox1.AppendText("number of experiments: " + numberOfExperiments + Environment.NewLine +
                                    "number of trials for each experiment: " + trialCount + Environment.NewLine);
        }


        private int fromXRealToXVirtual(double x, double minX, double maxX, int left, int w)
        {
            return left + (int)( w * (x - minX) / (maxX - minX));
        }

        private int fromYRealToYVirtual(double y, double minY, double maxY, int top, int h)
        {
            return top + (int)( h - h * (y - minY) / (maxY - minY));
        }
    }
}