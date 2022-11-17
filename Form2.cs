using System;
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
            DataExchange.button1ClickProv = true;

            DataExchange.text1 = textBox1.Text;
            DataExchange.text2 = textBox2.Text;

            textBox1.Text = "";
            textBox2.Text = "";

            Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DataExchange.button1ClickProv = false;
            Close();
        }
    }
}
