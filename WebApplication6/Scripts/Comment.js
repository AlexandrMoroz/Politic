function AnswerSucces(data) {
    var parent = $(this).parent();
    var answer = parent.children('.create-comment-container');
    answer.empty();
    answer.append(data);

    $(".textAreaId").keyup(function () {
        this.style.height = "1px";
        this.style.height = (20 + this.scrollHeight) + "px";
    });

    var link = parent.children('.answer-link');
    link.toggle();
    var answerForm = parent.children('.hide-answer-form');

    answerForm.toggle(function () {
        if ($(this).is(':visible'))
            $(this).css('display', 'inline-block');
    });

    answerForm.click(function () {
        if (answer.css('display') !== "none") {
            $(this).text("Открыть");
        } else {
            $(this).text("Закрыть");
        }
        answer.toggle('slow');
    });
}

function CommentsOnsuccess(data) {
    if (data === 'Please Login') {
        setTimeout(function () {
        var message = $("<h2 class='no-item'></h2>").append(data);
            $('.comments').empty().append(message);
        }, 1000);
    } else {
        setTimeout(function () {
            var content = $('.comments');
            content.empty();
            content.append(data);
        }, 1000);
    }
}
function ChildCommentSucces(data, success, requst, currentElem) {
    var parent;
    if (currentElem != undefined) {
        parent = currentElem;

    } else {
        parent = $(this).parent();      
    }
     var responsContainer = $("<div class='child_comment'></div>")
    responsContainer.empty();
    var link = $("<i class= 'fa fa-minus-square-o fa-onHover' aria-hidden='true'></i>");
    parent = $(this).parent();
    link.click(function () {
        $(this).toggleClass("fa-minus-square-o fa-plus-square-o")
       
        if ($(this).hasClass("fa-plus-square-o")) {
            var list = $(this).parent().children("ul").children("li");
            var count = list.length;
            var string = "eще " + count;
            if (count > 1 && count <= 4) {
                string += " коментария"
            } else if (count == 1) {
                string += " коментарий"
            } else {
                string += " коментариев"
            }
           var moreComent = $("<span class='more-coments'>" + string + "</span>");
            $(this).append(moreComent);
        } else {
            $(this).empty();
        }
        $(this).parent().children('ul').toggle();
    });
    responsContainer.append(link);
    responsContainer.append(data);
    TimePass(responsContainer);
    responsContainer.last().hover(function () {
        link.css("color", "#8ac858");
    }, function () {
        link.css("color", "white");
    });
    $(link).hover(function () {
        $(this).parent().toggleClass("onHover");
    });
    parent.append(responsContainer);
    if (parent.children("#child_link_text").css("display") != "none") {
        parent.children("#child_link_text").toggle(); 
    } 
    
 
}

function CreateComment(data, status) {
    if (data.error) {
        Error404(data.Error);
    }
    else {
        var html = $.parseHTML(data.view);
        TimePass($(html));
        if (data.ischild) {
            var parent = $(this).parent().parent().parent().parent();
            var child = parent.children().last();
            if (child.attr("id") == "child_link_text") {
                child.click();
                $(this).find("textarea").val("");
                parent.find("#hide-answer").click();

            }
            else if (child.hasClass("child_comment")) {
                child.empty();
                var a = parent.children("#child_link_text");
                a.click();
                $(this).find("textarea").val("");
                parent.find("#hide-answer").click();
                parent
            }
            else {
                $(this).find("textarea").val("");
                parent.find("#hide-answer").click();
                ChildCommentSucces(html, "success", null, parent)
            }
             

        } else {
            var list = $(this).parents(".comments");
            if (list.children().eq(0).hasClass("no-item")) {
                list.children().eq(0).remove();
                list.prepend(html);
            } else {
                list.children().eq(0).append(html);
            }
            $("#Text").val("");
        }
    }
}

function DeleteComment(data, status) {
    if (data.error) {
        if (data.error == "404") {
            Error404(data.Error);
        }
        else {
            alert(data.error);
        }
        
    }
    else {
        var link = data;
        var parent = $(this).parents(".comments");
        
        $.ajax({
            type: "GET",
            url: link,
            dataType: 'html',
            crossDomain: true,
            success: function (result) {
                $(parent).empty();
                var res = $(result);
                TimePass(res);
                $(parent).append(res);
            },
            error: function (error) {
                $(parent).empty();
                $(parent).append(error.massage);
            }
        });
    }
} 

