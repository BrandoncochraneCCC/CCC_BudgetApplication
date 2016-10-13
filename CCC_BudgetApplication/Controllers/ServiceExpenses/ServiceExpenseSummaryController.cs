using Application.Controllers.Queries;
using Application.Controllers.Services;
using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers.ServiceExpenses
{
    public class ServiceExpenseSummaryController : ObjectInstanceController
    {
        private int BURSARYID = 9;

        private int year;
        private ServiceExpenseQueries queries;
        private ArrayServices arrayServices;
        private DataTableServices tableServices = new DataTableServices();
        public ServiceExpenseSummaryController(int year)
        {
            this.year = year;
            queries = new ServiceExpenseQueries(year);
            arrayServices = new ArrayServices();
        }

        // GET: ServiceExpenseSummary
        public DataTable ExpenseTable(int ID = 0)
        {
            DepartmentServices dServices = new DepartmentServices(year);
            DataTable table = new DataTable();


            try
            {
                List<DataLine> list = new List<DataLine>();
                if (ID == 0)
                {
                    table.tableName = "Service Expenses";
                    list = dServices.buildServiceExpense();
                }
                else
                {
                    var expense = queries.getExpense(ID);
                    table.tableName = expense.Name;
                }
                table.sourceID = ID;
                table.Year = year;
                table.dataList = combineList(list, expenseDataList(ID));

            }
            catch (Exception ex)
            {
                log.Warn("Service expense table failed", ex);
            }

            return table;


        }

        public decimal salariesAndBenefits()
        {
            DepartmentServices dServices = new DepartmentServices(year);
            var list = dServices.buildServiceExpense();
            decimal sum = 0;
            foreach (var item in list)
            {
                foreach (var d in item.Values)
                {
                    sum += d;
                }
            }
            return sum;
        }

        public decimal ContractServicesData()
        {
            DataTable consulting = ExpenseTable(10);

            var sum = tableServices.sumTable(consulting);
            return arrayServices.sumArray(sum);
        }
        public DataTable ContractServices()
        {

            DataTable consulting = ExpenseTable(10);
            DataTable consultingFees = TotalConsultingFees(consulting);
            DataTable counsellor = ExpenseTable(2);
            DataTable total = ExpenseContractTotalsTable(consultingFees, counsellor);

            DataTable display = combineTables(consulting, consultingFees, counsellor, total);

            return display;
        }


        public DataTable TotalConsultingFees(DataTable fees)
        {
            DataTable table = new DataTable();
            table.dataList = totalFeeDataList(fees);
            return table;
        }

        public DataTable combineTables(DataTable one, DataTable two, DataTable three, DataTable four)
        {
            DataTable table = new DataTable();
            table.tableName = "Contract Services";
            table.dataList = tableServices.combineTablesWithEmpty(one, two, three, four);
            return table;
        }

        public DataTable ExpenseContractTotalsTable(DataTable fees, DataTable contractCounsellor)
        {
            DataTable table = new DataTable();
            table.dataList = contractTotalDataList(fees, contractCounsellor);

            return table;
        }

        private List<DataLine> totalFeeDataList(DataTable data)
        {
            List<DataLine> result = new List<DataLine>();
            result.Add(createDataLine("Total Consulting Fees", tableServices.sumTable(data)));

            return result;
        }

        private List<DataLine> contractCounsellorDataList(DataTable data)
        {
            EmployeeServices controller = new EmployeeServices(year);
            List<DataLine> result = new List<DataLine>();
            decimal[] total = tableServices.sumTable(data);
            decimal[] cpp = controller.deductionValues(total, EmployeeServices.CPP_DEDUCTIONTYPEID);
            decimal[] ei = controller.deductionValues(total, EmployeeServices.NO_STD_DEDUCTIONTYPEID);
            result.Add(createDataLine("Total CPP, for Contract Counsellors", cpp));
            result.Add(createDataLine("Total EI, for Contract Counsellors", ei));
            result.Add(createDataLine("Total EI and CPP, for Contract Counsellors", arrayServices.combineArrays(ei, cpp)));

            return result;
        }

        private List<DataLine> contractTotalDataList(DataTable fees, DataTable contractCounsellor)
        {
            List<DataLine> result = new List<DataLine>();
            DataLine grandTotal = createDataLine("Grand Total (Excluding CPP & EI)", tableServices.sumTable(fees, contractCounsellor), "highlight");
            List<DataLine> deduction = contractCounsellorDataList(contractCounsellor);
            DataLine totalCost = createDataLine("Total Cost - Contract Employees Including CPP and EI", arrayServices.combineArrays(grandTotal.Values, deduction.Last().Values), "highlight");

            result.Add(grandTotal);
            result = combineList(result, deduction);
            result.Add(totalCost);
            return result;
        }

        private List<DataLine> contractCounsellorDeductionDataList()
        {
            List<DataLine> result = new List<DataLine>();

            return result;
        }

        private List<DataLine> combineList(List<DataLine> one, List<DataLine> two)
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

        private List<DataLine> expenseDataList(int expenseID)
        {
            IQueryable<ServiceExpense> expenses;
            List<DataLine> list = new List<DataLine>();
            if (expenseID != 0)
            {
                expenses = queries.getChildren(expenseID);
            }
            else
            {
                expenses = queries.getExpenses(expenseID);
            }
            if (expenses.Count() != 0)
            {
                foreach (var e in expenses)
                {
                    list.Add(ServiceExpenseDataLine(e));
                }

            }
            else
            {
                list.Add(expenseDataLine(queries.getExpense(expenseID)));
            }


            return list;
        }

        public DataLine ServiceExpenseDataLine(ServiceExpense e)
        {
            DataLine line = new DataLine();
            try
            {
                switch (e.ServiceExpenseID)
                {
                    case 2: line = contractCounsellorDataLine(e); break;
                    case 3: break;
                    case 4: break;
                    case 5: break;
                    case 6: break;
                    case 9: line = getBursaryDataLine(BURSARYID); break;
                    default: line = expenseDataLine(e); break;
                }
            }
            catch (Exception ex)
            {
                log.Info("service expense switch error", ex);
            }

            return line;
        }

        public DataLine ServiceExpenseDataLine(int id)
        {
            return ServiceExpenseDataLine(db.ServiceExpenses.Find(id));
        }

        private decimal[] getContractServicesCost(int ID)
        {
            var e = queries.getExpense(ID);
            return contractCounsellorDataLine(e).Values;
        }

        private DataLine contractCounsellorDataLine(ServiceExpense e)
        {
            DataLine line = expenseDataLine(e);
            EmployeeServices controller = new EmployeeServices(year);
            decimal[] cpp = controller.deductionValues(line.Values, EmployeeServices.CPP_DEDUCTIONTYPEID);
            decimal[] ei = controller.deductionValues(line.Values, EmployeeServices.EI_DEDUCTIONTYPEID);
            line.Values = arrayServices.combineArrays(line.Values, cpp, ei);

            return line;
        }

        public DataLine getBursaryDataLine(int id)
        {
            DataLine line = new DataLine();
            var bursaries = queries.getAllBursaries();
            line.Action = "Index";
            line.Controller = "Bursaries";
            line.Name = "Bursaries";
            line.hasChildren = true;
            line.Values = bursaryValues(bursaries);
            return line;
        }

        private decimal[] bursaryValues(IQueryable<Bursary> data)
        {
            decimal[] values = new decimal[12];
            for (var i = 0; i < 12; i++)
            {
                values[i] += queries.totalMonthlyBursaries(data, i + 1);
            }

            return values;
        }
        public decimal[] EmployeeSalaries()
        {
            DepartmentServices dept = new DepartmentServices(year);
            var departments = dept.GetDepartments();
            decimal[] salary = new decimal[12];
            try
            {
                foreach (var d in departments)
                {
                    var summary = dept.CreateDepartmentSummary(d);
                    salary = arrayServices.combineArrays(salary, summary[0].Values);
                }

            }catch(Exception ex)
            {
                log.Info("department salary failure", ex);
            }

            return salary;
        }

        //public decimal[] EmployeeBenefits()
        //{
        //    DepartmentServices dept = new DepartmentServices(year);
        //    var departments = dept.GetDepartments();
        //    decimal[] benefits = new decimal[12];

        //    foreach (var d in departments)
        //    {
        //        var summary = dept.CreateDepartmentSummary(d);
        //        benefits = arrayServices.combineArrays(benefits, summary[5].Values);
        //    }
        //    return benefits;
        //}

        public decimal[] EmployeeRemittance()
        {
            DepartmentServices dept = new DepartmentServices(year);
            var departments = dept.GetDepartments();
            decimal[] cpp = new decimal[12];
            decimal[] ei = new decimal[12];
            try
            {
                foreach (var d in departments)
                {
                    var summary = dept.CreateDepartmentSummary(d);
                    cpp = arrayServices.combineArrays(cpp, summary[1].Values);
                    ei = arrayServices.combineArrays(ei, summary[2].Values);
                }
            }
            catch (Exception ex)
            {
                log.Info("employee remittance failure", ex);
            }

            return arrayServices.combineArrays(cpp, ei);
        }

        public DataLine expenseDataLine(int id)
        {
            return expenseDataLine(queries.getExpense(id));
        }
        private DataLine expenseDataLine(ServiceExpense expense)
        {
            DataLine line = new DataLine();
            try
            {
                if (expense != null)
                {
                    line.Action = "ServiceExpense";
                    line.Controller = "ServiceExpense";

                    if (hasChildren(expense))
                    {
                        var children = queries.getChildren(expense.ServiceExpenseID);
                        line.hasChildren = true;
                        line.Values = arrayServices.combineArrays(line.Values, iterate(children));
                        line.SourceID = expense.ServiceExpenseID;

                    }
                    else
                    {
                        line.Values = arrayServices.combineArrays(line.Values, expenseData(expense));
                    }

                    line.year = year;
                    line.Name = expense.Name;
                    if (isContractor(expense))
                    {
                        line.isContractor = true;
                        line.usedInGSTCalculation = usedInGST(expense);
                    }
                    if (expense.ParentID != null)
                    {
                        line.ParentID = (int)expense.ParentID;
                    }
                    line.SourceID = expense.ServiceExpenseID;
                }
            }
            catch(Exception ex)
            {
                log.Warn("expense data line failure", ex);
            }
            
             return line;
        }

        public IQueryable<ServiceExpense> getChildren(ServiceExpense expense)
        {
            return queries.getChildren(expense.ServiceExpenseID);
        }

        public decimal[] iterate(IQueryable<ServiceExpense> expenses)
        {
            decimal[] values = new decimal[12];

            try
            {
                foreach (var child in expenses)
                {
                    var gChildren = queries.getChildren(child.ServiceExpenseID);
                    foreach (var g in gChildren)
                    {
                        values = arrayServices.combineArrays(values, iterate(queries.getChildren(g.ServiceExpenseID)));
                        expenseData(g, values);

                    }
                    expenseData(child, values);
                }
            }
            catch(Exception ex)
            {
                log.Warn("calculating service expense children failure", ex);
            }
            
            return values;
        }

        public decimal[] expenseData(ServiceExpense expense)
        {
            decimal[] values = new decimal[12];
            expenseData(expense, values);
            return values;
        }

        private void expenseData(ServiceExpense expense, decimal[] values)
        {
            if (expense != null)
            {
                var data = queries.getChildData(expense.ServiceExpenseID);
                for (var i = 0; i < 12; i++)
                {
                    var result = queries.getMonthlyExpenseData(data, i + 1);
                    if (result != null)
                    {
                        values[i] += result.Value;

                    }
                }
            }

        }

        private bool hasData(ServiceExpense expense)
        {
            bool hasData = false;

            if (expense.ServiceExpenseDatas != null)
            {
                hasData = true;
            }

            return hasData;
        }

        private bool hasChildren(ServiceExpense expense)
        {
            bool result = false;
            if (expense != null && queries.getChildren(expense.ServiceExpenseID).Count() != 0)
            {
                result = true;
            }
            return result;
        }

        private DataLine createDataLine(string name, decimal[] values, string viewClass = "")
        {
            DataLine line = new DataLine();
            line.Name = name;
            line.Values = values;
            line.viewClass = viewClass;
            return line;
        }

        private bool isContractor(ServiceExpense expense)
        {
            bool result = false;
            if (expense.ParentID == 1002 || expense.ParentID == 1003 || expense.ParentID == 1004)
            {
                result = true;
            }

            return result;
        }

        private bool usedInGST(ServiceExpense expense)
        {
            bool result = false;
            if (queries.getGSTRejection(expense.ServiceExpenseID) != null)
            {
                result = true;
            }

            return result;
        }

    }
}