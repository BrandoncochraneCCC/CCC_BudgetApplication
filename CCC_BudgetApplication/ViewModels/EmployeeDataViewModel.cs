using Application.Models;
using Application.ViewModels.employeeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.ViewModels
{
    public class EmployeeDataViewModel
    {
        public string EmployeeName { get; set; }
        public int deptID { get; set; }
        public int year { get; set; }
        public DataTable BudgetedSalaryTable { get; set; }
        public InformationTable InformationTable { get; set; }
        public DataTable SalaryTable { get; set; }
        public Deductions DeductionTable { get; set; }
        public DataTable RaiseTable { get; set; }
        public EmployeeBenefits BenefitTable { get; set; }
        public DataTable TargetTable { get; set; }
        public List<EmployeeRaise> EmployeeRaise { get; set; }

    }
}