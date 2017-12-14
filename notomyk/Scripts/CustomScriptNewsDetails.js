$(document).ready(function () {


    $("#frmCommentText").MaxLength({
        MaxLength: 3000,
        CharacterCountControl: $('.charCounter')
    });

    $("#frmReplyText").MaxLength({
        MaxLength: 3000,
        CharacterCountControl: $('.charCounterR')
    });    


    window.fbAsyncInit = function () {
        FB.init({
            appId: '288016351717557',
            cookie: true,
            xfbml: true,
            version: 'v2.11'
        });

        FB.AppEvents.logPageView();

    };

    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) { return; }
        js = d.createElement(s); js.id = id;
        js.src = "https://connect.facebook.net/en_US/sdk.js";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));


    $('#fbButton').click(function () {

        FB.ui({
            method: 'share',
            mobile_iframe: true,
            href: $('#fbButton').data('url')
        })
    });

    $('.button.fakt').popup({
        on: 'click'
    });

    $('.button.fake').popup({
        on: 'click'
    });

    $('.commentFakeVote').popup({
        on: 'click'
    });

    $('.commentFaktVote').popup({
        on: 'click'
    });

    //Remove news
    $(document).ready(function () {
        $(document).on('click', '.deleteNews', function (event) {
            event.preventDefault();
            //event.stopPropagation();
            var $News = $(this);
            var NewsID = $News.closest('.dropDownParent').data('newsid')

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
                        eventNotification('Usuwanie newsów .', 'Wystąpił błąd podczas usuwania newsów.', 'negative')
                    }
                },
                error: function () {
                    eventNotification('Usuwanie newsów .', 'Wystąpił błąd podczas usuwania newsów.', 'negative')
                }
            });
        });
    });

    $(document).ready(function () {
        $(document).on('click', '.reportNews', function (event) {
            event.preventDefault();
            //event.stopPropagation();
            var $News = $(this);
            var NewsID = $News.closest('.dropDownParent').data('newsid')

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
                        eventNotification('Zgłaszanie newsów.', 'News został zgłoszony do moderacji.')
                    }
                    else {
                        eventNotification('Zgłaszanie newsów.', response.ResultMsg, 'negative')
                    }
                },
                error: function () {
                    eventNotification('Zgłaszanie newsów.', 'Wystąpił błąd', 'negative')
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
        document.getElementById("myBtn").style.display = "block";
    } else {
        document.getElementById("myBtn").style.display = "none";
    }
}

function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}

