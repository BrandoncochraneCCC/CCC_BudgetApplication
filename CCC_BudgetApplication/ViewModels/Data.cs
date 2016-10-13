using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class Data
    {
        public int SourceID { get; set; }
        public string SourceName { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal Value { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{MM-dd-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public int Year { get; set; }

        public string Action { get; set; }
        public string Controller { get; set; }

    }
}