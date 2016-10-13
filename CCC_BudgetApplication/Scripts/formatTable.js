$(document).ready(function () {
    //iterate through each row in table
    eachRow();
});

function eachRow() {
    $('tr').each(function () {
        //get elements and sum them
        if ($(this).hasClass("hour") || $(this).hasClass("hourSubTotal")) {

        }
        else {
            $(this).find('.data').each(function () {
                var data = $(this).text();
                if (!isNaN(data) && data.length != 0) {
                    data = parseFloat(data);
                    if ($(this).hasClass("percent")) {
                        data = data.toFixed(1);
                        $(this).html(percentFormatter(data));
                    }
                    else {
                        data = data.toFixed(0);
                        $(this).html(CommaFormatted(data));
                    }

                }

            });
        }
        
    });
}
       
