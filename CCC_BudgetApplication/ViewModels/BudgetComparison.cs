using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class BudgetComparison
    {
        public List<Comparison> Revenues { get; set; }
        public List<Comparison> ServiceExpenses { get; set; }
        public List<Comparison> GeneralExpenses { get; set; }

        public int Year { get; set; }
        public Comparison RevenueTotal { get; set; }
        public Comparison ServiceExpenseTotal { get; set; }
        public Comparison GAExpenseTotal { get; set; }
        public Comparison ExpenseTotal { get; set; }
        public Comparison NetIncome { get; set; }
        public Comparison Amortization { get; set; }
        public Comparison Depreciation { get; set; }
        public Comparison NetWithAmortization { get; set; }
    }
}