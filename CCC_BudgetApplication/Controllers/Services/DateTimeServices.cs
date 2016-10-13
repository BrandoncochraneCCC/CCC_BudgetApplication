using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Controllers.Services
{
    public  class DateTimeServices
    {
        private int year;

        public DateTimeServices(int year)
        {
            this.year = year;
        }
        /// <summary>
        /// Calculates number of business days, taking into account:
        ///  - weekends (Saturdays and Sundays)
        ///  - bank holidays in the middle of the week
        /// </summary>
        /// <param name="firstDay">First day in the time interval</param>
        /// <param name="lastDay">Last day in the time interval</param>
        /// <param name="bankHolidays">List of bank holidays excluding weekends</param>
        /// <returns>Number of business days during the 'span'</returns>
        public int BusinessDaysUntil(DateTime firstDay, DateTime lastDay)
        {
            HashSet<DateTime> bankHolidays = GetHolidays(year);
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            //if (firstDay > lastDay)
            //    throw new ArgumentException("Incorrect last day " + lastDay);

            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = (int)firstDay.DayOfWeek;
                int lastDayOfWeek = (int)lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            foreach (DateTime bankHoliday in bankHolidays)
            {
                DateTime bh = bankHoliday.Date;
                if (firstDay <= bh && bh <= lastDay)
                    --businessDays;
            }

            return businessDays;
        }

        private  HashSet<DateTime> GetHolidays(int year)
        {
            HashSet<DateTime> holidays = new HashSet<DateTime>();
            //NEW YEARS 
            DateTime newYearsDate = AdjustForWeekendHoliday(new DateTime(year, 1, 1).Date);
            holidays.Add(newYearsDate);

            //FAMILY DAY -- third monday in february
            DateTime familyDay = new DateTime(year, 2, 28);
            DayOfWeek dayOfWeek = familyDay.DayOfWeek;
            while (dayOfWeek != DayOfWeek.Monday)
            {
                familyDay = familyDay.AddDays(1);
                dayOfWeek = familyDay.DayOfWeek;
            }
            holidays.Add(familyDay.Date);

            //GOOD FRIDAY -- friday before easter 
            DateTime goodFriday = EasterSunday(year);
            dayOfWeek = goodFriday.DayOfWeek;
            while (dayOfWeek != DayOfWeek.Monday)
            {
                goodFriday = goodFriday.AddDays(1);
                dayOfWeek = goodFriday.DayOfWeek;
            }
            holidays.Add(goodFriday.Date);

            //INDEPENCENCE DAY 
            DateTime independenceDay = AdjustForWeekendHoliday(new DateTime(year, 7, 1).Date);
            holidays.Add(independenceDay);

            //LABOR DAY -- 1st Monday in September 
            DateTime laborDay = new DateTime(year, 9, 1);
            dayOfWeek = laborDay.DayOfWeek;
            while (dayOfWeek != DayOfWeek.Monday)
            {
                laborDay = laborDay.AddDays(1);
                dayOfWeek = laborDay.DayOfWeek;
            }
            holidays.Add(laborDay.Date);

            //THANKSGIVING DAY - 2nd in october
            var thanksgiving = (from day in Enumerable.Range(1, 30)
                                where new DateTime(year, 11, day).DayOfWeek == DayOfWeek.Monday
                                select day).ElementAt(1);
            DateTime thanksgivingDay = new DateTime(year, 11, thanksgiving);
            holidays.Add(thanksgivingDay.Date);

            //REMEMBERANCE DAY 
            DateTime rememberanceDay = AdjustForWeekendHoliday(new DateTime(year, 11, 11).Date);
            holidays.Add(rememberanceDay);

            //christmas
            DateTime christmasDay = AdjustForWeekendHoliday(new DateTime(year, 12, 25).Date);
            holidays.Add(christmasDay);
            return holidays;
        }

        public  DateTime AdjustForWeekendHoliday(DateTime holiday)
        {
            if (holiday.DayOfWeek == DayOfWeek.Saturday)
            {
                return holiday.AddDays(-1);
            }
            else if (holiday.DayOfWeek == DayOfWeek.Sunday)
            {
                return holiday.AddDays(1);
            }
            else
            {
                return holiday;
            }
        }


        public  DateTime EasterSunday(int year)
        {
            int day = 0;
            int month = 0;

            int g = year % 19;
            int c = year / 100;
            int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
            int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

            day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
            month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return new DateTime(year, month, day - 2);
        }

        public int workdaysToEndMonth(DateTime startDate)
        {
                var lastDay = DateTime.DaysInMonth(year, startDate.Month);
                DateTime endDate = new DateTime(year, startDate.Month, lastDay);

            return BusinessDaysUntil(startDate, endDate);
        }

    }
}