$(document).ready(function () {


    //Add Topic
    $('.frmAddComment').on('submit', function (event) {
        event.preventDefault();
        var $catID = $('.frmNewsID').val();
        var $subject = $('#frmCommentText');
        var $subject = $("#commentsList");

        ajaxAddComment($comment, $newsID, 0, $template, $commentList)
    });
});

function ajaxAddTopic($catID, $sub, $desc) {
    $.ajax({
        url: '/Forum/Add',
        type: 'POST',
        //timeout: 3000,
        data: {
            catID: $catID,
            sub: $sub,
            desc: $desc
        },
        success: function (response) {
            if (response.success == true) {
                eventNotification(response.errHeader, response.errMessage);
            } else {
                eventNotification(response.errHeader, response.errMessage, 'negative');
            }
        },
        error: function () {
            eventNotification('Dodawanie komentarza.', 'Wystąpił błąd podczas dodawania komentarza.', 'negative')
        }
    });
};
