$(document).ready(function () {

    var dropZone = $('#dropZone'),
        maxFileSize = 5000000; // максимальный размер файла - 1 мб.
   
    dropZone[0].ondragover = function () {
        dropZone.addClass('hover');
        return false;
    };

    dropZone[0].ondragleave = function () {
        dropZone.removeClass('hover');
        return false;
    };

    dropZone[0].ondrop = function (event) {
        event.preventDefault();
        if (typeof (window.FileReader) === 'undefined' && event.dataTransfer.files[0].size > maxFileSize) {
            dropZone.text('Не поддерживается браузером!');
            dropZone.addClass('error');
        }
        dropZone.removeClass('hover');
        dropZone.addClass('drop');
        document.getElementById('file').files = event.dataTransfer.files;
        var reader = new FileReader();
        reader.onload = function (e) {
            dropZone.html("<img id='img'/>");
            dropZone.css("width", "auto");
            $('#img').attr('src', e.target.result);
        };
        reader.readAsDataURL(event.dataTransfer.files[0]);
        
    };
    
});
