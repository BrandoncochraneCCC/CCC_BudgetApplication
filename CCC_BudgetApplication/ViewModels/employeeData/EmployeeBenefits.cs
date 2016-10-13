using Application.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.ViewModels.employeeData
{
    public class EmployeeBenefits
    {
        public bool hasVacation { get; set; }
        public bool hasSTD { get; set; }
        public bool hasFamily { get; set; }
        public bool hasIndividual { get; set; }
        public DataTable BenefitTable { get; set; }
        public Employee Employee { get; set; }
        public int deductionID { get; set; }
        public IEnumerable<SelectListItem> family { get; set; }
        public IEnumerable<SelectListItem> individual { get; set; }
        public IEnumerable<SelectListItem> std { get; set; }
        [DisplayFormat(DataFormatString = "{0:p}", ApplyFormatInEditMode = true)]
        public IEnumerable<SelectListItem> vacation { get; set; }

       
    }
}