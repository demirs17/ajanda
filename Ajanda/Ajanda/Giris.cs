using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Ajanda
{
    public partial class Giris : Form
    {
        public Giris()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string connStr = "Server=localhost;Database=Ajanda;Trusted_Connection=True";
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            SqlCommand command;SqlDataReader reader;
            string sql = "select * from kullanicilar where kullanici_adi = '" + textBox1.Text + "' and sifre = '" + textBox2.Text + "'";

            command = new SqlCommand(sql, conn);
            reader = command.ExecuteReader();

            int kullaniciId = 0;
            int yetki = 0;

            int i = 0;
            while (reader.Read())
            {
                i++;
                kullaniciId = int.Parse(reader.GetValue(0).ToString());
                yetki = int.Parse(reader.GetValue(3).ToString());
            }
            conn.Close();

            if (i == 1 && kullaniciId != 0)
            {
                Logger.logTxt("Giriş Yapıldı, Kullanıcı ID : " + kullaniciId, BugunTarih(), DateTime.Now.ToString("HH:mm"));

                Form1 form1 = new Form1(yetki, kullaniciId);
                form1.Show();
            }
            else
            {
                Logger.logTxt("Hatalı giriş", BugunTarih(), DateTime.Now.ToString("HH:mm"));
            }

        }

        private string BugunTarih(int gunEkle = 0)
        {
            if (gunEkle == 0)
            {
                DateTime now = DateTime.Now; string g = now.Day.ToString().Length == 1 ? "0" + now.Day : now.Day.ToString(); string a = now.Month.ToString().Length == 1 ? "0" + now.Month : now.Month.ToString(); int y = now.Year;
                return g + "." + a + "." + y;
            }
            else
            {
                DateTime gun = DateTime.Now.Date.AddDays(gunEkle);
                return gun.Day + "." + gun.Month + "." + gun.Year;
            }
        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            Kayit kayitForm = new Kayit();
            kayitForm.ShowDialog();
        }
    }
}
