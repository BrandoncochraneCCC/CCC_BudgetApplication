using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class CostOfServiceViewModel
    {
        public List<CostOfService> data { get; set; }
        public string name { get; set; }
        public CostOfService total { get; set; }
        public CostOfService percentTotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]

        public decimal weighted { get; set; }

    }
}