// Write your Javascript code.
$(function () {
    debugger;
    $('.UTCTime').each(function () {
        try {
            var text = $(this).html() + ' UTC'; //Append ' UTC'

            var n = new Date(text);

            var dateStr = n.getDate() + "." + n.getMonth() + "." + n.getFullYear();
            $(this).html(dateStr + " " + n.toLocaleTimeString());

            $(this).attr("title", "Converted from UTC " + text);
        }
        catch (ex) {
            console.warn("Error converting time", ex);
        }
    });
});