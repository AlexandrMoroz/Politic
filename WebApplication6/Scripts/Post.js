function ResizeElements(post) {

    var desc = post.find(".post-description");
    desc.each(function () {
        var h = $(this).height();
        if (h > 500) {
            $(this).css('max-height', 400);
            $(this).find(".post-description-content").css('max-height', 400);
            $(this).append($("<div class='text-overlay'>Показать полностью</div>"))
        }
    });
    var text = $('.text-overlay');
    text.each(
        function () {
            $(this).click(function () {
                $(this).css('display', 'none').css('visibility', 'hidden');
                $(this).parent().css('max-height', 'fit-content');
                $(this).parent().find(".post-description-content").css('max-height', 'fit-content');
            }
            );
        });
}