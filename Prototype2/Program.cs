using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Cuda;

namespace Prototype2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        //global variables
        
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ItemSelectForm());
            
            /*
            string file = " ";
            char[] trim = new char[1] { '"' };
            string temp = "";
            while (file != "0")
            {
                Console.WriteLine("input image path: ");
                file = Console.ReadLine();
                //file = "images/" + file + ".jpg";
                file = file.Trim(trim);
                if (file == "0" || file == "a")
                    return;
                else
                    Console.WriteLine("Enter mode: \n [d] - debug mode \n [*] - normal mode");
                if (Console.ReadLine() == "d")
                    Application.Run(new Form1(file));
                else
                    Application.Run(new Form2(file));
            }*/
            /*try
            {
                
            }
            catch (Exception e)
            {
                Console.WriteLine("invalid image");
                
            }*/
        }
        static void Run(string file)
        {
            //load image
            Mat image;// = new Mat(file,LoadImageType.Color);
            long detectionTime;
            
            //declare rectangles for detection
            List<Rectangle> breast = new List<Rectangle>();
            
            //disable cuda module
            bool tryUseCuda = false;
            bool tryUseOpenCL = true;
            int itr = 0;
            //capture video file
            Capture capture = new Capture(file);
            image = capture.QueryFrame();
            while (image != null)
            {
                if (itr == 161) break;
                Console.WriteLine(itr++);
                image = capture.QueryFrame();
                detect.detect1(image, "cascade.xml", breast, tryUseCuda, tryUseOpenCL,out detectionTime);
                //put rectangles
                foreach (Rectangle b in breast)
                    CvInvoke.Rectangle(image, b, new Bgr(Color.Red).MCvScalar, 2);
                
            }
            capture.Dispose();
            
            
            
            //show image
            /*ImageViewer.Show(image, String.Format(
            "Completed face and eye detection using {0} in {1} milliseconds",
            (tryUseCuda && CudaInvoke.HasCuda) ? "GPU"
            : (tryUseOpenCL && CvInvoke.HaveOpenCLCompatibleGpuDevice) ? "OpenCL"
            : "CPU",
            detectionTime));*/
        }
    }
}

