//// Write your Javascript code.
$(function () {
    $('.vp-datetime').each(function () {
        $(this).datetimepicker({
            //defaultDate: start.toDateString(),
            format: 'MM.D.YYYY',
            locale: 'ru'
        });
        //try {
        //    var text = $(this).html() + ' UTC'; //Append ' UTC'

        //    var n = new Date(text);

        //    var dateStr = n.getDate() + "/" + n.getMonth() + "/" + n.getFullYear();
        //    $(this).html(dateStr + " " + n.toLocaleTimeString());

        //    $(this).attr("title", "Converted from UTC " + text);
        //}
        //catch (ex) {
        //    console.warn("Error converting time", ex);
        //}
    });
});


function goBack() {
    window.history.back();
}