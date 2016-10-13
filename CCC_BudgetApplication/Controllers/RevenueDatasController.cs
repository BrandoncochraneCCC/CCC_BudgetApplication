
/**
* Author - BRANDON COCHRANE
* Organization: Calgary Counselling Centre
* 
* builds dataTables of children for given revenue
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

namespace Application.Controllers
{
    public class RevenueDatasController : ObjectInstanceController
    {
    
        // GET: RevenueDatas
        //Gets the children of given revenue and builds datatable
        public ActionResult Index(int id)
        {

            List<DataLine> list = new List<DataLine>();
            try
            {
                var data = db.RevenueDatas.Where(x => x.RevenueID == id);
                var years = data.Select(x => x.Date.Year).Distinct().ToList();

                foreach (var x in years)
                {
                    decimal[] values = new decimal[12];

                    var query = from r in data
                                where r.Date.Year == x
                                select r;

                    for (var i = 0; i < 12; i++)
                    {
                        var result = from r in query
                                     where r.Date.Month == i + 1
                                     select r.Value;
                        values[i] += (Decimal)result.FirstOrDefault();
                    }
                    DataLine line = new DataLine();
                    var n = from r in db.Revenues
                            where r.RevenueID == id
                            select r.Name;
                    line.tableName = n.FirstOrDefault().ToString();
                    line.Name = x.ToString();
                    line.Values = values;
                    list.Add(line);
                }
            }
            catch(Exception ex)
            {
                log.Info("revenue data index list failed", ex);
            }
            
            

            return View(list);
        }

        // GET: RevenueDatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RevenueData revenueData = db.RevenueDatas.Find(id);
            if (revenueData == null)
            {
                return HttpNotFound();
            }
            return View(revenueData);
        }

        // GET: RevenueDatas/Create
        public ActionResult Create(Revenue revenue, int year )
        {
            Data model = new Data();
            model.SourceID = revenue.RevenueID;
            model.SourceName = revenue.Name;
            model.Year = year;
            ViewBag.RevenueID = revenue.RevenueID;
            ViewBag.Year = year;
            return View();
        }

        // POST: RevenueDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RevenueDataID,RevenueID,Value,Date")] RevenueData revenueData, int year)
        {
            if (ModelState.IsValid)
            {
                db.RevenueDatas.Add(revenueData);
                db.SaveChanges();
                return RedirectToAction("Index", "RevenueDatas", new { RevenueID = revenueData.RevenueID , Year = year });
            }

            ViewBag.RevenueID = new SelectList(db.Revenues, "RevenueID", "Name", revenueData.RevenueID);
            return View(revenueData);
        }

        // GET: RevenueDatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RevenueData revenueData = db.RevenueDatas.Find(id);
            if (revenueData == null)
            {
                return HttpNotFound();
            }
            ViewBag.RevenueID = new SelectList(db.Revenues, "RevenueID", "Name", revenueData.RevenueID);
            return View(revenueData);
        }

        // POST: RevenueDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RevenueDataID,RevenueID,Value,Date")] RevenueData revenueData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(revenueData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RevenueID = new SelectList(db.Revenues, "RevenueID", "Name", revenueData.RevenueID);
            return View(revenueData);
        }

        // GET: RevenueDatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RevenueData revenueData = db.RevenueDatas.Find(id);
            if (revenueData == null)
            {
                return HttpNotFound();
            }
            return View(revenueData);
        }

        // POST: RevenueDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RevenueData revenueData = db.RevenueDatas.Find(id);
            db.RevenueDatas.Remove(revenueData);
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
        //edits data based on user input 
        public ActionResult EditData(RevenueData[] revDatas, string urlOfPage)
        {
            for (int i = 0; i < 12; i++)
            {
                revDatas[i].Date = getDate(i + 1, YEAR);
            }
            string url = urlOfPage;
            int id = revDatas[0].RevenueID;
            DateTime date;
            decimal value;
            decimal prev;

            for (int i = 0; i < revDatas.Length; i++)
            {
                date = revDatas[i].Date;
                value = revDatas[i].Value;
                var test = from r in db.RevenueDatas
                           where r.Date == date && r.RevenueID == id
                           select r;

                var test1 = test.FirstOrDefault();

                if (test1 == null && value > 0)
                {
                    prev = 0;
                    addRev(date, value, id);

                    ChangeLog.addChangeLog(url, value, prev, getRevenueName(id));
                }
                else if(test1 == null && value == 0)
                {
                    //do nothing
                }
                else if(test1 != null)
                {
                    prev = test1.Value;
                    
                    if(prev != value)
                    {
                        test1.Value = value;
                        ChangeLog.addChangeLog(url, value, prev, getRevenueName(id));                       
                    }               
                }
                else if(test1 == null && value < 0)
                {
                    addRev(date, value, id);
                    prev = 0;
                    ChangeLog.addChangeLog(url, value, prev, getRevenueName(id));
                }
                
            }
                db.SaveChanges();
            

            return Json(url, JsonRequestBehavior.AllowGet);
        }

        private string getRevenueName(int id)
        {
            var rev = from r in db.Revenues
                      where r.RevenueID == id
                      select r;
            return rev.FirstOrDefault().Name;
        }

        private void addRev(DateTime date, decimal value, int revID)
        {
            RevenueData rev = new RevenueData();
            rev.Date = date;
            rev.Value = value;
            rev.RevenueID = revID;

            db.RevenueDatas.Add(rev);
            db.SaveChanges();
        }

        private DateTime getDate(int month, int year)
        {
            DateTime date = new DateTime(year, month, 1);

            return date;
        }

        


    }
}
