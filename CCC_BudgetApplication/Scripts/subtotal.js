$(document).ready(function () {

    $("table").each(function () {
        subTotal(this);
    });

});



function subTotal(table) {
    var hour = false;
    
    $(table).find('tr').each(function(){
        if ($(this).hasClass("hourSubTotal")) {
            hour = true;
            return;
        }
    });
        
    
    if(hour){
        hourSubTotal(table);
    }
        var tableTotal = 0;
        for (var i = 1; i < 13; i++) {
            var sub = 0;
            var fee = 0;
            var column = ".fee" + i;
            var element = ".subTotalCol" + i;
            var total = ".colTotal" + i;

            $(table).find(element).each(function () {
                var data = $(this).text().replace(/\$|,/g, '');
                var total = parseFloat(data);

                if (!isNaN(total && data.length != 0)) {
                    sub = parseFloat(total);

                }
            });
            $(table).find(column).each(function () {
                var data = $(this).text().replace(/\$|,/g, '');
                if (!isNaN(data) && data.length != 0) {
                    fee = parseFloat(data);
                };
            });
            var product = sub * fee;
            tableTotal += product;

            $(table).find(total).text(CommaFormatted(product));
        }
        $(table).find(".tableTotal").text(CommaFormatted(tableTotal));
    }
    

function hourSubTotal(table) {
    var tableTotal = 0;
    for (var i = 1; i < 13; i++) {
        var sub = 0;
        var fee = 0;
        var column = ".fee" + i;
        var element = ".hourSubCol" + i;
        var total = ".colTotal" + i;

        $(table).find(element).each(function () {
            var data = $(this).text().replace(/\$|,/g, '');
            var total = parseFloat(data);

            if (!isNaN(total && data.length != 0)) {
                sub = parseFloat(total);

            }
        });
        $(table).find(column).each(function () {
            var data = $(this).text().replace(/\$|,/g, '');
            if (!isNaN(data) && data.length != 0) {
                fee = parseFloat(data);
            };
        });
        var product = sub * fee;
        tableTotal += product;

        $(table).find(total).text(HourFormatted(product));
    }
    $(table).find(".tableTotal").text(CommaFormatted(tableTotal));
}

