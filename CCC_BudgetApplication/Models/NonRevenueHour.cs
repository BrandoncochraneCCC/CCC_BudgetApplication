//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Application.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NonRevenueHour
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NonRevenueHour()
        {
            this.NonRevenueHourDatas = new HashSet<NonRevenueHourData>();
        }
    
        public int NonRevenueHourID { get; set; }
        public string Name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NonRevenueHourData> NonRevenueHourDatas { get; set; }
    }
}
