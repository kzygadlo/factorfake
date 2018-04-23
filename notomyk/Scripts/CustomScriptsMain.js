
$(document).ready(function () {

    $('.ui.rating').rating();

    getListOfNews();
    $('.ui.dropdown').dropdown();
    $('#search-select').dropdown();

    $('#multi-select')
        .dropdown()
        ;

    $('.newspaperFilter.ui.dropdown').change(
        function () {
            beforeFiltering()
        });

    $('.newspaperFilter.ui.dropdown').dropdown({
        maxSelections: 6
    });

    $('.tagFilter.ui.dropdown').change(
        function () {
            beforeFiltering()
        });

    $('.tagFilter.ui.dropdown').dropdown({
        maxSelections: 6
    });

    $('.whatNewsFilter').dropdown({
        onChange:
        function () {
            beforeFiltering();
        }
    });

    $('.whatPeriodFilter').dropdown({
        onChange:
        function () {
            beforeFiltering();
        }
    });

});

function beforeFiltering(xs) {
    $('#newsContainer').find('li').remove();
    $('#newPageButton').data('id', 0);
    $('#newPageButton').addClass("hidden");
    $('#loadingImage').removeClass("hidden");
    getListOfNews();
}

//Display first 10 rows
function getListOfNews() {
    if ($("#newsContainer").length > 0) {

        $.fn.inc = function (prop, val) {
            return this.each(function () {
                var data = $(this).data();
                if (!(prop in data)) {
                    data[prop] = 0;
                }
                data[prop] += val;
            });
        }

        var $template = $('#newsPattern').html();
        var $newsList = $("#newsContainer");

        var $newspapersList = $('#newspapersList').val();
        var $tagList = $('#tagsList').val();

        var $whatNews = $('.whatNewsFilter').dropdown('get value');
        var $whatPeriod = $('.whatPeriodFilter').dropdown('get value');


        var $page = $('#newPageButton').data('id');

        //Main page filtering
        var $MainPage = $('#MainPage').data('id');


        if ($tagList.length > 0) {
            var arrListTag = $tagList.split(',');
        }
        else {
            var arrListTag = [];
        }

        if ($newspapersList.length > 0) {
            var arrList = $newspapersList.split(',');
        }
        else {
            var arrList = [];
        }

        var model = {
            NewspapersList: arrList,
            TagsList: arrListTag,
            WhatNews: $whatNews,
            Period: $whatPeriod,
            Page: $page,
            MainPage: $MainPage
        };

        $('#newPageButton').val("... wczytywanie ...");
        $.ajax({
            url: '/Main/Get',
            type: "POST",
            data: model,
            success: function (result) {

                $('#loadingImage').removeClass("hidden");
                $.each(result, function (key, val) {
                    var $template = $('#newsPattern').html();

                    var i;
                    var $htmlList = '';
                    for (i = 0; i < val.tagList.length; ++i) {
                        $htmlList = $htmlList + '<span class="ui teal basic label">' + val.tagList[i] + '</span>';
                    }

                    fulfillNewsListTemplate($template, $newsList, val.urlActionLink, val.newspaperPictureLink, val.newsPictureLink, val.newsTitle, val.newsDescription, val.numberOfVisitors, val.numberOfComments, val.dateAdded, val.ratingClass, val.ratingValue, val.newsID, $htmlList, val.faktValue, val.fakeValue, val.manipulatedValue);

                    var resultRemaining = val.remainingRows;

                    if (resultRemaining >= 10) {
                        $('#newPageButton').removeClass("hidden");
                        $('#newPageButton').val("Pokaż kolejne 10 z " + resultRemaining + " pozostałych.");
                    }
                    else if (resultRemaining > 0) {
                        $('#newPageButton').removeClass("hidden");
                        $('#newPageButton').val("Pokaż kolejne " + resultRemaining + " z " + resultRemaining + " pozostałych.");
                    }
                    else {
                        $('#newPageButton').addClass("hidden");
                    }
                });

                $('#loadingImage').addClass("hidden");
                $('#newPageButton').fadeIn('slow');

                if (result.length === 0) {
                    $('#newPageButton').removeClass("hidden");
                    $('#newPageButton').val("Brak wyników dla wybranych filtrów.");
                }

                $('#newPageButton').inc('id', 1);
            },
            error: function () {

                $whereAppend = $('#newsContainer')
                showNotification('negative', 'Newsy', 'Wystąpił błąd podczas wyświetlania newsów.', $whereAppend)
            }
        });
    }
}

//load additional 10 rows
$(document).ready(function () {
    $(document).on('click', '#newPageButton', function () {
        getListOfNews();
    });
});

function fulfillNewsListTemplate(Template, AppendTo, uAl, newsPl, newspaperPl, nTitle, nDescr, numV, numC, dAdded, ratingC, ratingV, newsID, tags, FaktTag, FakeTag, ManipulatedTag) {
    var newsVariables =
        {
            urlActionLink: uAl,
            newspaperPictureLink: newsPl,
            newsPictureLink: newspaperPl,
            newsTitle: nTitle,
            newsDescription: nDescr,
            numberOfVisitors: numV,
            numberOfComments: numC,
            dateAdded: dAdded,
            newsRating: ratingV,
            newsID: newsID,
            tagsList: tags,
            faktTag: FaktTag,
            fakeTag: FakeTag,
            manipulatedTag: ManipulatedTag
        };


    var html = Mustache.to_html(Template, newsVariables);
    var resultsRemaining = 0;

    $(html).hide().appendTo(AppendTo).fadeIn('slow');

    switch (ratingC) {

        case -1:
            $('#newsContainer').find('#' + newsID + '').find('.info > div').addClass("bRedColor");
            $('#newsContainer').find('#' + newsID + '').find('#FakeTag').text("Fake");
            break;

        case 1:
            $('#newsContainer').find('#' + newsID + '').find('.info > div').addClass("bGreenColor");
            $('#newsContainer').find('#' + newsID + '').find('#FaktTag').text("Fakt");
            break;        

        case 2:
            $('#newsContainer').find('#' + newsID + '').find('.info > div').addClass("bGreyColor");
            $('#newsContainer').find('#' + newsID + '').find('#ManipulatedTag').text("Manipulacja");
            break;
    }
}