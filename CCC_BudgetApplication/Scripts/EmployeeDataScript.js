var EditButtons = " <td> " +
                    " <input type='button' class='display edit-btn' value='Edit' />" +
                    " <input type='button' class='edit save-btn' value='Save' /> " +
                    " <input type='button' class='edit cancel-btn' value='Cancel' /> " +
                "</td>";

var input = "<input type = 'text' size ='3' value='' class =''/>";

var values = [];




function readCell(row, name) {

    var total = row.find("." + name).val();


    return total;
}

function getEmployeeID() {

    var path = location.pathname;
    var partialPath = path.split("EmployeeData/");

    return partialPath[1];
}

function ajaxCall(destination, obj) {
    var data;
    $.ajax({

        url: destination,

        data: JSON.stringify(obj),

        type: 'POST',

        cache: false,

        async: false,
        
        traditional: true,

        contentType: 'application/json; charset=utf-8',

        dataType: 'json',

        success: function (result) {
            data = result;
        },
        error: function (xhr) {
            alert(xhr.status);

        }
    });
    return data;
}

function createEmployeeTarget(empID, year) {

    var emp = 
        {
            "EmployeeID": empID,
            "Year": year,
            "URL": window.location.toString()
        }
    return emp;

}

function createTargets(obj) {

    var empID = getEmployeeID();
    var year = $(obj).attr('id');
    var emp = createEmployeeTarget(empID, year);
    var destination = '/EmployeeEdit/AddEmployeeTarget';

    ajaxCallParameter(destination, emp);

}

function cancel(obj) {
    rowCancel = $(obj).closest("tr");
    rowCancel.find(':text').remove();

}

function isNonRev(obj) {

    var isNonRev = false;
    var name = getNonRevName(obj);
    if (name != "TotalRevenueHours") {

        isNonRev = true;

    }
    return isNonRev;
}

function addTextBox(obj, start) {
    var end = start + 12;
    var row = $(obj).closest('tr');
    var className = "1";
    for (i = start; i < end; i++) {

        var value = row.find("td:nth-child(" + i + ")").html();
        var inputValue = value.replace(/\$|,/g, '');
        if (isNonRev(obj))
        {
            inputValue *= -1;
        }
        //var inputValueNB = inputValue.replace(/[{()}]/g, '');
        row.find("td:nth-child(" + i + ")").append(input);
        row.find("td:nth-child(" + i + ") input").val(inputValue);
        row.find("td:nth-child(" + i + ") input").addClass(className);
        className+= "1";
    }

}



function readValues(obj) {
    var row = $(obj).closest('tr');
    var name = "1";

    for (i = 0; i < 12; i++) {
        var total = readCell(row, name);
        if (total == "") {
            values[i] = 0;

        }
        else {
            values[i] = total;

        }
        name += "1";
    }
    
}

function getNonRevName(obj) {

    var row = $(obj).closest('tr');
    var editName = row.find('.name').html().replace(/\s/g, '');
    return editName;
}

function createEmpTarIdObject(empID) {

    var empTarToChange = { "EmployeeID": empID }

    return empTarToChange;
}

function createTargetData(empTarID) {

    var targetData = [
        { "EmployeeTargetID": empTarID, "Date": "1/1/2015 12:00:00 AM", "RevenueHours": values[0] },
        { "EmployeeTargetID": empTarID, "Date": "2/1/2014 12:00:00 AM", "RevenueHours": values[1] },
        { "EmployeeTargetID": empTarID, "Date": "3/1/2013 12:00:00 AM", "RevenueHours": values[2] },
        { "EmployeeTargetID": empTarID, "Date": "4/1/2012 12:00:00 AM", "RevenueHours": values[3] },
        { "EmployeeTargetID": empTarID, "Date": "5/1/2011 12:00:00 AM", "RevenueHours": values[4] },
        { "EmployeeTargetID": empTarID, "Date": "6/1/2010 12:00:00 AM", "RevenueHours": values[5] },
        { "EmployeeTargetID": empTarID, "Date": "7/1/2009 12:00:00 AM", "RevenueHours": values[6] },
        { "EmployeeTargetID": empTarID, "Date": "8/1/2008 12:00:00 AM", "RevenueHours": values[7] },
        { "EmployeeTargetID": empTarID, "Date": "9/1/2007 12:00:00 AM", "RevenueHours": values[8] },
        { "EmployeeTargetID": empTarID, "Date": "10/1/2006 12:00:00 AM", "RevenueHours": values[9] },
        { "EmployeeTargetID": empTarID, "Date": "11/1/2005 12:00:00 AM", "RevenueHours": values[10] },
        { "EmployeeTargetID": empTarID, "Date": "12/1/2004 12:00:00 AM", "RevenueHours": values[11] }
    ];

    return targetData;
}

