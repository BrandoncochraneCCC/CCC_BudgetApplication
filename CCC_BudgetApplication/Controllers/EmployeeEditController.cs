using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Models;
using Application.ViewModels;

namespace Application.Controllers
{
    public class EmployeeEditController : ObjectInstanceController
    {
        [HttpPost]
        public void AddEmployeeTarget(int EmployeeID, int Year, string URL)
        {

            var empTarget = getEmployeeTarget(EmployeeID);
            if (empTarget == null)
            {
                addEmpTar(EmployeeID, Year);
                ChangeLog.addChangeLog(URL, "Added Targets", "N/A", "Targets");
            }
            else
            {
                empTarget.Hour = 150;
            }

            db.SaveChanges();

        }

        private EmployeeTarget getEmployeeTarget(int EmpID)
        {
            var empTar = from r in db.EmployeeTargets
                         where r.EmployeeID == EmpID && r.Year == YEAR
                         select r;
            return empTar.FirstOrDefault();
        }

        private void addEmpTar(int EmployeeID, int Year)
        {
            EmployeeTarget tarToAdd = new EmployeeTarget();
            tarToAdd.Hour = 150;
            tarToAdd.Year = Year;
            tarToAdd.EmployeeID = EmployeeID;

            db.EmployeeTargets.Add(tarToAdd);
        }

        //done
        [HttpPost]
        public void EditEmpSal(decimal? CurrentBudget, decimal? HourlyRate, int EmployeeID, string URL)
        {

            var sal = getEmpSalary(EmployeeID);

            if (sal != null)
            {
                if (CurrentBudget != sal.CurrentBudget)
                {
                    var prevCurrBud = sal.CurrentBudget;
                    sal.CurrentBudget = CurrentBudget;
                    ChangeLog.addChangeLog(URL, (decimal)CurrentBudget, (decimal)prevCurrBud, "Budgeted Salary");
                }
                if (HourlyRate != sal.HourlyRate)
                {
                    var prevHourlyRate = sal.HourlyRate;
                    sal.HourlyRate = HourlyRate;
                    ChangeLog.addChangeLog(URL, (decimal)HourlyRate, (decimal)prevHourlyRate, "Hourly Rate");
                }

            }
            else if (CurrentBudget != 0 || HourlyRate != 0)
            {
                addSalary(CurrentBudget, HourlyRate, EmployeeID);
                if (CurrentBudget != 0)
                {
                    ChangeLog.addChangeLog(URL, (decimal)CurrentBudget, 0, "Budgeted Salary");
                }
                if (HourlyRate != 0)
                {
                    ChangeLog.addChangeLog(URL, (decimal)HourlyRate, 0, "Hourly Rate");
                }

            }
            db.SaveChanges();


        }

        private void addSalary(decimal? curr, decimal? hourly, int id)
        {
            Salary salToAdd = new Salary();

            salToAdd.CurrentBudget = curr;
            salToAdd.HourlyRate = hourly;
            salToAdd.EmployeeID = id;
            salToAdd.Year = YEAR;

            db.Salaries.Add(salToAdd);

            db.SaveChanges();
        }

        private Salary getEmpSalary(int EmployeeID)
        {
            var sal = from r in db.Salaries
                      where r.EmployeeID == EmployeeID && r.Year == YEAR
                      select r;

            return sal.FirstOrDefault();
        }

