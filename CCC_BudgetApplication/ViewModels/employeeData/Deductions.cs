using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.ViewModels.employeeData
{
    public class Deductions
    {

        public int rrspID { get; set; }
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        public decimal cppMax { get; set; }
        [DisplayFormat(DataFormatString = "{0:p2}", ApplyFormatInEditMode = true)]

        public decimal cppRate { get; set; }
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]

        public decimal eiMax { get; set; }
        [DisplayFormat(DataFormatString = "{0:p2}", ApplyFormatInEditMode = true)]

        public decimal eiRate { get; set; }
        [DisplayFormat(DataFormatString = "{0:p2}", ApplyFormatInEditMode = true)]

        public decimal rrspRate { get; set; }
        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]

        public decimal rrspMax { get; set; }
        public IEnumerable<SelectListItem> cpp { get; set; }
        public IEnumerable<SelectListItem> rrsp { get; set; }
        public IEnumerable<SelectListItem> ei { get; set; }

    }
}