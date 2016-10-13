/**
* Author - BRANDON COCHRANE
* Organization: Calgary Counselling Centre
* 
* index views
* */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    [Authorize(Roles ="BudgetUsers")]
    public class IndexController : ObjectInstanceController
    {
        // GET: Index
        //Launches Index view of pages 
        public ActionResult Index()
        {
            ViewBag.YEAR = InstanceYearController.YEAR;

            return View();
        }

        //empty table for complete user input
        public ActionResult Empty()
        {
            ViewBag.YEAR = InstanceYearController.YEAR;
            return View();
        }


        public ActionResult DebugMessage(string successMessage, string message)
        {
            List<string> model = new List<string>();

            model.Add(successMessage);
            model.Add(message);
            ViewBag.Button = "<button class='btn btn-default' type='button'><a href='" + Request.UrlReferrer.ToString() + "'>Go Back</a></button>";
            return View("ResultMessage", model);
        }

        public ActionResult ResultMessage(string message, string url)
        {
            List<string> model = new List<string>();
            model.Add(message);
            setButton(url);
            return View("ResultMessage", model);
        }
        private void setButton(string url)
        {
            ViewBag.Button = "<button class='btn btn-default' type='button'><a href='" + url + "'>Go Back</a></button>";
        }

    }
}