using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Controllers.Queries
{
    public class ServiceExpenseQueries
    {
        BudgetDataEntities db = new ObjectInstanceController().db;
        private int year;
        public ServiceExpenseQueries(int year)
        {
            this.year = year;
        }

        public IQueryable<ServiceExpense> serviceSummaryExpenses()
        {
            return db.ServiceExpenses.Where(s => s.ParentID == null);
        }

        public IQueryable<ServiceExpense> getChildren(int ID)
        {
            return db.ServiceExpenses.Where(s => s.ParentID == ID).Select(s => s);
        }

        public IQueryable<ServiceExpenseData> getChildData(int ID)
        {
            return db.ServiceExpenseDatas.Where(s => s.ServiceExpenseID == ID && s.Date.Year == year).Select(s =>s);
        }

        public ServiceExpenseData getMonthlyExpenseData (IQueryable<ServiceExpenseData> data, int month)
        {
            return data.Where(d => d.Date.Month == month).Select(d => d).FirstOrDefault();
        }

        public IQueryable<ServiceExpense> getExpenses(int ID)
        {
            if(ID == 0)
            {
                return serviceSummaryExpenses();
            }
            return db.ServiceExpenses.Where(s => s.ServiceExpenseID == ID).Select(s => s);
        }

        public ServiceExpense getExpense(int ID)
        {

            return db.ServiceExpenses.Where(s => s.ServiceExpenseID == ID).Select(s => s).FirstOrDefault();
        }

        public IQueryable<Bursary> getAllBursaries()
        {
            return db.Bursaries.Where(x => x.Date.Year == year).Select(x => x);
        }

        public decimal totalMonthlyBursaries(IQueryable<Bursary> data, int month)
        {
            decimal sum = 0;
            var result = data.Where(x => x.Date.Month == month).Select(x => x.BursaryValue);
            foreach(var item in result)
            {
                sum += item;
            }
            return sum;
        }

        public GSTRejection getGSTRejection(int itemID)
        {
            return db.GSTRejections.Where(x => x.GAGroupID == itemID || x.ServiceExpenseID == itemID).Select(x => x).SingleOrDefault();
        }
    }
}