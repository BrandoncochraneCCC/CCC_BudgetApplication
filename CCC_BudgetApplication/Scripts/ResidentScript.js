var _isCreate = false;

var EditButtons = " <td> " +
                    " <input type='button' class='display edit-btn' value='Edit' />" +
                    " <input type='button' class='edit save-btn' value='Save' /> " +
                    " <input type='button' class='edit cancel-btn' value='Cancel' /> " +
                "</td>";


var values = [];

var CreateButton = "<input type='button' class ='create' value='Create New' style='margin-bottom:20px;'>";

var NewResidentTbl = "<div class='col-lg-14' style='overflow:auto;'><table class='table' id='resTbl'>" +

                    "<tr class='col-lg-12'>" + 
                        "<td class='col-lg-1'> First Name: </td>" +
                        "<td class='col-lg-2'> <input type = 'text' value='' class = 'fName'/> </td><td class='col-lg-9' style='border:none;'></td>" +
                        "</tr><tr class='col-lg-12'>" +
                        "<td class='col-lg-1'> Last Name: </td>" +
                        "<td class='col-lg-2'> <input type = 'text' value='' class = 'lName'/> </td> <td class='col-lg-9' style='border:none;'></td>" +
                        "</tr><tr class='col-lg-12'>" +
                        "<td class='col-lg-1'> Start Date: </td>" +
                        "<td class='col-lg-2'> <input type = 'date' value='' class = 'sDate'/> </td> <td class='col-lg-9' style='border:none;'></td>" +
                        "</tr><tr class='col-lg-12'>" +
                        "<td class='col-lg-1'> End Date: </td>" +
                        "<td class='col-lg-2'> <input type = 'date' value='' class = 'eDate'/> </td> <td class='col-lg-9' style='border:none;'></td>" +
                    "</tr>" +

                    "<tr>" +
                        "<td>" +
                            " <input type='button' class='finish-btn' value='Finish' />" +
                            " <input type='button' class='edit cancel-btn' value='Cancel' />" +
                        "</td>" +
                    "</tr>" +

                "</table></div>";

var EditDateButtons = "<td>" +
                   // " <input type='button' class='display edit-date' value='Edit' />" +
                    " <input type='button' class='save-date' value='Save' />" +
                  //  " <input type='button' class='edit cancel-btn' value='Cancel' />" +
                "</td>";
               

function editButtonClick(obj) {

    var input = "<input type = 'text' size ='3' value='' class = 'input'/>";

    row = $(obj).closest("tr");
    var className = "1";

    for (i = 3; i < 15; i++) {

        var value = row.find("td:nth-child(" + i + ")").html();
        var inputValue = value.replace(/\$|,/g, '');
        //var inputValueNB = inputValue.replace(/[{()}]/g, '');
        if (inputValue == "") {
            row.find("td:nth-child(" + i + ")").html("0");
            inputValue = 0;
        }

        row.find("td:nth-child(" + i + ")").append(input);
        row.find("td:nth-child(" + i + ") input").val(inputValue);
        row.find("td:nth-child(" + i + ") input").addClass(className);
        className += "1";

        $('.cancel-btn').on("click", function () {
            cancel(this);

        });
    }
}

function readTypeID(object) {
    var typeName = [];
    var tr = $(object).closest('tr');

    var editName = tr.find('th').html();
    typeName[0] = editName;

    var link = getIDLink(object);
    
    residentID = getResidentID(link);
    alert(residentID);
    typeName[1] = residentID;
    return typeName;

}

function getIDLink(obj) {

    var tr = $(obj).closest('tr');

    var link = tr.parent().parent().prev().find('a').attr('href');
    
    return link;
}

function getResidentID(link) {
    var linkSplitted = link.split('employeeID=');
    var half = linkSplitted[1];
    var halfSplitted = half.split("&");

    return halfSplitted[0];
}

function readCell(row, name) {

    var total = row.find("." + name).val();

    
    return total;
}


function readData(object) {

    var tr = $(object).closest('tr');

    var isSuccess = -1;
    var name = "1";
    for (i = 0; i < 12; i++) {
        var total = readCell(tr, name)
        if (total == "") {
            values[i] = 0;

        }
        else {
            values[i] = total;

        }
        name += "1";
    }
    
}

function saveButtonClick(obj) {

    readData(obj);

    var s = [];
    s = readTypeID(obj);

    var typeNameNoSpace = s[0].replace(/\s/g, '');
    var id = s[1]; 
    
    var destination;
    
    var ResidentList = [
   { "EmployeeID": id, "Date": "1/1/2015 12:00:00 AM" },
   { "EmployeeID": id, "Date": "2/1/2014 12:00:00 AM" },
   { "EmployeeID": id, "Date": "3/1/2013 12:00:00 AM" },
   { "EmployeeID": id, "Date": "4/1/2012 12:00:00 AM" },
   { "EmployeeID": id, "Date": "5/1/2011 12:00:00 AM" },
   { "EmployeeID": id, "Date": "6/1/2010 12:00:00 AM" },
   { "EmployeeID": id, "Date": "7/1/2009 12:00:00 AM" },
   { "EmployeeID": id, "Date": "8/1/2008 12:00:00 AM" },
   { "EmployeeID": id, "Date": "9/1/2007 12:00:00 AM" },
   { "EmployeeID": id, "Date": "10/1/2006 12:00:00 AM" },
   { "EmployeeID": id, "Date": "11/1/2005 12:00:00 AM" },
   { "EmployeeID": id, "Date": "12/1/2004 12:00:00 AM" },
    ];
    

    switch (typeNameNoSpace) {

        case "Targets":
            destination = '/Bursaries/EditTarget';
            for(i = 0; i < 12; i++){
                    ResidentList[i]["Hour"] = values[i];
                }
        break;


        case "Bursary":
            destination = '/Bursaries/EditBursary';
            for (i = 0; i < 12; i++) {
                ResidentList[i]["BursaryValue"] = values[i];
            }
            break;


        case "Clawback":
            destination = '/Bursaries/EditClawback';
            for (i = 0; i < 12; i++) {
                ResidentList[i]["Clawback"] = values[i];
            }
            break;
            
    }
    var URLtoChange = getIDLink(obj);
  
    var fullURL = createURL(URLtoChange);
    
    ajaxCallArray(destination, ResidentList, fullURL);
}

