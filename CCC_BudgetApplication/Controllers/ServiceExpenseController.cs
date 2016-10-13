/**
* Author - BRANDON COCHRANE
* Organization: Calgary Counselling Centre
* 
* handles service expense summaries
* */


using Application.Controllers.Queries;
using Application.Controllers.ServiceExpenses;
using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace Application.Controllers
{
    public class ServiceExpenseController : ObjectInstanceController
    {

        private int year;
        // GET: ServiceExpense
        public ActionResult ServiceExpenseSummary()
        {
            year = YEAR;
            List<DataTable> result = new List<DataTable>();
            ServiceExpenseSummaryController controller = new ServiceExpenseSummaryController(year);
            result.Add(controller.ExpenseTable(0));

            return View(result);
        }

        //GET: service expense data
        public ActionResult ServiceExpense(int expenseID = 0)
        {
            year = YEAR;
            ServiceExpenseSummaryController controller = new ServiceExpenseSummaryController(year);
            List<DataTable> result = new List<DataTable>();
            try
            {
                if (expenseID == 2 || expenseID == 10) //merges contractor tables to display same view
                {
                    DataTable display = controller.ContractServices();
                    display.sourceID = expenseID;
                    result.Add(display);
                }
                else
                {
                    result.Add(controller.ExpenseTable(expenseID));
                }
                ServiceExpenseQueries q = new ServiceExpenseQueries(year);
                var expense = q.getExpense(expenseID);
                if (expenseID != 0)
                {
                    if (expense.ParentID == 2 || expense.ParentID == 10)
                    {
                        return View("Consultant", result);
                    }
                }
            }catch(Exception ex)
            {
                log.Error("Service expense table failure", ex);
            }
           
            return View(result);
        }

       

    }
}