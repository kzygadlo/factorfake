
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


    $(".rNews").click(function () {        
        $('.rightTable a').removeClass('activePane');
        $('.forumList .post').removeClass('activePane');
        $(this).addClass('activePane');
        //$('#editTags').addClass('hidden');
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
    $whereAppend = $('#SingleNews');

    $.ajax({
        url: '/Tag/Add',
        type: 'POST',
        //timeout: 3000,
        data: {
            newsID: $newsID,
            tagsList: $tagsList
        },
        error: function () {

            showNotification('negative', 'Tagowanie.', 'Wystąpił błąd podczas edytowania tagów.', $whereAppend)
        }
    });
};

$("#editTags").clickToggle(allowEdit, endEdit);
