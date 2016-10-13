using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Models
{
    public class Admin
    {
        public int YEAR { get; set; }
        public static ArrayServices arrayServices { get; set; }
        public Admin(int year)
        {
            if(year != 0)
            {
                YEAR = year;
            }
            else
            {
                YEAR = DateTime.Now.Year;
            }

            arrayServices = new ArrayServices();
        }


    }
}