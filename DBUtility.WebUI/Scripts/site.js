function onFormLoad() {
    alert($("#back").attr("disabled"));
}

function navigateForm(element) {
    var data = $("form").serializeArray();
    data.push({ name: "direction", value: element.id });
    $.ajax({
        url: "/Home/Navigate",
        type: "post",
        data: data,
        success: function (result) {
            $("#partial").html(result);
        }
    });
}

//function previousForm() {
//    $.ajax({
//        url: "/Home/Back",
//        type: "post",
//        data: $("form").serialize(), //if you need to post Model data, use this
//        success: function (result) {
//            $("#partial").html(result);
//        }
//    });
//}