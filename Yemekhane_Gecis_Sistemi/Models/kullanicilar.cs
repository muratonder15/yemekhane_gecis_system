//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Yemekhane_Gecis_Sistemi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class kullanicilar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public kullanicilar()
        {
            this.gecis_loglari = new HashSet<gecis_loglari>();
            this.kart_bilgileri = new HashSet<kart_bilgileri>();
        }
    
        public int kullanici_id { get; set; }
        public string kullanici_adi { get; set; }
        public string sifre { get; set; }
        public string ad { get; set; }
        public string soyad { get; set; }
        public string email { get; set; }
        public string tc { get; set; }
        public Nullable<System.DateTime> kayit_tarihi { get; set; }
        public Nullable<System.DateTime> guncelleme_tarihi { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<gecis_loglari> gecis_loglari { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kart_bilgileri> kart_bilgileri { get; set; }
    }
}
