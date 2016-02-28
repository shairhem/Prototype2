using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Prototype2
{
    class detectInfo
    {
        public int bnn { get; set; }
        public int fnn { get; set; }
        public int dnn { get; set; }
        public int boxDim { get; set; }
        public int boobsModel { get; set; }
        public int pussyModel { get; set; }
        public int dickModel { get; set; }

        public detectInfo()
        {
            readConfig();
        }

        public void writeConfig(string container, string value)
        {
            List<string> list = new List<string>();
            StreamReader reader;
            string[] temp;
            string line = null;
            string tempLine = "";
            if (File.Exists("video.config"))
            {
                reader = new StreamReader("video.config");
                while ((line = reader.ReadLine()) != null)
                {
                    temp = line.Split(':');
                    if(temp[0] == container)
                    {
                        tempLine = temp[0] + ":" + value;
                        list.Add(tempLine);
                    }
                    else
                    {
                        list.Add(line);
                    }
                }
                reader.Close();
            }
            //StreamWriter writer = new StreamWriter("video.config");
            //foreach(string x in list)
            //{
            //    writer.WriteLine(x);
            //}
            File.WriteAllLines("video.config", list);
        }

        public void readConfig()
        {
            StreamReader reader;
            string[] temp;
            if (File.Exists("video.config"))
            {
                reader = new StreamReader("video.config");
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    temp = line.Split(':');
                    switch (temp[0])
                    {
                        case "bnn": bnn = int.Parse(temp[1]); break;
                        case "fnn": fnn = int.Parse(temp[1]); break;
                        case "dnn": dnn = int.Parse(temp[1]); break;
                        case "boxDim": boxDim = int.Parse(temp[1]); break;
                        case "boobModel": boobsModel = int.Parse(temp[1]); break;
                        case "pussyModel": pussyModel = int.Parse(temp[1]); break;
                        case "dickModel": dickModel = int.Parse(temp[1]); break;
                    }
                }
                reader.Close();
            }
        }
        public string readConfig(string container)
        {
            StreamReader reader;
            string[] temp;
            if (File.Exists("video.config"))
            {
                reader = new StreamReader("video.config");
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    temp = line.Split(':');
                    if (temp[0] == container)
                        return temp[1];
                }
                
            }
            return "";
        }
    }
}
