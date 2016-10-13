using Application.Controllers.Services;
using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers.Employees
{
    public class DepartmentSummaryController : ObjectInstanceController
    {
        private DepartmentServices services;
        private ArrayServices arrayServices = new ArrayServices();
        private int year;
        // GET: DepartmentSummary

        public DepartmentSummaryController()
        {
            year = YEAR;
            services = new DepartmentServices(year);
        }

        public List<DataTable> DepartmentSummaryTables(int departmentID)
        {
            List<DataTable> tables = new List<DataTable>();
            var departments = services.getDepartment(departmentID);
            foreach (var d in departments)
            {
                if (services.hasSalaryDataForYear(d))
                {
                    tables.Add(DepartmentSummary(d));
                }
            }

            return tables;
        }

        public List<Comparison> EmployeeComparisonList()
        {
            List<Comparison> list = new List<Comparison>();
            var departments = services.getDepartment(0);
            foreach(var d in departments)
            {
                if (services.hasSalaryDataForYear(d))
                {
                    list.Add(EmployeeComparison(d));
                }
            }
            return list;
        }

        private Comparison EmployeeComparison(Department d)
        {
            Comparison item = new Comparison();
            item.SourceID = d.DepartmentID;
            item.Name = d.Name;
            item.Employee = services.BuildComparison(d);
            item.year = year;
            foreach(var e in item.Employee)
            {
                item.ActualPrev += e.ActualPrev;
                item.BudgetedPrev += e.BudgetedPrev;
                item.BudgetedCurrent += e.BudgetedCurrent;
            }

            return item;
        }

        private DataTable DepartmentSummary(Department d)
        {
            DataTable table = new DataTable();
            table.sourceID = d.DepartmentID;
            table.tableName = d.Name;
            table.dataList = services.CreateDepartmentSummary(d);
            table.Year = year;

            if (table.dataList == null)
            {
                table = null;
            }
            return table;
        }

        public List<EmployeeListViewModel> employeeListTable(int year, int departmentID)
        {

            this.year = year;
            services = new DepartmentServices(year);

            List<EmployeeListViewModel> list = new List<EmployeeListViewModel>();
            var departments = services.getDepartment(departmentID);
            foreach (var d in departments)
            {
                if (services.hasSalaryDataForYear(d))
                {
                    list.Add(employeeList(d));
                }
            }

            return list;
        }

        private EmployeeListViewModel employeeList(Department d)
        {
            EmployeeListViewModel model = new EmployeeListViewModel();
            model.Department = d;
            model.Employees = services.getEmployeeData(d);
            model.Year = year;

            return model;
        }

        public List<DataTable> serviceExpenseTables()
        {
            year = YEAR;
            var deptTables = DepartmentSummaryTables(0);

            List<DataTable> tables = new List<DataTable>();
            var departments = services.getDepartment(0).ToList();
            tables.Add(ServiceExpense(departments));

            return tables;
        }

        private DataTable ServiceExpense(List<Department> d)
        {
            services = new DepartmentServices(year);

            DataTable table = new DataTable();
            table.tableName = "Service Expenses";
            table.dataList = services.buildServiceExpense(d);
            table.Year = year;

            if (table.dataList == null)
            {
                table = null;
            }
            return table;
        }

        public List<DataTable> EmployeeCostTable()
        {
            year = YEAR;
            services = new DepartmentServices(year);

            List<DataTable> tables = new List<DataTable>();
            var departments = services.getSummaryDepartments();
            tables.Add(EmployeeCost(departments));

            return tables;
        }

        private DataTable EmployeeCost(IEnumerable<Department> d)
        {
            DataTable table = new DataTable();
            table.tableName = "Employee Costs";
            table.dataList = services.allDepartmentSummaries(d);
            table.Year = year;

            if (table.dataList == null)
            {
                table = null;
            }
            return table;
        }

    }
}