$(document).ready(function () {

    //Tags click

    $('.tag').click(function (event) {
        var textValue = $(this).text();

        window.location.href = '/Main/Index/?tag=' + textValue;
    });

    $("#frmCommentText").MaxLength({
        MaxLength: 3000,
        CharacterCountControl: $('.charCounter')
    });

    window.fbAsyncInit = function () {
        FB.init({
            appId: '158285538190656',
            xfbml: true,
            version: 'v2.12'
        });
        FB.AppEvents.logPageView();
    };


    $('#fbButton').click(function () {
        FB.ui({
            method: 'share',
            mobile_iframe: true,
            href: $('#fbButton').data('url')
        })
    });

    //Remove news
    $(document).ready(function () {
        $(document).on('click', '.deleteNews', function (event) {
            event.preventDefault();
            //event.stopPropagation();
            var $News = $(this);
            var NewsID = $News.closest('.dropDownParent').data('newsid')
            var $whereAppend = $('#SingleNews')

            $.ajax({
                url: '/News/Remove',
                type: 'POST',
                //timeout: 3000,
                data: {
                    newsID: NewsID
                },
                success: function (response) {
                    if (response.Success == true) {
                        window.location.href = response.redirectUrl;
                    }
                    else {
                        showNotification('negative', 'Usuwanie newsów.', response.errMessage, $whereAppend)
                    }
                },
                error: function () {
                    showNotification('negative', 'Usuwanie newsów.', 'Wystąpił błąd podczas usuwania newsów.', $whereAppend)
                }
            });
        });
    });


    //Report news
    $(document).ready(function () {
        $(document).on('click', '.reportNews', function (event) {
            event.preventDefault();
            //event.stopPropagation();
            var $News = $(this);
            var NewsID = $News.closest('.dropDownParent').data('newsid')
            var $whereAppend = $('#SingleNews')

            $.ajax({
                url: '/News/Report',
                type: 'POST',
                //timeout: 3000,
                data: {
                    newsID: NewsID,
                    ToReport: true
                },
                success: function (response) {

                    if (response.Success == true) {

                        if ($('.newsReported').hasClass('hidden')) {
                            showNotification('positive', 'Zgłaszanie newsów.', 'News został zgłoszony do moderacji.', $whereAppend)
                            $('.newsReported').removeClass('hidden');
                        }
                        else {
                            showNotification('negative', 'Zgłaszanie newsa.', 'News został już zgłoszony.', $whereAppend);
                        }
                    }
                    else {
                        showNotification('negative', 'Zgłaszanie newsów.', response.errMessage, $whereAppend)
                    }
                },
                error: function () {
                    showNotification('negative', 'Zgłaszanie newsów.', 'Wystąpił błąd podczas zgłazania newsa.', $whereAppend)
                }
            });
        });
    });


    $('.message .close').on('click', function () {
        $(this)
            .closest('.message')
            .transition('fade')
            ;
    });

});


// When the user clicks on the button, scroll to the top of the document

window.onscroll = function () { scrollFunction() };


function scrollFunction() {
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        document.getElementById("myBtn").style.display = 'block';
    } else {
        document.getElementById("myBtn").style.display = 'none';
    }
}

function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}

