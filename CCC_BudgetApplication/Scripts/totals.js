
function rowTotal() {
    //iterate through each row in table
    $('tr').each(function () {

        //running total of each row
        var sum = 0;
        //get elements and sum them
        $(this).find('.data').each(function () {
            var data = $(this).text().replace(/\$|,/g, '');
            if(data.indexOf('(') >= 0){
                data = data.replace(/[{()}]/g, '');
                data = data * -1;
                }
            if (!isNaN(data) && data.length != 0) {
                sum += parseFloat(data);

            }
            
        });
        if ($(this).hasClass('hour')) {
            $('.total', this).html(HourFormatted(sum.toFixed(0)));
        }
        else if ($(this).hasClass('hourSubTotal')) {
            $('.total', this).html(HourFormatted(sum.toFixed(0)));
        }
        else {
            if (parseFloat(sum) < 0) {
                $('.total', this).html(negativeFormatted(sum.toFixed(0)).replace(/-/g, ''));

            }
            else {
                $('.total', this).html(CommaFormatted(sum.toFixed(0)));

            }
        }
        //set value into element
    });
}

function avgTotal() {
    $('tr').each(function () {

        //running total of each row
        var sum = 0;
        var count = 0;
        //get elements and sum them
        $(this).find('.average').each(function () {
            var data = $(this).text();
            if (!isNaN(data) && data.length != 0) {
                sum += parseFloat(data);
                count++;
            }

        });

        sum /= count

        //set value into element
        $('.avgTotal', this).html(CommaFormatted(sum.toFixed(0)));
    });

}

function hourTotal() {
    $('tr').each(function () {

        //running total of each row
        var sum = 0;
        var count = 0;
        //get elements and sum them
        $(this).find('.hour').each(function () {
            var data = $(this).text().replace(/\$|,/g, '');
            if (!isNaN(data) && data.length != 0) {
                sum += parseFloat(data);
                count++;
            }

        });

        sum /= count

        //set value into element
        $('.total', this).html(CommaFormatted(sum.toFixed(0)));
    });

}

function percentTotal() {
    $('tr').each(function () {

        //running total of each row
        var sum = 0;
        var count = 0;
        //get elements and sum them
        $(this).find('.percent').each(function () {
            var data = $(this).text();
            if (!isNaN(data) && data.length != 0) {
                sum += parseFloat(data);
                count++;
            }

        });

        sum /= count
        var text = "" + sum.toFixed(1) + "%";
        if (text == "NaN%") {
            text = "0%";
        }

        //set value into element
        $('.percentTotal', this).html(text);
    });
}


function tableTotal() {
    var sum = 0;
    $(".total").each(function () {
        var data = $(this).text().replace(/\$|,/g, '');
        if (!isNaN(data) && data.length != 0) {
            sum += parseFloat(data);
        }
    });
    $(".total").text(CommaFormatted(sum.toFixed(0)));
}

function colTotal(table) {
    var tableTotal = 0;
    for (var i = 1; i < 13; i++) {
        var sum = 0;
        var column = ".col" + i;
        $(table).find(column).each(function () {
            var data = $(this).text().replace(/\$|,/g, '');
            if (!isNaN(data) && data.length != 0) {
                sum += parseFloat(data);
            }
        });
        var element = ".colTotal" + i;
        $(table).find(element).text(CommaFormatted(sum.toFixed(0)));
        tableTotal += sum;
    }
    $(table).find(".tableTotal").text(CommaFormatted(tableTotal.toFixed(0)));
}
function hourTotal(row) {
    var tableTotal = 0;
    for (var i = 1; i < 13; i++) {
        var column = ".colTotal" + i;
        $(row).find(column).each(function () {
            var data = $(this).text().replace(/\$|,/g, '');
            $(this).html(data);
            if (!isNaN(data) && data.length != 0) {
                $(this).html(data);
                tableTotal += parseFloat(data);
            }
        });
    }
    $(".tableTotal").text(HourFormatted(tableTotal.toFixed(0)));

}

function scrollBar() {
        $.fn.hasScrollBar = function () {
            return this.get(0).scrollWidth > this.get(0).clientWidth;
        }
   
}

function hideEmptyLine() {
    $('.total').each(function () {
        var data = $(this).text().replace(/\$|,/g, '');
        if (data == 0) {
            $(this).parent().toggle();
        }
    });

    $('.hide-btn').toggle();
    $('.show-btn').toggle();

}


$(document).ready(function () {
    rowTotal();
    percentTotal();
    avgTotal();
    $("table").each(function () {
        colTotal(this);
    });
    $(".hourTotal").each(function () {
        hourTotal(this);

    });
    $('.btn-year-submit').on("click", function () {
        window.location.reload();
    });

    $('.hide-btn').on("click", function () {
        hideEmptyLine(this);
    });
    $('.show-btn').on("click", function () {
        hideEmptyLine();
    });
    $('#hideRightMenu').on("click", function () {
        $('.hideThis').toggle();
    });
    


});
