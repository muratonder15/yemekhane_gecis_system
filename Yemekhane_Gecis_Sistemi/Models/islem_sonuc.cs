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
    
    public partial class islem_sonuc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public islem_sonuc()
        {
            this.gecis_loglari = new HashSet<gecis_loglari>();
        }
    
        public int sonuc_id { get; set; }
        public string islem_mesaji { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<gecis_loglari> gecis_loglari { get; set; }
    }
}
