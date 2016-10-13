using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.ViewModels
{
    public class YearInfo
    {
        public IEnumerable<SelectListItem> Vacation { get; set; }
        public IEnumerable<SelectListItem> STD { get; set; }
        public IEnumerable<SelectListItem> IndividualBenefit { get; set; }
        public IEnumerable<SelectListItem> FamilyBenfit { get; set; }
        public IEnumerable<SelectListItem> CPP { get; set; }
        public IEnumerable<SelectListItem> EI { get; set; }
        public IEnumerable<SelectListItem> RRSP { get; set; }
        public IEnumerable<SelectListItem> HoursPerYear { get; set; }
        public IEnumerable<SelectListItem> Bursary { get; set; }
        public IEnumerable<SelectListItem> Clawback { get; set; }
        public IEnumerable<SelectListItem> TotalBursary { get; set; }
        public bool IsLocked { get; set; }
        public IEnumerable<SelectListItem> AverageFeeCounsellor { get; set; }
        public IEnumerable<SelectListItem> AverageFeeFT { get; set; }
        public IEnumerable<SelectListItem> AverageFeePT { get; set; }
        public IEnumerable<SelectListItem> AverageFeeStudent { get; set; }
        public IEnumerable<SelectListItem> CostSupervision { get; set; }
    }
}