using Application.Controllers.Counselling;
using Application.Controllers.Employees;
using Application.Controllers.Queries;
using Application.Controllers.Services;
using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers.CounsellingSummaries
{
    public class CostOfServiceCounsellingHoursController : ObjectInstanceController
    {

        private int year;
        private EmployeeServices services;
        private CounsellingServices counsellingService;
        private ArrayServices arrayServices = new ArrayServices();
        private SummaryCounsellingController summary = new SummaryCounsellingController();
        private CounsellingServicesQueries queries;
        private SupervisionHoursController supervision;

        public CostOfServiceCounsellingHoursController()
        {
            year = YEAR;
            queries = new CounsellingServicesQueries(year);
            services = new EmployeeServices(year);
            counsellingService = new CounsellingServices(year);
            supervision = new SupervisionHoursController(year);
        }

        public CostOfServiceViewModel CostOfServiceCounsellingHours(int typeID)
        {
            CostOfServiceViewModel item = new CostOfServiceViewModel();
            IQueryable<Employee> employees = null;
            if (typeID == ObjectInstanceController.INTERN_EMPLOYEETYPEID)
            {
                employees = queries.getEmployeeByTypeAndDept(typeID, VOLUNTEER_INTERN_DEPTID);
            }
            else
            {
                employees = queries.getEmployeeByTypeAndDept(typeID, COUNSELLING_DEPT_ID);
            }
            List<CostOfService> list = new List<CostOfService>();
            if(employees != null)
            {
                foreach (var e in employees)
                {
                    CostOfService temp = costOfServiceDataTable(e);
                    if (temp != null)
                    {
                        list.Add(temp);
                    }
                }
                item.data = list;
                item.name = queries.getEmployeeType(typeID).Name;
                item.total = totalLine(item);
            }
            

            return item;
        }

        private CostOfService costOfServiceDataTable(Employee e)
        {
            CostOfService item = new CostOfService();
            item.Name = e.FirstName + " " + e.LastName;
            item.SourceID = e.EmployeeID;
            switch (e.TypeID)
            {
                case 1:
                    item.Counselling = arrayServices.sumArray(counsellingService.employeeData(e));
                    item.Supervising = arrayServices.sumArray(supervision.supervisionDataValues(e));
                    item.Groups = 0;
                    item.TotalHoursBilled = item.Counselling + item.Groups;
                    item.CostPerHour = services.getHourlyRate(e);
                    item.AnnualCost = item.TotalHoursBilled * item.CostPerHour;

                    if (item.AnnualCost == 0)
                    {
                        item = null;
                    }
                    break;
                case 4:
                    item.CostPerHour = queries.getResidentCostPerHour();
                    BursariesController c = new BursariesController();
                    item.Counselling = arrayServices.sumArray(c.retrieveResidentData(e).Targets);
                    item.AnnualCost = item.CostPerHour * item.Counselling;
                    item.Groups = 0;
                    item.TotalHoursBilled = item.Groups + item.Counselling;
                    break;
                case 6:
                    InternController i = new InternController();
                    item.Counselling = arrayServices.sumArray(i.InternData(e));
                    item.Groups = 0;
                    item.TotalHoursBilled = item.Counselling + item.Groups;
                    break;
            }

            return item;
        }

        private CostOfService totalLine(CostOfServiceViewModel data)
        {
            CostOfService item = new CostOfService();
            item.Name = data.name + " Sub Total";
            item.SourceID = 0;
            foreach (var d in data.data)
            {
                item.Counselling += d.Counselling;
                item.Supervising += d.Supervising;
                item.Groups += d.Groups;
                item.TotalHoursBilled += d.TotalHoursBilled;
                item.CostPerHour += d.CostPerHour;
                item.AnnualCost += d.AnnualCost;
            }


            return item;
        }

        public CostOfServiceViewModel CostOfServicetotalHour(List<CostOfServiceViewModel> data)
        {
            CostOfServiceViewModel item = new CostOfServiceViewModel();
            List<CostOfService> list = new List<CostOfService>();
            CostOfService cost = new CostOfService();
            cost.Name = "Combined Total";
            foreach (var d in data)
            {
                foreach (var value in d.data)
                {
                    cost.TotalHoursBilled += value.TotalHoursBilled;
                    cost.CostPerHour += value.CostPerHour;
                    cost.AnnualCost += value.AnnualCost;
                    cost.Counselling += value.Counselling;
                    cost.Groups += value.Groups;
                    cost.Supervising += value.Supervising;

                }
            }
            list.Add(cost);
            item.data = list; 

            return item;
        }

        public CostOfService CostOfServicePercent(CostOfServiceViewModel item, List<CostOfService> data = null)
        {
            CostOfService percentTotal = new CostOfService();
            percentTotal.Name = "Percent of Total";
            if (data != null)
            {
                var total = data.FirstOrDefault();
                if (total != null)
                {
                    if (total.TotalHoursBilled != 0)
                    {
                        percentTotal.TotalHoursBilled = item.total.TotalHoursBilled / total.TotalHoursBilled;

                    }
                    if (total.CostPerHour != 0)
                    {
                        percentTotal.CostPerHour = item.total.CostPerHour / total.CostPerHour;

                    }
                    if (total.AnnualCost != 0)
                    {
                        percentTotal.AnnualCost = item.total.AnnualCost / total.AnnualCost;

                    }
                    if (total.Counselling != 0)
                    {
                        percentTotal.Counselling = item.total.Counselling / total.Counselling;

                    }
                    if (total.Groups != 0)
                    {
                        percentTotal.Groups = item.total.Groups / total.Groups;

                    }
                    if (total.Supervising != 0)
                    {
                        percentTotal.Supervising = item.total.Supervising / total.Supervising;

                    }
                }
            }



            return percentTotal;
        }

    }
}