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

        int fps = 0;
        List<frameInfo> list = new List<frameInfo>();
        public videoPlayer(List<frameInfo> frameInfoList,int fps,string filename, int totalFrames)
        {
            InitializeComponent();
            //this.Text = fps.ToString();
            axWindowsMediaPlayer1.URL = @filename;
            this.fps = fps;
            //ArrayList list = new ArrayList();
            
            foreach (frameInfo f in frameInfoList)
            {
                list.Add(new frameInfo(getPosition(f.frameNum,fps),f.frameNum, f.boobDetected, f.pussDetected, f.penDetected));
            }
            dataGridView1.DataSource = list;
            dataGridView1.Columns["frameNum"].Visible = false;
            
        }


        public string getPosition(int frameNum,int fps)
        {
            double sec = getTimeFrame(frameNum,fps);
            TimeSpan time = TimeSpan.FromSeconds(sec);

            return time.ToString(@"hh\:mm\:ss\:fff");
        }

        public double getTimeFrame(int frameNum, int fps)
        {
            double seconds = 0;
            seconds = frameNum / fps;
            //MessageBox.Show(seconds + "  ");
            
            return seconds;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(axWindowsMediaPlayer1.Ctlcontrols.currentPosition.ToString());
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int frameNum = list.ElementAt(e.RowIndex).frameNum;
            //MessageBox.Show(frameNum.ToString());
            double timeFrame = getTimeFrame(frameNum, fps);
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = timeFrame;
        }
    }
}
