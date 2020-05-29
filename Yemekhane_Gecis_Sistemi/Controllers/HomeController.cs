using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Yemekhane_Gecis_Sistemi.Models;
using Yemekhane_Gecis_Sistemi.ViewModels;
using PagedList;
using PagedList.Mvc;
using System.Net.Mail;

namespace Yemekhane_Gecis_Sistemi.Controllers
{
    public class HomeController : Controller
    {
        DB db = new DB();
        IslemController islem = new IslemController();


        public ActionResult Login()
        {
            Session.Abandon();
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {

            var login_kontrol = (from k in db.kullanicilar where k.kullanici_adi == username && !string.IsNullOrEmpty(password) && k.aktif_mi == 1 select k).FirstOrDefault();
            if (login_kontrol != null)
            {
                var kart_bilgi = (from kb in db.kart_bilgileri where kb.kullanici_id == login_kontrol.kullanici_id && kb.durum == 1 select kb).FirstOrDefault();
                Session["ad"] = login_kontrol.ad;
                Session["soyad"] = login_kontrol.soyad;
                Session["yetki_id"] = login_kontrol.yetki_id;
                Session["bakiye"] = login_kontrol.bakiye;
                if (kart_bilgi == null) Session["kart_tipi_id"] = 0;
                else Session["kart_tipi_id"] = kart_bilgi.kart_tipi_id;
                Session["kullanici_id"] = login_kontrol.kullanici_id;
                islem.SistemLog(login_kontrol.kullanici_id, 8, "Giriş Başarılı");
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["mesaj"] = "Kullanıcı Adı Veya Şifre Hatalı!!";               
                return View();
            }

        }

        public ActionResult Index()
        {
            if (Session["yetki_id"] != null)
            {
                string kullanici_id = Session["kullanici_id"].ToString();
                var kullanici_kart_tipleri = (from kb in db.kart_bilgileri
                                              where
                 kb.kullanici_id.ToString() == kullanici_id &&
                 kb.durum == 1
                                              select kb.kart_tipi_id).ToList();
                string kart_tipi_id = Session["kart_tipi_id"].ToString();
                var mesajlar = (from d in db.duyurular where kullanici_kart_tipleri.Contains(d.kart_tipi_id) orderby d.id descending select d).ToList();
                TempData["mesaj_sayisi"] = mesajlar.Count();

                
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }

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
            if (Session["yetki_id"] != null)
            {
                KartKullanicilari model = GetData();
                model.Baslik = "Kayıt Ekleme";
                string txtKartNo = islem.KartOku();
                if (txtKartNo != "0" && txtKartNo != "")
                {
                    //model.KartNo = TempData["KartNo"].ToString();
                    model.KartNo = txtKartNo;

                }


                return View(model);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult KullaniciEkle(KartKullanicilari model)
        {

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
                kullanicilarModel.birim_id = model.BirimId;
                kullanicilarModel.unvan_id = model.UnvanId;
                kullanicilarModel.yetki_id = 2;
                kullanicilarModel.kullanici_adi = model.EMail;
                kullanicilarModel.sifre = model.Sifre;
                kullanicilarModel.bakiye = "0";
                kullanicilarModel.kayit_tarihi = DateTime.Now;
                kullanicilarModel.guncelleme_tarihi = DateTime.Now;
                kullanicilarModel.aktif_mi = 1;
                db.kullanicilar.Add(kullanicilarModel);

                kart_bilgileri kartBilgileriModel = new kart_bilgileri();
                kartBilgileriModel.kullanici_id = (from a in db.kullanicilar where a.tc == model.TcKimlikNo select a.kullanici_id).FirstOrDefault();
                kartBilgileriModel.kart_tipi_id = model.KartTipiId;
                kartBilgileriModel.kart_no = model.KartNo;
                //kartBilgileriModel.bakiye = "0";
                kartBilgileriModel.durum = 1;
                if (model.SonGecerlilikTarihi == null)
                {
                    kartBilgileriModel.son_gecerlilik_tarihi = DateTime.Now.AddYears(4);
                }
                else
                {

                    kartBilgileriModel.son_gecerlilik_tarihi = Convert.ToDateTime(model.SonGecerlilikTarihi);

                }

                db.kart_bilgileri.Add(kartBilgileriModel);
                db.SaveChanges();



                islem.SistemLog(Convert.ToInt32(Session["kullanici_id"]), 3, model.TcKimlikNo + " tc numaralı " + model.Ad + " " + model.Soyad + " kişisi sisteme eklendi");

                ViewBag.KullaniciMesaji = "Kayıt Başarıyla Gerçekleşti";
            }
            model = GetData();
            return View(model);

        }

        public ActionResult BirimEkle()
        {
            if (Session["yetki_id"] != null)
            {
                Birimler model = new Birimler();
                model.Baslik = "Birim Ekleme";
                return View(model);
            }
            else
            {
                return RedirectToAction("Login");
            }
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
            islem.SistemLog(Convert.ToInt32(Session["kullanici_id"]), 3, model.BirimAdi + " birimi sisteme eklendi");
            return View(model);
        }

        public ActionResult BirimListesi()
        {
            if (Session["yetki_id"] != null)
            {
                var model = db.birimler.ToList();
                return View(model);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult BirimGuncelle(int id)
        {
            if (Session["yetki_id"] != null)
            {
                var model = db.birimler.Find(id);
                return View("BirimGuncelle", model);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult BirimGuncelle(birimler a)
        {
            var model = db.birimler.Find(a.birim_id);
            model.birim_adi = a.birim_adi;
            db.SaveChanges();
            ViewData["mesaj"] = "Birim güncellendi.";
            return View();
        }
        public ActionResult UnvanEkle()
        {
            if (Session["yetki_id"] != null)
            {
                Unvanlar model = new Unvanlar();
                model.Baslik = "Birim Ekleme";
                return View(model);
            }
            return RedirectToAction("Login");
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
            islem.SistemLog(Convert.ToInt32(Session["kullanici_id"]), 3, model.UnvanAdi + " unvanı sisteme eklendi");
            return View(model);
        }

        public ActionResult UnvanListesi()
        {
            if (Session["yetki_id"] != null)
            {
                var model = db.unvanlar.ToList();
                return View(model);
            }
            else
            {
                return RedirectToAction("Login");
            }

        }
        public ActionResult UnvanGuncelle(int id)
        {
            if (Session["yetki_id"] != null)
            {
                var model = db.unvanlar.Find(id);
                return View("UnvanGuncelle", model);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult UnvanGuncelle(unvanlar a)
        {
            var model = db.unvanlar.Find(a.unvan_id);
            model.unvan_adi = a.unvan_adi;
            db.SaveChanges();
            ViewData["mesaj"] = "Unvan güncellendi.";
            return View();

        }

        public ActionResult IslemLog()
        {
            if (Session["yetki_id"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]

        public ActionResult IslemLog(string tckimlikno)
        {
            var kullanici_id = (from a in db.kullanicilar where a.tc == tckimlikno select a).FirstOrDefault();

            if (kullanici_id != null)
            {

                var gecis_loglari_model = (from a in db.view_gecis_loglari where a.kullanici_id == kullanici_id.kullanici_id select a).ToList();
                var kart_bilgileri = (from a in db.view_kullanicilar where a.kullanici_id == kullanici_id.kullanici_id select a).FirstOrDefault();
                ViewData["ad"] = kullanici_id.ad;
                ViewData["soyad"] = kullanici_id.soyad;
                ViewData["kart_tipi"] = kart_bilgileri.kart_tipi;
                ViewData["bakiye"] = kart_bilgileri.bakiye.ToString();
                return View(gecis_loglari_model);
            }
            else
            {

                ViewData["mesaj"] = "Kullanıcı bulunamadı!";
                return View();
            }
            //return View(gecis_loglari_model);
        }
        public ActionResult KullaniciListele()
        {
            if (Session["yetki_id"] != null)
            {
                var model = db.kullanicilar.Where(x => x.aktif_mi == 1).ToList();
                return View(model);
            }
            else
            {
                return RedirectToAction("Login");
            }


        }
        [HttpPost]
        public ActionResult KullaniciListele(string model)
        {
            return View(model);
        }

        public ActionResult KullaniciGetir(int id)

        {
            if (Session["yetki_id"] != null)
            {
                string txtKartNo = islem.KartOku();

                var model = (from kb in db.kart_bilgileri where kb.kullanici_id == id orderby kb.guncelleme_tarihi descending, kb.ID descending select kb).FirstOrDefault();
                if (txtKartNo != "0" && txtKartNo != "")
                {

                    model.kart_no = txtKartNo;

                }

                var kullanici_model = (from k in db.kullanicilar where k.kullanici_id == id && k.aktif_mi == 1 select k).FirstOrDefault();

                List<SelectListItem> kartListesi = (from a in db.kart_tipleri
                                                    select new SelectListItem
                                                    {
                                                        Text = a.kart_tipi,
                                                        Value = a.kart_tipi_id.ToString(),
                                                        Selected = (a.kart_tipi_id == model.kart_tipi_id)

                                                    }).ToList();
                ViewBag.secim = model.kart_tipi_id;
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
            else
            {
                return RedirectToAction("Login");
            }
        }


        public ActionResult KartDetaylari(int? id)
        {
            if (Session["yetki_id"] != null)
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
            else
            {
                return RedirectToAction("Login");
            }
        }


        public ActionResult ResimYukle(HttpPostedFileBase resim_yol, int id)
        {
            if (resim_yol != null && resim_yol.ContentLength > 0)
            {
                string[] resimUzantilari = new string[] { "image/bmp", "image/jpeg", "image/gif", "image/png" };
                if (resimUzantilari.Contains(resim_yol.ContentType))
                {
                    resim_yol.SaveAs(Server.MapPath("~/images/" + id + ".jpg"));
                    var kullanicilar = db.kullanicilar.Find(id);
                    kullanicilar.resim = id + ".jpg";
                    db.SaveChanges();
                    ViewData["mesaj"] = "Fotoğraf güncellendi";
                }
                else
                {
                    ViewData["mesaj"] = "Fotoğraf uygun formatta olmalıdır.";
                }

            }
            return RedirectToAction("KullaniciListele");

        }

        public ActionResult Guncelle(kart_bilgileri a)
        {
            if (Session["yetki_id"] != null)
            {
                var kullanicilar = db.kullanicilar.Find(a.kullanici_id);
                kullanicilar.ad = a.kullanicilar.ad;
                kullanicilar.soyad = a.kullanicilar.soyad.Trim();
                kullanicilar.email = a.kullanicilar.email.Trim();
                kullanicilar.tc = a.kullanicilar.tc.Trim();
                kullanicilar.kullanici_adi = a.kullanicilar.email.Trim();
                kullanicilar.guncelleme_tarihi = DateTime.Now;

                var kart_kontrol = (from kb in db.kart_bilgileri where kb.kart_no == a.kart_no && kb.kullanici_id != a.kullanici_id select kb).FirstOrDefault();
                if (kart_kontrol == null)
                {

                    var aktif_kartlar = (from kt in db.kart_bilgileri
                                         where
                                          kt.kullanici_id == a.kullanici_id &&
                                          kt.durum == 1 &&
                                          kt.kart_tipi_id == a.kart_tipi_id &&
                                          kt.kart_no != a.kart_no
                                         select kt).ToList();






                    var kart_varmi = (from kt in db.kart_bilgileri
                                      where
                                       kt.kullanici_id == a.kullanici_id &&
                                       kt.kart_no == a.kart_no &&
                                       kt.durum == 1
                                      select kt).ToList();

                    if ((aktif_kartlar.Count > 0 || kart_varmi.Count < 1) && kart_varmi.Count < 1)
                    {

                        foreach (var kart_listesi in aktif_kartlar)
                        {

                            kart_listesi.durum = 2;
                            kart_listesi.guncelleme_tarihi = DateTime.Now;

                        }

                        kart_bilgileri kart_bilgileri_model = new kart_bilgileri();
                        kart_bilgileri_model.kullanici_id = a.kullanici_id;
                        kart_bilgileri_model.kart_no = a.kart_no;
                        //kart_bilgileri_model.bakiye = "0";
                        kart_bilgileri_model.kart_tipi_id = a.kart_tipi_id;
                        kart_bilgileri_model.durum = 1;
                        if (a.son_gecerlilik_tarihi != null)
                        {
                            kart_bilgileri_model.son_gecerlilik_tarihi = a.son_gecerlilik_tarihi;
                        }
                        else
                        {
                            kart_bilgileri_model.son_gecerlilik_tarihi = DateTime.Now.AddYears(4);
                        }

                        kart_bilgileri_model.kayit_tarihi = DateTime.Now;
                        kart_bilgileri_model.guncelleme_tarihi = DateTime.Now;
                        db.kart_bilgileri.Add(kart_bilgileri_model);
                        TempData["mesaj"] = "Kart Eklendi";
                        ViewBag.KullaniciMesaji = "Kart Eklendi";
                        ViewBag.UyariRengi = "";

                    }
                    else
                    {
                        if (a.son_gecerlilik_tarihi != null)
                        {
                            kart_varmi.FirstOrDefault().son_gecerlilik_tarihi = a.son_gecerlilik_tarihi;
                        }

                        TempData["mesaj"] = "Kişi Bilgisi Guncellendi";
                        ViewBag.KullaniciMesaji = "Kişi Bilgisi Güncellendi";
                        ViewBag.UyariRengi = "";
                    }


                }
                else
                {
                    TempData["mesaj"] = "Bu Kart Numarası Başka Bir Kullanıcıya Aittir!!";
                    ViewBag.KullaniciMesaji = "Bu Kart Numarası Başka Bir Kullanıcıya Aittir!!";
                    ViewBag.UyariRengi = "";
                }

                db.SaveChanges();


                islem.SistemLog(Convert.ToInt32(Session["kullanici_id"]), 4, a.kullanicilar.tc + " tc numaralı " + a.kullanicilar.ad + " " + a.kullanicilar.soyad + " kişisi güncellendi");

                //return RedirectToAction("KullaniciListele");
                return RedirectToAction("KullaniciGetir/" + a.kullanici_id);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult KullaniciSil(int id)
        {
            var kart_kontrol = (from kb in db.kart_bilgileri where kb.kullanici_id == id && kb.durum == 1 select kb).FirstOrDefault();
            if (kart_kontrol == null)
            {
                var kullanici = db.kullanicilar.Find(id);
                kullanici.aktif_mi = 0;
                //db.kullanicilar.Remove(kullanici);
                db.SaveChanges();
                TempData["mesaj"] = "Kişi silindi.";
                islem.SistemLog(Convert.ToInt32(Session["kullanici_id"]), 5, kullanici.tc + " tc numaralı " + kullanici.ad + " " + kullanici.soyad + " kişisi silindi.");
            }
            else
            {
                TempData["mesaj"] = "Seçtiğiniz kullanıcının aktif kartı bulunmatkatdır. Detay ekranından kartları iptal etmeniz gerekmektedir.";
            }
            return RedirectToAction("KullaniciListele");
        }

        public ActionResult KartIptal(int id)
        {
            var kart = (from kb in db.kart_bilgileri where kb.ID == id && kb.durum == 1 select kb).FirstOrDefault();
            kart.durum = 2;
            db.SaveChanges();
            TempData["mesaj"] = "Kart İptal Edildi.";
            islem.SistemLog(Convert.ToInt32(Session["kullanici_id"]), 7, kart.kart_no + " numaralı " + kart.kart_tipleri.kart_tipi + " kartı iptal edildi.");
            return RedirectToAction("KullaniciListele");
        }
        public ActionResult BakiyeYukle()
        {
            if (Session["yetki_id"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult BakiyeYukle(kullanicilar a)
        {

            kullanicilar model = db.kullanicilar.Where(x => x.tc.Equals(a.tc) && x.aktif_mi == 1).FirstOrDefault();
            if (model == null)
            {
                TempData["mesaj"] = "Kullanıcı bulunamadı";
            }

            else if (a.bakiye != null)
            {
                var kart_no = (from kb in db.kart_bilgileri where kb.kullanici_id == model.kullanici_id && kb.durum == 1 select kb.kart_no).FirstOrDefault();
                if (kart_no != null)
                {

                    model.bakiye = (Convert.ToDouble(model.bakiye) + Convert.ToDouble(a.bakiye)).ToString();
                    db.SaveChanges();
                    islem.LogTut(kart_no.ToString(), 1, 1, Convert.ToDouble(a.bakiye), Convert.ToDouble(model.bakiye));
                    islem.SistemLog(Convert.ToInt32(Session["kullanici_id"]), 1, a.tc + " tc numaralı " + model.ad + " " + model.soyad + " kişisine " + a.bakiye + " TL yüklendi.");
                    TempData["mesaj"] = a.tc + " tc numaralı " + model.ad + " " + model.soyad + " kişisine " + a.bakiye + " TL yüklendi.";
                    //return Redirect("KullaniciListele");

                }
                else
                {
                    TempData["mesaj"] = "Kişiye ait aktif kart bulunmamaktadır. Lütfen kişiye ait kartları kontrol ediniz.";
                }

            }


            return View(model);
        }



        public ActionResult KisiGecisRaporu()
        {
            if (Session["yetki_id"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult KisiGecisRaporu(string baslangic_tarihi, string bitis_tarihi)
        {

            if (baslangic_tarihi != "" && bitis_tarihi != "")
            {
                if (Convert.ToDateTime(baslangic_tarihi) <= Convert.ToDateTime(bitis_tarihi))
                {
                    List<SP_KISI_GECIS_RAPORU_Result> kisi = db.SP_KISI_GECIS_RAPORU(baslangic_tarihi, bitis_tarihi + " 23:59:59").ToList();
                    if (kisi.Count > 0)
                    {

                       
                        Session["baslangic_tarihi"] = baslangic_tarihi;
                        Session["bitis_tarihi"] = bitis_tarihi;
                        return View(kisi);
                    }
                    else
                    {
                        ViewData["mesaj"] = "Belirtilen tarihlere ait geçiş kaydı bulunmamaktadır!";
                        return View();
                    }
                }
                else
                {
                    ViewData["mesaj"] = "Başlangıç tarihi bitiş tarihinden büyük olamaz!";
                    return View();
                }




            }
            else
            {
                ViewData["mesaj"] = "Tarih alanını boş bırakmayınız";
                return View();
            }


        }


        [HttpPost]
        public ActionResult KisiGecisRaporuExcel(string baslangic_tarihi, string bitis_tarihi)
        {
            baslangic_tarihi = Session["baslangic_tarihi"].ToString();
            bitis_tarihi = Session["bitis_tarihi"].ToString() ;

            if (baslangic_tarihi != "" && bitis_tarihi != "")
            {

                if (Convert.ToDateTime(baslangic_tarihi) <= Convert.ToDateTime(bitis_tarihi))
                {
                    List<SP_KISI_GECIS_RAPORU_Result> kisi = db.SP_KISI_GECIS_RAPORU(baslangic_tarihi, bitis_tarihi + " 23:59:59").ToList();
                    if (kisi.Count > 0)
                    {

                        ExportToExcelFile<SP_KISI_GECIS_RAPORU_Result, List<SP_KISI_GECIS_RAPORU_Result>> excelExport = new
                        ExportToExcelFile<SP_KISI_GECIS_RAPORU_Result, List<SP_KISI_GECIS_RAPORU_Result>>();
                        excelExport.dataToPrint = kisi;
                        excelExport.GenerateReport();
                        islem.SistemLog(Convert.ToInt32(Session["kullanici_id"]), 6, "Kişi geçiş raporu alındı.");
                         return RedirectToAction("KisiGecisRaporu");

                        //return View(kisi);
                    }
                    else
                    {
                        ViewData["mesaj"] = "Belirtilen tarihlere ait geçiş kaydı bulunmamaktadır!";
                        return View();
                    }
                }
                else
                {
                    ViewData["mesaj"] = "Başlangıç tarihi bitiş tarihinden büyük olamaz!";
                    return View();
                }




            }
            else
            {
                ViewData["mesaj"] = "Tarih alanını boş bırakmayınız";
                return View();
            }
        }
        public ActionResult GunlukGecisRaporu()
        {
            if (Session["yetki_id"] != null)
            {
                List<GunlukRapor> grapor = new List<GunlukRapor>();
                var gunluk_gecisler = (from a in db.view_gecis_loglari
                                       where a.islem_mesaji == "Başarılı" && a.islem_adi == "Yemek Geçişi"
                                       && a.kayit_tarihi >= DateTime.Today && a.kayit_tarihi < DateTime.Now
                                       group a by a.kart_tipi into grp
                                       select new { krt = grp.Key, cnt = grp.Count() }).ToList();
                if (gunluk_gecisler.Count > 0)
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
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult GunlukGecisRaporu(string model)
        {
            return View(model);
        }


        public ActionResult BakiyeYuklemeRaporu()
        {
            if (Session["yetki_id"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult BakiyeYuklemeRaporu(string baslangic_tarihi, string bitis_tarihi)
        {
            List<SP_GUNLUK_YUKLENEN_BAKIYE_Result> yuklenen_bakiye = new List<SP_GUNLUK_YUKLENEN_BAKIYE_Result>();
            if (baslangic_tarihi != "" && bitis_tarihi != "")
            {

                yuklenen_bakiye = db.SP_GUNLUK_YUKLENEN_BAKIYE(baslangic_tarihi, bitis_tarihi + " 23:59:59").ToList();

            }
            else
            {
                ViewData["mesaj"] = "Tarih alanını boş bırakmayınız!!";
            }

            return View(yuklenen_bakiye);
        }



        public ActionResult SistemLog()
        {
            if (Session["yetki_id"] != null)
            {
                var sistem_loglari = db.view_sistem_log.ToList();

                return View(sistem_loglari);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult SistemLog(string model)
        {
            return View(model);
        }

        public ActionResult KartTipiEkle()
        {
            if (Session["yetki_id"] != null)
            {
                KartTipiBilgileri model = new KartTipiBilgileri();
                //model.KullaniciMesaji = "";
                //KartTipiBilgileri model = new KartTipiBilgileri();
                //return View(model);
                return View(model);
            }
            else
            {
                return RedirectToAction("Login");
            }
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
            islem.SistemLog(Convert.ToInt32(Session["kullanici_id"]), 3, model.KartTipi + " kartı sisteme eklendi");
            return View(model);
        }

        public ActionResult KartTipiListesi()
        {
            if (Session["yetki_id"] != null)
            {
                var model = db.kart_tipleri.ToList();
                return View(model);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult KartTipiGuncelle(int id)
        {
            if (Session["yetki_id"] != null)
            {
                kart_tipleri modelDB = db.kart_tipleri.Find(id);
                return View("KartTipiGuncelle", modelDB);
            }
            else
            {
                return RedirectToAction("Login");
            }
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
            islem.SistemLog(Convert.ToInt32(Session["kullanici_id"]), 4, model.kart_tipi + " kartı güncellendi");
            return View();
        }


        public ActionResult Duyurular()
        {
            if (Session["yetki_id"] != null)
            {
                ViewBag.kart_tipi_listesi = db.kart_tipleri.Select(x => new SelectListItem
                { Value = x.kart_tipi_id.ToString(), Text = x.kart_tipi }).ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }



        [HttpPost]
        public ActionResult Duyurular(duyurular model)
        {
            model.gonderen_id = Convert.ToInt32(Session["kullanici_id"]);
            model.kayit_tarihi = DateTime.Now;
            db.duyurular.Add(model);
            db.SaveChanges();
            ViewBag.kart_tipi_listesi = db.kart_tipleri.Select(x => new SelectListItem
            { Value = x.kart_tipi_id.ToString(), Text = x.kart_tipi }).ToList();
            ViewBag.KullaniciMesaji = "Mesaj kullanıcılara gönderildi.";
            return View();
        }

        public ActionResult DuyurulariGoruntule()
        {
            if (Session["yetki_id"] != null)
            {
                string kullanici_id = Session["kullanici_id"].ToString();
                var kullanici_kart_tipleri = (from kb in db.kart_bilgileri where
                                              kb.kullanici_id.ToString() == kullanici_id &&
                                              kb.durum==1 
                                              select kb.kart_tipi_id).ToList();
                string kart_tipi_id = Session["kart_tipi_id"].ToString();
                var mesajlar = (from d in db.duyurular where kullanici_kart_tipleri.Contains(d.kart_tipi_id) orderby d.id descending select d).ToList();
                //TempData["mesaj_sayisi"] = mesajlar.Count();
                return View(mesajlar);
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        public ActionResult KisiGecisleri(int id)
        {
            if (Session["yetki_id"] != null)
            {
                var gecisler = (from vgs in db.view_gecis_loglari where vgs.kullanici_id == id select vgs).ToList();
                return View(gecisler);
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        public ActionResult MailSifreGonder()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MailSifreGonder(string username)
        {
            var kullanici = (from k in db.kullanicilar where k.email == username && k.aktif_mi == 1 select k).FirstOrDefault();
            if (kullanici != null)
            {

                if (kullanici.sifre == null)
                {
                    Random rastgele = new Random();
                    int sayi = rastgele.Next(10000000, 99999999);
                    kullanici.sifre = sayi.ToString();
                    db.SaveChanges();
                }
                SmtpClient sc = new SmtpClient();
                sc.UseDefaultCredentials = false;
                sc.Port = 587;
                sc.Host = "smtp.live.com";
                sc.EnableSsl = true;
                sc.Credentials = new NetworkCredential("sifre_gonderme_hesabi@hotmail.com", "09003101344p");

                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("sifre_gonderme_hesabi@hotmail.com", "Şifre Gönderme Sistemi");
                mail.To.Add(kullanici.email);
                mail.Subject = "Şifreniz"; mail.IsBodyHtml = true; mail.Body = "Kullanıcı Adınız: " + kullanici.kullanici_adi + "<br>Şifreniz:" + kullanici.sifre;
                sc.Send(mail);

                ViewData["mesaj"] = "Şifreniz e posta adresinize gönderildi.";
            }
            else
            {
                ViewData["mesaj"] = "Sistemde böyle bir e posta adresi bulunmamaktadır.";
            }
            return View();
        }

        public ActionResult SifreDegistir()
        {
            if (Session["yetki_id"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        public ActionResult SifreDegistir(string eski_sifre, string yeni_sifre)
        {
            if (Session["yetki_id"] != null)
            {
                int id = Convert.ToInt32(Session["kullanici_id"].ToString());
                if (!string.IsNullOrEmpty(eski_sifre) && !string.IsNullOrEmpty(yeni_sifre))
                {
                    var kullanici = (from k in db.kullanicilar where k.kullanici_id == id && k.sifre == eski_sifre select k).FirstOrDefault();
                    if (kullanici != null)
                    {
                        kullanici.sifre = yeni_sifre;
                        db.SaveChanges();
                        ViewData["mesaj"] = "Sifre Değiştirme Başarıyla Gerçekleşti.";
                        islem.SistemLog(id, 9, " Şifre değiştirildi.");
                    }
                    else
                    {
                        ViewData["mesaj"] = "Eski Şifrenizi Doğru Giriniz!!!.";
                    }
                }
                else
                {
                    ViewData["mesaj"] = "Boş Değer Girilemez !!!.";
                }
            }
            else
            {
                return RedirectToAction("Login");
            }

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