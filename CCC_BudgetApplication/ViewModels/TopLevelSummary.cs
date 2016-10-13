using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class TopLevelSummary
    {
        public DataTable Revenue { get; set; }
        public DataTable Service { get; set; }
        public DataTable General { get; set; }
        public DataTable totals { get; set; }
    }
}