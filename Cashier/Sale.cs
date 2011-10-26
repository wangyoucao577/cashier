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
            //检查是否输入已符合规范

            if (dataGridView.Rows.Count <= 1)
            {
                MessageBox.Show("没有需要付款的项，请检查！");
                return;
            }
            foreach (DataGridViewRow item in dataGridView.Rows)
            {
                if (    null != item.Cells[1].Value
                    &&  !string.IsNullOrEmpty(item.Cells[1].Value.ToString()))
                {
                    //有商品标签代码

                    if (    null == item.Cells[2].Value
                        ||  string.IsNullOrEmpty(item.Cells[2].Value.ToString()))
                    {
                        //但是没有找到相应的商品名称

                        MessageBox.Show("商品填写不正确，请检查标签代码！");
                        return;
                    }

                    if (    null == item.Cells[3].Value
                        ||  string.IsNullOrEmpty(item.Cells[3].Value.ToString())
                        ||  !ValueMarked.CheckMoney(item.Cells[3].Value.ToString()))
                    {
                        //原价没有填

                        MessageBox.Show("原价填写不正确，请检查！");
                        return;
                    }

                    if (null == item.Cells[5].Value
                        || string.IsNullOrEmpty(item.Cells[5].Value.ToString())
                        || !ValueMarked.CheckMoney(item.Cells[5].Value.ToString()))
                    {
                        //结算价没有填

                        MessageBox.Show("结算价填写不正确，请检查！");
                        return;
                    }

                    if (null == item.Cells[7].Value
                        || string.IsNullOrEmpty(item.Cells[7].Value.ToString())
                        || !ValueMarked.CheckMoney(item.Cells[7].Value.ToString()))
                    {
                        //金额价没有填

                        MessageBox.Show("金额填写不正确，请检查！");
                        return;
                    }
                }
            }

            if (    string.IsNullOrEmpty(saleRecvTextBox.Text)
                ||  !ValueMarked.CheckMoney(saleRecvTextBox.Text))
            {
                MessageBox.Show("应付金额填写不正确，请检查！");
                return;
            }

            //检查是否有付款页面已经打开
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
                            &&  item.Cells[7].Value != null
                            &&  !string.IsNullOrEmpty(item.Cells[1].Value.ToString())
                            &&  !string.IsNullOrEmpty(item.Cells[2].Value.ToString())
                            &&  !string.IsNullOrEmpty(item.Cells[3].Value.ToString())
                            &&  !string.IsNullOrEmpty(item.Cells[4].Value.ToString())
                            &&  !string.IsNullOrEmpty(item.Cells[5].Value.ToString())
                            &&  !string.IsNullOrEmpty(item.Cells[6].Value.ToString())
                            &&  !string.IsNullOrEmpty(item.Cells[7].Value.ToString()))
                        {
                            SalesClothes sc = new SalesClothes();
                            sc.TagCode = item.Cells[1].Value.ToString();
                            sc.Name = item.Cells[2].Value.ToString();
                            sc.Price = item.Cells[3].Value.ToString();
                            sc.Count = item.Cells[6].Value.ToString();
                            sc.SalePrice = item.Cells[7].Value.ToString();

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
                            dataGridView.Rows.Clear();
                            upOffTextBox.Text = "";
                            saleRecvTextBox.Text = "";
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
            else if (4 == e.ColumnIndex || 5 == e.ColumnIndex || 6 == e.ColumnIndex)
            {
                if (4 == e.ColumnIndex)
                {
                    //折扣变化，则根据原价修改结算价

                    if (    null != dataGridView[3, e.RowIndex].Value
                        &&  null != dataGridView[4, e.RowIndex].Value
                        &&  !string.IsNullOrEmpty(dataGridView[3, e.RowIndex].Value.ToString())
                        &&  !string.IsNullOrEmpty(dataGridView[4, e.RowIndex].Value.ToString()))
                    {
                        int oldPrice;
                        int discount;
                        if (    int.TryParse(dataGridView[3, e.RowIndex].Value.ToString(), out oldPrice)
                            && int.TryParse(dataGridView[4, e.RowIndex].Value.ToString(), out discount)
                            &&  discount > 0
                            &&  discount <= 100
                            &&  oldPrice >= 0)
                        {
                            double new_price = (double)oldPrice * discount / 100;
                            if ((double)(int)new_price == new_price)
                            {
                                dataGridView[5, e.RowIndex].Value = (int)new_price;
                            }
                            else
                            {
                                dataGridView[5, e.RowIndex].Value = (int)new_price + 1;
                            }
                            
                        }
                    }
                }
                else if (5 == e.ColumnIndex)
                {
                    //结算价变化，则根据原价计算折扣
                    if (null != dataGridView[3, e.RowIndex].Value
                    && null != dataGridView[5, e.RowIndex].Value
                    && !string.IsNullOrEmpty(dataGridView[3, e.RowIndex].Value.ToString())
                    && !string.IsNullOrEmpty(dataGridView[5, e.RowIndex].Value.ToString()))
                    {
                        int oldPrice;
                        int newPrice;
                        if (int.TryParse(dataGridView[3, e.RowIndex].Value.ToString(), out oldPrice)
                            && int.TryParse(dataGridView[5, e.RowIndex].Value.ToString(), out newPrice)
                            && oldPrice > 0
                            && newPrice > 0
                            && newPrice <= oldPrice)
                        {
                            dataGridView[4, e.RowIndex].Value = newPrice * 100 / oldPrice;
                        }
                    }
                }


                //结算价或数量或折扣变化，则计算金额
                if (    null != dataGridView[5, e.RowIndex].Value
                    &&  null != dataGridView[6, e.RowIndex].Value
                    &&  !string.IsNullOrEmpty(dataGridView[5, e.RowIndex].Value.ToString())
                    &&  !string.IsNullOrEmpty(dataGridView[6, e.RowIndex].Value.ToString()))
                {
                    int factPrice;
                    int count;
                    if(     int.TryParse(dataGridView[5, e.RowIndex].Value.ToString(), out factPrice)
                        &&  int.TryParse(dataGridView[6, e.RowIndex].Value.ToString(), out count))
                    {
                        dataGridView[7, e.RowIndex].Value = factPrice * count;
                    }
                    
                }

                
            }
            else if (3 == e.ColumnIndex)
            {
                //原价变化，则根据结算价修改折扣

                if (    null != dataGridView[3, e.RowIndex].Value
                    &&  null != dataGridView[5, e.RowIndex].Value
                    &&  !string.IsNullOrEmpty(dataGridView[3, e.RowIndex].Value.ToString())
                    &&  !string.IsNullOrEmpty(dataGridView[5, e.RowIndex].Value.ToString()))
                {
                    int oldPrice;
                    int newPrice;
                    if (    int.TryParse(dataGridView[3, e.RowIndex].Value.ToString(), out oldPrice)
                        &&  int.TryParse(dataGridView[5, e.RowIndex].Value.ToString(), out newPrice)
                        &&  oldPrice > 0 
                        &&  newPrice > 0
                        &&  newPrice <= oldPrice)
                    {
                        dataGridView[4, e.RowIndex].Value = newPrice * 100 / oldPrice;
                    }
                }
                
            }
             

            
            //金额有改动，则修改应付款和优惠金额

            int prices = 0, salesPrices = 0, upOffPrices = 0;
            foreach (DataGridViewRow item in dataGridView.Rows)
            {
                int price; 
                int count;
                if (    null != item.Cells[3].Value && int.TryParse(item.Cells[3].Value.ToString(), out price)
                    && null != item.Cells[6].Value && int.TryParse(item.Cells[6].Value.ToString(), out count))
                {
                    prices = (price * count);
                }

                int salePrice;
                if (null != item.Cells[7].Value && int.TryParse(item.Cells[7].Value.ToString(), out salePrice))
                {
                    salesPrices += salePrice;

                    if (salePrice > 0)
                    {
                        upOffPrices += prices - salePrice;
                    }
                }

            }

            saleRecvTextBox.Text = salesPrices.ToString();
            upOffTextBox.Text = upOffPrices.ToString();
            
        }

        private void dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            
        }

        private void dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (3 == e.ColumnIndex || 5 == e.ColumnIndex || 7 == e.ColumnIndex)
            {
                int oldPrice;
                int newPrice;
                if (   !string.IsNullOrEmpty(e.FormattedValue.ToString()) 
                   &&  !ValueMarked.CheckMoney(e.FormattedValue.ToString()))
                {
                    //非货币
                    e.Cancel = true;
                    this.dataGridView.CancelEdit();
                }
                else if (e.ColumnIndex == 3)
                {
                    if (int.TryParse(e.FormattedValue.ToString(), out oldPrice))
                    {
                        if (oldPrice < 0)
                        {
                            //原价输入负值
                            e.Cancel = true;
                            this.dataGridView.CancelEdit();
                        }

                        if (dataGridView[5, e.RowIndex].Value != null
                           &&   int.TryParse(dataGridView[5, e.RowIndex].Value.ToString(), out newPrice)
                           &&   newPrice > oldPrice)
                        {
                                                        //结算价高于原价或原价输入负值
                            e.Cancel = true;
                            this.dataGridView.CancelEdit();
                        }
                    }

                }
                else if (e.ColumnIndex == 5)
                {

                    if (int.TryParse(e.FormattedValue.ToString(), out newPrice))
                    {
                        if (newPrice < 0)
                        {
                            //结算价输入负值
                            e.Cancel = true;
                            this.dataGridView.CancelEdit();
                        }

                        if (dataGridView[3, e.RowIndex].Value != null
                           && int.TryParse(dataGridView[3, e.RowIndex].Value.ToString(), out oldPrice)
                           && newPrice > oldPrice)
                        {
                            //结算价高于原价
                            e.Cancel = true;
                            this.dataGridView.CancelEdit();
                        }
                    }

                }
                
            }
            else if (4 == e.ColumnIndex)
            {
                int discount;
                if (    string.IsNullOrEmpty(e.FormattedValue.ToString()) 
                    ||  !int.TryParse(e.FormattedValue.ToString(), out discount)
                    ||  discount > 100
                    ||  discount <= 0)
                {
                    e.Cancel = true;
                    this.dataGridView.CancelEdit();
                }
            }
            else if (6 == e.ColumnIndex)
            {
                int count;
                if (string.IsNullOrEmpty(e.FormattedValue.ToString())
                    || !int.TryParse(e.FormattedValue.ToString(), out count)
                    || count > 999
                    || count < -999)
                {
                    e.Cancel = true;
                    this.dataGridView.CancelEdit();
                }
            }
        }

        

        private void dataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            
        }

        private void dataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            
            e.Row.Cells[0].Value = e.Row.Index + 1;
            e.Row.Cells[3].Value = null;
            e.Row.Cells[4].Value = 100;
            e.Row.Cells[5].Value = null;
            e.Row.Cells[6].Value = 1;
            e.Row.Cells[7].Value = null;


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

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (    null != dataGridView.CurrentRow 
                &&  dataGridView.CurrentRow.Index < dataGridView.Rows.Count - 1)
            {
                
                dataGridView.Rows.Remove(dataGridView.CurrentRow);

            }
            
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
            upOffTextBox.Text = "";
            saleRecvTextBox.Text = "";
        }

        private void dataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {

            foreach (DataGridViewRow item in dataGridView.Rows)
            {
                item.Cells[0].Value = item.Index + 1;
            }
        }

    }
}
