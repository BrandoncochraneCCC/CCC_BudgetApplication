using Application.ViewModels;
using System.Collections.Generic;

namespace Application.Controllers.Services
{
    public class DataTableServices
    {
        private ArrayServices arrayServices = new ArrayServices();

        public decimal[] sumTable(DataTable table)
        {
            decimal[] values = new decimal[12];

            foreach (var item in table.dataList)
            {
                for (var i = 0; i < 12; i++)
                {
                    values[i] += item.Values[i];
                }

            }
            return values;

        }

        public decimal[] sumTable(DataTable one, DataTable two)
        {
            decimal[] values = new decimal[12];

            values = sumTable(one);
            values = arrayServices.combineArrays(values, sumTable(two));

            return values;

        }

        public List<DataLine> combineTables(DataTable one, DataTable two, DataTable three, DataTable four)
        {
            List<DataLine> list = new List<DataLine>();
            list = combineTables(one, two, three);
            return combineList(list, four.dataList);
        }
        public List<DataLine> combineTables(DataTable one, DataTable two, DataTable three)
        {
            List<DataLine> list = new List<DataLine>();
            list = combineTables(one, two);
            return combineList(list, three.dataList);
        }
        public List<DataLine> combineTables(DataTable one, DataTable two)
        {
            return combineList(one.dataList, two.dataList);
        }


        public List<DataLine> combineTablesWithEmpty(DataTable one, DataTable two, DataTable three, DataTable four)
        {
            List<DataLine> list = new List<DataLine>();
            list = combineTablesWithEmpty(one, two, three);
            return combineListWithEmpty(list, four.dataList);
        }
        public List<DataLine> combineTablesWithEmpty(DataTable one, DataTable two, DataTable three)
        {
            List<DataLine> list = new List<DataLine>();
            list = combineTablesWithEmpty(one, two);
            return combineListWithEmpty(list, three.dataList);
        }
        public List<DataLine> combineTablesWithEmpty(DataTable one, DataTable two)
        {
            return combineListWithEmpty(one.dataList, two.dataList);
        }

        public List<DataLine> combineListWithEmpty(List<DataLine> one, List<DataLine> two)
        {
            List<DataLine> result = new List<DataLine>();
            
            foreach (var item in one)
            {
                result.Add(item);
            }
            result.Add(createEmptyLine());
            foreach (var item in two)
            {
                result.Add(item);
            }

            return result;
        }
        public List<DataLine> combineList(List<DataLine> one, List<DataLine> two)
        {
            List<DataLine> result = new List<DataLine>();

            foreach (var item in one)
            {
                result.Add(item);
            }
            foreach (var item in two)
            {
                result.Add(item);
            }

            return result;
        }

        public DataLine createEmptyLine()
        {
            DataLine empty = new DataLine();
            empty.viewClass = "empty";
            empty.Name = "";
            empty.Values = new decimal[12];
            return empty;
        }

    }
}