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
    
    public partial class Program
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Program()
        {
            this.ProgramSections = new HashSet<ProgramSection>();
        }
    
        public int ProgramID { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }
        public Nullable<int> CounsellingGroupTypeID { get; set; }
    
        public virtual CounsellingGroupType CounsellingGroupType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProgramSection> ProgramSections { get; set; }
    }
}
