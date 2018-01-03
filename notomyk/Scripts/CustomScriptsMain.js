
$(document).ready(function () {

    $('.ui.rating').rating();

    getListOfNews();
    $('.ui.dropdown').dropdown();
    $('#search-select').dropdown();

    $('.newspaperFilter.ui.dropdown').change(
    function () {
        beforeFiltering();
    });

    $('.tagFilter.ui.dropdown').change(
    function () {
        beforeFiltering();
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

    $(function () {

        $(window).bind("resize", function () {
            console.log($(this).width())
            if ($(this).width() < 991) {
                $('.newspaperFilter').removeClass('search');
                $('.newspaperFilter').find('.search').addClass('hidden');
                $('.newspaperFilter').find('.sizer').addClass('hidden');

                $('.tagFilter').removeClass('search');
                $('.tagFilter').find('.search').addClass('hidden');
                $('.tagFilter').find('.sizer').addClass('hidden');
            }
            else {
                $('.newspaperFilter').addClass('search');
                $('.newspaperFilter').find('.search').removeClass('hidden');
                $('.newspaperFilter').find('.sizer').removeClass('hidden');

                $('.tagFilter').addClass('search');
                $('.tagFilter').find('.search').removeClass('hidden');
                $('.tagFilter').find('.sizer').removeClass('hidden');
            }
        })
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

                    fulfillNewsListTemplate($template, $newsList, val.urlActionLink, val.newspaperPictureLink, val.newsPictureLink, val.newsTitle, val.newsDescription, val.numberOfVisitors, val.numberOfComments, val.dateAdded, val.ratingClass, val.ratingValue, val.newsID, $htmlList, val.faktValue, val.fakeValue);

                    resultRemaining = val.remainingRows;
                });

                $('#loadingImage').addClass("hidden");
                $('#newPageButton').fadeIn('slow');

                if (resultRemaining >= 10) {
                    $('#newPageButton').removeClass("hidden");
                    $('#newPageButton').val("Pokaż kolejne 10 z " + resultRemaining + " pozostałych.");
                }
                else {
                    $('#newPageButton').removeClass("hidden");
                    $('#newPageButton').val("Pokaż kolejne " + resultRemaining + " z " + resultRemaining + " pozostałych.");
                }

                if (result.length == 0) {
                    $('#newPageButton').removeClass("hidden");
                    $('#newPageButton').val("Brak wyników dla wybranych filtrów.");
                }
                else {
                    if (resultRemaining <= 0) {
                        $('#newPageButton').addClass("hidden");
                    }
                }
                $('#newPageButton').inc('id', 1);
            },
            error: function () {
                eventNotification('Newsy.', 'Wystąpił błąd podczas wyświetlania newsów.', 'negative')
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

function fulfillNewsListTemplate(Template, AppendTo, uAl, newsPl, newspaperPl, nTitle, nDescr, numV, numC, dAdded, ratingC, ratingV, newsID, tags, FaktTag, FakeTag) {
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
    };


    var html = Mustache.to_html(Template, newsVariables);
    var resultsRemaining = 0;

    $(html).hide().appendTo(AppendTo).fadeIn('slow');

    switch (ratingC) {
        case 1:
            $('#newsContainer').find('#' + newsID + '').find('.social').addClass("greenColor");
            break;

        case 2:
            $('#newsContainer').find('#' + newsID + '').find('.social').addClass("greenColor");
            $('#newsContainer').find('#' + newsID + '').find('.info').addClass("bGreenColor");
            $('#newsContainer').find('#' + newsID + '').find('#FaktTag').text("Fakt");

            break;

        case 3:
            $('#newsContainer').find('#' + newsID + '').find('.social').addClass("redColor");
            break;

        case 4:
            $('#newsContainer').find('#' + newsID + '').find('.social').addClass("redColor");
            $('#newsContainer').find('#' + newsID + '').find('.info').addClass("bRedColor");
            $('#newsContainer').find('#' + newsID + '').find('#FakeTag').text("Fake");
            break;
    }
}