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
        private int totalFrames = 0;
        public videoPlayerv2(string file)
        {
            InitializeComponent();
            this.file = file;
            try
            {
                _capture = new Capture(file);
                totalFrames = (int)_capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
                Video_CNTRL.Minimum = 0;
                Video_CNTRL.Maximum = totalFrames;
                _capture.ImageGrabbed += ProcessFrame;
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
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
                imageBox1.Image = frame;
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
                this.Refresh();
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

        private void videoPlayerv2_Load(object sender, EventArgs e)
        {

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
    }
}
