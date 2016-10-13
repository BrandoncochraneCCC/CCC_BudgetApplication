
/**
* Author - BRANDON COCHRANE
* Organization: Calgary Counselling Centre
* 
* controlls resident data
* */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using Application.Models;
using System.Web;
using System.Web.Mvc;
using Application.ViewModels;
using Application.Controllers.Queries;

namespace Application.Controllers
{
    public class BursariesController : ObjectInstanceController
    {
        public static int RESIDENTTYPEID = 4;
        ArrayServices services = new ArrayServices();
        private int year;
        private EmployeeQueries queries;


        // GET: Bursaries
        //displays all resident data
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {            
            year = YEAR;
            queries = new EmployeeQueries(year);
            ResidentSummary summary = new ResidentSummary();
            List<Resident> list = new List<Resident>();


            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "fname_desc" : "";
            ViewBag.LastNameSortParm = sortOrder == "LName" ? "lname_desc" : "LName";
            ViewBag.StartSortParm = sortOrder == "StartDate" ? "startdate_desc" : "StartDate";
            ViewBag.EndSortParm = sortOrder == "EndDate" ? "enddate_desc" : "EndDate";

            ViewBag.TargetSortParam = sortOrder == "Target" ? "target_desc" : "Target";

            IQueryable<Employee> allResidents = db.Employees.Where(r => r.TypeID == RESIDENTTYPEID).Select(r => r);
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            try
            {
                if (!String.IsNullOrEmpty(searchString))//gets data containing search string
                {
                    allResidents = allResidents.Where(s => s.LastName.Contains(searchString)
                                           || s.FirstName.Contains(searchString)
                                           || s.StartDate.ToString().Contains(searchString)
                                           || s.EndDate.ToString().Contains(searchString));
                }
                switch (sortOrder)//sorts data based on sortOrder
                {
                    case "fname_desc":
                        allResidents = allResidents.OrderByDescending(r => r.FirstName).Select(r => r);
                        break;
                    case "LName":
                        allResidents = allResidents.OrderBy(r => r.LastName).Select(r => r);
                        break;
                    case "lname_desc":
                        allResidents = allResidents.OrderByDescending(r => r.LastName).Select(r => r);
                        break;
                    case "Target":
                        allResidents = allResidents.OrderBy(r => r.ResidentTargets.Sum(y => y.Hour)).Select(r => r);
                        break;
                    case "target_desc":
                        allResidents = allResidents.OrderByDescending(r => r.ResidentTargets.Sum(y => y.Hour)).Select(r => r);
                        break;
                    case "StartDate":
                        allResidents = allResidents.OrderBy(r => r.StartDate).Select(r => r);
                        break;
                    case "startdate_desc":
                        allResidents = allResidents.OrderByDescending(r => r.StartDate).Select(r => r);
                        break;
                    case "EndDate":
                        allResidents = allResidents.OrderBy(r => r.EndDate).Select(r => r);
                        break;
                    case "enddate_desc":
                        allResidents = allResidents.OrderByDescending(r => r.EndDate).Select(r => r);
                        break;
                    default:
                        allResidents = allResidents.OrderBy(r => r.FirstName).Select(r => r);
                        break;
                }


                foreach (var r in allResidents)
                {
                    if (HasBursaryForYear(r) || HasTargetForYear(r))
                    {
                        var resident = retrieveResidentData(r);

                        list.Add(resident);
                    }
                }
            }
            catch(Exception ex)
            {
                log.Info("bursaries index view failed", ex);
            }
            

            summary.residents = list;
            summary.totalTable = residentTotal(summary);
            summary.bursaryTable = bursaryTable(summary);


            return View(summary);
        }

        //builds bursary table
        private ViewModels.DataTable bursaryTable(ResidentSummary summary)
        {
            ViewModels.DataTable table = new ViewModels.DataTable();
            table.dataList = bursaryTableList(summary);
            return table;
        }

