using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ajanda
{
    class Hatirlatici
    {
        public int Id;
        public string Baslik;
        public int KullaniciId;
        public string Tarih;
        public string Saat;
        public string Kategori;
        public string Aciklama;
        public bool Tamamlandi;
        public int Kime;
        public string KullaniciAdi;
        public int AtayanYetki;
        public Hatirlatici(int id,string baslik,int kullaniciId,string tarih,string saat,string kategori, string aciklama, string tamamlandi, int kime, string kullaniciadi, int atayanyetki)
        {
            Id = id;
            Baslik = baslik;
            KullaniciId = kullaniciId;
            Tarih = tarih;
            Saat = saat;
            Kategori = kategori;
            Aciklama = aciklama;
            Tamamlandi = (tamamlandi == "e" || tamamlandi == "E" ? true : false);
            Kime = kime;
            KullaniciAdi = kullaniciadi;
            AtayanYetki = atayanyetki;
        }
    }
}
