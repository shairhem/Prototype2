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
    public partial class loadingForm : Form
    {
        public loadingForm()
        {
            InitializeComponent();
        }
        public loadingForm(int max)
        {
            InitializeComponent();
            progressBar1.Maximum = max;
        }
        public void increment(int x)
        {
            progressBar1.Increment(1);
        }

        private void loadingForm_Load(object sender, EventArgs e)
        {

        }
    }
}
