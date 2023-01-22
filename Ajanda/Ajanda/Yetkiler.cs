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
    public partial class Yetkiler : Form
    {
        public Yetkiler()
        {
            InitializeComponent();
        }

        List<int> IDler = new List<int>();
        List<TextBox> KullaniciAdiInputlar = new List<TextBox>();
        List<TextBox> YetkilerInputlar = new List<TextBox>();
        private void Yetkiler_Load(object sender, EventArgs e)
        {
            SqlDataReader reader = Sorgu.select("select * from kullanicilar");
            int i = 0;
            while (reader.Read())
            {
                TableLayoutPanel tbl = new TableLayoutPanel();
                tbl.Width = 500;
                tbl.Height = 30;
                tbl.BackColor = Color.White;
                tbl.ForeColor = Color.Black;
                tbl.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
                tbl.RowCount = 1;

                IDler.Add(int.Parse(reader.GetValue(0).ToString()));
                tbl.Controls.Add(new Label { Text = reader.GetValue(0).ToString(), Width = 50 }, 0, 0);

                KullaniciAdiInputlar.Add(new TextBox { Text = reader.GetValue(1).ToString() });
                tbl.Controls.Add(KullaniciAdiInputlar[i], 1, 0);

                YetkilerInputlar.Add(new TextBox { Text = reader.GetValue(3).ToString() });
                tbl.Controls.Add(YetkilerInputlar[i], 2, 0);

                Button btn = new Button();
                btn.Tag = i.ToString();
                btn.Text = "Kaydet";
                btn.Click += new EventHandler(kaydetButtonClick);
                tbl.Controls.Add(btn, 3, 0);

                flowLayoutPanel1.Controls.Add(tbl);

                i++;
            }
        }
        void kaydetButtonClick(object sender, EventArgs e)
        {
            var kaydetButton = sender as Button;

            int i = 0;
            string sql = "update kullanicilar set kullanici_adi = '" + KullaniciAdiInputlar[int.Parse(kaydetButton.Tag.ToString())].Text + "' where id = " + IDler[int.Parse(kaydetButton.Tag.ToString())];
            SqlCommand command = Sorgu.query(sql);
            if (command.ExecuteNonQuery() == 0)
            {
                MessageBox.Show("kullanıcı adı güncellenemedi");
                i++;
            }

            sql = "update kullanicilar set yetki = '" + YetkilerInputlar[int.Parse(kaydetButton.Tag.ToString())].Text + "' where id = " + IDler[int.Parse(kaydetButton.Tag.ToString())];
            command = Sorgu.query(sql);
            if (command.ExecuteNonQuery() == 0)
            {
                MessageBox.Show("yetki güncellenemedi");
                i++;
            }

            if (i == 0)
            {
                MessageBox.Show("Kullanıcı Adı ve yetki güncellendi");
            }

            Sorgu.conn.Close();
        }
    }
}
