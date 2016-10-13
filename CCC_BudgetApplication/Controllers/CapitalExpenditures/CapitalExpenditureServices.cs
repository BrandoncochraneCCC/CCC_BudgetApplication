using Application.Controllers.Queries;
using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers.CapitalExpenditures
{
    public class CapitalExpenditureServices : CapitalExpendituresController
    {

        private int year;
        private CapitalExpenditureQueries queries;
        public CapitalExpenditureServices(int year)
        {
            this.year = year;
            queries = new CapitalExpenditureQueries(year);
        }

        public List<DataTable> CapitalExpendituresTables(int id)
        {
            List<ViewModels.DataTable> tables = new List<ViewModels.DataTable>();
            tables.Add(CapitalExpendituresTable(id));
            return tables;
        }

        public DataTable CapitalExpendituresTable(int id)
        {
            DataTable table = new DataTable();
            try
            {
                if (id == 0)
                {
                    table.tableName = "Capital Expenditures";
                }
                else
                {
                    var expenditure = queries.getCapitalExpenditure(id);
                    table.tableName = expenditure.Name;
                }
                table.sourceID = id;
                table.Year = year;
                table.dataList = CapitalExpendituresDataList(id);
            }
            catch(Exception ex)
            {
                log.Warn("capital expenditure table failed", ex);
            }
            
            return table;
        }


        private List<DataLine> CapitalExpendituresDataList(int id)
        {
            List<DataLine> list = new List<DataLine>();
            IQueryable<CapitalExpenditure> expenditures = queries.getChildren(id);
                
            if(expenditures.Count() != 0)
            {
                foreach(var item in expenditures)
                {
                    list.Add(CapitalExpendituresDataLine(item));
                }
            }
            else
            {
                list.Add(CapitalExpendituresDataLine(queries.getCapitalExpenditure(id)));

            }


            return list;
        }
        public DataLine CapitalExpendituresDataLine(int id)
        {
            return CapitalExpendituresDataLine(queries.getCapitalExpenditure(id));
        }


        private DataLine CapitalExpendituresDataLine(CapitalExpenditure item)
        {
            DataLine line = new DataLine();

            var children = queries.getChildren(item.CapitalExpenditureID);
            line.Action = "CapitalExpenditure";
            line.Controller = "CapitalExpenditures";

            if(children.FirstOrDefault() != null)
            {
                line.hasChildren = true;
                line.Values = iterate(children);
            }
            else
            {
                line.Values = ARRAYSERVICES.combineArrays(line.Values, CapitalExpenditureData(item));
            }

            line.SourceID = item.CapitalExpenditureID;
            line.year = year;
            line.Name = item.Name;
            if(item.ParentID != null)
            {
                line.ParentID = (int)item.ParentID;
            }

            return line;
        }

        private decimal[] CapitalExpenditureData(CapitalExpenditure item)
        {
            decimal[] values = new decimal[12];
            var data = queries.getChildData(item.CapitalExpenditureID);
            for(var i = 0; i < 12; i++)
            {
                var result = queries.getMonthlyExpenditure(data, i + 1);
                if(result != null)
                {
                    values[i] += result.Value;
                }
            }
            return values;
        }

        public decimal[] iterate(IQueryable<CapitalExpenditure> groups)
        {
            decimal[] values = new decimal[12];
            foreach (var child in groups)
            {
                values = ARRAYSERVICES.combineArrays(values, ChildData(child));                 
            }
            return values;
        }

        public decimal[] ChildData(CapitalExpenditure child)
        {
            decimal[] values = new decimal[12];
            var gChildren = queries.getChildren(child.CapitalExpenditureID);

            foreach(var g in gChildren)
            {
                decimal[] temp = ChildData(g);
                temp = CapitalExpenditureData(g);
                values = ARRAYSERVICES.combineArrays(values, temp);
            }
            values = ARRAYSERVICES.combineArrays(values, CapitalExpenditureData(child));

            return values;
        }

    }
}