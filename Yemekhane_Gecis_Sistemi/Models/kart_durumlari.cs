
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
    
public partial class kart_durumlari
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public kart_durumlari()
    {

        this.kart_bilgileri = new HashSet<kart_bilgileri>();

    }


    public int id { get; set; }

    public string durum_adi { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<kart_bilgileri> kart_bilgileri { get; set; }

}

}
