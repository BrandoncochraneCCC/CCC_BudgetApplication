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
    
    public partial class GAExpense
    {
        public int GAExpenseID { get; set; }
        public int GroupID { get; set; }
        public decimal Value { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<int> AccountNum { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual GAGroup GAGroup { get; set; }
    }
}
