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
        //models
        private CascadeClassifier cascadeBreastLeft = new CascadeClassifier("model/haarcascade_left_boob_2.xml");
        private CascadeClassifier cascadeBreastRight = new CascadeClassifier("model/haarcascade_right_boob_1.xml");
        private CascadeClassifier cascadePuss1 = new CascadeClassifier("model/haarcascade_vagina_front18.xml");
        private CascadeClassifier cascadePen1 = new CascadeClassifier("model/haarcascade-penis_h_20.xml");
        private CascadeClassifier cascadePuss2 = new CascadeClassifier("model/haarcascade_vagina_1.xml");
        private CascadeClassifier cascadeBreast = new CascadeClassifier("model/cascade_breast.xml");
        private CascadeClassifier cascadePuss = new CascadeClassifier("model/cascade_pussy.xml");
        private CascadeClassifier cascadePen = new CascadeClassifier("model/cascade_pen.xml");
        

        public int totalBreastCount = 0;
        public int totalPussyCount = 0;
        public int totalDickCount = 0;

        double totalFrameCount = 0;
        double frameNum = 0;
        int selectedMode = 0;
        int roiView = 0;
        frameInfo _frameInfo;
        List<frameInfo> frameList = new List<frameInfo>();
        //videoPlayer player;
        //VideoWriter writer;
        Stopwatch timer = new Stopwatch();
        recInfo recInfo;
        List<recInfo> recList = new List<recInfo>();

        detectInfo detect = new detectInfo();

        public Form2()
        {
            InitializeComponent();
        }

        public Form2(string file, int selectedMode, int roiView)
        {
            InitializeComponent();
            _file = file;
            this.selectedMode = selectedMode;
            this.roiView = roiView;
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
            //if (timer.ElapsedMilliseconds == 1000) _capture.Stop();
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            Mat frame = new Mat();
            Image<Bgr, Byte> frame1;
            frameNum = _capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames);
            _capture.Retrieve(frame, 0);
            frame1 = frame.ToImage<Bgr, Byte>();
            //frame1 = frame1.Resize(.5, Emgu.CV.CvEnum.Inter.Cubic);
            //frame = frame1.Mat;
            _frameInfo = new frameInfo();
            recInfo = new recInfo();
            _frameInfo.frameNum = (int)frameNum;
            //MessageBox.Show(_capture.Height + " " + _capture.Width + "\n" + frame1.Height + " " + frame1.Width);
            if (frame != null)
            {
                using (UMat ugray = new UMat())
                {
                    CvInvoke.CvtColor(frame, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
                    CvInvoke.EqualizeHist(ugray, ugray);
                    Rectangle[] breastDetected;
                    Rectangle[] pussyDetected;
                    Rectangle[] dickDetected;
                    if (selectedMode == 0 || selectedMode == 1)
                    {
                        if(detect.boobsModel == 2)
                        {
                            breastDetected = cascadeBreastLeft.DetectMultiScale(
                            ugray,
                            1.1,
                            detect.bnn,
                            new Size(detect.boxDim, detect.boxDim));
                            if (breastDetected.Count() > 0)
                            {
                                _frameInfo.boobDetected = true;
                                if (roiView > 0)
                                {
                                    recInfo.boobsLeft = breastDetected;
                                    recInfo.frameNum = (int)frameNum;
                                }
                            }
                            breastDetected = cascadeBreastRight.DetectMultiScale(
                            ugray,
                            1.1,
                            detect.bnn,
                            new Size(detect.boxDim, detect.boxDim));
                            if (breastDetected.Count() > 0)
                            {
                                _frameInfo.boobDetected = true;
                                if (roiView > 0)
                                {
                                    recInfo.boobsRight = breastDetected;
                                    recInfo.frameNum = (int)frameNum;
                                }
                            }
                        }
                        if (detect.boobsModel == 1)
                        {
                            breastDetected = cascadeBreastLeft.DetectMultiScale(
                            ugray,
                            1.1,
                            detect.bnn,
                            new Size(detect.boxDim, detect.boxDim));
                            if (breastDetected.Count() > 0)
                            {
                                _frameInfo.boobDetected = true;
                                if (roiView > 0)
                                {
                                    recInfo.boobs = breastDetected;
                                    recInfo.frameNum = (int)frameNum;
                                }
                            }
                        }

                    }
                    if (selectedMode == 0 || selectedMode == 2)
                    {
                        Console.WriteLine(detect.pussyModel);
                        if (detect.pussyModel == 1)
                        {
                            Console.WriteLine("model 1 pussy");
                            pussyDetected = cascadePuss.DetectMultiScale(
                            ugray,
                            1.1,
                            detect.fnn,
                            new Size(detect.boxDim, detect.boxDim));
                            if (pussyDetected.Count() > 0)
                            {
                                _frameInfo.pussDetected = true;
                                if (roiView > 0)
                                {
                                    recInfo.pussy = pussyDetected;
                                    recInfo.frameNum = (int)frameNum;
                                }
                            }
                        }
                        else if (detect.pussyModel == 2)
                        {
                            Console.WriteLine("model 2 pussy");
                            pussyDetected = cascadePuss1.DetectMultiScale(
                            ugray,
                            1.1,
                            detect.fnn,
                            new Size(detect.boxDim, detect.boxDim));
                            if (pussyDetected.Count() > 0)
                            {
                                _frameInfo.pussDetected = true;
                                if (roiView > 0)
                                {
                                    recInfo.pussy = pussyDetected;
                                    recInfo.frameNum = (int)frameNum;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("model 3 pussy");
                            pussyDetected = cascadePuss2.DetectMultiScale(
                            ugray,
                            1.1,
                            detect.fnn,
                            new Size(detect.boxDim, detect.boxDim));
                            if (pussyDetected.Count() > 0)
                            {
                                _frameInfo.pussDetected = true;
                                if (roiView > 0)
                                {
                                    recInfo.pussy = pussyDetected;
                                    recInfo.frameNum = (int)frameNum;
                                }
                            }
                        }                            
                    }
                    if (selectedMode == 0 || selectedMode == 3)
                    {
                        if(detect.dickModel == 1)
                        {
                            dickDetected = cascadePen.DetectMultiScale(
                            ugray,
                            1.1,
                            detect.dnn,
                            new Size(detect.boxDim, detect.boxDim));
                            if (dickDetected.Count() > 0)
                            {
                                _frameInfo.penDetected = true;
                                if (roiView > 0)
                                {
                                    recInfo.dick = dickDetected;
                                    recInfo.frameNum = (int)frameNum;
                                }
                            }
                        }
                        else
                        {
                            dickDetected = cascadePen1.DetectMultiScale(
                            ugray,
                            1.1,
                            detect.dnn,
                            new Size(detect.boxDim, detect.boxDim));
                            if (dickDetected.Count() > 0)
                            {
                                _frameInfo.penDetected = true;
                                if (roiView > 0)
                                {
                                    recInfo.dick = dickDetected;
                                    recInfo.frameNum = (int)frameNum;
                                }
                            }
                        }
                            
                    }
                    if (_frameInfo.boobDetected || _frameInfo.pussDetected || _frameInfo.penDetected)
                    {
                        frameList.Add(_frameInfo);
                        recList.Add(recInfo);
                    }
                    progressBar1.Invoke(new MethodInvoker(delegate { progressBar1.Increment(1); label2.Text = frameNum.ToString();}));   
                }
            }
            
            if (frameNum == totalFrameCount)
            {
                progressBar1.Invoke(new MethodInvoker(delegate { button2.Enabled = true; }));
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

        private void button2_Click(object sender, EventArgs e)
        {
            //player = new videoPlayer(frameList, (int)_capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps), _file,(int)totalFrameCount);
            videoPlayerv2 player;
            //MessageBox.Show(roiView.ToString());
            if (roiView == 0)
            {
                player = new videoPlayerv2(_file, frameList);
                player.Show();
                this.Close();
            }
            else
            {
                //MessageBox.Show(recList.Count.ToString());
                //foreach(recInfo d in recList)
                //{
                //    foreach(Rectangle x in d.boobs)
                //    {
                //        Console.WriteLine(d.frameNum + " " + x.Location + " " + x.Size);

                //    }
                //}
                player = new videoPlayerv2(_file, frameList, roiView,recList);
                player.Show();
                this.Close();
            }
        }
    }
}
