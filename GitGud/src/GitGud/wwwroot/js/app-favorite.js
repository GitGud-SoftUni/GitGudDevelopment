// app-favorite.js
function favorite(id) {
    let favUrl = "../api/favorite";
    $.ajax({
        type: "POST",
        url: favUrl,
        data: `{"id": "${id}"}`,
        contentType: "application/json",
        success: function () {
            console.log("success");
            $('#favBox').load(document.URL + ' #favBox');
        },
        error: function (err) {
            console.log(err);
        }
    });
}

$(function () {
    $('.fav-Unfav').click(function () {
        var obj = $(this);
        if (obj.data('favorited')) {
            obj.data('favorited', false);
            obj.html('Add to favorites!');
        }
        else {
            obj.data('favorited', true);
            obj.html('Remove from favorites!');
        }
    });
});