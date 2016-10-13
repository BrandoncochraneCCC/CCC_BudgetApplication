using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class QueryableData
    {
        public int ID { get; set; }
        public decimal Value { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int parentID { get; set; }
    }
}