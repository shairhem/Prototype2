using System;
using System.Windows.Forms;
using Prototype2.Views;

namespace Prototype2
{
    public partial class StartPage : Form
    {
        public StartPage()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void StartPage_Load(object sender, EventArgs e)
        {

        }

        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Video files|*.mp4;*.avi;*.mkv";
            openFileDialog.FilterIndex = 1;
            openFileDialog.FileName = "";
            openFileDialog.ShowDialog();
            textBox1.Text = openFileDialog.FileName;

            if (!string.IsNullOrWhiteSpace(openFileDialog.FileName))
            {
                ScanButton.Enabled = true;
            }
        }

        private void ScanButton_Click(object sender, EventArgs e)
        {
            char[] trim = new char[1] { '"' };
            var file = textBox1.Text.Trim(trim);
            var player = new Player(file);
            this.Hide();
            player.Show();

        }
    }
}
