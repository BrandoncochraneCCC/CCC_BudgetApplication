using Application.Controllers.Queries;
using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Controllers.CounsellingSummaries
{
    public class CounsellingHours
    {
        private int year;
        CounsellingGroupQueries queries;
        IndividualProgramSummary controller;
        ArrayServices arrayServices = new ArrayServices();
        public CounsellingHours(int year)
        {
            this.year = year;
            queries = new CounsellingGroupQueries(year);
            controller = new IndividualProgramSummary(year);
        }


        public DataTable totalRevenueHours(CounsellingGroupType group)
        {
            DataTable table = new DataTable();
            table.tableName = group.Name;
            table.sourceID = group.Id;
            table.dataList = revenueHoursDataList(group);
            return table;
        }

        private decimal[] totalRevenueHours(List<DataTable> data)
        {
            decimal[] values = new decimal[12];
            foreach(var table in data)
            {
                var group = queries.getCounsellingGroup(table.sourceID).FirstOrDefault();
                var result = totalRevenueHours(group);
                foreach(var line in result.dataList)
                {
                    for(var i = 0; i< 12; i++)
                    {
                        values[i] += line.Values[i];
                    }
                }
            }
            return values;
        }

        private List<DataLine> preparationHoursDataList(CounsellingGroupType group)
        {
            List<DataLine> list = new List<DataLine>();
            var programs = queries.getProgramByGroupTypeID(group.Id);
            foreach (var p in programs)
            {
                list.Add(preparationHoursDataLine(p));
            }


            return list;
        }

        public DataTable totalPreparationHours(CounsellingGroupType group)
        {
            DataTable table = new DataTable();
            table.tableName = group.Name;
            table.sourceID = group.Id;
            table.dataList = preparationHoursDataList(group);
            return table;
        }

        private decimal[] totalPreparationHours(List<DataTable> data)
        {
            decimal[] values = new decimal[12];
            foreach (var table in data)
            {
                var group = queries.getCounsellingGroup(table.sourceID).FirstOrDefault();
                var result = totalPreparationHours(group);
                foreach (var line in result.dataList)
                {
                    for (var i = 0; i < 12; i++)
                    {
                        values[i] += line.Values[i];
                    }
                }
            }
            return values;
        }


        private List<DataLine> revenueHoursDataList(CounsellingGroupType group)
        {
            List<DataLine> list = new List<DataLine>();
            var programs = queries.getProgramByGroupTypeID(group.Id);
            foreach (var p in programs)
            {
                list.Add(revenueHoursDataLine(p));
            }



            return list;
        }

        private DataLine revenueHoursDataLine(Program p)
        {
            DataLine line = new DataLine();

            line.Name = p.Name;
            line.Controller = "CounsellingProgram";
            line.Action = "IndividualProgram";
            line.SourceID = p.ProgramID;
            line.viewClass = "hour";
            var section = queries.getProgramSection(p.ProgramID).FirstOrDefault();
            line.Values = controller.revenueHours(section);

            return line;
        }


        private DataLine preparationHoursDataLine(Program p)
        {
            DataLine line = new DataLine();

            line.Name = p.Name;
            line.SourceID = p.ProgramID;
            line.Controller = "CounsellingProgram";
            line.Action = "IndividualProgram";
            line.viewClass = "hour";
            var section = queries.getProgramSection(p.ProgramID).FirstOrDefault();
            line.Values = controller.preparationHours(section);

            return line;
        }


        public DataTable revenueHourTotalsTable(List<DataTable> list)
        {
            DataTable table = new DataTable();
            table.tableName = "";
            table.dataList = revenueTotalLines("Revenue Hours", "Fee Per Revenue Hour", "Total Revenue", list);
            return table;
        }

        private List<DataLine> revenueTotalLines(string one, string two, string three, List<DataTable> list)
        {
            List<DataLine> lines = new List<DataLine>();
            var lineOne = (createDataLine(one, sumTable(list), "hour"));
            var lineTwo = (createDataLine(two, feePerRevHour()));
            lines.Add(lineOne);
            lines.Add(lineTwo);
            lines.Add(createDataLine(three, arrayServices.multiplyArrays(lineOne.Values, lineTwo.Values), "money"));

            return lines;
        }

        public DataTable preparationHourTotalsTable(List<DataTable> list)
        {
            DataTable table = new DataTable();
            table.tableName = "";

            table.dataList = preparationTotalLines("Expense Hours", "Fee Per Expense Hour", "Total Preparation Expenses", list);
            return table;
        }

        private List<DataLine> preparationTotalLines(string one, string two, string three, List<DataTable> list)
        {
            List<DataLine> lines = new List<DataLine>();
            var lineOne = (createDataLine(one, sumTable(list), "hour"));
            var lineTwo = (createDataLine(two, feePerPrepHour()));
            lines.Add(lineOne);
            lines.Add(lineTwo);
            lines.Add(createDataLine(three, arrayServices.multiplyArrays(lineOne.Values, lineTwo.Values)));

            return lines;
        }

        protected DataLine createDataLine(string name, decimal[] values, string viewClass = "")
        {
            DataLine line = new DataLine();
            line.Name = name;
            line.Values = values;
            line.viewClass = viewClass;

            return line;
        }
        private decimal[] sumTable(List<DataTable> list)
        {
            decimal[] values = new decimal[12];
            foreach (var item in list)
            {
                foreach (var line in item.dataList)
                {
                    int count = 0;
                    foreach (var value in line.Values)
                    {
                        values[count] += value;
                        count++;
                    }

                }
            }

            return values;
        }

        private decimal[] feePerRevHour()
        {
            GroupSummaryByProgram controller = new GroupSummaryByProgram(year);
            var feePerRevHr =  controller.feeRevHour();
            return arrayServices.populateArray(feePerRevHr);
        }

        private decimal[] feePerExpHour()
        {
            GroupSummaryByProgram controller = new GroupSummaryByProgram(year);
            var feePerRevHr = controller.feePerExpHour();
            return arrayServices.populateArray(feePerRevHr);
        }
        private decimal[] feePerPrepHour()
        {
            GroupSummaryByProgram controller = new GroupSummaryByProgram(year);
            var feePrepHr = controller.feePerPrepHour();
            return arrayServices.populateArray(feePrepHr);
        }

        public DataTable groupHourTotalsTable(CounsellingGroupType group)
        {
            DataTable table = new DataTable();
            table.tableName = group.Name;
            table.sourceID = group.Id;
            table.dataList = totalHoursDataList(group);
            return table;
        }

        private List<DataLine> totalHoursDataList(CounsellingGroupType group)
        {
            List<DataLine> list = new List<DataLine>();
            var programs = queries.getProgramByGroupTypeID(group.Id);
            foreach (var p in programs)
            {
                list.Add(totalHoursDataLine(p));
            }
            return list;
        }
        
        private DataLine totalHoursDataLine(Program p)
        {
            DataLine line = new DataLine();
            line.Name = p.Name;
            line.Controller = "CounsellingProgram";
            line.Action = "IndividualProgram";
            line.SourceID = p.ProgramID;
            line.viewClass = "hour";
            var section = queries.getProgramSection(p.ProgramID).FirstOrDefault();
            line.Values = controller.totalHours(section);
            return line;
        }
        public DataTable groupTotalsTable(List<DataTable> list)
        {
            DataTable table = new DataTable();
            table.tableName = "";
            table.dataList = groupTotalLine(list);
            return table;
        }
        public DataTable groupTotalsHourTable(List<DataTable> list)
        {
            DataTable table = new DataTable();
            table.tableName = "";
            table.dataList = groupHoursDataLine(list);
            return table;
        }

        public decimal[] revenueGroupsDataValues(List<DataTable> data)
        {
            decimal[] feePerRevHr = controller.feePerRevenueHour();
            decimal[] revenueHours = totalRevenueHours(data);

            return arrayServices.multiplyArrays(revenueHours, feePerRevHr);
        }

        private List<DataLine> groupTotalLine(List<DataTable> data)
        {
            List<DataLine> list = new List<DataLine>();
            DataLine groupHrs = createDataLine("Total Group Hours", totalGroupHours(data), "hour");
            DataLine revHrs = createDataLine("Total Revenue Hours", totalRevenueHours(data), "hour");
            DataLine prepHrs = createDataLine("Total Preparation Hours", totalPreparationHours(data), "hour");
            DataLine feePerRevHr = createDataLine("Fee Per Revenue Hour", controller.feePerRevenueHour());
            DataLine totalRev = createDataLine("Total Revenue", arrayServices.multiplyArrays(revHrs.Values, feePerRevHr.Values));
            DataLine hfFunding = createDataLine("Homefront Funding", homefrontFunding());
            DataLine totalGroupRev = createDataLine("Total Group Revenue", arrayServices.combineArrays(totalRev.Values, hfFunding.Values));

            list.Add(groupHrs);
            list.Add(revHrs);
            list.Add(prepHrs);
            list.Add(feePerRevHr);
            list.Add(totalRev);
            list.Add(hfFunding);
            list.Add(totalGroupRev);

            return list;
        }

        private List<DataLine> groupHoursDataLine(List<DataTable> data)
        {
            List<DataLine> list = new List<DataLine>();
            var totalHours = totalGroupHours(data);
            DataLine primary = createDataLine("Total Primary Facilitator Hours", arrayServices.divideArrayByValue(totalHours, 2), "hour");
            DataLine co = createDataLine("Total Co Facilitator Hours", arrayServices.divideArrayByValue(totalHours, 2), "hour");
            DataLine facilitator = createDataLine("TotalFacilitator Hours", totalHours, "hour");
            DataLine feePerExpenseHr = createDataLine("Fee per Expense Hour", feePerExpHour());
            DataLine primaryExpense  = createDataLine("Total Primary Facilitator Expenses", arrayServices.multiplyArrays(facilitator.Values, feePerExpenseHr.Values));

            list.Add(primary);
            list.Add(co);
            list.Add(facilitator);
            list.Add(feePerExpenseHr);
            list.Add(primaryExpense);


            return list;
        }


        public decimal[] homefrontFunding()
        {
            decimal[] values = new decimal[12];
            var result = queries.getCounsellingService(ObjectInstanceController.HOMEFRONTFUNDINGID);
            if(result.FirstOrDefault() != null)
            {
                for (var i = 0; i < 12; i++)
                {
                    values[i] += queries.getMonthlyServiceData(result, i + 1).Value;
                }
            }
            
            return values;
        }

        private decimal[] totalGroupHours(List<DataTable> data)
        {
            var programs = queries.getAllPrograms();
            decimal[] total = new decimal[12];
            foreach (var p in programs)
            {
                var s = queries.getProgramSection(p.ProgramID).FirstOrDefault();
                var groupData = controller.sectionGroupTable(s.ProgramSectionID);
                total = arrayServices.combineArrays(total, controller.hourData(groupData, s.NumFacilitator));
            }

            return total;
        }

    }
}