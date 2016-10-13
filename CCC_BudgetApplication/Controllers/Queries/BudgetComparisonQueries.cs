using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Controllers.Queries
{
    public class BudgetComparisonQueries
    {

        private BudgetDataEntities db = new ObjectInstanceController().db;
        private int year;
        public BudgetComparisonQueries(int year)
        {
            this.year = year;
        }

        public IQueryable<RevenueComparison> getRevenueComparisons()
        {
            return db.RevenueComparisons.Where(x => x.Year == year).Select(x => x);
        }

        public IQueryable<GAExpenseComparison> getGAExpenseComparisons()
        {
            return db.GAExpenseComparisons.Where(x => x.Year == year).Select(x => x);
        }
        public IQueryable<ServiceExpenseComparison> getServiceExpenseComparisons()
        {
            return db.ServiceExpenseComparisons.Where(x => x.Year == year).Select(x => x);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                db.Dispose();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}