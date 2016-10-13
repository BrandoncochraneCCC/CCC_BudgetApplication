using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers.Counselling
{
    public class CounsellingAnalysisController : ObjectInstanceController
    {



        private int year;
        private CounsellingServices services;
        private ArrayServices arrayServices = new ArrayServices();
        private SummaryCounsellingController summary = new SummaryCounsellingController();
        private CounsellingServicesQueries queries;

        public CounsellingAnalysisController()
        {
            year = YEAR;
            services = new CounsellingServices(year);
            queries = new CounsellingServicesQueries(year);

        }


        public List<DataTable> CounsellingAnalysis()
        {


            List<DataTable> list = new List<DataTable>();
            try
            {
                DataTable revenues = projectedRevenue(); //added
                DataTable totalRevenues = projectedRevenueTotal(revenues); //added
                DataTable hour = analysisHours(); //added
                DataTable totalHours = analysisHoursTotal(hour);//added
                DataTable rateProjections = hourlyRateProjection(totalRevenues, totalHours); //added
                DataTable categoryRate = rateByCategory(revenues, hour);//added
                DataTable groups = groupRevenue();//added

                list.Add(revenues);
                list.Add(totalRevenues);
                list.Add(hour);
                list.Add(totalHours);
                list.Add(rateProjections);
                list.Add(categoryRate);
                list.Add(groups);
            }
            catch(Exception ex)
            {
                log.Fatal("counselling analysis failed to build", ex);
            }
           

            return list;
        }

        private DataTable analysisHours()
        {
            DataTable table = new DataTable();
            table.tableName = "Average Projected Hours";
            table.dataList = analysisHoursDataList();
            return table;
        }

        private List<DataLine> analysisHoursDataList()
        {
            List<DataLine> list = new List<DataLine>();

            DataLine fullTime = createDataLine("Full Time", adjustedFTHoursData(), "hour");
            DataLine actualFT = createDataLine("Actual Full Time Hours", actualData(ACTUAL_FT_HOURSID), "hour");
            DataLine resHrs = services.residentHours();
            DataLine actualRes = createDataLine("Actual Residents Hours", actualData(ACTUAL_RESIDENT_HOURSID), "hour");
            DataLine stuHrs = createDataLine("Students & Volunteer", services.studentHours().Values, "hour"); ;
            DataLine actualStu = createDataLine("Actual Students & Volunteer Hours", actualData(ACTUAL_STUDENT_HOURSID), "hour");

            list.Add(fullTime);
            list.Add(actualFT);
            list.Add(resHrs);
            list.Add(actualRes);
            list.Add(stuHrs);
            list.Add(actualStu);


            return list;
        }

        private decimal[] adjustedFTHoursData()
        {
            DataLine FTHours = services.fullTimeEmployeeHours();
            DataLine discount = services.adjustmentData(services.getDiscount(), "Discount for sick and PD days");
            discount.Values = arrayServices.multiplyArrays(FTHours.Values, discount.Values);
            DataLine reduction = services.adjustmentData(services.getReduction(), "Reduction for High River & BICS");
            DataLine adjustments = services.adjustmentData(services.adjustmentValues(), "Other Adjustments");
            decimal[] adj = arrayServices.combineArrays(discount.Values, reduction.Values, adjustments.Values);
            return summary.calculateAdjFTHours("", adj, FTHours.Values).Values;
        }

        private decimal[] actualData(int id)
        {
            decimal[] values = new decimal[12];
            var data = queries.getActualData(id);
            if (data != null)
            {
                for (var i = 0; i < 12; i++)
                {
                    var result = queries.getMonthlyActualData(data, i + 1);
                    if (result != null)
                    {
                        values[i] = result.Value;
                    }
                }

            }


            return values;
        }

        private DataTable analysisHoursTotal(DataTable hours)
        {
            DataTable table = new DataTable();
            table.tableName = "Projected Totals";
            table.dataList = analysisHoursTotalDataList(hours);
            return table;
        }

        private List<DataLine> analysisHoursTotalDataList(DataTable hours)
        {
            List<DataLine> list = new List<DataLine>();
            decimal[] ft = hours.dataList.ElementAt(0).Values;
            decimal[] res = hours.dataList.ElementAt(2).Values;
            decimal[] stu = hours.dataList.ElementAt(4).Values;

            decimal[] ftActual = hours.dataList.ElementAt(1).Values;
            decimal[] resActual = hours.dataList.ElementAt(3).Values;
            decimal[] stuActual = hours.dataList.ElementAt(5).Values;
            DataLine totalProjected = createDataLine("Total Projected Hours", arrayServices.combineArrays(ft, res, stu), "hour");
            DataLine actual = createDataLine("Actual", arrayServices.combineArrays(ftActual, resActual, stuActual), "hour");
            DataLine difference = createDataLine("Difference", arrayServices.subtractArrays(actual.Values, totalProjected.Values), "hour");
            DataLine cumulative = createDataLine("Accumulative Difference", arrayServices.cumulativeArray(difference.Values), "hour");

            list.Add(totalProjected);
            list.Add(actual);
            list.Add(difference);
            list.Add(cumulative);
            return list;
        }

        private DataLine createDataLine(string name, decimal[] values, string viewClass = "", bool isAverage = false)
        {
            DataLine line = new DataLine();
            line.Name = name;
            line.Values = values;
            line.viewClass = viewClass;
            line.isAverage = isAverage;
            return line;
        }

        private DataTable hourlyRateProjection(DataTable revenue, DataTable hour)
        {
            DataTable table = new DataTable();
            table.tableName = "Hourly Rate Projections";
            table.dataList = hourlyRateProjectionDataList(revenue, hour);
            return table;
        }

        private List<DataLine> hourlyRateProjectionDataList(DataTable revenues, DataTable hours)
        {
            decimal[] revenue = revenues.dataList.ElementAt(0).Values;
            decimal[] hour = hours.dataList.ElementAt(0).Values;

            decimal[] actualRevenue = revenues.dataList.ElementAt(1).Values;
            decimal[] actualHour = hours.dataList.ElementAt(1).Values;

            List<DataLine> list = new List<DataLine>();
            DataLine totalProjected = createDataLine("Projected average rate ($/hr)", arrayServices.divideArrays(revenue, hour), "", true);
            DataLine actual = createDataLine("Actual Average Rate", arrayServices.divideArrays(actualRevenue, actualHour), "", true);

            list.Add(totalProjected);
            list.Add(actual);

            return list;
        }

        private DataTable groupRevenue()
        {
            DataTable table = new DataTable();
            CounsellingProgram program = new CounsellingProgram(year);
            table.tableName = "Groups";
            table.dataList = groupRevenueDataList(program);
            return table;
        }

        private List<DataLine> groupRevenueDataList(CounsellingProgram program)
        {
            List<DataLine> list = new List<DataLine>();
            DataLine projected = createDataLine("Projected", program.RevenueSummaryGroupDataValues(year));
            DataLine actual = createDataLine("Actual Group Revenue", actualData(ACTUAL_GROUP_REVENUEID));
            DataLine difference = createDataLine("Difference", arrayServices.subtractArrays(actual.Values, projected.Values));
            DataLine cumulative = createDataLine("Accumulative Difference", arrayServices.cumulativeArray(difference.Values));

            list.Add(projected);
            list.Add(actual);
            list.Add(difference);
            list.Add(cumulative);


            return list;
        }



        private DataTable rateByCategory(DataTable revenue, DataTable hour)
        {
            DataTable table = new DataTable();
            table.tableName = "Average Rate by Category";
            table.dataList = rateByCategoryDataList(revenue, hour);
            return table;
        }

        private List<DataLine> rateByCategoryDataList(DataTable revenue, DataTable hour)
        {
            List<DataLine> list = new List<DataLine>();
            var ftRate = services.getFTRate();
            var resRate = services.getResRate();
            var intRate = services.getStuRate();

            decimal[] actFtRev = revenue.dataList.ElementAt(1).Values;
            decimal[] actFthr = hour.dataList.ElementAt(1).Values;

            decimal[] actResRev = revenue.dataList.ElementAt(3).Values;
            decimal[] actReshr = hour.dataList.ElementAt(3).Values;

            decimal[] actStuRev = revenue.dataList.ElementAt(5).Values;
            decimal[] actStuhr = hour.dataList.ElementAt(5).Values;

            DataLine ft = createDataLine("Full Time (" + ftRate + ")", arrayServices.divideArrays(actFtRev, actFthr), "", true);
            DataLine res = createDataLine("Resident (" + resRate + ")", arrayServices.divideArrays(actResRev, actReshr), "", true);
            DataLine stu = createDataLine("Interns (" + intRate + ")", arrayServices.divideArrays(actStuRev, actStuhr), "", true);

            list.Add(ft);
            list.Add(res);
            list.Add(stu);

            return list;
        }


        private DataTable projectedRevenue()
        {
            DataTable table = new DataTable();
            table.tableName = "Average Projected Revenue";
            table.dataList = projectedRevenueDataList();
            return table;
        }

        private List<DataLine> projectedRevenueDataList()
        {
            List<DataLine> list = new List<DataLine>();
            try
            {
                decimal[] ft = summary.getRevenue("Full Time", services.getFTRate(), adjustedFTHoursData()).Values;
                decimal[] res = summary.getRevenue("Residents", services.getResRate(), services.residentHours().Values).Values;
                decimal[] stu = summary.getRevenue("Students & Volunteers", services.getStuRate(), services.studentHours().Values).Values;

                DataLine FTRevenue = createDataLine("Full Time", ft);
                DataLine resRevenue = createDataLine("Residents", res);
                DataLine stuRevenue = createDataLine("Students & Volunteers", stu);
                DataLine actualFT = createDataLine("Actual Full Time Revenue", actualData(ACTUAL_FT_REVENUEID));
                DataLine actualRes = createDataLine("Actual Residents Revenue", actualData(ACTUAL_RESIDENT_REVENUEID));
                DataLine actualStu = createDataLine("Actual Students & Volunteer Revenue", actualData(ACTUAL_STUDENT_REVENUEID));


                list.Add(FTRevenue);
                list.Add(actualFT);
                list.Add(resRevenue);
                list.Add(actualRes);
                list.Add(stuRevenue);
                list.Add(actualStu);

            }
            catch(Exception ex)
            {
                log.Error("projected revenue failed to build", ex);
            }
            


            return list;
        }

        private DataTable projectedRevenueTotal(DataTable revenues)
        {
            DataTable table = new DataTable();
            table.tableName = "Revenue Projections";
            table.dataList = projectedRevenueTotalDataList(revenues);

            return table;
        }

        private List<DataLine> projectedRevenueTotalDataList(DataTable revenues)
        {
            List<DataLine> list = new List<DataLine>();
            decimal[] ft = revenues.dataList.ElementAt(0).Values;
            decimal[] res = revenues.dataList.ElementAt(2).Values;
            decimal[] stu = revenues.dataList.ElementAt(4).Values;

            decimal[] ftActual = revenues.dataList.ElementAt(1).Values;
            decimal[] resActual = revenues.dataList.ElementAt(3).Values;
            decimal[] stuActual = revenues.dataList.ElementAt(5).Values;
            DataLine totalProjected = createDataLine("Total Projected Revenue", arrayServices.combineArrays(ft, res, stu));
            DataLine actual = createDataLine("Actual", arrayServices.combineArrays(ftActual, resActual, stuActual));
            DataLine difference = createDataLine("Difference", arrayServices.subtractArrays(actual.Values, totalProjected.Values));
            DataLine cumulative = createDataLine("Accumulative Difference", arrayServices.cumulativeArray(difference.Values));

            list.Add(totalProjected);
            list.Add(actual);
            list.Add(difference);
            list.Add(cumulative);
            return list;
        }

    }
}