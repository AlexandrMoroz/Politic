var MaxSize = 10000000;
var TextArray = new Object();
var FileArray = new Object();
var Order = new Object();
$(window).on("load", function () {
    $('.add-text').click(function () {

        var index = rand();
        var textInput = $('<textarea class="text-input" id="' + index + '" placeholder="Введите текст"></textarea>');
        var deleteOption = $('<i class="delete-option  fa-times fa "></i>');
        Order[index] = { key: "text", value: textInput[0] };
        deleteOption.click(function () {
            var parent = $(this).parent();
            parent.remove();
            delete Order[textInput[0].getAttribute('id')];
        });

        textInput.keyup(function (e) {
            this.style.height = "1px";
            this.style.height = (14 + this.scrollHeight) + "px";
        });
        var container = $('<div class="element-container text">').append(deleteOption, textInput);
        $('#tab-content').append(container);
    });
    $('.add-img').click(function () {
        var index = rand();
        var inputFile = $('<input type="file" id="' + index + '" class="form-content order" />');
        var deleteOption = $('<i class="delete-option  fa-times  fa "></i>');


        deleteOption.click(function () {
            var parent = $(this).parent();
            parent.remove();
            inputFile.remove();
            delete FileArray[inputFile[0].getAttribute('id')];
            delete Order[inputFile[0].getAttribute('id')];
        });
        var imgView = $('<div class="img-output"></div>');
        var container = $('<div class="element-container img">').append(deleteOption, imgView);


        inputFile.change(function () {
            var file = inputFile[0].files[0];
            if (file.size > MaxSize) {
                imgView.append("<h2>Привышен максимальный размер файла</h2>");
                $('#tab-content').append(container);
                return;
            }
            var reader = new FileReader();
            var img = $('<img />');

            reader.onload = function () {
                img.attr("src", reader.result);

                FileArray[index] = inputFile[0];
                Order[index] = { key: "img", value: file.name };
            };

            if (file) {
                reader.readAsDataURL(file);
            }


            imgView.append(img);
            $('#tab-content').append(container);
        });
        inputFile.click();

    });
    $('#submit').click(function (e) {
        e.preventDefault();

        var Title = $('#title').val();
        var Tags = $('#tags').val();
        var form = $('#PostForm');
        var formData = new FormData(form.get(0));
        formData.append('Title', Title);
        formData.append('Tags', Tags);
        formData.append('PostType', form.children().eq(1));
        formData.append('PersonId', form.children().eq(2));

        var counter = 0;
        var fileCounter = 0;

        for (var i in Order) {

            formData.append('order[' + counter + '].key', Order[i].key + counter);
            if (Order[i].key === 'img') {
                formData.append('order[' + counter + '].value', Order[i].value);
ч            }
            else {
                formData.append('order[' + counter + '].value', Order[i].value.value);
            }
            counter++;

        }
        fileCounter = 0;
        for (var i in FileArray) {
            formData.append('files', FileArray[i].files[0]);
        }
        $.ajax({
            type: "POST",
            url: 'Create',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            headers: {
                '__RequestVerificationToken': form.get(0).value
            },
            success: function (response) {
                if (getResponseValidationObject(response)) {
                    CheckValidationErrorResponse(response, form);
                }
                else {
                    var url = "/Posts/Details/" + response.PostId + "?TagId=" + response.TagId + "&message=" + response.message;
                    window.location.href = url;
                }
            },
            error: function (error) {
                alert(error);
            }
        });
    })
});
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
function rand() {
    var d = new Date();
    var n = d.getTime();
    return n.toString(36).substring(2, 15) + n.toString(36).substring(2, 15)
}

function getValidationSummary() {
    var el = $(".validation-summary-errors");
    if (el.length == 0) {
        $("#title").before("<div class='validation-summary-errors text-danger'></div>");
        el = $(".validation-summary-errors");
    }
    return el;
}

function getResponseValidationObject(response) {
    if (response && response.Tag && response.Tag == "ValidationError")
        return response;
    return null;
}

function CheckValidationErrorResponse(response, form ) {
    var data = getResponseValidationObject(response);
    if (!data) return;

    var SummaryErrors = getValidationSummary();
    SummaryErrors.html('');
    var list = $('<ul></ul>').appendTo(SummaryErrors);
    $.each(data.State, function (i, item) {
        list.append("<li>" + item.Errors.join("</li><li>") + "</li>");
        if (form && item.Name.length > 0)
            $(form).find("*[name='" + item.Name + "']").addClass("ui-state-error");
    });
    HideMessage();
}

function getSuccessMessage() {
    var el = $("<p class='text-success'></div>");
    $("#title").before(el);
    return el;
}
function AddSuccessMessage(response) {
    var SuccessMessage = getSuccessMessage();
    SuccessMessage.html(response.message);
    HideMessage();
} 
