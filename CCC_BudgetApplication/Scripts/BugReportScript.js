function ajaxCall(id, destination) {
    var returnValue;

    $.ajax({

        url: destination,

        data: JSON.stringify({Id: id}),

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

    $('.inProgress').on("click", function () {
        var id = $(this).val();
        var destination = '/BugReports/toggleProgress';
        ajaxCall(id, destination);
    });

    $('.resolved').on("click", function () {
        var id = $(this).val();
        var destination = '/BugReports/toggleResolved';
        ajaxCall(id, destination);

    });

});