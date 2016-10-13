using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class EmployeeCounsellingHoursController : ObjectInstanceController
    {
        private int year;
        private CounsellingServices services;
        private ArrayServices arrayServices = new ArrayServices();

        public List<DataTable> EmployeeCounsellingHours()
        {
            year = YEAR;
            services = new CounsellingServices(year);
            List<DataTable> tables = new List<DataTable>();

            tables.Add(fullTimeCounsellingHours());
            tables.Add(contractCounsellingHours());
            tables.Add(residentCounsellingHours());
            tables.Add(internCounsellingHours());


            return tables;
        }

        

        private DataTable fullTimeCounsellingHours()
        {
            DataTable table = new DataTable();
            var counsellingHours = services.getEmployeeTargetsList();
            table.tableName = "Full Time";
            counsellingHours.Add(totalHours(counsellingHours));
            counsellingHours.Add(services.AssumptionFee(1));
            table.dataList = counsellingHours;

            return table;
        }

        private DataTable contractCounsellingHours()
        {
            DataTable table = new DataTable();
            List<DataLine> list = new List<DataLine>();
            var counsellingHours = services.getContractTargetsList();
            table.tableName = "Contract";
            counsellingHours.Add(totalHours(counsellingHours));
            counsellingHours.Add(services.AssumptionFee(3));
            table.dataList = counsellingHours;
            return table;
        }

        private DataTable residentCounsellingHours()
        {
            DataTable table = new DataTable();
            List<DataLine> list = new List<DataLine>();
            var counsellingHours = services.getResidentTargetList();
            counsellingHours.Add(totalHours(counsellingHours));
            table.tableName = "Residents";
            counsellingHours.Add(services.AssumptionFee(4));
            table.dataList = counsellingHours;


            return table;
        }
        private DataTable internCounsellingHours()
        {
            DataTable table = new DataTable();
            List<DataLine> list = new List<DataLine>();
            var counsellingHours = services.getInternTargetList();
            counsellingHours.Add(totalHours(counsellingHours));
            table.tableName = "Volunteers/Interns";
            counsellingHours.Add(services.AssumptionFee(6));
            table.dataList = counsellingHours;
            return table;
        }


        private DataLine totalHours(List<DataLine> data)
        {
            DataLine line = new DataLine();
            line.Name = "Total Hours";
            line.viewClass = "hourSubTotal";
            decimal[] values = new decimal[12];
            foreach(var item in data)
            {
                for(var i =0; i < 12; i++)
                {
                    values[i] += item.Values[i];
                }
            }
            line.Values = values;
            return line;
        }

    }
}