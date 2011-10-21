using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Cashier
{
    enum DataSourceErr
    {
        Success,
        FileNotExist,
    }

    class Clothing
    {
        private string tagCode;
        private string name;
        private string unit;

        public string TagCode
        {
            get { return this.tagCode; }
            set { this.tagCode = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Unit
        {
            get { return this.unit; }
            set { this.unit = value; }
        }
    }

    class DataSource
    {
        private Hashtable m_hashData = new Hashtable();

        public DataSourceErr Init(string dataFile)
        {
            //查询文件是否在
            if (!File.Exists(dataFile))
            {
                return DataSourceErr.FileNotExist;
            }

            //从文件读取数据，分析，并加入到Hashtable中去
            uint linesCount = 0;
            StreamReader sr = new StreamReader(dataFile, Encoding.GetEncoding("GB18030"));
            string fileData = sr.ReadToEnd();
            string[] dataArray = fileData.Split('\n');

            string line;
            //while ((line = sr.ReadLine()) != null) 
            while (linesCount < dataArray.Length)
            {
                line = dataArray[linesCount];
                ++linesCount;
                //Trace.WriteLine(line);

                string[] sArray = line.Split('\t');
                if (sArray.Length <= 4)
                {
                    Console.WriteLine("Not enough string, just {0} len, data is {1}.\n", 
                        sArray.Length, line);
                    //char[] bytes = line.ToCharArray();
                    continue;
                }

                Clothing clo = new Clothing();
                clo.TagCode = sArray[1];
                clo.Name = sArray[2];
                clo.Unit = sArray[3];

                m_hashData.Add(clo.TagCode, clo);
            }

            string str = linesCount.ToString() + " lines has been added.";
            Trace.WriteLine(str);
            return DataSourceErr.Success;
        }

        public Clothing GetClothing(string key)
        {
            return (Clothing)m_hashData[key]; ;
        }
    }
}
