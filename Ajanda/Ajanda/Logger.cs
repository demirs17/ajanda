using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Ajanda
{
    class Logger
    {
        string Text;
        string Tarih;
        string Saat;
        public Logger(string text, string tarih, string saat)
        {
            Text = text;
            Tarih = tarih;
            Saat = saat;
        }
        public void txtYaz()
        {
            if (Text != null && Saat != null && Tarih != null)
            {
                string dirname = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                File.AppendAllText(dirname + "/logs.txt", Tarih + " - " + Saat + " - " + Text + Environment.NewLine);
            }
        }
        public static void logTxt(string text, string tarih, string saat)
        {
            if (text != null && tarih != null && saat != null)
            {
                string dirname = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                File.AppendAllText(dirname + "/logs.txt", tarih + " - " + saat + " - " + text + Environment.NewLine);
            }
        }
    }
}
