using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Cashier
{
    public partial class Sale : Form
    {
        private DataSource m_dataSource = new DataSource();
        private string faceSalesPrice = "";
        SaleConfirm m_sc = null;

        public Sale()
        {
            InitializeComponent();
        }

        private void 关于ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (null == m_sc)
            {
                m_sc = new SaleConfirm();
                m_sc.SetSalesRecv(saleRecvTextBox.Text);
                DialogResult dr = m_sc.ShowDialog();
                
                if (dr == DialogResult.OK)
                {
                    //打印发票

                    TicketPrinter tp = new TicketPrinter();
                    

                    foreach (DataGridViewRow item in dataGridView.Rows)
                    {
                        if (    item.Cells[1].Value != null
                            &&  item.Cells[2].Value != null
                            &&  item.Cells[3].Value != null
                            &&  item.Cells[4].Value != null
                            &&  item.Cells[5].Value != null
                            &&  item.Cells[6].Value != null
                            &&  !string.IsNullOrEmpty(item.Cells[1].Value.ToString())
                            &&  !string.IsNullOrEmpty(item.Cells[2].Value.ToString())
                            &&  !string.IsNullOrEmpty(item.Cells[3].Value.ToString())
                            &&  !string.IsNullOrEmpty(item.Cells[4].Value.ToString())
                            &&  !string.IsNullOrEmpty(item.Cells[5].Value.ToString())
                            &&  !string.IsNullOrEmpty(item.Cells[6].Value.ToString()))
                        {
                            SalesClothes sc = new SalesClothes();
                            sc.Name = item.Cells[2].Value.ToString();
                            sc.TagCode = item.Cells[1].Value.ToString();
                            sc.Count = item.Cells[3].Value.ToString();
                            sc.Price = item.Cells[4].Value.ToString();
                            sc.SalePrice = item.Cells[5].Value.ToString();

                            tp.AddItem(sc);
                        }
                    }

                    if (    !string.IsNullOrEmpty(upOffTextBox.Text)
                        &&  !string.IsNullOrEmpty(saleRecvTextBox.Text))
                    {
                        tp.SetPrices(upOffTextBox.Text, saleRecvTextBox.Text);
                    }

                    PrinterErr perr = tp.Print();
                    switch (perr)
                    {

                        case PrinterErr.NoLPTPrinter:
                            MessageBox.Show("找不到LPT打印机，请检查打印机的连接！");
                            break;
                        case PrinterErr.CanNotOpenPrinter:
                            MessageBox.Show("无法打开LPT1打印机，请检查打印机的端口是否正确！");
                            break;
                        case PrinterErr.Success:
                        default:
                            MessageBox.Show("发票打印成功！");
                            break;
                    }
                }

                m_sc = null;
            }
            
        }

        private void Sale_Load(object sender, EventArgs e)
        {
            m_dataSource.Init("SaleProducts.dat");
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView_CurrentCellChanged(object sender, EventArgs e)
        {
            
        }

        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (1 == e.ColumnIndex && m_dataSource.IsInited())
            {
                Clothing clo = m_dataSource.GetClothing((string)dataGridView[1, e.RowIndex].Value);
                if (null != clo)
                {
                    dataGridView[2, e.RowIndex].Value = clo.Name;
                }
            }
            else if (4 == e.ColumnIndex || 5 == e.ColumnIndex)
            {
                if (    null != dataGridView[4, e.RowIndex].Value
                    &&  null != dataGridView[5, e.RowIndex].Value
                    &&  !string.IsNullOrEmpty(dataGridView[4, e.RowIndex].Value.ToString())
                    &&  !string.IsNullOrEmpty(dataGridView[5, e.RowIndex].Value.ToString()))
                {
                    double factPrice;
                    int count;
                    if(     double.TryParse(dataGridView[4, e.RowIndex].Value.ToString(), out factPrice)
                        &&  int.TryParse(dataGridView[5, e.RowIndex].Value.ToString(), out count))
                    {
                        dataGridView[6, e.RowIndex].Value = factPrice * count;
                    }
                    
                }
            }
            
            //金额有改动，则修改应付款和优惠金额

            double prices = 0.0, salesPrices = 0.0;
            foreach (DataGridViewRow item in dataGridView.Rows)
            {
                double price;
                if (null != item.Cells[3].Value && double.TryParse(item.Cells[3].Value.ToString(), out price))
                {
                    prices += price;
                }

                if (null != item.Cells[6].Value && double.TryParse(item.Cells[6].Value.ToString(), out price))
                {
                    salesPrices += price;
                }
            }

            saleRecvTextBox.Text = salesPrices.ToString();
            upOffTextBox.Text = (prices - salesPrices).ToString();
            
        }

        private void dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            
        }

        private void dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (3 == e.ColumnIndex || 4 == e.ColumnIndex || 6 == e.ColumnIndex)
            {
                if (!string.IsNullOrEmpty(e.FormattedValue.ToString()) && !ValueMarked.CheckMoney(e.FormattedValue.ToString()))
                {
                    e.Cancel = true;
                    this.dataGridView.CancelEdit();
                }
            }
        }

        

        private void dataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //dataGridView[5, e.RowIndex].Value = 1;

            //foreach (DataGridViewRow item in dataGridView.Rows)
            //{
            //    item.Cells[0].Value = item.Index + 1;
            //}
        }

        private void dataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            
            e.Row.Cells[0].Value = e.Row.Index + 1;
            e.Row.Cells[5].Value = 1;

        }

        private void 删除行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void dataContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            
        }

        private void dataGridView_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void upoffTextBox_Validating(object sender, CancelEventArgs e)
        {
            
        }

        private void saleRecvTextBox_Validating(object sender, CancelEventArgs e)
        {

        }

        private void upOffTextBox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void saleRecvTextBox_TextChanged(object sender, EventArgs e)
        {
            string word = ((TextBox)sender).Text.Trim();

            if (!string.IsNullOrEmpty(word) && !ValueMarked.CheckMoney(word))
            {
                ((TextBox)sender).Text = faceSalesPrice;//不成功就等于原来的数
                ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;//把鼠标移到最后面

            }
            else
            {
                faceSalesPrice = word;
            }
        }

    }
}
