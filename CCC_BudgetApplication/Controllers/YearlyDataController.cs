using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class YearlyDataController : ObjectInstanceController
    {
        CompareObjectsController<string> cmp = new CompareObjectsController<string>();
        // GET: YearlyData
        public ActionResult Index()
        {
            CurrentYearData model = new CurrentYearData();
           
            model.HoursPerYear = 0;
            model.EmployeeTargetHours = 0; //add this to unchanging value

            model.AverageCostFT = 0;
            model.AveragteCostPT = 0;
            model.AverageCostResident = 0;
            model.AverageCostStudent = 0;

            model.AverageCostCounselling = 0;

            model.Clawback = 0;
        
            return View("YearData", null);
        }

        private DeductionList getDeductionListRecord(int deductionTypeID)
        {
            return db.DeductionLists.Where(x => x.DeductionTypeID == deductionTypeID && x.Year == YEAR).Select(x => x).SingleOrDefault();
        }

        private UnchangingValue getUnchangingValueRecord(string name)
        {
            return db.UnchangingValues.Where(x => cmp.compareString(x.Name, name) && x.Year == YEAR).Select(x => x).SingleOrDefault();
        }
        private UnchangingValue getUnchangingValueRecord(int id)
        {
            return db.UnchangingValues.Find(id);
        }

        private void SetDeductions(CurrentYearData model)
        {
            var cpp = getDeductionListRecord(CPP_DEDUCTIONTYPEID);
            var ei = getDeductionListRecord(EI_DEDUCTIONTYPEID);
            var rrsp = getDeductionListRecord(RRSP_DEDUCTIONTYPEID);

            if (cpp != null)
            {
                model.CppRate = (decimal)cpp.Rate;
                model.CppMax = (decimal)cpp.Max;
            }
            if (ei != null)
            {
                model.EIRate = (decimal)ei.Rate;
                model.EIMax = (decimal)ei.Max;
            }
            if (rrsp != null)
            {
                model.RRSPRate = (decimal)rrsp.Rate;
                model.RRSPMax = (decimal)rrsp.Max;
            }
        }

        private void SetBenefits(CurrentYearData model)
        {
            var family = getDeductionListRecord(FAMILY_DEDUCTIONTYPEID);
            var individual = getDeductionListRecord(INDIVIDUAL_DEDUCTIONTYPEID);
            var vacation = getDeductionListRecord(VACATION_DEDUCTIONTYPEID);

            if(family != null)
            {
                model.FamilyBenefit = (decimal)family.Max;
            }
            if (individual != null)
            {
                model.IndividualBenefit = (decimal)individual.Max;
            }
            if (vacation != null)
            {
                model.Vacation = (decimal)vacation.Max;
            }
        }

        private void SetUnchangingValues(CurrentYearData model)
        {
            var residentTarget = getUnchangingValueRecord("Monthly Resident Target");
            var totalBursary = getUnchangingValueRecord("Total Bursary Value");
            var bursary = getUnchangingValueRecord("Bursary");
            var gst = getUnchangingValueRecord("GST");
            var supervision = getUnchangingValueRecord("Average Supervision");
            var groupExpensePerHour = getUnchangingValueRecord("Group Expense Per Hour");

            if(residentTarget != null)
            {
                model.ResidentTargetHours = residentTarget.Value;
            }
            if(totalBursary != null)
            {
                model.TotalBursaryValue = totalBursary.Value;
            }
            if(bursary != null)
            {
                model.Bursary = bursary.Value;
            }
            if(gst != null)
            {
                model.GSTRate = gst.Value;
            }
            if(supervision != null)
            {
                model.AverageCostSupervision = supervision.Value;
            }
            if(groupExpensePerHour != null)
            {
                model.GroupExpense = groupExpensePerHour.Value;
            }

        }

        private bool UnchangingValueExists(string name)
        {
            if (getUnchangingValueRecord(name) == null)
                return false;
            else
                return true;
            
        }
        private bool DeductionListExists(int id)
        {
            if (getDeductionListRecord(id) == null)
                return false;
            else
                return true;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeductionListRate(int deductionTypeID, decimal oldValue, decimal newValue)
        {
            var message = "";
            try
            {
                if (DeductionListExists(deductionTypeID))
                {
                    message = "Error changing Rate";
                    var item = getDeductionListRecord(deductionTypeID);
                    item.Rate = newValue;
                }
                else
                {
                    message = "Error create Rate";
                    DeductionList item = new DeductionList();
                    item.DeductionTypeID = deductionTypeID;
                    item.Year = YEAR;
                    item.Rate = newValue;
                    item.Max = 0;

                    db.DeductionLists.Add(item);
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                log.Error(message, ex);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeductionListMax(int deductionTypeID, decimal oldValue, decimal newValue)
        {
            var message = "";
            try
            {
                if (DeductionListExists(deductionTypeID))
                {
                    message = "Error Changing Max";
                    var item = getDeductionListRecord(deductionTypeID);
                    item.Max = newValue;
                }
                else
                {
                    message = "Error Creating Max";
                    DeductionList item = new DeductionList();
                    item.DeductionTypeID = deductionTypeID;
                    item.Year = YEAR;
                    item.Rate = 0;
                    item.Max = newValue;

                    db.DeductionLists.Add(item);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error(message, ex);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeductionListMax(string name, decimal oldValue, decimal newValue)
        {
            var message = "";
            try
            {
                if (UnchangingValueExists(name))
                {
                    message = "Error Changing value";
                    var item = getUnchangingValueRecord(name);
                    item.Value = newValue;
                }
                else
                {
                    message = "Error Creating Max";
                    UnchangingValue item = new UnchangingValue();
                    item.Name = name;
                    item.Value = newValue;
                    item.Year = YEAR;
                }

            }
            catch (Exception ex)
            {
                log.Error(message, ex);
            }

            return View();
        }















    }
}