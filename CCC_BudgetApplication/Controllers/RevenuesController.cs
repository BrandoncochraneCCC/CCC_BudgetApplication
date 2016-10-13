/**
* Author - BRANDON COCHRANE
* Organization: Calgary Counselling Centre
*
* Handles Requests regarding revenues
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
using Application.ViewModels;
using Application.Controllers.RevenueTable;
using PagedList;


namespace Application.Controllers
{
    public class RevenuesController : ObjectInstanceController
    {
        private int year;
        public ActionResult Index()
        {
            year = YEAR;
            List<ViewModels.DataTable> tables = new List<ViewModels.DataTable>();
            RevenueTableController controller = new RevenueTableController();
            tables.Add(controller.RevenueTable(0));

            return View(tables);
        }

        //GET: lists all revenues 
        //Sorts data based on parameters
        public ActionResult ListAll(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.ParentSortParm = sortOrder == "Parent" ? "parent_desc" : "Parent";
            ViewBag.numDataSortParm = sortOrder == "NumData" ? "NumData_desc" : "NumData";

            var revenues = db.Revenues.Include(e => e.RevenueDatas);
            try
            {
                
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                ViewBag.CurrentFilter = searchString;

                if (!String.IsNullOrEmpty(searchString))//filters output for searchString
                {
                    revenues = revenues.Where(s => s.Name.Contains(searchString)
                                           || s.Revenue2.Name.Contains(searchString)
                                           || s.RevenueDatas.Count().ToString().Contains(searchString));
                }
                switch (sortOrder)//sort outout based on sortOrder
                {
                    case "name_desc":
                        revenues = revenues.OrderByDescending(s => s.Name);
                        break;
                    case "Parent":
                        revenues = revenues.OrderBy(s => s.Revenue2.Name);
                        break;
                    case "parent_desc":
                        revenues = revenues.OrderByDescending(s => s.Revenue2.Name);
                        break;
                    case "NumData":
                        revenues = revenues.OrderByDescending(s => s.RevenueDatas.Count());
                        break;
                    case "NumData_desc":
                        revenues = revenues.OrderBy(s => s.RevenueDatas.Count());
                        break;
                    default:
                        revenues = revenues.OrderBy(s => s.Name);
                        break;
                }

                
            }
            catch(Exception ex)
            {
                log.Warn("revenue list failed", ex);
            }
            int pageSize = 75; //number of items to display on each page
            int pageNumber = (page ?? 1);
            return View(revenues.ToPagedList(pageNumber, pageSize));

        }



        // GET: Revenues/Create
        public ActionResult Create()
        {
            ViewBag.ParentID = new SelectList(db.Revenues, "RevenueID", "Name");
            return View();
        }

        // POST: Revenues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToRouteResult Create([Bind(Include = "RevenueID,ParentID,Name")] Revenue revenue)
        {
            if (ModelState.IsValid)
            {
                db.Revenues.Add(revenue);
                db.SaveChanges();
                return RedirectToActionPermanent("Index", "RevenueGroup", new { revenueID = revenue.RevenueID });
            }

            ViewBag.ParentID = new SelectList(db.Revenues, "RevenueID", "Name", revenue.ParentID);
            return RedirectToActionPermanent("Index");
        }

        // GET: Revenues/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Revenue revenue = db.Revenues.Find(id);
            if (revenue == null)
            {
                return HttpNotFound();
            }
            //ViewBag.ParentID = new SelectList(db.Revenues, "RevenueID", "Name", revenue.ParentID);
            ObjectInstanceController obj = new ObjectInstanceController();
            ViewBag.ParentID = obj.revenues(revenue.ParentID).AsEnumerable();
            return View(revenue);
        }

        // POST: Revenues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RevenueID,ParentID,Name")] Revenue revenue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(revenue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentID = new SelectList(db.Revenues, "RevenueID", "Name", revenue.ParentID);
            return View(revenue);
        }

        // GET: Revenues/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Revenue revenue = db.Revenues.Find(id);
            if (revenue == null)
            {
                return HttpNotFound();
            }
            return View(revenue);
        }

        // POST: Revenues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Revenue revenue = db.Revenues.Find(id);
            db.Revenues.Remove(revenue);
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
        //POST: adds new revenue to database
        public JsonResult Add(Revenue r, string url)
        {

            try
            {

            }catch(Exception ex)
            {
                log.Error("Adding revenue exception", ex);
            }
            string result = "";

            int PrevRevID = r.RevenueID;

            var Name = r.Name;

            Revenue revToAdd = new Revenue();

            revToAdd.ParentID = PrevRevID;
            revToAdd.Name = Name;

            db.Revenues.Add(revToAdd);
            db.SaveChanges();

            var value = "New Item";
            var prev = "";
            ChangeLog.addChangeLog(url, value, prev, getRevenueName(revToAdd.RevenueID));

            result += revToAdd.RevenueID;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string getRevenueName(int id)
        {
            var rev = from r in db.Revenues
                      where r.RevenueID == id
                      select r;
            return rev.FirstOrDefault().Name;
        }

        [HttpPost]
        //POST: find revenue by name
        public JsonResult Find(Revenue r)
        {
            var RevID = db.Revenues.Where(x => x.Name == r.Name).Select(x => x.RevenueID).SingleOrDefault();
            return Json(RevID, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //POST: adds revenue with no children
        public JsonResult AddNoChildren(Revenue r)
        {
            string result = string.Empty;
            Revenue revToAdd = new Revenue();
            var Name = r.Name;
            var RevenueID = r.RevenueID;
            revToAdd.ParentID = r.RevenueID;
            revToAdd.Name = r.Name;
            db.Revenues.Add(revToAdd);
            db.SaveChanges();

            result += revToAdd.RevenueID;


            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
