$(window).on("load", function () {
    var wraper = $('.post-wrapper');
    TimePass(wraper);

    $("#SingIn").click(function () {
        $("#SingInBlock").css("visibility", "visible");
        $(".menu-overlay").css("visibility", "visible");
    });

    $("#MenuClose").click(function () {
        $("#SingInBlock").css("visibility", "hidden");
        $(".menu-overlay").css("visibility", "hidden");
    });
    HideMessage();
    if (location.pathname.includes("Details")) {
       
        $("#ajaxInfo").click();
    }
    $(".tabs > li").click(tabClick);
   
    $(".fa-times").each(function () {
        $(this).click(function () {
            var form = $(this).parent(".form-tag");
            form.submit();
        });
    });
    $(".people-text-input").keyup(function (e) {
        this.style.height = "1px";
        this.style.height = (14 + this.scrollHeight) + "px";
    });

    $('.slider').slick({
        dots: false,
        infinite: false,
        variableWidth: true,
        speed: 300,
        slidesToShow: 4,
        slidesToScroll: 1,
        prevArrow: "<i class='fa fa-arrow-circle-left prev-but'></i>",
        nextArrow: "<i class='fa fa-arrow-circle-right next-but'></i>",
    });


});
function HideMessage(){
    var messege = $('.text-danger > ul>li ').html();
    if (messege === "") {
        $('.text-danger').css("visibility", "hidden");
        $('.text-danger').css("display", "none");
    }
    else {
        $('.text-danger').css("visibility", "visible");
        $('.text-danger').css("display", "block");
    }

    messege = $('.text-success').html();
    if (messege === "") {
        $('.text-success').css("visibility", "hidden");
        $('.text-success').css("display", "none");
    }
    else {
        $('.text-success').css("visibility", "visible");
        $('.text-success').css("display", "block");
    }
}
function ChengeColorLike(flag,id) {
    if (flag){
        $("#Like" + id).addClass("green");
    } else {
        $("#Like" + id).removeClass("green");
    }
}
function ChengeColorDisLike(flag, id ) {
    if (flag) {
        $("#DisLike" + id).addClass("red");
    } else {
        $("#DisLike" + id).removeClass("red");
    }
}
function Login(data, status) {
        var s = window.location.origin;
        window.location.replace(s+"/Account/Login" );
}
function Error404(data, status) {
    if (data === "404") {
        var s = window.location.origin;
        window.location.replace(s+"Error");
    }
} 
function LikeSucces(data, status) {
    if (data === "Please Login") {
        Login(data,status);
    }
    else {
        if (data.flag === 0) {
            ChengeColorDisLike(false, data.id);
            ChengeColorLike(false, data.id);
        }
        else if (data.flag === 1) {
            ChengeColorLike(true, data.id);
        }
        document.getElementById("LikeCounter" + data.id).innerHTML=data.rate;
    }
}
function DisLikeSucces(data, status) {
    if (data === "Пожалуйста введите логин и пароль" ) {
        Login(data, status);
    }
    else {
        if (data.flag === 0) {
            ChengeColorDisLike(false, data.id);
            ChengeColorLike(false, data.id);
        }
        else if (data.flag === 2) {
            ChengeColorDisLike(true, data.id);
        }
        document.getElementById("LikeCounter" + data.id).innerHTML=data.rate;
    }
}



