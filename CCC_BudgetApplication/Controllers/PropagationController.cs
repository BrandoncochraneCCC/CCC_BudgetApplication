using Application.Controllers.GeneralExpenses;
using Application.Controllers.RevenueTable;
using Application.Controllers.ServiceExpenses;
using Application.Controllers.Services;
using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{


    public class PropagationController : ObjectInstanceController
    {
        // GET: Propagation
        public DataLine PropagateDataLine(UserBuiltSummaryData data)
        {
            DataLine line = new DataLine();
            string table = data.Table.ToLower();
            try
            {
                switch (table)
                {
                    case "gagroup":
                        GeneralExpense g = new GeneralExpense(YEAR);
                        line = g.expenseDataLine(data.TableItemID);
                        break;
                    case "revenue":
                        RevenueTableController r = new RevenueTableController();
                        line = r.RevenueDataLine(data.TableItemID);
                        break;
                    case "serviceexpense":
                        ServiceExpenseSummaryController s = new ServiceExpenseSummaryController(YEAR);
                        line = s.ServiceExpenseDataLine(data.TableItemID);
                        break;
                    case "department":
                        EmployeesController d = new EmployeesController();
                        line = d.DepartmentCost(data.TableItemID);
                        break;
                    case "userbuiltsummary":
                        UserSummaryController c = new UserSummaryController();
                        line = c.userDataLine(data.TableItemID);
                        break;
                    case "employee":
                        line.Values = new decimal[12];
                        break;
                    case "salary":
                        line.Values = new decimal[12];
                        break;
                    case "capitalexpenditure":
                        line.Values = new decimal[12];

                        break;
                    default:
                        break;
                }

                line.Name = data.Name;
            }
            catch(Exception ex)
            {
                log.Warn("propagation failed", ex);
            }
            

            return line;
        }

        private DataLine UserBuiltDataLine(UserBuiltSummaryData data)
        {
            DataLine line = new DataLine();

            

            return line;
        }






    }   
}