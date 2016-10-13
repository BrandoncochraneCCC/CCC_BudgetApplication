using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class ExistingHardwareViewModel
    {
        public IQueryable<ExistingHardware> Servers { get; set; }
        public IQueryable<ExistingHardware> PCs { get; set; }
        public IQueryable<ExistingHardware> ThinClients { get; set; }
        public IQueryable<ExistingHardware> Other { get; set; }
        public decimal amortizationTotal { get; set; }
    }
}