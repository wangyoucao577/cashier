using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cashier
{
    public partial class Sale : Form
    {
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
    }
}
