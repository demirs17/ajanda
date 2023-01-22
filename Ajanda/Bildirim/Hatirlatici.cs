using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bildirim
{
    class Hatirlatici
    {
        public int Id;
        public string Baslik;
        //public int KullaniciId;
        public string Tarih;
        public string Saat;
        //public string Kategori;
        //public string Aciklama;
        public bool Tamamlandi;
        public Hatirlatici(int id, string baslik, /*int kullaniciId,*/ /*string tarih,*/ string saat, /*string kategori,*/ /*string aciklama,*/ string tamamlandi)
        {
            Id = id;
            Baslik = baslik;
            //KullaniciId = kullaniciId;
            //Tarih = tarih;
            Saat = saat;
            //Kategori = kategori;
            //Aciklama = aciklama;
            Tamamlandi = (tamamlandi == "e" || tamamlandi == "E" ? true : false);
        }
    }
}