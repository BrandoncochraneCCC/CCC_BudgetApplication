using Application.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class ITSummaryViewModel
    {

        public ExistingHardwareViewModel ExistingHardware { get; set; }
        public List<DataTable> Hardware { get; set; }
        public List<DataTable> Software { get; set; }
        public List<DataTable> ITCommunications { get; set; }

        public ITSummaryViewModel()
        {
            GeneralExpensesController controller = new GeneralExpensesController();           
            ExistingHardware = controller.ExistingHardwareViewModel();
            Hardware = controller.GeneralExpenseViewModel(16);
            Software = controller.GeneralExpenseViewModel(17);
            ITCommunications = controller.GeneralExpenseViewModel(18);

        }
    }
}