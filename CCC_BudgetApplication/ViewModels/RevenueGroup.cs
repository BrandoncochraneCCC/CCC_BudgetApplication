using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class RevenueGroup
    {
        public int Year { get; set; }
        public List<DataLine> Revenues { get; set; }
        public string Header { get; set; }

    }
}