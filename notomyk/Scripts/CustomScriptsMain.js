//Scripts for List of News page [index]

$(document).ready(function () {

    getListOfNews();
    $('.ui.dropdown').dropdown();
    $('#search-select').dropdown();

    $('.newspaperFilter.ui.dropdown').change(
    function () {
        $('#newsContainer').find('li').remove();
        $('#newPageButton').data('id', 0);
        getListOfNews();
    });

    $('.whatNewsFilter').dropdown({
        onChange:
            function () {
                $('#newsContainer').find('li').remove();
                $('#newPageButton').data('id', 0);
                getListOfNews();
            }
    });
});


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
        var $page = $('#newPageButton').data('id');

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
            Page: $page
        };

        $.ajax({
            url: '/Main/Get',
            type: "POST",
            data: model,
            success: function (result) {
                $('#loadingImage').removeClass("hidden");

                $.each(result, function (key, val) {
                    var $template = $('#newsPattern').html();
                    fulfillNewsListTemplate($template, $newsList, val.urlActionLink, val.newspaperPictureLink, val.newsPictureLink, val.newsTitle, val.newsDescription, val.numberOfVisitors, val.numberOfComments, val.dateAdded, val.ratingClass, val.ratingValue, val.newsID, val.tagList, val.faktValue, val.fakeValue);

                    resultRemaining = val.remainingRows;
                });

                $('#loadingImage').addClass("hidden");
                $('.addNewPage').fadeIn('slow');

                if (resultRemaining >= 10) {
                    $('#newPageButton').removeClass("hidden");
                    $('#newPageButton').val("pokaz kolejne 10 z " + resultRemaining + " pozostałych");
                }
                else {
                    $('#newPageButton').removeClass("hidden");
                    $('#newPageButton').val("pokaz kolejne " + resultRemaining + " z " + resultRemaining + " pozostałych");
                }

                if (resultRemaining <= 0) {
                    $('#newPageButton').addClass("hidden");

                }
                $('#newPageButton').inc('id', 1);
            },
            error: function () {
                alert("Nie można wyświetlić newsów.");
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
        fakeTag: FakeTag
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