using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PNTN_prov
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataText.button1ClickProv = true;

            DataText.text1 = textBox1.Text;
            DataText.text2 = textBox2.Text;

            textBox1.Text = "";
            textBox2.Text = "";

            Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DataText.button1ClickProv = false;
            Close();
        }


    }
}
