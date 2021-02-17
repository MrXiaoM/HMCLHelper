using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormDemo
{
    public partial class DialogTextBox : Form
    {
        public DialogTextBox(String code)
        {
            InitializeComponent();
            textBox1.Text = code;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox1.Text);
            MessageBox.Show("Code 已复制到你的剪贴板", "HMCLHelper");
        }
    }
}
