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
    
    public partial class ucretler
    {
        public int ucret_id { get; set; }
        public Nullable<int> kart_tipi_id { get; set; }
        public string ucret { get; set; }
        public Nullable<System.DateTime> kayit_tarihi { get; set; }
        public Nullable<System.DateTime> guncelleme_tarihi { get; set; }
    
        public virtual kart_tipleri kart_tipleri { get; set; }
    }
}