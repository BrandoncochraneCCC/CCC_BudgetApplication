using Application.Controllers.CounsellingSummaries;
using Application.Controllers.Queries;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class CounsellingProgram
    {

        private RevenueSummaryQueries queries;

        public CounsellingProgram(int year)
        {
            queries = new RevenueSummaryQueries(year);
        }


        /**
         * steps to get data to calculate group values
         * for the top level summary
         **/
        public decimal[] RevenueSummaryGroupDataValues(int year)
        {
            CounsellingHours controller = new CounsellingHours(year);
            List<DataTable> result = new List<DataTable>();
            CounsellingGroupQueries queries = new CounsellingGroupQueries(year);

            var groups = queries.getCounsellingGroup(0);
            foreach (var g in groups)
            {
                result.Add(controller.groupHourTotalsTable(g));
            }
            decimal[] totalRevenue = controller.revenueGroupsDataValues(result);
            decimal[] funding = controller.homefrontFunding();


            //return combineArrays(tGroupRev, funding);
            return combineArrays(totalRevenue, funding);
        }

        /**
         * gets homefront Funding data 
         * 
         * returns array of data index + 1 = month data corresponds to
         **/
        private decimal[] homeFrontFunding()
        {
            decimal[] values = new decimal[12];

            //not implemented in database

            return values;
        }

        /**
         * gets revenue for all programs 
         * 
         * returns array of data index + 1 = month 
         **/
        private decimal[] totalGroupRevenue()
        {
            decimal[] totalGroupRevenue = new decimal[12];

            var programs = queries.getSections();
            foreach(var p in programs)
            {
                decimal[] tRevenue = totalRevenue(p);

                totalGroupRevenue = combineArrays(totalGroupRevenue, tRevenue);
            }


            return totalGroupRevenue;
        }

        /**
         * gets total revenue for a specific program 
         * 
         * returns array of data index + 1 = month 
         **/
        private decimal[] totalRevenue(ProgramSection p)
        {
            decimal[] totalRevenue = new decimal[12];

            decimal feePerHour = feePerRevenueHour(p);
            decimal[] revHrs = revenueHours(p);


            return totalRevenue;
        }

        /**
         * revenue gained for each revenue hour
         * 
         * returns fee per revenue hour 
         **/
        private decimal feePerRevenueHour(ProgramSection p)
        {
            decimal fee = numberOfClients(p) * p.Fee;
            int totalNumClients = numberOfClients(p);
            decimal[] revHrs = revenueHours(p);

            var tRevHrs = sumArray(revHrs);

            if (tRevHrs != 0)
            {
                fee /= tRevHrs;
            }

            return fee;
        }

        /**
         * gets total revenue for a specific program 
         * 
         * returns array of data index + 1 = month 
         **/
        private decimal[] revenueHours(ProgramSection p)
        {      
            decimal[] revenueHours = totalHours(p);
            decimal[] percentRevHrs = percentRevenueHours(p);

            revenueHours = multiplyArrays(revenueHours, percentRevHrs);

            return revenueHours;
        }

        /**
         * gets total hours for a program
         * monthly hours * number of facilitators
         * 
         * returns array of data index + 1 = month 
         **/
        private decimal[] totalHours(ProgramSection p)
        {
            decimal[] totalHours = monthlyHours(p);

            totalHours = multiplyArraybyValue(totalHours, p.NumFacilitator);

            return totalHours;
        }

        /**
         * gets monthly hours for a program
         * 
         * returns array of data index + 1 = month 
         **/
        private decimal[] monthlyHours(ProgramSection p)
        {
            decimal[] monthlyHours = new decimal[12];

            var groups = queries.getGroups(p);
            foreach(var g in groups)
            {
                decimal[] hours = new decimal[12];

                var groupData = queries.getMonthlyGroupData(g);
                
                foreach(var d in groupData)
                {
                    hours[d.Date.Month - 1] = d.Hours;
                }

                combineArrays(monthlyHours, hours);
            }

            return monthlyHours;
        }

        /**
         * percent of revenue hours for a program
         **/
        private decimal[] percentRevenueHours(ProgramSection p)
        {
            decimal[] percentRevenueHours = new decimal[12];

            var result = queries.getSectionRevenueHours(p);
            foreach(var r in result)
            {
                percentRevenueHours[r.Date.Month - 1] = r.PercentRevenueHours / 100;
            }

            return percentRevenueHours;
        }

        /**
         * number of clients for a specific program 
         **/
        private int numberOfClients(ProgramSection p)
        {
            int clients = 0;

            var groups = queries.getGroups(p);
            foreach(var g in groups)
            {
                clients += g.NumClients;
            }

            return clients;
        }


        private decimal[] combineArrays(decimal[] destination, decimal[] source)
        {
            var length = destination.Length;
            if (source.Length < destination.Length)
            {
                length = source.Length;
            }

            for (var i = 0; i < length; i++)
            {
                destination[i] += source[i];
            }

            return destination;
        }

        private decimal[] multiplyArraybyValue(decimal[] data, decimal value)
        {
            for(var i = 0; i < data.Length; i++)
            {
                data[i] *= value;
            }

            return data;
        }

        private decimal[] multiplyArrays(decimal[] destination, decimal[] source)
        {
            for (var i = 0; i < destination.Length; i++)
            {
                destination[i] *= source[i];
            }

            return destination;
        }

        private decimal sumArray(decimal[] array)
        {
            decimal sum = 0;
            for(var i = 0; i < array.Length; i++)
            {
                sum += array[i];
            }
            return sum;
        }


        //private void iteratePrograms(decimal[] values)
        //{

        //}

        //private decimal[] iterateSections(ProgramSection s)
        //{
        //    decimal[] values = new decimal[12];



        //    return values;
        //}

        //private decimal[] multiplyArray(decimal[] array, int value)
        //{
        //    return multiplyArray(array, (decimal)value);
        //}

        //private decimal[] multiplyArray(decimal[] array, decimal value)
        //{
        //    if(value == 0) { value = 1; }
        //    for (var i = 0; i < array.Length - 1; i++)
        //    {
        //        array[i] *= value;
        //    }

        //    return array;
        //}


        //private decimal[] combineArray(decimal[] destination, decimal[] source)
        //{
        //    var length = destination.Length;
        //    if (source.Length < destination.Length)
        //    {
        //        length = source.Length;
        //    }

        //    for (var i = 0; i < 12; i++)
        //    {
        //        destination[i] += source[i];
        //    }

        //    return destination;
        //}


        //private decimal[] getTotalHours(ProgramSection s)
        //{
        //    decimal[] totalHours = new decimal[12];

        //    var groups = queries.getGroups(s);
        //    foreach(var g in groups)
        //    {
        //        var data = queries.getMonthlyGroupData(g);

        //        for (var i = 0; i < 12; i++)
        //        {
        //            try
        //            {
        //                var hours = queries.getSingleMonthGroupData(data, i + 1);
        //                totalHours[i] += hours.FirstOrDefault().Hours;
        //            }
        //            catch (Exception e) { }

        //        }
        //    }
        //    //for (var i = 0; i < 12; i++)
        //    //{
        //    //    totalHours[i] *= s.NumFacilitator;
        //    //}

        //        return totalHours;
        //}



        //private decimal[] getPercentRevenueHours(ProgramSection s)
        //{
        //    decimal[] percents = new decimal[12];

        //    var revenueHours = queries.getSectionRevenueHours(s);
        //    for (var i = 0; i < 12; i++)
        //    {
        //        try
        //        {
        //            var result = queries.getMonthPercentRevenueHours(revenueHours, i + 1);
        //            if (result != null)
        //            {
        //                percents[i] = (result.FirstOrDefault().PercentRevenueHours) / 100;
        //            }
        //        }
        //        catch (Exception e) { }
        //    }

        //    return percents;
        //}

        //private decimal programSectionFee(ProgramSection section)
        //{
        //    var value = 0;

        //    if (section.Groups != null)
        //    {
        //        var numClients = (section.Groups.Select(r => r.NumClients)).Sum();
        //        value = (Int32)(numClients * section.Fee);
        //    }


        //    return value;
        //}

        //private decimal getValueTotalArray(decimal[] data)
        //{
        //    decimal value = 0;

        //    for (var i = 0; i < data.Length - 1; i++)
        //    {
        //        value += (decimal)data[i];
        //    }

        //    return value;
        //}

        //private decimal[] getTotalRevenueHours(decimal[] data, decimal[] percents)
        //{
        //    var length = data.Length;
        //    if(percents.Length < length)
        //    {
        //        length = percents.Length;
        //    }

        //    for (var i = 0; i < length; i++)
        //    {
        //        decimal percent = percents[i];
        //        if(percents[i] == 0) { percent = 1; }
        //        data[i] *= percent;
        //    }

        //    return data;
        //}

        //private decimal feePerHour(ProgramSection s)
        //{
        //    var hours = totalGroupHour(queries.getFirstFullGroupCurrentYear(s));

        //    if (hours == 0) { return 0; }
        //    return s.Fee / hours;
        //}

        //private int totalGroupHour(Group g)
        //{
        //    var hrs = 0;
        //    try
        //    {
        //        if (hasMonthlyGroup(g))
        //        {
        //            foreach (var data in queries.getMonthlyGroupData(g))
        //            {
        //                if (data.Hours != 0)
        //                {
        //                    hrs += data.Hours;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e) { }

        //    return hrs;
        //}

        //private bool hasMonthlyGroup(Group g)
        //{
        //    bool hasGroups = false;

        //    var result = queries.getMonthlyGroupData(g);
        //    if (result != null)
        //    {
        //        hasGroups = true;
        //    }

        //    return hasGroups;
        //}

    }
}