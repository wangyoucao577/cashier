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
        private ManualResetEvent m_event = new ManualResetEvent(false);

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
            SaleConfirm sc = new SaleConfirm();
            sc.Show();
        }

        private void Sale_Load(object sender, EventArgs e)
        {
            m_dataSource.Init("SaleProducts.dat");
            m_event.Set();
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (1 == e.ColumnIndex)
            {
                //m_event.WaitOne();
                //Clothing clo = m_dataSource.GetClothing((string)dataGridView[1, e.RowIndex].Value);
                //if (null != clo)
                //{
                //    dataGridView[2, e.RowIndex].Value = clo.Name;
                //    dataGridView[3, e.RowIndex].Value = clo.Name;
                //}
                
            }
        }
    }
}
