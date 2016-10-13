using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class EmployeeView
    {
        public DataTable Interns { get; set; }
        public List<EmployeeListViewModel> EmployeeList { get; set; }
    }
}