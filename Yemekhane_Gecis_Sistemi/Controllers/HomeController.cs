using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Yemekhane_Gecis_Sistemi.Models;
using Yemekhane_Gecis_Sistemi.ViewModels;
namespace Yemekhane_Gecis_Sistemi.Controllers
{
    public class HomeController : Controller
    {
        DB db = new DB();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
        //[HttpGet]
        public ActionResult KullaniciEkle()
        {                    
            KartKullanicilari model = GetData();
            model.Baslik = "Kayıt Ekleme";
            return View(model);
        }
        [HttpPost]
        public ActionResult KullaniciEkle(KartKullanicilari model)
        {
            //DB db = new DB();
            kullanicilar kullanicilarModel = new kullanicilar();
            kart_bilgileri kartBilgileriModel = new kart_bilgileri();

            kullanicilarModel.tc = model.TcKimlikNo;
            kullanicilarModel.email = model.EMail;
            kullanicilarModel.ad = model.Ad;
            kullanicilarModel.soyad = model.Soyad;
            kullanicilarModel.kullanici_adi = model.EMail;
            kullanicilarModel.sifre = model.Sifre;           
            kullanicilarModel.kayit_tarihi = DateTime.Now;
            kullanicilarModel.guncelleme_tarihi = DateTime.Now;
            db.kullanicilar.Add(kullanicilarModel);
            db.SaveChanges();

            kartBilgileriModel.kullanici_id = (from a in db.kullanicilar where a.tc == model.TcKimlikNo select a.kullanici_id).FirstOrDefault();
            kartBilgileriModel.kart_tipi_id = model.KartTipiId;
            kartBilgileriModel.kart_no = model.KartNo;
            kartBilgileriModel.bakiye = "0";
            db.kart_bilgileri.Add(kartBilgileriModel);
            db.SaveChanges();

            model = GetData();
            model.KullaniciMesaji = "Kayıt Başarıyla Gerçekleşti";
           
            return View(model);
        }

        public ActionResult BirimEkle()
        {
            Birimler model = new Birimler();
            model.Baslik = "Birim Ekleme";
            return View(model);
        }

        [HttpPost]
        public ActionResult BirimEkle(Birimler model)
        {
            //DB db = new DB();
            birimler birimlerModel = new birimler();
            birimlerModel.birim_adi = model.BirimAdi;
            db.birimler.Add(birimlerModel);
            db.SaveChanges();
            model.KullaniciMesaji = "Birim Başarıyla Eklendi";
            return View(model);
        }

        public ActionResult BirimListesi()
        {
            var model = db.birimler.ToList();
            return View(model);
        }

        public ActionResult UnvanEkle()
        {
            Unvanlar model = new Unvanlar();
            model.Baslik = "Birim Ekleme";
            return View(model);
        }

        [HttpPost]
        public ActionResult UnvanEkle(Unvanlar model)
        {
            //DB db = new DB();
            unvanlar unvanlarModel = new unvanlar();
            unvanlarModel.unvan_adi = model.UnvanAdi;
            db.unvanlar.Add(unvanlarModel);
            db.SaveChanges();
            model.KullaniciMesaji = "Unvan Başarıyla Eklendi";
            return View(model);
        }

        public ActionResult UnvanListesi()
        {
            var model = db.unvanlar.ToList();
            return View(model);

        }

