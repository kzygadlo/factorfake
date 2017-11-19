$(document).ready(function () {

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
            var NewsID = $News.data('newsid')

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
                        alert(response.ResultMsg);
                    }
                },
                error: function () {
                    alert("nie mozna usunac newsa");
                }
            });
        });
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
