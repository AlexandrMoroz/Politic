function OnTagDelete(data) {
    if (data.error) {
        alert(data.error);
    }
    else {
        $(this).parent().parent("li").remove();
    }
}
