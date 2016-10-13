using Application.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class DataLine
    {
        public int SourceID { get; set; }

        public int ParentID { get; set; }

        public string Name { get; set; }
        public Data[] headerLine { get; set; }

        public decimal[] Values { get; set; }
        public string[] stringValues { get; set; }

        public bool hasChildren { get; set; }

        public string tableName { get; set; }

        public string Controller { get; set; }
        public string Action { get; set; }

        public string FieldName { get; set; }

        public bool isPercent { get; set; }
        public bool isAverage { get; set; }

        [DisplayFormat(DataFormatString = "{0:p2}", ApplyFormatInEditMode = true)]
        public decimal[] percentValues { get; set; }
        public string viewClass { get; set; }
        public int year { get; set; }
        public bool isContractor { get; set; }
        public bool usedInGSTCalculation { get; set; }

        public DataLine()
        {
            Values = new Decimal[12];
            isContractor = false;
        }

        public DataLine(string name, decimal[] values, string viewClass = "")
        {
            Name = name;
            Values = values;
            this.viewClass = viewClass;
        }
    }
}