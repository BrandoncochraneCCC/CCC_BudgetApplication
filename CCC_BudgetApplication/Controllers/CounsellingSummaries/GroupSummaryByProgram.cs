using Application.Controllers.Queries;
using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Controllers.CounsellingSummaries
{
    public class GroupSummaryByProgram
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();

        private int year;
        CounsellingGroupQueries queries;
        ArrayServices arrayServices = new ArrayServices();
        public GroupSummaryByProgram(int year)
        {
            this.year = year;
            queries = new CounsellingGroupQueries(year);
        }

        public List<groupCounselling> programSummaryDataList()
        {
            List<groupCounselling> list = new List<groupCounselling>();
            var programs = queries.getAllPrograms();
            foreach(var p in programs)
            {                
                    list.Add(programData(p));
            }
            list.Add(tableSum(list));

            return list;
        }

        private groupCounselling tableSum(List<groupCounselling> list)
        {
            groupCounselling total = new groupCounselling();
            total.name = "Total";
            var count = 0;

            decimal clientPerProgram = 0;
            decimal feePerClient = 0;
            decimal revPerHr = 0;
            decimal revPerCHr = 0;
            decimal revPerPHr = 0;
            foreach(var item in list)
            {
                total.numPrograms += item.numPrograms;
                total.totalClients += item.totalClients;               
                total.totalFee += item.totalFee;
                total.totalHours += item.totalHours;
                total.revenueHours += item.revenueHours;
                total.prepHours += item.prepHours;
                total.primaryHour += item.primaryHour;
                total.coHours += item.coHours;

                clientPerProgram += item.clientsPerProgram;
                feePerClient += item.feePerClient;
                revPerHr += item.revPerHour;
                revPerCHr += item.revPerCounsellingHour;
                revPerPHr += item.revPerPrepHour;

                if (item.revPerHour != 0)
                {
                    count++;
                }
            }

            if(count != 0)
            {
                total.clientsPerProgram = clientPerProgram / count;
                total.feePerClient = feePerClient / count;
                total.revPerHour = revPerHr / count;
                total.revPerCounsellingHour = revPerCHr / count;
                total.revPerPrepHour = revPerPHr / count;
            }

            total.ViewClass = "highlight";
            return total;

        }

        public groupCounselling programData(Program p)
        {
            groupCounselling item = new groupCounselling();
            try
            {
                var section = queries.getProgramSection(p.ProgramID).FirstOrDefault();
                item.name = p.Name;
                item.programID = p.ProgramID;
                item.numPrograms = section.Groups.Count();
                item.totalClients = section.Groups.Sum(g => g.NumClients);
                if (item.numPrograms != 0)
                {
                    item.clientsPerProgram = item.totalClients / item.numPrograms;
                }
                item.feePerClient = section.Fee;
                item.totalFee = section.Fee * item.totalClients;
                setSectionHours(item, section);
                if (item.revenueHours != 0)
                {
                    item.revPerHour = item.totalFee / item.revenueHours;
                }
                if (item.totalHours != 0)
                {
                    item.revPerCounsellingHour = (item.revenueHours / item.totalHours) * item.revPerHour;
                    item.revPerPrepHour = (item.prepHours / item.totalHours) * item.revPerHour;
                }
                item.ViewClass = "";
            }
            catch(Exception ex)
            {
                log.Error("program data error", ex);
            }
            
            return item;
        }

        private void setSectionHours(groupCounselling item, ProgramSection section)
        {
            var groups = queries.getProgramGroups(section.ProgramSectionID);
            decimal[] percent = getPercent(section.ProgramSectionID);          
            decimal[] total = totalHours(groups, section);
            decimal[] revenue = arrayServices.multiplyArrays(total, percent);
            decimal[] prep = arrayServices.subtractArrays(total, revenue);
            decimal[] primary = arrayServices.divideArrayByValue(total, 2);
            decimal[] co = primary;

            item.totalHours = (int)arrayServices.sumArray(total);
            item.revenueHours = (int)arrayServices.sumArray(revenue);
            item.prepHours = (int)arrayServices.sumArray(prep);
            item.primaryHour = (int)arrayServices.sumArray(primary);
            item.coHours = (int)arrayServices.sumArray(co);

        }

        public decimal feeRevHour()
        {
            decimal fee = 0;
            decimal totalFee = 0;
            decimal totalRevenueHours = 0;

            try
            {
                var programs = queries.getAllPrograms();

                foreach (var p in programs)
                {
                    groupCounselling temp = programData(p);
                    totalFee += temp.totalFee;
                    totalRevenueHours += temp.revenueHours;
                }

                if (totalRevenueHours != 0)
                {
                    fee = totalFee / totalRevenueHours;
                }
            }
            catch(Exception ex)
            {
                log.Info("fee per revenue hour", ex);
            }


            return fee;
        }

        public decimal feePerExpHour()
        {
            decimal fee = 0;
            decimal totalFee = 0;
            decimal totalRevenueHours = 0;
            decimal totalPrepHours = 0;
            decimal totalHours = 0;

            try
            {
                var programs = queries.getAllPrograms();

                foreach (var p in programs)
                {
                    groupCounselling temp = programData(p);
                    totalFee += temp.totalFee;
                    totalRevenueHours += temp.revenueHours;
                    totalPrepHours += temp.prepHours;
                    totalHours += temp.totalHours;
                }
                if (totalHours != 0 && totalRevenueHours != 0)
                {

                    var hours = totalPrepHours / totalHours;
                    var revPerHour = totalFee / totalRevenueHours;
                    fee = hours * revPerHour;
                }
            }
            catch(Exception ex)
            {
                log.Info("fee per exp hour", ex);              
            }
            


            return fee;
        }

        public decimal feePerPrepHour()
        {
            decimal fee = 0;
            decimal totalFee = 0;
            decimal totalRevenueHours = 0;
            decimal totalHours = 0;
            try
            {
                var programs = queries.getAllPrograms();

                foreach (var p in programs)
                {
                    groupCounselling temp = programData(p);
                    totalFee += temp.totalFee;
                    totalRevenueHours += temp.revenueHours;
                    totalHours += temp.totalHours;
                }
                if (totalHours != 0 && totalRevenueHours != 0)
                {

                    var hours = totalRevenueHours / totalHours;
                    var revPerHour = totalFee / totalRevenueHours;
                    fee = hours * revPerHour;
                }
            }
            catch(Exception ex)
            {
                log.Info("fee per prep hour", ex);
            }
            
            

            return fee;
        }



        public decimal[] totalHours(IQueryable<Group> groups, ProgramSection s)
        {
            decimal[] hours = new decimal[12];

            try
            {
                foreach (var g in groups)
                {
                    var data = queries.getMonthlyGroupData(g.GroupID);
                    if (data.FirstOrDefault() != null)
                    {
                        for (var i = 0; i < 12; i++)
                        {
                            var result = queries.getSingleMonthGroupData(data, i + 1);
                            if (result != null)
                            {
                                hours[i] += result.Hours;

                            }
                        }
                    }

                }

                for (var i = 0; i < 12; i++)
                {
                    hours[i] *= s.NumFacilitator;
                }
            }
            catch(Exception ex)
            {
                log.Warn("total hours calculation failed", ex);
            }
            

            return hours;
        }

        private decimal[] getPercent(int sectionID)
        {
            decimal[] values = new decimal[12];
            try
            {
                var data = queries.getPercentData(sectionID);
                if (data.FirstOrDefault() != null)
                {
                    for (var i = 0; i < 12; i++)
                    {
                        var result = queries.getMonthlyPercentData(data, i + 1);
                        if (result != null)
                        {
                            values[i] += result.PercentRevenueHours / 100;

                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Warn("get percent calculation failed", ex);
            }

             

            return values;
        }

        

    }
}