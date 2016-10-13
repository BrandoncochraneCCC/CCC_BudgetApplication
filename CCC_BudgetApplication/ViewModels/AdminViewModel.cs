using Application.Controllers;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.ViewModels
{
    public class AdminViewModel
    {
        public static readonly log4net.ILog log = LogHelper.GetLogger();
        public BudgetDataEntities db = new BudgetDataEntities();
        

        public IEnumerable<SelectListItem> Vacation { get; set; }
        public IEnumerable<SelectListItem> STD { get; set; }
        public IEnumerable<SelectListItem> IndividualBenefit { get; set; }
        public IEnumerable<SelectListItem> FamilyBenfit { get; set; }
        public IEnumerable<SelectListItem> CPP { get; set; }
        public IEnumerable<SelectListItem> EI { get; set; }
        public IEnumerable<SelectListItem> RRSP { get; set; }
        public IEnumerable<SelectListItem> HoursPerYear { get; set; }
        public IEnumerable<SelectListItem> Department { get; set; }
        public IEnumerable<SelectListItem> Type { get; set; }
        public IEnumerable<SelectListItem> Account { get; set; }
        public IEnumerable<SelectListItem> ActualValue { get; set; }
        public IEnumerable<SelectListItem> Bursary { get; set; }
        public IEnumerable<SelectListItem> Clawback { get; set; }
        public IEnumerable<SelectListItem> TotalBursary { get; set; }
        public IEnumerable<SelectListItem> CounsellingProgram { get; set; }
        public IEnumerable<SelectListItem> CounsellingGroup { get; set; }

        public IEnumerable<SelectListItem> CounsellingService { get; set; }
        //public IEnumerable<SelectListItem> Lock { get; set; }
        //public IEnumerable<SelectListItem> Rollover { get; set; }
        public IEnumerable<SelectListItem> UnchangingValue { get; set; }
        public IEnumerable<SelectListItem> AverageFeeCounsellor { get; set; }
        public IEnumerable<SelectListItem> AverageFeeFT { get; set; }
        public IEnumerable<SelectListItem> AverageFeePT { get; set; }
        public IEnumerable<SelectListItem> AverageFeeStudent { get; set; }
        public IEnumerable<SelectListItem> CostSupervision { get; set; }
        private ObjectInstanceController instance = new ObjectInstanceController();

        public AdminViewModel()
        {
            try
            {
                Vacation = instance.VACATION_LIST;
                STD = instance.STD_LIST;
                IndividualBenefit = instance.INDIVIDUALS_LIST;
                FamilyBenfit = instance.FAMILY_LIST;
                EI = deduction(2);
                CPP = deduction(1);
                RRSP = deduction(3, 1011);
                HoursPerYear = hoursPerYear();
                Department = instance.DEPARTMENTS_LIST;
                Type = instance.TYPES_LIST;
                Account = instance.accountList();
                //actualValue();
                Bursary = unchangingValue("Bursary");
                //clawback();
                CounsellingProgram = counsellingPrograms();
                CounsellingGroup = counsellingGroups();
                //counsellingService();
                //lock();
                //rollover();
                TotalBursary = unchangingValue("Total Bursary Value");
                //averageFeeCounsellor();
                //averageFeeFT();
                //averageFeePT();
                //averageFeeStudent();
                //costSupervision();
            }
            catch (Exception ex)
            {
                log.Error("error creating admin lists", ex);
            }
        }

        public IEnumerable<SelectListItem> hoursPerYear()
        {
            return from d in db.HoursPerYears
                   orderby d.Hours
                   select new SelectListItem
                   {
                       Text = d.Hours.ToString(),
                       Value = d.Year.ToString()
                   };
        }

        public IEnumerable<SelectListItem> counsellingPrograms()
        {
            return from d in db.Programs
                   orderby d.Name
                   select new SelectListItem
                   {
                       Text = d.Name,
                       Value = d.ProgramID.ToString()
                   };
        }

        public IEnumerable<SelectListItem> counsellingGroups()
        {
            return from d in db.CounsellingGroupTypes
                   orderby d.Name
                   select new SelectListItem
                   {
                       Text = d.Name,
                       Value = d.Id.ToString()
                   };
        }

        public IEnumerable<SelectListItem> unchangingValue(string match)
        {
            return from d in db.UnchangingValues
                   where compareString(d.Name, match)
                   orderby d.Value
                   select new SelectListItem
                   {
                       Text = d.Value.ToString(),
                       Value = d.UnchangingValueID.ToString()
                   };


        }

        public IEnumerable<SelectListItem> deduction(int Id, int Id2 = 0)
        {
            if(Id2 != 0)
            {
                return from d in db.DeductionLists
                       where d.DeductionTypeID == Id || d.DeductionTypeID == Id2
                       select new SelectListItem
                       {
                           Text = d.DeductionType.Name + " Rate: " + d.Rate.ToString().Remove(d.Rate.ToString().Length - 2),
                           Value = d.DeductionTypeID.ToString(),
                           Selected = true
                       };
            }
            else
            {
                return from d in db.DeductionLists
                       where d.DeductionTypeID == Id
                       select new SelectListItem
                       {
                           Text = "Max: " + d.Max.ToString().Remove(d.Max.ToString().Length - 2) + " | Rate: " + d.Rate.ToString().Remove(d.Rate.ToString().Length - 2),
                           Value = d.DeductionTypeID.ToString(),
                           Selected = true
                       };
            }


        }

        public bool compareString(string s1, string s2)
        {
            var value1 = s1.ToLower();
            value1 = s1.Replace(" ", String.Empty);

            var value2 = s2.ToLower();
            value2 = s2.Replace(" ", String.Empty);

            return s1.Equals(s2);
        }
        protected  void Dispose(bool disposing)
        {
        }


    }
}