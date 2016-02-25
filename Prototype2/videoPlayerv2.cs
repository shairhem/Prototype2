using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
//emgu
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;


namespace Prototype2
{
    public partial class videoPlayerv2 : Form
    {
        private Capture _capture = null;
        private bool _captureInProgress;
        public string file = null;
        private int fps = 0;
        private int totalFrames = 0;
        private int roiView = 0;
        //list
        List<frameInfo> frameInfoList;
        List<frameInfo> list = new List<frameInfo>();

        //shapes
        Rectangle[] boobs = null;
        Rectangle[] pussy = null;
        Rectangle[] dick = null;

        public videoPlayerv2(string file, List<frameInfo> frameInfoList)
        {
            InitializeComponent();
            this.file = file;
            this.frameInfoList = frameInfoList;
            try
            {
                frameProcessing();   
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }

        public videoPlayerv2(string file, List<frameInfo> frameInfoList,int roiView)
        {
            InitializeComponent();
            this.file = file;
            this.frameInfoList = frameInfoList;
            try
            {
                frameProcessing();
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }

        public void frameProcessing()
        {
            _capture = new Capture(file);
            totalFrames = (int)_capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
            fps = (int)_capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
            fillDataGrid(frameInfoList, fps, totalFrames);
            Video_CNTRL.Minimum = 0;
            Video_CNTRL.Maximum = totalFrames;
            _capture.ImageGrabbed += ProcessFrame;
        }

        private void videoPlayerv2_Load(object sender, EventArgs e)
        {
            
        }

        private void fillDataGrid(List<frameInfo> frameInfoList, int fps, int frameCount)
        {
            //MessageBox.Show(frameInfoList.Count.ToString() + "dit");
            foreach (frameInfo f in frameInfoList)
            {
                list.Add(new frameInfo(getPosition(f.frameNum, fps), f.frameNum, f.boobDetected, f.pussDetected, f.penDetected));
            }
            dataGridView1.DataSource = list;
            dataGridView1.Columns["frameNum"].Visible = false;
        }

        public string getPosition(int frameNum, int fps)
        {
            double sec = getTimeFrame(frameNum, fps);
            //MessageBox.Show(sec.ToString());
            TimeSpan time = TimeSpan.FromSeconds(sec);
            //return time.Milliseconds.ToString();
            return time.ToString(@"hh\:mm\:ss\.ffff");
        }

        public double getTimeFrame(int frameNum, int fps)
        {
            double seconds = 0;
            seconds = (double)Decimal.Divide(frameNum, fps);
            //MessageBox.Show(frameNum + " " + fps + " " + seconds + "  ");
            return seconds;
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            try
            {                
                //display image
                Mat frame = new Mat();
                _capture.Retrieve(frame,0);
                DisplayImage(frame);
                
                //show time stamp
                double timeIndex = _capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosMsec);
                updateTextBox("Time: " + TimeSpan.FromMilliseconds(timeIndex).ToString(), label1);

                //show frame number
                double framenumber = _capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames);
                updateTextBox("Frame: " + framenumber.ToString(), label2);

                double frameRate = _capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);

                //update trackbar
                UpdateVideo_CNTRL(framenumber);

                //Wait to display correct framerate
                Thread.Sleep((int)(1000.0 / frameRate));

                if (framenumber == totalFrames)
                {
                    framenumber = 0;
                    button1.BackgroundImage = Prototype2.Properties.Resources.widget_play_pressed;
                    _captureInProgress = !_captureInProgress;
                    UpdateVideo_CNTRL(framenumber);
                    _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, framenumber);
                    //call the process frame to update the picturebox
                    _capture.Pause();
                    ProcessFrame(null, null);
                }
            }
            catch { }
        }

        private delegate void DisplayImageDelegate(Mat frame);
        private void DisplayImage(Mat frame)
        {
            if (imageBox1.InvokeRequired)
            {
                try
                {
                    DisplayImageDelegate DI = new DisplayImageDelegate(DisplayImage);
                    this.BeginInvoke(DI, new object[] { frame });
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                if(roiView == 0)
                    imageBox1.Image = frame;
                else if(roiView == 1)
                {
                    foreach (Rectangle b in boobs)
                    {
                        CvInvoke.Rectangle(frame, b, new Bgr(Color.Red).MCvScalar, 2);
                    }
                    foreach (Rectangle b in dick)
                    {
                        CvInvoke.Rectangle(frame, b, new Bgr(Color.Blue).MCvScalar, 2);
                    }
                    foreach (Rectangle b in pussy)
                    {
                        CvInvoke.Rectangle(frame, b, new Bgr(Color.Green).MCvScalar, 2);
                    }
                    imageBox1.Image = frame;
                }
                else
                {

                    foreach (Rectangle b in boobs)
                    {
                        
                        CvInvoke.Rectangle(frame, b, new Bgr(Color.Red).MCvScalar, 0);
                    }
                    foreach (Rectangle b in dick)
                    {
                        CvInvoke.Rectangle(frame, b, new Bgr(Color.Blue).MCvScalar, 0);
                    }
                    foreach (Rectangle b in pussy)
                    {
                        CvInvoke.Rectangle(frame, b, new Bgr(Color.Green).MCvScalar, 0);
                    }
                    imageBox1.Image = frame;
                }
            }
        }

        private delegate void updateTextBoxDelegate(String Text, Label Control);
        private void updateTextBox(String Text, Label Control)
        {
            if (Control.InvokeRequired)
            {
                try
                {
                    updateTextBoxDelegate UT = new updateTextBoxDelegate(updateTextBox);
                    this.BeginInvoke(UT, new object[] { Text, Control });
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                Control.Text = Text;
                label1.Refresh();
                label2.Refresh();
            }
        }

        private delegate void UpdateVideo_CNTRLDelegate(double Value);
        private void UpdateVideo_CNTRL(double Value)
        {
            if (Video_CNTRL.InvokeRequired)
            {
                try
                {
                    UpdateVideo_CNTRLDelegate UVC = new UpdateVideo_CNTRLDelegate(UpdateVideo_CNTRL);
                    this.BeginInvoke(UVC, new object[] { Value });
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                //Do a quick in range check as sometime the codec may not tell the truth
                if (Value < Video_CNTRL.Maximum) Video_CNTRL.Value = (int)Value;
            }
        }
        /// <summary>
        /// Threadsafe method toe Enable/Disable the Video_CNTRL trackbar
        /// </summary>
        /// <param name="State"></param>
        private delegate void EnableVideo_CNTRLDelegate(bool State);
        private void UpdateVideo_CNTRL(bool State)
        {
            if (Video_CNTRL.InvokeRequired)
            {
                try
                {
                    EnableVideo_CNTRLDelegate UVC = new EnableVideo_CNTRLDelegate(UpdateVideo_CNTRL);
                    this.BeginInvoke(UVC, new object[] { State });
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                //Do a quick in range check as sometime the codec may not tell the truth
                Video_CNTRL.Enabled = State;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("pasok");
            if (_capture != null)
            {
                if (_captureInProgress)
                {  //stop the capture
                    button1.BackgroundImage = Prototype2.Properties.Resources.widget_play_pressed;
                    //reset.Show();
                    _capture.Pause();
                }
                else
                {
                    //start the capture
                    button1.BackgroundImage = Prototype2.Properties.Resources.widget_pause_pressed;
                    _capture.Start();
                }

                _captureInProgress = !_captureInProgress;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            //fillDataGrid(frameInfoList, fps, totalFrames);
            List<String> fuck = new List<String>();
            loadingForm loadForm = new loadingForm(frameInfoList.Count);
            loadForm.Show();
            foreach (frameInfo x in frameInfoList)
            {
                _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, x.frameNum);
                fuck.Add(_capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosMsec).ToString() + "|| x.frameNum");
                loadForm.increment(1);
            }
            dataGridView1.DataSource = fuck;
            _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, 0);
            //dataGridView1.Columns["frameNum"].Visible = false;

        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int frameNum = list.ElementAt(e.RowIndex).frameNum;
            //MessageBox.Show(frameNum.ToString());
            double timeFrame = getTimeFrame(frameNum, fps);
            timeFrame = timeFrame * 1000;
            _capture.Pause();
            _capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosMsec, timeFrame);
            _capture.Start();
            //MessageBox.Show(timeFrame.ToString());
        }
    }
}