        public ActionResult IslemLog()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IslemLog(kullanicilar model)
        {
            var kullanici_id = (from a in db.kullanicilar where a.tc == model.tc select a.kullanici_id).FirstOrDefault();
            var gecis_loglari_model = (from a in db.gecis_loglari where a.kullanici_id == kullanici_id select a).ToList();

            
            return View(gecis_loglari_model);
        }
        public ActionResult KullaniciListele()
        {
            //DB db = new DB();
            var model = db.kart_bilgileri.ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult KullaniciListele(string model)
        {
            return View(model);
        }

        public ActionResult KullaniciGetir(int id)
            
        {
            //var model = db.kullanicilar.Find(id);

            var model = (from kb in db.kart_bilgileri where kb.kullanici_id == id select kb).FirstOrDefault();

            //var model = (from kb in db.kart_bilgileri
            //             join kl in db.kullanicilar on kb.kullanici_id equals kl.kullanici_id
            //             join kt in db.kart_tipleri on kb.kart_tipi_id equals kt.kart_tipi_id
            //             where kl.kullanici_id == id
            //             select new { kb, kl, kt }
            //           ).FirstOrDefault();
            var model2 = (from kb in db.kart_bilgileri where kb.kullanici_id==id select kb).FirstOrDefault();
            List<SelectListItem> kartListesi = (from a in db.kart_tipleri select new SelectListItem 
                                                { 
                                               Text = a.kart_tipi,
                                               Value=a.kart_tipi_id.ToString(),
                                               Selected=(a.kart_tipi_id== model2.kart_tipi_id)
                                               
                                                }).ToList();
            ViewBag.secim = model2.kart_tipi_id;
            ViewBag.kartList = kartListesi;
            
            
            return View("KullaniciGetir",model);
        }

        public ActionResult Guncelle(kart_bilgileri a)
        {
            
            //DB db = new DB();
            var kullanicilar = db.kullanicilar.Find(a.kullanici_id);
            var kartlar = (from kt in db.kart_bilgileri where kt.kullanici_id == a.kullanici_id select kt).FirstOrDefault();
            kullanicilar.ad = a.kullanicilar.ad;
            kullanicilar.soyad=a.kullanicilar.soyad.Trim();
            kullanicilar.email = a.kullanicilar.email.Trim();
            kullanicilar.tc = a.kullanicilar.tc.Trim();
            kullanicilar.kullanici_adi = a.kullanicilar.email.Trim();
            kartlar.kart_tipi_id =a.kart_tipi_id;
            kartlar.kart_no = a.kart_no.Trim();
            

            db.SaveChanges();

            return RedirectToAction("KullaniciListele");
        }
       

        public ActionResult BakiyeYukle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BakiyeYukle(kart_bilgileri a)
        {
            
            kart_bilgileri model = db.kart_bilgileri.Where(x => x.kullanicilar.tc.Equals(a.kullanicilar.tc)).FirstOrDefault();
            if (a.bakiye != null)
            {
                model.bakiye = (Convert.ToDouble(model.bakiye)+Convert.ToDouble(a.bakiye)).ToString();
                db.SaveChanges();
                return Redirect("KullaniciListele");
            }
            return View(model);
        }

        
                   
        public ActionResult KisiGecisRaporu()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KisiGecisRaporu(string model)
        {
            return View(model);
        }
        public ActionResult GunlukGecisRaporu()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GunlukGecisRaporu(string model)
        {
            return View(model);
        }
        public ActionResult SistemLog()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SistemLog(string model)
        {
            return View(model);
        }

        public ActionResult KartTipiEkle()
        {
            KartTipiBilgileri model = new KartTipiBilgileri();
            model.KullaniciMesaji = "";
            //KartTipiBilgileri model = new KartTipiBilgileri();
            //return View(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult KartTipiEkle(KartTipiBilgileri model)
        {
            //DB db = new DB();
            kart_tipleri kartTipleriModel = new kart_tipleri();
            kartTipleriModel.kart_tipi = model.KartTipi;
            kartTipleriModel.ucret = model.Ucret;
            kartTipleriModel.kayit_tarihi = DateTime.Now;
            kartTipleriModel.guncelleme_tarihi = DateTime.Now;
            db.kart_tipleri.Add(kartTipleriModel);
            db.SaveChanges();
            model.KullaniciMesaji = "Kart Tipi Başarıyla Eklendi";
            return View(model);
        }

        public ActionResult KartTipiListesi()
        {
            var model = db.kart_tipleri.ToList();
            return View(model);
        }

        public ActionResult KartTipiGuncelle(int id)
        {
            kart_tipleri modelDB = db.kart_tipleri.Find(id);
            return View("KartTipiGuncelle",modelDB);
        }

        [HttpPost]
        public ActionResult KartTipiGuncelle(kart_tipleri model)
        {
            kart_tipleri modelDB = db.kart_tipleri.Find(model.kart_tipi_id);
            modelDB.kart_tipi = model.kart_tipi;
            modelDB.ucret = model.ucret;
            modelDB.guncelleme_tarihi = DateTime.Now;
            db.SaveChanges();
            ViewData["KullaniciMesaji"] = "Kart Tipi Güncellendi";
            return View();
        }



        private KartKullanicilari GetData()
        {
            KartKullanicilari model = new KartKullanicilari();
            model.KartListesi = (from a in db.kart_tipleri.ToList()
                                 select new SelectListItem
                                 {
                                     Selected=false,
                                     Text=a.kart_tipi,
                                     Value=a.kart_tipi_id.ToString()
                                 }).ToList();
            model.KartListesi.Insert(0, new SelectListItem
            {   
                Selected=true,
                Text = "--seçiniz--",
                Value=""
            });
            
            return model;
        }
    }
}