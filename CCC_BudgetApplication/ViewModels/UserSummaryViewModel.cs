using Application.Controllers;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class UserSummaryViewModel
    {
        public List<DataTable> Tables { get; set; }
        public UserBuiltSummary Summary { get; set; }

        public UserSummaryViewModel()
        {
            Summary = null;
            Tables = null;
        }

        public UserSummaryViewModel(UserBuiltSummary Summary, List<DataTable> Tables)
        {
            this.Summary = Summary;
            this.Tables = Tables;
        }
        public UserSummaryViewModel(UserBuiltSummary Summary, DataTable Table)
        {
            List<DataTable> Tables = new List<DataTable>();
            Tables.Add(Table);
               
            this.Summary = Summary;
            this.Tables = Tables;
        }

        public UserSummaryViewModel(string Name, List<DataTable> Tables)
        {
            UserBuiltSummary Summary = new UserBuiltSummary();

            Summary.Name = Name;

            this.Summary = Summary;
            this.Tables = Tables;
        }

        public UserSummaryViewModel(string Name, DataTable Table)
        {
            UserBuiltSummary Summary = new UserBuiltSummary();
            Summary.Name = Name;
            List<DataTable> Tables = new List<DataTable>();
            Tables.Add(Table);

            this.Summary = Summary;
            this.Tables = Tables;
        }

    }
}