$('.date').each(function () {
    var postTime = $(this).text();
    var passtime = moment(postTime, "DD.MM.YYYY, h:mm:ss").startOf("day").fromNow();
});