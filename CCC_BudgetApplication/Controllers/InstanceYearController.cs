/**
* Author - BRANDON COCHRANE
* Organization: Calgary Counselling Centre
* 
* Contains static running instance year of application
* */


using System;

namespace Application.Controllers
{
    public static class InstanceYearController
    {
        public static int YEAR { get; set; }

        // GET: InstanceYear
        public static void SETYEAR(int year = 0)
        {
            if(year == 0)
            {
                year = DateTime.Now.Year;
            }
            YEAR = year;
           
        }

    }
}