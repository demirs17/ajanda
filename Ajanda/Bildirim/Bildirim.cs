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
using Microsoft.Toolkit.Uwp.Notifications;

namespace Bildirim
{
    public partial class Bildirim : Form
    {
        public Bildirim()
        {
            InitializeComponent();
        }

        private void Bildirim_Load(object sender, EventArgs e)
        {
            List<Hatirlatici> hatirlaticilar = new List<Hatirlatici>();

            DateTime now = DateTime.Now; int g = now.Day; int a = now.Month; int y = now.Year;

            string connStr = "Server=localhost;Database=Ajanda;Trusted_Connection=True";
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand command;
            SqlDataReader reader;
            String sql;
            //sql = "select * from gorevler where tarih = '"+ tarih +"' and kullanici_id = " + KullaniciId;
            sql = "select * from gorevler where tarih = '" + g + "." + a + "." + y + "'";
            command = new SqlCommand(sql, conn);
            reader = command.ExecuteReader();
            int i = 0;
            while (reader.Read())
            {
                i++;
                hatirlaticilar.Add(new Hatirlatici(id: int.Parse(reader.GetValue(0).ToString()), baslik: reader.GetValue(1).ToString(), saat: reader.GetValue(4).ToString(), tamamlandi: reader.GetValue(7).ToString()));
                // id baslik kullanici_id tarih saat kategori aciklama tamamlandi
            }
            conn.Close();

            // MessageBox.Show(DateTime.Now.ToString("HH:mm"));


            SetInterval(hatirlaticilar);

            for (var j = 0;j<hatirlaticilar.Count;j++)
            {
                flowLayoutPanel1.Controls.Add(new Label { Text = hatirlaticilar[j].ToString(), Width = 350 });
            }
        }
        async void SetInterval(List<Hatirlatici> array)
        {
            for (var i = 0;i<array.Count;i++)
            {
                if (array[i].Saat == DateTime.Now.ToString("HH:mm") && array[i].Tamamlandi == false)
                {
                    new ToastContentBuilder()
                        .AddArgument("action", "viewConversation")
                        .AddArgument("conversationId", 9813)
                        .AddText(array[i].Baslik)
                        .AddText(array[i].Saat + " - " + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year)
                        .Show();

                    array[i].Tamamlandi = true;

                    SetInterval(array);
                }
            }
            //flowLayoutPanel1.Controls.Add(new Label { Text = "lable" });
            await Task.Delay(1000);
            SetInterval(array);




            //new ToastContentBuilder()
            //    .AddArgument("action", "viewConversation")
            //    .AddArgument("conversationId", 9813)
            //    .AddText("Andrew sent you a picture")
            //    .AddText("Check this out, The Enchantments in Washington!")
            //    .Show();
            //SetInterval();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
