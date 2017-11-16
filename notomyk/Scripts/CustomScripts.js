//$(document).ready(function () {
//    $(".dropdown-toggle").dropdown();
//});

//$(function () {
//    $('input, select').on('change', function (event) {
//        var $element = $(event.target),
//          $container = $element.closest('.example');
//        if (!$element.data('tagsinput'))
//            return;
//        var val = $element.val();
//        if (val === null)
//            val = "null";
//        $('code', $('pre.val', $container)).html(($.isArray(val) ? JSON.stringify(val) : "\"" + val.replace('"', '\\"') + "\""));
//        $('code', $('pre.items', $container)).html(JSON.stringify($element.tagsinput('items')));
//    }).trigger('change');
//});


$(document).ready(function () {
    $(document).on('click',
        '#editTags',
        function (event) {
            $('#endEditTags').addClass('hidden');
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

};

var endEdit = function () {

    $(".bootstrap-tagsinput").find('input').removeClass("tagsInput");
    $(".bootstrap-tagsinput > span >span").removeClass("tagsSpan");


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
            alert("nie mozna zapisac tagow");
        }
    });


};

$("#editTags").clickToggle(allowEdit, endEdit);