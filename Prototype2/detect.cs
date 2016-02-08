using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
namespace Prototype2
{
    public static class detect
    {
        public static void detect1(Mat image, String fileName, List<Rectangle> breast, bool tryUseCuda, bool tryUseOpenCL,out long detectionTime)
        {

            //Many opencl functions require opencl compatible gpu devices. 
            //As of opencv 3.0-alpha, opencv will crash if opencl is enable and only opencv compatible cpu device is presented
            //So we need to call CvInvoke.HaveOpenCLCompatibleGpuDevice instead of CvInvoke.HaveOpenCL (which also returns true on a system that only have cpu opencl devices).
            CvInvoke.UseOpenCL = tryUseOpenCL && CvInvoke.HaveOpenCLCompatibleGpuDevice;

            Stopwatch watch;
            //Read the HaarCascade objects
            using (CascadeClassifier face = new CascadeClassifier(fileName))
            {
                watch = Stopwatch.StartNew();
                using (UMat ugray = new UMat())
                {
                    CvInvoke.CvtColor(image, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

                    //normalizes brightness and increases contrast of the image
                    CvInvoke.EqualizeHist(ugray, ugray);

                    //Detect the faces  from the gray scale image and store the locations as rectangle
                    //The first dimensional is the channel
                    //The second dimension is the index of the rectangle in the specific channel
                    Rectangle[] facesDetected = face.DetectMultiScale(
                       ugray,
                       1.1,
                       10,
                       new Size(20, 20));

                    breast.AddRange(facesDetected);


                }
                watch.Stop();
            }

            detectionTime = watch.ElapsedMilliseconds;
            
        }
    }
}

