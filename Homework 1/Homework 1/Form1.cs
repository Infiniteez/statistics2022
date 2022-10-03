namespace Homework_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void checkBoxAccept_CheckedChanged(object sender, EventArgs e)
        {
            this.buttonSend.Enabled = this.checkBoxAccept.Checked;
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Clear();
        }

        private void buttonReset_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(buttonReset, "Clears the above textbox");
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Enabled = false;
            this.checkBoxAccept.Enabled = false;
            this.buttonReset.Enabled = false;
            this.buttonSend.Enabled = false;
        }

        private void buttonSend_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(buttonSend, "Sends the form");
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonClose_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(buttonClose, "Closes the app");
        }
    }
}