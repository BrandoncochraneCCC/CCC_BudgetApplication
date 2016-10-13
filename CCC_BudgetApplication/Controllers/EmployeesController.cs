/**
* Author - BRANDON COCHRANE
* Organization: Calgary Counselling Centre
* 
* handles calls regarding employees
* */

using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Application.Models;
using Application.ViewModels;
using Application.Controllers.Employees;
using PagedList;
using Application.Controllers.Services;

namespace Application.Controllers
{
    public class EmployeesController : ObjectInstanceController
    {

        private int year;
        // GET: Employees
        //lists all employees based on search and sort parameters
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "firstName_desc" : "";
            ViewBag.LastNameSortParm = sortOrder == "lastname" ? "lastname_desc" : "lastname";
            ViewBag.DeptSortParm = sortOrder == "Department" ? "dept_desc" : "Department";
            ViewBag.TypeSortParm = sortOrder == "Type" ? "type_desc" : "Type";
            ViewBag.StartSortParm = sortOrder == "Start" ? "start_desc" : "Start";
            ViewBag.EndSortParm = sortOrder == "End" ? "end_desc" : "End";
            var employees = db.Employees.Include(e => e.Department).Include(e => e.EmployeeType);
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
                if (!String.IsNullOrEmpty(searchString)) //filters results by search paramter
                {
                    employees = employees.Where(s => s.LastName.Contains(searchString)
                                           || s.FirstName.Contains(searchString)
                                           || s.Department.Name.Contains(searchString)
                                           || s.EmployeeType.Name.Contains(searchString)
                                           || s.StartDate.ToString().Contains(searchString)
                                           || s.EndDate.ToString().Contains(searchString));
                }
                switch (sortOrder) //sorts result by sort parameter
                {
                    case "firstName_desc":
                        employees = employees.OrderByDescending(s => s.FirstName);
                        break;
                    case "lastname":
                        employees = employees.OrderBy(s => s.LastName);
                        break;
                    case "lastname_desc":
                        employees = employees.OrderByDescending(s => s.LastName);
                        break;
                    case "dept_desc":
                        employees = employees.OrderByDescending(s => s.Department.Name);
                        break;
                    case "Department":
                        employees = employees.OrderBy(s => s.Department.Name);
                        break;
                    case "type_desc":
                        employees = employees.OrderByDescending(s => s.EmployeeType.Name);
                        break;
                    case "Type":
                        employees = employees.OrderBy(s => s.EmployeeType.Name);
                        break;
                    case "start_desc":
                        employees = employees.OrderByDescending(s => s.StartDate);
                        break;
                    case "Start":
                        employees = employees.OrderBy(s => s.StartDate);
                        break;
                    case "end_desc":
                        employees = employees.OrderByDescending(s => s.EndDate);
                        break;
                    case "End":
                        employees = employees.OrderBy(s => s.EndDate);
                        break;
                    default:
                        employees = employees.OrderBy(s => s.FirstName);
                        break;
                }
                }catch(Exception ex)
            {
                log.Info("listing all employees exception", ex);
            }
            

