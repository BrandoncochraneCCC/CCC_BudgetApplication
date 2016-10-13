using Application.Controllers;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class RevenueSummaryQueries
    {
        BudgetDataEntities db = new ObjectInstanceController().db;
        private int year;

        public RevenueSummaryQueries (int year)
        {
            this.year = year;
        }

        public IQueryable<ProgramSection> getSections()
        {
            return db.ProgramSections.Where(x => x.Year == year).Select(x => x);
        }

        public IQueryable<ProgramSection> getSections(Program p)
        {
            return db.ProgramSections.Where(x => x.ProgramID == p.ProgramID).Select(x => x);
        }

        public IQueryable<Program> getPrograms()
        {
            return db.Programs.Select(s => s); 
        }

        public IQueryable<Bursary> getBursaries()
        {
            return db.Bursaries.Where(r => r.Date.Year == year);
        }

        public decimal getMonthlyClawback(IQueryable<Bursary> data, int month)
        {
            decimal value = 0;
            var result = data.Where(r => r.Date.Month == month).Select(r => r.Clawback);
            foreach(var item in result)
            {
                value += item;
            }
            return value;
        }

        public IQueryable<Program> getPrograms(int programID)
        {
            var programs = db.Programs.Select(s => s);
            if (programID != 0)
            {
                programs = programs.Where(p => p.ProgramID == programID).Select(p => p);
            }
            return programs;
        }

        public IEnumerable<SectionRevenueHour> getSectionRevenueHours(int year, ProgramSection s)
        {
            return s.SectionRevenueHours.Where(x => x.Date.Year == year).Select(x => x);
        }

        public IQueryable<Group> getGroups(ProgramSection s)
        {
            return db.Groups.Where(x => x.ProgramSectionID == s.ProgramSectionID).Select(x => x);

        }
        public IQueryable<MonthlyGroup> getMonthlyGroupData()
        {
            return db.MonthlyGroups.Select(r => r);

        }

        public IQueryable<MonthlyGroup> getMonthlyGroupData(Group g)
        {
            return db.MonthlyGroups.Where(r => r.GroupID == g.GroupID).Select(r => r);

        }

        public IQueryable<MonthlyGroup> getSingleMonthGroupData(IQueryable<MonthlyGroup> data, int month)
        {
            return data.Where(r => r.Date.Month == month).Select(r => r);
        }

        public IQueryable<SectionRevenueHour> getSectionRevenueHours(ProgramSection s)
        {
            return db.SectionRevenueHours.Where(x => x.ProgramSectionID == s.ProgramSectionID && x.Date.Year == year).Select(x => x);
        }

        public IQueryable<SectionRevenueHour> getMonthPercentRevenueHours(IQueryable<SectionRevenueHour> s, int month)
        {
            return s.Where(d => d.Date.Month == month).Select(d => d);
        }

        public Group getFirstFullGroupCurrentYear(ProgramSection s)
        {
            return s.Groups.Where(g => g.StartDate.Year == year && g.EndDate.Year == year).Select(g => g).FirstOrDefault();
        }

        public string getRevenueName(int revenueID)
        {
            string result = null;
            var revenue = db.Revenues.Where(r => r.RevenueID == revenueID).Select(r => r).FirstOrDefault();
            if ( revenue != null)
            {
                result = revenue.Name;
            }
            return result;
        }

        public IQueryable<Revenue> getRevenueChildren(int parentID)
        {
            return db.Revenues.Where(r => r.ParentID == parentID).Select(r => r);
        }

        public IQueryable<Revenue> getRevenueChildrenNoSelfJoins(int parentID)
        {
            return getRevenueChildren(parentID).Where(r => r.RevenueID != parentID).Select(r => r);
        }

        public IQueryable<RevenueData> getRevenueData(int revenueID)
        {
            return db.RevenueDatas.Where(d => d.RevenueID == revenueID && d.Date.Year == year).Select(d => d);
        }

        public decimal getMonthlyRevenueData(IQueryable<RevenueData> data, int month)
        {
            return data.Where(r => r.Date.Month == month).Select(r => r.Value).FirstOrDefault();
        }

        public Revenue getRevenue(int revenueID)
        {
            return db.Revenues.Where(r => r.RevenueID == revenueID).Select(r => r).FirstOrDefault();
        }

        public IQueryable<Revenue> getQueryableRevenue(int revenueID)
        {
            return db.Revenues.Where(r => r.RevenueID == revenueID).Select(r => r);
        }

        public IQueryable<Revenue> getTopLevelRevenues()
        {
            return db.Revenues.Where(r => r.ParentID == null || r.ParentID == r.RevenueID).Select(r => r);          
        }
    }
}