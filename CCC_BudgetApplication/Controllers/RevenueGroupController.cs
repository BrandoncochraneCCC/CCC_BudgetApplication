/**
* Author - BRANDON COCHRANE
* Organization: Calgary Counselling Centre
* 
* handles summaries of revenues with parents
* */


using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class RevenueGroupController : ObjectInstanceController
    {
        RevenueSummaryQueries queries;
        private int year;
        ArrayServices arrayServices = new ArrayServices();


        // GET: Revenue Group
        public ActionResult Index(int revenueID = 0)
        {
            year = YEAR;
            queries = new RevenueSummaryQueries(year);
            DataTable table = new DataTable();
            List<DataTable> list = new List<DataTable>();
            try
            {
                var children = queries.getRevenueChildren(revenueID);
                List<DataLine> dataList = new List<DataLine>();
                if (children.Count() != 0)
                {
                    dataList = childData(children);
                }
                else
                {
                    dataList = childData(queries.getQueryableRevenue(revenueID));
                }

                table.tableName = queries.getRevenueName(revenueID);
                table.Year = year;
                table.dataList = dataList;
                table.sourceID = revenueID;
                list.Add(table);
            }
            catch(Exception ex)
            {
                log.Error("Revenue group table failure", ex);
            }
            

            return View(list);
        }

        //gets the child data of each revenue
        private List<DataLine> childData(IQueryable<Revenue> revenue)
        {
            List<DataLine> result = new List<DataLine>();
            try
            {
                foreach (var c in revenue)
                {
                    if (hasChildren(c))
                    {
                        DataLine line = getValueChildren(c);
                        line.hasChildren = true;
                        result.Add(line);

                    }
                    else
                    {
                        result.Add(getTableData(c));
                    }
                }
            }
            catch(Exception ex)
            {
                log.Warn("revenue group child data not acquired", ex);
            }
            

            return result;
        }

        //adds list item to beginning of table 
        private List<DataTable> placeItemFrontList(List<DataTable> list, DataTable table)
        {
            List<DataTable> newList = new List<DataTable>();
            list.Add(table);
            foreach(var t in list)
            {
                newList.Add(t);
            }
            return newList;
        }

        //builds data line for revenue including values of all children
        private DataLine getValueChildren(Revenue r)
        {
            DataLine line = new DataLine();
            try
            {
                line.Name = r.Name;
                line.SourceID = r.RevenueID;
                if (r.ParentID != 0)
                {
                    line.ParentID = (Int32)r.ParentID;
                }
                decimal[] values = new decimal[12];
                line.Values = childrenData(r, values);
            }
            catch(Exception ex)
            {
                log.Info("children data not calculated", ex);
            }
           

            return line;
        }

        //gathers all data from children and builds it into an array
        private decimal[] childrenData(Revenue r, decimal[] values)
        {
            var children = queries.getRevenueChildrenNoSelfJoins(r.RevenueID);
            foreach(var c in children)
            {
                if (hasChildren(c))
                {
                    childrenData(c, values);
                }
                values = arrayServices.combineArrays(values, revenueData(c));
            }

            return values;
        }

        //checks if a revenue has children
        private bool hasChildren(Revenue r)
        {
            bool hasChildren = false;
            if(queries.getRevenueChildren(r.RevenueID).FirstOrDefault() != null && r.ParentID != r.RevenueID)
            {
                hasChildren = true;
            }

            return hasChildren;
        }

        //builds a datatable for the given revenue
        private DataTable getRevenueTable(Revenue r)
        {
            DataTable table = new DataTable();
            try
            {
                table.tableName = queries.getRevenueName(r.RevenueID);
                var children = queries.getRevenueChildrenNoSelfJoins(r.RevenueID);
                List<DataLine> list = new List<DataLine>();
                foreach (var c in children)
                {
                    list.Add(getTableData(c));
                }
                table.dataList = list;
                table.Year = year;
                table.sourceID = r.RevenueID;
            }
            catch(Exception ex)
            {
                log.Info("revenue table not built", ex);
            }


            return table;
        }

        //builds a single dataline from the given revenue
        private DataLine getTableData(Revenue r)
        {
            DataLine line = new DataLine();
            line.Name = r.Name;
            line.SourceID = r.RevenueID;
            if (r.ParentID != 0 && r.ParentID != null)
            {
                line.ParentID = (Int32)r.ParentID;
            }
            line.Values = revenueData(r);

            return line;
        }
  
        //builds dataline values for given revenue
        private decimal[] revenueData(Revenue r)
        {
            var data = queries.getRevenueData(r.RevenueID);
            decimal[] values = sortData(data);

            return values;
        }

        //sorts given data by month and places it into an array
        private decimal[] sortData(IQueryable<RevenueData> data)
        {
            decimal[] values = new decimal[12];

            for (var i = 0; i < 12; i++)
            {
                values[i] = queries.getMonthlyRevenueData(data, i + 1); 
            }

            return values;
        }


        public ActionResult Create(Data data)
        {
            ViewBag.RevenueID = new SelectList(db.Revenues, "RevenueID", "Name");
            return View(data);
        }

        // POST: Revenues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RevenueID, Value, Date")] RevenueData data)
        {
            if (ModelState.IsValid)
            {
                RedirectToAction("Create", "RevenueData", data);
            }

            return View(data);
        }




    }
}