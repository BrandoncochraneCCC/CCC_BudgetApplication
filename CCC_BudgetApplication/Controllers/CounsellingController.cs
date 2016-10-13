
/**
* Author - BRANDON COCHRANE
* Organization: Calgary Counselling Centre
* 
* counselling services summaries
* */

using Application.Controllers.Counselling;
using Application.Controllers.CounsellingSummaries;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class CounsellingController : ObjectInstanceController
    {
        private int year;
        // GET: Counselling\
        public ActionResult Index()
        {
            return View();
        }

        //displays cost of service counseling hours summary
        public ActionResult CostofServiceCounsellingHour(int typeID = 0)
        {
            year = YEAR;
            List<CostOfServiceViewModel> result = new List<CostOfServiceViewModel>();
            try
            {
                CostOfServiceCounsellingHoursController controller = new CostOfServiceCounsellingHoursController();
                CostOfServiceViewModel ft = null;
                CostOfServiceViewModel res = null;
                CostOfServiceViewModel intern = null;
                if (typeID == 0)
                {

                    ft = controller.CostOfServiceCounsellingHours(ObjectInstanceController.FULLTIME_EMPLOYEETYPEID);
                    res = controller.CostOfServiceCounsellingHours(ObjectInstanceController.RESIDENT_EMPLOYEETYPEID);
                    intern = controller.CostOfServiceCounsellingHours(ObjectInstanceController.INTERN_EMPLOYEETYPEID);


                }
                else
                {
                    ft = controller.CostOfServiceCounsellingHours(typeID);
                }
                if (ft != null) { result.Add(ft); }
                if (res != null) { result.Add(res); }
                if (intern != null) { result.Add(intern); }


                if (result != null)
                {
                    CostOfServiceViewModel total = controller.CostOfServicetotalHour(result);
                    if (total != null)
                    {
                        foreach (var item in result)
                        {
                            var temp = controller.CostOfServicePercent(item, total.data);
                            if (temp != null)
                            {
                                item.percentTotal = temp;

                            }
                        }

                    }
                    if (ft != null && res != null)
                    {
                        var x = (res.total.TotalHoursBilled + ft.total.TotalHoursBilled);
                        if (x != 0)
                        {
                            //total.weighted = (res.total.CostPerHour * res.percentTotal.CostPerHour) + (ft.total.CostPerHour * ft.percentTotal.CostPerHour) / ;
                        }
                    }
                    result.Add(total);
                }
            }
            catch(Exception ex)
            {
                log.Fatal("cost of service counselling hour report failed to build", ex);
            }
            

            return View(result);
        }


        //displays counselling analysis summary
        public ActionResult CounsellingAnalysis()
        {
            year = YEAR;
            CounsellingAnalysisController controller = new CounsellingAnalysisController();
            var result = controller.CounsellingAnalysis();
            return View(result);
        }

        //displays summary counselling view
        public ActionResult SummaryCounselling()
        {
            year = YEAR;
            SummaryCounsellingController controller = new SummaryCounsellingController();
            var result = controller.SummaryCounselling();
            return View(result);
        }

        //displays counselling hours view
        public ActionResult EmployeeCounsellingHours()
        {
            year = YEAR;
            EmployeeCounsellingHoursController controller = new EmployeeCounsellingHoursController();
            var result = controller.EmployeeCounsellingHours();
            return View(result);
        }
        //displays supervision hours view
        public ActionResult SupervisionHours()
        {
            year = YEAR;
            SupervisionHoursController controller = new SupervisionHoursController(year);
            var result = controller.EmployeeSupervisionHours();
            return View(result);
        }

        public ActionResult GroupCounsellingHours()
        {
            year = YEAR;
            GroupCounsellingHoursController controller = new GroupCounsellingHoursController(year);
            var result = controller.GroupCounsellingHours();
            return View(result);
        }
    }
}