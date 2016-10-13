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
    public class AmortizationsController : Controller
    {
        private BudgetDataEntities db = new BudgetDataEntities();

        // GET: Amortizations
        public ActionResult Index()
        {
            var amortizations = db.Amortizations.Include(a => a.Pool);
            return View(amortizations.ToList());
        }

        // GET: Amortizations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Amortization amortization = db.Amortizations.Find(id);
            if (amortization == null)
            {
                return HttpNotFound();
            }
            return View(amortization);
        }

        // GET: Amortizations/Create
        public ActionResult Create()
        {
            ViewBag.PoolID = new SelectList(db.Pools, "PoolID", "Name");
            return View();
        }

        // POST: Amortizations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AmortizationID,PoolID,PoolBalance,AccumulatedAmortization,Year")] Amortization amortization)
        {
            if (ModelState.IsValid)
            {
                db.Amortizations.Add(amortization);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PoolID = new SelectList(db.Pools, "PoolID", "Name", amortization.PoolID);
            return View(amortization);
        }

        // GET: Amortizations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Amortization amortization = db.Amortizations.Find(id);
            if (amortization == null)
            {
                return HttpNotFound();
            }
            ViewBag.PoolID = new SelectList(db.Pools, "PoolID", "Name", amortization.PoolID);
            return View(amortization);
        }

        // POST: Amortizations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AmortizationID,PoolID,PoolBalance,AccumulatedAmortization,Year")] Amortization amortization)
        {
            if (ModelState.IsValid)
            {
                db.Entry(amortization).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PoolID = new SelectList(db.Pools, "PoolID", "Name", amortization.PoolID);
            return View(amortization);
        }

        // GET: Amortizations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Amortization amortization = db.Amortizations.Find(id);
            if (amortization == null)
            {
                return HttpNotFound();
            }
            return View(amortization);
        }

        // POST: Amortizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Amortization amortization = db.Amortizations.Find(id);
            db.Amortizations.Remove(amortization);
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
