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


    //    var $fakt = $('#voteFakt').data('value');
    //    var $fake = $('#voteFake').data('value');
    //    var $percentage = Math.round($fakt / ($fakt + $fake) * 100);

    //    if ($percentage > 50) {
    //        var $color = "#5cb85c";
    //        var $speed = 1000;
    //    }
    //    else {
    //        var $color = "#d9534f";
    //        var $speed = 500;
    //    }

    //    var options = {
    //        height: "130px",
    //        width: "130px",
    //        line_width: 6,
    //        color: $color,
    //        starting_position: 0,
    //        percent: 0,
    //        text: "percent"
    //    }
    //    var progress_circle = $("#progress-circle").gmpc(options);
    //    progress_circle.gmpc('animate', $percentage, $speed);
    //});


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

});