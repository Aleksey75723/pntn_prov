using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace PNTN_prov
{
    public partial class Form1 : Form
    {
        decimal U;
        decimal I;
        decimal const_;
        public decimal const1;
        public decimal const2;

        public double accuracy = 0.1; //погрешность

        Form2 frm2 = new Form2();//форма ввода данных блока для сохранения в протокол
        Form3 frm3 = new Form3();//форма схем проверок

        //для создания файла протокола
        private string path = Environment.CurrentDirectory + "/" + "pntn_protokol.txt";
        //для создания файла констант
        private string path_const = Environment.CurrentDirectory + "/" + "pntn_const.txt";

        //вводимые данные
        decimal δ_X23_X26_62;
        decimal I_X24_X25_63; decimal δ_X24_X25_63;
        decimal δ_X15_X16_64;
        decimal I_X27_X28_65; decimal δ_X27_X28_65;
        decimal δ_X11_X12_66;
        decimal δ_X21_X22_67;


        //-------------------------------------------------------------
        private void Form1_Load(object sender, EventArgs e)
        {
            //Создаёт файл pntn_const при запуске если не существует
            if (!File.Exists(path_const))
            {
                using (FileStream fstream = new FileStream("pntn_const.txt", FileMode.OpenOrCreate))
                {
                }

                using (StreamWriter sw = new StreamWriter(path_const))
                {
                    sw.WriteLine("δ X2:3-X2:6 = "); sw.WriteLine("1,21224002");
                    sw.WriteLine("");
                    sw.WriteLine("I X2:4-X2:5 = "); sw.WriteLine("2,498096032");
                    sw.WriteLine("");
                    sw.WriteLine("δ X2:4-X2:5 = "); sw.WriteLine("1,248513674");
                    sw.WriteLine("");
                    sw.WriteLine("δ X1:5-X1:6 = "); sw.WriteLine("1,2120382");
                    sw.WriteLine("");
                    sw.WriteLine("I X2:7-X2:8 = "); sw.WriteLine("2,498096032");
                    sw.WriteLine("");
                    sw.WriteLine("δ X2:7-X2:8 = "); sw.WriteLine("1,248513674");
                    sw.WriteLine("");
                    sw.WriteLine("δ X1:1-X1:2 = "); sw.WriteLine("9090,04933015");
                    sw.WriteLine("");
                    sw.WriteLine("δ X2:1-X2:2 = "); sw.WriteLine("30,04933015");
                    sw.WriteLine(""); sw.WriteLine(""); sw.WriteLine("");
                    sw.WriteLine("------------------------------");
                    sw.WriteLine("Важно! Значения констант должны стоять на определенных строках. " +
                        "Для разделения целой и дробной части использовать запятую (,).");
                }
            }

            //загружает из файла константы в программу
            if (File.Exists(path_const))
            {
                using (StreamReader sr = new StreamReader(path_const))
                {
                    //в случае некорректной запси в файле выдаст предупреждение
                    try { δ_X23_X26_62 = Convert.ToDecimal(GetString(2, "pntn_const.txt")); }//считывает только определенную строку 
                    catch { MessageBox.Show("Ошибка чтения файла pntn_const\nзначение константы δ X2:3-X2:6 некорректное", "Ошибка"); }

                    try { I_X24_X25_63 = Convert.ToDecimal(GetString(5, "pntn_const.txt")); }
                    catch { MessageBox.Show("Ошибка чтения файла pntn_const\nзначение константы I X2:4-X2:5 некорректное", "Ошибка"); }

                    try { δ_X24_X25_63 = Convert.ToDecimal(GetString(8, "pntn_const.txt")); }
                    catch { MessageBox.Show("Ошибка чтения файла pntn_const\nзначение константы δ X2:4-X2:5 некорректное", "Ошибка"); }

                    try { δ_X15_X16_64 = Convert.ToDecimal(GetString(11, "pntn_const.txt")); }
                    catch { MessageBox.Show("Ошибка чтения файла pntn_const\nзначение константы δ X1:5-X1:6 некорректное", "Ошибка"); }

                    try { I_X27_X28_65 = Convert.ToDecimal(GetString(14, "pntn_const.txt")); }
                    catch { MessageBox.Show("Ошибка чтения файла pntn_const\nзначение константы I X2:7-X2:8 некорректное", "Ошибка"); }

                    try { δ_X27_X28_65 = Convert.ToDecimal(GetString(17, "pntn_const.txt")); }
                    catch { MessageBox.Show("Ошибка чтения файла pntn_const\nзначение константы δ X2:7-X2:8 некорректное", "Ошибка"); }

                    try { δ_X11_X12_66 = Convert.ToDecimal(GetString(20, "pntn_const.txt")); }
                    catch { MessageBox.Show("Ошибка чтения файла pntn_const\nзначение константы δ X1:1-X1:2 некорректное", "Ошибка"); }

                    try { δ_X21_X22_67 = Convert.ToDecimal(GetString(23, "pntn_const.txt")); }
                    catch { MessageBox.Show("Ошибка чтения файла pntn_const\nзначение константы δ X2:1-X2:2 некорректное", "Ошибка"); }
                }
            }

            //Создаёт файл pntn_protokol при запуске если не существует
            if (!File.Exists(path))
            {
                using (FileStream fstream = new FileStream("pntn_protokol.txt", FileMode.OpenOrCreate))
                {
                }
            }
        }
        //-------------------------------------------------------------
        private void Сalc_method1(decimal U_, decimal I_, TextBox R_calc, TextBox measurement_calc, Label label_, decimal const__)
        {

            if (I_ != 0)
            {

                R_calc.Text = Convert.ToString(U_ / I_);

                measurement_calc.Text = Convert.ToString(100 * (((U_ / I_) - const__) / const__));

                if ((Convert.ToDouble(measurement_calc.Text)) < accuracy &
                    (Convert.ToDouble(measurement_calc.Text)) > -(accuracy) &
                    (Convert.ToDouble(measurement_calc.Text)) != 0)
                {
                    measurement_calc.BackColor = Color.SpringGreen;
                    label_.Text = "Исправен";
                }
                else
                {
                    measurement_calc.BackColor = Color.White;
                    label_.Text = "Не исправен";
                }
            }

        }//метод расчёта 1
        //-------------------------------------------------------------
        private void Сalc_method2(decimal U1, decimal U2, TextBox I_calc, TextBox R_calc, TextBox measurement_calc, Label label_, decimal const_1_, decimal const_2_)
        {
            if (U1 != 0)
            {

                I_calc.Text = Convert.ToString(U1 / const_1_);

                R_calc.Text = Convert.ToString(U2 / (U1 / const_1_));

                measurement_calc.Text = Convert.ToString(100 * (((U2 / (U1 / const_1_)) - const_2_) / const_2_));

                if ((Convert.ToDouble(measurement_calc.Text)) < accuracy &
                    (Convert.ToDouble(measurement_calc.Text)) > -(accuracy) &
                    (Convert.ToDouble(measurement_calc.Text)) != 0)
                {
                    measurement_calc.BackColor = Color.SpringGreen;
                    label_.Text = "Исправен";
                }
                else
                {
                    measurement_calc.BackColor = Color.White;
                    label_.Text = "Не исправен";
                }

            }
        }//метод расчёта 2
        //-------------------------------------------------------------------------
        private static void CharMethod(KeyPressEventArgs e)//не вписывает буквы
        {
            if (Char.IsNumber(e.KeyChar) | (e.KeyChar == Convert.ToChar(",")) | e.KeyChar == '\b') return;
            else
                e.Handled = true;
        }
        //-------------------------------------------------------------------------
        private void TextMethod(TextBox text)//метод вписывания "0" и ","
        {
            if (text.Text == ",")
            {
                text.Text = "0,";
                text.SelectionStart = text.Text.Length;
            }
        }
        //-------------------------------------------------------------------------                                                               
        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)//Спрашивает перед закрытием
        {
            var result = MessageBox.Show("Выйти из программы?", "Выход",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);
            // If the no button was pressed ...
            if (result == DialogResult.No)
            {
                // cancel the closure of the form.
                e.Cancel = true;
            }
        }
        //-------------------------------------------------------------------------  
        private void LightCorrect(TextBox δ_, Label correct)
        {

            if ((Convert.ToDouble(δ_.Text)) < accuracy &
                (Convert.ToDouble(δ_.Text)) > -(accuracy) &
                (Convert.ToDouble(δ_.Text)) != 0)
                {
                    δ_.BackColor = Color.SpringGreen;
                    correct.Text = "Исправен";
                }
            else
                {
                    δ_.BackColor = Color.White;
                    correct.Text = "Не исправен";
                }
        }//подстветка исправности/неисправности
        //-------------------------------------------------------------------------  
        private void TrackBar1_Scroll(object sender, EventArgs e)//Бегунок расчётной погрешности, изменение подсветки исправности
        {
            label20.Text = String.Format("Допустимая расчётная погрешность: {0}%", accuracy);
            accuracy = Convert.ToDouble(trackBar1.Value) / 10d;

            LightCorrect(textBox3, label1);
            LightCorrect(textBox6, label27);
            LightCorrect(textBox11, label38);
            LightCorrect(textBox16, label49);
            LightCorrect(textBox23, label60);
            LightCorrect(textBox28, label71);
        }
        //-------------------------------------------------------------------------  


        public Form1()
        {
            InitializeComponent();

            trackBar1.Scroll += TrackBar1_Scroll;
            label20.Text = String.Format("Допустимая расчётная погрешность: {0}%", accuracy);
        }
        private void VersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Версия 1.0.4", "Версия");
        }
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ClearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "0";
            textBox2.Text = "0";
            textBox3.Text = "0";//
            textBox4.Text = "0";
            textBox5.Text = "0";
            textBox6.Text = "0";//
            textBox7.Text = "0";
            textBox8.Text = "0";
            textBox9.Text = "0";
            textBox11.Text = "0";//
            textBox12.Text = "0";
            textBox13.Text = "0";
            textBox14.Text = "0";
            textBox15.Text = "0";
            textBox16.Text = "0";//
            textBox17.Text = "0";
            textBox18.Text = "0";
            textBox20.Text = "0";
            textBox21.Text = "0";
            textBox23.Text = "0";//
            textBox24.Text = "0";
            textBox25.Text = "0";
            textBox26.Text = "0";
            textBox28.Text = "0";//
            textBox29.Text = "0";
            textBox30.Text = "0";

            textBox3.BackColor = Color.White;
            textBox6.BackColor = Color.White;
            textBox11.BackColor = Color.White;
            textBox16.BackColor = Color.White;
            textBox23.BackColor = Color.White;
            textBox28.BackColor = Color.White;

            label1.Text = "Не исправен";
            label27.Text = "Не исправен";
            label38.Text = "Не исправен";
            label49.Text = "Не исправен";
            label60.Text = "Не исправен";
            label71.Text = "Не исправен";

        }


        #region Пункты ТУ
        //-------6.2---------------------------------------------------
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            CharMethod(e);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            TextMethod(textBox1);

            if (textBox1.Text != "")
                I = decimal.Parse(textBox1.Text);

            if (I != 0)
            {
                textBox20.Text = Convert.ToString(U / I);
                textBox3.Text = Convert.ToString(100 * (((U / I) - δ_X23_X26_62) / δ_X23_X26_62));


                if ((Convert.ToDouble(textBox3.Text)) < accuracy &
                       (Convert.ToDouble(textBox3.Text)) > -(accuracy) &
                       (Convert.ToDouble(textBox3.Text)) != 0)
                {

                    textBox3.BackColor = Color.SpringGreen;
                    label1.Text = "Исправен";
                                    }
                else
                {
                    textBox3.BackColor = Color.White;
                    label1.Text = "Не исправен";
                }
            }
        }


        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            CharMethod(e);
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

            TextMethod(textBox2);




            if (textBox2.Text != "")
                U = decimal.Parse(textBox2.Text);


            if (I != 0)
            {
                textBox20.Text = Convert.ToString(U / I);
                textBox3.Text = Convert.ToString(100 * (((U / I) - δ_X23_X26_62) / δ_X23_X26_62));



                if ((Convert.ToDouble(textBox3.Text)) < accuracy &
                        (Convert.ToDouble(textBox3.Text)) > -(accuracy) &
                        (Convert.ToDouble(textBox3.Text)) != 0)
                {

                    textBox3.BackColor = Color.SpringGreen;
                    label1.Text = "Исправен";

                }
                else
                {
                    textBox3.BackColor = Color.White;
                    label1.Text = "Не исправен";
                }


            }
        }





        //-------6.3----------------------------------------------------
        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            CharMethod(e);
        }
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            TextMethod(textBox8);

            if (textBox8.Text != "")
                I = decimal.Parse(textBox8.Text);

            Сalc_method2(I, U, textBox5, textBox4, textBox6, label27, I_X24_X25_63, δ_X24_X25_63);


        }


        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            CharMethod(e);
        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            TextMethod(textBox7);

            if (textBox7.Text != "")
                U = decimal.Parse(textBox7.Text);

            Сalc_method2(I, U, textBox5, textBox4, textBox6, label27, I_X24_X25_63, δ_X24_X25_63);
        }





        //-------6.4--------------------------------------------------
        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
            CharMethod(e);
        }
        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            TextMethod(textBox13);

            if (textBox13.Text != "")
                I = decimal.Parse(textBox13.Text);

            const_ = δ_X15_X16_64;
            Сalc_method1(U, I, textBox9, textBox11, label38, const_);
        }


        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            CharMethod(e);
        }
        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            TextMethod(textBox12);

            if (textBox12.Text != "")
                U = decimal.Parse(textBox12.Text);

            const_ = δ_X15_X16_64;
            Сalc_method1(U, I, textBox9, textBox11, label38, const_);
        }





        //-------6.5----------------------------------------------------
        private void textBox18_KeyPress(object sender, KeyPressEventArgs e)
        {
            CharMethod(e);
        }
        private void textBox18_TextChanged(object sender, EventArgs e)
        {
            TextMethod(textBox18);

            if (textBox18.Text != "")
                I = decimal.Parse(textBox18.Text);

            Сalc_method2(I, U, textBox15, textBox14, textBox16, label49, I_X27_X28_65, δ_X27_X28_65);
        }


        private void textBox17_KeyPress(object sender, KeyPressEventArgs e)
        {
            CharMethod(e);
        }
        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            TextMethod(textBox17);

            if (textBox17.Text != "")
                U = decimal.Parse(textBox17.Text);

            Сalc_method2(I, U, textBox15, textBox14, textBox16, label49, I_X27_X28_65, δ_X27_X28_65);
        }





        //-------6.6-------------------------------------------------
        private void textBox25_KeyPress(object sender, KeyPressEventArgs e)
        {
            CharMethod(e);
        }
        private void textBox25_TextChanged(object sender, EventArgs e)
        {
            TextMethod(textBox25);


            if (textBox25.Text != "")
                I = decimal.Parse(textBox25.Text);
            const_ = δ_X11_X12_66;
            Сalc_method1(U, I, textBox21, textBox23, label60, const_);
        }


        private void textBox24_KeyPress(object sender, KeyPressEventArgs e)
        {
            CharMethod(e);
        }
        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            TextMethod(textBox24);

            if (textBox24.Text != "")
                U = decimal.Parse(textBox24.Text);

            const_ = δ_X11_X12_66;
            Сalc_method1(U, I, textBox21, textBox23, label60, const_);
        }




        //-------6.7-----------------------------------------------
        private void textBox30_KeyPress(object sender, KeyPressEventArgs e)
        {
            CharMethod(e);
        }
        private void textBox30_TextChanged(object sender, EventArgs e)
        {
            TextMethod(textBox30);

            if (textBox30.Text != "")
                I = decimal.Parse(textBox30.Text);

            const_ = δ_X21_X22_67;
            Сalc_method1(U, I, textBox26, textBox28, label71, const_);
        }


        private void textBox29_KeyPress(object sender, KeyPressEventArgs e)
        {
            CharMethod(e);
        }
        private void textBox29_TextChanged(object sender, EventArgs e)
        {
            TextMethod(textBox29);

            if (textBox29.Text != "")
                U = decimal.Parse(textBox29.Text);

            const_ = δ_X21_X22_67;
            Сalc_method1(U, I, textBox26, textBox28, label71, const_);
        }

        #endregion



        //-------схемыПроверок----------------------------------------------------------------------
        private void InspectionSchemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm3.Show();
        }


        //---------Запись в протокол-------------------------------------------------------------
        private void RecordProtocolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm2.ShowDialog();

            if (DataText.button1ClickProv==true)
            {
                using (StreamWriter sw = new StreamWriter(path, true))
                {

                    sw.WriteLine("------------------------------------------------------------------------------");
                    sw.Write("Номер блока: {0}", DataText.text1); sw.WriteLine("({0})", DataText.text2);
                    //sw.WriteLine("");
                    sw.WriteLine("Дата проверки: " + DateTime.Now);
                    sw.WriteLine("Текущая допустимая погрешность: " + accuracy + "%");

                    sw.WriteLine("4.6.1");
                    sw.WriteLine("Измерение I X1:3-X1:4 (PA1): " + textBox1.Text);
                    sw.WriteLine("Измерение U X2:3-X2:6 (PV3): " + textBox2.Text);
                    sw.WriteLine("Расчёт R X2:3-X2:6: " + textBox20.Text);
                    sw.WriteLine("Расчёт δ X2:3-X2:6: " + textBox3.Text);
                    sw.WriteLine("Результат проверки блока: " + label1.Text);

                    sw.WriteLine("4.6.2");
                    sw.WriteLine("Измерение U X2:3-X2:6 (PV3): " + textBox8.Text);
                    sw.WriteLine("Измерение U X2:4-X2:5 (PV2): " + textBox7.Text);
                    sw.WriteLine("Расчёт I X2:4-X2:5: " + textBox5.Text);
                    sw.WriteLine("Расчёт R X2:4-X2:5: " + textBox4.Text);
                    sw.WriteLine("Расчёт δ X2:4-X2:5: " + textBox6.Text);
                    sw.WriteLine("Результат проверки блока: " + label27.Text);

                    sw.WriteLine("4.6.3");
                    sw.WriteLine("Измерение I X1:5-X1:6 (PA1): " + textBox13.Text);
                    sw.WriteLine("Измерение U E4-E5 (PV3): " + textBox12.Text);
                    sw.WriteLine("Расчёт R X1:5-X1:6: " + textBox9.Text);
                    sw.WriteLine("Расчёт δ X1:5-X1:6: " + textBox11.Text);
                    sw.WriteLine("Результат проверки блока: " + label38.Text);

                    sw.WriteLine("4.6.4");
                    sw.WriteLine("Измерение U E4-E5 (PV3): " + textBox18.Text);
                    sw.WriteLine("Измерение U X2:7-X2:8 (PV2): " + textBox17.Text);
                    sw.WriteLine("Расчёт I X2:7-X2:8: " + textBox15.Text);
                    sw.WriteLine("Расчёт R X2:7-X2:8: " + textBox14.Text);
                    sw.WriteLine("Расчёт δ X2:7-X2:8: " + textBox16.Text);
                    sw.WriteLine("Результат проверки блока: " + label49.Text);

                    sw.WriteLine("4.6.5");
                    sw.WriteLine("Измерение I X1:1-X1:2 (PA1): " + textBox25.Text);
                    sw.WriteLine("Измерение U E2-E3 (PV3): " + textBox24.Text);
                    sw.WriteLine("Расчёт R X1:1-X1:2: " + textBox21.Text);
                    sw.WriteLine("Расчёт δ X1:1-X1:2: " + textBox23.Text);
                    sw.WriteLine("Результат проверки блока: " + label60.Text);

                    sw.WriteLine("4.6.6");
                    sw.WriteLine("Измерение I X1:1-X1:2 (PA1): " + textBox30.Text);
                    sw.WriteLine("Измерение U X2:1-X2:2 (PV2): " + textBox29.Text);
                    sw.WriteLine("Расчёт R X2:1-X2:2: " + textBox26.Text);
                    sw.WriteLine("Расчёт δ X2:1-X2:2: " + textBox28.Text);
                    sw.WriteLine("Результат проверки блока: " + label71.Text);

                    sw.WriteLine("------------------------------------------------------------------------------");
                    sw.WriteLine("");
                }
            }
        }
        private void OpenProtocolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(path))
            {
                Process.Start("pntn_protokol.txt");
                //Process.Start(Path.Combine(Application.StartupPath, "protokol.txt"));
            }
            else
            {
                MessageBox.Show("Файла не существует", "Ошибка");
            }
        }



        //-----Константы-------------------------------------------------------------------------
        private void CurrentConstantsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("δ X2:3-X2:6 = " + δ_X23_X26_62 + "\n" +
                            "I X2:4-X2:5 = " + I_X24_X25_63 + "   δ X2:4-X2:5 = " + δ_X24_X25_63 + "\n" +
                            "δ X1:5-X1:6 = " + δ_X15_X16_64 + "\n" +
                            "I X2:7-X2:8 = " + I_X27_X28_65 + "   δ X2:7-X2:8 = " + δ_X27_X28_65 + "\n" +
                            "δ X1:1-X1:2 = " + δ_X11_X12_66 + "\n" +
                            "δ X2:1-X2:2 = " + δ_X21_X22_67, "Константы");
        }
        private void OpenConstantsPntnconstToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (!File.Exists(path_const))
            {
                MessageBox.Show("Файл не существует, файл будет создан");

                using (FileStream fstream = new FileStream("pntn_const.txt", FileMode.OpenOrCreate))
                {
                    //MessageBox.Show("файл создан");
                }
            }
            else
            {
                //MessageBox.Show("файл уже существует");
            }


            if (File.Exists(path_const))
            {
                Process.Start("pntn_const.txt");
            }
            else
            {
                MessageBox.Show("файла не существует");
            }
        }
        private void DefaultConstantsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (StreamWriter sw = new StreamWriter(path_const))
            {
                sw.WriteLine("δ X2:3-X2:6 = "); sw.WriteLine("1,21224002");
                sw.WriteLine("");
                sw.WriteLine("I X2:4-X2:5 = "); sw.WriteLine("2,498096032");
                sw.WriteLine("");
                sw.WriteLine("δ X2:4-X2:5 = "); sw.WriteLine("1,248513674");
                sw.WriteLine("");
                sw.WriteLine("δ X1:5-X1:6 = "); sw.WriteLine("1,2120382");
                sw.WriteLine("");
                sw.WriteLine("I X2:7-X2:8 = "); sw.WriteLine("2,498096032");
                sw.WriteLine("");
                sw.WriteLine("δ X2:7-X2:8 = "); sw.WriteLine("1,248513674");
                sw.WriteLine("");
                sw.WriteLine("δ X1:1-X1:2 = "); sw.WriteLine("9090,04933015");
                sw.WriteLine("");
                sw.WriteLine("δ X2:1-X2:2 = "); sw.WriteLine("30,04933015");
                sw.WriteLine(""); sw.WriteLine(""); sw.WriteLine("");
                sw.WriteLine("------------------------------");
                sw.WriteLine("Важно! Значения констант должны стоять на определенных строках. " +
                    "Для разделения целой и дробной части использовать запятую (,) .");

            }
        }
        #region Метод считывания строки из файла
        //--------Cчитываем строку. В skip пишем какую строку нужно получить, в take - сколько строк начиная со skip(n) выводить--------
        /// <summary>
        /// Reads a specific line in the file
        /// </summary>
        /// <param name="numberString">Line number in the file</param>
        /// <param name="filePath">File name</param>
        /// <returns>Returns the string type string of the specified string</returns>
        public string GetString(int numberString, string filePath)
        {
            IEnumerable<string> result = File.ReadLines(filePath).Skip(numberString - 1).Take(1);

            string newString = null;

            foreach (string str in result)
            {
                newString += str;
            }
            return newString;
        }


        #endregion

        //-------about-----------------------------------------------------------------------------
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ООО \"НПО САУТ\"\n\nАнтонов А.В.\na.antonov@saut.ru\n\nMarch 2022", "about");
        }

    }
}
