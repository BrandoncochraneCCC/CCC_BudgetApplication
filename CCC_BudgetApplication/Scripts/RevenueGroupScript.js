
var values = [];

var EditButtons = " <td class='editCell'> " +
                    " <input type='button' class='display edit-btn' value='Edit' />" +
                    " <input type='button' class='edit save-btn' value='Save' /> " +
                    " <input type='button' class='edit cancel-btn' value='Cancel' /> " +
                "</td>";

var CreateButton = "<input type='button' class ='create' value='Create New'>";

var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};

var table;
function ajaxCall(destination, obj, id) {
    $.ajax({

        url: destination,

        data: JSON.stringify({ revValues:obj, urlOfPage:id }),

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

function readData(object) {
    var tr = $(object).parents('tr:first');
    var id = tr.find("span").attr('id');
    
    var isSuccess = -1;
    var name = "1";
    for (i = 0; i < 12; i++) {
        var total = tr.find("." + name);
        if (total.val() == "") {
            values[i] = 0;
        }
        else {
            values[i] = total.val();
        }
        name += "1";
    }
    return id;
}
function editButtonClick(obj) {

    var input = "<input type = 'text' size ='3' value='' class = 'input'/>";

    row = $(obj).closest("tr");
    var className = "1";

    for (i = 3; i < 15; i++) {

        var value = row.find("td:nth-child(" + i + ")").html();
        var inputValue = value.replace(/\$|,/g, '');
        var inputValueNB = inputValue.replace(/[{()}]/g, '');
        if (inputValueNB == "") {
            row.find("td:nth-child(" + i + ")").html("0");
            inputValue = 0;
        }
        row.find("td:nth-child(" + i + ")").append(input);
        row.find("td:nth-child(" + i + ") input").val(inputValueNB);

        row.find("td:nth-child(" + i + ") input").addClass(className);
        className += "1";

        $('.cancel-btn').on("click", function () {

            rowCancel = $(this).closest("tr");
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
        });
    }
}

function saveButtonClick(obj) {
    var id = readData(obj);

    var destination = '/RevenueDatas/EditData';

    var RevenueDatas = [
        { "RevenueID": id, "Value": values[0], "Date": "1/1/2015 12:00:00 AM" },
        { "RevenueID": id, "Value": values[1], "Date": "2/1/2014 12:00:00 AM" },
        { "RevenueID": id, "Value": values[2], "Date": "3/1/2013 12:00:00 AM" },
        { "RevenueID": id, "Value": values[3], "Date": "4/1/2012 12:00:00 AM" },
        { "RevenueID": id, "Value": values[4], "Date": "5/1/2011 12:00:00 AM" },
        { "RevenueID": id, "Value": values[5], "Date": "6/1/2010 12:00:00 AM" },
        { "RevenueID": id, "Value": values[6], "Date": "7/1/2009 12:00:00 AM" },
        { "RevenueID": id, "Value": values[7], "Date": "8/1/2008 12:00:00 AM" },
        { "RevenueID": id, "Value": values[8], "Date": "9/1/2007 12:00:00 AM" },
        { "RevenueID": id, "Value": values[9], "Date": "10/1/2006 12:00:00 AM" },
        { "RevenueID": id, "Value": values[10], "Date": "11/1/2005 12:00:00 AM" },
        { "RevenueID": id, "Value": values[11], "Date": "12/1/2004 12:00:00 AM" },
    ];

    var urlOfPage = window.location.toString();

   
    ajaxCall(destination, RevenueDatas, urlOfPage);

}

$(document).ready(function () {

  

    $(function edit() {


        var CreateLocation = $('.createCell');
        var EditLocation = $('.canEdit').parent();

        EditLocation.append(EditButtons);
        CreateLocation.prepend(CreateButton);

        $('.source').hide();
        $('.edit').hide();

        $('.ins-btn, .edit-btn, .cancel-btn').on("click", function () {
            var tr = $(this).parents('tr:first');
            tr.find('.edit, .display').toggle();

        });


        $('.edit-btn').on("click", function () {           
            editButtonClick(this);
        });

        $('.save-btn').on("click", function () {
            saveButtonClick(this);

            location.reload();
        });
    });

    $(function create() {
        $('.create').on("click", function () {

            var ParRevName = $('h2 a').html();

            var NewRow = "<tr>" +
                            "<td> <input type = 'text' size ='20' value='New Revenue Name' class = 'revName'/> </td>" +
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
            var NewRowLocation = $(this).parent().parent().parent().find("tr").eq(-2);
            NewRowLocation.after(NewRow);

            $('.cancel').on("click", function () {
                rowCancel = $(this).closest("tr");
                rowCancel.empty();
            });

            $('.finish').on("click", function () {

                var tr = $(this).parents('tr:first');
                var RevenueName = tr.find('.revName').val();

                var revValues = [];

                for (i = 0; i < 12; i++) {
                    revValues[i] = tr.find("." + i).val();
                }

                var AboveRevenueID = tr.parent().parent().prev().attr('class');

                    var NewRevID;

                    var NewRev =
                    {
                        "RevenueID": AboveRevenueID,
                        "Name": RevenueName
                    };
                    var urlName = window.location.toString();

                    $.ajax({
                        url: '/Revenues/Add/',

                        data: JSON.stringify({ r: NewRev, url:urlName }),

                        type: 'POST',

                        contentType: 'application/json; charset=utf-8',

                        dataType: 'json',

                        success: function (result) {
                            NewRevID = result;
                        },
                        error: function (result) {
                            NewRevID = result;

                        }

                    }).done(function () {


                        var RevDatas = [
                            { "RevenueID": NewRevID, "Value": revValues[0], "Date": "1/1/2015 12:00:00 AM" },
                            { "RevenueID": NewRevID, "Value": revValues[1], "Date": "2/1/2014 12:00:00 AM" },
                            { "RevenueID": NewRevID, "Value": revValues[2], "Date": "3/1/2013 12:00:00 AM" },
                            { "RevenueID": NewRevID, "Value": revValues[3], "Date": "4/1/2012 12:00:00 AM" },
                            { "RevenueID": NewRevID, "Value": revValues[4], "Date": "5/1/2011 12:00:00 AM" },
                            { "RevenueID": NewRevID, "Value": revValues[5], "Date": "6/1/2010 12:00:00 AM" },
                            { "RevenueID": NewRevID, "Value": revValues[6], "Date": "7/1/2009 12:00:00 AM" },
                            { "RevenueID": NewRevID, "Value": revValues[7], "Date": "8/1/2008 12:00:00 AM" },
                            { "RevenueID": NewRevID, "Value": revValues[8], "Date": "9/1/2007 12:00:00 AM" },
                            { "RevenueID": NewRevID, "Value": revValues[9], "Date": "10/1/2006 12:00:00 AM" },
                            { "RevenueID": NewRevID, "Value": revValues[10], "Date": "11/1/2005 12:00:00 AM" },
                            { "RevenueID": NewRevID, "Value": revValues[11], "Date": "12/1/2004 12:00:00 AM" },
                        ];

                        var url = window.location.toString();

                        $.ajax({
                            url: '/RevenueDatas/EditData',

                            data: JSON.stringify({ revDatas: RevDatas, urlOfPage:url }),

                            type: 'POST',

                            contentType: 'application/json; charset=utf-8',

                            dataType: 'json',

                            success: function (result) {
                                
                            },
                            error: function (result) {
                                
                            }

                        }).done(function () {
                            
                            location.reload();

                        });

                    });

                           
            });

        });

    });
  
});


