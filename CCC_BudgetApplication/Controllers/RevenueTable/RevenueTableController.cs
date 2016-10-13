using Application.ViewModels;
using System;
using System.Collections.Generic;
using Application.Models;
using Application.Controllers;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers.RevenueTable
{
    public class RevenueTableController : Controllers.RevenuesController
    {

        private int year;
        RevenueSummaryQueries queries;
        ArrayServices arrayServices = new ArrayServices();
        CounsellingProgram program;
        CounsellingServices services;
        public RevenueTableController()
        {
            year = YEAR;
            program = new CounsellingProgram(year);
            services = new CounsellingServices(year);
            queries = new RevenueSummaryQueries(year);
        }

        public ActionResult RevenueDataTable(int revenueID = 0)
        {
            return View("Index", "RevenueGroup");
        }

        // GET: Revenue
        public DataTable RevenueTable(int revenueID = 0)
        {
            DataTable table = new DataTable();
            table.sourceID = revenueID;
            table.tableName = "Revenues";
            if (revenueID != 0)
            {
                table.tableName = queries.getRevenue(revenueID).Name;
            }
            table.dataList = revenueDataList(revenueID);
            return table;
        }

        private List<DataLine> revenueDataList(int revenueID)
        {
            List<DataLine> list = new List<DataLine>();

            //gets all revenues that belong to top level summary
            var revenues = queries.getTopLevelRevenues();

            foreach (var item in revenues)
            {
               
                var line = RevenueDataLine(item);
                //get data from all children and get cell display value
                if (line != null)
                {
                    list.Add(line);
                }
            }

            return list;
        }
        public DataLine RevenueDataLine(int id)
        {
            if (id == 0)
            {
                return allRevenues();
            }
            else
            {
                return RevenueDataLine(db.Revenues.Find(id));
            }
        }
        private DataLine RevenueDataLine(Revenue item)
        {
            DataLine line = getChildren(item);

            var destination = "RevenueGroup";

            switch (line.Name.ToLower())
            {
                case "counselling":
                    destination = "Counselling";
                    line.Action = "Index";
                    line.Values = services.getRevenueSummaryCounselling();
                    break;
                case "groups":
                    destination = "Counselling";
                    line.Action = "Index";
                    line.Values = program.RevenueSummaryGroupDataValues(year);
                    break;
                case "supervision":
                    destination = "Bursaries";
                    getSupervision(line);
                    break;
            }
            line.SourceID = item.RevenueID;
            line.Controller = destination;
            line.FieldName = "RevenueID";

            return line;
        }
        

        public DataLine getChildren(int id)
        {
            DataLine line = getChildren(queries.getRevenue(id));
            switch (line.Name.ToLower())
            {
                case "counselling":
                    line.Values = services.getRevenueSummaryCounselling();
                    break;
                case "groups":
                    line.Values = program.RevenueSummaryGroupDataValues(year);
                    break;
                case "supervision":
                    getSupervision(line);
                    break;
            }
            return line;
        }

        public DataLine getChildren(Revenue revenue)
        {
            RevenueSummaryQueries queries = new RevenueSummaryQueries(YEAR);

            DataLine line = new DataLine();
            //returns list of children of the revenue passed in
            var children = queries.getRevenueChildren(revenue.RevenueID);
            line.Name = revenue.Name;
            line.SourceID = revenue.RevenueID;
            line.hasChildren = true;

            //creates array to populate with display values for each month
            line.Values = iterate(children);
            //iterates through children adding values

            return line;
        }

        public void getSupervision(DataLine line)
        {
            decimal[] values = new decimal[12];

            var bursaries = queries.getBursaries();

            for (var i = 0; i < 12; i++)
            {
                values[i] = queries.getMonthlyClawback(bursaries, i + 1);
            }

            line.Values = values;
        }
        private decimal[] iterate(IQueryable<Revenue> revenues)
        {
            DataLine line = new DataLine();
            decimal[] values = new decimal[12];
            foreach (var child in revenues)
            {
                //recursively enters children to retreive and add data
                var gChildren = queries.getRevenueChildrenNoSelfJoins(child.RevenueID);
                foreach (var grandChild in gChildren)
                {
                    var temp = queries.getRevenueChildrenNoSelfJoins(grandChild.RevenueID);
                    values = arrayServices.combineArrays(values, iterate(temp));
                    getRevenueData(grandChild, values);
                }
                getRevenueData(child, values);
            }
            return values;
        }

        private void getRevenueData(Revenue revenue, decimal[] values)
        {

            //gets data contained in children and adds values to array   

            var data = queries.getRevenueData(revenue.RevenueID);

            //sorts and adds values to display
            for (var i = 0; i < 12; i++)
            {
                var result = from r in data
                             where r.Date.Month == i + 1
                             select r.Value;
                values[i] += (Decimal)result.FirstOrDefault();

            }
        }

        private List<Revenue> getGrandChildren(Revenue revenue)
        {
            //returns the children of a revenue
            return queries.getRevenueChildrenNoSelfJoins(revenue.RevenueID).ToList();
        }



        private DataLine allRevenues()
        {
            DataLine line = new DataLine();
            line.Values = sumAllRevenues();
            line.Name = "Revenues " + YEAR;
            line.SourceID = 0;

            return line;

        }

        private decimal[] sumAllRevenues()
        {
            decimal[] values = new decimal[12];
            var data = db.RevenueDatas.Where(x => x.Date.Year == YEAR).Select(x => x);
            for (var i = 0; i < 12; i++)
            {
                values[i] = data.Where(x => x.Date.Month == i + 1).Select(x => x.Value).Sum();
            }

            return values;
        }

    }


}
