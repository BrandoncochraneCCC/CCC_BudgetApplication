using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class CurrentYearData
    {
        public decimal CppRate { get; set; }
        public decimal CppMax { get; set; }
        public decimal EIRate { get; set; }
        public decimal EIMax { get; set; }
        public decimal RRSPRate { get; set; }
        public decimal RRSPMax { get; set; }
        public decimal FamilyBenefit { get; set; }
        public decimal IndividualBenefit { get; set; }
        public decimal Vacation { get; set; }
        public decimal STD { get; set; }
        public decimal HoursPerYear { get; set; }
        public decimal EmployeeTargetHours{ get; set; }
        public decimal GroupExpense { get; set; }
        public decimal GSTRate { get; set; }
        public decimal AverageCostFT { get; set; }
        public decimal AveragteCostPT { get; set; }
        public decimal AverageCostResident { get; set; }
        public decimal AverageCostStudent { get; set; }
        public decimal AverageCostCounselling { get; set; }
        public decimal AverageCostSupervision { get; set; }
        public decimal ResidentTargetHours { get; set; }
        public decimal TotalBursaryValue { get; set; }
        public decimal Bursary { get; set; }
        public decimal Clawback { get; set; }



        public CurrentYearData()
        {
            CppRate = 0;
            CppMax = 0;
            EIRate = 0;
            EIMax = 0;
            RRSPRate = 0;
            RRSPMax = 0;
            FamilyBenefit = 0;
            IndividualBenefit = 0;
            Vacation = 0;
            HoursPerYear = 0;
            EmployeeTargetHours = 0;
            GroupExpense = 0;
            GSTRate = 0;
            AverageCostFT = 0;
            AveragteCostPT = 0;
            AverageCostResident = 0;
            AverageCostStudent = 0;
            AverageCostCounselling = 0;
            AverageCostSupervision = 0;
            ResidentTargetHours = 0;
            TotalBursaryValue = 0;
            Bursary = 0;
            Clawback = 0;
        }

    }
}