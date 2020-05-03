using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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
        private AKS.ReaderManager mDevice = new AKS.ReaderManager();
        public void SistemLog(int kullanici_id, int islem_tipi_id, string mesaj)
        {
            sistem_log sistem_log_model = new sistem_log();
            sistem_log_model.islem_tarihi = DateTime.Now;
            sistem_log_model.kullanici_id = kullanici_id;
            sistem_log_model.islem_tipi_id = islem_tipi_id;
            sistem_log_model.mesaj = mesaj;
            sistem_log_model.ip = IpCek();
            db.sistem_log.Add(sistem_log_model);
            db.SaveChanges();
        }
        private string IpCek()
        {
            var webClient = new WebClient();

            string dnsString = webClient.DownloadString("http://checkip.dyndns.org");
            dnsString = (new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b")).Match(dnsString).Value;

            webClient.Dispose();
            return dnsString;
        }
        public void LogTut(string kartno, int islem_tipi, int islem_sonuc, double ucret, double kalan)
        {

            gecis_loglari gecisLog = new gecis_loglari();

            var kullanici_kodu = (from a in db.kart_bilgileri
                                  where a.kart_no == kartno && a.durum == 1
                                  select new { a.kullanici_id, a.ID }).FirstOrDefault();
            if (islem_tipi == 2 && islem_sonuc == 1)
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
            else if (islem_tipi == 2 && islem_sonuc == 2)
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
            var kart_kontrol = (from a in db.kart_bilgileri where a.kart_no == kartno select a).FirstOrDefault();

            if (kart_kontrol != null)
            {
                var kart_bilgisi = (from a in db.kart_bilgileri
                                    where
                                     a.kart_no == kartno &&
                                     a.durum == 1 &&
                                     a.son_gecerlilik_tarihi > DateTime.Now
                                    select a).FirstOrDefault();
                var son_kullanma_tarihi = (from a in db.kart_bilgileri
                                           where
                                            a.kart_no == kartno &&
                                            a.durum == 1 &&
                                            a.son_gecerlilik_tarihi < DateTime.Now
                                           select a).FirstOrDefault();
                if (kart_bilgisi != null)
                {
                    var kullanici = (from k in db.kullanicilar where k.kullanici_id == kart_bilgisi.kullanici_id select k).FirstOrDefault();
                    var ucret = (from a in db.kart_bilgileri
                                 join b in db.kart_tipleri
                                 on a.kart_tipi_id equals b.kart_tipi_id
                                 where a.kart_no == kartno && a.durum == 1
                                 select b.ucret).FirstOrDefault();
                    //var kart_bilgisi = (from a in db.kart_bilgileri where a.kart_no == kartno select a).FirstOrDefault();
                    if (Convert.ToDouble(kullanici.bakiye) >= Convert.ToDouble(ucret))
                    {
                        kullanici.bakiye = (Convert.ToDouble(kullanici.bakiye) - Convert.ToDouble(ucret)).ToString();
                        db.SaveChanges();
                        LogTut(kartno, 2, 1, Convert.ToDouble(ucret), Convert.ToDouble(kullanici.bakiye));
                    }
                    else
                    {
                        LogTut(kartno, 2, 2, Convert.ToDouble(ucret), Convert.ToDouble(kullanici.bakiye));
                    }

                }

                else if (son_kullanma_tarihi != null)
                {
                    ViewData["KullaniciMesaji"] = "Son kullanma tarihi geçmiş!!";
                }
                else
                {
                    ViewData["KullaniciMesaji"] = "Kart İptal!!";
                }
            }


            else
            {
                ViewData["KullaniciMesaji"] = "Geçersiz Kart!!";
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

        public string KartOku()
        {
            string txtBaudrate = "115200";
            string txtComport = "";
            string txtKartNo = "";
            if (!mDevice.IsReady)
            {
                for (int i = 1; i < 17; i++)
                {

                    txtComport = "COM" + i.ToString();
                    if (mDevice.OpenPort(txtComport, Convert.ToInt32(txtBaudrate)))
                    {

                        txtKartNo = KartDataGonder(txtComport);
                        //TempData["KartNo"]= txtKartNo;                                           
                        mDevice.Close();
                        //return Redirect("/Home/KullaniciEkle");
                        return txtKartNo;
                        //break;
                    }
                }

            }
            return "0";
            //return Redirect("/Home/KullaniciEkle");
        }

        private string KartDataGonder(string port)
        {
            AKS.Reader mSelectedReader = null;

            if (mDevice.Readers.Contains(port)) mSelectedReader = mDevice.Readers[port];

            else return "0";

            string txtReceiveData = "";
            string RetValue = "";
            string txtReaderId = "150";
            string txtCommand = "11";
            string txtParameter = "";
            string txtTimeOut = "500";
            if (mSelectedReader.SendData(Convert.ToByte(txtReaderId), Convert.ToByte(txtCommand), txtParameter, out RetValue, Convert.ToInt32(txtTimeOut)))
            {
                if (RetValue != "a11")
                {
                    txtReceiveData = RetValue.Substring(3, 8);
                }
                //txtReceiveData.Text = RetValue.Substring(3,8); //3. karakterden sonraki kart id'sini çeker.
                return txtReceiveData;

            }
            else
            {

                return txtReceiveData = "Error";
            }
        }

    }
}