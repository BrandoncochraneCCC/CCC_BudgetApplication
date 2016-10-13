var EditButtons = " <td> " +
                    " <input type='button' class='display edit-btn' value='Edit' />" +
                    " <input type='button' class='edit save-btn' value='Save' /> " +
                    " <input type='button' class='edit cancel-btn' value='Cancel' /> " +
                "</td>";

var input = "<input type = 'text' size ='3' value='' class =''/>";

var values = [];

var CreateButton = "<input type='button' class ='create' value='Create New'>";

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

function addTextBox(obj, start) {
    var end = start + 12;
    var row = $(obj).closest('tr');
    var className = "1";
    for (i = start; i < end; i++) {

        var value = row.find("td:nth-child(" + i + ")").html();
        var inputValue = value.replace(/\$|,/g, '');

        if (inputValue == "") {

            row.find("td:nth-child(" + i + ")").html("0");
            inputValue = 0;

        }
        //var inputValueNB = inputValue.replace(/[{()}]/g, '');
        row.find("td:nth-child(" + i + ")").append(input);
        row.find("td:nth-child(" + i + ") input").val(inputValue);

        row.find("td:nth-child(" + i + ") input").addClass(className);
        className += "1";
    }

}

function readValues(obj) {

    var row = $(obj).closest('tr');
    var id = row.find('.name').attr('id');
    var className = "1";
    for (i = 0; i < 12; i++) {
        values[i] = row.find('.' + className).val();
        className += "1";
    }
    return id;
}

function createSerExp(values, id) {

    var service = [
   { "ServiceExpenseID": id, "Value": values[0], "Date": "1/1/2015 12:00:00 AM" },
   { "ServiceExpenseID": id, "Value": values[1], "Date": "2/1/2014 12:00:00 AM" },
   { "ServiceExpenseID": id, "Value": values[2], "Date": "3/1/2013 12:00:00 AM" },
   { "ServiceExpenseID": id, "Value": values[3], "Date": "4/1/2012 12:00:00 AM" },
   { "ServiceExpenseID": id, "Value": values[4], "Date": "5/1/2011 12:00:00 AM" },
   { "ServiceExpenseID": id, "Value": values[5], "Date": "6/1/2010 12:00:00 AM" },
   { "ServiceExpenseID": id, "Value": values[6], "Date": "7/1/2009 12:00:00 AM" },
   { "ServiceExpenseID": id, "Value": values[7], "Date": "8/1/2008 12:00:00 AM" },
   { "ServiceExpenseID": id, "Value": values[8], "Date": "9/1/2007 12:00:00 AM" },
   { "ServiceExpenseID": id, "Value": values[9], "Date": "10/1/2006 12:00:00 AM" },
   { "ServiceExpenseID": id, "Value": values[10], "Date": "11/1/2005 12:00:00 AM" },
   { "ServiceExpenseID": id, "Value": values[11], "Date": "12/1/2004 12:00:00 AM" },

    ];

    return service;

}

function ajaxCallParameter(destination, data, url) {
    var returnValue;

    $.ajax({

        url: destination,

        data: JSON.stringify({ ServExp: data, Name: url }),

        type: 'POST',

        cache: false,

        dataType: 'json',

        contentType: 'application/json; charset=utf-8',

        success: function (result) {
            returnValue = result;

            if (returnValue != "") {
                alert(returnValue);
            }
        },
        error: function (xhr, status, error) {


        }
    });
    return returnValue;
}

$(document).ready(function () {

    $('.editable').parent().append(EditButtons);

    $('.edit').hide();

    $('.edit-btn, .cancel-btn').on("click", function () {
        var tr = $(this).parents('tr:first');
        tr.find('.edit, .display').toggle();

        $('.cancel-btn').on("click", function () {
            cancel(this);
        });
    });

   

    $(function edit() {
        $('.edit-btn').on('click', function () {

            addTextBox(this, 3);

        });

        $('.save-btn').on('click', function () {

            var id = readValues(this);
            var SerExp = createSerExp(values, id);
            var destination = '/ServiceExpenses/EditServExp';
            var url = window.location.toString();
            ajaxCallParameter(destination, SerExp, url);
            location.reload();
        });
       
    });

});