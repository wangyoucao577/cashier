﻿using System;
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
    }
}
