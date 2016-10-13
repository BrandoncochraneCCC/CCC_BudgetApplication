using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class SummaryCounsellingController : ObjectInstanceController
    {



        private int year;
        private CounsellingServices services;
        private ArrayServices arrayServices = new ArrayServices();
        private CounsellingServicesQueries queries;

        public SummaryCounsellingController()
        {
            year = YEAR;
            services = new CounsellingServices(year);
            queries = new CounsellingServicesQueries(year);

        }


        // GET: SummaryCounselling
        public List<DataTable> SummaryCounselling()
        {
            List<DataTable> tables = new List<DataTable>();


            //create line data
            DataLine FTHours = services.fullTimeEmployeeHours();
            //add contract hours here
            DataLine stuHrs = services.studentHours();
            DataLine resHrs = services.residentHours();
            DataLine contractHours = services.contractHours();
            DataLine subTotal = counsellingHrsSubTotal(FTHours, stuHrs, resHrs, contractHours); //include contract hours in this calculation
            DataLine discount = services.adjustmentData(services.getDiscount(), "Discount for sick and PD days");
            discount.Values = arrayServices.multiplyArrays(FTHours.Values, discount.Values);
            DataLine reduction = services.adjustmentData(services.getReduction(), "Reduction for High River & BICS");
            DataLine adjustments = services.adjustmentData(services.adjustmentValues(), "Other Adjustments");
            decimal[] adj = arrayServices.combineArrays(discount.Values, reduction.Values, adjustments.Values);
            DataLine totalBudgetHours = calculateTotalBudgetHours("Total Budget Hours", subTotal, adj);
            DataLine adjFTHours = calculateAdjFTHours("Adjusted Full Time Hours", adj, FTHours.Values);
            //add group contract hours here

            DataTable averageHours = new ViewModels.DataTable();
            averageHours.tableName = "Average Hours";
            //adds line data into list
            List<DataLine> averageList = new List<DataLine>();
            averageList.Add(FTHours);
            averageList.Add(contractHours);
            averageList.Add(resHrs);
            averageList.Add(stuHrs);
            averageList.Add(subTotal);
            averageList.Add(discount);
            averageList.Add(reduction);
            averageList.Add(adjustments);
            averageList.Add(totalBudgetHours);
            averageList.Add(adjFTHours);

            averageHours.dataList = averageList;

            //percentList hours section
            DataTable percentTable = new ViewModels.DataTable();
            percentTable.tableName = "Mix of Hours (% of Total Hours) ";
            percentTable.dataList = percentHours(adjFTHours, totalBudgetHours, resHrs, stuHrs, contractHours);

            //average projected revenue  
            DataTable projectedTable = new ViewModels.DataTable();
            projectedTable.tableName = "Average Projected Revenue";
            projectedTable.dataList = avgProjectedRevenue(adjFTHours, resHrs, stuHrs, totalBudgetHours);

            tables.Add(averageHours);
            tables.Add(percentTable);
            tables.Add(projectedTable);

            return tables;
        }

        private List<DataLine> percentHours(DataLine adjFTHours, DataLine totalBudgetHours, DataLine resHrs, DataLine stuHrs, DataLine contractHours)
        {
            List<DataLine> list = new List<DataLine>();

            DataLine FTPercent = getHourPercent("Full Time", adjFTHours.Values, totalBudgetHours.Values);
            DataLine contractPercent = getHourPercent("Contract", contractHours.Values, totalBudgetHours.Values);
            DataLine residentPercent = getHourPercent("Residents", resHrs.Values, totalBudgetHours.Values);
            DataLine InternPercent = getHourPercent("Interns", stuHrs.Values, totalBudgetHours.Values);

            list.Add(FTPercent);
            list.Add(contractPercent);
            list.Add(residentPercent);
            list.Add(InternPercent);

            return list;
        }

        private List<DataLine> avgProjectedRevenue(DataLine adjFTHours, DataLine resHrs, DataLine stuHrs, DataLine totalBudgetHours)
        {
            List<DataLine> list = new List<DataLine>();

            DataLine FTRevenue = getRevenue("Full Time", services.getFTRate(), adjFTHours.Values);
            DataLine resRevenue = getRevenue("Residents", services.getResRate(), resHrs.Values);
            DataLine stuRevenue = getRevenue("Students/Volunteers", services.getStuRate(), stuHrs.Values);
            //FT adjusted rev, needs temp data
            DataLine adjFTRevenue = getadjustedFTRevenue("Full Time Adjusted Revenue", FTRevenue);
            DataLine discountedRevenue = getDiscountedRevenue("Discounted Revenue(use for budget)", resRevenue, stuRevenue, adjFTRevenue);
            DataLine avgFee = getAverageFee("Average Fee", totalBudgetHours, discountedRevenue);
            DataLine totalRevenue = getDiscountedRevenue("Total Revenue", FTRevenue, resRevenue, stuRevenue);

            list.Add(FTRevenue);
            list.Add(resRevenue);
            list.Add(stuRevenue);
            list.Add(avgFee);
            list.Add(adjFTRevenue);
            list.Add(totalRevenue);
            list.Add(discountedRevenue);


            return list;
        }

        private DataLine getAverageFee(string name, DataLine budgetHours, DataLine revenue)
        {
            DataLine line = new ViewModels.DataLine();

            line.Name = name;
            line.Values = arrayServices.divideArrays(revenue.Values, budgetHours.Values);
            line.isAverage = true;

            return line;
        }

        private DataLine getadjustedFTRevenue(string name, DataLine fullTime)
        {
            DataLine line = new ViewModels.DataLine();

            line.Name = name;
            line.Values = fullTime.Values;

            return line;
        }


        private DataLine getDiscountedRevenue(string name, DataLine resident, DataLine student, DataLine fullTime)
        {
            DataLine line = new DataLine();
            line.Name = name;
            decimal[] values = arrayServices.combineArrays(resident.Values, student.Values, fullTime.Values);
            line.Values = values;

            return line;
        }

        public DataLine getRevenue(string name, decimal rate, decimal[] adjFTHours)
        {
            DataLine line = new DataLine();

            line.Name = name + " @" + rate + " per hour";

            decimal[] values = new decimal[12];
            var count = 0;
            foreach (var h in adjFTHours)
            {
                values[count] = rate * h;
                count++;
            }

            line.Values = values;

            return line;
        }

        private List<DataLine> combineLists(List<DataLine> destination, List<DataLine> source)
        {
            foreach (var item in source)
            {
                destination.Add(item);
            }

            return destination;
        }



        private DataLine counsellingHrsSubTotal(DataLine one, DataLine two, DataLine three, DataLine four)
        {
            DataLine newLine = new DataLine();
            decimal[] values = new decimal[12];

            values = arrayServices.combineArrays(one.Values, two.Values, three.Values, four.Values);

            newLine.Name = "Sub-Total Counselling Hours";
            newLine.Values = values;
            newLine.viewClass = "hour";


            return newLine;
        }

        private DataLine calculateTotalBudgetHours(string name, DataLine subTotal, decimal[] values)
        {
            DataLine line = new ViewModels.DataLine();
            line.Name = name;

            line.Values = arrayServices.combineArrays(subTotal.Values, values);
            line.viewClass = "hour";

            return line;
        }

        public DataLine calculateAdjFTHours(string name, decimal[] one, decimal[] two)
        {
            DataLine line = new DataLine();
            line.Name = name;
            line.Values = arrayServices.combineArrays(one, two);
            line.viewClass = "hour";

            return line;
        }

        private DataLine emptyLine()
        {
            return noDataLine("");
        }

        private DataLine noDataLine(string name)
        {
            DataLine line = new DataLine();
            line.Name = name;
            line.Values = null;

            return line;
        }

        private DataLine getHourPercent(string name, decimal[] numerator, decimal[] denominator)
        {
            DataLine line = new DataLine();
            line.Name = name;
            line.isPercent = true;
            decimal[] values = arrayServices.divideArrays(numerator, denominator);
            line.percentValues = formatValuesToPercent(values);

            return line;
        }


        private decimal[] formatValuesToPercent(decimal[] values)
        {
            for (var i = 0; i < 12; i++)
            {
                values[i] = values[i] * 100;
            }

            return values;
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

       


       

    }
}