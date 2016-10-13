using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Controllers.Queries
{
    public class UserSummaryQueries
    {
        BudgetDataEntities db = new ObjectInstanceController().db;
        private int year;
        public UserSummaryQueries(int year)
        {
            this.year = year;
        }


    }
}