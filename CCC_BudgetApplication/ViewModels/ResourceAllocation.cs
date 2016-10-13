using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class ResourceAllocation
    {
        public string Header { get; set; }
        public List<ProgramBudgetViewModel> Revenues { get; set; }
        public List<ProgramBudgetViewModel> Expenses { get; set; }
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]

        public decimal Total { get; set; }
        [DisplayFormat(DataFormatString = "{0:p2}", ApplyFormatInEditMode = true)]

        public decimal Percentage { get; set; }
    }
}