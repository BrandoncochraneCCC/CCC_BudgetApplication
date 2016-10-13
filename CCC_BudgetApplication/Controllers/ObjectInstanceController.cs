/**
* Author - BRANDON COCHRANE
* Organization: Calgary Counselling Centre
* 
* Contains instance data for entire application
* */


using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{

    public class ObjectInstanceController : Controller
    {

        //logger instance
        public static readonly log4net.ILog log = LogHelper.GetLogger();

        public static ChangeLogsController ChangeLog = new ChangeLogsController();
        //Department ID
        public static int COUNSELLING_DEPT_ID = 1;
        public static int VOLUNTEER_INTERN_DEPTID = 10;
        public static int STUDENT_DEPARTMENT = 8;
        //deducion type IDs
        public static int CPP_DEDUCTIONTYPEID = 1;
        public static int EI_DEDUCTIONTYPEID = 2;
        public static int RRSP_DEDUCTIONTYPEID = 3;
        public static int OTHER_DEDUCTIONTYPEID = 4;
        public static int FAMILY_DEDUCTIONTYPEID = 11;
        public static int INDIVIDUAL_DEDUCTIONTYPEID = 12;
        public static int STD_DEDUCTIONTYPEID = 10;
        public static int NO_STD_DEDUCTIONTYPEID = 2014;
        public static int CEO_RRSP_DEDUCTIONTYPEID = 1011;
        public static int CEO_FAMILY_DEDUCTIONTYPEID = 1012;
        public static int VACATION_DEDUCTIONTYPEID = 9;
        //employee type IDs
        public static int STUDENT_EMPLOYEETYPEID = 5;
        public static int FULLTIME_EMPLOYEETYPEID = 1;
        public static int PARTTIME_EMPLOYEETYPEID = 2;
        public static int CONTRACT_EMPLOYEETYPEID = 3;
        public static int INTERN_EMPLOYEETYPEID = 6;
        public static int RESIDENT_EMPLOYEETYPEID = 4;

        //Actual data IDs
        public static int ACTUAL_FT_HOURSID = 1;
        public static int ACTUAL_RESIDENT_HOURSID = 2;
        public static int ACTUAL_STUDENT_HOURSID = 3;
        public static int ACTUAL_FT_REVENUEID = 4;
        public static int ACTUAL_RESIDENT_REVENUEID = 5;
        public static int ACTUAL_STUDENT_REVENUEID = 6;
        public static int ACTUAL_GROUP_REVENUEID = 7;
        //homefront funding
        public static int HOMEFRONTFUNDINGID = 2001;
        //expense ID
        public static int HARDWARE = 16;
        public static int SOFTWARE = 17;
        public static int ITCOMMUNICATIONS = 18;


        public enum UserSummaryTables { CapitalExpenditure, GAGroup, Revenue, ServiceExpense };
        public int YEAR { get; set; }
        public BudgetDataEntities db = new BudgetDataEntities();
        public ArrayServices ARRAYSERVICES = new ArrayServices();
        public List<int> LOCKEDYEARS { get; set; }
        public IEnumerable<SelectListItem> TYPES_LIST { get; set; }
        public IEnumerable<SelectListItem> DEPARTMENTS_LIST { get; set; }
        public IEnumerable<SelectListItem> YEAR_LIST { get; set; }
        public IEnumerable<SelectListItem> EI_LIST { get; set; }
        public IEnumerable<SelectListItem> INDIVIDUALS_LIST { get; set; }
        public IEnumerable<SelectListItem> CPP_LIST { get; set; }
        public IEnumerable<SelectListItem> RRSP_LIST { get; set; }
        public IEnumerable<SelectListItem> FAMILY_LIST { get; set; }
        public IEnumerable<SelectListItem> STD_LIST { get; set; }
        public IEnumerable<SelectListItem> VACATION_LIST { get; set; }
        public IEnumerable<SelectListItem> NON_REVENUE_HOURS { get; set; }



        public ObjectInstanceController()
        {
            if (InstanceYearController.YEAR == 0)
            {
                YEAR = DateTime.Now.Year;
            }
            else
            {
                YEAR = InstanceYearController.YEAR;

            }
            setLists();
            setViewBag(YEAR);

        }

        public RedirectToRouteResult ObjectInstance(string action, string controller, int year)
        {
            setLists();
            setViewBag(YEAR);
            return RedirectToAction(action, controller);

        }

        public RedirectToRouteResult ObjectInstance(string action, string controller, string query, int year)
        {

            setLists();
            setViewBag(YEAR);
            return RedirectToAction(action, controller, query);

        }
        public RedirectResult redirect(string url, int year = 222)
        {
            InstanceYearController.SETYEAR(year);
            YEAR = InstanceYearController.YEAR;


            return Redirect(url);
        }

        public RedirectToRouteResult setYear(string action, string controller, int year = 222, string query = "")
        {
            InstanceYearController.YEAR = year;
            YEAR = InstanceYearController.YEAR;
            if (query.Equals(""))
            {
                InstanceYearController.SETYEAR(year);
                return ObjectInstance(action, controller, year);
            }
            else
            {
                InstanceYearController.SETYEAR(year);
                return ObjectInstance(action, controller, query, year);
            }

        }

        //public void setYear(int year = 2011)
        //{
        //    YEAR = year;
        //}

        public void setViewBag(int year)
        {
            try
            {
                ViewBag.Year = year;
                ViewBag.EmptyTableRow = "<tr style='height:30px;'></tr>";
                ViewBag.YEAR_LIST = YEAR_LIST;
            }
            catch (Exception ex)
            {
                log.Debug("viewbag not set");
            }


        }

        public void setInstanceYear(int newYear = 0)
        {
            try
            {
                YEAR = newYear;
                ViewBag.YEAR = newYear;
            }
            catch (Exception ex)
            {
                log.Warn("setting current year failed");
            }

        }

        private void setLists()
        {
            try
            {
                departmentList();
                typeList();
                cppList();
                EIList();
                rrspList();
                familyList();
                IndividualList();
                stdList();
                vacationList();
                nonRevenueHour();
                yearList();
            }
            catch (Exception ex)
            {
                log.Error("error creating lists", ex);
            }

        }

        public IEnumerable<SelectListItem> revenues(int? id = 0)
        {
            var r = from d in db.Revenues
                    orderby d.Name
                    select new SelectListItem
                    {
                        Text = d.Name,
                        Value = d.RevenueID.ToString()
                    };
            List<SelectListItem> revenues = new List<SelectListItem>();
            revenues.Add(new SelectListItem { Text = " " });
            foreach (var item in r)
            {
                if (item.Value == id.ToString())
                {
                    item.Selected = true;
                }
                revenues.Add(item);

            }
            return revenues;

        }

        public IEnumerable<SelectListItem> general(int? id = 0)
        {
            var r = from d in db.GAGroups
                    orderby d.Name
                    select new SelectListItem
                    {
                        Text = d.Name,
                        Value = d.GAGroupID.ToString()
                    };
            List<SelectListItem> generals = new List<SelectListItem>();
            generals.Add(new SelectListItem { Text = " " });
            foreach (var item in r)
            {
                if (item.Value == id.ToString())
                {
                    item.Selected = true;
                }
                generals.Add(item);

            }
            return generals;

        }

        public IEnumerable<SelectListItem> accountList(int? id = 0)
        {
            var r = from d in db.Accounts
                    orderby d.AccountNum
                    select new SelectListItem
                    {
                        Text = " (Acct# " + d.AccountNum + ") " + d.Name,
                        Value = d.AccountNum.ToString()
                    };
            List<SelectListItem> accounts = new List<SelectListItem>();
            accounts.Add(new SelectListItem { Text = " " });
            foreach (var item in r)
            {
                if (item.Value == id.ToString())
                {
                    item.Selected = true;
                }
                accounts.Add(item);

            }
            return accounts;

        }

        public IEnumerable<SelectListItem> pools(int? id = 0)
        {
            var r = from d in db.Pools
                    orderby d.Name
                    select new SelectListItem
                    {
                        Text = d.Name,
                        Value = d.PoolID.ToString()
                    };
            List<SelectListItem> pools = new List<SelectListItem>();
            pools.Add(new SelectListItem { Text = " " });
            foreach (var item in r)
            {
                if (item.Value == id.ToString())
                {
                    item.Selected = true;
                }
                pools.Add(item);

            }
            return pools;

        }

        public IEnumerable<SelectListItem> service(int? id = 0)
        {
            var r = from d in db.ServiceExpenses
                    orderby d.Name
                    select new SelectListItem
                    {
                        Text = d.Name,
                        Value = d.ServiceExpenseID.ToString()
                    };
            List<SelectListItem> services = new List<SelectListItem>();
            services.Add(new SelectListItem { Text = " " });
            foreach (var item in r)
            {
                if (item.Value == id.ToString())
                {
                    item.Selected = true;
                }
                services.Add(item);

            }
            return services;

        }

        public IEnumerable<SelectListItem> accounts(int? id = 0)
        {
            var r = from d in db.Accounts
                    orderby d.Name
                    select new SelectListItem
                    {
                        Text = d.Name,
                        Value = d.AccountNum.ToString()
                    };
            List<SelectListItem> accounts = new List<SelectListItem>();
            accounts.Add(new SelectListItem { Text = " " });
            foreach (var item in r)
            {
                if (item.Value == id.ToString())
                {
                    item.Selected = true;
                }
                accounts.Add(item);

            }
            return accounts;

        }
        public IEnumerable<SelectListItem> NonRevenueHours()
        {
            var nonRevenueHours =
                from d in db.NonRevenueHours
                select new SelectListItem
                {
                    Text = d.Name,
                    Value = d.NonRevenueHourID.ToString(),
                };

            return nonRevenueHours;
        }
        private void yearList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = 2014.ToString(), Value = 2014.ToString() });
            list.Add(new SelectListItem { Text = 2014.ToString(), Value = 2014.ToString() });
            list.Add(new SelectListItem { Text = 2014.ToString(), Value = 2014.ToString() });
            list.Add(new SelectListItem { Text = 2014.ToString(), Value = 2014.ToString() });


            YEAR_LIST = list.AsEnumerable();

        }

        private void departmentList()
        {
            DEPARTMENTS_LIST =
               from d in db.Departments
               select new SelectListItem
               {
                   Text = d.Name,
                   Value = d.DepartmentID.ToString()
               };
        }

        private void typeList()
        {
            TYPES_LIST =
               from d in db.EmployeeTypes
               select new SelectListItem
               {
                   Text = d.Name,
                   Value = d.EmployeeTypeID.ToString()
               };
        }

        private void cppList()
        {
            CPP_LIST =
               from d in db.DeductionLists
               where d.DeductionTypeID == 1 && d.Year == YEAR
               select new SelectListItem
               {
                   Text = d.DeductionType.Acronym,
                   Value = d.DeductionTypeID.ToString(),
                   Selected = true
               };
        }

        private void EIList()
        {
            EI_LIST =
               from d in db.DeductionLists
               where d.DeductionTypeID == 2 && d.Year == YEAR
               select new SelectListItem
               {
                   Text = d.DeductionType.Acronym,
                   Value = d.DeductionTypeID.ToString(),
                   Selected = true
               };
        }
        private void rrspList()
        {
            RRSP_LIST =
               from d in db.DeductionLists
               where d.DeductionTypeID == 3 || d.DeductionTypeID == 1011 && d.Year == YEAR
               select new SelectListItem
               {
                   Text = d.DeductionType.Acronym,
                   Value = d.DeductionTypeID.ToString()
               };
        }
        private void familyList()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            var result =
               from d in db.DeductionLists
               where d.DeductionTypeID == 11 || d.DeductionTypeID == 1012 && d.Year == YEAR
               select d;
            foreach (var item in result)
            {
                if (item.Year == YEAR)
                {
                    var s = String.Format("{0:C0}", item.Max);
                    SelectListItem x = new SelectListItem
                    {
                        Text = s,
                        Value = item.DeductionTypeID.ToString()

                    };

                    list.Add(x);
                }

            }
            FAMILY_LIST = list;
        }
        private void IndividualList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var result =
               from d in db.DeductionLists
               where d.DeductionTypeID == 12 && d.Year == YEAR
               select d;
            foreach (var item in result)
            {
                var s = String.Format("{0:C0}", item.Max);
                SelectListItem x = new SelectListItem
                {
                    Text = s,
                    Value = item.DeductionTypeID.ToString()

                };

                list.Add(x);
            }
            INDIVIDUALS_LIST = list;

        }
        private void stdList()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            var result =
               from d in db.DeductionLists
               where d.DeductionTypeID == 10 && d.Year == YEAR
               select d;
            foreach (var item in result)
            {
                var s = String.Format("{0:C0}", item.Max);
                SelectListItem x = new SelectListItem
                {
                    Text = s,
                    Value = item.DeductionTypeID.ToString()

                };

                list.Add(x);
            }
            STD_LIST = list;
        }
        private void vacationList()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            var result =
               from d in db.DeductionLists
               where d.DeductionTypeID == 9 && d.Year == YEAR
               select d;
            foreach (var item in result)
            {
                var s = String.Format("{0:p0}", item.Rate / 100);
                SelectListItem x = new SelectListItem
                {
                    Text = s,
                    Value = item.DeductionTypeID.ToString()

                };

                list.Add(x);
            }
            VACATION_LIST = list;
        }

        private void nonRevenueHour()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            NON_REVENUE_HOURS = from d in db.NonRevenueHours
                                select new SelectListItem
                                {
                                    Text = d.Name,
                                    Value = d.NonRevenueHourID.ToString()

                                };
        }


    }
}