        private Employee getEmployee(int empID)
        {
            var emp = from r in db.Employees
                      where r.EmployeeID == empID
                      select r;
            var employee = emp.FirstOrDefault();

            return employee;
        }
        //done
        [HttpPost]
        public JsonResult EditEmpInfo(int DepartmentID, int TypeID, DateTime? StartDate, DateTime? EndDate, int EmployeeID, string URL)
        {
            var emp = getEmployee(EmployeeID);

            var residentID = RESIDENT_EMPLOYEETYPEID;
            var counsellorID = COUNSELLING_DEPT_ID;

            string error = "";

            if (TypeID == residentID && DepartmentID != counsellorID)
            {
                error += "Error, residents can only be in the counselling department.";
            }
            else
            {
                if (emp.DepartmentID != DepartmentID)
                {
                    var oldDepart = emp.Department.Name;
                    emp.DepartmentID = DepartmentID;
                    db.SaveChanges();
                    ChangeLog.addChangeLog(URL, emp.Department.Name, oldDepart, "Department");
                }
                if (emp.TypeID != TypeID)
                {
                    var oldType = "";
                    if (emp.TypeID != null)
                    {
                        oldType = emp.EmployeeType.Name;
                    }
                    emp.TypeID = TypeID;
                    db.SaveChanges();
                    ChangeLog.addChangeLog(URL, emp.EmployeeType.Name, oldType, "Employee Type");
                }


            }
            if (StartDate != null)
            {
                if (emp.StartDate != StartDate)
                {
                    var prevStart = emp.StartDate;
                    emp.StartDate = StartDate;
                    db.SaveChanges();
                    ChangeLog.addChangeLog(URL, emp.StartDate.ToString(), prevStart.ToString(), "Start Date");
                }

            }

            if (EndDate != null)
            {
                if (emp.EndDate != EndDate)
                {
                    var prevEnd = emp.EndDate;
                    emp.EndDate = EndDate;
                    db.SaveChanges();
                    ChangeLog.addChangeLog(URL, emp.EndDate.ToString(), prevEnd.ToString(), "End Date");
                }

            }

            db.SaveChanges();
            return Json(error, JsonRequestBehavior.AllowGet);
        }

        //done
        [HttpPost]
        public void EditEmpRRSP(int EmployeeID, int Selected, string URL)
        {
            var allEmpDeductions = getEmployeeDeduction(EmployeeID);
            var emp = getEmployee(EmployeeID);

            var ded = allEmpDeductions.Where(p => p.DeductionTypeID == RRSP_DEDUCTIONTYPEID || p.DeductionTypeID == CEO_RRSP_DEDUCTIONTYPEID).Select(p => p);

            if (!ded.Any())
            {
                if (Selected == RRSP_DEDUCTIONTYPEID)
                {

                }
                else if (Selected == CEO_RRSP_DEDUCTIONTYPEID)
                {
                    addDeduction(Selected, EmployeeID);
                    var name = getDeductionName(Selected);
                    ChangeLog.addChangeLog(URL, name, "none", "RRSP Rate");
                }
            }
            else
            {
                if (ded.FirstOrDefault().DeductionTypeID == RRSP_DEDUCTIONTYPEID)
                {
                    if (Selected == RRSP_DEDUCTIONTYPEID)
                    {

                    }
                    else if (Selected == CEO_RRSP_DEDUCTIONTYPEID)
                    {
                        ded.FirstOrDefault().DeductionTypeID = Selected;
                        var name = getDeductionName(Selected);
                        var prevName = getDeductionName(RRSP_DEDUCTIONTYPEID);
                        ChangeLog.addChangeLog(URL, name, prevName, "RRSP");
                    }
                }
                else if (ded.FirstOrDefault().DeductionTypeID == CEO_RRSP_DEDUCTIONTYPEID)
                {
                    if (Selected == RRSP_DEDUCTIONTYPEID)
                    {
                        ded.FirstOrDefault().DeductionTypeID = Selected;
                        var name = getDeductionName(Selected);
                        var prevName = getDeductionName(CEO_RRSP_DEDUCTIONTYPEID);
                        ChangeLog.addChangeLog(URL, name, prevName, "RRSP");
                    }
                    else if (Selected == CEO_RRSP_DEDUCTIONTYPEID)
                    {

                    }
                }
            }
            db.SaveChanges();
        }

        private string getDeductionName(int deductionID)
        {
            var name = from r in db.DeductionTypes
                       where r.DeductionTypeID == deductionID
                       select r;

            return name.FirstOrDefault().Name;
        }

        private IQueryable<EmployeeDeduction> getEmployeeDeduction(int EmployeeID)
        {
            var empDed = from r in db.EmployeeDeductions
                         where r.EmployeeID == EmployeeID && r.Year == YEAR
                         select r;

            return empDed;
        }

