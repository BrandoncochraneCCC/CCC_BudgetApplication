using Application.Controllers;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Controllers.Queries
{
    public class GeneralExpenseQueries
    {
        private int year;
        BudgetDataEntities db = new ObjectInstanceController().db;

        public GeneralExpenseQueries(int year)
        {
            this.year = year;
        }

        public IQueryable<GAGroup> getGeneralExpenses()
        {
            return db.GAGroups.Select(e => e);
        }

        public IQueryable<GAGroup> getGeneralExpenses(int GAGroupID)
        {
            if (GAGroupID == 0)
            {
                return getGeneralExpenses();
            }
            else
            {
                return db.GAGroups.Where(e => e.GAGroupID == GAGroupID).Select(e => e);
            }
        }

        public IQueryable<GAGroup> getTopLevelGeneralExpenses()
        {
            return db.GAGroups.Where(e => e.ParentID == null).Select(e => e);
        }

        public IQueryable<GAExpense> getGeneralExpenseData(GAGroup expense)
        {
            return db.GAExpenses.Where(e => e.GroupID == expense.GAGroupID && e.Date.Year == year).Select(e => e);
        }

        public GAExpense getMonthlyGeneralExpenseData(IQueryable<GAExpense> expense, int month)
        {
            return expense.Where(e => e.Date.Month == month).Select(e => e).FirstOrDefault();
        }

        public IQueryable<Airfare> getAir(int conferenceID)
        {
            if(conferenceID == 0)
            {
                return db.Airfares.Where(a => a.Date.Year == year).Select(a => a);
            }
            else
            {
                return db.Airfares.Where(a => a.ConferenceID == conferenceID && a.Date.Year == year).Select(a => a);
            }
        }

        public IQueryable<GasMileage> getGas(int conferenceID)
        {
            if (conferenceID == 0)
            {
                return db.GasMileages.Where(a => a.Date.Year == year).Select(a => a);
            }
            else
            {
                return db.GasMileages.Where(a => a.ConferenceID == conferenceID && a.Date.Year == year).Select(a => a);
            }
        }
        public IQueryable<Hotel> getHotel(int conferenceID)
        {
            if (conferenceID == 0)
            {
                return db.Hotels.Where(a => a.Date.Year == year).Select(a => a);
            }
            else
            {
                return db.Hotels.Where(a => a.ConferenceID == conferenceID && a.Date.Year == year).Select(a => a);
            }
        }

        public IQueryable<MealEntertainment> getMeal(int conferenceID)
        {
            if (conferenceID == 0)
            {
                return db.MealEntertainments.Where(a => a.Date.Year == year).Select(a => a);
            }
            else
            {
                return db.MealEntertainments.Where(a => a.ConferenceID == conferenceID && a.Date.Year == year).Select(a => a);
            }
        }
        public IQueryable<MiscTravel> getMisc(int conferenceID)
        {
            if (conferenceID == 0)
            {
                return db.MiscTravels.Where(a => a.Date.Year == year).Select(a => a);
            }
            else
            {
                return db.MiscTravels.Where(a => a.ConferenceID == conferenceID && a.Date.Year == year).Select(a => a);
            }
        }

        public decimal getMonthlyAirExpense(IQueryable<Airfare> data, int month)
        {
           return data.Where(d => d.Date.Month == month).Select(d => d.Value).FirstOrDefault();
        }
        public decimal getMonthlyGasExpense(IQueryable<GasMileage> data, int month)
        {
            return data.Where(d => d.Date.Month == month).Select(d => d.Value).FirstOrDefault();
        }
        public decimal getMonthlyHotelExpense(IQueryable<Hotel> data, int month)
        {
            return data.Where(d => d.Date.Month == month).Select(d => d.Value).FirstOrDefault();
        }
        public decimal getMonthlyMealExpense(IQueryable<MealEntertainment> data, int month)
        {
            return data.Where(d => d.Date.Month == month).Select(d => d.Value).FirstOrDefault();
        }
        public decimal getMonthlyMiscExpense(IQueryable<MiscTravel> data, int month)
        {
            return data.Where(d => d.Date.Month == month).Select(d => d.Value).FirstOrDefault();
        }

        public IQueryable<Conference> getConferences()
        {
            return db.Conferences.Where(c => c.Year == year).Select(c => c);
        }
        public IQueryable<ServiceExpense> getChildren(ServiceExpense expense)
        {
                return db.ServiceExpenses.Where(g => g.ParentID == expense.ServiceExpenseID).Select(g => g);
        }


        public IQueryable<GAGroup> getChildren(int expenseID)
        {
            if (expenseID == 0)
            {
                return getTopLevelGeneralExpenses();
            }
            else
            {
                return db.GAGroups.Where(g => g.ParentID == expenseID).Select(g => g);
            }
        }

        public bool hasChildren(int expenseID)
        {
            bool hasChildren = false;

            if (db.GAGroups.Where(g => g.ParentID == expenseID).Select(g => g).FirstOrDefault() != null)
            {
                hasChildren = true;
            }


            return hasChildren;
        }

        public GAGroup getGeneralExpense(int expenseID)
        {
            return db.GAGroups.Where(g => g.GAGroupID == expenseID).Select(g => g).FirstOrDefault();
        }

        public IQueryable<ExistingHardware> getExistingHardware(int expenseID)
        {
            return db.ExistingHardwares.Where(g => g.GAGroupID == expenseID).Select(g => g);
        }

        public IQueryable<GAExpense> getChildData(int expenseID)
        {
            return db.GAExpenses.Where(e => e.GroupID == expenseID && e.Date.Year == year).Select(e => e);
        }

        public bool hasData(int expenseID)
        {
            bool hasData = false;

            if (db.GAExpenses.Where(e => e.GroupID == expenseID && e.Date.Year == year).Select(e => e).FirstOrDefault() != null)
            {
                hasData = true;
            }


            return hasData;
        }

        public decimal getGSTRate()
        {
            return getGSTRate(0);
        }

        public decimal getGSTRate(int yr)
        {
            if(yr == 0)
            {
                yr = year;
            }
            return db.UnchangingValues.Where(g => g.Name.ToLower() == "gst" && g.Year == yr).Select(getGSTRate => getGSTRate.Value).FirstOrDefault();
        }

        public IQueryable<ServiceExpense> getServiceGSTExpenses()
        {
            return db.ServiceExpenses.Where(r => r.GSTRejections.Count > 0).Select(r => r);

        }

        public IEnumerable<GAGroup> getGAGSTExpenses()
        {
            return db.GAGroups.Where(r => r.GSTRejections.Count > 0).Select(r => r);

        }


    }
}