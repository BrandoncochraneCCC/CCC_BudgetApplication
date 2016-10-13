using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Controllers.Queries
{
    public class EmployeeQueries
    {
        BudgetDataEntities db = new ObjectInstanceController().db;
        int year;

        public EmployeeQueries(int year)
        {
            this.year = year;
        }

        public Employee getSingleEmployee(int id)
        {
            return db.Employees.Where(r => r.EmployeeID == id).Select(r => r).FirstOrDefault();
        }

        public int getWorkHoursPerYear()
        {
            return db.HoursPerYears.Where(x => x.Year == year).Select(x => x.Hours).SingleOrDefault();
        }
        public Salary getEmployeeSalary(int id)
        {
            return db.Salaries.Where(s => s.EmployeeID == id && s.Year == year).Select(s => s).FirstOrDefault();
        }

        public IQueryable<Salary> getEmployeeMonthlySalary(int id)
        {
            return db.Salaries.Where(s => s.EmployeeID == id && s.Year == year).Select(s => s);
        }

        public int getHoursPerYear()
        {
            return db.HoursPerYears.Where(h => h.Year == year).Select(h => h.Hours).FirstOrDefault();
        }

        public int getHoursPerMonth()
        {
            return db.HoursPerYears.Where(h => h.Year == year).Select(h => h.Hours).FirstOrDefault() / 12;
        }

        public decimal previousBudgetSalary(Salary s)
        {
            decimal result = 0;
            if (s != null)
            {
               var data = db.Salaries.Where(x => s.SalaryID == x.SalaryID && x.Year == year).Select(x => s.PrevBudget);
                if (data.FirstOrDefault() != null)
                {
                    result = (decimal)data.FirstOrDefault();
                }

            }

            return result;

        }

        public IQueryable<InternTarget> getInternData(Employee e)
        {
            return db.InternTargets.Where(x => x.EmployeeID == e.EmployeeID && x.Date.Year == year).Select(x => x);
        }


        public decimal getMonthlyInternData(IQueryable<InternTarget> data, int month)
        {
            return data.Where(x => x.Date.Month == month).Select(x => x.Hour).FirstOrDefault();
        }

        public IQueryable<Employee> getDepartmentEmployeesByType(IQueryable<Employee> data, int typeID)
        {
            return data.Where(x => x.TypeID == typeID).Select(x => x);
        }


        public IQueryable<Employee> getEmployeeByType(int typeID)
        {
            return db.Employees.Where(e => e.TypeID == typeID).Select(e => e);
        }

        public decimal deductionMax(int DEDUCTIONTYPEID)
        {
            decimal result = 0;
            var deduction = db.DeductionLists.Where(d => d.DeductionTypeID == DEDUCTIONTYPEID && d.Year == year).Select(d => d).SingleOrDefault();
            if(deduction != null)
            {
                result = (decimal)deduction.Max;
            }
            return result;
        }

        public decimal deductionRate(int DEDUCTIONTYPEID)
        {
            decimal value = 0;
            var result = db.DeductionLists.Where(d => d.DeductionTypeID == DEDUCTIONTYPEID && d.Year == year).Select(d => d).FirstOrDefault();
            if(result != null)
            {
                value = (decimal)result.Rate;
            }
            return value;
        }

        public decimal previousActualSalary(Salary s)
        {
            decimal result = 0;
            if (s != null)
            {
                var data = db.Salaries.Where(x => s.SalaryID == x.SalaryID && x.Year == year).Select(x => s.PrevActual).FirstOrDefault();
                if(data != null)
                {
                    result = (decimal)data;

                }
            }

            return result;
        }

        public decimal currentBudgetSalary(Salary s)
        {
            decimal result = 0;
            if (s != null && s.CurrentBudget != null)
            {
                result = (decimal)db.Salaries.Where(x => s.SalaryID == x.SalaryID && x.Year == year).Select(x => s.CurrentBudget).FirstOrDefault();
            }

            return result;
        }
        public decimal hourlyRate(Salary s)
        {
            decimal result = 0;
            if (s != null)
            {
                result = (decimal)db.Salaries.Where(x => s.SalaryID == x.SalaryID && x.Year == year).Select(x => s.HourlyRate).FirstOrDefault();
            }

            return result;
        }

        public DeductionList deductionData(int deductionTypeID)
        {
            return db.DeductionLists.Where(d => d.DeductionTypeID == deductionTypeID && d.Year == year).Select(d => d).SingleOrDefault();
        }

        public EmployeeDeduction getEmpDed(int empID, int dedTypeID)
        {
            return db.EmployeeDeductions.Where(e => e.EmployeeID == empID && e.DeductionTypeID == dedTypeID && e.Year == year).Select(e => e).FirstOrDefault();
        }


        public IEnumerable<Department> getDepartment()
        {
            return getDepartment(0);
        }

        public IEnumerable<Department> getSummaryDepartments()
        {
            return db.Departments.Where(d => d.DepartmentID < 9).Select(d => d).ToList();
        }

        public IEnumerable<Department> getDepartment(int departmentID)
        {
            if (departmentID == 0)
            {
                return db.Departments.Select(d => d).ToList();
            }
            else
            {
                return db.Departments.Where(d => d.DepartmentID == departmentID).Select(d => d).ToList();
            }
        }

        public Department getSimgleDepartment(int departmentID)
        {
            return db.Departments.Where(d => d.DepartmentID == departmentID).Select(d => d).FirstOrDefault();
        }

        public IEnumerable<Employee> departmentEmployees(int departmentID)
        {
            return db.Employees.Where(e => e.DepartmentID == departmentID).Select(e => e);
        }

        public IEnumerable<Salary> allsalariesforCurrentYear()
        {
            return allsalariesforYear(year);
        }

        public IEnumerable<Salary> allsalariesforYear(int targetYear)
        {
            return db.Salaries.Where(s => s.Year == targetYear).Select(s => s);
        }

        public IEnumerable<Employee> currentDeptEmployees(int departmentID)
        {
            var deptEmp = departmentEmployees(departmentID);
            var salaries = allsalariesforCurrentYear();
            return deptEmp.Where(e => e.Salaries.Where(s => s.Year == year).Select(s => s) != null).Select(e => e);
        }

        public IQueryable<EmployeeRaise> getEmployeeRaise(int employeeID)
        {
            return db.EmployeeRaises.Where(r => r.EmployeeID == employeeID && r.Date.Year == year).Select(r => r);
        }

        public IQueryable<EmployeeRaise> getRaiseHistory(int employeeID)
        {
            return db.EmployeeRaises.Where(r => r.EmployeeID == employeeID).Select(r => r);
        }

        public IQueryable<EmployeeRaise> getRaiseHistoryPerYear(int employeeID)
        {
            return db.EmployeeRaises.Where(r => r.EmployeeID == employeeID && r.Date.Year == year).Select(r => r);
        }
        public bool hasSalaryDataForYear(Department d)
        {
            var salaries = allsalariesforCurrentYear().Select(s => s);
            var deptEmp = departmentEmployees(d.DepartmentID);
            foreach (var employee in deptEmp)
            {
                if (salaries.Any(salary => salary.EmployeeID == employee.EmployeeID))
                {
                    return true;
                }
            }
            return false;

        }

        public bool hasSalary(Employee employee)
        {
            var salary = getEmployeeSalary(employee.EmployeeID);
            if (salary != null)
            {
                return true;
            }
            return false;
        }

        public bool hasRaise(Employee employee)
        {
            var hasRaise = false;
            var raise = db.EmployeeRaises.Where(r => r.EmployeeID == employee.EmployeeID && r.Date.Year == year).Select(r => r).FirstOrDefault();
            if (raise != null)
            {
                hasRaise = true;
            }
            return hasRaise;
        }

        public bool hasVacation(Employee employee)
        {
            var hasRaise = false;
            var vacation = db.EmployeeDeductions.Where(r => r.EmployeeID == employee.EmployeeID && r.Year == year && r.DeductionTypeID == 9).Select(r => r).FirstOrDefault();
            if (vacation != null)
            {
                hasRaise = true;
            }
            return hasRaise;
        }
        public IQueryable<InternTarget> getInternTarget(int employeeID)
        {
            return db.InternTargets.Where(t => t.EmployeeID == employeeID && t.Date.Year == year).Select(t => t);
        }
        public IQueryable<ResidentTarget> getResidentTarget(int employeeID)
        {
            return db.ResidentTargets.Where(t => t.EmployeeID == employeeID && t.Date.Year == year).Select(t => t);
        }
        public IQueryable<GroupTherapyTarget> getGroupTarget(int employeeID)
        {
            return db.GroupTherapyTargets.Where(t => t.EmployeeID == employeeID && t.Date.Year == year).Select(t => t);
        }
        public IQueryable<EmployeeTarget> getEmployeeTarget(int employeeID)
        {
            return db.EmployeeTargets.Where(t => t.EmployeeID == employeeID && t.Year == year).Select(t => t);
        }
        public bool hasTarget(Employee employee)
        {
            var hasTarget = false;
            if(employee.DepartmentID == ObjectInstanceController.COUNSELLING_DEPT_ID)
            {
                switch (employee.TypeID)
                {
                    case 6:
                        if (getInternTarget(employee.EmployeeID) != null || getGroupTarget(employee.EmployeeID) != null)
                        {
                            hasTarget = true;
                        };

                        break;
                    case 4:
                        if (getResidentTarget(employee.EmployeeID) != null || getGroupTarget(employee.EmployeeID) != null)
                        {
                            hasTarget = true;
                        };
                        break;
                    case 1:
                    case null:
                            if (getEmployeeTarget(employee.EmployeeID) != null)
                            {
                                hasTarget = true;
                            };
                        break;

                }
            }
            

            return hasTarget;
        }

        public IQueryable<ProjectedFeePerHour> getAssumption(int ID)
        {
            return db.ProjectedFeePerHours.Where(a => a.EmployeeTypeID == ID && a.Date.Year == year).Select(a => a);
        }

        
        public IQueryable<Employee> getEmployeeByDepartmentAndType(int departmentID, int typeID)
        {
            return db.Employees.Where(e => e.DepartmentID == departmentID && e.TypeID == typeID).Select(e => e);
        }

        public UnchangingValue getSupervisionFee()
        {
            return db.UnchangingValues.Where(x => x.Year == year && x.Name.ToLower().Equals( "average supervision")).Select(x => x).SingleOrDefault();
        }
        public UnchangingValue getGroupFee()
        {
            return db.UnchangingValues.Where(x => x.Year == year && x.Name.ToLower().Equals("average group")).Select(x => x).SingleOrDefault();
        }
    }
}