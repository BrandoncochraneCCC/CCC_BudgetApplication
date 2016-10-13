/**
* Author - BRANDON COCHRANE
* Organization: Calgary Counselling Centre
* 
* Propogates data flow to build requested summary
* */


using Application.Controllers.GeneralExpenses;
using Application.Controllers.Queries;
using Application.Controllers.Services;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;


namespace Application.Controllers
{
    public class GeneralExpensesController : ObjectInstanceController
    {
        private int year;
        // GET: GeneralExpenses
        //Lists all general expenses
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ParentSortParm = sortOrder == "Parent" ? "parent_desc" : "Parent";
            ViewBag.numDataSortParm = sortOrder == "NumData" ? "NumData_desc" : "NumData";
            var expense = db.GAGroups.Select(g => g);
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            try
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    expense = expense.Where(s => s.Name.Contains(searchString)
                                           || s.GAGroup2.Name.Contains(searchString)
                                           || s.GAExpenses.Count().ToString().Contains(searchString));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        expense = expense.OrderByDescending(s => s.Name);
                        break;
                    case "Parent":
                        expense = expense.OrderBy(s => s.GAGroup2.Name);
                        break;
                    case "parent_desc":
                        expense = expense.OrderByDescending(s => s.GAGroup2.Name);
                        break;
                    case "NumData":
                        expense = expense.OrderByDescending(s => s.GAExpenses.Count());
                        break;
                    case "NumData_desc":
                        expense = expense.OrderBy(s => s.GAExpenses.Count());
                        break;
                    default:
                        expense = expense.OrderBy(s => s.Name);
                        break;
                }
            }
            catch(Exception ex)
            {
                log.Warn("General Expenses index list failure", ex);
            }
            

            int pageSize = 75;
            int pageNumber = (page ?? 1);
            return View(expense.ToPagedList(pageNumber, pageSize));
        }

        //Builds general expense summary
        public ActionResult GeneralExpenseSummary()
        {
            year = YEAR;
            List<DataTable> result = new List<DataTable>();
            GeneralExpense controller = new GeneralExpense(year);
            result.Add(controller.generalExpenseTable(0));

            return View(result);
        }


        //builds DataTable for given expense
        public ActionResult GeneralExpense(int expenseID = 0)
        {
            year = YEAR;
            List<DataTable> result = new List<DataTable>();
            GeneralExpense controller = new GeneralExpense(year);
            result = controller.generalExpenseTables(expenseID);

            return View(result);
        }

        //builds ITSummary view
        public ActionResult ITSummary()
        {
            return View(new ITSummaryViewModel());
        }

        //gets expense table to be used in a summary elsewhere
        public List<DataTable> GeneralExpenseViewModel(int expenseID = 0)
        {
            year = YEAR;
            List<DataTable> result = new List<DataTable>();
            GeneralExpense controller = new GeneralExpense(year);
            return controller.generalExpenseTables(expenseID);

        }

        //builds travel expense view
        public ActionResult TravelExpense(int expenseID = 0)
        {
            year = YEAR;
            List<DataTable> result = new List<DataTable>();
            GeneralExpense controller = new GeneralExpense(year);
            result = controller.travelTable(expenseID);

            return View(result);
        }

        //builds GST Expense view
        public ActionResult GSTExpense(int expenseID = 0)
        {
            year = YEAR;
            List<DataTable> result = new List<DataTable>();
            GeneralExpense controller = new GeneralExpense(year);
            result = controller.GSTExpenseTable(expenseID);

            return View("GeneralExpense" ,result);
        }

        //builds existing Hardware view
        public ActionResult ExistingHardware()
        {
            year = YEAR;
            GeneralExpense controller = new GeneralExpense(year);
            var result = controller.ExistingHardware();

            return View(result);

        }

        //creates view model for existingHardware
        public ExistingHardwareViewModel ExistingHardwareViewModel()
        {
            year = YEAR;
            GeneralExpense controller = new GeneralExpense(year);
            return controller.ExistingHardware();

        }

        //creates view for new hardware additions
        public ActionResult HardwareAdditions()
        {
            year = YEAR;
            List<DataTable> result = new List<DataTable>();
            GeneralExpense controller = new GeneralExpense(year);
            result = controller.generalExpenseTables(16);
            return View("GeneralExpense", result);
        }

        //expands item list for data
        public ActionResult ExpandExisting(int ID, string popUp)
        {

            ViewBag.element = "element" + ID;
            ViewBag.popUp = popUp;

            GeneralExpenseQueries queries = new GeneralExpenseQueries(year);
            return PartialView(queries.getExistingHardware(ID));
        }

    }
}