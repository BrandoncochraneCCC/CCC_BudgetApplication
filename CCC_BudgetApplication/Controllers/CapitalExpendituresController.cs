 /**
* Author - BRANDON COCHRANE
* Organization: Calgary Counselling Centre
* 
* handles requests for capital expenditures 
* */

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Application.ViewModels;
using Application.Controllers.CapitalExpenditures;
using PagedList;

namespace Application.Controllers
{
    public class CapitalExpendituresController : ObjectInstanceController
    {
        private int year;
        // GET: CapitalExpenditures
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ParentSortParm = sortOrder == "Parent" ? "parent_desc" : "Parent";
            ViewBag.numDataSortParm = sortOrder == "NumData" ? "NumData_desc" : "NumData";

            var expense = db.CapitalExpenditures.Select(g => g);
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
                                           || s.CapitalExpenditure2.Name.Contains(searchString)
                                           || s.CapitalExpenditureDatas.Count().ToString().Contains(searchString));
                }
                switch (sortOrder)//sorts data based on sortOrder
                {
                    case "name_desc":
                        expense = expense.OrderByDescending(s => s.Name);
                        break;
                    case "Parent":
                        expense = expense.OrderBy(s => s.CapitalExpenditure2.Name);
                        break;
                    case "parent_desc":
                        expense = expense.OrderByDescending(s => s.CapitalExpenditure2.Name);
                        break;
                    case "NumData":
                        expense = expense.OrderByDescending(s => s.CapitalExpenditureDatas.Count());
                        break;
                    case "NumData_desc":
                        expense = expense.OrderBy(s => s.CapitalExpenditureDatas.Count());
                        break;
                    default:
                        expense = expense.OrderBy(s => s.Name);
                        break;
                }
            }
            catch(Exception ex)
            {
                log.Info("capital expenditure index failed", ex);
            }
            

            int pageSize = 75; //number of units to display per page
            int pageNumber = (page ?? 1);
            return View(expense.ToPagedList(pageNumber, pageSize));
        }

        //displays summary for capital expenditures
        public ActionResult CapitalExpenditureSummary()
        {
            year = YEAR;
            CapitalExpenditureServices controller = new CapitalExpenditureServices(year);
            List<DataTable> result = controller.CapitalExpendituresTables(0);
            return View(result);
        }

        //displays data for capital expenditure
        public ActionResult CapitalExpenditure(int id = 0)
        {
            year = YEAR;
            CapitalExpenditureServices controller = new CapitalExpenditureServices(year);
            List<DataTable> result = controller.CapitalExpendituresTables(id);
            return View(result);
        }

        // GET: CapitalExpenditures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.CapitalExpenditure capitalExpenditure = db.CapitalExpenditures.Find(id);
            if (capitalExpenditure == null)
            {
                return HttpNotFound();
            }
            return View(capitalExpenditure);
        }

        // GET: CapitalExpenditures/Create
        public ActionResult Create()
        {
            ViewBag.ParentID = new SelectList(db.CapitalExpenditures, "CapitalExpenditureID", "Name");
            ViewBag.PoolID = new SelectList(db.Pools, "PoolID", "Name");
            return View();
        }

        // POST: CapitalExpenditures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CapitalExpenditureID,ParentID,PoolID,Name")] Models.CapitalExpenditure capitalExpenditure)
        {
            if (ModelState.IsValid)
            {
                db.CapitalExpenditures.Add(capitalExpenditure);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParentID = new SelectList(db.CapitalExpenditures, "CapitalExpenditureID", "Name", capitalExpenditure.ParentID);
            ViewBag.PoolID = new SelectList(db.Pools, "PoolID", "Name", capitalExpenditure.PoolID);
            return View(capitalExpenditure);
        }

        // GET: CapitalExpenditures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.CapitalExpenditure capitalExpenditure = db.CapitalExpenditures.Find(id);
            if (capitalExpenditure == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentID = new SelectList(db.CapitalExpenditures, "CapitalExpenditureID", "Name", capitalExpenditure.ParentID);
            ViewBag.PoolID = new SelectList(db.Pools, "PoolID", "Name", capitalExpenditure.PoolID);
            return View(capitalExpenditure);
        }

        // POST: CapitalExpenditures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CapitalExpenditureID,ParentID,PoolID,Name")] Models.CapitalExpenditure capitalExpenditure)
        {
            if (ModelState.IsValid)
            {
                db.Entry(capitalExpenditure).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentID = new SelectList(db.CapitalExpenditures, "CapitalExpenditureID", "Name", capitalExpenditure.ParentID);
            ViewBag.PoolID = new SelectList(db.Pools, "PoolID", "Name", capitalExpenditure.PoolID);
            return View(capitalExpenditure);
        }

        // GET: CapitalExpenditures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.CapitalExpenditure capitalExpenditure = db.CapitalExpenditures.Find(id);
            if (capitalExpenditure == null)
            {
                return HttpNotFound();
            }
            return View(capitalExpenditure);
        }

        // POST: CapitalExpenditures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Models.CapitalExpenditure capitalExpenditure = db.CapitalExpenditures.Find(id);
            db.CapitalExpenditures.Remove(capitalExpenditure);
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
    }
}
