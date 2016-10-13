using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetFramework.Models
{
    public class BudgetUser
    {
        public string UserID { get; set; }
        public string ReportsTo { get; set; }
        public string DisplayName { get; set; }
        public string Initials { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
    }
}