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
    public class DeferredAmortizationsController : Controller
    {
        private BudgetDataEntities db = new BudgetDataEntities();

        // GET: DeferredAmortizations
        public ActionResult Index()
        {
            var deferredAmortizations = db.DeferredAmortizations.Include(d => d.Pool);
            return View(deferredAmortizations.ToList());
        }

        // GET: DeferredAmortizations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeferredAmortization deferredAmortization = db.DeferredAmortizations.Find(id);
            if (deferredAmortization == null)
            {
                return HttpNotFound();
            }
            return View(deferredAmortization);
        }

        // GET: DeferredAmortizations/Create
        public ActionResult Create()
        {
            ViewBag.PoolID = new SelectList(db.Pools, "PoolID", "Name");
            return View();
        }

        // POST: DeferredAmortizations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DeferredAmortizationID,PoolID,PoolBalance,AccumulatedAmortization,Year")] DeferredAmortization deferredAmortization)
        {
            if (ModelState.IsValid)
            {
                db.DeferredAmortizations.Add(deferredAmortization);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PoolID = new SelectList(db.Pools, "PoolID", "Name", deferredAmortization.PoolID);
            return View(deferredAmortization);
        }

        // GET: DeferredAmortizations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeferredAmortization deferredAmortization = db.DeferredAmortizations.Find(id);
            if (deferredAmortization == null)
            {
                return HttpNotFound();
            }
            ViewBag.PoolID = new SelectList(db.Pools, "PoolID", "Name", deferredAmortization.PoolID);
            return View(deferredAmortization);
        }

        // POST: DeferredAmortizations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DeferredAmortizationID,PoolID,PoolBalance,AccumulatedAmortization,Year")] DeferredAmortization deferredAmortization)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deferredAmortization).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PoolID = new SelectList(db.Pools, "PoolID", "Name", deferredAmortization.PoolID);
            return View(deferredAmortization);
        }

        // GET: DeferredAmortizations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeferredAmortization deferredAmortization = db.DeferredAmortizations.Find(id);
            if (deferredAmortization == null)
            {
                return HttpNotFound();
            }
            return View(deferredAmortization);
        }

        // POST: DeferredAmortizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeferredAmortization deferredAmortization = db.DeferredAmortizations.Find(id);
            db.DeferredAmortizations.Remove(deferredAmortization);
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
