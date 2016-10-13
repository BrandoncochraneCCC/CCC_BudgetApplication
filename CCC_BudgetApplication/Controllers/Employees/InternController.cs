using Application.Controllers.Queries;
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
    public class InternController : ObjectInstanceController
    {
        private int INTERNTYPEID = 6;
        private DepartmentServices services;
        private ArrayServices arrayServices = new ArrayServices();
        private EmployeeQueries queries;
        private int year;
        // GET: DepartmentSummary

        public InternController()
        {
            year = YEAR;
            services = new DepartmentServices(year);
            queries = new EmployeeQueries(year);
        }
        public List<Resident> InternTables(int departmentID)
        {
            return InternList(departmentID);
        }

        private List<DataLine> InternDataList(int departmentID)
        {
            List<DataLine> list = new List<DataLine>();
            var employees = queries.getEmployeeByType(INTERNTYPEID);
            try
            {
                if (departmentID != 0)
                {
                    employees = queries.getDepartmentEmployeesByType(employees, INTERNTYPEID);
                }
                foreach (var e in employees)
                {
                    if (hasDataForYear(e))
                    {
                        list.Add(InternDataLine(e));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Info("intern data exception", ex);
            }
            
            return list;
        }

        public DataTable InternTable(int departmentID)
        {
            year = YEAR;
            services = new DepartmentServices(year);
            queries = new EmployeeQueries(year);
            DataTable table = new DataTable();
            table.tableName = "Volunteers/Interns";
            if (departmentID != 0)
            {
                var name = queries.getDepartment(departmentID).FirstOrDefault().Name;
                table.tableName = name + " " + table.tableName;
            }
            table.dataList = InternDataList(departmentID);


            return table;
        }

        private DataLine InternDataLine(Employee e)
        {
            DataLine line = new DataLine();
            line.Name = e.FirstName + " " + e.LastName;
            line.SourceID = e.EmployeeID;
            line.viewClass = "hour";
            line.Values = InternData(e);
            line.Action = "Edit";
            line.Controller = "Employees";
            return line;
        }


        private List<Resident> InternList(int departmentID)
        {
            List<Resident> list = new List<Resident>();
            try
            {
                var employees = queries.getEmployeeByType(INTERNTYPEID);
                if (departmentID != 0)
                {
                    employees = queries.getDepartmentEmployeesByType(employees, INTERNTYPEID);
                }
                foreach (var e in employees)
                {
                    if (hasDataForYear(e))
                    {
                        list.Add(InternGroupLine(e));
                    }
                }
            }
            catch(Exception ex)
            {
                log.Info("intern list exception", ex);
            }
            
            return list;
        }

        private Resident InternGroupLine(Employee e)
        {
            Resident resident = new Resident();
            resident.Name = e.FirstName + " " + e.LastName;
            resident.residentID = e.EmployeeID;
            resident.Targets = InternData(e);
            resident.GroupTargets = InternGroupData(e);
            return resident;
        }

        public decimal[] InternData(Employee e)
        {
            decimal[] values = new decimal[12];
            IQueryable<InternTarget> data = null;
            if (e != null)
            {
                data = queries.getInternData(e);
            }
            if (data != null)
            {
                for (var i = 0; i < 12; i++)
                {
                    values[i] = queries.getMonthlyInternData(data, i + 1);
                }
            }

            return values;
        }
        public decimal[] InternGroupData(Employee e)
        {
            decimal[] values = new decimal[12];
            IQueryable<GroupTherapyTarget> data = null;
            if (e != null)
            {
                data = queries.getGroupTarget(e.EmployeeID);
            }
            if (data != null)
            {
                for (var i = 0; i < 12; i++)
                {
                    values[i] = data.Where(x => x.Date.Month == i + 1).Select(x => x.Hour).SingleOrDefault();
                }
            }

            return values;
        }

        private bool hasDataForYear(Employee e)
        {
            if (queries.getInternData(e).FirstOrDefault() != null)
            {
                return true;
            }
            return false;
        }

    }
}