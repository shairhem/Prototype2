using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prototype2
{
    public partial class itemBrowser : Form
    {
        public itemBrowser()
        {
            InitializeComponent();
        }
        string container;
        detectInfo detect = new detectInfo();
        public itemBrowser(string container)
        {
            InitializeComponent();
            this.CenterToScreen();
            textBox2.Text = detect.readConfig(container);
            this.container = container;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            detect.writeConfig(container,textBox1.Text);
            if(MessageBox.Show("done") == DialogResult.OK)
            {
                this.Close();
            }
        }
    }
}
