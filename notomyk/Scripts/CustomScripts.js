
$(document).ready(function () {
    $(document).on('click',
        '#editTags',
        function (event) {

            var $tInput = $('#SingleNews .bootstrap-tagsinput')
            $('#endEditTags').addClass('hidden');
            if ($tInput.hasClass('tagBG')) {
                $tInput.removeClass('tagBG');
            } else {
                $tInput.addClass('tagBG');
            }
        });
});

$(document).ready(function () {
    $(document).on('click',
        '#endEditTags',
        function (event) {
            $('#editTags').addClass('hidden');
        });
});

$.fn.clickToggle = function (func1, func2) {
    var funcs = [func1, func2];
    this.data('toggleclicked', 0);
    this.click(function () {
        var data = $(this).data();
        var tc = data.toggleclicked;
        $.proxy(funcs[tc], this)();
        data.toggleclicked = (tc + 1) % 2;
    });
    return this;
};


var allowEdit = function () {
    $(".bootstrap-tagsinput").find('input').addClass("tagsInput");
    $(".bootstrap-tagsinput > span > span").addClass("tagsSpan");

    $("#editTags").removeClass("tags");
    $("#editTags").addClass("save");
};

var endEdit = function () {
    $(".bootstrap-tagsinput").find('input').removeClass("tagsInput");
    $(".bootstrap-tagsinput > span >span").removeClass("tagsSpan");

    $("#editTags").removeClass("save");
    $("#editTags").addClass("tags");

    $tagsList = $('#commaDelimitedTags').val();
    $newsID = $('.frmNewsID').val();

    $.ajax({
        url: '/Tag/Add',
        type: 'POST',
        //timeout: 3000,
        data: {
            newsID: $newsID,
            tagsList: $tagsList
        },
        error: function () {
            eventNotification('Tagowanie.', 'Wystąpił błąd podczas edytowania tagów.', 'negative')
        }
    });
};

$("#editTags").clickToggle(allowEdit, endEdit);

function eventNotification(notifiationHeader, notificationMessage, whatKind, delay) {

    $notificationBox = $('.notificationBox');
    whatKind = whatKind || 'positive';
    delay = delay || 3000;

    $('.notificationBox .message').removeClass('positive');
    $('.notificationBox .message').removeClass('negative');

    $('.notificationBox .message').addClass(whatKind);
    $notificationBox.find('.header').text(notifiationHeader);
    $notificationBox.find('p').text(notificationMessage);
    $notificationBox.removeClass('hidden');

    setTimeout(function () {
        $notificationBox.addClass('hidden');
        $notificationBox.find('.header').text('');
        $notificationBox.find('p').text('');
    }, delay
    );
};