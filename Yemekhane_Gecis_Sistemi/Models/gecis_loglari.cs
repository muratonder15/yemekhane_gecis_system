
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
    
public partial class gecis_loglari
{

    public int log_id { get; set; }

    public Nullable<int> kullanici_id { get; set; }

    public Nullable<int> kart_id { get; set; }

    public Nullable<int> islem_tipi_id { get; set; }

    public Nullable<int> islem_sonuc_id { get; set; }

    public string ucret { get; set; }

    public string kalan_bakiye { get; set; }

    public string mesaj { get; set; }

    public Nullable<System.DateTime> kayit_tarihi { get; set; }



    public virtual islem_sonuc islem_sonuc { get; set; }

    public virtual islem_tipleri islem_tipleri { get; set; }

    public virtual kullanicilar kullanicilar { get; set; }

    public virtual kart_bilgileri kart_bilgileri { get; set; }

}

}
