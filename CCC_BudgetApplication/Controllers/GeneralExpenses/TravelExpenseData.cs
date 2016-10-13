using Application.Controllers.Queries;
using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Controllers.GeneralExpenses
{
    public class TravelExpenseData
    {
        private int year;
        private GeneralExpenseQueries queries;
        private ArrayServices arrayServices;

        public TravelExpenseData(int year)
        {
            this.year = year;
            queries = new GeneralExpenseQueries(year);
            arrayServices = new ArrayServices();
        }
        public List<DataTable> travelExpenseTables(int expenseID)
        {
            List<DataTable> tables = new List<DataTable>();

            var conferences = queries.getConferences();
            foreach (var item in conferences)
            {
                tables.Add(conferenceTable(item));
            }

            return tables;
        }

        private DataTable conferenceTable(Conference item)
        {
            DataTable table = new DataTable();

            table.sourceID = item.ConferenceID;
            table.tableName = item.ConferenceName;
            table.Year = item.Year;
            table.dataList = conferenceDataLine(item);
            var attendee = "N/A";
            if(item.AttendeeName != null)
            {
                attendee = item.AttendeeName;
            }
            table.emptyCellValue = attendee;

            return table;
        }

        private List<DataLine> conferenceDataLine(Conference c)
        {
            List<DataLine> list = new List<DataLine>();
            list.Add(conferenceLine("Airfare", airExpenseData(c.ConferenceID)));
            list.Add(conferenceLine("Gas Mileage", gasExpenseData(c.ConferenceID)));
            list.Add(conferenceLine("Hotel", hotelExpenseData(c.ConferenceID)));
            list.Add(conferenceLine("Meals and Entertainment", mealExpenseData(c.ConferenceID)));
            list.Add(conferenceLine("Miscellaneous", miscExpenseData(c.ConferenceID)));

            return list;
        }

        private DataLine conferenceLine(string name, decimal[] values)
        {
            DataLine line = new DataLine();
            line.Name = name;
            line.Values = values;
            return line;
        }

        private DataLine ConferenceAttendee(string name)
        {
            DataLine line = new DataLine();
            line.Name = name;
            return line;
        }

        public decimal[] conferenceData()
        {
            decimal[] values = new decimal[12];
            values = arrayServices.combineArrays(values, airExpenseData());
            values = arrayServices.combineArrays(values, gasExpenseData());
            values = arrayServices.combineArrays(values, hotelExpenseData());
            values = arrayServices.combineArrays(values, mealExpenseData());
            values = arrayServices.combineArrays(values, miscExpenseData());


            return values;
        }

        private decimal[] airExpenseData(int ID = 0)
        {        
            decimal[] values = new decimal[12];
            var query = queries.getAir(ID);
            if (query != null)
            {
                for (var i = 0; i < 12; i++)
                {
                    values[i] = queries.getMonthlyAirExpense(query, i + 1);
                }
            }

            return values;
        }
        private decimal[] hotelExpenseData(int ID = 0)
        {
            decimal[] values = new decimal[12];
            var query = queries.getHotel(ID);
            if (query != null)
            {
                for (var i = 0; i < 12; i++)
                {
                    values[i] = queries.getMonthlyHotelExpense(query, i + 1);
                }
            }

            return values;
        }
        private decimal[] gasExpenseData(int ID = 0)
        {
            decimal[] values = new decimal[12];
            var query = queries.getGas(ID);
            if (query != null)
            {
                for (var i = 0; i < 12; i++)
                {
                    values[i] = queries.getMonthlyGasExpense(query, i + 1);
                }
            }

            return values;
        }
        private decimal[] mealExpenseData(int ID = 0)
        {
            decimal[] values = new decimal[12];
            var query = queries.getMeal(ID);
            if (query != null)
            {
                for (var i = 0; i < 12; i++)
                {
                    values[i] = queries.getMonthlyMealExpense(query, i + 1);
                }
            }

            return values;
        }
        private decimal[] miscExpenseData(int ID = 0)
        {
            decimal[] values = new decimal[12];
            var query = queries.getMisc(ID);
            if (query != null)
            {
                for (var i = 0; i < 12; i++)
                {
                    values[i] = queries.getMonthlyMiscExpense(query, i + 1);
                }
            }

            return values;
        }

    }
}