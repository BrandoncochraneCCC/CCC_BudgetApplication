using Application.Controllers;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class CounsellingServicesQueries
    {
        BudgetDataEntities db = new ObjectInstanceController().db;
        private int year;

        public CounsellingServicesQueries(int year)
        {
            this.year = year;
        }

        public IEnumerable<ResidentTarget> getResidentTargets(Employee e)
        {
            return db.ResidentTargets.Where(t => t.EmployeeID == e.EmployeeID && t.Date.Year == year).Select(t => t);
        }

        public IEnumerable<InternTarget> getInternTargets(Employee e)
        {
            return db.InternTargets.Where(t => t.EmployeeID == e.EmployeeID && t.Date.Year == year).Select(t => t);
        }

        public decimal getMonthlyTargetHours(IEnumerable<InternTarget> t, int month)
        {
            return t.Where(x => x.Date.Month == month).Select(x => x.Hour).FirstOrDefault();
        }

        public decimal getMonthlyTargetHours(IEnumerable<ResidentTarget> t, int month)
        {
            return t.Where(x => x.Date.Month == month).Select(x => x.Hour).FirstOrDefault();
        }

        public IEnumerable<Employee> getEmployees(int typeID)
        {
            return db.Employees.Where(e => e.TypeID == typeID).Select(e => e);
        }

        public EmployeeTarget getEmployeeTargets(int employeeID)
        {
            return db.EmployeeTargets.Where(t => t.EmployeeID == employeeID && t.Year == year).FirstOrDefault();
        }

        public IEnumerable<TargetData> getTargetData(int employeeTargetID)
        {
            return db.TargetDatas.Where(t => t.EmployeeTargetID == employeeTargetID && t.Date.Year == year).Select(t => t);
        }

        public IQueryable<NonRevenueHourData> getNonRevenue(int targetDataID)
        {
            return db.NonRevenueHourDatas.Where(t => t.TargetDataID == targetDataID).Select(t => t);
        }
        public NonRevenueHour nonRevenueName(int NonRevenueHourID)
        {
            return db.NonRevenueHours.Where(t => t.NonRevenueHourID == NonRevenueHourID).Select(t => t).FirstOrDefault();
        }


        public IQueryable<int> getNonRevenueHours(int targetDataID)
        {
            return db.NonRevenueHourDatas.Where(t => t.TargetDataID == targetDataID).Select(t => t.Value);
        }

        public IQueryable<CounsellingServiceData> getAdjustments(int counsellingServiceID)
        {
            return db.CounsellingServiceDatas.Where(d => d.CounsellingServiceID == counsellingServiceID && d.Date.Year == year).Select(d => d);
        }

        public TargetData getMonthlyTargetData(IEnumerable<TargetData> data, int month)
        {
            return data.Where(d => d.Date.Month == month).Select(d => d).FirstOrDefault();

        }
        
        public GroupTherapyTarget getMonthlyGroupTargetData(IQueryable<GroupTherapyTarget> data, int month)
        {
            return data.Where(d => d.Date.Month == month).Select(d => d).FirstOrDefault();

        }
         public InternTarget getMonthlyInternTargetData(IQueryable<InternTarget> data, int month)
        {
            return data.Where(d => d.Date.Month == month).Select(d => d).FirstOrDefault();

        }
        public decimal retreiveMonthlyValue(List<QueryableData> data, int month)
        {
            return data.Where(d => d.Month == month).Select(d => d.Value).FirstOrDefault();
        }

        public string employeeTypeName(int employeeTypeID)
        {
            return db.EmployeeTypes.Where(x => x.EmployeeTypeID == employeeTypeID).Select(x => x.Name).FirstOrDefault();
        }

       

        public IQueryable<ContractHour> getContractHours(Employee e)
        {
            return db.ContractHours.Where( c => c.EmployeeID == e.EmployeeID && c.Date.Year == year).Select(c => c);
        }

        public decimal getMonthlyContractHours(IQueryable<ContractHour> data, int month)
        {
            return data.Where(p => p.Date.Month == month).Select(p => p.Hour).FirstOrDefault();
        }

        public IQueryable<InternTarget> getInternHours(Employee e)
        {
            return db.InternTargets.Where(c => c.EmployeeID == e.EmployeeID && c.Date.Year == year).Select(c => c);
        }

        public decimal getMonthlyInternHours(IQueryable<InternTarget> data, int month)
        {
            return data.Where(p => p.Date.Month == month).Select(p => p.Hour).FirstOrDefault();
        }

        public IEnumerable<Employee> getEmployee(int employeeID)
        {
            return db.Employees.Where(e => e.EmployeeID == employeeID).Select(e => e);
        }

        public IQueryable<ProjectedFeePerHour> getProjectedFee(int ID)
        {
            return db.ProjectedFeePerHours.Where(p => p.EmployeeTypeID == ID && p.Date.Year == year).Select(p => p);
        }

        public decimal getMonthlyProjectedFee(IQueryable<ProjectedFeePerHour> data, int month)
        {
            return data.Where(p => p.Date.Month == month).Select(p => p.Value).FirstOrDefault();
        }

        public NonRevenueHourData supervisionHours(int ID)
        {
            return db.NonRevenueHourDatas.Where(a => a.TargetDataID == ID && a.NonRevenueHourID == 1).Select(a => a).FirstOrDefault();
        }

        public NonRevenueHourData groupHours(int ID)
        {
            return db.NonRevenueHourDatas.Where(a => a.TargetDataID == ID && a.NonRevenueHourID == 10).Select(a => a).FirstOrDefault();
        }

        public IQueryable<ActualData> getActualData(int actualValueID)
        {
            return db.ActualDatas.Where(x => x.ActualValueID == actualValueID && x.Date.Year == year).Select(x => x);
        }

        public ActualData getMonthlyActualData(IQueryable<ActualData> data, int month)
        {
            return data.Where(x => x.Date.Month == month).Select(x => x).SingleOrDefault();
        }

        public EmployeeType getEmployeeType(int id)
        {
            return db.EmployeeTypes.Where(x => x.EmployeeTypeID == id).Select(x => x).SingleOrDefault();
        }

        public IQueryable<Employee> getEmployeeByTypeAndDept(int type, int dept)
        {
            return db.Employees.Where(x => x.DepartmentID == dept && x.TypeID == type ).Select(x => x);
        }

        public decimal getResidentCostPerHour()
        {
            return db.UnchangingValues.Where(x => x.Name.ToLower() == "resident cost per hour" && x.Year == year).Select(x => x.Value).SingleOrDefault();
        }


    }
}