            int pageSize = 75; //number of items per page
            int pageNumber = (page ?? 1);
            return View(employees.ToPagedList(pageNumber, pageSize));
        }

        //builds employee data view
        public ActionResult EmployeeData(int ID = 0)
        {
            year = YEAR;
            EmployeeDataController controller = new EmployeeDataController();
            var result = controller.EmployeeDataTables(ID);
            return View(result);
        }
        public DataLine DepartmentCost(int departmentID)
        {
            DataLine line = new DataLine();
            DepartmentServices controller = new DepartmentServices(YEAR);
            decimal[] values = new decimal[12];

            try
            {
                var department = db.Departments.Find(departmentID);
                if (department != null)
                {
                    var data = controller.CreateDepartmentSummary(department);
                    foreach (var item in data)
                    {
                        for (var i = 0; i < item.Values.Length; i++)
                        {
                            values[i] += item.Values[i];
                        }
                    }
                }


                line.Name = department.Name;
                line.Values = values;
    
            }catch(Exception ex)
            {
                log.Warn("failed to retreieve department costs", ex);
            }


            return line;
        }
        //builds department summary view
        public ActionResult DepartmentSummary(int departmentID = 0)
        {
            year = YEAR;
            DepartmentSummaryController controller = new DepartmentSummaryController();
            var result = controller.DepartmentSummaryTables(departmentID);

            return View(result);
        }

        //builds intern view
        public ActionResult Intern(int departmentID = 0)
        {
            year = YEAR;
            InternController controller = new InternController();
            var result = controller.InternTables(departmentID);

            return View(result);
        }

        //builds employee cost table
        public ActionResult EmployeeCost()
        {
            year = YEAR;
            DepartmentSummaryController controller = new DepartmentSummaryController();
            var result = controller.EmployeeCostTable();

            return View(result);
        }

        //builds emplyee comparison table 
        public ActionResult EmployeeComparison()
        {
            year = YEAR;
            DepartmentSummaryController controller = new DepartmentSummaryController();
            var result = controller.EmployeeComparisonList();

            return View(result);
        }

        //builds target list view
        public ActionResult Targets()
        {
            year = YEAR;
            EmployeeDataController controller = new EmployeeDataController();
            var result = controller.EmployeeTargetTables();

            return View(result);
        }

        // GET: Employees/Details/5
        public ActionResult Details(int departmentID = 0)
        {
            year = YEAR;
            EmployeeView view = new EmployeeView();
            DepartmentSummaryController controller = new DepartmentSummaryController();
            view.EmployeeList = controller.employeeListTable(year, departmentID);
            InternController intern = new InternController();
            if (departmentID == 0)
            {
                view.Interns = intern.InternTable(departmentID);
            }
            return View(view);
        }



        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name");
            ViewBag.TypeID = new SelectList(db.EmployeeTypes, "EmployeeTypeID", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,DepartmentID,TypeID, FirstName,LastName,StartDate,EndDate")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("EmployeeData", new { ID = employee.EmployeeID });
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", employee.DepartmentID);
            ViewBag.TypeID = new SelectList(db.EmployeeTypes, "EmployeeTypeID", "Name", employee.TypeID);
            return View("Index");
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", employee.DepartmentID);
            ViewBag.TypeID = new SelectList(db.EmployeeTypes, "EmployeeTypeID", "Name", employee.TypeID);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,DepartmentID,TypeID,FirstName,LastName,StartDate,EndDate")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();


            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentID", "Name", employee.DepartmentID);
            ViewBag.TypeID = new SelectList(db.EmployeeTypes, "EmployeeTypeID", "Name", employee.TypeID);
            return View(employee);
        }


        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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
        //saves employee data
        public JsonResult SaveData([Bind(Include = "EmployeeID,DepartmentID,TypeID,BenefitPlanID,FirstName,LastName,StartDate,EndDate")] Employee employee, string id)
        {
            //int index = 0;
            String result = String.Empty;

            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                result = "1";
            }


            return Json(result, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        //adds employee to database
        public JsonResult AddEmp(string FirstName, string LastName, DateTime StartDate, DateTime EndDate, int DepartmentID, int TypeID, string URL)
        {
            Employee empToAdd = new Employee();
            empToAdd.FirstName = FirstName;
            empToAdd.LastName = LastName;
            empToAdd.StartDate = StartDate;
            empToAdd.EndDate = EndDate;
            empToAdd.DepartmentID = DepartmentID;
            empToAdd.TypeID = TypeID;

            int start=1;
            int end=12;
            if (empToAdd.StartDate != null)
            {
                start = empToAdd.StartDate.Value.Month;
            }

            if (empToAdd.EndDate != null)
            {
                end = empToAdd.EndDate.Value.Month;
            }

            db.Employees.Add(empToAdd);
            db.SaveChanges();
            
            int empID = empToAdd.EmployeeID;

            string newURL = URLNewEmp(empID, URL);

            ChangeLog.addChangeLog(newURL, "N/A", "N/A", "Added New Resident");

            generateTargets(empID, start, end);
            generateBursaries(empID, start, end);
            return Json(empID, JsonRequestBehavior.AllowGet);
        }

        private string URLNewEmp(int empID, string URL)
        {
            string newURL = "";
            newURL += URL + "/singleResident?employeeID=" + empID + "&year=" + YEAR;
            return newURL;
        }

        private void generateTargets(int empID, int start, int end )
        {

            year = YEAR;
            string res = "Monthly Resident Target";

            DateTime date = new DateTime();

            var hour = getMonthlyTarget(res, year);

            for (int i = 0; i < 12; i++)
            {

                date = getDate(i+1, year);
                addResidentTarget(hour, empID, date);

            }
            db.SaveChanges();

        }

        private decimal getMonthlyTarget(string res, int year)
        {
            var hour = from r in db.UnchangingValues
                       where r.Name == res && r.Year == year
                       select r;

            var monthlyHour = hour.FirstOrDefault().Value;

            return monthlyHour;
        }

        private DateTime getDate(int month, int year)
        {
            DateTime date = new DateTime(year, month, 1);

            return date;
        }

        private void addResidentTarget(decimal hour, int empID, DateTime date)
        {
            ResidentTarget resTar = new ResidentTarget();
            resTar.Hour = hour;
            resTar.EmployeeID = empID;
            resTar.Date = date;
            db.ResidentTargets.Add(resTar);
        }


        private void generateBursaries(int empID, int start, int end)
        {
            year = YEAR;
            string bur = "Bursary";

            DateTime date = new DateTime();

            var burVal = getMonthlyBur(year, bur);

            for (int i = 0; i < 12; i++)
            {

                date = getDate(i+1, year);
                addBursary(burVal, empID, date);

            }
            db.SaveChanges();
        }

        private decimal getMonthlyBur(int year, string bur)
        {
            var burVal = from r in db.UnchangingValues
                         where r.Name == bur && r.Year == year
                         select r;

            var bursary = burVal.FirstOrDefault().Value;

            return bursary;
        }

        private void addBursary(decimal burVal, int empID, DateTime date)
        {
            var clawback = Decimal.Multiply(burVal, (decimal)0.4);

            Bursary bur = new Bursary();
            bur.BursaryValue = burVal;
            bur.EmployeeID = empID;
            bur.Date = date;
            bur.Clawback = clawback;
            db.Bursaries.Add(bur);
        }

        [HttpPost]
        public void EditDate(int EmployeeID, DateTime? StartDate, DateTime? EndDate, String URL)
        {
            
            int empID = EmployeeID;

            var emp = getEmployee(empID);

            if (StartDate != null && EndDate != null)
            {
                if(emp.StartDate != StartDate)
                {
                    var prev = emp.StartDate;
                    emp.StartDate = StartDate;
                    ChangeLog.addChangeLog(URL, (DateTime)StartDate, (DateTime)prev, "Start Date");
                }
                if(emp.EndDate != EndDate)
                {
                    var prev = emp.EndDate;
                    emp.EndDate = EndDate;
                    ChangeLog.addChangeLog(URL, (DateTime)EndDate, (DateTime)prev, "End Date");
                }
            }
            else if (StartDate != null && EndDate == null)
            {
                if (emp.StartDate != StartDate)
                {
                    var prev = emp.StartDate;
                    emp.StartDate = StartDate;
                    ChangeLog.addChangeLog(URL, (DateTime)StartDate, (DateTime)prev, "Start Date");
                }

            }
            else if (StartDate == null && EndDate != null)
            {
                if(emp.EndDate != EndDate)
                {
                    var prev = emp.EndDate;
                    emp.EndDate = EndDate;
                    ChangeLog.addChangeLog(URL, (DateTime)EndDate, (DateTime)prev, "End Date");
                }
               
            }
            else
            {
                //do nothing
            }

            db.SaveChanges();

        }

        private Employee getEmployee(int empID)
        {
            var emp = from r in db.Employees
                      where r.EmployeeID == empID
                      select r;
            var employee = emp.FirstOrDefault();

            return employee;
        }

       

        private IQueryable<Bursary> getResidentBursary(int empID)
        {
            var bur = from r in db.Bursaries
                      where r.EmployeeID == empID && r.Date.Year == YEAR
                      select r;
            return bur;
        }

        private IQueryable<ResidentTarget> getResidentTargets(int empID)
        {
            var tar = from r in db.ResidentTargets
                      where r.EmployeeID == empID && r.Date.Year == YEAR
                      select r;
            return tar;
        }

        private EmployeeTarget getEmployeeTarget(int EmpID)
        {
            var empTar = from r in db.EmployeeTargets
                         where r.EmployeeID == EmpID && r.Year == YEAR
                         select r;
            return empTar.FirstOrDefault();
        }


       
        //end
        new public ActionResult setInstanceYear(int newYear = 0)
        {
            if (newYear != 0)
            {
                YEAR = newYear;
                setViewBag(newYear);
            }
            return View("Index");
        }

        public DataLine EmployeeTypeCosts(int typeID)
        {
            DataLine line = new DataLine();
            var employees = db.Employees.Where(x => x.TypeID == typeID);
            foreach (var e in employees)
            {

            }

            return line;
        }

        private decimal[] totalSalary(Employee e)
        {
            decimal[] values = new decimal[12];

            if (e.TypeID == 4)
            {
                BursariesController b = new BursariesController();
                values = b.totalResidentCost();
            }
            else
            {

            }

            return values;
        }
    


}
}
