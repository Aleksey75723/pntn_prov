using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PNTN_prov
{
    class Constants
    {
        internal static readonly string _pathConstantFile = $"{Environment.CurrentDirectory}/{Properties.Resources.ConstantsFile}";

        //считываемые из файла
        internal static decimal δ_X23_X26 { get; private set; }
        internal static decimal I_X24_X25 { get; private set; }
        internal static decimal δ_X24_X25 { get; private set; }
        internal static decimal δ_X15_X16 { get; private set; }
        internal static decimal I_X27_X28 { get; private set; }
        internal static decimal δ_X27_X28 { get; private set; }
        internal static decimal δ_X11_X12 { get; private set; }
        internal static decimal δ_X21_X22 { get; private set; }

        #region Method of reading a string from a file
        //Cчитываем строку. В skip пишем какую строку нужно получить, в take - сколько строк начиная со skip(n) выводить
        /// <summary>
        /// Reads a specific line in the file
        /// </summary>
        /// <param name="numberString">Line number in the file</param>
        /// <param name="filePath">File name</param>
        /// <returns>Returns the string type string of the specified string</returns>
        private static string _GetString(int numberString, string filePath)
        {
            var result = File.ReadLines(filePath).Skip(numberString - 1).Take(1);

            string newString = null;

            foreach (string str in result)
            {
                newString += str;
            }
            return newString;
        }
        #endregion

        internal static void LoadsConstantData()
        {
            if (File.Exists(_pathConstantFile))
            {
                using (var sr = new StreamReader(_pathConstantFile))
                {
                    δ_X23_X26 = (_ReadingNeedString(2, Properties.Resources.accuracy_X2_3_X2_6));
                    I_X24_X25 = (_ReadingNeedString(5, Properties.Resources.current_X2_4_X2_5));
                    δ_X24_X25 = (_ReadingNeedString(8, Properties.Resources.accuracy_X2_4_X2_5));
                    δ_X15_X16 = (_ReadingNeedString(11, Properties.Resources.accuracy_X1_5_X1_6));
                    I_X27_X28 = (_ReadingNeedString(14, Properties.Resources.current_X2_7_X2_8));
                    δ_X27_X28 = (_ReadingNeedString(17, Properties.Resources.accuracy_X2_7_X2_8));
                    δ_X11_X12 = (_ReadingNeedString(20, Properties.Resources.accuracy__X1_1_X1_2));
                    δ_X21_X22 = (_ReadingNeedString(23, Properties.Resources.accuracy_X2_1_X2_2));
                }
            }//считывает значения из файла
        }
        internal static decimal _ReadingNeedString(int lineNumber, string nameInputData)
        {
            try
            {
                return Convert.ToDecimal(_GetString(lineNumber, Properties.Resources.ConstantsFile));//считывает только определенную строку 
            }
            catch
            {
                MessageBox.Show($"Ошибка чтения файла pntn_const\nзначение константы {nameInputData} некорректное", "Ошибка"); return default;
            }
        }
        internal static void _EnterDefaultConstantsInFile()
        {
            using (var sw = new StreamWriter(_pathConstantFile))
            {
                sw.WriteLine($"{Properties.Resources.accuracy_X2_3_X2_6} = "); sw.WriteLine("1,21224002"); sw.WriteLine("");
                sw.WriteLine($"{Properties.Resources.current_X2_4_X2_5} = "); sw.WriteLine("2,498096032"); sw.WriteLine("");
                sw.WriteLine($"{Properties.Resources.accuracy_X2_4_X2_5} = "); sw.WriteLine("1,248513674"); sw.WriteLine("");
                sw.WriteLine($"{Properties.Resources.accuracy_X1_5_X1_6} = "); sw.WriteLine("1,2120382"); sw.WriteLine("");
                sw.WriteLine($"{Properties.Resources.current_X2_7_X2_8} = "); sw.WriteLine("2,498096032"); sw.WriteLine("");
                sw.WriteLine($"{Properties.Resources.accuracy_X2_7_X2_8} = "); sw.WriteLine("1,248513674"); sw.WriteLine("");
                sw.WriteLine($"{Properties.Resources.accuracy__X1_1_X1_2} = "); sw.WriteLine("9090,04933015"); sw.WriteLine("");
                sw.WriteLine($"{Properties.Resources.accuracy_X2_1_X2_2} = "); sw.WriteLine("30,04933015");
                sw.WriteLine("\n\n");
                sw.WriteLine("------------------------------");
                sw.WriteLine("Важно! Значения констант должны стоять на определенных строках. " +
                             "Для разделения целой и дробной части использовать запятую (,).");
            }
        }
        internal static void _ShowCurrentConstants()
        {
            MessageBox.Show($"{Properties.Resources.accuracy_X2_3_X2_6} = {δ_X23_X26}\n" +
                            $"{Properties.Resources.current_X2_4_X2_5} = {I_X24_X25}   " +
                            $"{Properties.Resources.accuracy_X1_5_X1_6} = {δ_X15_X16}\n" +
                            $"{Properties.Resources.current_X2_7_X2_8} = {I_X27_X28}\n" +
                            $"{Properties.Resources.accuracy_X2_7_X2_8} = {δ_X27_X28}  " +
                            $"{Properties.Resources.accuracy__X1_1_X1_2} = {δ_X11_X12}\n" +
                            $"{Properties.Resources.accuracy_X2_1_X2_2} = {δ_X21_X22}", " Константы");
        }
    }
}
