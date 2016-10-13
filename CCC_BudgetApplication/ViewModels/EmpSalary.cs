using Application.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class EmpSalary
    {

        public string name { get; set; }
        public int SourceID { get; set; }
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        public decimal BudgetedPrev { get; set; }
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        public decimal ActualPrev { get; set; }
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        public decimal BudgetedCurrent { get; set; }

        public EmpSalary(string first, string last, decimal? BudgetedPrev, decimal? ActualPrev, decimal? BudgetedCurrent, int SourceID)
        {
            name = first + " " + last;
            this.BudgetedPrev = (decimal)BudgetedPrev;
            this.ActualPrev = (decimal)ActualPrev;
            this.BudgetedCurrent = (decimal)BudgetedCurrent;
            this.SourceID = SourceID;
        }


    }
}