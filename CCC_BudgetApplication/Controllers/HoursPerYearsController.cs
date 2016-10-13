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
    public class HoursPerYearsController : Controller
    {
        private BudgetDataEntities db = new BudgetDataEntities();

        // GET: HoursPerYears
        public ActionResult Index()
        {
            return View(db.HoursPerYears.ToList());
        }

        [Authorize(Roles = "BudgetSysAdmin")]
        // GET: HoursPerYears/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoursPerYear hoursPerYear = db.HoursPerYears.Find(id);
            if (hoursPerYear == null)
            {
                return HttpNotFound();
            }
            return View(hoursPerYear);
        }

        // GET: HoursPerYears/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HoursPerYears/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Year,Hours")] HoursPerYear hoursPerYear)
        {
            if (ModelState.IsValid)
            {
                db.HoursPerYears.Add(hoursPerYear);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hoursPerYear);
        }

        // GET: HoursPerYears/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoursPerYear hoursPerYear = db.HoursPerYears.Find(id);
            if (hoursPerYear == null)
            {
                return HttpNotFound();
            }
            return View(hoursPerYear);
        }

        // POST: HoursPerYears/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Year,Hours")] HoursPerYear hoursPerYear)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hoursPerYear).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hoursPerYear);
        }

        // GET: HoursPerYears/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoursPerYear hoursPerYear = db.HoursPerYears.Find(id);
            if (hoursPerYear == null)
            {
                return HttpNotFound();
            }
            return View(hoursPerYear);
        }

        // POST: HoursPerYears/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HoursPerYear hoursPerYear = db.HoursPerYears.Find(id);
            db.HoursPerYears.Remove(hoursPerYear);
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
