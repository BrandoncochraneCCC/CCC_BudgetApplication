using Application.Controllers.Queries;
using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Application.Controllers.Services
{
    public class DepartmentServices
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();

        private ArrayServices arrayServices = new ArrayServices();
        private EmployeeServices empServices;
        private int year;
        private EmployeeQueries queries;

        public DepartmentServices(int year)
        {
            this.year = year;
            queries = new EmployeeQueries(year);
            empServices = new EmployeeServices(year);
        }

        public IEnumerable<Department> getDepartment(int departmentID)
        {
            return queries.getDepartment(departmentID);
        }

        public bool hasSalaryDataForYear(Department d)
        {
            return queries.hasSalaryDataForYear(d);
        }

        public List<EmpSalary> BuildComparison(Department d)
        {
            List<EmpSalary> list = new List<EmpSalary>();
            var employees = queries.departmentEmployees(d.DepartmentID);

            foreach(var e in employees)
            {
                var s = queries.getEmployeeSalary(e.EmployeeID);
                if(s != null)
                {
                    var obj = new EmpSalary(e.FirstName, e.LastName, s.PrevBudget, s.PrevActual, s.CurrentBudget, e.EmployeeID);
                    list.Add(obj);
                }            
            }

            return list; 
        }

        public List<DataLine> CreateDepartmentSummary(Department d)
        {
            List<DataLine> list = new List<DataLine>();
            var employees = queries.departmentEmployees(d.DepartmentID);

            decimal[] salary = new decimal[12];
            decimal[] cpp = new decimal[12];
            decimal[] ei = new decimal[12];
            decimal[] rrsp = new decimal[12];
            decimal[] other = new decimal[12];
            decimal[] benefits = new decimal[12];
            decimal[] vacation = new decimal[12];
            decimal[] std = new decimal[12];

            try
            {
                foreach (var e in employees)
                {
                    var empData = empServices.createEmployeeDataList(e);
                    if (empData != null)
                    {
                        salary = arrayServices.combineArrays(salary, empData[0].Values);
                        cpp = arrayServices.combineArrays(cpp, empData[1].Values);
                        ei = arrayServices.combineArrays(ei, empData[2].Values);
                        rrsp = arrayServices.combineArrays(rrsp, empData[3].Values);
                        other = arrayServices.combineArrays(other, empData[4].Values);
                        benefits = arrayServices.combineArrays(benefits, empData[5].Values);
                        vacation = arrayServices.combineArrays(vacation, empData[6].Values);
                        std = arrayServices.combineArrays(std, empData[7].Values);
                    }

                }


                list.Add(createDataLine("Salary", salary));
                list.Add(createDataLine("CPP", cpp));
                list.Add(createDataLine("EI", ei));
                list.Add(createDataLine("RRSP", rrsp));
                list.Add(createDataLine("Other", other));
                list.Add(createDataLine("Benefits", benefits));
                list.Add(createDataLine("Vacation", vacation));
                list.Add(createDataLine("STD", std));
            }
            catch(Exception ex)
            {
                log.Error("department exception", ex);
            }
            

            return list;
        }

        private Data createData(Department d)
        {
            Data data = new Data();
            data.Action = "DepartmentSummary";
            data.Controller = "Employees";
            data.SourceID = d.DepartmentID;
            data.SourceName = d.Name;
            return data;
        }
        private DataLine createDataLine(string name, decimal[] values)
        {
            DataLine line = new DataLine();
            line.Name = name;
            line.Values = values;

            return line;
        }

        private DataLine createDataLine(string name, decimal[] values, string action, string controller)
        {
            DataLine line = new DataLine();
            line.Name = name;
            line.Values = values;
            line.Action = action;
            line.Controller = controller;

            return line;
        }


        private DataLine createHeaderLine(Data[] values, string name = "")
        {
            DataLine line = new DataLine();
            line.Name = name;
            line.headerLine = values;

            return line;
        }

        public List<Employee> getEmployeeData(Department d)
        {

            List<Employee> result = new List<Employee>();
            try
            {
                var emp = queries.departmentEmployees(d.DepartmentID);
                var list = emp.Where(e => e.TypeID != BursariesController.RESIDENTTYPEID).Select(e => e).ToList();
                foreach (var item in list)
                {
                    var hasSalary = item.Salaries.Where(x => x.Year == year).Select(x => x).SingleOrDefault();
                    if (item.StartDate != null && item.StartDate.Value.Year <= year || hasSalary != null)
                    {
                        result.Add(item);
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error("employee data error", ex);
            }
            
            return result;
        }

        public List<DataLine> allDepartmentSummaries(IEnumerable<Department> departments)
        {
            List<DataLine> list = new List<DataLine>();
            var size = departments.Count();
            Data[] names = new Data[size];
            decimal[] salary = new decimal[size];
            decimal[] cpp = new decimal[size];
            decimal[] ei = new decimal[size];
            decimal[] rrsp = new decimal[size];
            decimal[] other = new decimal[size];
            decimal[] benefits = new decimal[size];
            decimal[] vacation = new decimal[size];
            decimal[] std = new decimal[size];
            try
            {
                var count = 0;
                foreach (var d in departments)
                {

                    decimal[] data = departmentData(d);

                    names[count] = createData(d);
                    salary[count] += data[0];
                    cpp[count] += data[1];
                    ei[count] += data[2];
                    rrsp[count] += data[3];
                    other[count] += data[4];
                    benefits[count] += data[5];
                    vacation[count] += data[6];
                    std[count] += data[7];
                    count++;

                }

                list.Add(createHeaderLine(names));
                list.Add(createDataLine("Salary", salary));
                list.Add(createDataLine("CPP", cpp));
                list.Add(createDataLine("EI", ei));
                list.Add(createDataLine("RRSP", rrsp));
                list.Add(createDataLine("Other", other));
                list.Add(createDataLine("Benefits", benefits));
                list.Add(createDataLine("Vacation", vacation));
                list.Add(createDataLine("STD", std));
            }
            catch(Exception ex)
            {
                log.Warn("department summary exception", ex);
            }
            

            return list;
        }


        public IEnumerable<Department> getSummaryDepartments()
        {
            return queries.getSummaryDepartments();
        }        

        public IEnumerable<Department> GetDepartments()
        {
            return queries.getDepartment();

        }
        public List<DataLine> buildServiceExpense()
        {
            var result = queries.getDepartment().ToList();
            return buildServiceExpense(result);
        }
        public List<DataLine> buildServiceExpense(List<Department> departments)
        {
            List<DataLine> list = new List<DataLine>();

            decimal[] salary = new decimal[12];
            decimal[] cpp = new decimal[12];
            decimal[] ei = new decimal[12];
            decimal[] rrsp = new decimal[12];
            decimal[] benefits = new decimal[12];
            decimal[] std = new decimal[12];

            try
            {
                foreach (var d in departments)
                {
                    var summary = CreateDepartmentSummary(d);
                    salary = arrayServices.combineArrays(salary, summary[0].Values);
                    cpp = arrayServices.combineArrays(cpp, summary[1].Values);
                    ei = arrayServices.combineArrays(ei, summary[2].Values);
                    rrsp = arrayServices.combineArrays(rrsp, summary[3].Values);
                    benefits = arrayServices.combineArrays(benefits, summary[4].Values);
                    std = arrayServices.combineArrays(std, summary[6].Values);
                }

                list.Add(createDataLine("Full-Time Salaries", salary, "EmployeeCost", "Employees"));
                list.Add(createDataLine("Employee CPP", cpp, "EmployeeCost", "Employees"));
                list.Add(createDataLine("Employee EI", ei, "EmployeeCost", "Employees"));
                list.Add(createDataLine("Employee Benefits", arrayServices.combineArrays(rrsp, std), "EmployeeCost", "Employees"));
                //parking
                //secretarial services
                //bursaries
                //contract services
                //other service expenses
            }
            catch (Exception ex)
            {
                log.Warn("department service expense exception", ex);
            }
            

            return list;

        }

        private decimal departmentSalary(int ID)
        {
            decimal value = 0;
            return value;
        }

        private decimal[] departmentData(Department d)
        {
            decimal[] values = new decimal[8];
            try
            {
                var employees = queries.currentDeptEmployees(d.DepartmentID);

                foreach (var e in employees)
                {
                    decimal[] salary = new decimal[12];
                    salary = empServices.getSalaryData(e);
                    values[0] += arrayServices.sumArray(salary);
                    values[1] += arrayServices.sumArray(empServices.calculateCPP(salary));
                    values[2] += arrayServices.sumArray(empServices.calculateEI(e, salary));
                    values[3] += arrayServices.sumArray(empServices.calculateRRSP(e, salary));
                    values[4] += arrayServices.sumArray(empServices.calculateOther(e, salary));
                    values[5] += arrayServices.sumArray(empServices.calculateBenefits(e, salary));
                    values[6] += arrayServices.sumArray(empServices.calculateVacation(e, salary));
                    values[7] += arrayServices.sumArray(empServices.calculateSTD(e, salary));

                }
            } catch(Exception ex)
            {
                log.Info("department data", ex);
            }


            return values;
        }

        private decimal[] employeeSalaryData(Employee e)
        {
            decimal[] values = new decimal[8];
            try
            {
                decimal[] salary = new decimal[12];
                salary = empServices.getSalaryData(e);
                values[0] += arrayServices.sumArray(salary);
                values[1] += arrayServices.sumArray(empServices.calculateCPP(salary));
                values[2] += arrayServices.sumArray(empServices.calculateEI(e, salary));
                values[3] += arrayServices.sumArray(empServices.calculateRRSP(e, salary));
                values[4] += arrayServices.sumArray(empServices.calculateOther(e, salary));
                values[5] += arrayServices.sumArray(empServices.calculateBenefits(e, salary));
                values[6] += arrayServices.sumArray(empServices.calculateVacation(e, salary));
                values[7] += arrayServices.sumArray(empServices.calculateSTD(e, salary));
            }
            catch(Exception ex)
            {
                log.Debug("employee salary data array population exception", ex);
            }


            return values;
        }

        public DataLine EmployeeTypeCost(int typeID)
        {
            DataLine line = new DataLine();

            return line;
        }

    }
}