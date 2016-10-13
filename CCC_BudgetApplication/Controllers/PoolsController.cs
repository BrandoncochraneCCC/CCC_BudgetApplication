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
    public class PoolsController : Controller
    {
        private BudgetDataEntities db = new BudgetDataEntities();

        // GET: Pools
        public ActionResult Index()
        {
            var pools = db.Pools.Include(p => p.Account);
            return View(pools.ToList());
        }

        // GET: Pools/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pool pool = db.Pools.Find(id);
            if (pool == null)
            {
                return HttpNotFound();
            }
            return View(pool);
        }

        // GET: Pools/Create
        public ActionResult Create()
        {
            ViewBag.AccountNum = new SelectList(db.Accounts, "AccountNum", "Name");
            return View();
        }

        // POST: Pools/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PoolID,Name,AccountNum,DepreciationRate,StraightLine")] Pool pool)
        {
            if (ModelState.IsValid)
            {
                db.Pools.Add(pool);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountNum = new SelectList(db.Accounts, "AccountNum", "Name", pool.AccountNum);
            return View(pool);
        }

        // GET: Pools/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pool pool = db.Pools.Find(id);
            if (pool == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountNum = new SelectList(db.Accounts, "AccountNum", "Name", pool.AccountNum);
            return View(pool);
        }

        // POST: Pools/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PoolID,Name,AccountNum,DepreciationRate,StraightLine")] Pool pool)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pool).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountNum = new SelectList(db.Accounts, "AccountNum", "Name", pool.AccountNum);
            return View(pool);
        }

        // GET: Pools/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pool pool = db.Pools.Find(id);
            if (pool == null)
            {
                return HttpNotFound();
            }
            return View(pool);
        }

        // POST: Pools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pool pool = db.Pools.Find(id);
            db.Pools.Remove(pool);
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
