using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cashier
{
    public partial class SaleConfirm : Form
    {
        private string faceRecvPrices = "";

        public SaleConfirm()
        {
            InitializeComponent();
        }

        public void SetSalesRecv(string price)
        {
            salesRecvTextBox.Text = price;
        }

        private void FactRecvTextBox_TextChanged(object sender, EventArgs e)
        {
            //先验证输入是否正确
            string word = ((TextBox)sender).Text.Trim();

            if (!string.IsNullOrEmpty(word) && !ValueMarked.CheckMoney(word))
            {
                ((TextBox)sender).Text = faceRecvPrices;//不成功就等于原来的数
                ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;//把鼠标移到最后面

            }
            else
            {
                faceRecvPrices = word;
            }

            //计算应找零的值
            int wantPrice, faceRecv;
            if (    !string.IsNullOrEmpty(salesRecvTextBox.Text)
                &&  int.TryParse(salesRecvTextBox.Text, out wantPrice)
                &&  !string.IsNullOrEmpty(FactRecvTextBox.Text)
                &&  int.TryParse(FactRecvTextBox.Text, out faceRecv))
            {
                returnBackTextBox.Text = (faceRecv - wantPrice).ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
