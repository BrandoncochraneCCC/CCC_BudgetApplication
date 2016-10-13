using Application.Controllers.GeneralExpenses;
using Application.Controllers.RevenueTable;
using Application.Controllers.ServiceExpenses;
using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class ProgramBudgetController : ObjectInstanceController
    {
        private ArrayServices arrayServices = new ArrayServices();
        public static int COUNSELLING_FEESID = 4;
        public static int UNITED_WAY_ALLOCATION = 1;
        public static int COUNSELLING = 5;
        public static int FCSS = 3;
        public static int FUNDRAISING = 7;

        public static int SALARY = 3;
        public static int EI = 4;
        public static int CPP = 5;
        public static int BENEFITS = 6;
        public static int BURSARY = 9;
        public static int CONSULTANT = 10;

        public static int RENT = 13;



        // GET: ProgramBudget
        public ActionResult ProgramBudget()
        {
            ResourceAllocation item = new ResourceAllocation();
            //list.Add(summaryRevenue());
            item.Header = "Summary";
            return View(item);
        }

        private List<ProgramBudgetViewModel> combineLists(List<ProgramBudgetViewModel> one, List<ProgramBudgetViewModel> two)
        {
            List<ProgramBudgetViewModel> list = new List<ProgramBudgetViewModel>();
            foreach (var item in one)
            {
                list.Add(item);
            }
            foreach (var item in two)
            {
                list.Add(item);
            }
            return list;
        }

        public ActionResult Counselling()
        {
            ResourceAllocation item = new ResourceAllocation();
            item.Percentage = db.UnchangingValues.Where(x => x.Name.ToLower() == "percent of resources allocated" && x.Year == YEAR).Select(x => x.Value).SingleOrDefault() / 100;

            List<ProgramBudgetViewModel> revenues = counsellingRevenueList(item.Percentage);
            List<ProgramBudgetViewModel> expenses = counsellingExpenseList(item.Percentage, true);

            item.Revenues = revenues;
            item.Expenses = expenses;
            item.Header = "Counselling";
            item.Total = revenues.LastOrDefault().value + expenses.LastOrDefault().value;

            return View("ProgramBudget", item);
        }

        public ActionResult Training()
        {
            ResourceAllocation item = new ResourceAllocation();
            item.Percentage = db.UnchangingValues.Where(x => x.Name.ToLower() == "percent of resources allocated" && x.Year == YEAR).Select(x => x.Value).SingleOrDefault() / 100;

            List<ProgramBudgetViewModel> revenues = counsellingRevenueList(item.Percentage);
            List<ProgramBudgetViewModel> expenses = counsellingExpenseList(item.Percentage, false);

            item.Revenues = revenues;
            item.Expenses = expenses;
            item.Header = "Training";

            item.Total = revenues.LastOrDefault().value + expenses.LastOrDefault().value;

            return View("ProgramBudget", item);
        }

        public ActionResult FamilyViolence()
        {
            ResourceAllocation item = new ResourceAllocation();
            item.Percentage = db.UnchangingValues.Where(x => x.Name.ToLower() == "percent of resources allocated" && x.Year == YEAR).Select(x => x.Value).SingleOrDefault() / 100;

            List<ProgramBudgetViewModel> revenues = familyViolenceRevenues(item.Percentage);
            List<ProgramBudgetViewModel> expenses = familyViolenceExpenses(item.Percentage);

            item.Revenues = revenues;
            item.Expenses = expenses;
            item.Header = "Family Violence";

            item.Total = revenues.LastOrDefault().value + expenses.LastOrDefault().value;

            return View("ProgramBudget", item);
        }

        public ActionResult TFTB()
        {
            ResourceAllocation item = new ResourceAllocation();
            item.Percentage = db.UnchangingValues.Where(x => x.Name.ToLower() == "resource allocation" && x.Year == YEAR).Select(x => x.Value).SingleOrDefault() / 100;
            var FVPercent = db.UnchangingValues.Where(x => x.Name.ToLower() == "percent of resources allocated" && x.Year == YEAR).Select(x => x.Value).SingleOrDefault() / 100;

            List<ProgramBudgetViewModel> revenues = ProgramRevenues(item.Percentage, FVPercent);
            List<ProgramBudgetViewModel> expenses = ProgramExpenses(item.Percentage, FVPercent);

            item.Revenues = revenues;
            item.Expenses = expenses;
            item.Header = "Turn For The Better";

            item.Total = revenues.LastOrDefault().value + expenses.LastOrDefault().value;

            return View("ProgramBudget", item);
        }
        public ActionResult RCMen()
        {
            ResourceAllocation item = new ResourceAllocation();
            item.Percentage = db.UnchangingValues.Where(x => x.Name.ToLower() == "resource allocation" && x.Year == YEAR).Select(x => x.Value).SingleOrDefault() / 100;
            var FVPercent = db.UnchangingValues.Where(x => x.Name.ToLower() == "percent of resources allocated" && x.Year == YEAR).Select(x => x.Value).SingleOrDefault() / 100;

            List<ProgramBudgetViewModel> revenues = ProgramRevenues(item.Percentage, FVPercent);
            List<ProgramBudgetViewModel> expenses = ProgramExpenses(item.Percentage, FVPercent);

            item.Revenues = revenues;
            item.Expenses = expenses;
            item.Header = "Responsible Choices for Men";

            item.Total = revenues.LastOrDefault().value + expenses.LastOrDefault().value;

            return View("ProgramBudget", item);
        }
        public ActionResult RCWomen()
        {
            ResourceAllocation item = new ResourceAllocation();
            item.Percentage = db.UnchangingValues.Where(x => x.Name.ToLower() == "resource allocation" && x.Year == YEAR).Select(x => x.Value).SingleOrDefault() / 100;
            var FVPercent = db.UnchangingValues.Where(x => x.Name.ToLower() == "percent of resources allocated" && x.Year == YEAR).Select(x => x.Value).SingleOrDefault() / 100;

            List<ProgramBudgetViewModel> revenues = ProgramRevenues(item.Percentage, FVPercent);
            List<ProgramBudgetViewModel> expenses = ProgramExpenses(item.Percentage, FVPercent);

            item.Revenues = revenues;
            item.Expenses = expenses;
            item.Header = "Responsible Choices for Women";

            item.Total = revenues.LastOrDefault().value + expenses.LastOrDefault().value;

            return View("ProgramBudget", item);
        }
        public ActionResult YNA()
        {
            ResourceAllocation item = new ResourceAllocation();
            item.Percentage = db.UnchangingValues.Where(x => x.Name.ToLower() == "resource allocation" && x.Year == YEAR).Select(x => x.Value).SingleOrDefault() / 100;
            var FVPercent = db.UnchangingValues.Where(x => x.Name.ToLower() == "percent of resources allocated" && x.Year == YEAR).Select(x => x.Value).SingleOrDefault() / 100;
            List<ProgramBudgetViewModel> revenues = ProgramRevenues(item.Percentage, FVPercent);
            List<ProgramBudgetViewModel> expenses = ProgramExpenses(item.Percentage, FVPercent);

            item.Revenues = revenues;
            item.Expenses = expenses;
            item.Header = "You're Not Alone";

            item.Total = revenues.LastOrDefault().value + expenses.LastOrDefault().value;

            return View("ProgramBudget", item);
        }
        private List<ProgramBudgetViewModel> ProgramRevenues(decimal percent, decimal FVPercent)
        {
            RevenueTableController c = new RevenueTableController();
            List<ProgramBudgetViewModel> list = new List<ProgramBudgetViewModel>();
            decimal total = 0;
            ProgramBudgetViewModel service = new ProgramBudgetViewModel("Fees for Service", (arrayServices.sumArray(c.getChildren(5).Values) * FVPercent) * percent, percent);
            ProgramBudgetViewModel CCCContribution = new ProgramBudgetViewModel("Calgary Counselling Centres Contribution", (arrayServices.sumArray(c.getChildren(6).Values) * FVPercent) * percent, percent);
            ProgramBudgetViewModel UnitedWay = new ProgramBudgetViewModel("United Way", (arrayServices.sumArray(c.getChildren(1).Values) * FVPercent) * percent, percent);
            ProgramBudgetViewModel FCSS = new ProgramBudgetViewModel("FCSS", (arrayServices.sumArray(c.getChildren(3).Values) * FVPercent) * percent, percent);
            ProgramBudgetViewModel RBC = new ProgramBudgetViewModel("RBC Foundation", 0, percent);//not done
            ProgramBudgetViewModel Nickle = new ProgramBudgetViewModel("Nickle Family Foundation", 0, percent); //not done
            ProgramBudgetViewModel Fundraising = new ProgramBudgetViewModel("Fundraising", UnitedWay.value * percent);

            list.Add(service);
            list.Add(CCCContribution);
            list.Add(UnitedWay);
            list.Add(FCSS);
            list.Add(RBC);
            list.Add(Nickle);
            list.Add(Fundraising);

            foreach (var i in list) { total += i.value; }
            ProgramBudgetViewModel totalLine = new ProgramBudgetViewModel("Total Revenues", total, "highlight");
            list.Add(totalLine);
            return list;
        }

        private List<ProgramBudgetViewModel> ProgramExpenses(decimal percent, decimal FVPercent)
        {
            ServiceExpenseSummaryController c = new ServiceExpenseSummaryController(YEAR);
            GeneralExpense g = new GeneralExpense(YEAR);

            List<ProgramBudgetViewModel> list = new List<ProgramBudgetViewModel>();
            decimal total = 0;
            ProgramBudgetViewModel Coordinator = new ProgramBudgetViewModel("Program Coordinator",      0, percent);//not done
            ProgramBudgetViewModel Compensation = new ProgramBudgetViewModel("Counsellor Compensation", (arrayServices.sumArray(c.EmployeeSalaries()) * FVPercent) * percent, percent);
            var materialValue = arrayServices.sumArray(g.expenseDataLine(184).Values);
            materialValue += arrayServices.sumArray(g.expenseDataLine(185).Values);
            ProgramBudgetViewModel Support = new ProgramBudgetViewModel("Program Support", (materialValue * FVPercent) * percent, percent);
            ProgramBudgetViewModel Tech = new ProgramBudgetViewModel("Software and Technology", (g.ITMaterials() * FVPercent) * percent, percent);
            ProgramBudgetViewModel Supplies = new ProgramBudgetViewModel("Program Supplies", (arrayServices.sumArray(g.expenseDataLine(25).Values) * FVPercent)  * percent, percent);
            ProgramBudgetViewModel Training = new ProgramBudgetViewModel("Recruitment and Training", 0, percent);//not implemented
            ProgramBudgetViewModel Promotion = new ProgramBudgetViewModel("Promotion and Publicity", (arrayServices.sumArray(c.expenseDataLine(8).Values) * FVPercent) * percent, percent);
            ProgramBudgetViewModel Facility = new ProgramBudgetViewModel("Facility", (arrayServices.sumArray(c.expenseDataLine(13).Values) * FVPercent) * percent, percent);
            ProgramBudgetViewModel Services = new ProgramBudgetViewModel("Purchased Services", (arrayServices.sumArray(c.expenseDataLine(10).Values) * FVPercent) * percent, percent);
            list.Add(Coordinator);
            list.Add(Compensation);
            list.Add(Support);
            list.Add(Tech);
            list.Add(Supplies);
            list.Add(Training);
            list.Add(Promotion);
            list.Add(Facility);
            list.Add(Services);
            foreach (var i in list) { total += i.value; }
            ProgramBudgetViewModel totalLine = new ProgramBudgetViewModel("Total Revenues", total, "highlight");
            list.Add(totalLine);
            return list;
        }
        private List<ProgramBudgetViewModel> familyViolenceRevenues(decimal percent)
        {
            RevenueTableController c = new RevenueTableController();
            List<ProgramBudgetViewModel> list = new List<ProgramBudgetViewModel>();
            decimal total = 0;
            ProgramBudgetViewModel UnitedWay = new ProgramBudgetViewModel("United Way", arrayServices.sumArray(c.getChildren(1).Values) * percent, percent);
            ProgramBudgetViewModel FCSS = new ProgramBudgetViewModel("FCSS", arrayServices.sumArray(c.getChildren(3).Values) * percent, percent);
            ProgramBudgetViewModel Donations = new ProgramBudgetViewModel("Donations", arrayServices.sumArray(c.getChildren(4).Values) * percent, percent);
            ProgramBudgetViewModel Fees = new ProgramBudgetViewModel("Fees for Service", arrayServices.sumArray(c.getChildren(5).Values) * percent, percent);
            ProgramBudgetViewModel CounsellingPrograms = new ProgramBudgetViewModel("Calgary Counselling Programs", arrayServices.sumArray(c.getChildren(6).Values) * percent, percent);
            ProgramBudgetViewModel Fundraising = new ProgramBudgetViewModel("Fundraising", arrayServices.sumArray(c.getChildren(7).Values) * percent, percent);
            ProgramBudgetViewModel Supervision = new ProgramBudgetViewModel("Supervision", arrayServices.sumArray(c.getChildren(9).Values) * percent, percent);
            ProgramBudgetViewModel Grants = new ProgramBudgetViewModel("Grants", arrayServices.sumArray(c.getChildren(10).Values) * percent, percent);
            ProgramBudgetViewModel Amortization = new ProgramBudgetViewModel("Amortization od Deferred Capital", arrayServices.sumArray(c.getChildren(13).Values) * percent, percent);//not implemented
            ProgramBudgetViewModel Other = new ProgramBudgetViewModel("Other (Training, workshops)", arrayServices.sumArray(c.getChildren(14).Values) * percent, percent);
            list.Add(Fees);
            list.Add(UnitedWay);
            list.Add(FCSS);
            list.Add(CounsellingPrograms);
            list.Add(Other);
            list.Add(Donations);
            list.Add(Supervision);
            list.Add(Grants);
            list.Add(Amortization);



            foreach (var i in list) { total += i.value; }
            ProgramBudgetViewModel totalLine = new ProgramBudgetViewModel("Total Revenues", total, "highlight");
            list.Add(totalLine);
            return list;
        }

        private List<ProgramBudgetViewModel> familyViolenceExpenses(decimal percent)
        {
            List<ProgramBudgetViewModel> list = new List<ProgramBudgetViewModel>();
            list = combineLists(list, PersonellExpense(percent));
            list = combineLists(list, TraveParkingExpense(percent));
            list = combineLists(list, MaterialsSuppliesExpense(percent));
            list = combineLists(list, OtherExpense(percent));

            return list;
        }



        private List<ProgramBudgetViewModel> counsellingRevenueList(decimal percent)
        {
            List<ProgramBudgetViewModel> list = new List<ProgramBudgetViewModel>();

            RevenueTableController c = new RevenueTableController();
            decimal total = 0;
            ProgramBudgetViewModel item = new ProgramBudgetViewModel("Counselling Fees for Service", arrayServices.sumArray(c.getChildren(COUNSELLING_FEESID).Values) * percent, percent);
            ProgramBudgetViewModel united = new ProgramBudgetViewModel("United Way Funding", arrayServices.sumArray(c.getChildren(UNITED_WAY_ALLOCATION).Values) * percent, percent);
            var supervision = c.getChildren(COUNSELLING);
            c.getSupervision(supervision);
            ProgramBudgetViewModel counseling = new ProgramBudgetViewModel("Counselling Grants", arrayServices.sumArray(supervision.Values) * percent, percent);
            var fcss = arrayServices.sumArray(c.getChildren(FCSS).Values);
            ProgramBudgetViewModel donation = new ProgramBudgetViewModel("Donations", fcss + arrayServices.sumArray(c.getChildren(FUNDRAISING).Values) * percent, percent);
            ProgramBudgetViewModel amortization = new ProgramBudgetViewModel("Amortization", 0, percent);



            list.Add(item);
            list.Add(united);
            list.Add(counseling);
            list.Add(donation);
            list.Add(amortization);
            foreach (var i in list) { total += i.value; }
            ProgramBudgetViewModel totalLine = new ProgramBudgetViewModel("Total Revenues", total, "highlight");
            list.Add(totalLine);


            return list;
        }

        private List<ProgramBudgetViewModel> counsellingExpenseList(decimal percent, bool includeSalary)
        {
            List<ProgramBudgetViewModel> list = new List<ProgramBudgetViewModel>();

            ServiceExpenseSummaryController c = new ServiceExpenseSummaryController(YEAR);
            decimal total = 0;
            if (includeSalary)
            {
                var salary = c.salariesAndBenefits();
                ProgramBudgetViewModel salaries = new ProgramBudgetViewModel("Salaries and Benefits", salary * percent, percent);
                list.Add(salaries);
            }

            ProgramBudgetViewModel bursary = new ProgramBudgetViewModel("Bursaries: Resident", arrayServices.sumArray(c.getBursaryDataLine(BURSARY).Values) * percent, percent);

            ProgramBudgetViewModel counsultant = new ProgramBudgetViewModel("Consultants", c.ContractServicesData() * percent, percent);

            GeneralExpense g = new GeneralExpense(YEAR);
            ProgramBudgetViewModel rent = new ProgramBudgetViewModel("Rent", arrayServices.sumArray(g.expenseDataLine(RENT).Values) * percent, percent);
            ProgramBudgetViewModel admin = new ProgramBudgetViewModel("General Administration", 0, percent);
            ProgramBudgetViewModel depreciaiton = new ProgramBudgetViewModel("Depreciation", 0, percent);


            list.Add(bursary);
            list.Add(counsultant);
            list.Add(rent);
            list.Add(admin);
            list.Add(depreciaiton);
            foreach (var i in list) { total += i.value; }
            ProgramBudgetViewModel totalLine = new ProgramBudgetViewModel("Total Expenses", total, "highlight");

            list.Add(totalLine);


            return list;
        }


        private List<ProgramBudgetViewModel> PersonellExpense(decimal percent)
        {
            List<ProgramBudgetViewModel> list = new List<ProgramBudgetViewModel>();
            ServiceExpenseSummaryController c = new ServiceExpenseSummaryController(YEAR);
            decimal total = 0;
            ProgramBudgetViewModel salary = new ProgramBudgetViewModel("Employee Salaries", arrayServices.sumArray(c.EmployeeSalaries()) * percent, percent);
            ProgramBudgetViewModel benefit = new ProgramBudgetViewModel("Employee Benefits", arrayServices.sumArray(c.expenseDataLine(6).Values) * percent, percent);
            ProgramBudgetViewModel remittance = new ProgramBudgetViewModel("Employee Remittances", arrayServices.sumArray(c.EmployeeRemittance()) * percent, percent);


            list.Add(salary);
            list.Add(benefit);
            list.Add(remittance);
            foreach (var i in list) { total += i.value; }
            ProgramBudgetViewModel totalLine = new ProgramBudgetViewModel("Total Personnel", total, "highlight");

            list.Add(totalLine);
            return list;
        }

        private List<ProgramBudgetViewModel> TraveParkingExpense(decimal percent)
        {
            List<ProgramBudgetViewModel> list = new List<ProgramBudgetViewModel>();
            GeneralExpense g = new GeneralExpense(YEAR);
            ServiceExpenseSummaryController c = new ServiceExpenseSummaryController(YEAR);
            decimal total = 0;
            ProgramBudgetViewModel Travel = new ProgramBudgetViewModel("Travel", arrayServices.sumArray(g.expenseDataLine(15).Values) * percent, percent);
            ProgramBudgetViewModel Parking = new ProgramBudgetViewModel("Parking", arrayServices.sumArray(c.expenseDataLine(7).Values) * percent, percent);


            list.Add(Travel);
            list.Add(Parking);

            foreach (var i in list) { total += i.value; }
            ProgramBudgetViewModel totalLine = new ProgramBudgetViewModel("Total Travel/Parking", total, "highlight");

            list.Add(totalLine);
            return list;
        }
        private List<ProgramBudgetViewModel> MaterialsSuppliesExpense(decimal percent)
        {
            List<ProgramBudgetViewModel> list = new List<ProgramBudgetViewModel>();
            GeneralExpense g = new GeneralExpense(YEAR);
            decimal total = 0;
            ProgramBudgetViewModel Supplies = new ProgramBudgetViewModel ("Office Supplies ", arrayServices.sumArray(g.expenseDataLine(25).Values) * percent, percent);
            ProgramBudgetViewModel postage = new ProgramBudgetViewModel  ("Postage ", arrayServices.sumArray(g.expenseDataLine(189).Values) * percent, percent);
            ProgramBudgetViewModel computers = new ProgramBudgetViewModel("Computers/Software/Internet", g.ITMaterials() * percent, percent);
            ProgramBudgetViewModel equipment = new ProgramBudgetViewModel("Equipment", arrayServices.sumArray(g.expenseDataLine(7).Values) * percent, percent);
            var materialValue = arrayServices.sumArray(g.expenseDataLine(184).Values);
            materialValue += arrayServices.sumArray(g.expenseDataLine(185).Values);
            ProgramBudgetViewModel materials = new ProgramBudgetViewModel("Program Materials/Exp", materialValue * percent, percent);


            list.Add(Supplies);
            list.Add(postage);
            list.Add(computers);
            list.Add(equipment);
            list.Add(materials);
            foreach (var i in list) { total += i.value; }
            ProgramBudgetViewModel totalLine = new ProgramBudgetViewModel("Total Materials & Supplies", total, "highlight");

            list.Add(totalLine);
            return list;
        }
        private List<ProgramBudgetViewModel> OtherExpense(decimal percent)
        {
            List<ProgramBudgetViewModel> list = new List<ProgramBudgetViewModel>();
            CounsellingServices services = new CounsellingServices(YEAR);
            GeneralExpense c = new GeneralExpense(YEAR);
            ServiceExpenseSummaryController s = new ServiceExpenseSummaryController(YEAR);

            decimal total = 0;
            ProgramBudgetViewModel Facilitators = new ProgramBudgetViewModel("Group Facilitators", arrayServices.sumArray(services.getRevenueSummaryCounselling()) * percent, percent);
            ProgramBudgetViewModel Advertising = new ProgramBudgetViewModel("Advertising", arrayServices.sumArray(c.expenseDataLine(6).Values) * percent, percent);
            ProgramBudgetViewModel Events = new ProgramBudgetViewModel("Meetings/Fundraising/Special Events", arrayServices.sumArray(c.expenseDataLine(8).Values) * percent, percent);
            ProgramBudgetViewModel Development = new ProgramBudgetViewModel("Staff Development", 0, percent);//not implemented
            ProgramBudgetViewModel Insurance = new ProgramBudgetViewModel("Insurance", arrayServices.sumArray(c.expenseDataLine(10).Values) * percent, percent);
            var bank = arrayServices.sumArray(c.expenseDataLine(11).Values);
            bank += arrayServices.sumArray(c.expenseDataLine(24).Values);
            ProgramBudgetViewModel Audit = new ProgramBudgetViewModel("Audit/Bank Charges", bank * percent, percent);
            ProgramBudgetViewModel Subscription = new ProgramBudgetViewModel("Subscription Fees", 0, percent);//not implemented
            ProgramBudgetViewModel Library = new ProgramBudgetViewModel("Library", arrayServices.sumArray(c.expenseDataLine(178).Values) * percent, percent);
            ProgramBudgetViewModel Telephone = new ProgramBudgetViewModel("Telephone", arrayServices.sumArray(c.expenseDataLine(14).Values) * percent, percent);
            ProgramBudgetViewModel Rent = new ProgramBudgetViewModel("Rent", arrayServices.sumArray(c.expenseDataLine(13).Values) * percent, percent);
            ProgramBudgetViewModel Debts = new ProgramBudgetViewModel("Bad Debts", arrayServices.sumArray(c.expenseDataLine(27).Values) * percent, percent);
            ProgramBudgetViewModel GST = new ProgramBudgetViewModel("GST", arrayServices.sumArray(c.expenseDataLine(9).Values) * percent, percent);
            ProgramBudgetViewModel Consulting = new ProgramBudgetViewModel("Consulting Fees", arrayServices.sumArray(s.expenseDataLine(10).Values) * percent, percent);
            ProgramBudgetViewModel Amortization = new ProgramBudgetViewModel("Amortization", 0, percent);//not implemented


            list.Add(Facilitators);
            list.Add(Advertising);
            list.Add(Events);
            list.Add(Development);
            list.Add(Insurance);
            list.Add(Audit);
            list.Add(Subscription);
            list.Add(Library);
            list.Add(Telephone);
            list.Add(Rent);
            list.Add(Debts);
            list.Add(GST);
            list.Add(Consulting);
            list.Add(Amortization);

            foreach (var i in list) { total += i.value; }
            ProgramBudgetViewModel totalLine = new ProgramBudgetViewModel("Total Other Expenses", total, "highlight");

            list.Add(totalLine);
            return list;
        }

    }
}