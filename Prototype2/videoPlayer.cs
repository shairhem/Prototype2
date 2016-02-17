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

        public videoPlayer(List<frameInfo> frameInfoList,int fps,string filename)
        {
            InitializeComponent();
            axWindowsMediaPlayer1.URL = @filename;
            ArrayList list = new ArrayList();

            foreach (frameInfo f in frameInfoList)
            {
                list.Add(new frameInfo(f.frameNum,f.boobDetected,f.pussDetected,f.penDetected));
            }
            dataGridView1.DataSource = list;
        }
        
    }
}
