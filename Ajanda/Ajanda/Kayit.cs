using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ajanda
{
    public partial class Kayit : Form
    {
        public Kayit()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> kullanici_adlari = new List<string>();
            SqlDataReader reader = Sorgu.select("select kullanici_adi from kullanicilar");
            while (reader.Read())
            {
                kullanici_adlari.Add(reader.GetValue(0).ToString());
            }
            if (kullanici_adlari.Contains(textBox1.Text))
            {
                MessageBox.Show("Kullanıcı Adı Daha Önce Alınmış");
            }
            else
            {
                SqlCommand command = Sorgu.query("insert into kullanicilar(kullanici_adi, sifre, yetki) values('" + textBox1.Text + "','" + textBox2.Text + "', 9000)");

                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Kayıt Yapıldı");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Kayıt Yapılamadı");
                }
            }
            Sorgu.conn.Close();
        }
    }
}
