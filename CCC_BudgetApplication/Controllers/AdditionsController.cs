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
    public class AdditionsController : ObjectInstanceController
    {

        // GET: Additions
        public ActionResult Index()
        {
            var additions = db.Additions.Include(a => a.Amortization).OrderBy(x => x.Amortization.Pool.Name);

            return View(additions.ToList());
        }

        // GET: Additions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var amortization = db.Amortizations.Where(x => x.PoolID == id && x.Year == YEAR).Select(x => x).SingleOrDefault();
            IQueryable<Addition> additions = db.Additions.Where(x => x.AmortizationID == amortization.AmortizationID).Select(x => x);
            if (additions == null)
            {
                return HttpNotFound();
            }
            return View(additions);
        }

        // GET: Additions/Create
        public ActionResult Create()
        {
            ViewBag.AmortizationID = new SelectList(db.Amortizations, "AmortizationID", "AmortizationID");
            return View();
        }

        // POST: Additions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdditionID,AmortizationID,Name,Value,Date")] Addition addition)
        {
            if (ModelState.IsValid)
            {
                var aID =  db.Amortizations
                    .Where(x => x.PoolID == addition.AmortizationID && x.Year == addition.Date.Year)
                    .Select(x => x.AmortizationID).SingleOrDefault();
                addition.AmortizationID = aID;
                db.Additions.Add(addition);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AmortizationID = new SelectList(db.Amortizations, "AmortizationID", "AmortizationID", addition.AmortizationID);
            return View(addition);
        }

        // GET: Additions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Addition addition = db.Additions.Find(id);
            if (addition == null)
            {
                return HttpNotFound();
            }
            ViewBag.AmortizationID = new SelectList(db.Amortizations, "AmortizationID", "AmortizationID", addition.AmortizationID);
            return View(addition);
        }

        // POST: Additions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdditionID,AmortizationID,Name,Value,Date,DeferredAmortizationID")] Addition addition)
        {
            if (ModelState.IsValid)
            {
                db.Entry(addition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AmortizationID = new SelectList(db.Amortizations, "AmortizationID", "AmortizationID", addition.AmortizationID);
            return View(addition);
        }

        // GET: Additions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Addition addition = db.Additions.Find(id);
            if (addition == null)
            {
                return HttpNotFound();
            }
            return View(addition);
        }

        // POST: Additions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Addition addition = db.Additions.Find(id);
            db.Additions.Remove(addition);
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
