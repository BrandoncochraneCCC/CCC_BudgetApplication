var EditButtons = " <td> " +
                    " <input type='button' class='display edit-btn' value='Edit' />" +
                    " <input type='button' class='edit save-btn' value='Save' /> " +
                    " <input type='button' class='edit cancel-btn' value='Cancel' /> " +
                "</td>";

var input = "<input type = 'text' size ='3' value='' class =''/>";

var values = [];

var newValues = [];

var CreateButton = "<input type='button' class ='create' value='Create New'>";

var NewRow = "<tr>" +
                            "<td> <input type = 'text' size ='20' value='New G&A' class = 'GAName'/> </td>" +
                            "<td> </td>" +
                            "<td> <input type = 'text' size ='6' value='' class = '0'/> </td>" +
                            "<td> <input type = 'text' size ='6' value='' class = '1'/> </td>" +
                            "<td> <input type = 'text' size ='6' value='' class = '2'/> </td>" +
                            "<td> <input type = 'text' size ='6' value='' class = '3'/> </td>" +
                            "<td> <input type = 'text' size ='6' value='' class = '4'/> </td>" +
                            "<td> <input type = 'text' size ='6' value='' class = '5'/> </td>" +
                            "<td> <input type = 'text' size ='6' value='' class = '6'/> </td>" +
                            "<td> <input type = 'text' size ='6' value='' class = '7'/> </td>" +
                            "<td> <input type = 'text' size ='6' value='' class = '8'/> </td>" +
                            "<td> <input type = 'text' size ='6' value='' class = '9'/> </td>" +
                            "<td> <input type = 'text' size ='6' value='' class = '10'/> </td>" +
                            "<td> <input type = 'text' size ='6' value='' class = '11'/> </td>" +
                            "<td>" +
                                "<input type = 'button' value = 'Finish' class = 'finish'>" +
                                "<input type = 'button' value = 'Cancel' class = 'cancel'>" +
                            "</td>"
"</tr>";

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

function readValues(obj)
{

    var row = $(obj).closest('tr');
    var id = row.find('.name').attr('id');
    var className = "1";
    for(i = 0; i<12; i++)
    {
        values[i] = row.find('.' + className).val();
        className += "1";
    }
    return id;
}

function createGAExp(values, id)
{
    var GA = [
   { "GroupID": id, "Value": values[0], "Date": "1/1/2015 12:00:00 AM" },
   { "GroupID": id, "Value": values[1], "Date": "2/1/2014 12:00:00 AM" },
   { "GroupID": id, "Value": values[2], "Date": "3/1/2013 12:00:00 AM" },
   { "GroupID": id, "Value": values[3], "Date": "4/1/2012 12:00:00 AM" },
   { "GroupID": id, "Value": values[4], "Date": "5/1/2011 12:00:00 AM" },
   { "GroupID": id, "Value": values[5], "Date": "6/1/2010 12:00:00 AM" },
   { "GroupID": id, "Value": values[6], "Date": "7/1/2009 12:00:00 AM" },
   { "GroupID": id, "Value": values[7], "Date": "8/1/2008 12:00:00 AM" },
   { "GroupID": id, "Value": values[8], "Date": "9/1/2007 12:00:00 AM" },
   { "GroupID": id, "Value": values[9], "Date": "10/1/2006 12:00:00 AM" },
   { "GroupID": id, "Value": values[10], "Date": "11/1/2005 12:00:00 AM" },
   { "GroupID": id, "Value": values[11], "Date": "12/1/2004 12:00:00 AM" },
   
    ];
  
    return GA;
}


function ajaxCallParameter(destination, data, url) {
    var returnValue;

    $.ajax({

        url: destination,

        data: JSON.stringify({ GAExp:data, Name:url }),

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

function addNewRow(obj)
{
    var row = $(obj).closest('tr');
    var newRowLocation = row.parent().parent().find('tr').eq(-2);

    newRowLocation.after(NewRow);
}

function getParentID(obj)
{
    
    var row = $(obj).closest('tr');

    var parentID = row.parent().parent().prev().attr('class');

    return parentID;
}

function readNewValues(obj)
{
    var row = $(obj).closest('tr');
    var className = 0;
    var GAName = row.find('.GAName').val()

    for (i = 0; i < 12; i++) {
        newValues[i] = row.find('.' + className).val();
        className++;
    }

    return GAName;
}

$(document).ready(function () {

    $(function edit() {
        $('.editable').parent().append(EditButtons);

        $('.edit').hide();

        $('.edit-btn, .cancel-btn').on("click", function () {
            var tr = $(this).parents('tr:first');
            tr.find('.edit, .display').toggle();

            $('.cancel-btn').on("click", function () {
                cancel(this);
            });
        });


        $('.edit-btn').on('click', function () {

            addTextBox(this, 3);

        });

        $('.save-btn').on('click', function () {

            var id = readValues(this);
            var GAExp = createGAExp(values, id);
            var destination = '/GAGroups/EditGAExp';
            var url = window.location.toString();
            ajaxCallParameter(destination, GAExp, url);
            location.reload();

        });
    });

    $(function create() {

        $('.createCell').prepend(CreateButton);

        $('.create').on('click', function () {

            addNewRow(this);

            $('.cancel').on("click", function () {
                rowCancel = $(this).closest("tr");
                rowCancel.empty();
            });
            
            $('.finish').on('click', function () {
                var parentID = getParentID(this);
                var GAName = readNewValues(this);
                var url = window.location.toString();
                url += "*"+GAName;
                
                var ga = createGAExp(newValues, parentID);
                var destination = '/GAGroups/AddGAExp';
                ajaxCallParameter(destination, ga, url);
                location.reload();

            });
        });

        
    });

});