using Application.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class Comparison
    {
        public string Name { get; set; }
        public int SourceID { get; set; }
        public List<EmpSalary> Employee { get; set; }
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        public decimal BudgetedPrev { get; set; }
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        public decimal BudgetedCurrent { get; set; }
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        public decimal ActualPrev { get; set; }
        [DisplayFormat(DataFormatString = "{0:p2}", ApplyFormatInEditMode = true)]
        public decimal percentDiffPrevYear { get; set; }
        [DisplayFormat(DataFormatString = "{0:p2}", ApplyFormatInEditMode = true)]
        public decimal PercentDiffCurrentPrevActual { get; set; }

        public int year { get; set; }

        public Comparison()
        {
            Name = "";
            SourceID = 0;
            Employee = null;
            BudgetedPrev = 0;
            BudgetedCurrent = 0;
            ActualPrev = 0;
            year = 0;
        }

        public Comparison(string Name, decimal? BudgetedPrev, decimal BudgetedCurrent, decimal? ActualPrev, int year, int SourceID)
        {
            this.Name = Name;
            this.SourceID = SourceID;
            Employee = null;
            this.BudgetedPrev = (decimal)BudgetedPrev;
            this.BudgetedCurrent = BudgetedCurrent;
            this.ActualPrev = (decimal)ActualPrev;
            this.year = year;
            percentDiffPrevYear = calculatePrevYearPercent(this.ActualPrev, this.BudgetedPrev);
            PercentDiffCurrentPrevActual = calculatePrevYearActualPercent(this.ActualPrev, this.BudgetedCurrent);
        }

        private decimal calculatePrevYearPercent(decimal actual, decimal budgeted)
        {
            if(budgeted != 0)
            {
                return (actual / budgeted) - 1;
            }
            else
            {
                return 0;
            }
        }

        private decimal calculatePrevYearActualPercent(decimal actual, decimal budgeted)
        {
            if (actual != 0)
            {
                return (budgeted / actual) - 1;
            }
            else
            {
                return 0;
            }
        }

    }
}