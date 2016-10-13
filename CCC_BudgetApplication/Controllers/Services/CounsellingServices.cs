using Application.Controllers;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class CounsellingServices
    {

        private BudgetDataEntities db = new ObjectInstanceController().db;
        private ArrayServices arrayServices = new ArrayServices();
        private int year;
        private CounsellingServicesQueries queries;

        public CounsellingServices(int year)
        {
            this.year = year;
            queries = new CounsellingServicesQueries(year);

        }

        public decimal[] getRevenueSummaryCounselling()
        {
            decimal[] values = new decimal[12];
            decimal[] residentRevenue = calculateResidentRevenue();
            decimal[] studentRevenue = calculateInternRevenue();
            decimal[] FTRevenue = calculateFTRevenue();

            values = arrayServices.combineArrays(residentRevenue, studentRevenue, FTRevenue);

            return values;
        }


        public DataLine fullTimeEmployeeHours()
        {
            IEnumerable<Employee> employees = queries.getEmployees(ObjectInstanceController.FULLTIME_EMPLOYEETYPEID);

            DataLine line = new DataLine();
            line.Name = queries.employeeTypeName(ObjectInstanceController.FULLTIME_EMPLOYEETYPEID);
            line.Values = employeeData(employees);
            line.viewClass = "hour";

            return line;
        }

        public DataLine studentHours()
        {
            IEnumerable<Employee> students = queries.getEmployees(ObjectInstanceController.INTERN_EMPLOYEETYPEID);

            DataLine line = new DataLine();
            line.Name = queries.employeeTypeName(ObjectInstanceController.INTERN_EMPLOYEETYPEID);
            line.Values = getstudentHours(students);
            line.viewClass = "hour";

            return line;
        }

        public DataLine residentHours()
        {
            IEnumerable<Employee> residents = queries.getEmployees(ObjectInstanceController.RESIDENT_EMPLOYEETYPEID);

            DataLine line = new DataLine();
            line.Name = queries.employeeTypeName(ObjectInstanceController.RESIDENT_EMPLOYEETYPEID);
            line.Values = getResidentHours(residents);
            line.viewClass = "hour";

            return line;
        }

        public DataLine contractHours()
        {
            IEnumerable<Employee> contractors = queries.getEmployees(ObjectInstanceController.CONTRACT_EMPLOYEETYPEID);

            DataLine line = new DataLine();
            line.Name = queries.employeeTypeName(ObjectInstanceController.CONTRACT_EMPLOYEETYPEID);
            line.Values = getContractHours(contractors);
            line.viewClass = "hour";

            return line;
        }

        private decimal[] calculateResidentRevenue()
        {
            IEnumerable<Employee> residents = queries.getEmployees(ObjectInstanceController.RESIDENT_EMPLOYEETYPEID);
            decimal rate = getResRate();

            decimal[] values = new decimal[12];
            values = getResidentHours(residents);

            values = getProjectedRevenue(values, rate);

            return values;
        }

        private decimal[] getResidentHours(IEnumerable<Employee> students)
        {
            decimal[] values = new decimal[12];

            foreach (var item in students)
            {
                var data = queries.getResidentTargets(item);
                //sorts and adds values to display
                foreach (var d in data)
                {
                    values[d.Date.Month - 1] += d.Hour;
                }
            }

            return values;
        }

        private decimal[] calculateInternRevenue()
        {
            IEnumerable<Employee> interns = queries.getEmployees(ObjectInstanceController.INTERN_EMPLOYEETYPEID);
            decimal rate = getStuRate();

            decimal[] values = new decimal[12];

            values = getstudentHours(interns);

            values = getProjectedRevenue(values, rate);

            return values;
        }

        private decimal[] getstudentHours(IEnumerable<Employee> students)
        {
            decimal[] values = new decimal[12];

            foreach (var item in students)
            {
                var data = queries.getInternTargets(item);
                //sorts and adds values to display
                foreach (var d in data)
                {
                    values[d.Date.Month - 1] += d.Hour;
                }
            }

            return values;
        }

        private decimal[] calculateFTRevenue()
        {
            IEnumerable<Employee> employees = queries.getEmployees(ObjectInstanceController.FULLTIME_EMPLOYEETYPEID);
            decimal rate = getFTRate(); ;
            decimal[] values = new decimal[12];

            values = employeeData(employees);
            decimal[] adjustments = getAdjustments(values);
            values = getProjectedRevenue(adjustments, rate);

            return values;
        }

        public List<DataLine> getEmployeeTargetsList()
        {
            List<DataLine> list = new List<DataLine>();
            IEnumerable<Employee> employees = queries.getEmployees(ObjectInstanceController.FULLTIME_EMPLOYEETYPEID);

            foreach (var e in employees)
            {
                DataLine line = new DataLine();
                line.Values = employeeTarget(e);
                line.SourceID = e.EmployeeID;
                line.Name = e.FirstName + " " + e.LastName;
                line.Action = "EmployeeData";
                line.Controller = "Employees";
                line.viewClass = "hour";
                line.year = year;

                if (arrayServices.sumArray(line.Values) != 0)
                {
                    list.Add(line);
                }
            }

            return list;
        }

        public List<DataLine> getResidentTargetList()
        {
            List<DataLine> list = new List<DataLine>();
            IEnumerable<Employee> employees = queries.getEmployees(ObjectInstanceController.RESIDENT_EMPLOYEETYPEID);

            foreach (var e in employees)
            {
                DataLine line = new DataLine();
                line.Values = residentTarget(e);
                line.SourceID = e.EmployeeID;
                line.Name = e.FirstName + " " + e.LastName;
                line.Action = "singleResident";
                line.Controller = "Bursaries";
                line.viewClass = "hour";
                line.year = year;

                if (arrayServices.sumArray(line.Values) != 0)
                {
                    list.Add(line);
                }
            }

            return list;
        }

        private decimal[] residentTarget(Employee e)
        {
            decimal[] values = new decimal[12];

            var targets = queries.getResidentTargets(e);
            values = sortData(targets);

            return values;
        }

        private decimal[] sortData(IEnumerable<ResidentTarget> targets)
        {
            decimal[] values = new decimal[12];

            for (var i = 0; i < 12; i++)
            {
                values[i] = queries.getMonthlyTargetHours(targets, i + 1);
            }

            return values;
        }

        public decimal[] employeeData(Employee e)
        {
            var employee = queries.getEmployee(e.EmployeeID);
            return employeeData(employee);
        }

        private decimal[] employeeData(IEnumerable<Employee> employees)
        {
            decimal[] values = new decimal[12];

            foreach (var e in employees)
            {
                if (hasTarget(e))
                {
                    var target = queries.getEmployeeTargets(e.EmployeeID);
                    if(target != null)
                    {
                        var revenueHours = queries.getTargetData(target.EmployeeTargetID);
                        foreach (var data in revenueHours)
                        {
                            var sum = 0;
                            sum = (Int32)data.RevenueHours;
                            var result = queries.getNonRevenueHours(data.TargetDataID);
                            foreach (var r in result)
                            {
                                sum -= r;
                            }
                            values[data.Date.Month - 1] += (Decimal)sum;
                        }
                    }
                    
                }
            }

            return values;
        }

        private decimal[] employeeTarget(Employee e)
        {
            decimal[] values = new decimal[12];
            var target = queries.getEmployeeTargets(e.EmployeeID);

            var revenueHours = getRevenueHour(target);
            if (revenueHours != null)
            {
                foreach (var data in revenueHours)
                {
                    var sum = 0;
                    sum = (Int32)data.RevenueHours;
                    var result = queries.getNonRevenueHours(data.TargetDataID);
                    foreach (var r in result)
                    {
                        sum -= r;
                    }
                    values[data.Date.Month - 1] = (Decimal)sum;
                }
            }

            return values;
        }

        public List<DataLine> targetTable(Employee e)
        {
            List<DataLine> list = new List<DataLine>();

            var target = queries.getEmployeeTargets(e.EmployeeID);
            if (target != null)
            {
                TargetData[] targetData = revenueHourData(target);
                if (targetData != null)
                {
                    decimal[] revHrs = revenueHours(targetData);
                    List<DataLine> nonRevenueHours = nonRevenueData(targetData);
                    decimal[] numStu = numberStudents(targetData);
                    decimal[] targetHours = employeeData(e);
                    decimal[] indHrs = indirectHours(target, revHrs, e);
                    list.Add(targetRevenueHour(target));
                    list.Add(createDataLine("Total Revenue Hours", revHrs, "editable"));
                    list = (combineLists(list, nonRevenueHours));
                    list.Add(createDataLine("Targets", targetHours));
                    list.Add(createDataLine("Indirect Hours", indHrs));
                    list.Add(createDataLine("# of Students", numStu));
                }

            }
            else
            {
                list = null;
            }


            return list;
        }

        private decimal[] indirectHours(EmployeeTarget target, decimal[] revenueHours, Employee e)
        {
            var start = 0;
            var end = 0;
            if (e.StartDate != null && e.StartDate.Value.Year == year)
            {
                start = e.StartDate.Value.Month - 1;
            }
            if (e.EndDate != null && e.EndDate.Value.Year == year)
            {
                end = e.EndDate.Value.Month;
            }
            decimal[] values = new decimal[12];
            for (var i = start; i < end; i++)
            {
                values[i] = target.Hour - revenueHours[i];
            }


            return values;
        }

        private DataLine targetRevenueHour(EmployeeTarget t)
        {
            DataLine line = new ViewModels.DataLine();
            line.SourceID = t.EmployeeTargetID;
            decimal[] value = new decimal[1];
            value[0] = t.Hour;

            line.Values = value;
            return line;
        }

        private List<DataLine> combineLists(List<DataLine> list1, List<DataLine> list2)
        {
            List<DataLine> result = new List<DataLine>();

            foreach (var item in list1)
            {
                result.Add(item);
            }
            foreach (var item in list2)
            {
                result.Add(item);
            }

            return result;
        }

        private DataLine createDataLine(string name, decimal[] values, string viewClass = "")
        {
            DataLine line = new ViewModels.DataLine();
            line.Name = name;
            line.Values = values;
            line.viewClass = viewClass;
            return line;
        }

        private List<DataLine> nonRevenueData(TargetData[] targetData)
        {
            List<DataLine> list = new List<DataLine>();
            if (targetData[0] != null)
            {
                var nrh = queries.getNonRevenue(targetData[0].TargetDataID);
                foreach (var item in nrh)
                {
                    DataLine line = new DataLine();
                    var data = queries.nonRevenueName(item.NonRevenueHourID);
                    line.Name = data.Name;
                    line.SourceID = data.NonRevenueHourID;
                    line.Values = new decimal[12];
                    line.viewClass = "editable";
                    list.Add(line);
                }
                for (var i = 0; i < 12; i++)
                {
                    IQueryable<NonRevenueHourData> nonRevenues = null;
                    if (targetData[i] != null)
                    {
                        nonRevenues = queries.getNonRevenue(targetData[i].TargetDataID);
                    }

                    foreach (var item in list)
                    {
                        nonRevenueHourData(item, nonRevenues, i);
                    }

                }
            }


            return list;
        }


        private void nonRevenueHourData(DataLine line, IQueryable<NonRevenueHourData> data, int index)
        {
            if (data != null)
            {
                foreach (var d in data)
                {
                    if (d.NonRevenueHourID == line.SourceID)
                    {
                        var temp = d.Value;
                        if(temp > 0)
                        {
                            temp *= -1;
                        }
                        line.Values[index] = temp;
                    }
                }
            }

        }

        private DataLine totalRevHrs(string name, decimal[] values)
        {
            DataLine line = new DataLine();
            line.Name = name;
            line.Values = values;

            return line;
        }

        private decimal[] revenueHours(TargetData[] t)
        {
            decimal[] values = new decimal[12];
            if (t != null)
            {
                for (var i = 0; i < 12; i++)
                {
                    if (t[i] != null && t[i].RevenueHours != 0)
                    {
                        values[i] = (decimal)t[i].RevenueHours;
                    }
                }
            }

            return values;
        }

        private decimal[] numberStudents(TargetData[] t)
        {
            decimal[] values = new decimal[12];
            for (var i = 0; i < 12; i++)
            {
                if (t[i] != null && t[i].NumStudents != null)
                {
                    values[i] = (decimal)t[i].NumStudents;
                }
            }
            return values;
        }

        private TargetData[] revenueHourData(EmployeeTarget t)
        {
            TargetData[] values = new TargetData[12];

            var hours = getRevenueHour(t);
            if (hours != null)
            {
                for (var i = 0; i < 12; i++)
                {
                    values[i] = hours.Where(h => h.Date.Month == i + 1).Select(h => h).FirstOrDefault();
                }
            }

            return values;
        }

        public IEnumerable<TargetData> getRevenueHour(EmployeeTarget t)
        {
            IEnumerable<TargetData> revenueHours = null;
            if (t != null)
            {
                revenueHours = queries.getTargetData(t.EmployeeTargetID);
            }

            return revenueHours;
        }

        private decimal[] getAdjustments(decimal[] values)
        {
            decimal[] adjustedValues = new decimal[12];

            var discount = getDiscount();
            var reduction = getReduction();
            var adjustment = adjustmentValues();

            decimal[] discounts = sortData(discount);
            decimal[] reductions = sortData(reduction);
            decimal[] adjustments = sortData(adjustment);

            adjustedValues = arrayServices.multiplyArrays(values, discounts);
            adjustedValues = arrayServices.combineArrays(adjustedValues, reductions, adjustments);
            adjustedValues = arrayServices.subtractArrays(values, adjustedValues);

            return adjustedValues;
        }

        public IQueryable<CounsellingServiceData> getDiscount()
        {
            return queries.getAdjustments(1001);
        }

        public IQueryable<CounsellingServiceData> getReduction()
        {
            return queries.getAdjustments(1);
        }

        public IQueryable<CounsellingServiceData> adjustmentValues()
        {
            return queries.getAdjustments(2);
        }

        public DataLine adjustmentData(IQueryable<CounsellingServiceData> data, string name)
        {
            decimal[] values = new decimal[12];
            DataLine line = new DataLine();

            //sorts and adds values to display
            foreach (var d in data)
            {
                values[d.Date.Month - 1] = d.Value * -1;
            }

            line.Values = values;
            line.Name = name;
            line.viewClass = "hour";

            return line;
        }



        private decimal[] sortData(IQueryable<CounsellingServiceData> dataSet)
        {
            List<QueryableData> data = queryableDataList(dataSet);

            decimal[] sortedData = arrayServices.sortData(data);

            return sortedData;
        }

        private List<QueryableData> queryableDataList(IQueryable<CounsellingServiceData> dataSet)
        {
            List<QueryableData> list = new List<QueryableData>();
            foreach (var d in dataSet)
            {
                QueryableData data = new QueryableData();
                data.Month = d.Date.Month;
                data.Year = d.Date.Year;
                data.Value = d.Value;

                list.Add(data);
            }

            return list;
        }

        private decimal[] getProjectedRevenue(decimal[] data, decimal rate)
        {
            decimal[] values = new decimal[12];
            for (var i = 0; i < 12; i++)
            {
                if (rate != 0)
                {
                    values[i] = data[i] * rate;
                }
            }
            return values;
        }


        private bool hasTarget(Employee e)
        {
            bool hasTarget = false;

            var result = e.EmployeeTargets.FirstOrDefault();
            if (result != null)
            {
                hasTarget = true;
            }

            return hasTarget;
        }

        private decimal getRate(Int32 typeID)
        {
            var result = from r in db.AverageRates
                         where r.EmployeeTypeID == typeID
                         && r.Year == year
                         select r.Value;
            var rate = result.FirstOrDefault();

            return rate;
        }

        public decimal getFTRate()
        {
            return getRate(ObjectInstanceController.FULLTIME_EMPLOYEETYPEID);
        }
        public decimal getResRate()
        {
            return getRate(ObjectInstanceController.RESIDENT_EMPLOYEETYPEID);
        }
        public decimal getStuRate()
        {
            return getRate(ObjectInstanceController.INTERN_EMPLOYEETYPEID);
        }

        public DataLine AssumptionFee(int employeeTypeID)
        {
            DataLine line = new DataLine();
            line.Name = "Assumption: Fee Per Hour";
            line.viewClass = "fee";
            decimal[] values = new decimal[12];
            var assumption = queries.getProjectedFee(employeeTypeID);
            for (var i = 0; i < 12; i++)
            {
                values[i] = queries.getMonthlyProjectedFee(assumption, i + 1);
            }
            line.Values = values;
            return line;
        }

        private decimal[] getContractHours(IEnumerable<Employee> contractors)
        {
            decimal[] values = new decimal[12];

            foreach (var item in contractors)
            {
                var data = queries.getContractHours(item);
                //sorts and adds values to display
                foreach (var d in data)
                {
                    values[d.Date.Month - 1] += d.Hour;
                }
            }

            return values;
        }

        public List<DataLine> getContractTargetsList()
        {
            List<DataLine> list = new List<DataLine>();
            var contractors = queries.getEmployees(ObjectInstanceController.CONTRACT_EMPLOYEETYPEID);
            foreach (var c in contractors)
            {
                DataLine line = new DataLine();
                line.SourceID = c.EmployeeID;
                line.Name = c.FirstName + " " + c.LastName;
                line.Action = "EmployeeData";
                line.Controller = "Employees";
                line.viewClass = "hour";
                line.year = year;

                decimal[] values = new decimal[12];
                var targets = queries.getContractHours(c);
                for (var i = 0; i < 12; i++)
                {
                    values[i] = queries.getMonthlyContractHours(targets, i + 1);
                }
            }

            return list;
        }
        public List<DataLine> getInternTargetList()
        {
            List<DataLine> list = new List<DataLine>();
            var interns = queries.getEmployees(ObjectInstanceController.INTERN_EMPLOYEETYPEID);
            foreach (var c in interns)
            {
                DataLine line = new DataLine();
                line.SourceID = c.EmployeeID;
                line.Name = c.FirstName + " " + c.LastName;
                line.Action = "EmployeeData";
                line.Controller = "Employees";
                line.viewClass = "hour";

                line.year = year;

                decimal[] values = new decimal[12];
                var targets = queries.getInternHours(c);
                for (var i = 0; i < 12; i++)
                {
                    values[i] = queries.getMonthlyInternHours(targets, i + 1);
                }
                line.Values = values;
                list.Add(line);
            }

            return list;
        }


    }
}