using Application.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class Resident
    {
        public string Name { get; set; }
        public Employee Employee { get; set; }
        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal[] GroupTargets { get; set; }

        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal[] Targets { get; set; }

        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal[] Bursaries { get; set; }

        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal[] Clawbacks { get; set; }

        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal[] Totals { get; set; }
        public int residentID { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public int Year { get; set; }
    }
}