using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace PNTN_prov
{
    public partial class MainForm : Form
    {
        public decimal U { get; private set; }
        public decimal I { get; private set; }
        public decimal СonstantFromFile { get; private set; }

        private double _accuracy = 0.1; //погрешность

        private readonly DeviceData _formForProtocol = new DeviceData();//форма ввода данных блока для сохранения в протокол
        private readonly CheckupSchems _formSchemes = new CheckupSchems();//форма схем проверок
        
        private readonly string _pathProtocolFile = $"{Environment.CurrentDirectory}/{Properties.Resources.ProtokolFile}";


        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!File.Exists(Constants._pathConstantFile))
            {
                using (var fstream = new FileStream(Properties.Resources.ConstantsFile, FileMode.OpenOrCreate)) { }
                Constants._EnterDefaultConstantsInFile();

            }//создаёт файл констант, вписвает значения по умолчанию

            Constants.LoadsConstantData();//считывает данные из файла канстант

            if (!File.Exists(_pathProtocolFile))
            {
                using (var fstream = new FileStream(Properties.Resources.ProtokolFile, FileMode.OpenOrCreate)) { }
            }//создаёт файл протокол

            trackBar1.Scroll += AccuracyTrackBar_Scroll;
            label20.Text = $"Допустимая расчётная погрешность: {_accuracy}";
        }

        private void _calculationR(decimal U, decimal I, TextBox calcR, TextBox measurementCalc, Label labelsTextСorrectFaulty, decimal constantFromFile)
        {
            if (I != 0)
            {
                calcR.Text = Convert.ToString(Math.Round((U / I), 5));

                measurementCalc.Text = Convert.ToString(Math.Round(100 * (((U / I) - constantFromFile) / constantFromFile), 5));

                if ((Convert.ToDouble(measurementCalc.Text)) < _accuracy &&
                    (Convert.ToDouble(measurementCalc.Text)) > -(_accuracy) &&
                    (Convert.ToDouble(measurementCalc.Text)) != 0)
                {
                    measurementCalc.BackColor = Color.SpringGreen;
                    labelsTextСorrectFaulty.Text = Properties.Resources.DefaultLabelsTextСorrect;
                }
                else
                {
                    measurementCalc.BackColor = Color.White;
                    labelsTextСorrectFaulty.Text = Properties.Resources.DefaultLabelsTextFaulty;
                }
            }

        }//метод расчёта 1
       
        private void _calculationIR(decimal U1, decimal U2, TextBox calcI, TextBox calcR, TextBox measurementCalc, Label labelsTextСorrectFaulty, decimal constantFromFile1, decimal constantFromFile2)
        {
            if (U1 != 0)
            {

                calcI.Text = Convert.ToString(Math.Round((U1 / constantFromFile1), 5));

                calcR.Text = Convert.ToString(Math.Round((U2 / (U1 / constantFromFile1)), 5));

                measurementCalc.Text = Convert.ToString(Math.Round((100 * (((U2 / (U1 / constantFromFile1)) - constantFromFile2) / constantFromFile2)), 5));

                if ((Convert.ToDouble(measurementCalc.Text)) < _accuracy &&
                    (Convert.ToDouble(measurementCalc.Text)) > -(_accuracy) &&
                    (Convert.ToDouble(measurementCalc.Text)) != 0)
                {
                    measurementCalc.BackColor = Color.SpringGreen;
                    labelsTextСorrectFaulty.Text = Properties.Resources.DefaultLabelsTextСorrect;
                }
                else
                {
                    measurementCalc.BackColor = Color.White;
                    labelsTextСorrectFaulty.Text = Properties.Resources.DefaultLabelsTextFaulty;
                }

            }
        }//метод расчёта 2
       
        private void _notWriteLetters(KeyPressEventArgs e)//не вписывает буквы
        {
            if (Char.IsNumber(e.KeyChar) || (e.KeyChar == Convert.ToChar(",")) || e.KeyChar == '\b')
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
        }
        
        private void _writeZeroComma(TextBox text)//метод вписывания "0" и ","
        {
            if (text.Text == ",")
            {
                text.Text = "0,";
                text.SelectionStart = text.Text.Length;
            }
        }
        
        private void _lightCorrect(TextBox measurementCalc, Label labelsTextСorrectFaulty)
        {
            if ((Convert.ToDouble(measurementCalc.Text)) < _accuracy &&
                (Convert.ToDouble(measurementCalc.Text)) > -(_accuracy) &&
                (Convert.ToDouble(measurementCalc.Text)) != 0)
            {
                measurementCalc.BackColor = Color.SpringGreen;
                labelsTextСorrectFaulty.Text = Properties.Resources.DefaultLabelsTextСorrect;
            }
            else
            {
                measurementCalc.BackColor = Color.White;
                labelsTextСorrectFaulty.Text = Properties.Resources.DefaultLabelsTextFaulty;
            }
        }//подстветка исправности/неисправности
       
        private void _incorrectData(TextBox textBox)
        {
            textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1);
            MessageBox.Show("Введено некорретное значение", "Ошибка");
        }//при вводе некорректных значений


        #region points of technical conditions
        //4.6.1
        private void CurrentMeasurement461Box_KeyPress(object sender, KeyPressEventArgs e)
        {
            _notWriteLetters(e);
        }
        
        private void CurrentMeasurement461Box_TextChanged(object sender, EventArgs e)
        {
            _writeZeroComma(CurrentMeasurement461);
            try
            {
                if (CurrentMeasurement461.Text != "")
                    I = decimal.Parse(CurrentMeasurement461.Text);
                СonstantFromFile = Constants.δ_X23_X26;

                _calculationR(U, I, textBox20, textBox3, label1, СonstantFromFile);
            }
            catch
            {
                _incorrectData(CurrentMeasurement461);
            }
        }
       
        private void VoltageMeasurement461Box_KeyPress(object sender, KeyPressEventArgs e)
        {
            _notWriteLetters(e);
        }
        
        private void VoltageMeasurement461Box_TextChanged(object sender, EventArgs e)
        {
            _writeZeroComma(VoltageMeasurement461);
            try
            {
                if (VoltageMeasurement461.Text != "")
                    U = decimal.Parse(VoltageMeasurement461.Text);
                СonstantFromFile = Constants.δ_X23_X26;

                _calculationR(U, I, textBox20, textBox3, label1, СonstantFromFile);
            }
            catch
            {
                _incorrectData(VoltageMeasurement461);
            }
        }

        //4.6.2
        private void VoltagePV3Measurement462Box_KeyPress(object sender, KeyPressEventArgs e)
        {
            _notWriteLetters(e);
        }
       
        private void VoltagePV3Measurement462Box_TextChanged(object sender, EventArgs e)
        {
            _writeZeroComma(VoltagePV3Measurement462);
            try
            {
                if (VoltagePV3Measurement462.Text != "")
                    I = decimal.Parse(VoltagePV3Measurement462.Text);

                _calculationIR(I, U, textBox5, textBox4, textBox6, label27, Constants.I_X24_X25, Constants.δ_X24_X25);
            }
            catch
            {
                _incorrectData(VoltagePV3Measurement462);
            }
        }
       
        private void VoltagePV2Measurement462BoxBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            _notWriteLetters(e);
        }
        
        private void VoltagePV2Measurement462BoxBox_TextChanged(object sender, EventArgs e)
        {
            _writeZeroComma(VoltagePV2Measurement462);
            try
            {
                if (VoltagePV2Measurement462.Text != "")
                    U = decimal.Parse(VoltagePV2Measurement462.Text);

                _calculationIR(I, U, textBox5, textBox4, textBox6, label27, Constants.I_X24_X25, Constants.δ_X24_X25);
            }
            catch
            {
                _incorrectData(VoltagePV2Measurement462);
            }
            
        }

        //4.6.3
        private void CurrentMeasurement463Box_KeyPress(object sender, KeyPressEventArgs e)
        {
            _notWriteLetters(e);
        }
       
        private void CurrentMeasurement463Box_TextChanged(object sender, EventArgs e)
        {
            _writeZeroComma(CurrentMeasurement463);
            try
            {
                if (CurrentMeasurement463.Text != "")
                    I = decimal.Parse(CurrentMeasurement463.Text);

                СonstantFromFile = Constants.δ_X15_X16;
                _calculationR(U, I, textBox9, textBox11, label38, СonstantFromFile);
            }
            catch
            {
                _incorrectData(CurrentMeasurement463);
            }
            
        }
        
        private void VoltageMeasurement463Box_KeyPress(object sender, KeyPressEventArgs e)
        {
            _notWriteLetters(e);
        }
        
        private void VoltageMeasurement463Box_TextChanged(object sender, EventArgs e)
        {
            _writeZeroComma(VoltageMeasurement463);
            try
            {
                if (VoltageMeasurement463.Text != "")
                    U = decimal.Parse(VoltageMeasurement463.Text);

                СonstantFromFile = Constants.δ_X15_X16;
                _calculationR(U, I, textBox9, textBox11, label38, СonstantFromFile);
            }
            catch 
            {
                _incorrectData(VoltageMeasurement463);
            }
            
        }

        //4.6.4
        private void VoltagePV3Measurement464Box_KeyPress(object sender, KeyPressEventArgs e)
        {
            _notWriteLetters(e);
        }
       
        private void VoltagePV3Measurement464Box_TextChanged(object sender, EventArgs e)
        {
            _writeZeroComma(VoltagePV3Measurement464);
            try
            {
                if (VoltagePV3Measurement464.Text != "")
                    I = decimal.Parse(VoltagePV3Measurement464.Text);

                _calculationIR(I, U, textBox15, textBox14, textBox16, label49, Constants.I_X27_X28, Constants.δ_X27_X28);
            }
            catch
            {
                _incorrectData(VoltagePV3Measurement464);
            }
            
        }
       
        private void VoltagePV2Measurement464Box_KeyPress(object sender, KeyPressEventArgs e)
        {
            _notWriteLetters(e);
        }
       
        private void VoltagePV2Measurement464Box_TextChanged(object sender, EventArgs e)
        {
            _writeZeroComma(VoltagePV2Measurement464);
            try
            {
                if (VoltagePV2Measurement464.Text != "")
                    U = decimal.Parse(VoltagePV2Measurement464.Text);

                _calculationIR(I, U, textBox15, textBox14, textBox16, label49, Constants.I_X27_X28, Constants.δ_X27_X28);
            }
            catch 
            {
                _incorrectData(VoltagePV2Measurement464);
            }
            
        }

        //4.6.5
        private void CurrentMeasurement465Box_KeyPress(object sender, KeyPressEventArgs e)
        {
            _notWriteLetters(e);
        }
       
        private void CurrentMeasurement465Box_TextChanged(object sender, EventArgs e)
        {
            _writeZeroComma(CurrentMeasurement465);
            try
            {
                if (CurrentMeasurement465.Text != "")
                    I = decimal.Parse(CurrentMeasurement465.Text);

                СonstantFromFile = Constants.δ_X11_X12;
                _calculationR(U, I, textBox21, textBox23, label60, СonstantFromFile);
            }
            catch
            {
                _incorrectData(CurrentMeasurement465);
            }
            
        }
       
        private void VoltageMeasurement465Box_KeyPress(object sender, KeyPressEventArgs e)
        {
            _notWriteLetters(e);
        }
        
        private void VoltageMeasurement465Box_TextChanged(object sender, EventArgs e)
        {
            _writeZeroComma(VoltageMeasurement465);
            try
            {
                if (VoltageMeasurement465.Text != "")
                    U = decimal.Parse(VoltageMeasurement465.Text);

                СonstantFromFile = Constants.δ_X11_X12;
                _calculationR(U, I, textBox21, textBox23, label60, СonstantFromFile);
            }
            catch
            {
                _incorrectData(VoltageMeasurement465);
            }
        }

        //4.6.6
        private void CurrentMeasurement466Box_KeyPress(object sender, KeyPressEventArgs e)
        {
            _notWriteLetters(e);
        }
       
        private void CurrentMeasurement466Box_TextChanged(object sender, EventArgs e)
        {
            _writeZeroComma(CurrentMeasurement466);
            try
            {
                if (CurrentMeasurement466.Text != "")
                    I = decimal.Parse(CurrentMeasurement466.Text);

                СonstantFromFile = Constants.δ_X21_X22;
                _calculationR(U, I, textBox26, textBox28, label71, СonstantFromFile);

            }
            catch
            {
                _incorrectData(CurrentMeasurement466);
            }
        
        }
       
        private void VoltageMeasurement466_KeyPress(object sender, KeyPressEventArgs e)
        {
            _notWriteLetters(e);
        }
       
        private void VoltageMeasurement466_TextChanged(object sender, EventArgs e)
        {
            _writeZeroComma(VoltageMeasurement466);
            try
            {
                if (VoltageMeasurement466.Text != "")
                    U = decimal.Parse(VoltageMeasurement466.Text);

                СonstantFromFile = Constants.δ_X21_X22;
                _calculationR(U, I, textBox26, textBox28, label71, СonstantFromFile);
            }
            catch
            {
                _incorrectData(VoltageMeasurement466);
            }
           
        }
        #endregion



        private void AccuracyTrackBar_Scroll(object sender, EventArgs e)
        {
            label20.Text = $"Допустимая расчётная погрешность: {_accuracy}%";
            _accuracy = Convert.ToDouble(trackBar1.Value) / 10d;

            _lightCorrect(textBox3, label1);
            _lightCorrect(textBox6, label27);
            _lightCorrect(textBox11, label38);
            _lightCorrect(textBox16, label49);
            _lightCorrect(textBox23, label60);
            _lightCorrect(textBox28, label71);
        }//Бегунок расчётной погрешности, изменение подсветки исправности
       
        private void ResetAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentMeasurement461.Text = "0";
            VoltageMeasurement461.Text = "0";
            VoltagePV2Measurement462.Text = "0";
            VoltagePV3Measurement462.Text = "0";
            VoltageMeasurement463.Text = "0";
            CurrentMeasurement463.Text = "0";
            VoltagePV2Measurement464.Text = "0";
            VoltagePV3Measurement464.Text = "0";
            VoltageMeasurement465.Text = "0";
            CurrentMeasurement465.Text = "0";
            VoltageMeasurement466.Text = "0";
            CurrentMeasurement466.Text = "0";
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

        //схемы проверок
        private void InspectionSchemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _formSchemes.Show();
        }


        //действия с протоколом проверки
        private void WriteProtocolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _formForProtocol.ShowDialog();

            if (DataExchange.WriteCheckProtocol)
            {
                using (var sw = new StreamWriter(_pathProtocolFile, true))
                {
                    sw.WriteLine("------------------------------------------------------------------------------");
                    sw.Write($"Номер блока: {DataExchange.blockNumber}");
                    sw.WriteLine($"({DataExchange.releaseDate})");
                    sw.WriteLine($"Дата проверки: {DateTime.Now}");
                    sw.WriteLine($"Текущая допустимая погрешность: {_accuracy} %");

                    sw.WriteLine("4.6.1");
                    sw.WriteLine($"Измерение I X1:3-X1:4 (PA1): {CurrentMeasurement461.Text}");
                    sw.WriteLine($"Измерение U X2:3-X2:6 (PV3): {VoltageMeasurement461.Text}");
                    sw.WriteLine($"Расчёт R X2:3-X2:6: {textBox20.Text}");
                    sw.WriteLine($"Расчёт measurementCalc X2:3-X2:6: {textBox3.Text}");
                    sw.WriteLine($"Результат проверки блока: {label1.Text}");

                    sw.WriteLine("4.6.2");
                    sw.WriteLine($"Измерение U X2:3-X2:6 (PV3): {VoltagePV3Measurement462.Text}");
                    sw.WriteLine($"Измерение U X2:4-X2:5 (PV2): {VoltagePV2Measurement462.Text}");
                    sw.WriteLine($"Расчёт I X2:4-X2:5: {textBox5.Text}");
                    sw.WriteLine($"Расчёт R X2:4-X2:5: {textBox4.Text}");
                    sw.WriteLine($"Расчёт measurementCalc X2:4-X2:5: {textBox6.Text}");
                    sw.WriteLine($"Результат проверки блока: {label27.Text}");

                    sw.WriteLine("4.6.3");
                    sw.WriteLine($"Измерение I X1:5-X1:6 (PA1): {CurrentMeasurement463.Text}");
                    sw.WriteLine($"Измерение U E4-E5 (PV3): {VoltageMeasurement463.Text}");
                    sw.WriteLine($"Расчёт R X1:5-X1:6: {textBox9.Text}");
                    sw.WriteLine($"Расчёт measurementCalc X1:5-X1:6: {textBox11.Text}");
                    sw.WriteLine($"Результат проверки блока: {label38.Text}");

                    sw.WriteLine("4.6.4");
                    sw.WriteLine($"Измерение U E4-E5 (PV3): {VoltagePV3Measurement464.Text}");
                    sw.WriteLine($"Измерение U X2:7-X2:8 (PV2): {VoltagePV2Measurement464.Text}");
                    sw.WriteLine($"Расчёт I X2:7-X2:8: {textBox15.Text}");
                    sw.WriteLine($"Расчёт R X2:7-X2:8: {textBox14.Text}");
                    sw.WriteLine($"Расчёт measurementCalc X2:7-X2:8: {textBox16.Text}");
                    sw.WriteLine($"Результат проверки блока: {label49.Text}");

                    sw.WriteLine("4.6.5");
                    sw.WriteLine($"Измерение I X1:1-X1:2 (PA1): {CurrentMeasurement465.Text}");
                    sw.WriteLine($"Измерение U E2-E3 (PV3): {VoltageMeasurement465.Text}");
                    sw.WriteLine($"Расчёт R X1:1-X1:2: {textBox21.Text}");
                    sw.WriteLine($"Расчёт measurementCalc X1:1-X1:2: {textBox23.Text}");
                    sw.WriteLine($"Результат проверки блока: {label60.Text}");

                    sw.WriteLine("4.6.6");
                    sw.WriteLine($"Измерение I X1:1-X1:2 (PA1): {CurrentMeasurement466.Text}");
                    sw.WriteLine($"Измерение U X2:1-X2:2 (PV2): {VoltageMeasurement466.Text}");
                    sw.WriteLine($"Расчёт R X2:1-X2:2: {textBox26.Text}");
                    sw.WriteLine($"Расчёт measurementCalc X2:1-X2:2: {textBox28.Text}");
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
            }
            else
            {
                MessageBox.Show("Файла не существует", "Ошибка");
            }
        }

        //действия с константами
        private void CurrentConstantsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Constants._ShowCurrentConstants();
        }

        private void OpenConstantsPntnConstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Constants._pathConstantFile))
            {
                MessageBox.Show("Файл не существует, файл будет создан");
                using (var fstream = new FileStream(Properties.Resources.ConstantsFile, FileMode.OpenOrCreate)) { }
            }

            if (File.Exists(Constants._pathConstantFile))
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
            Constants._EnterDefaultConstantsInFile();
        }


        //about 
        private void VersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Версия 4", "Версия");
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ООО \"НПО САУТ\"\n\nАнтонов А.В.\na.antonov@saut.ru\n\nMarch 2022", "about");
        }


        private void MainFormBeforeFormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Выйти из программы?", "Выход",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