        //builds datalist for bursary table
        private List<DataLine> bursaryTableList(ResidentSummary summary)
        {
            List<DataLine> list = new List<DataLine>();
            
            if(summary != null)
            {
                var totalBursaryValue = db.UnchangingValues.Where(x => x.Name.ToLower().Equals("total bursary value") && x.Year == year).Select(x => x.Value).SingleOrDefault();
                var postitionBursary = db.UnchangingValues.Where(x => x.Name.ToLower().Equals("bursary") && x.Year == year).Select(x => x.Value).SingleOrDefault();

                decimal[] bursary = services.populateArray(totalBursaryValue);
                bursary = services.divideArrayByValue(bursary, 12);
                var tBursary = totalBursary(summary).Values;
                decimal[] dollarsAvailable = services.subtractArrays(bursary, tBursary);
                decimal[] cumulativeDollars = services.cumulativeArray(dollarsAvailable);
                decimal[] positionsAvailable = services.divideArrayByValue(dollarsAvailable, postitionBursary);
                positionsAvailable = services.roundUp(positionsAvailable);
                var clawback = totalClawback(summary);

                list.Add(createDataLine("Average Monthly Bursary Dollars", bursary));
                list.Add(createDataLine("Bursary Dollars Available", dollarsAvailable));
                list.Add(createDataLine("Cumulative Dollars Available", cumulativeDollars));
                list.Add(createDataLine("Full Time Positions Available", positionsAvailable, "hour"));
            }
                       
            return list;
        }

        //creates a dataline with given data
        private DataLine createDataLine(string name, decimal[] values, string viewClass = "")
        {
            DataLine line = new DataLine();

            line.Name = name;
            line.Values = values;
            line.viewClass = viewClass;

            return line;
        }

        //totals all resident data
        private ViewModels.DataTable residentTotal(ResidentSummary summary)
        {
            ViewModels.DataTable table = new ViewModels.DataTable();
            table.dataList = totalData(summary);
            return table;
        }

        //creates datalist for resident total table
        private List<DataLine> totalData(ResidentSummary summary)
        {
            List<DataLine> list = new List<DataLine>();
            var bursary = totalBursary(summary);
            var clawback = totalClawback(summary);
            list.Add(bursary);
            list.Add(clawback);
            list.Add(netLine(bursary.Values, clawback.Values));
            return list;
        }
        //creates net data line 
        private DataLine netLine(decimal[] one, decimal[] two)
        {
            DataLine line = new DataLine();
            line.Name = "Net";
            line.Values = net(one, two);
            return line;
        }

        //sorts values for net data line
        private decimal[] net(decimal[] one, decimal[] two)
        {
            decimal[] values = new decimal[12];

            for(var i = 0; i<one.Length; i++)
            {
                values[i] += one[i];
            }
            for (var i = 0; i < two.Length; i++)
            {
                values[i] += two[i];
            }

            return values;
        }

        //creates dataline for total bursary value
        private DataLine totalBursary(ResidentSummary summary)
        {
            DataLine line = new DataLine();
            line.Name = "Total Bursary";
            line.Values = bursaryTotal(summary);
            return line;
        }
        //creates dataline for total clawback value
        private DataLine totalClawback(ResidentSummary summary)
        {
            DataLine line = new DataLine();
            line.Name = "Total Clawback/Supervision";
            line.Values = clawbackTotal(summary);
            return line;
        }
        public decimal[] totalResidentCost()
        {
            List<Resident> list = new List<Resident>();
            ResidentSummary s = new ResidentSummary();
            decimal[] values = new decimal[12];
            try
            {
                IQueryable<Employee> allResidents = db.Employees.Where(r => r.TypeID == RESIDENTTYPEID).Select(r => r);

                foreach (var r in allResidents)
                {
                    if (HasBursaryForYear(r) || HasTargetForYear(r))
                    {
                        var resident = retrieveResidentData(r);

                        list.Add(resident);
                    }
                }
                s.residents = list;
                values = ARRAYSERVICES.subtractArrays(bursaryTotal(s), clawbackTotal(s));
            }
            catch(Exception ex)
            {
                log.Warn("resident cost exception", ex);
            }
            

            return values;
        }

        //sorts data for bursary total data line
        private decimal[] bursaryTotal(ResidentSummary summary)
        {
            decimal[] values = new decimal[12];

            foreach (var item in summary.residents)
            {
                for(var i= 0; i < 12; i++)
                {
                    values[i] += item.Bursaries[i];
                }
            }

            return values;
        }
        //sorts data for clawback total data line

        private decimal[] clawbackTotal(ResidentSummary summary)
        {
            decimal[] values = new decimal[12];

            foreach (var item in summary.residents)
            {
                for (var i = 0; i < 12; i++)
                {
                    values[i] += item.Clawbacks[i];
                }
            }

            return values;
        }

        //returns data for a single resident
        public ActionResult singleResident(int employeeID, int year)
        {
            this.year = year;
            List<Resident> list = new List<Resident>();
            try
            {
                Employee resident = db.Employees.Where(r => r.EmployeeID == employeeID).Select(r => r).FirstOrDefault();
                if (HasBursaryForYear(resident))
                {
                    var r = retrieveResidentData(resident);

                    list.Add(r);
                }
            }
            catch(Exception ex)
            {
                log.Error("resident error", ex);
            }
            

            return View("Resident", list);
        }

