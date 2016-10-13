using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Controllers.Queries
{
    public class CounsellingGroupQueries
    {
        private int year;
        BudgetDataEntities db = new ObjectInstanceController().db;
        public CounsellingGroupQueries(int year)
        {
            this.year = year;
        }

        public Program getProgram(int programID)
        {
            return db.Programs.Where(p => p.ProgramID == programID).Select(p => p).FirstOrDefault();
        }

        public IQueryable<ProgramSection> getProgramSection(int programID)
        {
            return db.ProgramSections.Where(s => s.ProgramID == programID && s.Year == year).Select(s => s);
        }
        public IQueryable<Group> getProgramGroups(int sectionID)
        {
            return db.Groups.Where(g => g.ProgramSectionID == sectionID).Select(g => g);
        }

        public IQueryable<MonthlyGroup> getMonthlyGroupData(int groupID)
        {
            return db.MonthlyGroups.Where(m => m.GroupID == groupID).Select(m => m);
        }

        public MonthlyGroup getSingleMonthGroupData(IQueryable<MonthlyGroup> data, int month)
        {
            return data.Where(d => d.Date.Month == month).Select(d => d).FirstOrDefault();
        }

        public IQueryable<SectionRevenueHour> getPercentData(int sectionID)
        {
            return db.SectionRevenueHours.Where(s => s.ProgramSectionID == sectionID).Select(s => s);
        }

        public SectionRevenueHour getMonthlyPercentData(IQueryable<SectionRevenueHour> data, int month)
        {
            return data.Where(s => s.Date.Month == month).Select(s => s).FirstOrDefault();
        }

        public IQueryable<Program> getAllPrograms()
        {
            return db.Programs.Select(p => p);
        }

        public IQueryable<CounsellingGroupType> getCounsellingGroup(int ID)
        {
            if(ID == 0)
            {
                return db.CounsellingGroupTypes.Select(x => x);
            }
            else
            {
                return db.CounsellingGroupTypes.Where(x => x.Id == ID).Select(x => x);
            }
        }


        public IQueryable<Program> getProgramByGroupTypeID(int ID)
        {
            return db.Programs.Where(x => x.CounsellingGroupTypeID == ID).Select(x => x);
        }

        public IQueryable<CounsellingServiceData> getCounsellingService(int serviceID)
        {
            return db.CounsellingServiceDatas.Where(x => x.CounsellingServiceID == serviceID && x.Date.Year == year).Select(x => x);
        }

        public CounsellingServiceData getMonthlyServiceData(IQueryable<CounsellingServiceData> data, int month)
        {
            return data.Where(x => x.Date.Month == month).Select(x => x).FirstOrDefault();
        }

    }
}