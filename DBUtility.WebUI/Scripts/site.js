function onFormLoad() {
    alert($("#back").attr("disabled"));
}

function nextForm() {
    $.ajax({
        url: "/Home/Next",
        type: "post",
        data: $("form").serialize(), //if you need to post Model data, use this
        success: function (result) {
            $("#partial").html(result);
        }
    });
}

function previousForm() {
    $.ajax({
        url: "/Home/Back",
        type: "post",
        data: $("form").serialize(), //if you need to post Model data, use this
        success: function (result) {
            $("#partial").html(result);
        }
    });
}