$(document).ready(function () {

    $(document).on('click', '.voteFakt', function () {
        newsVoting(true, true);
    });

    $(document).on('click', '.voteFake', function () {
        newsVoting(false, true);
    });

    $(document).on('click', '.commentFaktVote', function () {
        $this = $(this).closest('.singleComment')
        //var $commentID = $(this).closest('.singleComment').find('#commentContainerJS').data('commentid');
        commentsVoting(true, $this);
    });

    $(document).on('click', '.commentFakeVote', function () {
        $this = $(this).closest('.singleComment')
        //var $commentID = $(this).closest('.singleComment').find('#commentContainerJS').data('commentid');
        commentsVoting(false, $this);
    });

    $(document).on('click', '.replyFaktVote', function () {
        $this = $(this).closest('.singleReply')
        //var $commentID = $(this).closest('.singleComment').find('#commentContainerJS').data('commentid');
        replyVoting(true, $this);
    });

    $(document).on('click', '.replyFakeVote', function () {
        $this = $(this).closest('.singleReply')
        //var $commentID = $(this).closest('.singleComment').find('#commentContainerJS').data('commentid');
        replyVoting(false, $this);
    });
});


function newsVoting(whatVote) {

    var $newsID = $('.frmNewsID').val();
    var $fakt = $(".voteFakt.button")
    var $fake = $(".voteFake.button")

    var $faktValue = $("a.voteFakt")
    var $fakeValue = $("a.voteFake")

    var $faktClass = "BGgreenColorLight";
    var $fakeClass = "BGredColorLight";

    ajaxVoteRequest(whatVote, true, $newsID, $fakt, $fake, $faktValue, $fakeValue, $faktClass, $fakeClass)
};


function commentsVoting(whatVote, $this) {

    var $commentID = $this.find('#commentContainerJS').data('commentid');
    var $fakt = $this.find(".commentFaktVote").find('i');
    var $fake = $this.find(".commentFakeVote").find('i');

    var $faktValue = $this.find("a#commentFaktValue")
    var $fakeValue = $this.find("a#commentFakeValue")

    var $faktClass = "outline";
    var $fakeClass = "outline";

    ajaxVoteRequest(whatVote, false, $commentID, $fakt, $fake, $faktValue, $fakeValue, $faktClass, $fakeClass)
};

function replyVoting(whatVote, $this) {

    var $commentID = $this.find('.DeleteReply').data('replyid');
    var $fakt = $this.find(".replyFaktVote").find('i');
    var $fake = $this.find(".replyFakeVote").find('i');

    var $faktValue = $this.find("a#replyFaktValue")
    var $fakeValue = $this.find("a#replyFakeValue")

    var $faktClass = "outline";
    var $fakeClass = "outline";

    ajaxVoteRequest(whatVote, false, $commentID, $fakt, $fake, $faktValue, $fakeValue, $faktClass, $fakeClass)
};

function ajaxVoteRequest(whatVote, whatType, itemID, $fakt, $fake, $faktValue, $fakeValue, $faktClass, $fakeClass) {
    $.ajax({
        url: '/Voting/Vote',
        type: 'POST',
        //timeout: 3000,
        data: {
            whatVote: whatVote,
            ID: itemID,
            newsVote: whatType
        },
        success: function (response) {
            votingAction(response.result, $fakt, $fake, $faktValue, $fakeValue, $faktClass, $fakeClass);
        },
        error: function () {
            eventNotification('Głosowanie.', 'Wystąpił błąd podczas głosowania.', 'negative')
        }
    });
};


function votingAction(whatVote, $fakt, $fake, $faktValue, $fakeValue, $faktClass, $fakeClass) {

    switch (whatVote) {

        case -2:
            $fakt.addClass($faktClass);
            $fake.removeClass($fakeClass);

            $fakeValue.html(parseInt($($fakeValue).html(), 10) + 1)
            $faktValue.html(parseInt($($faktValue).html(), 10) - 1)
            break;
        case -1:
            $fake.removeClass($fakeClass);
            $fakeValue.html(parseInt($($fakeValue).html(), 10) + 1)
            break;
        case 0:
            break;
        case 1:
            $fakt.removeClass($faktClass);
            $faktValue.html(parseInt($($faktValue).html(), 10) + 1)
            break;
        case 2:
            $fakt.removeClass($faktClass);
            $fake.addClass($fakeClass);

            $fakeValue.html(parseInt($($fakeValue).html(), 10) - 1)
            $faktValue.html(parseInt($($faktValue).html(), 10) + 1)
            break;
    }
}