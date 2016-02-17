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
    public partial class videoPlayer : Form
    {
        public videoPlayer()
        {
            InitializeComponent();
        }

        public videoPlayer(List<frameInfo> frameInfoList,int fps)
        {
            InitializeComponent();
            string temp = "";
            foreach(frameInfo f in frameInfoList)
            {
                temp += f.frameNum + " " + f.boobDetected + " " + f.pussDetected + " " + f.penDetected;
                temp += "\n";
            }
            textBox1.Text = temp;
        }
    }
}
