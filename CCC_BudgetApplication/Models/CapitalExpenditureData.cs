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
    
    public partial class CapitalExpenditureData
    {
        public int CapitalExpenditureDataID { get; set; }
        public int CapitalExpenditureID { get; set; }
        public decimal Value { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual CapitalExpenditure CapitalExpenditure { get; set; }
    }
}
