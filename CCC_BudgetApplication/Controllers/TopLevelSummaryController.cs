/**
* Author - BRANDON COCHRANE
* Organization: Calgary Counselling Centre
* 
* deals with top level summary data
* service expense summaries located in ServiceExpenseController
* */

using Application.Controllers.GeneralExpenses;
using Application.Controllers.RevenueTable;
using Application.Controllers.ServiceExpenses;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class TopLevelSummaryController : ObjectInstanceController
    {
        private int year;
        private ArrayServices arrayServices = new ArrayServices();
        // GET: TopLevelSummary

        /**
         * Retrieves summaries to build top level summary view
         * 
         * */
        public ActionResult Index()
        {
            year = YEAR;
            RevenueTableController revenue = new RevenueTableController();
            ServiceExpenseSummaryController service = new ServiceExpenseSummaryController(year);
            GeneralExpense general = new GeneralExpense(year);

            TopLevelSummary result = new TopLevelSummary();
            result.Revenue = revenue.RevenueTable();
            result.Service = service.ExpenseTable();
            result.General = general.generalExpenseTable();
            result.totals = Totals(result.Revenue, result.Service, result.General);
            return View(result);
        }

        /**
         * build view to compare top level budget from year to year
         * */
        public ActionResult BudgetComparison()
        {          
            BudgetComparisonController controller = new BudgetComparisonController();
            BudgetComparison comparison = new BudgetComparison();
            comparison.Year = YEAR;
            comparison.Revenues = controller.getRevenueComparison();//compares revenues
            comparison.ServiceExpenses = controller.getServiceExpenseComparison();//compares service expenses
            comparison.GeneralExpenses = controller.getGeneralExpenseComparison();//compares general expenses
            comparison.RevenueTotal = controller.comparisonTotal(comparison.Revenues);//totals revenue comparison
            comparison.GAExpenseTotal = controller.comparisonTotal(comparison.GeneralExpenses);//totals general expense comparison
            comparison.ServiceExpenseTotal = controller.comparisonTotal(comparison.ServiceExpenses);//totals service expense comparison
            comparison.ExpenseTotal = controller.total(comparison.ServiceExpenseTotal, comparison.GAExpenseTotal, "Total Expenses");//totals expenses
            comparison.NetIncome = controller.netIncome(comparison.RevenueTotal, comparison.ExpenseTotal);
            comparison.Amortization = null;//not implemented
            comparison.Depreciation = null;//not implemented
            comparison.NetWithAmortization = comparison.NetIncome;//not implemented

            return View(comparison);
        }

        /**
         * builds total table for the top level summary
         * */
        private DataTable Totals(DataTable revenue, DataTable service, DataTable general)
        {
            DataTable table = new DataTable();
            table.dataList = topLevelTotals(revenue, service, general);
            return table;
        }

        /*
         * build data lines for the top level totals table
         * */
        private List<DataLine> topLevelTotals(DataTable revenue, DataTable service, DataTable general)
        {
            List<DataLine> list = new List<DataLine>();
            decimal[] rev = sumTable(revenue); //total revenue
            decimal[] exp = sumTable(service, general); //totalexpenses
            decimal[] surplus = arrayServices.subtractArrays(rev, exp);
            list.Add(createDataLine("Total Revenue", rev));
            list.Add(createDataLine("Total Expenses", exp));
            list.Add(createDataLine("Surplus(Deficit)", surplus));
            list.Add(createDataLine("Cumulative Surplus(Deficit)", cumulativeSurplus(surplus)));

            //amortization currently not implemented
            //amortization of defferred capital
            //depreciation
            //revenue including depreciation                    
            return list;
        }

        /*
         * create a data line woth no values
         * @param name the name of the data line
         * */
        private DataLine createDataLine(string name = "")
        {
            return createDataLine(name, new decimal[12]);
        }

        /*
         * builds data line with the values given
         * @param name name of the data line
         * @param values array of the values to display
         * */
        private DataLine createDataLine(string name, decimal[] values)
        {
            DataLine line = new DataLine();
            line.Name = name;
            line.Values = values;

            return line;
        }

        /**
         * cumulatively adds and array
         * @param surplus array to get values from
         * 
         * @return values array of cumultive values from input
         * */
        private decimal[] cumulativeSurplus(decimal[] surplus)
        {
            decimal[] values = new decimal[12];

            for(var i = 0; i < 12; i++)
            {
                if(i == 0)
                {
                    values[i] = surplus[i];
                }else
                {
                    values[i] = values[i - 1] + surplus[i];
                }
            }

            return values;
        }


        /*
         * sums all values in a table
         * @param table the table to add together
         * 
         * @return values array of added values from table
         * */
        private decimal[] sumTable (DataTable table)
        {
            decimal[] values = new decimal[12];

            foreach(var item in table.dataList)
            {
                for (var i = 0; i < 12; i++)
                {
                    values[i] += item.Values[i];
                }
            }

            return values;
        }

        /*
         * sums two tables together
         * @param one first table to add
         * @param two second table to add
         * 
         * @return the sum of both tables as an array
         * */
        private decimal[] sumTable(DataTable one, DataTable two)
        {
            decimal[] oneTotal = sumTable(one);
            decimal[] twoTotal = sumTable(two);



            return arrayServices.combineArrays(oneTotal, twoTotal);
        }
    }
}