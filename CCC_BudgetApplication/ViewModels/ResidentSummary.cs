using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class ResidentSummary
    {
        public List<Resident> residents { get; set; }
        public DataTable totalTable { get; set; }
        public DataTable bursaryTable { get; set; }

        internal object ToPagedList(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}