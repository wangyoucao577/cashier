using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using ZQPort;

namespace Cashier
{
    
    class SalesClothes
    {
        string m_tagCode = "";
        string m_name = "";
        string m_count = "";
        string m_price = "";
        string m_salePrice = "";

        public string TagCode
        {
            get { return m_tagCode; }
            set { m_tagCode = value; }
        }

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string Count
        {
            get { return m_count; }
            set { m_count = value; }
        }

        public string Price
        {
            get { return m_price; }
            set { m_price = value; }
        }
        
        public string SalePrice
        {
            get { return m_salePrice; }
            set { m_salePrice = value; }
        }
    }

    enum PrinterErr
    {
        Success,
        NoLPTPrinter,
        CanNotOpenPrinter,
    }

    class TicketPrinter
    {
        [DllImport("kernel32.dll")]//调用系统API打印函数
        public static extern IntPtr CreateFile
            (
            string FileName,          // file name
        uint DesiredAccess,       // access mode
        uint ShareMode,           // share mode
        uint SecurityAttributes, // Security Attributes
        uint CreationDisposition, // how to create
        uint FlagsAndAttributes, // file attributes
        int hTemplateFile         // handle to template file

            );

        private const string m_cutOffLine = "----------------------------------------";
        private const string m_doubleCutOffLine = "========================================";
        private string m_ticketName = "哇哈哈童装世界";
        private string m_ticketNum = "小票号：   VIP： ";
        private string m_cashierName = "";
        private string m_mobilePhone = "";
        private string m_telPhone = "0523-82082991";
        private string m_storeName = "专卖店：泰州店";
        private string m_address = "泰州市海陵北路112号";
        private string m_netAddress = "www.wahahatongzhuang.com";
        private string m_clause = "请妥善保管小票，七日内有问题凭小票调换。";
        private string m_welcome = "欢迎下次光临！";

        private string m_upoffPrice = "";
        private string m_factPrice = "";

        private List<SalesClothes> m_clothesList = new List<SalesClothes>();

        public void AddItem(SalesClothes sc)
        {
            m_clothesList.Add(sc);
            
        }

        public void SetPrices(string upoff, string fact)
        {
            m_upoffPrice = upoff;
            m_factPrice = fact;
        }

        public PrinterErr Print()
        {
            int nLptMax = CZQPort.GetLPTMax();
            if (nLptMax < 1)
            {
                return PrinterErr.NoLPTPrinter;
            }

            //打开打印机端口
            string printerName = "LPT1";
            const int OPEN_EXISTING = 3;
            IntPtr iHandle = CreateFile(printerName, 0x40000000, 0, 0, OPEN_EXISTING, 0, 0);
            if (iHandle.ToInt32() == -1)
            {
                return PrinterErr.CanNotOpenPrinter;
            }

            //获取日期时间
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;
            string dateTime = currentTime.ToString("yyyy-MM-dd HH:mm:ss");

            //打印数据
            FileStream fs = new FileStream(iHandle, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("GB18030"));   //写数据 
            sw.WriteLine(m_ticketName);
            sw.WriteLine(m_storeName);
            //sw.WriteLine(m_ticketNum);
            sw.WriteLine(dateTime);
            sw.WriteLine(m_doubleCutOffLine);
            sw.WriteLine("品名/条码号     数量     原价     结算价");
            sw.WriteLine(m_cutOffLine);
            foreach (SalesClothes item in m_clothesList)
            {
                sw.WriteLine(item.Name);

                string downLine = item.TagCode + "   " + item.Count + "     " + item.Price + "      " + item.SalePrice;
                sw.WriteLine(downLine);
                sw.WriteLine(m_cutOffLine);
            }
            sw.WriteLine(m_cutOffLine);

            string upOffStr = "优惠金额： " + m_upoffPrice;
            sw.WriteLine(upOffStr);

            string realStr = "实收金额： " + m_factPrice;
            sw.WriteLine(realStr);

            sw.WriteLine("");
            

            sw.WriteLine(m_clause);
            sw.WriteLine(m_netAddress);
            sw.WriteLine(m_address);
            sw.WriteLine(m_telPhone);
            sw.WriteLine(m_welcome);

            sw.WriteLine("\n\n\n\n\n\n");


            sw.Close();
            fs.Close();

            //切纸
            
            return PrinterErr.Success;
        }

       
    }
}
