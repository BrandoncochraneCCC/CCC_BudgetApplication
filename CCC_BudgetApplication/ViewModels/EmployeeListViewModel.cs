using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class EmployeeListViewModel
    {
        public Department Department { get; set; }
        public List<Employee> Employees { get; set; }
        public int Year { get; set; }
    }
}