        //gets data for a single resident
        public Resident retrieveResidentData(Employee r)
        {
            Resident resident = new Resident();
            queries = new EmployeeQueries(YEAR);
            try
            {
                var data = from b in db.Bursaries
                           where b.Date.Year == YEAR
                           && b.EmployeeID == r.EmployeeID
                           select b;
                var residentTargets = db.ResidentTargets.Where(x => x.EmployeeID == r.EmployeeID).Select(x => x);

                resident.Employee = r;
                decimal[] bursaries = new decimal[12];
                decimal[] clawbacks = new decimal[12];
                decimal[] totals = new decimal[13];
                decimal tableTotal = 0;
                decimal[] targets = new decimal[12];

                var start = 0;
                var end = 12;
                if (r.StartDate != null)
                {
                    start = r.StartDate.Value.Month - 1;
                }
                if (r.EndDate != null)
                {
                    end = r.EndDate.Value.Month;
                }
                for (var i = start; i < end; i++)
                {
                    bursaries[i] = (Decimal)data.Where(x => x.Date.Month == i + 1).Select(x => x.BursaryValue).FirstOrDefault();
                    clawbacks[i] = (Decimal)data.Where(x => x.Date.Month == i + 1).Select(x => x.Clawback).FirstOrDefault();
                    targets[i] = (Decimal)residentTargets.Where(x => x.Date.Month == i + 1).Select(x => x.Hour).FirstOrDefault();
                    tableTotal += bursaries[i] - clawbacks[i];
                    totals[i + 1] = bursaries[i] - clawbacks[i];
                }


                totals[0] = tableTotal;
                resident.Bursaries = bursaries;
                resident.Clawbacks = clawbacks;
                resident.Targets = targets;
                resident.Totals = totals;
                resident.residentID = r.EmployeeID;
                resident.Action = "singleResident";
                resident.Controller = "Bursaries";
                resident.Name = r.FirstName + " " + r.LastName;
                resident.Year = year;
                resident.GroupTargets = GroupTargets(r.EmployeeID);
            }
            catch(Exception ex)
            {
                log.Error("resident data not retrieved", ex);
            }
            
            
            return resident;
        }


        private decimal[] GroupTargets(int id)
        {

            decimal[] values = new decimal[12];
            var result = queries.getGroupTarget(id);
            foreach (var item in result)
            {
                for (var i = 0; i < 12; i++)
                {
                    values[i] += result.Where(x => x.Date.Month == i + 1).Select(x => x.Hour).SingleOrDefault();
                }
            }

            return values;
        }

        //checks if resident has a bursary for current year
        private bool HasBursaryForYear(Employee resident)
        {
            var hasBursary = false;

            var bursary = db.Bursaries.Where(r => r.EmployeeID == resident.EmployeeID && r.Date.Year == year).Select(r => r);

            if (bursary.FirstOrDefault() != null)
            {
                hasBursary = true;
            }

            return hasBursary;
        }
        //checks if resident has target 
        private bool HasTargetForYear(Employee resident)
        {
            var hasTarget = false;

            var targets = db.ResidentTargets.Where(r => r.EmployeeID == resident.EmployeeID && r.Date.Year == year).Select(r => r);
        
            if(targets.FirstOrDefault() != null)
            {
                hasTarget = true;
            }

            return hasTarget;
        }

        // GET: Bursaries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bursary bursary = db.Bursaries.Find(id);
            if (bursary == null)
            {
                return HttpNotFound();
            }
            return View(bursary);
        }

