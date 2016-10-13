using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class ProgramBudgetViewModel
    {
        public string name { get; set; }
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]

        public decimal value { get; set; }
        [DisplayFormat(DataFormatString = "{0:p0}", ApplyFormatInEditMode = true)]

        public decimal percent { get; set; }
        public string viewClass { get; set; }


        public ProgramBudgetViewModel(string name, decimal value, decimal percent, string viewClass ="")
        {
            this.name = name;
            this.value = value;
            this.percent = percent;
            this.viewClass = viewClass;
        }

        public ProgramBudgetViewModel(string name, decimal value, string viewClass = "")
        {
            this.name = name;
            this.value = value;
            this.viewClass = viewClass;
        }
    }
}