namespace Homework_2_RANDOMTIMER
{
    public partial class Form1 : Form
    {
        Random random;
        List<string> values;
        public Form1()
        {
            InitializeComponent();
            random = new Random();
            values = new List<string>();
            values.Add("Automobile");
            values.Add("Treno");
            values.Add("Bicicletta");
            values.Add("Aereo");
            values.Add("Autobus");
            timer1.Interval = 500;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            richTextBox1.AppendText(values.ElementAt(random.Next() % values.Count()) + Environment.NewLine);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = button1.Text == "Start" ? "Stop" : "Start";
            if (timer1.Enabled) timer1.Stop(); else timer1.Start();
        }
    }
}