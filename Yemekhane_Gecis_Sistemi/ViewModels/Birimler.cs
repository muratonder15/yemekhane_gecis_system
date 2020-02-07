using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Yemekhane_Gecis_Sistemi.ViewModels
{
    public class Birimler
    {
        public int BirimId { get; set; }
        [Required(ErrorMessage ="Birim Adı Boş Kaydedilemez...!")]
        public string BirimAdi { get; set; }
        public string Baslik { get; set; }

        public string KullaniciMesaji { get; set; }
    }
}