        private void addDeduction(int deductionTypeID, int empID)
        {
            EmployeeDeduction dedToAdd = new EmployeeDeduction();
            dedToAdd.DeductionTypeID = deductionTypeID;
            dedToAdd.Year = YEAR;
            dedToAdd.EmployeeID = empID;

            db.EmployeeDeductions.Add(dedToAdd);
            db.SaveChanges();
        }

        //done 
        [HttpPost]
        public void EditEmpBenefits(int EmployeeID, int? Benefit, int? Vacation, int? STD, string URL)
        {
            var ded = getEmployeeDeduction(EmployeeID);

            editVacation(ded, Vacation, EmployeeID, URL);
            editSTD(ded, STD, EmployeeID, URL);
            editBenefitPlan(ded, EmployeeID, Benefit, URL);

        }

        private void editVacation(IQueryable<EmployeeDeduction> ded, int? Vacation, int EmployeeID, string URL)
        {

            if (Vacation != null)
            {
                var name = getDeductionName((int)Vacation);
                var empVaca = ded.Where(p => p.DeductionTypeID == Vacation).Select(p => p).FirstOrDefault();

                if (empVaca == null)
                {
                    addDeduction((int)Vacation, EmployeeID);
                    ChangeLog.addChangeLog(URL, name, "none", "Vacation");
                }
            }
            else
            {
                var empVacaToRemove = ded.Where(p => p.DeductionTypeID == VACATION_DEDUCTIONTYPEID).Select(p => p).FirstOrDefault();
                var name = getDeductionName(VACATION_DEDUCTIONTYPEID);
                if (empVacaToRemove != null)
                {
                    db.EmployeeDeductions.Remove(empVacaToRemove);
                    ChangeLog.addChangeLog(URL, "none", name, "Vacation");
                }
            }
            db.SaveChanges();
        }

        private void editSTD(IQueryable<EmployeeDeduction> ded, int? STD, int EmployeeID, string URL)
        {
            var name = "";
            var empSTD = ded.Where(p => p.DeductionTypeID == NO_STD_DEDUCTIONTYPEID).Select(p => p).FirstOrDefault();
            if (STD != null)
            {
                name = getDeductionName((int)STD);
                var prevName = getDeductionName(NO_STD_DEDUCTIONTYPEID);
                if (empSTD != null)
                {
                    db.EmployeeDeductions.Remove(empSTD);
                    ChangeLog.addChangeLog(URL, name, prevName, "Disability");
                }
            }
            else
            {
                name = getDeductionName(NO_STD_DEDUCTIONTYPEID);
                var prevName = getDeductionName(STD_DEDUCTIONTYPEID);

                if (empSTD == null)
                {
                    addDeduction(NO_STD_DEDUCTIONTYPEID, EmployeeID);
                    ChangeLog.addChangeLog(URL, name, prevName, "Disabilty");
                }
            }
            db.SaveChanges();
        }

