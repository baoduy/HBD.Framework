using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HBD.WinForms.Controls;

namespace TestWindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.listControlCollection1.ChildrenControlType = typeof(TextBox);
            this.Controls.Add(new DataGridViewSearchControl());
        }
    }
}
