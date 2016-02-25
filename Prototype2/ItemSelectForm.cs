using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace Prototype2
{
    public partial class ItemSelectForm : Form
    {
        bool itemselected = false;
        bool radioselected = false;
        int roiView = 0;
        public ItemSelectForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            button1.Enabled = false;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            comboBox3.SelectedIndex = 0;
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            openFileDialog1.Filter = "Video files|*.mp4;*.avi;*.mkv";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;
            if (openFileDialog1.FileName != null)
            {
                itemselected = true;
            }
            if (itemselected && radioselected)
            {
                button1.Enabled = true;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string file = textBox1.Text;
            char[] trim = new char[1] { '"' };
            int frameskip = 0;
            int nn = 25;
            double rescale = 1.0;
            //Form1 testMode;
            videoPlayerv2 testMode;
            Form2 stndMode;
            if (file.EndsWith(".mkv") || file.EndsWith(".mp4") || file.EndsWith(".avi"))
            {
                if (checkBox1.Checked)
                {
                    frameskip = int.Parse(comboBox1.SelectedItem.ToString());
                }
                if (checkBox2.Checked)
                {
                    rescale = double.Parse(comboBox2.SelectedItem.ToString());
                    //MessageBox.Show(rescale.ToString());
                    switch (rescale.ToString())
                    {
                        case "1": nn = 30; break;
                        case "0.9": nn = 30; break;
                        case "0.8": nn = 25; break;
                        case "0.7": nn = 20; break;
                        case "0.6": nn = 20; break;
                        case "0.5": nn = 15; break;
                        case "0.4": nn = 15; break;
                    }
                }
                //MessageBox.Show(frameskip.ToString());
                file = file.Trim(trim);
                if (file == "0" || file == "a")
                    return;
                if (radioButton1.Checked)
                {
                    testMode = new videoPlayerv2(file);
                    testMode.Show();

                }
                else
                {
                    if (checkBox3.Checked)
                        roiView = 1;
                    else if (checkBox4.Checked)
                        roiView = 2;
                    else
                        roiView = 0;

                    //stndMode = new Form2(file);
                    stndMode = new Form2(file, comboBox3.SelectedIndex,roiView);
                    stndMode.Show();
                }
            }
            else
                MessageBox.Show("Invalid Filetype!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked && itemselected)
            {
                button1.Enabled = true;
            }
            if (radioButton1.Checked)
            {
                radioselected = true;
            }
        }


        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked && itemselected)
            {
                button1.Enabled = true;
                checkBox3.Enabled = true;
                checkBox4.Enabled = true;
            }
            if (radioButton2.Checked)
            {
                radioselected = true;
                checkBox3.Enabled = true;
                checkBox4.Enabled = true;
            }
            else
            {
                checkBox3.Enabled = false;
                checkBox4.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                comboBox1.Enabled = true;
                comboBox1.SelectedIndex = 0;
            }
            else
                comboBox1.Enabled = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                comboBox2.Enabled = true;
                comboBox2.SelectedIndex = 0;
            }
            else
                comboBox2.Enabled = false;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
                checkBox4.Enabled = false;
            else
                checkBox4.Enabled = true;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox4.Checked)
                checkBox3.Enabled = false;
            else
                checkBox3.Enabled = true;
        }
    }
}