        private void editBenefitPlan(IQueryable<EmployeeDeduction> ded, int EmployeeID, int? Benefit, string URL)
        {
            var empBenefitPlan = ded.Where(p => p.DeductionTypeID == FAMILY_DEDUCTIONTYPEID || p.DeductionTypeID == INDIVIDUAL_DEDUCTIONTYPEID || p.DeductionTypeID == CEO_FAMILY_DEDUCTIONTYPEID).Select(p => p).FirstOrDefault();
            var name = "";
            //employee has no benefit plan 
            if (empBenefitPlan == null)
            {
                if (Benefit == null)
                {

                }
                else
                {
                    name = getDeductionName((int)Benefit);
                    addDeduction((int)Benefit, EmployeeID);
                    ChangeLog.addChangeLog(URL, name, "none", "Benefits");
                }
            }

            //employee has a benefit plan
            else
            {
                //employee has family plan
                if (empBenefitPlan.DeductionTypeID == FAMILY_DEDUCTIONTYPEID)
                {
                    var prevName = getDeductionName(FAMILY_DEDUCTIONTYPEID);

                    if (Benefit == null)
                    {
                        db.EmployeeDeductions.Remove(empBenefitPlan);
                        ChangeLog.addChangeLog(URL, "none", prevName, "Benefits");
                    }
                    else if (empBenefitPlan.DeductionTypeID != Benefit)
                    {

                        empBenefitPlan.DeductionTypeID = (int)Benefit;
                        name = getDeductionName((int)Benefit);
                        ChangeLog.addChangeLog(URL, name, prevName, "Benefits");
                    }
                }
                //employee has individual plan
                else if (empBenefitPlan.DeductionTypeID == INDIVIDUAL_DEDUCTIONTYPEID)
                {
                    var prevName = getDeductionName(INDIVIDUAL_DEDUCTIONTYPEID);

                    if (Benefit == null)
                    {
                        db.EmployeeDeductions.Remove(empBenefitPlan);
                        ChangeLog.addChangeLog(URL, "none", prevName, "Benefits");
                    }
                    else if (empBenefitPlan.DeductionTypeID != Benefit)
                    {
                        empBenefitPlan.DeductionTypeID = (int)Benefit;
                        name = getDeductionName((int)Benefit);
                        ChangeLog.addChangeLog(URL, name, prevName, "Benefits");
                    }

                }
                else if (empBenefitPlan.DeductionTypeID == CEO_FAMILY_DEDUCTIONTYPEID)
                {
                    var prevName = getDeductionName(CEO_FAMILY_DEDUCTIONTYPEID);

                    if (Benefit == null)
                    {
                        db.EmployeeDeductions.Remove(empBenefitPlan);
                        ChangeLog.addChangeLog(URL, "none", prevName, "Benefits");
                    }
                    else if (empBenefitPlan.DeductionTypeID != Benefit)
                    {
                        empBenefitPlan.DeductionTypeID = (int)Benefit;
                        name = getDeductionName((int)Benefit);
                        ChangeLog.addChangeLog(URL, name, prevName, "Benefits");
                    }
                }
            }
            db.SaveChanges();
        }

        //done 
        [HttpPost]
        public void EditEmpRaise(int EmployeeID, DateTime? Date, decimal? Value, bool isPercent, decimal oldSal, string URL)
        {
            
                
            
            var lastRaise = getPrevEmpRaise(EmployeeID);
            EmployeeRaise raiseToAdd = new EmployeeRaise();
            if (Date != null && Value != null)
            {
                var newSal = calcNewSalary(oldSal, Value, isPercent);
                raiseToAdd.Date = (DateTime)Date;
                raiseToAdd.EmployeeID = EmployeeID;
                raiseToAdd.Value = (decimal)Value;
                raiseToAdd.isPercent = isPercent;
                raiseToAdd.OldSalary = oldSal;
                raiseToAdd.NewSalary = newSal;

                db.EmployeeRaises.Add(raiseToAdd);
                db.SaveChanges();

                addRaiseChange(EmployeeID, Value, isPercent, URL, lastRaise);

            }
        }

        private void addRaiseChange(int empID, decimal? val, bool isPercent, string URL, EmployeeRaise lastRaise)
        {
            var prevVal = "N/A";

            if (lastRaise != null)
            {
                prevVal = "" + lastRaise.Value;
                if (lastRaise.isPercent)
                {
                    prevVal += "%";
                }
            }

            var newVal = "" + val;
            if (isPercent)
            {
                newVal += "%";
            }
            ChangeLog.addChangeLog(URL, newVal, prevVal, "Raise");
        }


        private EmployeeRaise getPrevEmpRaise(int empID)
        {
            var raise = from r in db.EmployeeRaises
                        where r.EmployeeID == empID
                        select r;

            return raise.OrderByDescending(p => p.EmployeeRaiseID).FirstOrDefault();
        }

        private decimal calcNewSalary(decimal oldSal, decimal? Value, bool isPercent)
        {
            decimal newSal;
            if (!isPercent)
            {
                newSal = oldSal + (decimal)Value;
            }
            else 
            {
                var multiplier = (decimal)Value / 100;
                multiplier = multiplier + 1;
                newSal = oldSal * multiplier;
            }

            return newSal;
        }

