using Application.Controllers.Queries;
using Application.Controllers.Services;
using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers.Counselling
{
    public class SupervisionHoursController : Controller
    {
        private int year;
        private EmployeeQueries queries;
        private CounsellingServicesQueries counsellingQueries;
        private ArrayServices arrayServices = new ArrayServices();
        public SupervisionHoursController(int year)
        {
            this.year = year;
            queries = new EmployeeQueries(year);
            counsellingQueries = new CounsellingServicesQueries(year);
        }
        // GET: SupervisionHours
        public List<DataTable> EmployeeSupervisionHours()
        {
            List<DataTable> tables = new List<DataTable>();
            tables.Add(supervisionFeeTable());
            tables.Add(fullTimeSupervisionHours());

            return tables;
        }
        private DataTable fullTimeSupervisionHours()
        {
            DataTable table = new DataTable();
            table.tableName = "Full Time";
            table.dataList = supervisionHours();
            return table;
        }

        private List<DataLine> supervisionHours()
        {
            List<DataLine> list = new List<DataLine>();
            var employees = queries.getEmployeeByDepartmentAndType(EmployeeServices.FULLTIME_EMPLOYEETYPEID, EmployeeServices.FULLTIME_EMPLOYEETYPEID);
            foreach(var e in employees)
            {
                if (queries.hasTarget(e))
                {
                    list.Add(supervisionData(e));
                }
            }
            supervisionTotalTable(list);
            return list;
        }

        private DataTable supervisionFeeTable()
        {
            DataTable table = new DataTable();
            List<DataLine> list = new List<DataLine>();
            list.Add(averageSupervision());
            table.dataList = list;
            return table;
        }

        private DataLine averageSupervision()
        {
            var fee = queries.getSupervisionFee();
            DataLine line = new DataLine();
            line.SourceID = fee.UnchangingValueID;
            line.Name = "Average Supervision Fee";
            line.Values = new decimal[12] { fee.Value, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            return line;
        }

        private void supervisionTotalTable(List<DataLine> list)
        {
            DataLine hours = supervisionHoursTotal(list);
            DataLine fee = supervisionFee();
            DataLine totalCost = totalSupervisionCost(hours.Values, fee.Values);

            list.Add(hours);
            list.Add(fee);
            list.Add(totalCost);
        }

        private DataLine supervisionHoursTotal(List<DataLine> list)
        {
            DataLine line = new DataLine();
            line.viewClass = "hourSubTotal";
            line.Name = "Total Hours";
            decimal[] total = new decimal[12];
            foreach (var item in list)
            {
                total = arrayServices.combineArrays(total, item.Values);
            }
            line.Values = total;
            return line;
        }

        private DataLine supervisionFee()
        {
            var fee = queries.getSupervisionFee();
            DataLine line = new DataLine();
            line.viewClass = "fee";
            line.Name = "Cost of Supervision";
            line.Values = arrayServices.populateArray(fee.Value);


            return line;
        }


        private DataLine totalSupervisionCost(decimal[] hours, decimal[] fee)
        {
            DataLine line = new DataLine();
            line.viewClass = "total";
            line.Name = "Total Cost of Supervision";
            line.Values = arrayServices.multiplyArrays(hours, fee);


            return line;
        }

        private DataLine supervisionData(Employee e)
        {
            DataLine line = new DataLine();
            line.Name = e.FirstName + " " + e.LastName;
            line.SourceID = e.EmployeeID;
            line.Values = supervisionDataValues(e);
            line.Action = "EmployeeData";
            line.Controller = "Employees";
            line.viewClass = "hour";
            return line;
        }

        public decimal[] supervisionDataValues(Employee e)
        {
            decimal[] values = new decimal[12];
            var target = queries.getEmployeeTarget(e.EmployeeID).FirstOrDefault();
            if(target != null)
            {
                var data = counsellingQueries.getTargetData(target.EmployeeTargetID);
                if (data.FirstOrDefault() != null)
                {
                    for (var i = 0; i < 12; i++)
                    {
                        var monthlyData = counsellingQueries.getMonthlyTargetData(data, i + 1);
                        if (monthlyData != null)
                        {
                            var result = counsellingQueries.supervisionHours(monthlyData.TargetDataID);
                            if(result != null)
                            {
                                values[i] = result.Value;

                            }
                        }
                    }
                }
            }
           
           

            return values;
        }
    }
}