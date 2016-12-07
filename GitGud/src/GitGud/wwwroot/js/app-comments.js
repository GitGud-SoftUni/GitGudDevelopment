// app-likes.js
function deleteComment(id) {
    let comUrl = "../api/comment";
    $.ajax({
        type: "POST",
        url: comUrl,
        data: `{"id": "${id}"}`,
        contentType: "application/json",
        success: function () {
            console.log("success, comment was removed");
            $('#comments').load(document.URL + ' #comments');
        },
        error: function (err) {
            console.log(err);
        }
    });
}