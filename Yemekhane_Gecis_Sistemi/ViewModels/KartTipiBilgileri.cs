using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Yemekhane_Gecis_Sistemi.ViewModels
{
    public class KartTipiBilgileri
    {
        [Required(ErrorMessage ="Kart Tipi Bilgisini Giriniz...!")]
        public string KartTipi { get; set; }

        [Required(ErrorMessage = "Ucret Bilgisini Giriniz...!")]
        public string Ucret { get; set; }
        public string KayitTarihi { get; set; }
        public string GuncellemeTarihi { get; set; }
        public string KullaniciMesaji { get; set; }
    }
}