using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Yemekhane_Gecis_Sistemi.ViewModels
{
    public class KartKullanicilari
    {
        [Required(ErrorMessage = "Tc Kimlik No Girilmedi...!")]
        [Display(Name ="Tc Kimlik No")]
        [MaxLength(11,ErrorMessage ="Tc Kimlik Numarası 11 Karakterli Olmalıdır")]
        [MinLength(11, ErrorMessage = "Tc Kimlik Numarası 11 Karakterli Olmalıdır")]
        
        public string TcKimlikNo { get; set; }

        [Required(ErrorMessage = "Email Girilmedi...!")]
        public string EMail { get; set; }

        [Required(ErrorMessage = "Ad Bilgisi Girilmedi...!")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad Bilgisi Girilmedi...!")]
        public string Soyad { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }

        [Required(ErrorMessage ="Kategori Seçiniz...!")]
        public int KartTipiId { get; set; }

        [Required(ErrorMessage = "Birim Seçiniz...!")]
        public int BirimId { get; set; }

        [Required(ErrorMessage = "Unvan Seçiniz...!")]
        public int UnvanId { get; set; }

        [Required(ErrorMessage = "Kart No Girilmedi...!")]
        [MaxLength(8, ErrorMessage = "Kart No 8 Karakterli Olmalıdır")]
        [MinLength(8, ErrorMessage = "Kart No 8 Karakterli Olmalıdır")]
        public string KartNo { get; set; }
        public string KayitTarihi { get; set; }
        public string GüncellemeTarihi { get; set; }

        public string SonGecerlilikTarihi { get; set; }

        public List<SelectListItem> KartListesi { get; set; }

        public List<SelectListItem> BirimListesi { get; set; }

        public List<SelectListItem> UnvanListesi { get; set; }

        public string KullaniciMesaji { get; set; }

        public string Baslik { get; set; }

        public int KullaniciID { get; set; }

    }
}