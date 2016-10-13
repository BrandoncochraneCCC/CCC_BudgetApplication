using Application.Controllers.Queries;
using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Controllers.CounsellingSummaries
{
    public class IndividualProgramSummary
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();

        private int year;
        CounsellingGroupQueries queries;
        ArrayServices arrayServices = new ArrayServices();
        public IndividualProgramSummary(int year)
        {
            this.year = year;
            queries = new CounsellingGroupQueries(year);
        }

        public List<ProgramViewModel> ProgramTableList(int programID = 0)
        {
            List<ProgramViewModel> tables = new List<ProgramViewModel>();

            return tables;
        }

        public ProgramViewModel IndividualProgramTable(int programID)
        {
            ProgramViewModel table = new ProgramViewModel();
            try
            {
                table.SourceID = programID;
                table.Name = queries.getProgram(programID).Name;

                var section = queries.getProgramSection(programID).FirstOrDefault();
                table.NumberFacilitators = section.NumFacilitator;
                table.NumberOfPrograms = section.Groups.Count();
                var groupData = sectionGroupTable(section.ProgramSectionID);
                table.Groups = groupData;
                table.Hours = sectionHoursTable(section, groupData);
                table.FacilitatorHours = facilitatorHourTable(groupData, section.NumFacilitator);
            }
            catch (Exception ex)
            {
                log.Error("program table error", ex);
            }


            return table;

        }

        private DataTable sectionHoursTable(ProgramSection section, DataTable groupData)
        {
            DataTable table = new DataTable();
            table.dataList = sectionHoursData(section, groupData);

            return table;
        }

        private List<DataLine> sectionHoursData(ProgramSection section, DataTable groupData)
        {
            List<DataLine> list = new List<DataLine>();
            try
            {
                var totalHrs = createDataLine("Total Hours", hourData(groupData, section.NumFacilitator), "hour");
                var percTotalHrs = createPercentDataLine("Percentage of Total Hours", percentHourData(section));

                var percentage = arrayServices.divideArrayByValue(percTotalHrs.percentValues, 100);
                var totalRevHrs = createDataLine("Total Revenue Hours", arrayServices.multiplyArrays(totalHrs.Values, percentage), "hour");
                var prepHrs = createDataLine("Preparation Hours", arrayServices.subtractArrays(totalHrs.Values, totalRevHrs.Values), "hour");
                var hourlyFee = feePerRevenueHour(section, totalRevHrs.Values);
                var feePerRevHr = createDataLine("Fee per Revenue Hour", arrayServices.populateArray(hourlyFee));
                var totalRev = createDataLine("Total Revenue", arrayServices.multiplyArrays(totalRevHrs.Values, feePerRevHr.Values), "highlight");
                list.Add(totalHrs);
                list.Add(percTotalHrs);
                list.Add(totalRevHrs);
                list.Add(prepHrs);
                list.Add(feePerRevHr);
                list.Add(totalRev);
            }
            catch (Exception ex)
            {
                log.Info("Section hours exception", ex);
            }


            return list;
        }

        private decimal[] totalRevenue(decimal[] revenue, decimal[] fee)
        {
            decimal[] values = new decimal[12];
            for (var i = 0; i < 12; i++)
            {
                values[i] = revenue[i] * fee[i];
            }

            return values;
        }

        private DataLine createDataLine(string name, decimal[] values, string viewClass = null)
        {
            DataLine line = new DataLine();
            line.Name = name;
            line.Values = values;
            line.viewClass = viewClass;
            return line;
        }

        private DataLine createPercentDataLine(string name, decimal[] values)
        {
            DataLine line = new DataLine();
            line.Name = name;
            line.percentValues = values;
            return line;
        }

        public DataTable facilitatorHourTable(DataTable groupData, int numFacilitator)
        {
            DataTable table = new DataTable();
            table.dataList = facilitatorDataList(groupData, numFacilitator);
            return table;
        }

        private List<DataLine> facilitatorDataList(DataTable groupData, int numFacilitator)
        {
            List<DataLine> list = new List<DataLine>();
            var data = hourData(groupData, numFacilitator);
            var primary = createDataLine("Primary Facilitator Hours", arrayServices.divideArrayByValue(data, 2));
            var secondary = createDataLine("Total Co Facilitator Hours", primary.Values);
            var total = createDataLine("Total Facilitator Hours", data);

            list.Add(primary);
            list.Add(secondary);
            list.Add(total);

            return list;
        }


        private decimal[] preparationHours(decimal[] totalHrs, decimal[] revenueHours)
        {
            decimal[] values = new decimal[12];

            for (var i = 0; i < 12; i++)
            {
                values[i] = totalHrs[i] - revenueHours[i];
            }

            return values;
        }


        private decimal[] percentHourData(ProgramSection section)
        {
            decimal[] values = new decimal[12];
            var data = queries.getPercentData(section.ProgramSectionID);
            if (data.FirstOrDefault() != null)
            {
                for (var i = 0; i < 12; i++)
                {
                    values[i] = queries.getMonthlyPercentData(data, i + 1).PercentRevenueHours;
                }
            }


            return values;
        }

        public decimal[] hourData(DataTable data, int numFacilitator)
        {
            decimal[] values = new decimal[12];

            foreach (var item in data.dataList)
            {
                for (var i = 0; i < 12; i++)
                {
                    values[i] += item.Values[i] * numFacilitator;
                }
            }

            return values;
        }
        public DataTable sectionGroupTable(int sectionID)
        {
            DataTable table = new DataTable();
            table.dataList = programGroupDataList(sectionID);
            return table;
        }

        private List<DataLine> programGroupDataList(int sectionID)
        {
            List<DataLine> list = new List<DataLine>();
            var groups = queries.getProgramGroups(sectionID);
            foreach (var g in groups)
            {
                list.Add(groupData(g));
            }

            return list;
        }

        private DataLine groupData(Group g)
        {
            DataLine line = new DataLine();
            line.Name = g.Name;
            line.SourceID = g.GroupID;
            line.Values = groupDataValues(g);

            return line;
        }

        private decimal[] groupDataValues(Group g)
        {
            decimal[] values = new decimal[12];
            var data = queries.getMonthlyGroupData(g.GroupID);
            if (data.FirstOrDefault() != null)
            {
                for (var i = 0; i < 12; i++)
                {
                    var temp = queries.getSingleMonthGroupData(data, i + 1);
                    if (temp != null)
                    {
                        values[i] = temp.Hours;
                    }
                }
            }

            return values;
        }

        private decimal feePerRevenueHour(ProgramSection section, decimal[] revenueHours)
        {
            decimal value = 0;
            var totalFee = totalSectionFee(section);
            var temp = revenueHours;
            decimal tHrs = arrayServices.sumArray(temp);

            if (tHrs == 0) { tHrs = 1; }
            value = totalFee / tHrs;
            return value;
        }

        private decimal totalSectionFee(ProgramSection s)
        {
            decimal value = 0;
            var groups = queries.getProgramGroups(s.ProgramSectionID);
            var clients = 0;
            foreach (var g in groups)
            {
                clients += g.NumClients;
            }
            value = s.Fee * clients;

            return value;
        }

        public List<decimal> groupSummaryData(ProgramSection s)
        {
            List<decimal> list = new List<decimal>();
            try
            {
                var groupData = sectionGroupTable(s.ProgramSectionID);

                var totalHrs = hourData(groupData, s.NumFacilitator);
                var percent = arrayServices.divideArrayByValue(percentHourData(s), 100);
                var totalRevHrs = arrayServices.multiplyArrays(totalHrs, percent);
                var prepHrs = arrayServices.subtractArrays(totalHrs, totalRevHrs);
                var hourlyFee = feePerRevenueHour(s, totalRevHrs);

                var total = arrayServices.sumArray(totalHrs);//total program hours
                var prep = arrayServices.sumArray(prepHrs); //totalpreparation hours
                var rev = arrayServices.sumArray(totalRevHrs); // total revenue hours

                list.Add(rev);
                list.Add(prep);
                list.Add(total);
                list.Add(hourlyFee);
                if (hourlyFee != 0)
                {
                    list.Add(rev / hourlyFee); //fee per revenue hour
                    list.Add(prep / hourlyFee);//fee per prep hour

                }
                else
                {
                    list.Add(0);
                    list.Add(0);
                }
                list.Add(total / 2); //primary facilitator hour
                list.Add(total / 2);//co-facilitator hours
            }
            catch (Exception ex)
            {
                log.Info("group summary data failed", ex);
            }
           

            return list;
        }

        public decimal[] revenueHours(ProgramSection s)
        {
            decimal[] result = new decimal[12];
            if (s != null)
            {
                var groupData = sectionGroupTable(s.ProgramSectionID);
                var totalHrs = hourData(groupData, s.NumFacilitator);
                var percent = arrayServices.divideArrayByValue(percentHourData(s), 100);
                result = arrayServices.multiplyArrays(totalHrs, percent);
            }
            return result;


        }

        public decimal[] preparationHours(ProgramSection s)
        {
            var groupData = sectionGroupTable(s.ProgramSectionID);
            var totalHrs = hourData(groupData, s.NumFacilitator);
            var percent = arrayServices.divideArrayByValue(percentHourData(s), 100);
            var totalRevHrs = arrayServices.multiplyArrays(totalHrs, percent);
            return arrayServices.subtractArrays(totalHrs, totalRevHrs);

        }

        public decimal[] feePerRevenueHour()
        {
            var programs = queries.getAllPrograms();
            decimal[] values = new decimal[12];
            decimal totalRevenueHours = 0;
            decimal fee = totalFee();
            foreach (var p in programs)
            {
                var section = queries.getProgramSection(p.ProgramID).FirstOrDefault();
                if (section != null)
                {
                    var groupData = sectionGroupTable(section.ProgramSectionID);
                    List<decimal> list = new List<decimal>();
                    var totalHrs = hourData(groupData, section.NumFacilitator);
                    var percent = arrayServices.divideArrayByValue(percentHourData(section), 100);
                    var totalRevHrs = arrayServices.multiplyArrays(totalHrs, percent);
                    totalRevenueHours += arrayServices.sumArray(totalRevHrs); // total revenue hours
                }

            }

            if (totalRevenueHours != 0)
            {
                var result = fee / totalRevenueHours;
                values = arrayServices.populateArray(result);
            }
            return values;
        }

        public decimal[] feePerPrepHour()
        {
            decimal[] values = new decimal[12];

            try
            {
                var programs = queries.getAllPrograms();

                decimal totalPrepHours = 0;
                decimal fee = totalFee();
                foreach (var p in programs)
                {
                    var section = queries.getProgramSection(p.ProgramID).FirstOrDefault();
                    var groupData = sectionGroupTable(section.ProgramSectionID);
                    List<decimal> list = new List<decimal>();
                    var totalHrs = hourData(groupData, section.NumFacilitator);
                    var percent = arrayServices.divideArrayByValue(percentHourData(section), 100);
                    var totalRevHrs = arrayServices.multiplyArrays(totalHrs, percent);
                    var prepHrs = arrayServices.subtractArrays(totalHrs, totalRevHrs);

                }

                if (totalPrepHours != 0)
                {
                    var result = fee / totalPrepHours;
                    values = arrayServices.populateArray(result);
                }
            }
            catch(Exception ex)
            {
                log.Warn("fee per revenue hour", ex);
            }

            return values;
        }

        private decimal totalFee()
        {
            var programs = queries.getAllPrograms();
            return totalFee(programs);

        }

        private decimal totalFee(IQueryable<Program> programs)
        {
            decimal totalFee = 0;
            foreach (var p in programs)
            {
                var section = queries.getProgramSection(p.ProgramID).FirstOrDefault();
                if (section != null)
                {
                    totalFee += section.Groups.Sum(g => g.NumClients) * section.Fee;
                }
            }

            return totalFee;
        }

        public decimal[] totalHours(ProgramSection s)
        {
            decimal[] values = new decimal[12];
            if (s != null)
            {
                var groupData = sectionGroupTable(s.ProgramSectionID);
                values = hourData(groupData, s.NumFacilitator);
            }
            return values;
        }


    }
}