using System;
using System.Windows.Forms;

namespace PNTN_prov
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();

            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;

            comboBox1.Items.AddRange(new string[] { "Общая схема", "4.6.1", "4.6.2", "4.6.3", "4.6.4", "4.6.5", "4.6.6" });
            comboBox1.Text = "Общая схема";

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    pictureBox1.Image = PNTN_prov.Properties.Resources.общая_схема;
                    comboBox1.Text = "Общая схема"; break;
                case 1:
                    pictureBox1.Image = PNTN_prov.Properties.Resources._4_6_1;
                    comboBox1.Text = "4.6.1"; break;
                case 2:
                    pictureBox1.Image = PNTN_prov.Properties.Resources._4_6_2;
                    comboBox1.Text = "4.6.2"; break;
                case 3:
                    pictureBox1.Image = PNTN_prov.Properties.Resources._4_6_3;
                    comboBox1.Text = "4.6.3"; break;
                case 4:
                    pictureBox1.Image = PNTN_prov.Properties.Resources._4_6_4;
                    comboBox1.Text = "4.6.4"; break;
                case 5:
                    pictureBox1.Image = PNTN_prov.Properties.Resources._4_6_5;
                    comboBox1.Text = "4.6.5"; break;
                case 6:
                    pictureBox1.Image = PNTN_prov.Properties.Resources._4_6_6;
                    comboBox1.Text = "4.6.6"; break;
            }
        }//выбор схемы
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            ((Control)sender).Hide();
        }//окно не удаляется при зыкрытии, даёт возможность открыть повторно
    }
}
