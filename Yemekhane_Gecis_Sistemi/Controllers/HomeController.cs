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
        IslemController islem = new IslemController();
        int session_kullanici_kodu = 1;
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



            var tc_kontrol = (from k in db.kullanicilar where k.tc == model.TcKimlikNo || k.email == model.EMail select k).FirstOrDefault();
            var kartno_kontrol = (from kb in db.kart_bilgileri where kb.kart_no == model.KartNo select kb).FirstOrDefault();
            if (tc_kontrol != null)
            {
                ViewBag.KullaniciMesaji = "Tc veya Mail Bilgisi Sistemde Kayıtlı!!!";
            }
            else if (kartno_kontrol != null)
            {
                ViewBag.KullaniciMesaji = "Girilen Kart Numarası Sistemde Kayıtlı!!!";
            }
            else
            {

                kullanicilar kullanicilarModel = new kullanicilar();
                kullanicilarModel.tc = model.TcKimlikNo;
                kullanicilarModel.email = model.EMail;
                kullanicilarModel.ad = model.Ad;
                kullanicilarModel.soyad = model.Soyad;
                kullanicilarModel.kullanici_adi = model.EMail;
                kullanicilarModel.sifre = model.Sifre;
                kullanicilarModel.bakiye = "0";
                kullanicilarModel.kayit_tarihi = DateTime.Now;
                kullanicilarModel.guncelleme_tarihi = DateTime.Now;
                kullanicilarModel.aktif_mi = 1;
                db.kullanicilar.Add(kullanicilarModel);
                //db.SaveChanges();
                kart_bilgileri kartBilgileriModel = new kart_bilgileri();
                kartBilgileriModel.kullanici_id = (from a in db.kullanicilar where a.tc == model.TcKimlikNo select a.kullanici_id).FirstOrDefault();
                kartBilgileriModel.kart_tipi_id = model.KartTipiId;
                kartBilgileriModel.kart_no = model.KartNo;
                kartBilgileriModel.bakiye = "0";
                kartBilgileriModel.durum = 1;
                kartBilgileriModel.son_gecerlilik_tarihi = DateTime.Now.AddYears(4);
                db.kart_bilgileri.Add(kartBilgileriModel);
                db.SaveChanges();



                islem.SistemLog(session_kullanici_kodu, 3, model.TcKimlikNo + " tc numaralı " + model.Ad + " " + model.Soyad + " kişisi sisteme eklendi");

                ViewBag.KullaniciMesaji = "Kayıt Başarıyla Gerçekleşti";
            }
            model = GetData();
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
            islem.SistemLog(session_kullanici_kodu, 3, model.BirimAdi + " birimi sisteme eklendi");
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
            islem.SistemLog(session_kullanici_kodu, 3, model.UnvanAdi + " unvanı sisteme eklendi");
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
        public ActionResult IslemLog(string tckimlikno)
        {
            var kullanici_id = (from a in db.kullanicilar where a.tc == tckimlikno select a).FirstOrDefault();
            var gecis_loglari_model = (from a in db.view_gecis_loglari where a.kullanici_id == kullanici_id.kullanici_id select a).ToList();
            var kart_bilgileri = (from a in db.view_kullanicilar where a.kullanici_id == kullanici_id.kullanici_id select a).FirstOrDefault();
            ViewData["ad"] = kullanici_id.ad;
            ViewData["soyad"] = kullanici_id.soyad;
            ViewData["kart_tipi"] = kart_bilgileri.kart_tipi;
            ViewData["bakiye"] = kart_bilgileri.bakiye.ToString();

            return View(gecis_loglari_model);
        }
        public ActionResult KullaniciListele()
        {
            //DB db = new DB();
            var model = db.kullanicilar.ToList();
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

            var model = (from kb in db.kart_bilgileri where kb.kullanici_id == id && kb.durum == 1 select kb).FirstOrDefault();

            //var model = (from kb in db.kart_bilgileri
            //             join kl in db.kullanicilar on kb.kullanici_id equals kl.kullanici_id
            //             join kt in db.kart_tipleri on kb.kart_tipi_id equals kt.kart_tipi_id
            //             where kl.kullanici_id == id
            //             select new { kb, kl, kt }
            //           ).FirstOrDefault();
            var model2 = (from kb in db.kart_bilgileri where kb.kullanici_id == id select kb).FirstOrDefault();

            var kullanici_model = (from k in db.kullanicilar where k.kullanici_id == id select k).FirstOrDefault();

            List<SelectListItem> kartListesi = (from a in db.kart_tipleri
                                                select new SelectListItem
                                                {
                                                    Text = a.kart_tipi,
                                                    Value = a.kart_tipi_id.ToString(),
                                                    Selected = (a.kart_tipi_id == model2.kart_tipi_id)

                                                }).ToList();
            ViewBag.secim = model2.kart_tipi_id;
            ViewBag.kartList = kartListesi;

            List<SelectListItem> birimListesi = (from a in db.birimler
                                                 select new SelectListItem
                                                 {
                                                     Text = a.birim_adi,
                                                     Value = a.birim_id.ToString(),
                                                     Selected = (a.birim_id == kullanici_model.birim_id)

                                                 }).ToList();

            ViewBag.birim_secim = kullanici_model.birim_id;
            ViewBag.birimList = birimListesi;

            List<SelectListItem> unvanListesi = (from a in db.unvanlar
                                                 select new SelectListItem
                                                 {
                                                     Text = a.unvan_adi,
                                                     Value = a.unvan_id.ToString(),
                                                     Selected = (a.unvan_id == kullanici_model.unvan_id)

                                                 }).ToList();

            ViewBag.unvan_secim = kullanici_model.unvan_id;
            ViewBag.unvanList = unvanListesi;

            return View("KullaniciGetir", model);
        }


        public ActionResult KartDetaylari(int? id)
        {
            var model = (from kb in db.kart_bilgileri where kb.kullanici_id == id select kb).ToList();
            string ad = (from k in db.kullanicilar where k.kullanici_id == id select k.ad).FirstOrDefault();
            string soyad = (from k in db.kullanicilar where k.kullanici_id == id select k.soyad).FirstOrDefault();
            ViewBag.AdSoyad = ad + " " + soyad + " kart detayları";
            ViewBag.kullanici_kodu = id;
            string resim = (from k in db.kullanicilar where k.kullanici_id == id select k.resim).FirstOrDefault();
            if (resim != null)
            {
                ViewBag.resim = resim;
            }
            else
            {
                ViewBag.resim = "empty.jpg";
            }

            return PartialView("KartDetaylari", model);
        }


        public ActionResult ResimYukle(HttpPostedFileBase resim_yol, int id)
        {
            if (resim_yol != null && resim_yol.ContentLength > 0)
            {

                resim_yol.SaveAs(Server.MapPath("~/images/" + id + ".jpg"));
                var kullanicilar = db.kullanicilar.Find(id);
                kullanicilar.resim = id + ".jpg";
                db.SaveChanges();
            }
            return RedirectToAction("KullaniciListele");

        }

        public ActionResult Guncelle(kart_bilgileri a)
        {

            var kullanicilar = db.kullanicilar.Find(a.kullanici_id);
            kullanicilar.ad = a.kullanicilar.ad;
            kullanicilar.soyad = a.kullanicilar.soyad.Trim();
            kullanicilar.email = a.kullanicilar.email.Trim();
            kullanicilar.tc = a.kullanicilar.tc.Trim();
            kullanicilar.kullanici_adi = a.kullanicilar.email.Trim();


            var kart_kontrol = (from kb in db.kart_bilgileri where kb.kart_no == a.kart_no && kb.kullanici_id != a.kullanici_id select kb).FirstOrDefault();
            if (kart_kontrol == null)
            {

                var kartlar = (from kt in db.kart_bilgileri
                               where
                                kt.kullanici_id == a.kullanici_id &&
                                kt.durum == 1 &&
                                kt.kart_tipi_id == a.kart_tipi_id &&
                                kt.kart_no != a.kart_no
                               select kt).ToList();


                foreach (var kart_listesi in kartlar)
                {

                    kart_listesi.durum = 2;

                }
              
                
                 if (kartlar.Count>0)
                {



                    kart_bilgileri kart_bilgileri_model = new kart_bilgileri();
                    kart_bilgileri_model.kullanici_id = a.kullanici_id;
                    kart_bilgileri_model.kart_no = a.kart_no;
                    kart_bilgileri_model.bakiye = "0";
                    kart_bilgileri_model.kart_tipi_id = a.kart_tipi_id;
                    kart_bilgileri_model.durum = 1;
                    kart_bilgileri_model.son_gecerlilik_tarihi = DateTime.Now.AddYears(4);
                    kart_bilgileri_model.kayit_tarihi = DateTime.Now;
                    db.kart_bilgileri.Add(kart_bilgileri_model);
                    ViewBag.KullaniciMesaji = "Kart Eklendi";
                    ViewBag.UyariRengi = "";

                }
                 else
                {
                    ViewBag.KullaniciMesaji = "Kişi Bilgisi Güncellendi";
                    ViewBag.UyariRengi = "";
                }
                
                
            }
            else
            {
                ViewBag.KullaniciMesaji = "Bu Kart Numarası Başka Bir Kullanıcıya Aittir!!";
                ViewBag.UyariRengi = "";
            }

            db.SaveChanges();


            islem.SistemLog(session_kullanici_kodu, 4, a.kullanicilar.tc + " tc numaralı " + a.kullanicilar.ad + " " + a.kullanicilar.soyad + " kişisi güncellendi");

            //return RedirectToAction("KullaniciListele");
            return RedirectToAction("KullaniciGetir/" + a.kullanici_id);
        }


        public ActionResult BakiyeYukle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BakiyeYukle(kullanicilar a)
        {

            kullanicilar model = db.kullanicilar.Where(x => x.tc.Equals(a.tc)).FirstOrDefault();


            if (a.bakiye != null)
            {
                var kart_no = (from kb in db.kart_bilgileri where kb.kullanici_id == model.kullanici_id && kb.durum == 1 select kb.kart_no).FirstOrDefault();
                model.bakiye = (Convert.ToDouble(model.bakiye) + Convert.ToDouble(a.bakiye)).ToString();
                db.SaveChanges();
                islem.LogTut(kart_no.ToString(), 1, 1, Convert.ToDouble(a.bakiye), Convert.ToDouble(model.bakiye));
                islem.SistemLog(session_kullanici_kodu, 1, a.tc + " tc numaralı " + a.ad + " " + a.soyad + " kişisine " + a.bakiye + " TL yüklendi.");
                return Redirect("KullaniciListele");
            }
            return View(model);
        }



        public ActionResult KisiGecisRaporu()
        {
            return View();
        }
        [HttpPost]
        public ActionResult KisiGecisRaporu(string baslangic_tarihi, string bitis_tarihi)
        {

            List<SP_KISI_GECIS_RAPORU_Result> kisi = db.SP_KISI_GECIS_RAPORU(baslangic_tarihi, bitis_tarihi + " 23:59:59").ToList();


            ExportToExcelFile<SP_KISI_GECIS_RAPORU_Result, List<SP_KISI_GECIS_RAPORU_Result>> excelExport = new
            ExportToExcelFile<SP_KISI_GECIS_RAPORU_Result, List<SP_KISI_GECIS_RAPORU_Result>>();
            excelExport.dataToPrint = kisi;
            excelExport.GenerateReport();
            islem.SistemLog(session_kullanici_kodu, 6, "Kişi geçiş raporu alındı.");

            return View(kisi);

        }
        public ActionResult GunlukGecisRaporu()
        {
            List<GunlukRapor> grapor = new List<GunlukRapor>();
            var gunluk_gecisler = (from a in db.view_gecis_loglari
                                   where a.islem_mesaji == "Başarılı" && a.islem_adi == "Yemek Geçişi"
                                   && a.kayit_tarihi >= DateTime.Today && a.kayit_tarihi < DateTime.Now
                                   group a by a.kart_tipi into grp
                                   select new { krt = grp.Key, cnt = grp.Count() }).ToList();
            if (gunluk_gecisler != null)
            {
                foreach (var a in gunluk_gecisler)
                {

                    grapor.Add(new GunlukRapor()
                    {
                        kart_tipi = a.krt,
                        gecis_sayisi = Convert.ToInt32(a.cnt)
                    });
                }
            }
            return View(grapor);
        }
        [HttpPost]
        public ActionResult GunlukGecisRaporu(string model)
        {
            return View(model);
        }
        public ActionResult SistemLog()
        {
            var sistem_loglari = db.view_sistem_log.ToList();

            return View(sistem_loglari);
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
            islem.SistemLog(session_kullanici_kodu, 3, model.KartTipi + " kartı sisteme eklendi");
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
            return View("KartTipiGuncelle", modelDB);
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
            islem.SistemLog(session_kullanici_kodu, 4, model.kart_tipi + " kartı güncellendi");
            return View();
        }

        public ActionResult Duyurular()
        {
            ViewBag.kart_tipi_listesi = db.kart_tipleri.Select(x => new SelectListItem
            { Value = x.kart_tipi_id.ToString(), Text = x.kart_tipi }).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Duyurular(duyurular model)
        {
            model.gonderen_id = session_kullanici_kodu;
            model.kayit_tarihi = DateTime.Now;
            //duyurular duyurular_model = new duyurular();
            db.duyurular.Add(model);
            db.SaveChanges();
            ViewBag.kart_tipi_listesi = db.kart_tipleri.Select(x => new SelectListItem
            { Value = x.kart_tipi_id.ToString(), Text = x.kart_tipi }).ToList();
            ViewBag.KullaniciMesaji = "Mesaj kullanıcılara gönderildi.";
            return View();
        }



        private KartKullanicilari GetData()
        {
            KartKullanicilari model = new KartKullanicilari();
            model.KartListesi = (from a in db.kart_tipleri.ToList()
                                 select new SelectListItem
                                 {
                                     Selected = false,
                                     Text = a.kart_tipi,
                                     Value = a.kart_tipi_id.ToString()
                                 }).ToList();

            model.BirimListesi = (from a in db.birimler.ToList()
                                  select new SelectListItem
                                  {
                                      Selected = false,
                                      Text = a.birim_adi,
                                      Value = a.birim_id.ToString()
                                  }).ToList();

            model.UnvanListesi = (from a in db.unvanlar.ToList()
                                  select new SelectListItem
                                  {
                                      Selected = false,
                                      Text = a.unvan_adi,
                                      Value = a.unvan_id.ToString()
                                  }).ToList();


            model.KartListesi.Insert(0, new SelectListItem
            {
                Selected = true,
                Text = "--seçiniz--",
                Value = ""
            });


            model.BirimListesi.Insert(0, new SelectListItem
            {
                Selected = true,
                Text = "--seçiniz--",
                Value = ""
            });


            model.UnvanListesi.Insert(0, new SelectListItem
            {
                Selected = true,
                Text = "--seçiniz--",
                Value = ""
            });



            return model;
        }
    }
}