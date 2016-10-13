using Application.Controllers.GeneralExpenses;
using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class UserSummaryController : ObjectInstanceController
    {


        // GET: UserSummary
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserSummary(int id = 0)
        {
            if(id == 0)
            {
                return Index();
            }
            else
            {
                var summary = db.UserBuiltSummaries.Find(id);
                List<DataTable> tables = new List<DataTable>();

                if(summary.UserBuiltSummary1 != null)
                {
                    foreach (var s in summary.UserBuiltSummary1)
                    {
                        tables.Add(BuildTable(s));
                    }
                }
                else
                {
                    tables.Add(BuildTable(summary));
                }

                UserSummaryViewModel model = new UserSummaryViewModel(summary, tables);
                return View(model);
            }
        }


        private DataTable BuildTable(UserBuiltSummary summary)
        {
            DataTable table = new DataTable();
            table.tableName = summary.Name;
            table.sourceID = summary.Id;
            table.dataList = BuildDataList(summary);


            return table;
        }

        private List<DataLine> BuildDataList(UserBuiltSummary summary)
        {
            PropagationController propagate = new PropagationController();
            List<DataLine> list = new List<DataLine>();
            if(summary.ParentID != null && summary.ParentID != 0)
            {
                //get parent data
            }
            var data = summary.UserBuiltSummaryDatas;
            foreach(var d in data)
            {
                list.Add(propagate.PropagateDataLine(d));
            }

            return list;
        }

        public DataLine userDataLine(int id)
        {
            PropagationController propagate = new PropagationController();
            DataLine line = new DataLine();
            var item = db.UserBuiltSummaryDatas.Find(id);
            var parent = db.UserBuiltSummaryDatas.Find(item.Id);
            var summary = db.UserBuiltSummaries.Find(item.SummaryID);
            var parentLine = propagate.PropagateDataLine(parent);
            decimal[] values = ARRAYSERVICES.multiplyArraybyValue(parentLine.Values, ((decimal)summary.Percentage / 100));
            return line;
        }

    }
}