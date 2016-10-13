using Application.Controllers.Queries;
using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Controllers.GeneralExpenses
{
    public class GeneralExpense
    {
        private int URBANLIGHTHOUSEID = 153;
        private int TRAVELID = 15;
        private int GSTEXPENSE = 9;
        private int SERVER = 2;
        private int PC = 3;
        private int THINCLIENT = 4;
        private int OTHERHARDWARE = 5;
        private static readonly log4net.ILog log = LogHelper.GetLogger();

        private int year;
        GeneralExpenseQueries queries;
        BudgetDataEntities db = new ObjectInstanceController().db;
        ArrayServices arrayServices = new ArrayServices();
        TravelExpenseData travel;
        public GeneralExpense(int year)
        {
            this.year = year;
            queries = new GeneralExpenseQueries(year);
            travel = new TravelExpenseData(year);
        }

        public List<DataTable> generalExpenseTables(int expenseID)
        {
            List<DataTable> tables = new List<DataTable>();
            tables.Add(generalExpenseTable(expenseID));

            return tables;
        }

        public List<DataTable> travelTable(int expenseID)
        {
            List<DataTable> tables = new List<DataTable>();

            tables = travel.travelExpenseTables(expenseID);
            tables.Add(generalExpenseTable(expenseID));

            return tables;
        }
        public List<DataTable> GSTExpenseTable(int expenseID)
        {
            GSTExpense gst = new GSTExpense(year);

            List<DataTable> tables = new List<DataTable>();
            tables = gst.GSTExpenseTables(expenseID);
            ///tables.Add(generalExpenseTable(expenseID));

            return tables;
        }
        public ExistingHardwareViewModel ExistingHardware()
        {
            ExistingHardwareViewModel item = new ExistingHardwareViewModel();
            item.Servers = queries.getExistingHardware(SERVER);
            item.PCs = queries.getExistingHardware(PC);
            item.ThinClients = queries.getExistingHardware(THINCLIENT);
            item.Other = queries.getExistingHardware(OTHERHARDWARE);
            item.amortizationTotal = amortizationTotal(item);

            return item;

        }

        


        public ExistingHardwareViewModel HardwareAdditions()
        {
            ExistingHardwareViewModel item = new ExistingHardwareViewModel();
            item.Servers = queries.getExistingHardware(SERVER);
            item.PCs = queries.getExistingHardware(PC);
            item.ThinClients = queries.getExistingHardware(THINCLIENT);
            item.Other = queries.getExistingHardware(OTHERHARDWARE);
            item.amortizationTotal = amortizationTotal(item);

            return item;

        }
        public decimal amortizationTotal(ExistingHardwareViewModel item)
        {
            decimal amortizationTotal = 0;

            foreach (var x in item.Servers)
            {
                if (x.Amortization != null)
                {
                    amortizationTotal += (decimal)x.Amortization;
                }
            }

            foreach (var x in item.PCs)
            {
                if (x.Amortization != null)
                {
                    amortizationTotal += (decimal)x.Amortization;
                }
            }


            foreach (var x in item.ThinClients)
            {
                if (x.Amortization != null)
                {
                    amortizationTotal += (decimal)x.Amortization;
                }
            }

            foreach (var x in item.Other)
            {
                if (x.Amortization != null)
                {
                    amortizationTotal += (decimal)x.Amortization;
                }
            }

            return amortizationTotal;
        }

        public DataTable generalExpenseTable(int expenseID = 0)
        {
            DataTable table = new DataTable();
            if (expenseID == 0)
            {
                table.tableName = "General Expenses";
            }
            else
            {
                var expense = queries.getGeneralExpense(expenseID);
                table.tableName = expense.Name;
                if (expenseID == 16)
                {
                    table.tableName += " Additions";

                }

            }
            table.sourceID = expenseID;
            table.Year = year;
            table.dataList = expenseDataList(expenseID);

            return table;
        }
        public decimal ITMaterials()
        {
            decimal value = arrayServices.sumArray(expenseDataLine(16).Values);
            value += arrayServices.sumArray(expenseDataLine(17).Values);
            value += arrayServices.sumArray(expenseDataLine(18).Values);
            return value;
        }

        private List<DataLine> expenseDataList(int expenseID)
        {

            List<DataLine> list = new List<DataLine>();
            try
            {
                var expenses = queries.getChildren(expenseID);
                if (expenses.Count() != 0)
                {
                    foreach (var item in expenses)
                    {
                        if (item.GAGroupID >= 6)
                        {
                            list.Add(expenseDataLine(item));
                        }
                    }

                }
                else
                {
                    list.Add(expenseDataLine(queries.getGeneralExpense(expenseID)));
                }
            }
            catch(Exception ex)
            {
                log.Error("general expense data list error", ex);
            }
           

            return list;
        }

        public DataLine expenseDataLine(int id)
        {
            if (id == 0)
            {
                return allGAExpenses();
            }
            else
            {
                return expenseDataLine(queries.getGeneralExpense(id));
            }
        }

        private DataLine allGAExpenses()
        {
            DataLine line = new DataLine();
            line.Values = sumAllExpenses();
            line.Name = "General Expenses " +  year;
            line.SourceID = 0;

            return line;

        }

        private decimal[] sumAllExpenses()
        {
            decimal[] values = new decimal[12];
            var data = db.GAExpenses.Where(x => x.Date.Year == year).Select(x => x);
            for (var i = 0; i < 12; i++)
            {
                values[i] = data.Where(x => x.Date.Month == i + 1).Select(x => x.Value).Sum();
            }

            return values;
        }

        private DataLine expenseDataLine(GAGroup expense)
        {
            DataLine line = new DataLine();
            try
            {
                if (expense != null)
                {
                    var children = queries.getChildren(expense.GAGroupID);
                    line.Action = "GeneralExpense";
                    line.Controller = "GeneralExpenses";
                    if (expense.GAGroupID == GSTEXPENSE)
                    {
                        line.Action = "GSTExpense";
                        line.Values = GSTExpense(expense);

                    }
                    else if (expense.GAGroupID == TRAVELID)
                    {
                        line.Values = travelData(expense);
                        line.Action = "TravelExpense";
                    }
                    if (children.FirstOrDefault() != null)
                    {
                        line.hasChildren = true;
                        line.Values = arrayServices.combineArrays(line.Values, iterate(children));
                    }
                    else
                    {

                        line.Values = arrayServices.combineArrays(line.Values, expenseData(expense));
                    }

                    line.SourceID = expense.GAGroupID;
                    line.year = year;
                    line.Name = expense.Name;
                    if (expense.ParentID != null)
                    {
                        line.ParentID = (int)expense.ParentID;
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error("general expense data line error", ex);
            }
            
            

            return line;
        }

        private decimal[] travelData(GAGroup expense)
        {
            decimal[] values = expenseData(expense);
            values = arrayServices.combineArrays(values, travel.conferenceData());

            return values;
        }

        private decimal[] GSTExpense(GAGroup expense)
        {
            decimal[] values = new decimal[12];
            GSTExpense controller = new GSTExpense(year);
            values = controller.GSTExpenseValues();
            return values;
        }

        public decimal[] iterate(IQueryable<GAGroup> groups)
        {
            decimal[] values = new decimal[12];
            try
            {
                foreach (var child in groups)
                {
                    if (child.GAGroupID != URBANLIGHTHOUSEID)
                    {
                        var gChildren = queries.getChildren(child.GAGroupID);
                        foreach (var g in gChildren)
                        {
                            if (g.GAGroupID != URBANLIGHTHOUSEID)
                            {
                                values = arrayServices.combineArrays(values, iterate(queries.getChildren(g.GAGroupID)));
                                expenseData(g, values);
                            }
                        }
                        expenseData(child, values);
                    }

                }
            }
            catch(Exception ex)
            {
                log.Warn("iterating general expense child failed", ex);
            }
            
            return values;
        }

        public decimal[] expenseData(GAGroup expense)
        {
            decimal[] values = new decimal[12];
            expenseData(expense, values);
            return values;
        }
        private void expenseData(GAGroup expense, decimal[] values)
        {
            try
            {
                var data = queries.getChildData(expense.GAGroupID);
                for (var i = 0; i < 12; i++)
                {
                    var result = queries.getMonthlyGeneralExpenseData(data, i + 1);
                    if (result != null)
                    {
                        values[i] += result.Value;

                    }
                }
            }catch(Exception ex)
            {
                log.Warn("general expense data failed", ex);
            }

        }

        private bool hasData(GAGroup expense)
        {
            bool hasData = false;

            if (queries.hasData(expense.GAGroupID))
            {
                hasData = true;
            }

            return hasData;
        }

        private bool hasChildren(GAGroup expense)
        {
            bool hasChild = false;

            if (queries.hasChildren(expense.GAGroupID))
            {
                hasChild = true;
            }

            return hasChild;
        }


    }
}