        [HttpPost]
        public void EditEmpTargetHours(int EmployeeID, int? TargetHours, string URL)
        {

            EmployeeTarget tarToChange = getEmployeeTarget(EmployeeID);
            var prev = tarToChange.Hour;
            if (tarToChange.Hour != TargetHours)
            {
                if (TargetHours == null)
                {

                    tarToChange.Hour = 0;
                    ChangeLog.addChangeLog(URL, 0, prev, "Total Target Hours");
                }
                else
                {
                    if (tarToChange.Hour != TargetHours)
                    {
                        tarToChange.Hour = (int)TargetHours;
                        ChangeLog.addChangeLog(URL, (decimal)TargetHours, prev, "Total Target Hours");
                    }
                }
            }
            db.SaveChanges();
        }

        //Non rev hour edit functions. Need to be re-written

        [HttpPost]
        public int GetEmployeeTargetID(EmployeeTarget e)
        {
            e.Year = YEAR;
            var empTar = getEmployeeTarget(e.EmployeeID);

            return empTar.EmployeeTargetID;

        }

        [HttpPost]
        public void EditTargetData(TargetData[] tarDatas)
        {
            int? value;

            for (int i = 0; i < 12; i++)
            {
                tarDatas[i].Date = getDate(i + 1, YEAR);
            }

            for (int i = 0; i < 12; i++)
            {
                value = tarDatas[i].RevenueHours;
                var targetData = getTargetData(tarDatas[i]);

                if (targetData == null && value > 0)
                {
                    addTargetData(tarDatas[i]);

                }
                else if (targetData == null && value == 0)
                {
                    //do nothing
                }
                else if (targetData != null)
                {
                    if (targetData.RevenueHours != value)
                    {
                        targetData.RevenueHours = value;
                    }
                }
            }
            db.SaveChanges();

        }


        private TargetData getTargetData(TargetData td)
        {
            var tarData = from r in db.TargetDatas
                          where r.EmployeeTargetID == td.EmployeeTargetID && r.Date == td.Date
                          select r;
            return tarData.FirstOrDefault();
        }

        private void addTargetData(TargetData td)
        {
            db.TargetDatas.Add(td);
            db.SaveChanges();
        }

        [HttpPost]
        public JsonResult AllTargetDataID(TargetData[] td)
        {
            int[] targetIDs = new int[12];

            for (int i = 0; i < 12; i++)
            {
                td[i].Date = getDate(i + 1, YEAR);
            }

            for (int i = 0; i < 12; i++)
            {
                targetIDs[i] = getTargetData(td[i]).TargetDataID;
            }

            return Json(targetIDs, JsonRequestBehavior.AllowGet);

        }

        public void EditNonRevHours(NonRevenueHourData[] nonRev)
        {
            NonRevenueHourData nonRevToChange;
            int value;

            for (int i = 0; i < 12; i++)
            {
                value = nonRev[i].Value;
                nonRevToChange = getNonRevHourData(nonRev[i]);

                if (nonRevToChange == null && value > 0)
                {
                    addNonRevHour(nonRev[i]);

                }
                else if (nonRevToChange == null && value == 0)
                {
                    //do nothing
                }
                else if (nonRevToChange != null)
                {
                    nonRevToChange.Value = value;
                }
            }
            db.SaveChanges();

        }

        private NonRevenueHourData getNonRevHourData(NonRevenueHourData rev)
        {
            var nonRev = from r in db.NonRevenueHourDatas
                         where r.TargetDataID == rev.TargetDataID && r.NonRevenueHourID == rev.NonRevenueHourID
                         select r;

            return nonRev.FirstOrDefault();
        }

        private void addNonRevHour(NonRevenueHourData rev)
        {
            
            db.NonRevenueHourDatas.Add(rev);
            db.SaveChanges();
        }

        private DateTime getDate(int month, int year)
        {
            DateTime date = new DateTime(year, month, 1);

            return date;
        }
    }  
}