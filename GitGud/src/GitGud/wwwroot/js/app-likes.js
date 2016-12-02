// app-likes.js
function like(id) {
    let likeUrl = "http://localhost:54204/api/like";
    $.ajax({
        type: "POST",
        url: likeUrl,
        data: `{"commentId": "${id}"}`,
        contentType: "application/json",
        success: function () {
            console.log("success");
        },
        error: function (err) {
            console.log(err);
        }
    });
}

$(function () {
    $('.like-Unlike').click(function () {
        var obj = $(this);
        if (obj.data('liked')) {
            obj.data('liked', false);
            obj.html('I <i class="fa fa-thumbs-o-up"></i> IT');
        }
        else {
            obj.data('liked', true);
            obj.html('I <i class="fa fa-thumbs-o-down"></i> IT');
        }
    });
});