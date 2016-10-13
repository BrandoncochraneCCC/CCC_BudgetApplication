/**
* Author - BRANDON COCHRANE
* Organization: Calgary Counselling Centre
* 
* used to create and edit general expenses
* */

using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Application.Models;
using System;

namespace Application.Controllers
{
    public class GAGroupsController : ObjectInstanceController
    {
        

        // GET: GAGroups
        public ActionResult Index()
        {
            var gAGroups = db.GAGroups.Include(g => g.Account).Include(g => g.GAGroup2);
            return View(gAGroups.ToList());
        }

        // GET: GAGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GAGroup gAGroup = db.GAGroups.Find(id);
            if (gAGroup == null)
            {
                return HttpNotFound();
            }
            return View(gAGroup);
        }

        // GET: GAGroups/Create
        public ActionResult Create()
        {
            ViewBag.AccountNum = new SelectList(db.Accounts, "AccountNum", "Name");
            ViewBag.ParentID = new SelectList(db.GAGroups, "GAGroupID", "Name");
            return View();
        }

        // POST: GAGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToRouteResult Create([Bind(Include = "GAGroupID,ParentID,Name,AccountNum")] GAGroup gAGroup)
        {
            if (ModelState.IsValid)
            {
                db.GAGroups.Add(gAGroup);
                db.SaveChanges();
                return RedirectToActionPermanent("GeneralExpense", "GeneralExpenses", new { expenseID = gAGroup.GAGroupID });
            }

            ViewBag.AccountNum = new SelectList(db.Accounts, "AccountNum", "Name", gAGroup.AccountNum);
            ViewBag.ParentID = new SelectList(db.GAGroups, "GAGroupID", "Name", gAGroup.ParentID);
            return RedirectToActionPermanent("Index");
        }

        // GET: GAGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GAGroup gAGroup = db.GAGroups.Find(id);
            if (gAGroup == null)
            {
                return HttpNotFound();
            }
            ObjectInstanceController obj = new ObjectInstanceController();
            ViewBag.AccountNum = obj.accounts(gAGroup.AccountNum).AsEnumerable();
            if(gAGroup.ParentID != null)
            {
                ViewBag.ParentID = obj.general(gAGroup.ParentID).AsEnumerable();
            }
            else
            {
                ViewBag.ParentID = obj.general().AsEnumerable();
            }
            return View(gAGroup);
        }

        // POST: GAGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GAGroupID,ParentID,Name,AccountNum")] GAGroup gAGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gAGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ObjectInstanceController obj = new ObjectInstanceController();

            ViewBag.AccountNum = obj.general(gAGroup.AccountNum);
            ViewBag.ParentID = obj.service(gAGroup.ParentID).AsEnumerable();
            return View(gAGroup);
        }

        // GET: GAGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GAGroup gAGroup = db.GAGroups.Find(id);
            if (gAGroup == null)
            {
                return HttpNotFound();
            }
            return View(gAGroup);
        }

        // POST: GAGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GAGroup gAGroup = db.GAGroups.Find(id);
            db.GAGroups.Remove(gAGroup);
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
        public void EditGAExp(GAExpense[] GAExp, string Name)
        {
            try
            {
                var GAExpUpdated = modifyDates(GAExp);

                var id = GAExpUpdated[0].GroupID;
                DateTime date;
                decimal value;
                var urlName = Name;
                decimal prev;

                for (int i = 0; i < GAExpUpdated.Length; i++)
                {
                    date = GAExpUpdated[i].Date;
                    value = GAExpUpdated[i].Value;

                    var GA = getGAExpense(id, date);

                    if (GA == null && value > 0)
                    {
                        prev = 0;
                        addGA(id, value, date);
                        ChangeLog.addChangeLog(urlName, value, prev, getGAExpName(id));
                    }

                    else if (GA == null && value == 0)
                    {
                        //do nothing
                    }

                    else if (GA != null)
                    {
                        prev = GA.Value;
                        if (GA.Value != value)
                        {
                            GA.Value = value;
                            ChangeLog.addChangeLog(urlName, value, prev, getGAExpName(id));
                        }
                    }

                    else if (GA == null && value < 0)
                    {
                        addGA(id, value, date);
                        prev = 0;
                        ChangeLog.addChangeLog(urlName, value, prev, getGAExpName(id));
                    }
                }
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                log.Info("editing GA expense threw exception", ex);
            }
            

        }

        private string getGAExpName(int id)
        {
            var rev = from r in db.GAGroups
                      where r.GAGroupID == id
                      select r;
            return rev.FirstOrDefault().Name;
        }

        private GAExpense[] modifyDates(GAExpense[] GAExp)
        {
            try
            {

            }catch(IndexOutOfRangeException ex)
            {
                log.Debug(ex.Message);
            }
            for (int i = 0; i < 12; i++)
            {
                GAExp[i].Date = getDate(i + 1, YEAR);
            }

            return GAExp;
        }

        private DateTime getDate(int month, int year)
        {
            DateTime date = new DateTime(year, month, 1);

            return date;
        }

        private GAExpense getGAExpense(int id, DateTime date)
        {
            var GAExp = from r in db.GAExpenses
                        where r.GroupID == id && r.Date == date
                        select r;

            return GAExp.FirstOrDefault();
        }

        private void addGA (int id, decimal value, DateTime date)
        {
            GAExpense GAtoAdd = new GAExpense();
            GAtoAdd.GroupID = id;
            GAtoAdd.Value = value;
            GAtoAdd.Date = date;

            db.GAExpenses.Add(GAtoAdd);
            db.SaveChanges();
        }

        [HttpPost]
        public void AddGAExp(GAExpense[] GAExp, string Name)
        {
            try
            {
                string[] split = Name.Split('*');
                string url = split[0];
                string GAName = split[1];
                int parentID = GAExp[0].GroupID;

                GAGroup GAtoAdd = new GAGroup();
                GAtoAdd.ParentID = parentID;
                GAtoAdd.Name = GAName;

                db.GAGroups.Add(GAtoAdd);
                db.SaveChanges();

                ChangeLog.addChangeLog(url, "New Item", "N/A", GAName);

                for (int i = 0; i < 12; i++)
                {
                    GAExp[i].GroupID = GAtoAdd.GAGroupID;
                }

                EditGAExp(GAExp, url);
            }
            catch (Exception ex)
            {
                log.Warn("adding new general expense threw error", ex);
            }

        }
    }
}