function createTargetIdObj(empTarID) {

    var targetData = [

        { "EmployeeTargetID": empTarID, "Date": "1/1/2015 12:00:00 AM"},
        { "EmployeeTargetID": empTarID, "Date": "2/1/2014 12:00:00 AM"},
        { "EmployeeTargetID": empTarID, "Date": "3/1/2013 12:00:00 AM"},
        { "EmployeeTargetID": empTarID, "Date": "4/1/2012 12:00:00 AM"},
        { "EmployeeTargetID": empTarID, "Date": "5/1/2011 12:00:00 AM"},
        { "EmployeeTargetID": empTarID, "Date": "6/1/2010 12:00:00 AM"},
        { "EmployeeTargetID": empTarID, "Date": "7/1/2009 12:00:00 AM"},
        { "EmployeeTargetID": empTarID, "Date": "8/1/2008 12:00:00 AM"},
        { "EmployeeTargetID": empTarID, "Date": "9/1/2007 12:00:00 AM"},
        { "EmployeeTargetID": empTarID, "Date": "10/1/2006 12:00:00 AM"},
        { "EmployeeTargetID": empTarID, "Date": "11/1/2005 12:00:00 AM"},
        { "EmployeeTargetID": empTarID, "Date": "12/1/2004 12:00:00 AM"}


    ]

    return targetData;

}

function getNonRevHourID(obj) {

    var row = $(obj).closest('tr');
    var id = row.attr('id');
    return id;
}

function createNonRevHourDatas(targetDataIDs, nonRevHourID) {
    var nonRevHours = [

        { "TargetDataID": targetDataIDs[0], "NonRevenueHourID": nonRevHourID, "Value": values[0] },
        { "TargetDataID": targetDataIDs[1], "NonRevenueHourID": nonRevHourID, "Value": values[1] },
        { "TargetDataID": targetDataIDs[2], "NonRevenueHourID": nonRevHourID, "Value": values[2] },
        { "TargetDataID": targetDataIDs[3], "NonRevenueHourID": nonRevHourID, "Value": values[3] },
        { "TargetDataID": targetDataIDs[4], "NonRevenueHourID": nonRevHourID, "Value": values[4] },
        { "TargetDataID": targetDataIDs[5], "NonRevenueHourID": nonRevHourID, "Value": values[5] },
        { "TargetDataID": targetDataIDs[6], "NonRevenueHourID": nonRevHourID, "Value": values[6] },
        { "TargetDataID": targetDataIDs[7], "NonRevenueHourID": nonRevHourID, "Value": values[7] },
        { "TargetDataID": targetDataIDs[8], "NonRevenueHourID": nonRevHourID, "Value": values[8] },
        { "TargetDataID": targetDataIDs[9], "NonRevenueHourID": nonRevHourID, "Value": values[9] },
        { "TargetDataID": targetDataIDs[10], "NonRevenueHourID": nonRevHourID, "Value": values[10] },
        { "TargetDataID": targetDataIDs[11], "NonRevenueHourID": nonRevHourID, "Value": values[11] }

    ]

    return nonRevHours;
}

function saveClick(obj) {
    readValues(obj);
    
    var nonRevName = getNonRevName(obj);
    var empID = getEmployeeID();
    var nonRevHourID = getNonRevHourID(obj);
    
    var destination = '/EmployeeEdit/GetEmployeeTargetID';
    var rev = createEmpTarIdObject(empID);
    var empTarID = ajaxCall(destination, rev);
    
    if (nonRevName === "TotalRevenueHours")
    {
        
        var targetDatas = createTargetData(empTarID);
        destination = '/EmployeeEdit/EditTargetData';
        ajaxCall(destination, targetDatas);
        
    }
    else
    {
        var targetData = createTargetIdObj(empTarID);
        destination = '/EmployeeEdit/AllTargetDataID';
        var targetDataIDs = ajaxCall(destination, targetData);

        var nonRevHours = createNonRevHourDatas(targetDataIDs, nonRevHourID);
        destination = '/EmployeeEdit/EditNonRevHours';
        ajaxCall(destination, nonRevHours);

    }
}

function readValue(tableName, id)
{
    var value = $('.' + tableName).find('#' + id).val();

    return value;
}

function editEmpSalary(budgetSalary, hourlyRate) {

    var data = { "CurrentBudget": budgetSalary, "HourlyRate": hourlyRate, "EmployeeID": getEmployeeID(), "URL": window.location.toString() };
    var destination = '/EmployeeEdit/EditEmpSal';

    ajaxCallParameter(destination, data);

}

function editEmpInfo(departmentID, typeID, startDate, endDate)
{
    var data = { "DepartmentID": departmentID, "TypeID": typeID, "StartDate": startDate, "EndDate": endDate, "EmployeeID": getEmployeeID(), "URL": window.location.toString() };
    var destination = '/EmployeeEdit/EditEmpInfo'

    ajaxCallParameter(destination, data);
}

