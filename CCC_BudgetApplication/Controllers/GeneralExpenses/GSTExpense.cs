using Application.Controllers.Queries;
using Application.Controllers.ServiceExpenses;
using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Controllers.GeneralExpenses
{
    public class GSTExpense
    {
        private int year;
        GeneralExpenseQueries queries;
        ArrayServices arrayServices = new ArrayServices();

        public GSTExpense(int year)
        {
            this.year = year;
            queries = new GeneralExpenseQueries(year);
        }

        public List<DataTable> GSTExpenseTables(int expenseID = 0)
        {
            List<DataTable> list = new List<DataTable>();
            list.Add(GSTExpenseTable(expenseID));

            return list;
        }

        public DataTable GSTExpenseTable(int expenseID)
        {
            DataTable table = new DataTable();

            table.tableName = "GST Expense";
            table.sourceID = expenseID;
            table.dataList = GSTDataList(expenseID);
            return table;
        }

        private List<DataLine> GSTDataList(int expenseID)
        {
            List<DataLine> list = new List<DataLine>();
            var children = queries.getChildren(expenseID);
            foreach(var c in children)
            {
                list.Add(GSTDataLine(c));
            }

            return list;
        }

        private DataLine GSTDataLine(GAGroup group)
        {
            DataLine line = new ViewModels.DataLine();
            switch (group.GAGroupID)
            {
                case 69: line = GSTonGA(); break;
                case 70: line = GSTonService(); break;
                default: line = GSTChildDataLine(group); break;
            }
            return line;
        }

        private DataLine GSTonGA()
        {
            DataLine line = new DataLine();
            line.Name = "GST on G&A Expenses";
            line.Values = GSTonGAData();
            return line;
        }

        private DataLine GSTChildDataLine(GAGroup group)
        {
            DataLine line = new DataLine();
            GeneralExpense controller = new GeneralExpense(year);
            line.Name = group.Name;
            line.Values = controller.expenseData(group);

            return line;
        }

        private DataLine GSTonService()
        {
            DataLine line = new ViewModels.DataLine();
            line.Name = "GST on Service Expenses";
            line.Values = GSTonServiceData();

            return line;
        }

        public decimal[] GSTExpenseValues()
        {
            decimal[] values = new decimal[12];

            values = GSTonGAData();
            values = arrayServices.combineArrays(values, GSTonServiceData());

            return values;
        }
        /**
         * GST Expense is calculated as follows:
         * 
         * Total of the accounts we pay GST on, times the 5% GST tax, 
         * times 50% as we get half back, and divided by 12 to reach a monthly estimate. 
         * 
         * G&A expenses - include all G&A except amortization, bad debt expense, 
         * interest and bank charges
         * 
         * Service expenses - include contract fees (less US contracts) and parking

         * */
        private decimal[] GSTonGAData(int yr = 0)
        {
            var gstRate = queries.getGSTRate(yr);
            gstRate /= 100;
            decimal[] gaExpenses = sumGAGST();
            decimal[] values = new decimal[12];
            for (var i = 0; i < 12; i++)
            {
                values[i] = serviceValue(gstRate, gaExpenses[i]);
            }
            return values;
        }
        private decimal[] GSTonServiceData(int yr = 0)
        {
            var gstRate = queries.getGSTRate(yr);
            gstRate /= 100;
            decimal[] serviceExpenses = sumServiceGST();
            decimal[] values = new decimal[12];
            for (var i = 0; i < 12; i++)
            {
                values[i] = serviceValue(gstRate, serviceExpenses[i]);
            }
            return values;
        }

        private decimal[] contractorGSTCosts()
        {
            decimal[] values = new decimal[12];


            return values;
        }

        private decimal[] sumServiceGST()
        {
            ServiceExpenseSummaryController controller = new ServiceExpenseSummaryController(year);
            var expenses = queries.getServiceGSTExpenses();
            decimal[] values = new decimal[12];
            foreach (var e in expenses)
            {
                decimal[] data = new decimal[12];
                if (hasChildren(e))
                {
                    var children = controller.getChildren(e);
                    data = controller.iterate(children);
                }
                else
                {
                    data = controller.expenseData(e);
                }

                values = arrayServices.combineArrays(values, data);
            }

            return values;
        }

        private bool hasChildren(ServiceExpense service)
        {
            if (queries.getChildren(service).FirstOrDefault() != null)
            {
                return true;
            }
            return false;
        }

        private bool hasChildren(GAGroup service)
        {
            if (queries.getChildren(service.GAGroupID).FirstOrDefault() != null)
            {
                return true;
            }
            return false;
        }

        private decimal[] sumGAGST()
        {
            var expenses = queries.getGAGSTExpenses();
            GeneralExpense controller = new GeneralExpense(year);

            decimal[] values = new decimal[12];
            foreach (var e in expenses)
            {
                decimal[] data = new decimal[12];
                if (hasChildren(e))
                {
                    var children = queries.getChildren(e.GAGroupID);
                    data = controller.iterate(children);
                }
                else
                {
                    data = controller.expenseData(e);
                }

                values = arrayServices.combineArrays(values, data);
            }


            return values;
        }


        private decimal serviceValue(decimal rate, decimal value)
        {
            return (value * rate) / 2;
        }

    }
}