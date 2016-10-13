/**
* Author - BRANDON COCHRANE
* Organization: Calgary Counselling Centre
* 
* controls requests about counselling programs
* */


using Application.Controllers.CounsellingSummaries;
using Application.Controllers.Queries;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class CounsellingProgramController : ObjectInstanceController
    {
        private int year;
        CounsellingGroupQueries queries;
        // displays data for a single program
        public ActionResult IndividualProgram(int ID)
        {
            year = YEAR;
            IndividualProgramSummary controller = new IndividualProgramSummary(year);
            List<ProgramViewModel> result = new List<ProgramViewModel>();
            result.Add(controller.IndividualProgramTable(ID));

            return View("Program", result);
        }

        //creates program view
        public ActionResult Program(int ID = 0)
        {
            year = YEAR;
            IndividualProgramSummary controller = new IndividualProgramSummary(year);
            List<ProgramViewModel> result = new List<ProgramViewModel>();
            queries = new CounsellingGroupQueries(year);
            var programs = queries.getAllPrograms();
            foreach(var p in programs)
            {
                result.Add(controller.IndividualProgramTable(p.ProgramID));
            }

            return View(result);
        }

        //displays summary by program
        public ActionResult SummaryByProgram(int ID = 0)
        {
            year = YEAR;
            GroupSummaryByProgram controller = new GroupSummaryByProgram(year);
            List<groupCounselling> result = controller.programSummaryDataList();

            return View(result);
        }

        //displays revenueHours summary
        public ActionResult RevenueHours(int ID = 0)
        {
            year = YEAR;
            CounsellingHours controller = new CounsellingHours(year);
            List<DataTable> result = new List<DataTable>();
            queries = new CounsellingGroupQueries(year);

            var groups = queries.getCounsellingGroup(ID);
            if(groups.FirstOrDefault() != null)
            {
                
                foreach (var g in groups)
                {
                    result.Add(controller.totalRevenueHours(g));
                }
                result.Add(controller.revenueHourTotalsTable(result));

            }                    
            

            return View(result);
        }

        //displays preparation hours view
        public ActionResult PreparationHours(int ID = 0)
        {
            year = YEAR;
            CounsellingHours controller = new CounsellingHours(year);
            List<DataTable> result = new List<DataTable>();
            queries = new CounsellingGroupQueries(year);

            var groups = queries.getCounsellingGroup(ID);
            foreach (var g in groups)
            {
                result.Add(controller.totalPreparationHours(g));
            }
            result.Add(controller.preparationHourTotalsTable(result));

            return View(result);
        }

        //displays totalgroupHours View
        public ActionResult TotalGroupHours(int ID = 0)
        {
            year = YEAR;
            CounsellingHours controller = new CounsellingHours(year);
            List<DataTable> result = new List<DataTable>();
            queries = new CounsellingGroupQueries(year);

            var groups = queries.getCounsellingGroup(ID);
            foreach (var g in groups)
            {
                result.Add(controller.groupHourTotalsTable(g));
            }
            result.Add(controller.groupTotalsTable(result));
            result.Add(controller.groupTotalsHourTable(result));

            return View(result);
        }

        //displays supervision hours view
        public ActionResult SupervisionHours(int ID = 0)
        {
            year = YEAR;
            CounsellingHours controller = new CounsellingHours(year);
            List<DataTable> result = new List<DataTable>();
            queries = new CounsellingGroupQueries(year);

            var groups = queries.getCounsellingGroup(ID);
            foreach (var g in groups)
            {
                result.Add(controller.groupHourTotalsTable(g));
            }
            result.Add(controller.groupTotalsTable(result));
            result.Add(controller.groupTotalsHourTable(result));

            return View(result);
        }


    }
}