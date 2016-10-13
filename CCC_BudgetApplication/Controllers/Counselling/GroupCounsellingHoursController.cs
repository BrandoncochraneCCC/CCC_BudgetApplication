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
    public class GroupCounsellingHoursController : Controller
    {
        private int year;
        private EmployeeQueries queries;
        private CounsellingServicesQueries counsellingQueries;
        private ArrayServices arrayServices = new ArrayServices();
        public GroupCounsellingHoursController(int year)
        {
            this.year = year;
            queries = new EmployeeQueries(year);
            counsellingQueries = new CounsellingServicesQueries(year);
        }
        // GET: SupervisionHours
        public List<DataTable> GroupCounsellingHours()
        {
            List<DataTable> tables = new List<DataTable>();
            //tables.Add(groupFeeTable());
            tables.Add(fullTimeGroupHours());
            tables.Add(residentGroupHours());
            tables.Add(internGroupHours());
            tables.Add(groupHoursTotal(tables));


            return tables;
        }
        private DataTable groupHoursTotal(List<DataTable> tables)
        {
            DataTable table = new DataTable();
            DataLine line = new DataLine();
            decimal[] values = new decimal[12];
            foreach(var item in tables)
            {
                values = arrayServices.combineArrays(values, item.dataList.LastOrDefault().Values);
            }
            line.tableName = "Total";
            table.tableName = "Total";
            line.viewClass = "Total";
            line.Values = values;
            List<DataLine> list = new List<DataLine>();
            list.Add(line);
            table.dataList = list;

            return table;
        }

        private DataTable fullTimeGroupHours()
        {
            DataTable table = new DataTable();
            table.tableName = "Full Time";
            table.dataList = GroupHours(ObjectInstanceController.COUNSELLING_DEPT_ID, ObjectInstanceController.FULLTIME_EMPLOYEETYPEID);
            table.dataList.Add(totalRow(table));

            return table;
        }
        private DataTable residentGroupHours()
        {
            DataTable table = new DataTable();
            table.tableName = "Resident";
            table.dataList = GroupHours(ObjectInstanceController.COUNSELLING_DEPT_ID, ObjectInstanceController.RESIDENT_EMPLOYEETYPEID);
            table.dataList.Add(totalRow(table));

            return table;
        }
        private DataTable internGroupHours()
        {
            DataTable table = new DataTable();
            table.tableName = "Intern";
            table.dataList = GroupHours(ObjectInstanceController.VOLUNTEER_INTERN_DEPTID, ObjectInstanceController.INTERN_EMPLOYEETYPEID);
            table.dataList.Add(totalRow(table));
            return table;
        }
        private DataLine totalRow(DataTable table)
        {
            DataLine line = new DataLine();
            line.viewClass = "total";
            line.Name = table.tableName + " Total";
            DataTableServices service = new DataTableServices();
            line.Values = service.sumTable(table);
            return line;
        }
        private List<DataLine> GroupHours(int dept, int type)
        {
            List<DataLine> list = new List<DataLine>();
            var employees = queries.getEmployeeByDepartmentAndType(dept, type);
            foreach (var e in employees)
            {
                    if (queries.hasTarget(e))
                    {
                        list.Add(groupHourData(e));
                    }

            }
            return list;
        }

        private DataTable groupFeeTable()
        {
            DataTable table = new DataTable();
            List<DataLine> list = new List<DataLine>();
            table.tableName = "";
            list.Add(averageGroup());
            table.dataList = list;
            return table;
        }

        private DataLine averageGroup()
        {
            var fee = queries.getGroupFee();
            DataLine line = new DataLine();
            line.SourceID = fee.UnchangingValueID;
            line.Name = "Average Group Fee";
            line.Values = new decimal[12] { fee.Value, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            return line;
        }

        private void groupTotalTable(List<DataLine> list)
        {
            DataLine hours = groupHoursTotal(list);
            DataLine fee = GroupFee();
            DataLine totalCost = totalGroupCost(hours.Values, fee.Values);

            list.Add(hours);
            list.Add(fee);
            list.Add(totalCost);
        }

        private DataLine groupHoursTotal(List<DataLine> list)
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

        private DataLine GroupFee()
        {
            var fee = queries.getGroupFee();
            DataLine line = new DataLine();
            line.viewClass = "fee";
            line.Name = "Cost of Group";
            line.Values = arrayServices.populateArray(fee.Value);


            return line;
        }


        private DataLine totalGroupCost(decimal[] hours, decimal[] fee)
        {
            DataLine line = new DataLine();
            line.viewClass = "total";
            line.Name = "Total Cost of Supervision";
            line.Values = arrayServices.multiplyArrays(hours, fee);


            return line;
        }

        private DataLine groupHourData(Employee e)
        {
            DataLine line = new DataLine();
            line.Name = e.FirstName + " " + e.LastName;
            line.SourceID = e.EmployeeID;
            line.Values = groupDataValues(e);
            line.Action = "EmployeeData";
            line.Controller = "Employees";
            line.viewClass = "hour";
            return line;
        }

        public decimal[] groupDataValues(Employee e)
        {
            decimal[] values = new decimal[12];
            switch (e.TypeID)
            {
                case 6: values = internDataValues(e); break;
                case 4: values = residentDataValues(e); break;
                case 1: values = fullTimeDataValues(e); break;

            }


            return values;
        }


        public decimal[] fullTimeDataValues(Employee e)
        {
            decimal[] values = new decimal[12];
            var target = queries.getEmployeeTarget(e.EmployeeID).FirstOrDefault();
            if (target != null)
            {
                var data = counsellingQueries.getTargetData(target.EmployeeTargetID);
                if (data.FirstOrDefault() != null)
                {
                    for (var i = 0; i < 12; i++)
                    {
                        var monthlyData = counsellingQueries.getMonthlyTargetData(data, i + 1);
                        if (monthlyData != null)
                        {
                            var result = counsellingQueries.groupHours(monthlyData.TargetDataID);
                            if (result != null)
                            {
                                values[i] = result.Value;

                            }
                        }
                    }
                }
            }
            return values;
        }
        public decimal[] residentDataValues(Employee e)
        {
            decimal[] values = new decimal[12];
            var target = queries.getGroupTarget(e.EmployeeID);
            if (target != null)
            {
                for (var i = 0; i < 12; i++)
                {
                    var monthlyData = counsellingQueries.getMonthlyGroupTargetData(target, i + 1);
                    if (monthlyData != null)
                    {
                        values[i] = monthlyData.Hour;
                    }
                }
            }
            return values;
        }
        public decimal[] internDataValues(Employee e)
        {
            decimal[] values = new decimal[12];
            var target = queries.getGroupTarget(e.EmployeeID);
            if (target != null)
            {
                for (var i = 0; i < 12; i++)
                {
                    var monthlyData = counsellingQueries.getMonthlyGroupTargetData(target, i + 1);
                    if (monthlyData != null)
                    {
                        values[i] = monthlyData.Hour;
                    }
                }
            }
            return values;
        }

    }
}