        // GET: Bursaries/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName");
            return View();
        }

        // POST: Bursaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BursaryID,EmployeeID,BursaryValue,Clawback,Date")] Bursary bursary)
        {
            if (ModelState.IsValid)
            {
                db.Bursaries.Add(bursary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName", bursary.EmployeeID);
            return View(bursary);
        }

        // GET: Bursaries/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bursary bursary = db.Bursaries.Find(id);
            if (bursary == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName", bursary.EmployeeID);
            return View(bursary);
        }

        // POST: Bursaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BursaryID,EmployeeID,BursaryValue,Clawback,Date")] Bursary bursary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bursary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName", bursary.EmployeeID);
            return View(bursary);
        }

        // GET: Bursaries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bursary bursary = db.Bursaries.Find(id);
            if (bursary == null)
            {
                return HttpNotFound();
            }
            return View(bursary);
        }

        // POST: Bursaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bursary bursary = db.Bursaries.Find(id);
            db.Bursaries.Remove(bursary);
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
        public void EditTarget(ResidentTarget[] ResValue, string urlOfPage)
        {

            for (int i = 0; i < 12; i++)
            {
                ResValue[i].Date = getDate(i + 1, YEAR);
            }

            string url = urlOfPage;
            int id = ResValue[0].EmployeeID;
            DateTime date;
            decimal hour;
            decimal prev;

            for (int i = 0; i < ResValue.Length; i++)
            {
                date = ResValue[i].Date;
                hour = ResValue[i].Hour;
                var test = from r in db.ResidentTargets
                           where r.Date == date && r.EmployeeID == id
                           select r;

                var test1 = test.FirstOrDefault();

                if (test1 == null && hour > 0)
                {
                    prev = 0;
                    addHour(date, hour, id);

                    ChangeLog.addChangeLog(url, hour, prev, "Monthly Target");

                }
                else if (test1 == null && hour == 0)
                {
                    //do nothing
                }
                else if (test1 != null)
                {
                    prev = test1.Hour;

                    if(prev != hour)
                    {
                        test1.Hour = hour;
                        ChangeLog.addChangeLog(url, hour, prev, "Monthly Target");
                    }
                    

                }


            }
            db.SaveChanges();
        }

        private void addHour(DateTime date, decimal hour, int empID)
        {
            ResidentTarget tar = new ResidentTarget();
            tar.Date = date;
            tar.Hour = hour;
            tar.EmployeeID = empID;

            db.ResidentTargets.Add(tar);
            db.SaveChanges();
        }


        [HttpPost]
        public void EditBursary(Bursary[] ResValue, string urlOfPage)
        {
            for (int i = 0; i < 12; i++)
            {
                ResValue[i].Date = getDate(i + 1, YEAR);
            }

            int id = ResValue[0].EmployeeID;
            DateTime date;
            decimal bursary;
            decimal prev;

            for (int i = 0; i < ResValue.Length; i++)
            {
                date = ResValue[i].Date;
                bursary = ResValue[i].BursaryValue;
                var test = from r in db.Bursaries
                           where r.Date == date && r.EmployeeID == id
                           select r;

                var test1 = test.FirstOrDefault();

                if (test1 == null && bursary > 0)
                {
                    prev = 0;
                    addBursary(date, bursary, id);
                    ChangeLog.addChangeLog(urlOfPage, bursary, prev, "Monthly Bursary");
                }
                else if (test1 == null && bursary == 0)
                {
                    //do nothing
                }
                else if (test1 != null)
                {
                    prev = test1.BursaryValue;
                    if(prev != bursary)
                    {
                        test1.BursaryValue = bursary;
                        ChangeLog.addChangeLog(urlOfPage, bursary, prev, "Monthly Bursary");
                    }
                    

                }


            }
            db.SaveChanges();
        }

        private void addBursary(DateTime date, decimal bursaryValue, int empID)
        {
            Bursary bur = new Bursary();
            bur.Date = date;
            bur.BursaryValue = bursaryValue;
            bur.EmployeeID = empID;

            db.Bursaries.Add(bur);
            db.SaveChanges();
        }

        [HttpPost]
        public void EditClawback(Bursary[] ResValue, string urlOfPage)
        {
            for (int i = 0; i < 12; i++)
            {
                ResValue[i].Date = getDate(i + 1, YEAR);
            }

            int id = ResValue[0].EmployeeID;
            DateTime date;
            decimal claw;
            decimal prev;

            for (int i = 0; i < ResValue.Length; i++)
            {
                date = ResValue[i].Date;
                claw = ResValue[i].Clawback;
                var test = from r in db.Bursaries
                           where r.Date == date && r.EmployeeID == id
                           select r;

                var test1 = test.FirstOrDefault();

                if (test1 == null && claw > 0)
                {
                    prev = 0;
                    addClaw(date, claw, id);
                    ChangeLog.addChangeLog(urlOfPage, claw, prev, "Monthly Clawback");
                }
                else if (test1 == null && claw == 0)
                {
                    //do nothing
                }
                else if (test1 != null)
                {
                    prev = test1.Clawback;
                    if(prev != claw)
                    {
                        test1.Clawback = claw;
                        ChangeLog.addChangeLog(urlOfPage, claw, prev, "Monthly Clawback");
                    }
                   

                }
            }
            db.SaveChanges();
        }

        private void addClaw(DateTime date, decimal claw, int empID)
        {

            Bursary bur = new Bursary();
            bur.Date = date;
            bur.Clawback = claw;
            bur.EmployeeID = empID;

            db.Bursaries.Add(bur);
            db.SaveChanges();

            
        }

        private DateTime getDate(int month, int year)
        {
            DateTime date = new DateTime(year, month, 1);

            return date;
        }




    }
}
