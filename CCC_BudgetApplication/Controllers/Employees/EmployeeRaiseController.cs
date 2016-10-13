using Application.Controllers.Queries;
using Application.Controllers.Services;
using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Controllers.Employees
{
    public class EmployeeRaiseController
    {
        private int year;
        private ArrayServices arrayServices = new ArrayServices();
        private EmployeeServices services;
        private EmployeeQueries queries;

        public EmployeeRaiseController(int year)
        {
            this.year = year;
            services = new EmployeeServices(year);
            queries = new EmployeeQueries(year);
        }

        public List<DataLine> RaiseDataLines(DataTable salaryTable, Employee e)
        {
            List<DataLine> list = new List<DataLine>();
            var raise = queries.getEmployeeRaise(e.EmployeeID);
            if(salaryTable != null)
            {
                var originalSalary = salaryTable.dataList.FirstOrDefault();
                decimal[] salaryData = null;
                if (originalSalary != null)
                {
                    salaryData = raiseData(e, originalSalary.Values, raise);
                    list = services.buildEmployeeDataList(e, salaryData);
                }


            }

            return list;
        }

        public List<EmployeeRaise> RaiseHistory(Employee e)
        {
            IQueryable<EmployeeRaise> result = queries.getRaiseHistory(e.EmployeeID);
            List<EmployeeRaise> list = new List<Models.EmployeeRaise>();
            if(result.FirstOrDefault() != null)
            {
                foreach (var item in result)
                {
                    list.Add(item);
                }
            }
            else
            {
                list = null;
            }
            return list;
        }

       

        private decimal[] raiseData(Employee e, decimal[] salary, IQueryable<EmployeeRaise> raise)
        {
            var currentSalary = (decimal)queries.getEmployeeSalary(e.EmployeeID).CurrentBudget;
            decimal[] newSalary = new decimal[12];
            foreach(var r in raise)
            {
                var raiseValue = RaiseValue(currentSalary, r);
                currentSalary += raiseValue;
                var monthlyValue = raiseValue / 12;

                for (var i = r.Date.Month - 1; i < 12; i++)
                {
                    newSalary[i] += monthlyValue;
                }
            }

            for(var i = 0; i < 12; i++)
            {
                if(salary[i] == 0)
                {
                    newSalary[i] = salary[i];
                }
                else
                {
                    newSalary[i] += salary[i];
                }
            }
            

            return newSalary;
        }

        private decimal RaiseValue(decimal current, EmployeeRaise r)
        {
            decimal value = r.Value;
                if (r.isPercent)
                {
                    var temp = r.Value / 100;
                    value = ((temp + 1) * current) - current;
                }

            return value;
        }

    }
}