/**
* Author - BRANDON COCHRANE
* Organization: Calgary Counselling Centre
* 
* controller for budget comparison
* */

using Application.Controllers.GeneralExpenses;
using Application.Controllers.Queries;
using Application.Controllers.RevenueTable;
using Application.Controllers.ServiceExpenses;
using Application.ViewModels;
using System;
using System.Collections.Generic;


namespace Application.Controllers
{
    public class BudgetComparisonController : ObjectInstanceController
    {
        int year;
        BudgetComparisonQueries queries;
        // GET: BudgetComparison

        public BudgetComparisonController()
        {
            year = YEAR;
            queries = new BudgetComparisonQueries(year);
        }
        //compares revenues
        public List<Comparison> getRevenueComparison()
        {
            RevenueTableController controller = new RevenueTableController();
            List<Comparison> list = new List<Comparison>();
            try
            {
                var revenues = queries.getRevenueComparisons();
                foreach (var r in revenues)
                {
                    var revenueLine = controller.getChildren(r.RevenueID);
                    decimal currentBudget = ARRAYSERVICES.sumArray(revenueLine.Values);
                    var obj = new Comparison(r.Revenue.Name, r.PrevBudget, currentBudget, r.PrevActual, year, r.RevenueID);
                    list.Add(obj);
                }
            }
            catch(Exception ex)
            {
                log.Info("revenue comparison failed", ex);
            }
            

            return list;
        }
        //compares service expenses
        public List<Comparison> getServiceExpenseComparison()
        {
            List<Comparison> list = new List<Comparison>();
            ServiceExpenseSummaryController controller = new ServiceExpenseSummaryController(year);
            try
            {
                var expenses = queries.getServiceExpenseComparisons();
                foreach (var e in expenses)
                {
                    var line = controller.expenseDataLine(e.ServiceExpenseID);
                    decimal currentBudget = ARRAYSERVICES.sumArray(line.Values);
                    var obj = new Comparison(e.ServiceExpense.Name, e.PrevBudget, currentBudget, e.PrevActual, year, e.ServiceExpenseID);
                    list.Add(obj);
                }
            }
            catch(Exception ex)
            {
                log.Info("service comparison failed", ex);
            }
            

            return list;
        }
        //compares general expenses
        public List<Comparison> getGeneralExpenseComparison()
        {
            List<Comparison> list = new List<Comparison>();
            GeneralExpense controller = new GeneralExpense(year);
            try
            {
                var expenses = queries.getGAExpenseComparisons();
                foreach (var e in expenses)
                {
                    var line = controller.expenseDataLine(e.GAGroupID);
                    decimal currentBudget = ARRAYSERVICES.sumArray(line.Values);
                    var obj = new Comparison(e.GAGroup.Name, e.PrevBudget, currentBudget, e.PrevActual, year, e.GAGroupID);
                    list.Add(obj);
                }
            }
            catch(Exception ex)
            {
                log.Info("general comparison failed", ex);
            }
            

            return list;
        }
        //totals single comparison tables
        public Comparison comparisonTotal(List<Comparison> data)
        {
            decimal prevBud = 0;
            decimal prevActual = 0;
            decimal currBud = 0;
            foreach(var d in data)
            {
                prevBud += d.BudgetedPrev;
                prevActual += d.ActualPrev;
                currBud += d.BudgetedCurrent;
            }

            return new Comparison("Total", prevBud, currBud, prevActual, year, 0);
        }
        //totals comparison tables
        public Comparison total(Comparison one, Comparison two, string name = "Total")
        {
            decimal prevBud = one.BudgetedPrev + two.BudgetedPrev;
            decimal prevActual = one.ActualPrev + two.ActualPrev;
            decimal currBud = one.BudgetedCurrent + two.BudgetedCurrent;

            return new Comparison(name, prevBud, currBud, prevActual, year, 0);
        }
        //calculates net income from comparison data
        public Comparison netIncome(Comparison revenue, Comparison expense, string name = "Net Income")
        {
            decimal prevBud = revenue.BudgetedPrev - expense.BudgetedPrev;
            decimal prevActual = revenue.ActualPrev - expense.ActualPrev;
            decimal currBud = revenue.BudgetedCurrent - expense.BudgetedCurrent;

            return new Comparison(name, prevBud, currBud, prevActual, year, 0);
        }

    }
}