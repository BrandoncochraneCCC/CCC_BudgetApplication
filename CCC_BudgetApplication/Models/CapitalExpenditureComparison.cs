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
    
    public partial class CapitalExpenditureComparison
    {
        public int CapitalExpenditureComparisonID { get; set; }
        public int CapitalExpenditureID { get; set; }
        public Nullable<decimal> PrevBudget { get; set; }
        public Nullable<decimal> PrevActual { get; set; }
        public int Year { get; set; }
    
        public virtual CapitalExpenditure CapitalExpenditure { get; set; }
    }
}
