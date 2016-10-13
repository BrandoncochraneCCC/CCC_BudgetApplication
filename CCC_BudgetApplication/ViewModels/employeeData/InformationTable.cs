using Application.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.ViewModels.employeeData
{
    public class InformationTable
    {
        public Employee informationTable { get; set; }

        public IEnumerable<SelectListItem> departments { get; set; }
        public IEnumerable<SelectListItem> types { get; set; }

    }
}