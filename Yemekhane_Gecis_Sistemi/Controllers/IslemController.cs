using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yemekhane_Gecis_Sistemi.Models;

namespace Yemekhane_Gecis_Sistemi.Controllers
{
    public class IslemController : Controller
    {
        // GET: Islem
        public ActionResult Index()
        {
            return View();
        }
        DB db = new DB();

        public void SistemLog(int kullanici_id,int islem_tipi_id,string mesaj)
        {
            sistem_log sistem_log_model = new sistem_log();
            sistem_log_model.islem_tarihi = DateTime.Now;
            sistem_log_model.kullanici_id = kullanici_id;
            sistem_log_model.islem_tipi_id = islem_tipi_id;
            sistem_log_model.mesaj = mesaj;
            db.sistem_log.Add(sistem_log_model);
            db.SaveChanges();
        }
        public void LogTut(string kartno,int islem_tipi,int islem_sonuc,double ucret,double kalan)
        {

            gecis_loglari gecisLog = new gecis_loglari(); 

                var kullanici_kodu = (from a in db.kart_bilgileri where a.kart_no == kartno 
                                      select new { a.kullanici_id,a.ID }).FirstOrDefault();
            if (islem_tipi == 2 && islem_sonuc==1)
            {
                gecisLog.kullanici_id = kullanici_kodu.kullanici_id;
                gecisLog.kart_id = kullanici_kodu.ID;
                gecisLog.islem_tipi_id = islem_tipi;
                gecisLog.islem_sonuc_id = islem_sonuc;
                gecisLog.ucret = ucret.ToString();
                gecisLog.kalan_bakiye = kalan.ToString();
                gecisLog.mesaj = "Ücret:" + ucret.ToString() + " Kalan Bakiye:" + kalan.ToString();
                ViewData["KullaniciMesaji"] = gecisLog.mesaj;
                gecisLog.kayit_tarihi = DateTime.Now;
                db.gecis_loglari.Add(gecisLog);
                db.SaveChanges();
            }           
            else if (islem_tipi==2 && islem_sonuc==2)
            {
                gecisLog.kullanici_id = kullanici_kodu.kullanici_id;
                gecisLog.kart_id = kullanici_kodu.ID;
                gecisLog.islem_tipi_id = islem_tipi;
                gecisLog.islem_sonuc_id = islem_sonuc;
                gecisLog.ucret = ucret.ToString();
                gecisLog.kalan_bakiye = kalan.ToString();
                gecisLog.mesaj = "Yetersiz Bakiye!!!";
                ViewData["KullaniciMesaji"] = gecisLog.mesaj;
                gecisLog.kayit_tarihi = DateTime.Now;
                db.gecis_loglari.Add(gecisLog);
                db.SaveChanges();
            }
            else if (islem_tipi == 1 && islem_sonuc == 1)
            {
                gecisLog.kullanici_id = kullanici_kodu.kullanici_id;
                gecisLog.kart_id = kullanici_kodu.ID;
                gecisLog.islem_tipi_id = islem_tipi;
                gecisLog.islem_sonuc_id = islem_sonuc;
                gecisLog.ucret = ucret.ToString();
                gecisLog.kalan_bakiye = kalan.ToString();
                gecisLog.mesaj = "Yüklenen:" + ucret.ToString() + " Bakiye:" + kalan.ToString();
                ViewData["KullaniciMesaji"] = gecisLog.mesaj;
                gecisLog.kayit_tarihi = DateTime.Now;
                db.gecis_loglari.Add(gecisLog);
                db.SaveChanges();
            }
            else
            {
                ViewData["KullaniciMesaji"] = "Bir problem oluştu";
            }
        }

        public void GecisYap(string kartno)
        {
            var kart_bilgisi = (from a in db.kart_bilgileri where a.kart_no == kartno select a).FirstOrDefault();
           
            if (kart_bilgisi!=null)
            {
                var kullanici = (from k in db.kullanicilar where k.kullanici_id == kart_bilgisi.kullanici_id select k).FirstOrDefault();
                var ucret = (from a in db.kart_bilgileri
                             join b in db.kart_tipleri
                             on a.kart_tipi_id equals b.kart_tipi_id
                             where a.kart_no==kartno
                             select b.ucret).FirstOrDefault();
                //var kart_bilgisi = (from a in db.kart_bilgileri where a.kart_no == kartno select a).FirstOrDefault();
                if (Convert.ToDouble(kullanici.bakiye) >= Convert.ToDouble(ucret))
                {
                    kullanici.bakiye = (Convert.ToDouble(kullanici.bakiye) - Convert.ToDouble(ucret)).ToString(); 
                db.SaveChanges();
                LogTut(kartno,2,1, Convert.ToDouble(ucret),Convert.ToDouble(kullanici.bakiye));
                }
                else
                {
                    LogTut(kartno, 2,2, Convert.ToDouble(ucret), Convert.ToDouble(kullanici.bakiye));
                }
                              
                
                
            }
            else
            {
                ViewData["KullaniciMesaji"] = "kayıt bulunamadı";
            }
        }

        public ActionResult GecisTest()
        {

            return View();
        }

        [HttpPost]
        public ActionResult GecisTest(kart_bilgileri kartno)
        {
            GecisYap(kartno.kart_no);
            return View();
        }
        

        
    }
}