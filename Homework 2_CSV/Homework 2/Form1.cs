using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using static System.Windows.Forms.LinkLabel;

namespace Homework_2
{
    public partial class Form1 : Form
    {
        Random random;
        IEnumerable<string> lines;
        public Form1()
        {
            InitializeComponent();
            random = new Random();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            //label1.Text = (random.Next() % 10).ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Int64 range1=0, range2=0, range3=0;
            int limit1=65, limit2=69;
            int column=1;
            bool skipHeader = true;

            richTextBox1.Clear();
            if (lines == null) return;
            foreach (var line in lines.Skip(skipHeader?1:0))
            {
                var list = line.Split(',');

                if (double.Parse(list[column], CultureInfo.InvariantCulture) < limit1)
                    range1++;
                else if (double.Parse(list[column], CultureInfo.InvariantCulture) < limit2)
                    range2++;
                else
                    range3++;

                richTextBox1.AppendText(list[column] + Environment.NewLine);
                richTextBox2.Clear();
                richTextBox2.AppendText("0 ≤ x < " + limit1 + ": " + range1 + Environment.NewLine);
                richTextBox2.AppendText(limit1 + " ≤ x < " + limit2 + ": " + range2 + Environment.NewLine);
                richTextBox2.AppendText("x ≥ " + limit2 + ": " + range3 +  Environment.NewLine);

            }
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                string filename = openFileDialog1.SafeFileName;
                try
                {
                    textBox1.Text = filename;
                    lines = File.ReadLines(path);
                    button1.Enabled = true;
                }
                catch (IOException)
                {
                }
            }
        }
    }
}