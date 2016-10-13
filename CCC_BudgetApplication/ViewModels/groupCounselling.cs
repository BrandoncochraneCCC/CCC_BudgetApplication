using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class groupCounselling
    {
        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal numPrograms { get; set; }

        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal clientsPerProgram { get; set; }

        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal totalClients { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal feePerClient { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        public decimal totalFee { get; set; }

        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal revenueHours { get; set; }

        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal prepHours { get; set; }

        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal totalHours { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal revPerHour { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal revPerCounsellingHour { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal revPerPrepHour { get; set; }

        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal primaryHour { get; set; }

        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal coHours { get; set; }
        public string ViewClass { get; set; }
        public decimal programID { get; set; }
        public string name { get; set; }
    }
}