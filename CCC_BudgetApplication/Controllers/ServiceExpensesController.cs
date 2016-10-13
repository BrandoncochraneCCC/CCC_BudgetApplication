/**
* Author - BRANDON COCHRANE
* Organization: Calgary Counselling Centre
* 
* modifying service expense data 
* */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Application.Models;
using PagedList;
namespace Application.Controllers
{
    public class ServiceExpensesController : ObjectInstanceController
    {

        /**
         * displays all service expenses as a list
         * @param sortOrder sorts output based on the value given
         * @param searchString modifies output containing values in the search string
         * @param currentFilter the current sorting and searching parameters
         * @param page the page to refresh the page to 
         * */
        // GET: ServiceExpenses
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {        
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ParentSortParm = sortOrder == "Parent" ? "parent_desc" : "Parent";
            ViewBag.numDataSortParm = sortOrder == "NumData" ? "NumData_desc" : "NumData";

            var expense = db.ServiceExpenses.Select(g => g);
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
                if (!String.IsNullOrEmpty(searchString))//gets data containing search string
                {
                    expense = expense.Where(s => s.Name.Contains(searchString)
                                           || s.ServiceExpense2.Name.Contains(searchString)
                                           || s.ServiceExpenseDatas.Count().ToString().Contains(searchString));
                }
            }
            catch(Exception ex)
            {
                log.Error("Service expense list search string", ex);
            }            

            switch (sortOrder)//sorts data based on sortOrder
            {
                case "name_desc":
                    expense = expense.OrderByDescending(s => s.Name);
                    break;
                case "Parent":
                    expense = expense.OrderBy(s => s.ServiceExpense2.Name);
                    break;
                case "parent_desc":
                    expense = expense.OrderByDescending(s => s.ServiceExpense2.Name);
                    break;
                case "NumData":
                    expense = expense.OrderByDescending(s => s.ServiceExpenseDatas.Count());
                    break;
                case "NumData_desc":
                    expense = expense.OrderBy(s => s.ServiceExpenseDatas.Count());
                    break;
                default:
                    expense = expense.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 75; //number of units to display per page
            int pageNumber = (page ?? 1);
            return View(expense.ToPagedList(pageNumber, pageSize));
        }

        // GET: ServiceExpenses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceExpense serviceExpense = db.ServiceExpenses.Find(id);
            if (serviceExpense == null)
            {
                return HttpNotFound();
            }
            return View(serviceExpense);
        }

        // GET: ServiceExpenses/Create
        public ActionResult Create()
        {
            ViewBag.AccountNum = new SelectList(db.Accounts, "AccountNum", "Name");
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name");
            ViewBag.ParentID = new SelectList(db.ServiceExpenses, "ServiceExpenseID", "Name");
            return View();
        }

        // POST: ServiceExpenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToRouteResult Create([Bind(Include = "ServiceExpenseID,AccountNum,ParentID,Name,DepartmentID")] ServiceExpense serviceExpense)
        {
            if (ModelState.IsValid)
            {
                db.ServiceExpenses.Add(serviceExpense);
                db.SaveChanges();
                return RedirectToActionPermanent("ServiceExpense", "ServiceExpense", new { expenseID = serviceExpense.ServiceExpenseID });
            }

            ViewBag.AccountNum = new SelectList(db.Accounts, "AccountNum", "Name", serviceExpense.AccountNum);
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", serviceExpense.DepartmentID);
            ViewBag.ParentID = new SelectList(db.ServiceExpenses, "ServiceExpenseID", "Name", serviceExpense.ParentID);
            return RedirectToActionPermanent("Index");
        }

        // GET: ServiceExpenses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceExpense serviceExpense = db.ServiceExpenses.Find(id);
            if (serviceExpense == null)
            {
                return HttpNotFound();
            }
            ObjectInstanceController obj = new ObjectInstanceController();

            ViewBag.AccountNum = obj.accounts(serviceExpense.AccountNum);
            ViewBag.ParentID = obj.service(serviceExpense.ParentID).AsEnumerable();
            return View(serviceExpense);
        }

        // POST: ServiceExpenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ServiceExpenseID,AccountNum,ParentID,Name,DepartmentID")] ServiceExpense serviceExpense)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviceExpense).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountNum = new SelectList(db.Accounts, "AccountNum", "Name", serviceExpense.AccountNum);
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", serviceExpense.DepartmentID);
            ViewBag.ParentID = new SelectList(db.ServiceExpenses, "ServiceExpenseID", "Name", serviceExpense.ParentID);
            return View(serviceExpense);
        }

        // GET: ServiceExpenses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceExpense serviceExpense = db.ServiceExpenses.Find(id);
            if (serviceExpense == null)
            {
                return HttpNotFound();
            }
            return View(serviceExpense);
        }

        // POST: ServiceExpenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServiceExpense serviceExpense = db.ServiceExpenses.Find(id);
            db.ServiceExpenses.Remove(serviceExpense);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public void EditServExp(ServiceExpenseData[] ServExp, string Name)
        {
            try
            {
                var ServExpUpdated = modifyDates(ServExp);

                string url = Name;
                int id = ServExpUpdated[0].ServiceExpenseID;
                DateTime date;
                decimal value;
                decimal prev;

                for (int i = 0; i < ServExpUpdated.Length; i++)
                {
                    date = ServExpUpdated[i].Date;
                    value = ServExpUpdated[i].Value;

                    var ser = getServiceExpenseData(id, date);

                    if (ser == null && value > 0)
                    {
                        prev = 0;
                        addServExp(id, value, date);
                        ChangeLog.addChangeLog(url, value, prev, getServExpName(id));
                    }
                    else if (ser == null && value == 0)
                    {
                        //do nothing
                    }
                    else if (ser != null)
                    {
                        prev = ser.Value;
                        if (ser.Value != value)
                        {
                            ser.Value = value;
                            ChangeLog.addChangeLog(url, value, prev, getServExpName(id));
                        }
                    }

                    else if (ser == null && value < 0)
                    {
                        addServExp(id, value, date);
                        prev = 0;
                        ChangeLog.addChangeLog(url, value, prev, getServExpName(id));
                    }
                }
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                log.Warn("Service expense did not edit", ex);
            }
            

            

        }

        private string getServExpName (int id)
        {
            var rev = from r in db.ServiceExpenses
                      where r.ServiceExpenseID == id
                      select r;
            return rev.FirstOrDefault().Name;
        }

        private ServiceExpenseData[] modifyDates(ServiceExpenseData[] ServExp)
        {
            try
            {
                if(ServExp != null)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        ServExp[i].Date = getDate(i + 1, YEAR);
                    }
                }

            }
            catch(Exception ex)
            {
                log.Info("cannot modify service expense data", ex);
            }

            return ServExp;
        }

        private DateTime getDate(int month, int year)
        {
            DateTime date = new DateTime(year, month, 1);

            return date;
        }

        private ServiceExpenseData getServiceExpenseData(int id, DateTime date)
        {
            var serExp = from r in db.ServiceExpenseDatas
                         where r.ServiceExpenseID == id && r.Date == date
                         select r;

            return serExp.FirstOrDefault();
        }
        
        private void addServExp(int id, decimal value, DateTime date)
        {
            try
            {
                ServiceExpenseData serToAdd = new ServiceExpenseData();
                serToAdd.ServiceExpenseID = id;
                serToAdd.Value = value;
                serToAdd.Date = date;

                db.ServiceExpenseDatas.Add(serToAdd);
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                log.Info("add service expense failed", ex);
            }

        } 
    }
}
