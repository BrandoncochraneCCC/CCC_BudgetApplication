/**
* Author - BRANDON COCHRANE
* Organization: Calgary Counselling Centre
* 
* August 3, 2016 - created (Brandon Cochrane)
*              - Add action to create Employee Data view model (Brandon Cochrane)
*              - Added create table for bugedtedSalary, Information, Salary, Deduction, Benefit (Brandon Cochrane)
* */

using Application.Controllers.Queries;
using Application.Controllers.Services;
using Application.Models;
using Application.ViewModels;
using Application.ViewModels.employeeData;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Application.Controllers.Employees
{
    public class EmployeeDataController : ObjectInstanceController
    {
        /**
         * Class services
         * calculations and logic to build objects
         * */
        private EmployeeServices services;
        private ArrayServices arrayServices = new ArrayServices();
        private EmployeeRaiseController raise;
        private int year;

        /**
         * create employee data view model object
        * @param employeeID - ID of employee to retreive data for
        * 
        * return table 
        * */
        public EmployeeDataViewModel EmployeeDataTables(int ID)
        {
            year = YEAR;
            raise = new EmployeeRaiseController(year);
            services = new EmployeeServices(year);
            EmployeeDataViewModel model = new EmployeeDataViewModel();

            try
            {
                var employee = services.getEmployee(ID);
                if (employee != null)
                {
                    model.deptID = (int)employee.DepartmentID;
                    model.year = year;
                    model.EmployeeName = services.formatName(employee);
                    model.InformationTable = EmployeeInformationTable(ID);//employee information

                    if (services.hasSalary(employee))
                    {
                        model.SalaryTable = EmployeeSalaryTable(ID);//salary table               
                    }
                    model.BudgetedSalaryTable = EmployeeBudgetedSalaryTable(ID);//Budgeted salary table
                    model.DeductionTable = EmployeeDeductionTable(ID);//deductions table
                    model.BenefitTable = EmployeeBenefitsTable(ID);//benefits table
                    model.EmployeeRaise = EmployeeRaiseTable(ID);
                    if (services.hasRaise(employee))
                    {
                        EmployeeRaiseController raise = new EmployeeRaiseController(year);
                        model.RaiseTable = EmployeeRaiseTable(model.SalaryTable, ID);//salary table  
                    }
                    if (services.hasTarget(employee))
                    {
                        model.TargetTable = EmployeeTargetTable(ID);//Target table
                    }
                }
            }
            catch(Exception ex)
            {
                log.Warn("creating employee data view exception", ex);
            }
            
           

            return model;
        }

        public List<DataTable> EmployeeTargetTables()
        {
            year = YEAR;
            raise = new EmployeeRaiseController(year);
            services = new EmployeeServices(year);
            EmployeeQueries queries = new EmployeeQueries(year);
            List<DataTable> tables = new List<DataTable>();
            try
            {
                var employees = queries.getEmployeeByType(FULLTIME_EMPLOYEETYPEID);

                foreach (var e in employees)
                {
                    if (e != null)
                    {
                        if (services.hasTarget(e))
                        {
                            var name = e.FirstName + " " + e.LastName;
                            tables.Add(EmployeeTargetTable(e.EmployeeID, name));
                        }
                    }


                }
            }
            catch(Exception ex)
            {
                log.Warn("Employee target table exception", ex);
            }
            
            return tables;
        }


        /**
         * create the employee salary table
         * @param employeeID - ID of employee to retreive data for
         * 
         * return table 
         * */
        private DataTable EmployeeSalaryTable(int ID)
        {
            DataTable table = new DataTable();
            try
            {
                var e = services.getEmployee(ID);
                table.sourceID = e.EmployeeID;
                table.tableName = "Salary";
                table.dataList = services.createEmployeeDataList(e);
                table.Year = year;

                if (table.dataList == null)
                {
                    table = null;
                }
            }
            catch(Exception ex)
            {
                log.Warn("employee salary table exception", ex);
            }
            

            return table;
        }

        /**
         * create the employee raise table
         * @param employeeID - ID of employee to retreive data for
         * 
         * return table 
         * */
        private List<Models.EmployeeRaise> EmployeeRaiseTable(int ID)
        {
            var e = services.getEmployee(ID);
            List<Models.EmployeeRaise> list = raise.RaiseHistory(e);

            return list;
        }
        /**
         * create the employee salary table with raise
         * @param employeeID - ID of employee to retreive data for
         * 
         * return table 
         * */
        private DataTable EmployeeRaiseTable(DataTable salaryTable, int ID)
        {
            var e = services.getEmployee(ID);
            DataTable table = new DataTable();
            table.sourceID = ID;
            table.tableName = "Salary After Raise";
            table.dataList = raise.RaiseDataLines(salaryTable, e);
            table.Year = year;

            if (table.dataList == null)
            {
                table = null;
            }

            return table;
        }

        /**
         * create the employee deduction table
         * @param employeeID - ID of employee to retreive data for
         * 
         * return table 
         * */
        private Deductions EmployeeDeductionTable(int ID)
        {
            Deductions deduction = new Deductions();

            try
            {
                var e = services.getEmployee(ID);
                DataTable table = new DataTable();
                table.sourceID = e.EmployeeID;
                table.tableName = "Deductions";
                table.dataList = services.createDeductionTable(e);
                table.Year = year;

                if (table.dataList == null)
                {
                    table = null;
                }

                deduction.cpp = CPP_LIST;
                deduction.ei = EI_LIST;
                deduction.rrsp = RRSP_LIST;
                services.setDeductions(deduction, e);
            }
            catch(Exception ex)
            {
                log.Warn("employee deduction table exception", ex);
            }
            
            return deduction;
        }

        /**
         * create the employee benefit table
         * @param employeeID - ID of employee to retreive data for
         * 
         * return table 
         * */
        private EmployeeBenefits EmployeeBenefitsTable(int ID)
        {
            EmployeeBenefits benefits = new EmployeeBenefits();

            try
            {
                var e = services.getEmployee(ID);
                benefits.Employee = e;
                DataTable table = new DataTable();
                table.sourceID = e.EmployeeID;
                table.tableName = "Benefits";
                table.dataList = services.createBenefitTable(e);
                table.Year = year;

                if (table.dataList == null)
                {
                    table = null;
                }
                benefits.BenefitTable = table;
                benefits.family = FAMILY_LIST;
                benefits.individual = INDIVIDUALS_LIST;
                benefits.std = STD_LIST;
                benefits.vacation = VACATION_LIST;
                services.setBenefits(benefits);
            }
            catch(Exception ex)
            {
                log.Warn("benefits table exception", ex);
            }
            

            return benefits;
        }

        

        /**
         * create the employee target table
         * @param employeeID - ID of employee to retreive data for
         * 
         * return table 
         * */
        private DataTable EmployeeTargetTable(int ID, string name = "Targets")
        {
            DataTable table = new DataTable();
            try
            {
                var e = services.getEmployee(ID);
                table.sourceID = e.EmployeeID;
                table.tableName = name;
                table.dataList = services.createTargetTable(e);
                table.Year = year;

                if (table.dataList == null)
                {
                    table = null;
                }
            }
            catch(Exception ex)
            {
                log.Warn("employee target table exception", ex);
            }
            

            return table;
        }

        /**
         * create the employee Information table
         * @param employeeID - ID of employee to retreive data for
         * 
         * return employee 
         * */
        private InformationTable EmployeeInformationTable(int ID)
        {
            InformationTable obj = new InformationTable();
            try
            {
                obj.informationTable = services.getEmployee(ID);
                obj.departments = DEPARTMENTS_LIST;
                obj.types = TYPES_LIST;
            }
            catch(Exception ex)
            {
                log.Warn("employee information exception", ex);
            }

            return obj;
        }


        /**
         * create the employee budgeted salary table
         * @param employeeID - ID of employee to retreive data for
         * 
         * return table 
         * */
        private DataTable EmployeeBudgetedSalaryTable(int ID)
        {
            DataTable table = new DataTable();
            try
            {
                var e = services.getEmployee(ID);
                table.sourceID = e.EmployeeID;
                table.tableName = "Yearly Salary";
                table.dataList = services.EmployeeBudgetedSalaryTable(e);
                table.Year = year;

                if (table.dataList == null)
                {
                    table = null;
                }
            }
            catch(Exception ex)
            {
                log.Warn("budgeted salary exception", ex);
            }


            return table;
        }

        private void selectDropDownValues(Employee e)
        {
            foreach(var d in DEPARTMENTS_LIST)
            {
                if (d.Value.Equals(e.DepartmentID.ToString()))
                {
                    d.Selected = true;
                }
            }
        }



        

    }
}