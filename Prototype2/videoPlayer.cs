using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Prototype2
{
    public partial class videoPlayer : Form
    {
        public videoPlayer()
        {
            InitializeComponent();
        }
        public videoPlayer(List<frameInfo> frameInfoList,int fps,string filename, int totalFrames)
        {
            InitializeComponent();
            //this.Text = fps.ToString();
            axWindowsMediaPlayer1.URL = @filename;
            ArrayList list = new ArrayList();

            foreach (frameInfo f in frameInfoList)
            {
                list.Add(new frameInfo(f.frameNum,f.boobDetected,f.pussDetected,f.penDetected));
            }
            dataGridView1.DataSource = list;
        }

        //public float getSeconds(int frameNum, int fps, int totalFrames)
        //{

        //}

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(axWindowsMediaPlayer1.Ctlcontrols.currentPosition.ToString());
        }
    }
}
