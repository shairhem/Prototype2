using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using System.IO;

namespace Prototype2
{
    public partial class Form1 : Form
    {
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
        string _file;
        int _frameskip = 0;
        int frameCtr = 0;
        int _nn = 0;
        double _rescale = 1.0;
        bool _pauseAtDetection = false;
        bool freezeFrame = false;
        StreamWriter writer;

        public Form1()
        {
            InitializeComponent();
        }
        public Form1(string file,int frameskip, double rescale,bool pauseAtDetection,int nn)
        {
            InitializeComponent();
            //reset.Hide();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            _file = file;
            _frameskip = frameskip;
            _rescale = rescale;
            _pauseAtDetection = pauseAtDetection;
            _nn = nn;
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
            _capture.ImageGrabbed += ProcessFrame;
        }
        private void ProcessFrame(object sender, EventArgs arg)
        {
            Mat frame = new Mat();
            frameNum = _capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames);
            Image<Bgr, Byte> frame1;
            int breastCount = 0;
            int pussyCount = 0;
            int dickCount = 0;
            string temp = "";
            _capture.Retrieve(frame, 0);
            frame1 = frame.ToImage<Bgr, Byte>();
            frame1 = frame1.Resize(_rescale, Emgu.CV.CvEnum.Inter.Cubic);
            frame = frame1.Mat;
            //MessageBox.Show(_nn.ToString());
            if (frame != null && frameCtr == _frameskip)
            {
                frameCtr = 0;
                 using(UMat ugray = new UMat())
                {
                    CvInvoke.CvtColor(frame, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
                    CvInvoke.EqualizeHist(ugray, ugray);
                    Rectangle[] breastDetected = cascadeBreast.DetectMultiScale(
                       ugray,
                       1.1,
                       _nn,
                       new Size(20, 20));
                    Rectangle[] pussyDetected = cascadePuss.DetectMultiScale(
                       ugray,
                       1.1,
                       _nn,
                       new Size(20, 20));
                    Rectangle[] dickDetected = cascadePen.DetectMultiScale(
                       ugray,
                       1.1,
                       50,
                       new Size(20, 20));
                    foreach (Rectangle b in breastDetected)
                    {
                        CvInvoke.Rectangle(frame, b, new Bgr(Color.Red).MCvScalar, 2);
                        
                    }
                    foreach (Rectangle b in pussyDetected)
                    {
                        CvInvoke.Rectangle(frame, b, new Bgr(Color.Blue).MCvScalar, 2);
                    }
                    foreach (Rectangle b in dickDetected)
                    {
                        CvInvoke.Rectangle(frame, b, new Bgr(Color.Green).MCvScalar, 2);
                    }
                     breastCount = breastDetected.Length;
                     pussyCount = pussyDetected.Length;
                     dickCount = dickDetected.Length;
                     totalBreastCount += breastCount;
                     totalPussyCount += pussyCount;
                     totalDickCount += dickCount;
                     if ((breastCount > 0 || pussyCount > 0 || dickCount > 0) && _pauseAtDetection)
                     {
                         _capture.Pause();
                         playToggle.Invoke(new MethodInvoker(delegate { playToggle.Text = "Start"; }));
                         _captureInProgress = false;
                         if (breastCount > 0)
                         {
                             temp += ""+ breastCount + "breast(s) found\n"; 
                         }
                         if (pussyCount > 0)
                         {
                             temp += ""+ pussyCount+"pussy(s) found\n";
                         }
                         if (dickCount > 0)
                         {
                             temp += "" + dickCount + "dick(s) found\n";
                         }
                         MessageBox.Show(temp);
                     }
                }            
            }
            if (_frameskip > 0)
            {
                frameCtr++;
            }
            label4.Invoke(new MethodInvoker(delegate { label4.Text = frameNum.ToString(); logger(frameNum, breastCount, pussyCount,dickCount); totalBreast.Text = totalBreastCount.ToString(); totalF.Text = totalPussyCount.ToString(); totalG.Text = totalDickCount.ToString(); }));
            imgBox.Image = frame;
            
        }
        private void logger(double frameCount, int breastCount, int fGenCount, int mGenCount)
        {
            string text = "=frame " + frameCount + ": Breast: " + breastCount + " || Female: "  + fGenCount + " || Male: " + mGenCount+  " \n";
            logbox.AppendText(text);
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void playToggle_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("pasok");
            if (_capture != null)
            {
                if (_captureInProgress)
                {  //stop the capture
                    playToggle.Text = "Start";
                    //reset.Show();
                    _capture.Pause();
                }
                else
                {
                    //start the capture
                    playToggle.Text = "Stop";
                    _capture.Start();
                }

                _captureInProgress = !_captureInProgress;
            }
        }
        private void reset_Click(object sender, EventArgs e)
        {
            _capture.Dispose();
            //playToggle.Text = "Start";
            frameProcessing();
            _capture.Start();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _capture.Pause();
            MessageBox.Show("Stopping playback", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
            MessageBoxDefaultButton.Button1);
            _capture.Dispose();

        }
        private void label5_Click(object sender, EventArgs e)
        {
            string temp = "";
            string[] separators = { "=" };
            string[] result;
            bool firstLine = true;

            if (!_captureInProgress)
            {
                temp = "logs/log" + DateTime.Now.ToFileTime().ToString() + ".txt";
                result = logbox.Text.Split(separators, StringSplitOptions.None);
                writer = File.AppendText(temp);
                foreach (string s in result)
                {
                    if (!firstLine)
                    {
                        writer.WriteLine(s);
                    }
                    else
                        firstLine = false;
                }
                writer.Close();
                MessageBox.Show("Log saved");
            }
        }   
    }
}