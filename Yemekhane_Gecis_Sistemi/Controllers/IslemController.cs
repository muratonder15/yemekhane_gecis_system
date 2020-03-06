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
        public void LogTut(string kartno,double ucret,double kalan)
        {

            gecis_loglari gecisLog = new gecis_loglari(); 
                var kullanici_kodu = (from a in db.kart_bilgileri where a.kart_no == kartno 
                                      select new { a.kullanici_id,a.ID }).FirstOrDefault();
            gecisLog.kullanici_id = kullanici_kodu.kullanici_id;
            gecisLog.kart_id = kullanici_kodu.ID;
            gecisLog.islem_tipi_id = 2;
            gecisLog.islem_sonuc_id = 1;
            gecisLog.ucret = ucret.ToString();
            gecisLog.kalan_bakiye= kalan.ToString();
            gecisLog.mesaj = "Ücret:" + ucret.ToString() + " Kalan Bakiye:"+kalan.ToString();
            ViewData["KullaniciMesaji"] = gecisLog.mesaj;
            gecisLog.kayit_tarihi = DateTime.Now;
            db.gecis_loglari.Add(gecisLog);
            db.SaveChanges();
        }

        public void GecisYap(string kartno)
        {
            var kayit_sayisi = (from a in db.kart_bilgileri where a.kart_no == kartno select a).Count();
           
            if (kayit_sayisi == 1)
            {
                var ucret = (from a in db.kart_bilgileri
                             join b in db.kart_tipleri
                             on a.kart_tipi_id equals b.kart_tipi_id
                             where a.kart_no==kartno
                             select b.ucret).FirstOrDefault();
                var kart_bilgisi = (from a in db.kart_bilgileri where a.kart_no == kartno select a).FirstOrDefault();
                kart_bilgisi.bakiye = (Convert.ToDouble(kart_bilgisi.bakiye) - Convert.ToDouble(ucret)).ToString();               
                db.SaveChanges();
                LogTut(kartno, Convert.ToDouble(ucret),Convert.ToDouble(kart_bilgisi.bakiye));
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