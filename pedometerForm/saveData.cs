using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace pedometerForm
{
    class saveData
    {


        public saveData()
        {
            FileInfo TheFile = new FileInfo("setting.txt");
            if (!TheFile.Exists)
            {
                File.Create("setting.txt");
            }
        }


        public string readGeData()
        {
            StreamReader objReader = new StreamReader("setting.txt");
            string sLine = "";
          
            if (sLine != null)
            {
                sLine = objReader.ReadLine();
                
            }
            objReader.Close();
            return sLine;
        }
        public void saveGEData(string data)
        {
            FileStream fs = new FileStream("setting.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(data);
            sw.Flush();
            sw.Close();
            fs.Close();

        }
    }
}
