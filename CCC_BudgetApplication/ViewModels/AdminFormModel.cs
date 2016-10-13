using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.ViewModels
{
    public class AdminFormModel
    {
        public string Id { get; set; }
        public IEnumerable<SelectListItem> List1 { get; set; }
        public IEnumerable<SelectListItem> List2 { get; set; }
        public int numTextBox { get; set; }

        public string Message { get; set; }
    }
}