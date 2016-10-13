using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class DataTable
    {
        public string tableName { get; set; }
        public List<DataLine> dataList { get; set; }
        public int Year { get; set; }
        public int sourceID { get; set; }
        public string emptyCellValue { get; set; }
    }
}