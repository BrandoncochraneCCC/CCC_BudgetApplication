using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class AmortizationViewModel
    {
        public string ViewClass { get; set; }
        public int SourceID { get; set; }
        public string Name { get; set; }
        public int AccountNum { get; set; }
        public decimal PoolBalance { get; set; }
        public bool StraightLine { get; set; }
        public decimal AmortizationBalance { get; set; }
        public decimal AdditionValue { get; set; }
        public IQueryable<Addition> AdditionList { get; set; }
        public decimal DisposalValue { get; set; }
        public IQueryable<Disposal> DisposalList { get; set; }
        public decimal Rate { get; set; }
        public decimal CurrentAfterRate { get; set; }
        public decimal  AdditionAfterRate { get; set; }
        public decimal Provision { get; set; }
        public decimal[] Values { get; set; }
        public int Year { get; set; }
    }
}