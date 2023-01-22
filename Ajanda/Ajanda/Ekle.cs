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
    public partial class Ekle : Form
    {
        bool Duzenle;
        int KullaniciId;
        string SeciliTarih;
        int GorevId;
        private Form1 MainForm = null;
        public Ekle(Form frm1 , int kullaniciId, string seciliTarih, bool duzenle = false, int gorevid = 0)
        {
            MainForm = frm1 as Form1;
            Duzenle = duzenle;
            KullaniciId = kullaniciId;
            SeciliTarih = seciliTarih;
            GorevId = gorevid;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Duzenle) // update
            {
                int kime_id = KullaniciId;
                SqlDataReader reader = Sorgu.select("select id from kullanicilar where kullanici_adi = '"+ textBox6.Text +"'");
                while (reader.Read()){ kime_id = int.Parse(reader.GetValue(0).ToString()); }

                MessageBox.Show(kime_id.ToString());

                string sql = "update gorevler set baslik = '" + textBox1.Text + "',kategori = '" + textBox3.Text + "' ,tarih = '" + textBox4.Text + "',aciklama = '" + textBox2.Text + "',saat = '" + textBox5.Text + "', kime = " + kime_id + " where id = " + GorevId;
                SqlCommand command = Sorgu.query(sql);

                if (command.ExecuteNonQuery() == 1)
                {
                    DateTime now = DateTime.Now; string g = now.Day.ToString().Length == 1 ? "0" + now.Day : now.Day.ToString(); string a = now.Month.ToString().Length == 1 ? "0" + now.Month : now.Month.ToString(); int y = now.Year;
                    Logger logger = new Logger("Düzenlendi, Görev ID : " + GorevId, g + "." + a + "." + y, DateTime.Now.ToString("HH:mm"));
                    logger.txtYaz();
                }
                else
                {
                    MessageBox.Show("Kaydedilemedi");
                }
                Sorgu.conn.Close();

                MainForm.Listele(MainForm.GunuYukle(SeciliTarih));
                MainForm.BildirimYenile();

                this.Close();
            }
            else // insert
            {
                SqlDataReader reader = Sorgu.select("select id from kullanicilar where kullanici_adi = '"+ textBox6.Text +"'");
                int kime_id = KullaniciId;
                while (reader.Read())
                {
                    kime_id = int.Parse(reader.GetValue(0).ToString());
                }

                string sql = "insert into gorevler(baslik, kullanici_id, tarih, saat, kategori, aciklama, tamamlandi, kime) values('" + textBox1.Text + "'," + KullaniciId + " ,'" + textBox4.Text + "','" + textBox5.Text + "','" + textBox3.Text + "','" + textBox2.Text + "','h', " + kime_id + ")";
                SqlCommand command = Sorgu.query(sql);

                if (command.ExecuteNonQuery() == 1)
                {
                    DateTime now = DateTime.Now; string g = now.Day.ToString().Length == 1 ? "0" + now.Day : now.Day.ToString(); string a = now.Month.ToString().Length == 1 ? "0" + now.Month : now.Month.ToString(); int y = now.Year;
                    Logger logger = new Logger("Yeni Hatırlatıcı oluşturuldu, Başlık: " + textBox1.Text, g + "." + a + "." + y, DateTime.Now.ToString("HH:mm"));
                    logger.txtYaz();
                }
                else
                {
                    MessageBox.Show("Hata Oluştu");
                }

                Sorgu.conn.Close();

                MainForm.Listele(MainForm.GunuYukle(SeciliTarih));
                MainForm.BildirimYenile();

                this.Close();
            }
        }

        private void Ekle_Load(object sender, EventArgs e)
        {
            if (Duzenle)
            {
                button1.Text = "Kaydet";

                string sql = "select * from gorevler where id = " + GorevId;
                SqlDataReader reader = Sorgu.select(sql);

                int i = 0;int kime_id = 0;
                while (reader.Read())
                {
                    i++;
                    textBox1.Text = reader.GetValue(1).ToString();
                    textBox2.Text = reader.GetValue(6).ToString();
                    textBox3.Text = reader.GetValue(5).ToString();
                    textBox4.Text = reader.GetValue(3).ToString();
                    textBox5.Text = reader.GetValue(4).ToString();
                    kime_id = int.Parse(reader.GetValue(8).ToString());
                    // id baslik kullanici_id tarih saat kategori aciklama tamamlandi kime
                }

                reader = Sorgu.select("select kullanici_adi from kullanicilar where id = " + kime_id);
                while (reader.Read()) { textBox6.Text = reader.GetValue(0).ToString(); }

                Sorgu.conn.Close();
            }
            else
            {
                textBox4.Text = SeciliTarih;
            }
        }

        private void Ekle_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}