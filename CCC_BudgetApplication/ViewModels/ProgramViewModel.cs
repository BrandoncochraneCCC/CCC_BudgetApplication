using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class ProgramViewModel
    {
        public string Name { get; set; }
        public int SourceID { get; set; }
        public DataTable Hours { get; set; }
        public DataTable Groups { get; set; }
        public DataTable FacilitatorHours { get; set; }

        public decimal NumberFacilitators { get; set; }
        public decimal NumberOfPrograms { get; set; }
    }
}