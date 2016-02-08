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

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Prototype2
{
    public partial class Form2 : Form
    {
        private string _file;
        private Capture _capture = null;
        private bool _captureInProgress;
        private CascadeClassifier cascadeBreast = new CascadeClassifier("models/cascade_breast.xml");
        private CascadeClassifier cascadePuss = new CascadeClassifier("models/cascade_pussy.xml");
        private CascadeClassifier cascadePen = new CascadeClassifier("models/cascade_pen.xml");
        public int totalBreastCount = 0;
        public int totalPussyCount = 0;
        public int totalDickCount = 0;
        double totalFrameCount;
        double frameNum = 0;
        Stopwatch timer = new Stopwatch();

        public Form2()
        {
            InitializeComponent();
        }
        public Form2(string file)
        {
            InitializeComponent();
            _file = file;
            CvInvoke.UseOpenCL = false;
            try
            {
                frameProcessing();

            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }
        private void frameProcessing()
        {
            _capture = new Capture(_file);
            totalFrameCount = _capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
            progressBar1.Maximum = (int)totalFrameCount;
            timer.Start();
            _capture.ImageGrabbed += ProcessFrame;
            if (timer.ElapsedMilliseconds == 1000) _capture.Stop();
        }
        private void ProcessFrame(object sender, EventArgs arg)
        {
            Mat frame = new Mat();
            Image<Bgr, Byte> frame1;
            frameNum = _capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames);
            _capture.Retrieve(frame, 0);
            frame1 = frame.ToImage<Bgr, Byte>();
            frame1 = frame1.Resize(.5, Emgu.CV.CvEnum.Inter.Cubic);
            frame = frame1.Mat;
            //MessageBox.Show(_capture.Height + " " + _capture.Width + "\n" + frame1.Height + " " + frame1.Width);
            if (frame != null)
            {
                using (UMat ugray = new UMat())
                {
                    CvInvoke.CvtColor(frame, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
                    CvInvoke.EqualizeHist(ugray, ugray);
                    Rectangle[] breastDetected = cascadeBreast.DetectMultiScale(
                       ugray,
                       1.1,
                       30,
                       new Size(20, 20));
                    /*Rectangle[] pussyDetected = cascadePuss.DetectMultiScale(
                       ugray,
                       1.1,
                       30,
                       new Size(20, 20));
                    Rectangle[] dickDetected = cascadePen.DetectMultiScale(
                       ugray,
                       1.1,
                       35,
                       new Size(20, 20));*/
                    progressBar1.Invoke(new MethodInvoker(delegate { progressBar1.Increment(1); label1.Text = frameNum.ToString();}));
                    
                }
                
            }
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_capture != null)
            {
                if (_captureInProgress)
                {  //stop the capture
                    button1.Text = "Start";
                    //reset.Show();
                    _capture.Pause();
                }
                else
                {
                    //start the capture
                    button1.Text = "Stop";
                    _capture.Start();

                }

                _captureInProgress = !_captureInProgress;
            }
        }
    }
}
