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
    
    public partial class EmployeeRaise
    {
        public int EmployeeRaiseID { get; set; }
        public int EmployeeID { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Value { get; set; }
        public bool isPercent { get; set; }
        public Nullable<decimal> OldSalary { get; set; }
        public Nullable<decimal> NewSalary { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
