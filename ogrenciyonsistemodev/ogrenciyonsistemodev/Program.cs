using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace OgrenciDersYonetimSistemi
{
    // Base Class
    public abstract class Kisi
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }

        public abstract void BilgiGoster();
    }

    // Interface
    public interface IKayit
    {
        void Kaydet();
        void Yukle();
    }

    // Student Class
    public class Ogrenci : Kisi
    {
        public int OgrenciNo { get; set; }

        public override void BilgiGoster()
        {
            Console.WriteLine($"Ogrenci No: {OgrenciNo}, Ad: {Ad}, Soyad: {Soyad}");
        }
    }

    // Instructor Class
    public class OgretimGorevlisi : Kisi
    {
        public string Departman { get; set; }

        public override void BilgiGoster()
        {
            Console.WriteLine($"Ogretim Gorevlisi: {Ad} {Soyad}, Departman: {Departman}");
        }
    }

    // Course Class
    public class Ders
    {
        public string DersAdi { get; set; }
        public int Kredi { get; set; }
        public OgretimGorevlisi OgretimGorevlisi { get; set; }
        public List<Ogrenci> Ogrenciler { get; set; } = new List<Ogrenci>();

        public void BilgiGoster()
        {
            Console.WriteLine($"Ders: {DersAdi}, Kredi: {Kredi}, Ogretim Gorevlisi: {OgretimGorevlisi.Ad} {OgretimGorevlisi.Soyad}");
            Console.WriteLine("Kayitli Ogrenciler:");
            foreach (var ogrenci in Ogrenciler)
            {
                ogrenci.BilgiGoster();
            }
        }
    }

    // Data Manager for XML Operations
    public static class XMLHelper<T>
    {
        public static void Serialize(string filePath, T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, obj);
            }
        }

        public static T Deserialize(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StreamReader reader = new StreamReader(filePath))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }

    class Program
    {
        static List<Ogrenci> Ogrenciler = new List<Ogrenci>();
        static List<OgretimGorevlisi> OgretimGorevlileri = new List<OgretimGorevlisi>();
        static List<Ders> Dersler = new List<Ders>();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1. Ogrenci Ekle");
                Console.WriteLine("2. Ogretim Gorevlisi Ekle");
                Console.WriteLine("3. Ders Ekle");
                Console.WriteLine("4. Ogrenci Dersi Kayit Et");
                Console.WriteLine("5. Tum Kayitlari Sifirla");
                Console.WriteLine("6. XML Dosyasini Goruntule");
                Console.WriteLine("7. Cikis");

                Console.Write("Seciminizi yapin: ");
                string secim = Console.ReadLine();

                switch (secim)
                {
                    case "1":
                        OgrenciEkle();
                        break;
                    case "2":
                        OgretimGorevlisiEkle();
                        break;
                    case "3":
                        DersEkle();
                        break;
                    case "4":
                        OgrenciDersiKayitEt();
                        break;
                    case "5":
                        TumKayitlariSifirla();
                        break;
                    case "6":
                        Console.WriteLine("Hangi XML dosyasini goruntulemek istersiniz?");
                        Console.WriteLine("1. Ogrenciler.xml");
                        Console.WriteLine("2. OgretimGorevlileri.xml");
                        Console.WriteLine("3. Dersler.xml");
                        string secimXml = Console.ReadLine();
                        if (secimXml == "1")
                        {
                            XMLDosyasiniGoruntule("ogrenciler.xml");
                        }
                        else if (secimXml == "2")
                        {
                            XMLDosyasiniGoruntule("ogretimGorevlileri.xml");
                        }
                        else if (secimXml == "3")
                        {
                            XMLDosyasiniGoruntule("dersler.xml");
                        }
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Gecersiz secim!");
                        break;
                }
            }
        }

        static void OgrenciEkle()
        {
            Console.Write("Ogrenci Adi: ");
            string ad = Console.ReadLine();
            Console.Write("Ogrenci Soyadi: ");
            string soyad = Console.ReadLine();
            Console.Write("Ogrenci Numarasi: ");
            int no = int.Parse(Console.ReadLine());

            Ogrenci ogrenci = new Ogrenci { Ad = ad, Soyad = soyad, OgrenciNo = no };
            Ogrenciler.Add(ogrenci);

            XMLHelper<List<Ogrenci>>.Serialize("ogrenciler.xml", Ogrenciler);
        }

        static void OgretimGorevlisiEkle()
        {
            Console.Write("Ogretim Gorevlisi Adi: ");
            string ad = Console.ReadLine();
            Console.Write("Ogretim Gorevlisi Soyadi: ");
            string soyad = Console.ReadLine();
            Console.Write("Departman: ");
            string departman = Console.ReadLine();

            OgretimGorevlisi gorevli = new OgretimGorevlisi { Ad = ad, Soyad = soyad, Departman = departman };
            OgretimGorevlileri.Add(gorevli);

            XMLHelper<List<OgretimGorevlisi>>.Serialize("ogretimGorevlileri.xml", OgretimGorevlileri);
        }

        static void DersEkle()
        {
            Console.Write("Ders Adi: ");
            string dersAdi = Console.ReadLine();
            Console.Write("Ders Kredisi: ");
            int kredi = int.Parse(Console.ReadLine());

            Console.WriteLine("Ogretim Gorevlisini Secin:");
            for (int i = 0; i < OgretimGorevlileri.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {OgretimGorevlileri[i].Ad} {OgretimGorevlileri[i].Soyad}");
            }

            int secim = int.Parse(Console.ReadLine()) - 1;
            OgretimGorevlisi secilenGorevli = OgretimGorevlileri[secim];

            Ders ders = new Ders { DersAdi = dersAdi, Kredi = kredi, OgretimGorevlisi = secilenGorevli };
            Dersler.Add(ders);

            XMLHelper<List<Ders>>.Serialize("dersler.xml", Dersler);
        }

        static void OgrenciDersiKayitEt()
        {
            Console.WriteLine("Ogrenciyi Secin:");
            for (int i = 0; i < Ogrenciler.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Ogrenciler[i].Ad} {Ogrenciler[i].Soyad}");
            }

            int ogrenciSecim = int.Parse(Console.ReadLine()) - 1;
            Ogrenci secilenOgrenci = Ogrenciler[ogrenciSecim];

            Console.WriteLine("Dersi Secin:");
            for (int i = 0; i < Dersler.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Dersler[i].DersAdi}");
            }

            int dersSecim = int.Parse(Console.ReadLine()) - 1;
            Ders secilenDers = Dersler[dersSecim];

            secilenDers.Ogrenciler.Add(secilenOgrenci);

            XMLHelper<List<Ders>>.Serialize("dersler.xml", Dersler);
        }

        // XML Dosyasını Görüntülemek
        static void XMLDosyasiniGoruntule(string dosyaAdi)
        {
            if (File.Exists(dosyaAdi))
            {
                string xmlIcerik = File.ReadAllText(dosyaAdi);
                Console.WriteLine("--- XML Dosyası İçeriği ---");
                Console.WriteLine(xmlIcerik);
            }
            else
            {
                Console.WriteLine("Dosya bulunamadi.");
            }
        }

        // Tüm Kayıtları Sıfırlama
        static void TumKayitlariSifirla()
        {
            Ogrenciler.Clear();
            OgretimGorevlileri.Clear();
            Dersler.Clear();

            // XML dosyalarını sıfırlamak
            File.Delete("ogrenciler.xml");
            File.Delete("ogretimGorevlileri.xml");
            File.Delete("dersler.xml");

            Console.WriteLine("Tüm kayıtlar sıfırlandı ve XML dosyaları temizlendi.");
        }
    }
}
