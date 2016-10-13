using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class AdminController : ObjectInstanceController
    {
        // GET: Admin
        public ActionResult Index()
        {
            AdminViewModel model = new AdminViewModel();

            return View(model);
        }




        // POST: programs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult programs([Bind(Include = "ProgramID,Name,Acronym,CounsellingGroupTypeID")] Program program)
        {
            CompareObjectsController<string> compare = new CompareObjectsController<string>();
            bool exists = false;

            string resultMessage = "";

            //CHECK IF EXISTS
            try
            {
                resultMessage = "Program: " + program.Name + "\n";
                var currentData = db.Programs.Select(x => x);
                foreach (var item in currentData)
                {
                    if (compare.compareString(program.Name, item.Name))
                    {
                        exists = true;
                    }

                }


                if (exists)
                {
                    resultMessage = program.Name + " Already exists.";
                }

                else
                {

                    if (ModelState.IsValid)
                    {
                        db.Programs.Add(program);
                        ChangeLog.addChangeLog(URL(), program.Name, "N/A", "Create Program");

                        db.SaveChanges();
                        resultMessage += "Added Successfully!";
                    }
                    else
                    {
                        resultMessage = "Model is invalid";

                    }

                }
            }
            catch (Exception ex)
            {
                resultMessage = "Error occured, record not added";
                log.Fatal("creating program failed", ex);
            }

            var destination = "/Admin/Index";
            return RedirectToAction("ResultMessage", "Index", new { message = resultMessage, url = destination });

        }

        // POST: Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDepartment([Bind(Include = "DepartmentID,Name,Description")] Department department)
        {
            
            CompareObjectsController<string> compare = new CompareObjectsController<string>();
            bool exists = false;

            string resultMessage = "Employee Type: " + department.Name + "\n";
            //CHECK IF EXISTS
            try
            {

                var currentData = db.Departments.Select(x => x);
                foreach (var item in currentData)
                {
                    if (compare.compareString(department.Name, item.Name))
                    {
                        exists = true;
                    }
                }


                if (exists)
                {
                    resultMessage = department.Name + " Already exists.";
                }

                else
                {

                    if (ModelState.IsValid)
                    {
                        db.Departments.Add(department);
                        ChangeLog.addChangeLog(URL(), department.Name, "N/A", "Create Department");

                        db.SaveChanges();
                        resultMessage += "Added Successfully!";
                    }
                    else
                    {
                        resultMessage = "Model is invalid";

                    }

                }
            }
            catch (Exception ex)
            {
                resultMessage = "Error occured, record not added";
                log.Fatal("creating department failed", ex);
            }

            var destination = "/Admin/Index";
            return RedirectToAction("ResultMessage", "Index", new { message = resultMessage, url = destination });

        }


        // POST: CounsellingType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CounsellingType([Bind(Include = "Id,Name")] CounsellingGroupType counsellingGroupType)
        {
            CompareObjectsController<string> compare = new CompareObjectsController<string>();
            bool exists = false;
            string resultMessage = "Counselling Type: " + counsellingGroupType.Name + "\n";
            //CHECK IF EXISTS
            try
            {
                var currentData = db.EmployeeTypes.Select(x => x);
                foreach (var item in currentData)
                {
                    if (compare.compareString(counsellingGroupType.Name, item.Name))
                    {
                        exists = true;
                    }
                }


                if (exists)
                {
                    resultMessage = counsellingGroupType.Name + " Already exists.";
                }

                else
                {

                    if (ModelState.IsValid)
                    {
                        db.CounsellingGroupTypes.Add(counsellingGroupType);
                        ChangeLog.addChangeLog(URL(), counsellingGroupType.Name, "N/A", "Create Counselling Group Type");

                        db.SaveChanges();
                        resultMessage += "Added Successfully!";
                    }
                    else
                    {
                        resultMessage = "Model is invalid";

                    }

                }
            }
            catch (Exception ex)
            {
                resultMessage = "Error occured, record not added";
                log.Fatal("creating counselling type failed", ex);
            }

            var destination = "/Admin/Index";
            return RedirectToAction("ResultMessage", "Index", new { message = resultMessage, url = destination });

        }

        // POST: Type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult type([Bind(Include = "EmployeeTypeID,Name")] EmployeeType employeeType)
        {
            CompareObjectsController<string> compare = new CompareObjectsController<string>();
            bool exists = false;
            string resultMessage = "Employee Type: " + employeeType.Name + "\n";
            //CHECK IF EXISTS
            try
            {
                var currentData = db.EmployeeTypes.Select(x => x);
                foreach (var item in currentData)
                {
                    if (compare.compareString(employeeType.Name, item.Name))
                    {
                        exists = true;
                    }
                }


                if (exists)
                {
                    resultMessage = employeeType.Name + " Already exists.";
                }

                else
                {

                    if (ModelState.IsValid)
                    {
                        db.EmployeeTypes.Add(employeeType);
                        ChangeLog.addChangeLog(URL(), employeeType.Name, "N/A", "Create Employee Type");
                        db.SaveChanges();
                        resultMessage += "Added Successfully!";
                    }
                    else
                    {
                        resultMessage = "Model is invalid";

                    }

                }
            }
            catch (Exception ex)
            {
                resultMessage = "Error occured, record not added";
                log.Fatal("creating employee type failed", ex);
            }

            var destination = "/Admin/Index";
            return RedirectToAction("ResultMessage", "Index", new { message = resultMessage, url = destination });

        }

        // POST: programs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult deduction([Bind(Include = "DeductionListID,DeductionTypeID,Year,Max,Rate")] DeductionList DeductionList)
        //{
        //    //Program program = new Program();
        //    CompareObjectsController<Program> compare = new CompareObjectsController<Program>();
        //    bool exists = false;
        //    string resultMessage = "Program: " + program.Name + "\n";
        //    //CHECK IF EXISTS
        //    try
        //    {
        //        var currentData = db.Programs.Select(x => x);
        //        foreach (var item in currentData)
        //        {
        //            if (compare.compareString(program.Name, item.Name))
        //            {
        //                exists = true;
        //            }
        //            if (program.Acronym != String.Empty && program.Acronym != null && compare.compareString(program.Acronym, item.Acronym))
        //            {
        //                exists = true;
        //            }
        //        }


        //        if (exists)
        //        {
        //            resultMessage = program.Name + " Already exists.";
        //        }

        //        else
        //        {

        //            if (ModelState.IsValid)
        //            {
        //                db.Programs.Add(program);
        //                db.SaveChanges();
        //                resultMessage += "Added Successfully!";
        //            }
        //            else
        //            {
        //                resultMessage = "Model is invalid";

        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Fatal("creating program failed", ex);
        //    }

        //    var destination = "/Admin/Index";
        //    return RedirectToAction("ResultMessage", "Index", new { message = resultMessage, url = destination });

        //}
        private string URL()
        {
            return Url.Action("Index", "Admin", null, "http");
        }
    }
}