function ajaxCallArray(destination, obj, id) {
    $.ajax({

        url: destination,

        data: JSON.stringify({ ResValue: obj, urlOfPage: id }),

        type: 'POST',

        cache: false,

        contentType: 'application/json; charset=utf-8',

        dataType: 'json',

        success: function (Result) {

        },
        error: function (Result) {

        }
    });
}


function ajaxCall(destination, obj) {
    var data;
    $.ajax({

        url: destination,

        data: JSON.stringify(obj),

        type: 'POST',

        cache: false,
        
        async: false,
        
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


function finishButton(object) {
    
    var tr = $(object).closest('tr');

    var endDateRow = tr.prev();
    var sDateRow = endDateRow.prev();
    var lNameRow = sDateRow.prev();
    var fNameRow = lNameRow.prev();
  
  

    var firstName = readCell(fNameRow, 'fName');
    var lastName = readCell(lNameRow, 'lName');
    var startDate = readCell(sDateRow, 'sDate');
    var endDate = readCell(endDateRow, 'eDate');

    var newEmp = createEmp(firstName, lastName, startDate, endDate);
    alert(newEmp.FirstName+", " +newEmp.StartDate);
    var destination = '/Employees/AddEmp';

    var id = ajaxCallParameter(destination, newEmp);

}




function createEmp(fName, lName, sDate, eDate) {

    var emp =
        {
            "FirstName": fName,
            "LastName": lName,
            "StartDate": sDate,
            "EndDate": eDate,
            "DepartmentID": 1,
            "TypeID": 4,
            "URL": window.location.toString()
        };
    return emp;
}


function editDate(obj) {

    var row = $(obj).closest('tr');

    var input = "<input type = 'text' size ='6' value='' class = ''/>";

    var startDate = "start-date-to-be-passed";
    var endDate = "end-date-to-be-passed";

 //   row.find("td:nth-child(1)").append(input);
 //   row.find("td:nth-child(2)").append(input);

    row.find("td:nth-child(1) input").addClass(startDate);
    row.find("td:nth-child(2) input").addClass(endDate);

    $('.cancel-btn').on("click", function () {
        cancel(this);
    });
}

function cancel(obj) {

    rowCancel = $(obj).closest("tr");
    rowCancel.find(':text').remove();
    var input;
    var current;
    for (i = 3; i < 15; i++) {
        current = rowCancel.find("td:nth-child(" + i + ")");
        input = current.html();
        if (input == 0) {
            current.html("");
        }

    }
}

function saveDate(obj) {

    var row = $(obj).closest('tr');

    var startDate = readCell(row, "start-date-to-be-passed");
    var endDate = readCell(row, "end-date-to-be-passed");

    startDate = startDate + " 12:00:00 AM";
    endDate = endDate + " 12:00:00 AM";

    var empID = getEmpID(obj);
    var destination = '/Employees/EditDate';

    var URLtoChange = getIDLink(obj);
  
    var fullURL = createURL(URLtoChange);


    var empData =
       {
           "EmployeeID": empID,
           "StartDate": startDate,
           "EndDate": endDate,
           "URL": fullURL
       };
   
    ajaxCallParameter(destination, empData);
    
}

function createURL(partialURL)
{
    var URL = window.location.toString();
    var URLchanged = partialURL.replace('/Bursaries', '');
    var fullURL = URL + URLchanged;
    return fullURL;
}


function getEmpID(obj){
    
    var link = getIDLink(obj);
    var residentID = getResidentID(link);

    return residentID;
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

        error: function (xhr, status, error, result) {


        }
    });
    return returnValue;
}


$(document).ready(function () {

    $(function edit() {


        //add edit button to appropriate rows in each table
        $('.table').find('tr:eq(1)').append(EditDateButtons);
        $('.table').find('tr:eq(4)').append(EditButtons);
        $('.table').find('tr:eq(5)').append(EditButtons);
        $('.table').find('tr:eq(7)').append(EditButtons);
        $('.table').find('tr:eq(8)').append(EditButtons);

        //remove save buttons on total tables
        $('.totalTable').find('.save-date').parent().remove();

        $('.edit').hide();
         
        $('.edit-date, .edit-btn, .cancel-btn').on("click", function () {
            var tr = $(this).parents('tr:first');
            tr.find('.edit, .display').toggle();
        });

        $('.edit-btn').on("click", function () {
            editButtonClick(this);
        });

        $('.edit-date').on("click", function () {
            editDate(this);
        });

        $('.save-btn').on("click", function () {
            saveButtonClick(this);
            location.reload();
        });

        $('.save-date').on("click", function () {
            editDate(this);
            saveDate(this);
            location.reload();

        });

    });

    $(function create() {

        $(CreateButton).insertAfter('#top-of-page');

        $('.create').on("click", function () {
            if (!_isCreate) {
                _isCreate = true;
                $(NewResidentTbl).after().insertAfter($(this));

                $('.cancel-btn').on("click", function () {

                    _isCreate = false;
                    $('#resTbl').remove();
                });

                $('.finish-btn').on("click", function () {
                    _isCreate = false;
                    finishButton(this);
                    location.reload();

                });
            }
           
           
        });

    });

});