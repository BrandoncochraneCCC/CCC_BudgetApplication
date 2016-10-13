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
    public class BugReportsController : ObjectInstanceController
    {
        // GET: BugReports
        public ActionResult Index(string sortOrder, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParam = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.ProgressSortParam = sortOrder == "inprogress" ? "not_inprogress" : "inprogress";
            ViewBag.ResolvedSortParam = sortOrder == "resolved" ? "not_resolved" : "resolved";
            List<BugReport> list = new List<BugReport>();
            var revenues = db.Revenues.Include(e => e.RevenueDatas);
            try
            {


                switch (sortOrder)//sort outout based on sortOrder
                {
                    case "date_desc":
                        list = db.BugReports.OrderBy(x => x.Date).ToList();
                        break;
                    case "inprogress":
                        list = db.BugReports.OrderBy(x => x.InProgress).ToList();
                        break;
                    case "not_inprogress":
                        list = db.BugReports.OrderByDescending(x => x.InProgress).ToList();
                        break;
                    case "resolved":
                        list = db.BugReports.OrderBy(x => x.Resolved).ToList();
                        break;
                    case "not_resolved":
                        list = db.BugReports.OrderByDescending(x => x.Resolved).ToList();
                        break;
                    default:
                        list = db.BugReports.OrderByDescending(x => x.Date).ToList();
                        break;
                }


            }
            catch (Exception ex)
            {
                log.Warn("bug log list failed", ex);
            }

            int pageSize = 25; //number of items to display on each page
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));

        }
        // GET: BugReports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BugReport bugReport = db.BugReports.Find(id);
            if (bugReport == null)
            {
                return HttpNotFound();
            }
            return View(bugReport);
        }

        // GET: BugReports/Create
        public ActionResult Create()
        {
            ViewBag.referrer = Request.UrlReferrer.ToString();
            return View();
        }

        // POST: BugReports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Date,description, Resolved, InProgress")] BugReport bugReport)
        {
            if (ModelState.IsValid)
            {
                bugReport.InProgress = false;
                bugReport.Resolved = false;
                bugReport.Date = DateTime.UtcNow;

                db.BugReports.Add(bugReport);
                db.SaveChanges();
                string outputMessage = "SUCCESS: Bug report submitted successfully! \n" +
                    "Submitted by: " + bugReport.Username.ToString() + "\n " +
                    "Timestamp: " + bugReport.Date.ToString() + " \n " +
                    "Description: " + bugReport.description.ToString();
                return RedirectToActionPermanent("ResultMessage", "Index", new { ResultMessage = outputMessage, url = ViewBag.referrer });
            }

            return View(bugReport);
        }

        // GET: BugReports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BugReport bugReport = db.BugReports.Find(id);
            if (bugReport == null)
            {
                return HttpNotFound();
            }
            return View(bugReport);
        }

        // POST: BugReports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Date,description")] BugReport bugReport)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bugReport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bugReport);
        }

        // GET: BugReports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BugReport bugReport = db.BugReports.Find(id);
            if (bugReport == null)
            {
                return HttpNotFound();
            }
            return View(bugReport);
        }

        // POST: BugReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BugReport bugReport = db.BugReports.Find(id);
            db.BugReports.Remove(bugReport);
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

        public void toggleProgress(int id)
        {
            var obj = db.BugReports.Find(id);
            obj.InProgress = !obj.InProgress;
            db.SaveChanges();

        }
        public void toggleResolved(int id)
        {
            var obj = db.BugReports.Find(id);
            obj.Resolved = !obj.Resolved;
            db.SaveChanges();

        }
    }
}
