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
    
    public partial class DeductionList
    {
        public int DeductionListID { get; set; }
        public int DeductionTypeID { get; set; }
        public int Year { get; set; }
        public Nullable<decimal> Max { get; set; }
        public Nullable<decimal> Rate { get; set; }
    
        public virtual DeductionType DeductionType { get; set; }
    }
}