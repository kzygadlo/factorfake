$(document).ready(function () {

    tinymce.init({
        selector: "#topicDescription",
        encoding: "xml"
    });

    //Add Topic
    //$("#addArticle").click(function () {

    //    tinyMCE.triggerSave(true, true);

    //    event.preventDefault();
    //    var $catID = $('#forumCategoryID').val();
    //    var $subject = $('#topicSubject').val();
    //    var $description = $('#topicDescription').val();

    //    ajaxAddTopic($catID, $subject, $description)
    //});

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
