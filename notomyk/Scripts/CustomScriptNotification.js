$(document).ready(function () {


    $("#frmCommentText").MaxLength({
        MaxLength: 3000,
        CharacterCountControl: $('.charCounter')
    });



});


function showNotification(whatKind, notificationHeader, notificationMessage, $whereAppend, delay) {

    var $template = $('#notificationTemplateJQ').html();

    whatKind = whatKind || 'positive';
    delay = delay || 3000;

    var notificationVariables =
    {
        Class: whatKind,
        Header: notificationHeader,
        Message: notificationMessage
    };

    var html = Mustache.to_html($template, notificationVariables);
    $(html).hide().appendTo($whereAppend).fadeIn('slow');
    //$(html).hide().prependTo($whereAppend).fadeIn('slow');

    $('.message .close').on('click', function () {
        $(this)
          .closest('.message')
          .transition('fade')
        ;
    });
    $('.notificationBox').focus();

    setTimeout(function () {
        $('.notificationBox').remove();
    }, delay
    );
};

