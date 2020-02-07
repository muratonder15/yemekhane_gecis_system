using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Yemekhane_Gecis_Sistemi.ViewModels
{
    public class Unvanlar
    {
        public int UnvanId { get; set; }

        [Required(ErrorMessage = "Unvan Adı Boş Kaydedilemez...!")]
        public string UnvanAdi { get; set; }
        public string Baslik { get; set; }

        public string KullaniciMesaji { get; set; }
    }
}