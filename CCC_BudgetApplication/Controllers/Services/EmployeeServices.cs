/**
 * Author - BRANDON COCHRANE
 * Organization: Calgary Counselling Centre
 * 
 * August 3, 2016 - created (Brandon Cochrane)
 * */

using Application.Controllers.Queries;
using Application.Models;
using Application.ViewModels;
using Application.ViewModels.employeeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Controllers.Services
{
    public class EmployeeServices
    {
        //deducion type IDs
        public static int CPP_DEDUCTIONTYPEID = 1;
        public static int EI_DEDUCTIONTYPEID = 2;
        public static int RRSP_DEDUCTIONTYPEID = 3;
        public static int OTHER_DEDUCTIONTYPEID = 4;
        public static int FAMILY_DEDUCTIONTYPEID = 11;
        public static int INDIVIDUAL_DEDUCTIONTYPEID = 12;
        public static int STD_DEDUCTIONTYPEID = 10;
        public static int NO_STD_DEDUCTIONTYPEID = 2014;
        public static int CEO_RRSP_DEDUCTIONTYPEID = 1011;
        public static int CEO_FAMILY_DEDUCTIONTYPEID = 1012;
        public static int VACATION_DEDUCTIONTYPEID = 9;
        public static int STUDENT_EMPLOYEETYPEID = 5;
        public static int FULLTIME_EMPLOYEETYPEID = 1;
        public static int PARTTIME_EMPLOYEETYPEID = 2;
        public static int CONTRACT_EMPLOYEETYPEID = 3;
        public static int INTERN_EMPLOYEETYPEID = 6;
        public static int STUDENT_DEPARTMENT = 8;



        //end constants


        /**
        * Class services
        * calculations and logic to build objects
        * */
        private ArrayServices arrayServices = new ArrayServices();
        private int year;
        private EmployeeQueries queries;
        private DateTimeServices dt;

        /**
         * constructor set current budget year
         * @Param year current budget year
         * */
        public EmployeeServices(int year)
        {
            this.year = year;
            queries = new EmployeeQueries(year);
            dt = new DateTimeServices(year);
        }

        /**
         * formats employee name for view
         * @Param e employee to format
         * 
         * @return string of formatted name
         * */
        public string formatName(Employee e)
        {
            return e.FirstName + " " + e.LastName;
        }

        /**
         * gets a single employee based on ID
         * @employeeID id of employee to find
         * 
         * @return employee
         * */
        public Employee getEmployee(int employeeID)
        {
            return queries.getSingleEmployee(employeeID);
        }


        /**
         * builds data for the Employee Salary Table
         * @Param e Employee to retreive data for
         * 
         * @return list list DataLine objects to display in view
         * */
        public List<DataLine> createEmployeeDataList(Employee e)
        {
            bool start = false; if (e.StartDate == null) { start = true; };
            bool end = false; if (e.StartDate == null) { end = true; };

            defaultStartEndDate(e);
            decimal[] daysWorking = workingMonths(e); //number of working days per month employee works in year

            decimal[] sData = salaryData(e);//monthly salary data
            List<DataLine> list = buildEmployeeDataList(e, sData);

            if (start) { e.StartDate = null; }
            if (end) { e.EndDate = null; }
            return list;
        }
        public List<DataLine> buildEmployeeDataList(Employee e, decimal[] salaryData)
        {
            List<DataLine> list = new List<DataLine>();
            if (arrayServices.sumArray(salaryData) != 0)
            {
                list.Add(createDataLine("Salary", salaryData)); // add monthly salary data line
                list.Add(createDataLine("CPP", calculateCPP(salaryData)));//CPP
                list.Add(createDataLine("EI", calculateEI(e, salaryData)));//EI
                list.Add(createDataLine("RRSP", calculateRRSP(e, salaryData)));//RRSP
                list.Add(createDataLine("Other", calculateOther(e, salaryData)));//Other: NOT IMPLEMENTED
                list.Add(createDataLine("Benefits", calculateBenefits(e, salaryData)));//benefits
                list.Add(createDataLine("Vacation", calculateVacation(e, salaryData))); //vacation
                list.Add(createDataLine("STD", calculateSTD(e, salaryData)));//std
                //list.Add(createDataLine("days", daysWorking)); //number of working days per month
            }
            else
            {
                list = null;
            }

            return list;
        }


        /**
         * builds data for the Employee Budgeted salary Table
         * @Param e Employee to retreive data for
         * 
         * @return list list DataLine objects to display in view
         * */
        public List<DataLine> EmployeeBudgetedSalaryTable(Employee e)
        {
            bool start = false; if (e.StartDate == null) { start = true; };
            bool end = false; if (e.StartDate == null) { end = true; };

            List<DataLine> list = new List<DataLine>();
            defaultStartEndDate(e);
            list.Add(salaryBudgetLine(e)); //previous and current budget salary total
            if (start) { e.StartDate = null; }
            if (end) { e.EndDate = null; }
            return list;
        }

        /**
         * gets data for the Other deductions for employee
         * @Param e Employee to retreive data for
         * @Param sData salary data for employee to calculate deduction from
         * 
         * @return list list DataLine objects to display in view
         * */
        public decimal[] calculateOther(Employee e, decimal[] sData)
        {
            return new decimal[12];
        }

        /**
         * sets start/end date to first/last day of year respectively
         * if employee does not have start/end date 
         * 
         * @param e employee to modify
         * */
        private void defaultStartEndDate(Employee e)
        {
            if (e.StartDate == null)
            {
                e.StartDate = new DateTime(year, 1, 1);
            }
            if (e.EndDate == null)
            {
                e.EndDate = new DateTime(year, 12, 31);
            }
        }


        /**
         * creates a DataLine object that is used in view
         * @Param name name of the dataLine
         * @param values values to place into the data line
         * 
         * @return line dataline to be sent to view
         * */
        private DataLine createDataLine(string name, decimal[] values)
        {
            DataLine line = new DataLine();
            line.Name = name;
            line.Values = values;

            return line;
        }

        /**
        * creates a DataLine object with EmployeeID that is used in view 
        * @Param name name of the dataLine
        * @param values values to place into the data line
        * @param e employee to build data line for
        * 
        * @return line dataline to be sent to view
        * */
        private DataLine createDataLine(string name, decimal[] values, Employee e)
        {
            DataLine line = new DataLine();
            line.Name = name;
            line.Values = values;
            line.SourceID = e.EmployeeID;

            return line;
        }

        /**
         * gets CPP
         * @Param salary data to calculate deduction from
         * 
         * @return array containing deduction values for each month
         * */
        public decimal[] calculateCPP(decimal[] salary)
        {
            return deductionValues(salary, CPP_DEDUCTIONTYPEID);
        }

        /**
         * gets EI
         * @Param salary data to calculate deduction from
         * @Param e employee deduction belongs to
         * 
         * @return array containing deduction values for each month
         * */
        public decimal[] calculateEI(Employee e, decimal[] salary)
        {
            int deductionID = findEI(e);
            defaultStartEndDate(e);
            return deductionValues(salary, deductionID, (DateTime)e.StartDate, (DateTime)e.EndDate);
        }

        /**
         * gets RRSP
         * @Param salary data to calculate deduction from
         * @Param e employee deduction belongs to
         * 
         * @return array containing deduction values for each month
         * */
        public decimal[] calculateRRSP(Employee e, decimal[] salary)
        {
            int deductionID = findRRSP(e);
            if (e.DepartmentID != STUDENT_DEPARTMENT)
            {
                return deductionValues(salary, deductionID, (DateTime)e.StartDate, (DateTime)e.EndDate);
            }
            else
            {
                return new decimal[12];
            }
        }

        /**
         * gets Benefits
         * @Param salary data to calculate deduction from
         * @Param e employee deduction belongs to
         * 
         * @return array containing deduction values for each month
         * */
        public decimal[] calculateBenefits(Employee e, decimal[] salary)
        {
            int deductionID = findBenefit(e);
            if (deductionID != 0 && e.DepartmentID != STUDENT_DEPARTMENT)
            {
                return deductionValues(salary, deductionID, (DateTime)e.StartDate, (DateTime)e.EndDate);
            }
            else
            {
                return new decimal[12];
            }
        }

        /**
         * gets Short term disability
         * @Param salary data to calculate deduction from
         * @Param e employee deduction belongs to
         * 
         * @return array containing deduction values for each month
         * */
        public decimal[] calculateSTD(Employee e, decimal[] salary)
        {
            if (!hasDeduction(e.EmployeeID, NO_STD_DEDUCTIONTYPEID) && e.DepartmentID != STUDENT_DEPARTMENT) //if employee has individual benefits
            {
                return deductionValues(salary, STD_DEDUCTIONTYPEID, (DateTime)e.StartDate, (DateTime)e.EndDate);
            }
            else
            {
                return new decimal[12];
            }

        }

        /**
         * gets Vacation
         * @Param salary data to calculate deduction from
         * @Param e employee deduction belongs to
         * 
         * @return array containing deduction values for each month
         * */
        public decimal[] calculateVacation(Employee e, decimal[] salary)
        {
            if (hasVacation(e)) //if employee has individual benefits
            {
                return deductionValues(salary, VACATION_DEDUCTIONTYPEID, (DateTime)e.StartDate, (DateTime)e.EndDate);
            }
            else
            {
                return new decimal[12];
            }
        }
        private bool hasVacation(Employee e)
        {
            return queries.hasVacation(e);
        }
        /**
         * gets deductions based on the deduction type
         * @Param salary data to calculate deduction from
         * @Param deductionTypeID ID of the deduction type
         * 
         * @return array containing deduction values for each month
         * */
        public decimal[] deductionValues(decimal[] salary, int deductionTypeID)
        {
            DateTime start = new DateTime(year, 1, 1);
            DateTime end = new DateTime(year, 12, 31);
            return deductionValues(salary, deductionTypeID, start, end);
        }

        /**
         * calculate deduction values
         * @Param salary data to calculate deduction from
         * @Param deductionTypeID ID of the deduction type
         * @Param start start date of employee
         * @Param end end date of employee
         * 
         * @return array containing deduction values for each month
         * */
        private decimal[] deductionValues(decimal[] salary, int deductionTypeID, DateTime start, DateTime end)
        {
            var deduction = queries.deductionData(deductionTypeID);
            decimal[] values = new decimal[12];

            if (deduction != null && salary != null)
            {
                decimal rate = (decimal)deduction.Rate / 100;
                if (rate != 0)
                {
                    if (deduction.Max != null && deduction.Max != 0)
                    {
                        values = arrayServices.calculateDeductionsWithMaxValues((decimal)deduction.Max, rate, salary);
                    }
                    else
                    {
                        values = arrayServices.calculateDeductionsNoMax(rate, salary);
                    }
                }
                else
                {
                    decimal[] tempArray = arrayServices.populateArray((decimal)deduction.Max);
                    values = arrayServices.divideArrayByValueIfSalary(tempArray, salary, 12);
                }
            }



            return values;
        }


        /**
         * creates data line object to be used in view
         * @param values values of data line
         * 
         * @return line line used in view
         * */
        private DataLine createDataLine(decimal[] values)
        {
            DataLine line = new DataLine();
            line.Values = values;

            return line;
        }

        /**
         * creates dataline for salary budget
         * @Param e employee salary belongs to
         * 
         * @return dataline object to be used in view
         * */
        private DataLine salaryBudgetLine(Employee e)
        {
            var salary = queries.getEmployeeSalary(e.EmployeeID);
            decimal hrlyRate = getHourlyRate(e);
            return createDataLine(prevSalaryData(salary, hrlyRate, e));
        }


        /**
         * creates values for budgeted salary table
         * @Param s salary to use
         * @Param hrlyRate hourly rate of employee
         * @Param e employee salary belongs to
         * 
         * @return values array of values used in dataLine object
         * */
        private decimal[] prevSalaryData(Salary s, decimal hrlyRate, Employee e)
        {
            decimal[] values = new decimal[6];

            values[0] = queries.previousBudgetSalary(s);
            values[1] = queries.previousActualSalary(s);
            fillSalaryData(s);
            values[2] = queries.currentBudgetSalary(s);
            values[3] = values[2];
            if (hasRaise(e))
            {
                var raise = queries.getEmployeeRaise(e.EmployeeID);
                foreach (var item in raise)
                {
                    var temp = item.Value;
                    if (item.isPercent)
                    {
                        temp = (((item.Value / 100) + 1) * values[2]) - values[2];
                    }
                    values[3] += temp;
                }
            }

            values[4] = hrlyRate;
            values[5] = queries.getWorkHoursPerYear();



            return values;
        }

        /**
         * gets hourly rate of employee
         * @param e employee to get rate for
         * 
         * @return perHour hourly rate of employee
         * */
        public decimal getHourlyRate(Employee e)
        {
            var salary = queries.getEmployeeSalary(e.EmployeeID);
            decimal perHour = 0;
            if (salary != null && salary.HourlyRate != 0)
            {
                if (salary.HourlyRate != null)
                {
                    perHour = (decimal)salary.HourlyRate;
                }
            }
            if (salary != null)
            {
                //decimal[] workDays = workDaysPerMonth((DateTime)e.StartDate);
                //decimal[] monthlyHours = arrayServices.workHours(workDays);
                //decimal termHours = arrayServices.sumArray(monthlyHours, (DateTime)e.StartDate, (DateTime)e.EndDate);
                var termHours = queries.getWorkHoursPerYear();
                defaultStartEndDate(e);
                var months = e.EndDate.Value.Month - (e.StartDate.Value.Month - 1);
                var hours = (termHours / 12) * months;
                if (perHour == 0)
                {
                    if(hours != 0 && salary.CurrentBudget != null)
                    {
                        perHour = (decimal)salary.CurrentBudget / hours;
                    }
                }

            }
            return perHour;

        }

        /**
         * fills in missing salary data from employee
         * @Param s salary to fill data for 
         * */
        private void fillSalaryData(Salary s)
        {

            if (hasHrlyRateNoSalary(s))
            {
                s.CurrentBudget = salaryFromHourly(s);
            }
            if (hasSalaryNoHrlyRate(s))
            {
                s.HourlyRate = calculateHourlyRate((decimal)s.CurrentBudget);
            }

        }

        /**
         * dataline object for employee salary
         * @param e employee salary belongs to
         * 
         * @return dataline object with employee salary
         * */
        private DataLine empSalaryData(Employee e)
        {
            decimal[] sData = salaryData(e);
            return createDataLine("Salary", sData);
        }

        /**
        * retreives values for employee salary
        * @param e employee salary belongs to
        * 
        * @return array with employee salary per month
        * */
        public decimal[] getSalaryData(Employee e)
        {
            return salaryData(e);
        }

        /**
        * calculates values for employee salary
        * @param e employee salary belongs to
        * 
        * @return array with employee salary per month
        * */
        private decimal[] salaryData(Employee e)
        {
            bool start = false; if (e.StartDate == null) { start = true; };
            bool end = false; if (e.StartDate == null) { end = true; };
            decimal[] values = new decimal[12];

            defaultStartEndDate(e);
            Salary salary = null;
            if (e != null)
            {
                salary = queries.getEmployeeSalary(e.EmployeeID);
            }
            if (salary != null)
            {
                decimal current = 0;
                if (salary.CurrentBudget != null)
                {
                    current = (decimal)salary.CurrentBudget;
                }
                if (hasHrlyRateNoSalary(salary))
                {
                    current = salaryFromHourly(salary, e);
                }
                decimal[] workDays = workDaysPerMonth((DateTime)e.StartDate);
                if (salary != null)
                {
                        values = arrayServices.monthlySalary(current, (DateTime)e.StartDate, (DateTime)e.EndDate, year);
                }
            }
            


            if (start) { e.StartDate = null; }
            if (end) { e.EndDate = null; }
            return values;
        }

        private decimal salaryFromHourly(Salary s)
        {
            var e = queries.getSingleEmployee(s.EmployeeID);
            return salaryFromHourly(s, e);
        }

        private decimal salaryFromHourly(Salary s, Employee e)
        {
            var yearly = calculateSalary((decimal)s.HourlyRate);
            var start = 0;
            var end = 12;
            if (e.StartDate != null) { start = e.StartDate.Value.Month - 1; }
            if (e.EndDate != null) { end = e.EndDate.Value.Month; }
            var months = end - start;
            return yearly / 12 * months;
        }

        /**
         * checks if employee is missing salary but has hourly rate
         * @Param s salary to check
         * 
         * @return boolean if has hourly rate and no salary
         * */
        private bool hasHrlyRateNoSalary(Salary s)
        {
            bool hasHrlyRate = false;
            if (s != null)
            {
                if (s.CurrentBudget == 0 || s.CurrentBudget == null && s.HourlyRate != 0 && s.HourlyRate != null)
                {
                    hasHrlyRate = true;
                }
            }


            return hasHrlyRate;
        }

        /**
         * checks if employee is missing hourly rate but has salary
         * @Param s salary to check
         * 
         * @return boolean if has salary and no hourly rate
         * */
        private bool hasSalaryNoHrlyRate(Salary s)
        {
            bool result = false;
            if (s != null)
            {
                if (s.HourlyRate == 0 || s.HourlyRate == null && s.CurrentBudget != 0 && s.CurrentBudget != null)
                {
                    result = true;
                }
            }
            return result;
        }

        /**
         * calculates hourly rate of employee
         * @param currentBudget budgeted salary for current year
         * 
         * @return currentBudget hourly rate of employee for current budget year
         * */
        private decimal calculateHourlyRate(decimal currentBudget)
        {
            if (queries.getHoursPerYear() != 0)
            {
                currentBudget /= queries.getHoursPerYear();
            }
            return currentBudget;
        }

        /**
        * calculates salary of employee
        * @param rate hourly rate of employee
        * 
        * @return salary of employee for current budget year
        * */
        private decimal calculateSalary(decimal rate)
        {
            return rate * queries.getHoursPerYear();
        }

        /**
         * checks if employee has salary for current budget year
         * @Param employee employee to check for
         * 
         * @return boolean if employee has salary for current budget year
         * */
        private bool hasSalaryForYear(Employee Employee)
        {
            bool hasSalary = false;

            if (queries.getEmployeeSalary(Employee.EmployeeID) != null)
            {
                hasSalary = true;
            }

            return hasSalary;
        }


        /**
         * checks if employee has deductions
         * @Param empID id of employee to check
         * @Param dedTypeID id of deduction to check
         * 
         * @return boolean if employee has a specific deduction
         * */
        private bool hasDeduction(int empID, int dedTypeID)
        {
            bool hasDeduction = false;

            if (queries.getEmpDed(empID, dedTypeID) != null)
            {
                hasDeduction = true;
            }
            return hasDeduction;
        }


        /**
         * calculated working days per month
         * @Param e employee to get working days for
         * 
         * @return array with number of working days per month
         * */
        private decimal[] workingMonths(Employee e)
        {
            DateTime start = new DateTime(year, 1, 1);
            DateTime end = new DateTime(year, 12, 31);

            if (e.StartDate != null && e.StartDate.Value.Year == year)
            {
                start = e.StartDate.Value;
            }
            if (e.EndDate != null && e.EndDate.Value.Year == year)
            {
                end = e.EndDate.Value;
            }

            decimal[] workingDays = workDaysPerMonth(start);
            //workingDays = workingDaysWithVacation(e);

            return workingDays;
        }

        /**
         * calculates working days per month
         * @Param startdate startdate of employee
         * 
         * @return array with number of working days per month
         * */
        private decimal[] workDaysPerMonth(DateTime startDate)
        {
            decimal[] values = new decimal[12];

            for (var i = 0; i < 12; i++)
            {
                var start = new DateTime(year, i + 1, 1);
                if (startDate.Month == i + 1 && startDate.Day != 1)
                {
                    start = startDate;
                }

                //values[i] = dt.workdaysToEndMonth(start);
            }

            return values;
        }

        /**
         * creates data for deduction table
         * @Param e employee data belongs to
         * 
         * @return list list of data used in view
         * */
        public List<DataLine> createDeductionTable(Employee e)
        {
            List<DataLine> list = new List<DataLine>();
            list.Add(deductionMax(e));
            list.Add(deductionRate(e));

            return list;
        }

        /**
         * get max values for each dudction type
         * @Param e employee to get values for
         * 
         * @return array of values 
         * */
        private DataLine deductionMax(Employee e)
        {
            var eiMax = findEI(e);
            DataLine line = new DataLine();
            decimal[] values = new decimal[3];
            values[0] = queries.deductionMax(CPP_DEDUCTIONTYPEID);
            values[1] = queries.deductionMax(eiMax);
            values[2] = queries.deductionMax(RRSP_DEDUCTIONTYPEID);
            line.Name = "Max";
            line.Values = values;

            return line;
        }

        /**
         * get rate for each dudction type
         * @Param e employee to get values for
         * 
         * @return array of values 
         * */
        private DataLine deductionRate(Employee e)
        {
            var eiRate = findEI(e);
            var rrspRate = findRRSP(e);
            DataLine line = new DataLine();
            decimal[] values = new decimal[3];
            values[0] = queries.deductionRate(CPP_DEDUCTIONTYPEID);
            values[1] = queries.deductionRate(eiRate);
            values[2] = queries.deductionRate(rrspRate);
            line.Name = "Rate";
            line.percentValues = values;

            return line;

        }

        /**
         * get deductionID to be used in determining EI values
         * @Param e employee to get values for
         * 
         * @return id
         * */
        private int findEI(Employee e)
        {
            int deductionID = EI_DEDUCTIONTYPEID;
            if (hasDeduction(e.EmployeeID, NO_STD_DEDUCTIONTYPEID)) //if employee has short term disability
            {
                deductionID = NO_STD_DEDUCTIONTYPEID;
            }
            return deductionID;
        }


        /**
         * get deductionID to be used in determining RRSP values
         * @Param e employee to get values for
         * 
         * @return id
         * */
        private int findRRSP(Employee e)
        {
            int deductionID = RRSP_DEDUCTIONTYPEID;
            if (hasDeduction(e.EmployeeID, CEO_RRSP_DEDUCTIONTYPEID)) //if employee has ceo RRSP rates
            {
                deductionID = CEO_RRSP_DEDUCTIONTYPEID;
            }
            return deductionID;
        }

        /**
         * creates data for benefit table
         * @Param e employee data belongs to
         * 
         * @return list list of data used in view
         * */
        public List<DataLine> createBenefitTable(Employee e)
        {
            List<DataLine> list = new List<DataLine>();
            list.Add(employeeBenefits(e));

            return list;
        }

        public void setBenefits(EmployeeBenefits benefits)
        {
            var id = benefits.Employee.EmployeeID;
            if (hasSTD(benefits.Employee))
            {
                benefits.hasSTD = true;
            }

            if (hasVacation(benefits.Employee))
            {
                benefits.hasVacation = true;
            }

            if (hasDeduction(id, FAMILY_DEDUCTIONTYPEID))
            {
                benefits.hasFamily = true;
                benefits.hasIndividual = false;
                benefits.deductionID = queries.getEmpDed(id, FAMILY_DEDUCTIONTYPEID).DeductionTypeID;
            }
            else if (hasDeduction(id, CEO_FAMILY_DEDUCTIONTYPEID))
            {
                benefits.hasFamily = true;
                benefits.hasIndividual = false;

                benefits.deductionID = queries.getEmpDed(id, CEO_FAMILY_DEDUCTIONTYPEID).DeductionTypeID;
            }
            else if (hasDeduction(id, INDIVIDUAL_DEDUCTIONTYPEID))
            {
                benefits.hasIndividual = true;
                benefits.hasFamily = false;

                benefits.deductionID = queries.getEmpDed(id, INDIVIDUAL_DEDUCTIONTYPEID).DeductionTypeID;
            }


        }
        public DeductionList getCPP()
        {
            return queries.deductionData(CPP_DEDUCTIONTYPEID);
        }
        public DeductionList getEI()
        {
            return queries.deductionData(EI_DEDUCTIONTYPEID);
        }
        public DeductionList getRRSP()
        {
            return queries.deductionData(RRSP_DEDUCTIONTYPEID);
        }

        public void setDeductions(Deductions deduction, Employee e)
        {
            var rrsp = queries.deductionData(RRSP_DEDUCTIONTYPEID);
            deduction.rrspID = RRSP_DEDUCTIONTYPEID;

            var cpp = queries.deductionData(CPP_DEDUCTIONTYPEID);

            var ei = queries.deductionData(EI_DEDUCTIONTYPEID);


            if (hasDeduction(e.EmployeeID, CEO_RRSP_DEDUCTIONTYPEID))
            {
                deduction.rrspID = CEO_RRSP_DEDUCTIONTYPEID;
                rrsp = queries.deductionData(CEO_RRSP_DEDUCTIONTYPEID);
            }

            if (hasDeduction(e.EmployeeID, NO_STD_DEDUCTIONTYPEID))
            {
                ei = queries.deductionData(NO_STD_DEDUCTIONTYPEID);
            }
            if(rrsp != null)
            {
                deduction.rrspRate = (decimal)rrsp.Rate / 100;
                deduction.rrspMax = (decimal)rrsp.Max;
            }

            if (cpp != null)
            {
                deduction.cppMax = (decimal)cpp.Max;
                deduction.cppRate = (decimal)cpp.Rate / 100;
            }

            if (ei != null)
            {
                deduction.eiMax = (decimal)ei.Max;
                deduction.eiRate = (decimal)ei.Rate / 100;
            }

        }

        /**
         * creates data for target table
         * @Param e employee data belongs to
         * 
         * @return list list of data used in view
         * */
        public List<DataLine> createTargetTable(Employee e)
        {
            CounsellingServices counsellingServices = new CounsellingServices(year);
            List<DataLine> list = counsellingServices.targetTable(e);

            return list;
        }


        /**
         * gets values for benefits table
         * @Param e employee data belongs to
         * 
         * @return line dataline used in view
         * */
        private DataLine employeeBenefits(Employee e)
        {
            DataLine line = new DataLine();
            decimal[] values = new decimal[3];
            values[0] = queries.deductionMax(findBenefit(e));
            values[1] = queries.deductionMax(INDIVIDUAL_DEDUCTIONTYPEID);
            if (hasSTD(e))
            {
                values[2] = queries.deductionMax(STD_DEDUCTIONTYPEID);
            }
            line.Values = values;
            return line;
        }

        /**
         * get deductionID to be used in determining benefit values
         * @Param e employee to get values for
         * 
         * @return id
         * */
        private int findBenefit(Employee e)
        {
            int deductionID = 0;
            if(hasDeduction(e.EmployeeID, FAMILY_DEDUCTIONTYPEID))
            {
                deductionID = FAMILY_DEDUCTIONTYPEID;
            }
            else if (hasDeduction(e.EmployeeID, INDIVIDUAL_DEDUCTIONTYPEID)) //if employee has individual benefits
            {
                deductionID = INDIVIDUAL_DEDUCTIONTYPEID;
            }
            else if (hasDeduction(e.EmployeeID, CEO_FAMILY_DEDUCTIONTYPEID))//if employee has ceo RRSP rates
            {
                deductionID = CEO_FAMILY_DEDUCTIONTYPEID;
            }

            return deductionID;
        }

        public bool hasSalary(Employee e)
        {
            return queries.hasSalary(e);
        }

        public bool hasRaise(Employee e)
        {
            return queries.hasRaise(e);
        }

        public bool hasTarget(Employee e)
        {
            return queries.hasTarget(e);
        }

        /**
         * checks if employee has short term disability
         * @Param e employee to get values for
         * 
         * @return boolean if employee has short term disability
         * */
        private bool hasSTD(Employee e)
        {
            bool hasSTD = false;

            if (!hasDeduction(e.EmployeeID, NO_STD_DEDUCTIONTYPEID)) //if employee has STD benefits
            {
                hasSTD = true;
            }

            return hasSTD;
        }

       

    }
}