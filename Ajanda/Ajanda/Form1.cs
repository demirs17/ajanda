using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ajanda
{
    public partial class Form1 : Form
    {
        int KullaniciId;
        int Yetki;
        string SeciliTarih;
        string bildirimYol = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../Bildirim/bin/Debug/");
        Process bildirim = new Process();
        public Form1(int yetki, int kullanici_id = 0)
        {
            KullaniciId = kullanici_id;
            Yetki = yetki;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("Bildirim")){process.Kill();}
            BildirimYenile();

            label1.Text = BugunTarih();
            SeciliTarih = BugunTarih();
            button1.Text = BugunTarih(-1);
            button2.Text = BugunTarih(1);
            textBox1.Text = BugunTarih();

            if (Yetki != 0){button5.Enabled = false;}


            List<List<Hatirlatici>> list = GunuYukle(BugunTarih());
            Listele(list);
        }
        internal List<List<Hatirlatici>> GunuYukle(string tarih)
        {
            List<Hatirlatici> hatirlaticilar = new List<Hatirlatici>();
            List<Hatirlatici> atadiklarim = new List<Hatirlatici>();

            //SqlDataReader reader = Sorgu.select("select * from gorevler where tarih = '" + tarih + "' and kime = " + KullaniciId);
            SqlDataReader reader = Sorgu.select("select * from gorevler, kullanicilar where tarih = '" + tarih + "' and kime = " + KullaniciId + "and gorevler.kullanici_id = kullanicilar.id");

            int i = 0;
            while (reader.Read())
            {
                i++;
                hatirlaticilar.Add(new Hatirlatici(int.Parse(reader.GetValue(0).ToString()), reader.GetValue(1).ToString(), int.Parse(reader.GetValue(2).ToString()), reader.GetValue(3).ToString(), reader.GetValue(4).ToString(), reader.GetValue(5).ToString(), reader.GetValue(6).ToString(), reader.GetValue(7).ToString(), int.Parse(reader.GetValue(8).ToString()), reader.GetValue(10).ToString(), int.Parse(reader.GetValue(12).ToString())));
                // id baslik kullanici_id(kimden) tarih saat kategori aciklama tamamlnadi kime
            }


            reader = Sorgu.select("select * from gorevler, kullanicilar where tarih = '" + tarih + "' and kullanici_id = " + KullaniciId + " and kime != kullanici_id and gorevler.kime = kullanicilar.id");
            //reader = Sorgu.select("select * from gorevler");

            int j = 0;
            while (reader.Read())
            {
                j++;
                atadiklarim.Add(new Hatirlatici(int.Parse(reader.GetValue(0).ToString()), reader.GetValue(1).ToString(), int.Parse(reader.GetValue(2).ToString()), reader.GetValue(3).ToString(), reader.GetValue(4).ToString(), reader.GetValue(5).ToString(), reader.GetValue(6).ToString(), reader.GetValue(7).ToString(), int.Parse(reader.GetValue(8).ToString()), reader.GetValue(10).ToString(), int.Parse(reader.GetValue(12).ToString())));
                // id baslik kullanici_id(kimden) tarih saat kategori aciklama tamamlnadi kime
            }
            Sorgu.conn.Close();

            return new List<List<Hatirlatici>>() { hatirlaticilar, atadiklarim };
        }
        List<TableLayoutPanel> tbls = new List<TableLayoutPanel>();
        internal void Listele(List<List<Hatirlatici>> list)
        {
            List<Hatirlatici> hatirlaticilar = list[0];
            List<Hatirlatici> atadiklarim = list[1];
            flowLayoutPanel1.Controls.Clear();

            if (hatirlaticilar.Count != 0)
            {
                flowLayoutPanel1.Controls.Add(new Label { Name = "label2" , Text = "Görevlerim", Width = flowLayoutPanel1.Width - 15, AutoSize = false, TextAlign = ContentAlignment.MiddleCenter});

                TableLayoutPanel tblBaslik = new TableLayoutPanel();
                tblBaslik.Width = flowLayoutPanel1.Width - 30;
                tblBaslik.Height = 25;
                tblBaslik.BackColor = Color.DarkGray;
                tblBaslik.ForeColor = Color.White;
                tblBaslik.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                tblBaslik.RowCount = 1;
                tbls.Add(tblBaslik);

                tblBaslik.Controls.Add(new Label { Text = "Saat", Width = 60, Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular) }, 0, 0);
                tblBaslik.Controls.Add(new Label { Text = "Baslik", Width = 280, Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular) }, 1, 0);
                tblBaslik.Controls.Add(new Label { Text = "Kategori", Width = 85, Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular) }, 2, 0);
                tblBaslik.Controls.Add(new Label { Text = "Atayan", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular) }, 3, 0);
                tblBaslik.Controls.Add(new Label { Text = "", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular) }, 4, 0);
                tblBaslik.Controls.Add(new Label { Text = "", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular) }, 5, 0);
                tblBaslik.Controls.Add(new Label { Text = "", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular) }, 6, 0);

                flowLayoutPanel1.Controls.Add(tblBaslik);
            }
            for (int i = 0; i < hatirlaticilar.Count; i++)
            {
                TableLayoutPanel tbl = new TableLayoutPanel();
                tbl.Width = flowLayoutPanel1.Width - 25;
                tbl.Height = 50;
                tbl.BackColor = Color.White;
                tbl.ForeColor = Color.Black;
                tbl.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
                tbl.RowCount = 1;
                tbls.Add(tbl);


                bool chckd = false;
                if (hatirlaticilar[i].Tamamlandi) { chckd = true; }

                tbl.Controls.Add(new Label { Text = hatirlaticilar[i].Saat, Width = 60, Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 13, FontStyle.Bold) }, 0, 0);
                tbl.Controls.Add(new Label { Text = hatirlaticilar[i].Baslik, Width = 280, Padding = new Padding(20, 0,0,0) , Anchor = AnchorStyles.Left, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular) }, 1, 0);
                tbl.Controls.Add(new Label { Text = hatirlaticilar[i].Kategori, Width = 85, Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular) }, 2, 0);
                tbl.Controls.Add(new Label { Text = hatirlaticilar[i].KullaniciAdi,Height = 40, Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular) }, 3, 0);

                CheckBox tamam = new CheckBox();
                tamam.Text="";tamam.Anchor=AnchorStyles.None;tamam.Checked=chckd;tamam.Width=25;tamam.CheckAlign = ContentAlignment.MiddleCenter;tamam.TabStop=false;
                tamam.Tag = hatirlaticilar[i].Id;
                tamam.Click += new EventHandler(tamamButtonClick);
                tbl.Controls.Add(tamam, 4, 0);

                if (hatirlaticilar[i].AtayanYetki >= Yetki)
                {
                    Button duzenleBtn = new Button();
                    duzenleBtn.Text = "Düzenle"; duzenleBtn.Width = 70; duzenleBtn.Height = 25;duzenleBtn.Anchor = AnchorStyles.None; duzenleBtn.TabStop = false; duzenleBtn.BackColor = Color.White;
                    duzenleBtn.Tag = hatirlaticilar[i].Id;
                    duzenleBtn.Click += new EventHandler(duzenleButtonClick);
                    tbl.Controls.Add(duzenleBtn, 5, 0);

                    Button silBtn = new Button();
                    silBtn.Name = "silbtn" + i; silBtn.Text = "Sil";silBtn.Width = 70; silBtn.Height = 25;silBtn.Anchor = AnchorStyles.None; silBtn.TabStop = false; silBtn.BackColor = Color.White;
                    silBtn.Tag = hatirlaticilar[i].Id;
                    silBtn.Click += new EventHandler(silButtonClick);
                    tbl.Controls.Add(silBtn, 6, 0);
                }
                else
                {
                    tbl.Controls.Add(new Label { }, 5, 0); tbl.Controls.Add(new Label { }, 6, 0);
                }

                flowLayoutPanel1.Controls.Add(tbl);
            }
            
            if (atadiklarim.Count != 0)
            {
                flowLayoutPanel1.Controls.Add(new Label { Name = "label3", Text = "Atadığım Görevler", Width = flowLayoutPanel1.Width - 15, AutoSize = false, TextAlign = ContentAlignment.MiddleCenter });

                TableLayoutPanel tblBaslik2 = new TableLayoutPanel();
                tblBaslik2.Width = flowLayoutPanel1.Width - 30;
                tblBaslik2.Height = 25;
                tblBaslik2.BackColor = Color.DarkGray;
                tblBaslik2.ForeColor = Color.White;
                tblBaslik2.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                tblBaslik2.RowCount = 1;
                tbls.Add(tblBaslik2);

                tblBaslik2.Controls.Add(new Label { Text = "Saat", Width = 60, Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular) }, 0, 0);
                tblBaslik2.Controls.Add(new Label { Text = "Baslik", Width = 280, Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular) }, 1, 0);
                tblBaslik2.Controls.Add(new Label { Text = "Kategori", Width = 85, Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular) }, 2, 0);
                tblBaslik2.Controls.Add(new Label { Text = "Atanan", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular) }, 3, 0);
                tblBaslik2.Controls.Add(new Label { Text = "", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular) }, 4, 0);
                tblBaslik2.Controls.Add(new Label { Text = "", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular) }, 5, 0);
                tblBaslik2.Controls.Add(new Label { Text = "", Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular) }, 6, 0);

                flowLayoutPanel1.Controls.Add(tblBaslik2);
            }
            for (int i = 0;i<atadiklarim.Count;i++)
            {
                TableLayoutPanel tbl = new TableLayoutPanel();
                tbl.Width = flowLayoutPanel1.Width - 25;
                tbl.Height = 50;
                tbl.BackColor = Color.White;
                tbl.ForeColor = Color.Black;
                tbl.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
                tbl.RowCount = 1;
                tbls.Add(tbl);


                bool chckd = false;
                if (atadiklarim[i].Tamamlandi) { chckd = true; }

                tbl.Controls.Add(new Label { Text = atadiklarim[i].Saat, Width = 60, Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 13, FontStyle.Bold) }, 0, 0);
                tbl.Controls.Add(new Label { Text = atadiklarim[i].Baslik, Width = 280, Padding = new Padding(20, 0, 0, 0), Anchor = AnchorStyles.Left, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular) }, 1, 0);
                tbl.Controls.Add(new Label { Text = atadiklarim[i].Kategori, Width = 85, Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular) }, 2, 0);
                tbl.Controls.Add(new Label { Text = atadiklarim[i].KullaniciAdi, Anchor = AnchorStyles.None, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular) }, 3, 0);

                // tbl.Controls.Add(new CheckBox { Text = "", Anchor = AnchorStyles.None, Checked = chckd, Width = 25, CheckAlign = ContentAlignment.MiddleCenter, TabStop = false }, 3, 0);
                CheckBox tamam = new CheckBox();
                tamam.Text = ""; tamam.Anchor = AnchorStyles.None; tamam.Checked = chckd; tamam.Width = 25; tamam.CheckAlign = ContentAlignment.MiddleCenter; tamam.TabStop = false;
                tamam.Tag = atadiklarim[i].Id;
                tamam.Click += new EventHandler(tamamButtonClick);
                tbl.Controls.Add(tamam, 4, 0);

                Button duzenleBtn = new Button();
                duzenleBtn.Text = "Düzenle"; duzenleBtn.Width = 70; duzenleBtn.Height = 25; duzenleBtn.Anchor = AnchorStyles.None; duzenleBtn.TabStop = false; duzenleBtn.BackColor = Color.White;
                duzenleBtn.Tag = atadiklarim[i].Id;
                duzenleBtn.Click += new EventHandler(duzenleButtonClick);
                tbl.Controls.Add(duzenleBtn, 5, 0);
                //tbl.Controls.Add(new Button { Text = "Düzenle", Width=70, Height=25, Anchor = AnchorStyles.None, TabStop=false, BackColor = Color.White }, 4, 0);

                Button silBtn = new Button();
                silBtn.Name = "silbtn" + i; silBtn.Text = "Sil"; silBtn.Width = 70; silBtn.Height = 25; silBtn.Anchor = AnchorStyles.None; silBtn.TabStop = false; silBtn.BackColor = Color.White;
                silBtn.Tag = atadiklarim[i].Id;
                silBtn.Click += new EventHandler(silButtonClick);
                tbl.Controls.Add(silBtn, 6, 0);
                //tbl.Controls.Add(new Button { Text = "Sil",Tag = hatirlaticilar[i].ID, Width=70, Height=25, Anchor = AnchorStyles.None, TabStop=false, BackColor = Color.White }, 5, 0);

                flowLayoutPanel1.Controls.Add(tbl);
            }
        }
        void tamamButtonClick(object sender, EventArgs e)
        {
            var tamamButton = sender as CheckBox;

            string sql = "";
            if (tamamButton.Checked)
            {sql = "update gorevler set tamamlandi = 'e' where id = " + tamamButton.Tag;}
            else
            {sql = "update gorevler set tamamlandi = 'h' where id = " + tamamButton.Tag;}

            SqlCommand command = Sorgu.query(sql);
            if (command.ExecuteNonQuery() == 1) { Logger.logTxt("Hatırlatıcı Tamamlandı, id: "+ tamamButton.Tag, BugunTarih(), DateTime.Now.ToString("HH:mm")); }
            else { MessageBox.Show("kaydedilemedi"); }

            Sorgu.conn.Close();
        }
        void duzenleButtonClick(object sender, EventArgs e)
        {
            var dznlbtn = sender as Button;
            Ekle duzenleForm = new Ekle(this, KullaniciId, SeciliTarih, duzenle: true, gorevid: int.Parse(dznlbtn.Tag.ToString()));
            duzenleForm.Show();

            BildirimYenile();
        }
        void silButtonClick(object sender, EventArgs e)
        {
            var btn = sender as Button;

            string sql = "delete from gorevler where id = " + btn.Tag;
            SqlCommand command = Sorgu.query(sql);

            if (command.ExecuteNonQuery() == 1)
            {
                Logger.logTxt("Hatırlatıcı silindi, Görev ID: " + btn.Tag, BugunTarih(), DateTime.Now.ToString("HH:mm"));
            } else {
                MessageBox.Show("Silinemedi"); 
            }
            Sorgu.conn.Close();

            List<List<Hatirlatici>> list = GunuYukle(SeciliTarih);
            Listele(list);
            BildirimYenile();
        }
        internal void BildirimYenile()
        {
            try
            { // process var
                int prcsid = bildirim.Id;
                if (bildirim.Id < 0)
                {
                    MessageBox.Show(bildirim.Id.ToString());
                }
                bildirim.Kill();
                bildirim.Start();
            }
            catch
            { // process yok
                bildirim.StartInfo = new ProcessStartInfo(bildirimYol + "Bildirim.exe");
                bildirim.Start();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Ekle ekleForm = new Ekle(this, kullaniciId: KullaniciId, seciliTarih: SeciliTarih, duzenle: false);
            ekleForm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string tarih = textBox1.Text;
            SeciliTarih = textBox1.Text;
            List<List<Hatirlatici>> list = GunuYukle(tarih);
            Listele(list);
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            foreach (var table in tbls)
            {
                table.Width = flowLayoutPanel1.Width - 25;
            }
            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                if (ctrl.Name == "label2" || ctrl.Name == "label3")
                {
                    ctrl.Width = flowLayoutPanel1.Width - 25;
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Listele(GunuYukle(BugunTarih()));
            textBox1.Text = BugunTarih();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Listele(GunuYukle(BugunTarih(-1)));
            textBox1.Text = BugunTarih(-1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Listele(GunuYukle(BugunTarih(1)));
            textBox1.Text = BugunTarih(1);
        }

        private string BugunTarih(int gunEkle = 0)
        {
            if (gunEkle == 0)
            {
                DateTime now = DateTime.Now; int g = now.Day; int a = now.Month; int y = now.Year;
                return g + "." + a + "." + y;
            }
            else
            {
                DateTime gun = DateTime.Now.Date.AddDays(gunEkle);
                return gun.Day + "." + gun.Month + "." + gun.Year;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Yetkiler yetkiler = new Yetkiler();
            yetkiler.ShowDialog();
        }
    }
}
