var Loader;

moment.locale("ru");
var curentPostId;
var curentPostTag;
var _incallback;
var pageIndex = 1;
var UrlString = '../../Posts/Index/';
var firstRequest = true;
var noItem = true;
$(window).on("load", function () {
    Loader = $('.Loader');
    $('#criminal').on('click', function () {
        RestaItems()
        curentPostId = $(this).attr('data_tag');
        curentPostTag = $(this).attr('data_persid');
        
        DoPostRequst();

    });
    $('#goodjob').on('click', function () {
        RestaItems()
        curentPostId = $(this).attr('data_tag');
        curentPostTag = $(this).attr('data_persid');
        DoPostRequst();

    });
    function RestaItems() {
        firstRequest = true;
        pageIndex = 1;
        noItem = true;
        $('#end').hide();
    }
});
$(window).scroll(function () {
    if ($('.post-ul').length != 0 && noItem) {
        var hT = $('.post-ul').offset().top,
            hH = $('.post-ul').outerHeight(),
            wH = $(window).height(),
            wS = $(window).scrollTop();
        // don't do it if we have reached last page OR we are still grabbing items
        if (!_incallback) {
            if (wS > (hT + hH - wH)) {
                DoPostRequst();
            }
        }
    }
});
function DoPostRequst() {
    _incallback = true;
    Loader.show();

    $.ajax({
        type: 'GET',
        url: UrlString + curentPostId,
        data: {
            "TagId": curentPostTag,
            "page": pageIndex
        },
        dataType: 'html',
        success: function (data) {
            if (data != null) {
                var html = $.parseHTML(data);

                if (firstRequest) {
                    PostOnLoad(data)
                    firstRequest = false;
                    pageIndex++;
                    var isEnd = $(html).hasClass('no-item');
                    if (isEnd) {
                        noItem = false;
                    }
                }
                else
                {
                    var isEnd = $(html).hasClass('no-item');
                    if (isEnd) {
                        noItem = false;
                        setTimeout(function () {
                            Loader.hide();
                            $('#end').show();
                        }, 1000);
                    }
                    else {
                        PostOnLoad(data)
                        pageIndex++;
                    }
                }
            }
        },
        beforeSend: function () {

        },
        complete: function () {
            
            _incallback = false;
        },
        error: function () {
            //alert("Error while retrieving data!");
            _incallback = false;
        }
    })
}

function StartOnLoad() {
    $('#end').hide();
    $('#main-content').empty();
    Loader.show();
}
function PostOnLoad(data) {
    var PostContent = GetPostContainer();
    var content = $('#main-content');
    var html =  $(data);
    TimePass(html);
   
    if (firstRequest) {
        content.empty();
        PostContent.empty();
    }
    PostContent.append(html);
    
    setTimeout(function () {
        Loader.hide();
        content.append(PostContent);
        PostContent.imagesLoaded(function () {
            ResizeElements(html);
        });
    },1000)
}
function GetPostContainer() {
    var postUl = $(".post-ul");
    if (postUl.length==0) {
       return $('<ul class="post-ul"></ul>')
    }
    return postUl;
}
function EndOnLoad(data) {
    setTimeout(function () {
        var content = $('#main-content');
        content.empty();
        Loader.hide();
        content.append(data.responseText);
        TimePass(content);
    }, 1000);

}
function tabClick(event) {
    // Declare all variables
    var tablinks;
    // Get all elements with class="tablinks" and remove the class "active"
    $(".tabs > li").removeClass("li-tab-active");

    $(this).addClass("li-tab-active");
}
function TimePass(content) {
    content.find("span.date").each(function () {
        var postTime = $(this).text();
        var passtime = moment(postTime, "DD.MM.YYYY, h:mm:ss").fromNow();
        $(this).text(passtime);
    });
}