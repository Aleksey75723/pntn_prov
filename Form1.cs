using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace PNTN_prov
{
    public partial class Form1 : Form
    {
        decimal U;
        decimal I;
        decimal constantFromFile;

        public double accuracy = 0.1; //погрешность

        Form2 formForProtocol = new Form2();//форма ввода данных блока для сохранения в протокол
        Form3 formSchemes = new Form3();//форма схем проверок

        private string _pathProtocolFile = $"{Environment.CurrentDirectory}/" + Properties.Resources.ProtokolFile;
        private string _pathConstantFile = $"{Environment.CurrentDirectory}/" + Properties.Resources.ConstantsFile;

        //считываемые из файла константы
        decimal δ_X23_X26;
        decimal I_X24_X25; decimal δ_X24_X25;
        decimal δ_X15_X16;
        decimal I_X27_X28; decimal δ_X27_X28;
        decimal δ_X11_X12;
        decimal δ_X21_X22;


        public Form1()
        {
            InitializeComponent();
        }

        //-------------------------------------------------------------
        private void Form1_Load(object sender, EventArgs e)
        {
            //Создаёт файл pntn_const при запуске если не существует
            if (!File.Exists(_pathConstantFile))
            {
                using (FileStream fstream = new FileStream(Properties.Resources.ConstantsFile, FileMode.OpenOrCreate)) { }

                EnterDefaultConstantsInFile();
            }

            //загружает из файла константы в программу, в случае некорректной записи в файле - выдаст сообщение
            if (File.Exists(_pathConstantFile))
            {
                using (StreamReader sr = new StreamReader(_pathConstantFile))
                {
                    δ_X23_X26 = (ReadingNeedString(2, Properties.Resources.accuracy_X2_3_X2_6));
                    I_X24_X25 = (ReadingNeedString(5, Properties.Resources.current_X2_4_X2_5));
                    δ_X24_X25 = (ReadingNeedString(8, Properties.Resources.accuracy_X2_4_X2_5));
                    δ_X15_X16 = (ReadingNeedString(11, Properties.Resources.accuracy_X1_5_X1_6));
                    I_X27_X28 = (ReadingNeedString(14, Properties.Resources.current_X2_7_X2_8));
                    δ_X27_X28 = (ReadingNeedString(17, Properties.Resources.accuracy_X2_7_X2_8));
                    δ_X11_X12 = (ReadingNeedString(20, Properties.Resources.accuracy__X1_1_X1_2));
                    δ_X21_X22 = (ReadingNeedString(23, Properties.Resources.accuracy_X2_1_X2_2));
                }
            }

            //Создаёт файл pntn_protokol при запуске если не существует
            if (!File.Exists(_pathProtocolFile))
            {
                using (FileStream fstream = new FileStream(Properties.Resources.ProtokolFile, FileMode.OpenOrCreate)) { }
            }


            trackBar1.Scroll += TrackBar1_Scroll;
            label20.Text = String.Format($"Допустимая расчётная погрешность: {accuracy}");
        }
        //-------------------------------------------------------------
        private void Calculation_R(decimal U_, decimal I_, TextBox R_calc, TextBox measurement_calc, Label label_, decimal const__)
        {

            if (I_ != 0)
            {

                R_calc.Text = Convert.ToString(Math.Round((U_ / I_), 5));

                measurement_calc.Text = Convert.ToString(Math.Round(100 * (((U_ / I_) - const__) / const__), 5));

                if ((Convert.ToDouble(measurement_calc.Text)) < accuracy &
                    (Convert.ToDouble(measurement_calc.Text)) > -(accuracy) &
                    (Convert.ToDouble(measurement_calc.Text)) != 0)
                {
                    measurement_calc.BackColor = Color.SpringGreen;
                    label_.Text = Properties.Resources.DefaultLabelsTextСorrect;
                }
                else
                {
                    measurement_calc.BackColor = Color.White;
                    label_.Text = Properties.Resources.DefaultLabelsTextFaulty;
                }
            }

        }//метод расчёта 1
        //-------------------------------------------------------------
        private void Calculation_I_R(decimal U1, decimal U2, TextBox I_calc, TextBox R_calc, TextBox measurement_calc, Label label_, decimal const_1_, decimal const_2_)
        {
            if (U1 != 0)
            {

                I_calc.Text = Convert.ToString(Math.Round((U1 / const_1_), 5));

                R_calc.Text = Convert.ToString(Math.Round((U2 / (U1 / const_1_)), 5));

                measurement_calc.Text = Convert.ToString(Math.Round((100 * (((U2 / (U1 / const_1_)) - const_2_) / const_2_)), 5));

                if ((Convert.ToDouble(measurement_calc.Text)) < accuracy &
                    (Convert.ToDouble(measurement_calc.Text)) > -(accuracy) &
                    (Convert.ToDouble(measurement_calc.Text)) != 0)
                {
                    measurement_calc.BackColor = Color.SpringGreen;
                    label_.Text = Properties.Resources.DefaultLabelsTextСorrect;
                }
                else
                {
                    measurement_calc.BackColor = Color.White;
                    label_.Text = Properties.Resources.DefaultLabelsTextFaulty;
                }

            }
        }//метод расчёта 2
        //-------------------------------------------------------------------------
        private static void NotWriteLetters(KeyPressEventArgs e)//не вписывает буквы
        {
            if (Char.IsNumber(e.KeyChar) | (e.KeyChar == Convert.ToChar(",")) | e.KeyChar == '\b')
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
        }
        //-------------------------------------------------------------------------
        private void WriteZeroComma(TextBox text)//метод вписывания "0" и ","
        {
            if (text.Text == ",")
            {
                text.Text = "0,";
                text.SelectionStart = text.Text.Length;
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
                correct.Text = Properties.Resources.DefaultLabelsTextСorrect;
            }
            else
            {
                δ_.BackColor = Color.White;
                correct.Text = Properties.Resources.DefaultLabelsTextFaulty;
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
        private void Form1_BeforeFormClosing_1(object sender, FormClosingEventArgs e)//Спрашивает перед закрытием
        {
            var result = MessageBox.Show("Выйти из программы?", "Выход",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
        //-------------------------------------------------------------------------  



        private void ClearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "0";
            textBox2.Text = "0";
            textBox7.Text = "0";
            textBox8.Text = "0";
            textBox12.Text = "0";
            textBox13.Text = "0";
            textBox17.Text = "0";
            textBox18.Text = "0";
            textBox24.Text = "0";
            textBox25.Text = "0";
            textBox29.Text = "0";
            textBox30.Text = "0";
            textBox3.Text = "0";
            textBox4.Text = "0";
            textBox5.Text = "0";
            textBox6.Text = "0";
            textBox9.Text = "0";
            textBox11.Text = "0";
            textBox14.Text = "0";
            textBox15.Text = "0";
            textBox16.Text = "0";
            textBox20.Text = "0";
            textBox21.Text = "0";
            textBox23.Text = "0";
            textBox26.Text = "0";
            textBox28.Text = "0";

            textBox3.BackColor = Color.White;
            textBox6.BackColor = Color.White;
            textBox11.BackColor = Color.White;
            textBox16.BackColor = Color.White;
            textBox23.BackColor = Color.White;
            textBox28.BackColor = Color.White;

            label1.Text = Properties.Resources.DefaultLabelsTextFaulty;
            label27.Text = Properties.Resources.DefaultLabelsTextFaulty;
            label38.Text = Properties.Resources.DefaultLabelsTextFaulty;
            label49.Text = Properties.Resources.DefaultLabelsTextFaulty;
            label60.Text = Properties.Resources.DefaultLabelsTextFaulty;
            label71.Text = Properties.Resources.DefaultLabelsTextFaulty;
        }


        #region Пункты ТУ
        //-------4.6.1---------------------------------------------------
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            NotWriteLetters(e);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            WriteZeroComma(textBox1);

            if (textBox1.Text != "")
                I = decimal.Parse(textBox1.Text);
            constantFromFile = δ_X23_X26;
            Calculation_R(U, I, textBox20, textBox3, label1, constantFromFile);
        }


        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            NotWriteLetters(e);
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            WriteZeroComma(textBox2);

            if (textBox2.Text != "")
                U = decimal.Parse(textBox2.Text);
            constantFromFile = δ_X23_X26;
            Calculation_R(U, I, textBox20, textBox3, label1, constantFromFile);
        }


        //-------4.6.2----------------------------------------------------
        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            NotWriteLetters(e);
        }
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            WriteZeroComma(textBox8);

            if (textBox8.Text != "")
                I = decimal.Parse(textBox8.Text);

            Calculation_I_R(I, U, textBox5, textBox4, textBox6, label27, I_X24_X25, δ_X24_X25);


        }


        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            NotWriteLetters(e);
        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            WriteZeroComma(textBox7);

            if (textBox7.Text != "")
                U = decimal.Parse(textBox7.Text);

            Calculation_I_R(I, U, textBox5, textBox4, textBox6, label27, I_X24_X25, δ_X24_X25);
        }





        //-------4.6.3--------------------------------------------------
        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
            NotWriteLetters(e);
        }
        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            WriteZeroComma(textBox13);

            if (textBox13.Text != "")
                I = decimal.Parse(textBox13.Text);

            constantFromFile = δ_X15_X16;
            Calculation_R(U, I, textBox9, textBox11, label38, constantFromFile);
        }


        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            NotWriteLetters(e);
        }
        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            WriteZeroComma(textBox12);

            if (textBox12.Text != "")
                U = decimal.Parse(textBox12.Text);

            constantFromFile = δ_X15_X16;
            Calculation_R(U, I, textBox9, textBox11, label38, constantFromFile);
        }





        //-------4.6.4----------------------------------------------------
        private void textBox18_KeyPress(object sender, KeyPressEventArgs e)
        {
            NotWriteLetters(e);
        }
        private void textBox18_TextChanged(object sender, EventArgs e)
        {
            WriteZeroComma(textBox18);

            if (textBox18.Text != "")
                I = decimal.Parse(textBox18.Text);

            Calculation_I_R(I, U, textBox15, textBox14, textBox16, label49, I_X27_X28, δ_X27_X28);
        }


        private void textBox17_KeyPress(object sender, KeyPressEventArgs e)
        {
            NotWriteLetters(e);
        }
        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            WriteZeroComma(textBox17);

            if (textBox17.Text != "")
                U = decimal.Parse(textBox17.Text);

            Calculation_I_R(I, U, textBox15, textBox14, textBox16, label49, I_X27_X28, δ_X27_X28);
        }





        //-------4.6.5-------------------------------------------------
        private void textBox25_KeyPress(object sender, KeyPressEventArgs e)
        {
            NotWriteLetters(e);
        }
        private void textBox25_TextChanged(object sender, EventArgs e)
        {
            WriteZeroComma(textBox25);


            if (textBox25.Text != "")
                I = decimal.Parse(textBox25.Text);
            constantFromFile = δ_X11_X12;
            Calculation_R(U, I, textBox21, textBox23, label60, constantFromFile);
        }


        private void textBox24_KeyPress(object sender, KeyPressEventArgs e)
        {
            NotWriteLetters(e);
        }
        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            WriteZeroComma(textBox24);

            if (textBox24.Text != "")
                U = decimal.Parse(textBox24.Text);

            constantFromFile = δ_X11_X12;
            Calculation_R(U, I, textBox21, textBox23, label60, constantFromFile);
        }




        //-------4.6.6-----------------------------------------------
        private void textBox30_KeyPress(object sender, KeyPressEventArgs e)
        {
            NotWriteLetters(e);
        }
        private void textBox30_TextChanged(object sender, EventArgs e)
        {
            WriteZeroComma(textBox30);

            if (textBox30.Text != "")
                I = decimal.Parse(textBox30.Text);

            constantFromFile = δ_X21_X22;
            Calculation_R(U, I, textBox26, textBox28, label71, constantFromFile);
        }


        private void textBox29_KeyPress(object sender, KeyPressEventArgs e)
        {
            NotWriteLetters(e);
        }
        private void textBox29_TextChanged(object sender, EventArgs e)
        {
            WriteZeroComma(textBox29);

            if (textBox29.Text != "")
                U = decimal.Parse(textBox29.Text);

            constantFromFile = δ_X21_X22;
            Calculation_R(U, I, textBox26, textBox28, label71, constantFromFile);
        }

        #endregion


        //-------схемыПроверок----------------------------------------------------------------------
        private void InspectionSchemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formSchemes.Show();
        }


        //---------Запись в протокол-------------------------------------------------------------
        private void WriteProtocolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formForProtocol.ShowDialog();

            if (DataExchange.button1ClickProv)
            {
                using (StreamWriter sw = new StreamWriter(_pathProtocolFile, true))
                {

                    sw.WriteLine("------------------------------------------------------------------------------");
                    sw.Write("Номер блока: {0}", DataExchange.text1); sw.WriteLine("({0})", DataExchange.text2);
                    sw.WriteLine($"Дата проверки: {DateTime.Now}");
                    sw.WriteLine($"Текущая допустимая погрешность: {accuracy} %");

                    sw.WriteLine("4.6.1");
                    sw.WriteLine($"Измерение I X1:3-X1:4 (PA1): {textBox1.Text}");
                    sw.WriteLine($"Измерение U X2:3-X2:6 (PV3): {textBox2.Text}");
                    sw.WriteLine($"Расчёт R X2:3-X2:6: {textBox20.Text}");
                    sw.WriteLine($"Расчёт δ X2:3-X2:6: {textBox3.Text}");
                    sw.WriteLine($"Результат проверки блока: {label1.Text}");

                    sw.WriteLine("4.6.2");
                    sw.WriteLine($"Измерение U X2:3-X2:6 (PV3): {textBox8.Text}");
                    sw.WriteLine($"Измерение U X2:4-X2:5 (PV2): {textBox7.Text}");
                    sw.WriteLine($"Расчёт I X2:4-X2:5: {textBox5.Text}");
                    sw.WriteLine($"Расчёт R X2:4-X2:5: {textBox4.Text}");
                    sw.WriteLine($"Расчёт δ X2:4-X2:5: {textBox6.Text}");
                    sw.WriteLine($"Результат проверки блока: {label27.Text}");

                    sw.WriteLine("4.6.3");
                    sw.WriteLine($"Измерение I X1:5-X1:6 (PA1): {textBox13.Text}");
                    sw.WriteLine($"Измерение U E4-E5 (PV3): {textBox12.Text}");
                    sw.WriteLine($"Расчёт R X1:5-X1:6: {textBox9.Text}");
                    sw.WriteLine($"Расчёт δ X1:5-X1:6: {textBox11.Text}");
                    sw.WriteLine($"Результат проверки блока: {label38.Text}");

                    sw.WriteLine("4.6.4");
                    sw.WriteLine($"Измерение U E4-E5 (PV3): {textBox18.Text}");
                    sw.WriteLine($"Измерение U X2:7-X2:8 (PV2): {textBox17.Text}");
                    sw.WriteLine($"Расчёт I X2:7-X2:8: {textBox15.Text}");
                    sw.WriteLine($"Расчёт R X2:7-X2:8: {textBox14.Text}");
                    sw.WriteLine($"Расчёт δ X2:7-X2:8: {textBox16.Text}");
                    sw.WriteLine($"Результат проверки блока: {label49.Text}");

                    sw.WriteLine("4.6.5");
                    sw.WriteLine($"Измерение I X1:1-X1:2 (PA1): {textBox25.Text}");
                    sw.WriteLine($"Измерение U E2-E3 (PV3): {textBox24.Text}");
                    sw.WriteLine($"Расчёт R X1:1-X1:2: {textBox21.Text}");
                    sw.WriteLine($"Расчёт δ X1:1-X1:2: {textBox23.Text}");
                    sw.WriteLine($"Результат проверки блока: {label60.Text}");

                    sw.WriteLine("4.6.6");
                    sw.WriteLine($"Измерение I X1:1-X1:2 (PA1): {textBox30.Text}");
                    sw.WriteLine($"Измерение U X2:1-X2:2 (PV2): {textBox29.Text}");
                    sw.WriteLine($"Расчёт R X2:1-X2:2: {textBox26.Text}");
                    sw.WriteLine($"Расчёт δ X2:1-X2:2: {textBox28.Text}");
                    sw.WriteLine($"Результат проверки блока: {label71.Text}");

                    sw.WriteLine("------------------------------------------------------------------------------");
                    sw.WriteLine("");
                }
            }
        }
        private void OpenProtocolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(_pathProtocolFile))
            {
                Process.Start(Properties.Resources.ProtokolFile);
                //Process.Start(Path.Combine(Application.StartupPath, "protokol.txt"));
            }
            else
            {
                MessageBox.Show("Файла не существует", "Ошибка");
            }
        }



        //-----Константы-------------------------------------------------------------------------
        public decimal ReadingNeedString(int lineNumber, string nameInputData)
        {
            try { return Convert.ToDecimal(GetString(lineNumber, Properties.Resources.ConstantsFile)); }//считывает только определенную строку 
            catch { MessageBox.Show("Ошибка чтения файла pntn_const\nзначение константы " + nameInputData + " некорректное", "Ошибка"); return 0; }
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
        private void EnterDefaultConstantsInFile()
        {
            using (StreamWriter sw = new StreamWriter(_pathConstantFile))
            {
                sw.WriteLine(Properties.Resources.accuracy_X2_3_X2_6 + " = "); sw.WriteLine("1,21224002"); sw.WriteLine("");
                sw.WriteLine(Properties.Resources.current_X2_4_X2_5 + " = "); sw.WriteLine("2,498096032"); sw.WriteLine("");
                sw.WriteLine(Properties.Resources.accuracy_X2_4_X2_5 + " = "); sw.WriteLine("1,248513674"); sw.WriteLine("");
                sw.WriteLine(Properties.Resources.accuracy_X1_5_X1_6 + " = "); sw.WriteLine("1,2120382"); sw.WriteLine("");
                sw.WriteLine(Properties.Resources.current_X2_7_X2_8 + " = "); sw.WriteLine("2,498096032"); sw.WriteLine("");
                sw.WriteLine(Properties.Resources.accuracy_X2_7_X2_8 + " = "); sw.WriteLine("1,248513674"); sw.WriteLine("");
                sw.WriteLine(Properties.Resources.accuracy__X1_1_X1_2 + " = "); sw.WriteLine("9090,04933015"); sw.WriteLine("");
                sw.WriteLine(Properties.Resources.accuracy_X2_1_X2_2 + " = "); sw.WriteLine("30,04933015");
                sw.WriteLine("\n\n");
                sw.WriteLine("------------------------------");
                sw.WriteLine("Важно! Значения констант должны стоять на определенных строках. " +
                    "Для разделения целой и дробной части использовать запятую (,).");
            }
        }
        private void CurrentConstantsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Properties.Resources.accuracy_X2_3_X2_6 + " = " + δ_X23_X26 + "\n" +
                            Properties.Resources.current_X2_4_X2_5 + " = " + I_X24_X25 + "  " +
                            Properties.Resources.accuracy_X2_4_X2_5 + " = " + δ_X24_X25 + "\n" +
                            Properties.Resources.accuracy_X1_5_X1_6 + " = " + δ_X15_X16 + "\n" +
                            Properties.Resources.current_X2_7_X2_8 + " = " + I_X27_X28 + "  " +
                            Properties.Resources.accuracy_X2_7_X2_8 + " = " + δ_X27_X28 + "\n" +
                            Properties.Resources.accuracy__X1_1_X1_2 + " = " + δ_X11_X12 + "\n" +
                            Properties.Resources.accuracy_X2_1_X2_2 + " = " + δ_X21_X22, "Константы");
        }
        private void OpenConstantsPntnconstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!File.Exists(_pathConstantFile))
            {
                MessageBox.Show("Файл не существует, файл будет создан");
                using (FileStream fstream = new FileStream(Properties.Resources.ConstantsFile, FileMode.OpenOrCreate)) { }
            }

            if (File.Exists(_pathConstantFile))
            {
                Process.Start(Properties.Resources.ConstantsFile);
            }
            else
            {
                MessageBox.Show("файла не существует");
            }
        }
        private void DefaultConstantsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnterDefaultConstantsInFile();
        }


        //-------about-----------------------------------------------------------------------------
        private void VersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Версия 4", "Версия");
        }
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ООО \"НПО САУТ\"\n\nАнтонов А.В.\na.antonov@saut.ru\n\nMarch 2022", "about");
        }

    }
}
