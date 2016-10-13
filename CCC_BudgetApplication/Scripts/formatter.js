function percentFormatter(str) {
        return parseFloat(str).toFixed(1) + "%";
}

function CommaFormatted(str) {
        return "$" + (str + "").replace(/\b(\d+)((\.\d+)*)\b/g, function (a, b, c) {
            return (b.charAt(0) > 0 && !(c || ".").lastIndexOf(".") ? b.replace(/(\d)(?=(\d{3})+$)/g, "$1,") : b) + c;
        });
}

function negativeFormatted(str) {
    var result = "($" + (str + "").replace(/\b(\d+)((\.\d+)*)\b/g, function (a, b, c) {
        return (b.charAt(0) > 0 && !(c || ".").lastIndexOf(".") ? b.replace(/(\d)(?=(\d{3})+$)/g, "$1,") : b) + c;
    });
    return result + ")";
}

function HourFormatted(str) {
    return (str + "").replace(/\b(\d+)((\.\d+)*)\b/g, function (a, b, c) {
        return (b.charAt(0) > 0 && !(c || ".").lastIndexOf(".") ? b.replace(/(\d)(?=(\d{3})+$)/g, "$1,") : b) + c;
    });
}