function editEmpRRSP(RRSP)
{
    var data = { "EmployeeID": getEmployeeID(), "Selected": RRSP, "URL": window.location.toString() };
    var destination = '/EmployeeEdit/EditEmpRRSP'

    ajaxCallParameter(destination, data);
}

function editEmpBenefit(benefitID, vacationID, stdID)
{
    var data = { "EmployeeID": getEmployeeID(), "Benefit": benefitID, "Vacation": vacationID, "STD": stdID, "URL": window.location.toString() }
    var destination = '/EmployeeEdit/EditEmpBenefits'

    ajaxCallParameter(destination, data);
}

function editEmpRaise(raiseDate, raiseValue, raisePercent, old)
{
    var data = { "EmployeeID": getEmployeeID(), "Date": raiseDate, "Value": raiseValue, "isPercent": raisePercent, "oldSal": old, "URL": window.location.toString() }
    var destination = '/EmployeeEdit/EditEmpRaise'

    ajaxCallParameter(destination, data);
}

function editEmpTargetHours(targetHours)
{
    var data = { "EmployeeID": getEmployeeID(), "TargetHours": targetHours, "URL": window.location.toString() }
    var destination = '/EmployeeEdit/EditEmpTargetHours'

    ajaxCallParameter(destination, data); 
}

function readEmpInfo()
{
    var budgetSalary = readValue("salary", "Current_Salary").replace(/\$|,/g, '');

    var hourlyRate = readValue("salary", "hourly_rate").replace(/\$|,/g, '');
    editEmpSalary(budgetSalary, hourlyRate);
    
    var departmentID = readValue("information", "informationTable_DepartmentID");
    var typeID = readValue("information", "informationTable_TypeID");
    var startDate = readValue("information", "informationTable_StartDate");
    var endDate = readValue("information", "informationTable_EndDate");

    editEmpInfo(departmentID, typeID, startDate, endDate);

    var RRSP = readValue("deduction", "rrspID");

    editEmpRRSP(RRSP);

    var benefitName = $('input[name=deductionID]:checked').val();
    var benefitRow = $('input[name=deductionID]:checked').closest('tr').next().find('#' + benefitName);

    var benefitID = benefitRow.find('#deductionID').val();
    alert(benefitID);
    var vacationID;
    var stdID;

    if ($('#hasVacation').is(':checked')) {
        var vacationLocation = $('#hasVacation').closest('tr').next().find('#vacation');
        vacationID = vacationLocation.find('#BenefitTable').val();
    }

    if ($('#hasSTD').is(':checked')) {

        var stdLocation = $('#hasVacation').closest('tr').next().find('#std');
        stdID = stdLocation.find('#BenefitTable').val();

    }

    editEmpBenefit(benefitID, vacationID, stdID);

    var raiseDate = $('.raise').find('#newRaiseDate').find('input').val();
    var raiseValue = $('.raise').find('#newRaiseValue').find('input').val();
    var raisePercent = false;
    var old = $('.salary').find("td:nth-child(4)").html().replace(/\$|,/g, '');

    if ($('#raise').is(':checked'))
    {
        raisePercent = true;
    }

    editEmpRaise(raiseDate, raiseValue, raisePercent, old);

    if ($('#hour').length) {
        
        var targetHours = $('#hour').val();

        editEmpTargetHours(targetHours);
    }
   
    
}

function errorMessage(result) {
    if (result != "") {
        alert(result);
    }
}

function ajaxCallParameter(destination, data) {
    var returnValue;

    $.ajax({

        url: destination,

        data: JSON.stringify(data),

        type: 'POST',

        cache: false,
        
        dataType: 'json',

        contentType: 'application/json; charset=utf-8',

        success: function (result) {
            returnValue = result;

            errorMessage(returnValue);
        },
        error: function(xhr, status, error, result) {
            
        
        }
    });
    return returnValue;
}


$(document).ready(function () {

    $(function targets() {

        $('.editable').append(EditButtons);

        $('.edit').hide();

        $('.edit-btn, .cancel-btn').on("click", function () {
            var row = $(this).closest('tr');
            row.find('.edit, .display').toggle();

            $('.cancel-btn').on("click", function () {    
                cancel(this);
            });

        });

        $('.edit-btn').on("click", function () {
            addTextBox(this, 3);
           

        });

        $('.save-btn').on("click", function () {

  
            saveClick(this);
            
            
        });

        $('.addTarget').on("click", function () {

            
            createTargets(this);
            
            location.reload();

        });

       


    });

    $(function empInfo() {

        $('.emp-save-btn').on('click', function () {
            
            readEmpInfo();

        });

    });

});