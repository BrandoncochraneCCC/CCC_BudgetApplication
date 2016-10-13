using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Application.Models;

namespace Application.Controllers
{
    public class ChangeLogsController : Controller
    {
        private BudgetDataEntities db = new BudgetDataEntities();
        public static readonly log4net.ILog log = LogHelper.GetLogger();

        // GET: ChangeLogs
        public ActionResult Index()
        {
            return View(db.ChangeLogs.OrderByDescending(x => x.TimeStamp).ToList());
        }


        public void addChangeLog(string URL, DateTime NewData, DateTime OldData, string Row = "N/A", string User = "Developer")
        {
            addChangeLog(URL, NewData.ToString(), OldData.ToString(), Row, User);
        }

        public void addChangeLog(string URL, decimal NewData, decimal OldData, string Row = "N/A", string User = "Developer")
        {
            addChangeLog(URL, NewData.ToString(), OldData.ToString(), Row, User);
        }

        public void addChangeLog(string URL, string NewData, string OldData = "N/A", string Row = "N/A", string User = "Developer")
        {
            try
            {
                ChangeLog item = new ChangeLog();
                item.URL = URL;
                item.Row = Row;
                item.Old = OldData;
                item.New = NewData;
                item.TimeStamp = DateTime.Now;
                item.User = User; // change default value for deployment

                db.ChangeLogs.Add(item);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                log.Info("change log object not created", ex);
            }
        }

        // GET: ChangeLogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChangeLog changeLog = db.ChangeLogs.Find(id);
            if (changeLog == null)
            {
                return HttpNotFound();
            }
            return View(changeLog);
        }

        // GET: ChangeLogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChangeLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,URL,Row,Old,New,TimeStamp,User")] ChangeLog changeLog)
        {
            if (ModelState.IsValid)
            {
                db.ChangeLogs.Add(changeLog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(changeLog);
        }

        // GET: ChangeLogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChangeLog changeLog = db.ChangeLogs.Find(id);
            if (changeLog == null)
            {
                return HttpNotFound();
            }
            return View(changeLog);
        }

        // POST: ChangeLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,URL,Row,Old,New,TimeStamp,User")] ChangeLog changeLog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(changeLog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(changeLog);
        }

        // GET: ChangeLogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChangeLog changeLog = db.ChangeLogs.Find(id);
            if (changeLog == null)
            {
                return HttpNotFound();
            }
            return View(changeLog);
        }

        // POST: ChangeLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChangeLog changeLog = db.ChangeLogs.Find(id);
            db.ChangeLogs.Remove(changeLog);
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
