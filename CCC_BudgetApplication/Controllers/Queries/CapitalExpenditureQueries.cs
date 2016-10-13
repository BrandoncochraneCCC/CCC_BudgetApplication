using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Controllers.Queries
{
    public class CapitalExpenditureQueries
    {
        private BudgetDataEntities db = new ObjectInstanceController().db;
        private int year;
        public CapitalExpenditureQueries(int year)
        {
            this.year = year;
        }

        public CapitalExpenditure getCapitalExpenditure(int id)
        {
            return db.CapitalExpenditures.Where(x => x.CapitalExpenditureID == id).Select(x => x).SingleOrDefault();
        }

        public IQueryable<CapitalExpenditure> getChildren(int id)
        {
            if(id == 0)
            {
                return db.CapitalExpenditures.Where(x => x.ParentID == null).Select(x => x);

            }
            else
            {
                return db.CapitalExpenditures.Where(x => x.ParentID == id).Select(x => x);
            }
        }

        public IQueryable<CapitalExpenditureData> getChildData(int id)
        {
            return db.CapitalExpenditureDatas.Where(x => x.CapitalExpenditureID == id && x.Date.Year == year).Select(x => x);
        }

        public CapitalExpenditureData getMonthlyExpenditure(IQueryable<CapitalExpenditureData> data, int month)
        {
            return data.Where(x => x.Date.Month == month).Select(x => x).SingleOrDefault();
        }


    }
}