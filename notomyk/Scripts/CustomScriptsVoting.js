$(document).ready(function () {

    $(document).on('click', '.voteFakt', function () {
        newsVoting(true, true);
    });

    $(document).on('click', '.voteFake', function () {
        newsVoting(false, true);
    });

    $(document).on('click', '.commentFaktVote', function () {
        $notifBox = $(this).closest('.commentBoxJQ');
        $this = $(this).closest('.singleComment')
        //var $commentID = $(this).closest('.singleComment').find('#commentContainerJS').data('commentid');
        commentsVoting(true, $this, $notifBox);
    });

    $(document).on('click', '.commentFakeVote', function () {
        $notifBox = $(this).closest('.commentBoxJQ');
        $this = $(this).closest('.singleComment')
        //var $commentID = $(this).closest('.singleComment').find('#commentContainerJS').data('commentid');
        commentsVoting(false, $this, $notifBox);
    });

    $(document).on('click', '.replyFaktVote', function () {
        $notifBox = $(this).closest('.commentBoxJQ');

        $this = $(this).closest('.singleReply')
        //var $commentID = $(this).closest('.singleComment').find('#commentContainerJS').data('commentid');
        replyVoting(true, $this, $notifBox);
    });

    $(document).on('click', '.replyFakeVote', function () {
        $notifBox = $(this).closest('.commentBoxJQ');
        $this = $(this).closest('.singleReply')
        //var $commentID = $(this).closest('.singleComment').find('#commentContainerJS').data('commentid');
        replyVoting(false, $this, $notifBox);
    });
});


function newsVoting(whatVote) {

    var $notifBox = $('#SingleNews');

    var $newsID = $('.frmNewsID').val();
    var $fake = $(".voteFake.button > i")
    var $fakt = $(".voteFakt.button > i")

    var $faktValue = $("a.voteFakt")
    var $fakeValue = $("a.voteFake")

    var $faktClass = "green";
    var $fakeClass = "red";

    ajaxVoteRequest(whatVote, true, $newsID, $fakt, $fake, $faktValue, $fakeValue, $faktClass, $fakeClass, $notifBox)
};


function commentsVoting(whatVote, $this, $notifBox) {

    var $commentID = $this.find('#commentContainerJS').data('commentid');
    var $fakt = $this.find(".commentFaktVote").find('i');
    var $fake = $this.find(".commentFakeVote").find('i');

    var $faktValue = $this.find(".commentFaktValue")
    var $fakeValue = $this.find(".commentFakeValue")

    var $faktClass = "green";
    var $fakeClass = "red";

    ajaxVoteRequest(whatVote, false, $commentID, $fakt, $fake, $faktValue, $fakeValue, $faktClass, $fakeClass, $notifBox)
};

function replyVoting(whatVote, $this, $notifBox) {

    var $commentID = $this.find('.DeleteReply').data('replyid');
    var $fakt = $this.find(".replyFaktVote").find('i');
    var $fake = $this.find(".replyFakeVote").find('i');

    var $faktValue = $this.find(".replyFaktValue")
    var $fakeValue = $this.find(".replyFakeValue")

    var $faktClass = "green";
    var $fakeClass = "red";

    ajaxVoteRequest(whatVote, false, $commentID, $fakt, $fake, $faktValue, $fakeValue, $faktClass, $fakeClass, $notifBox)
};

function ajaxVoteRequest(whatVote, whatType, itemID, $fakt, $fake, $faktValue, $fakeValue, $faktClass, $fakeClass, $notifBox) {

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
            showNotification('negative', 'Głosowanie.', 'Wystąpił błąd podczas głosowania.', $notifBox)
        }
    });
};


function votingAction(whatVote, $fakt, $fake, $faktValue, $fakeValue, $faktClass, $fakeClass) {

    switch (whatVote) {

        case -2:
            $fakt.removeClass($faktClass);
            $fake.addClass($fakeClass);            

            $fakeValue.html(parseInt($($fakeValue).html(), 10) + 1)
            $faktValue.html(parseInt($($faktValue).html(), 10) - 1)
            break;
        case -1:
            $fake.addClass($fakeClass);
            $fakeValue.html(parseInt($($fakeValue).html(), 10) + 1)
            break;
        case 0:
            break;
        case 1:
            $fakt.addClass($faktClass);
            $faktValue.html(parseInt($($faktValue).html(), 10) + 1)
            break;
        case 2:
            $fakt.addClass($faktClass);
            $fake.removeClass($fakeClass);

            $fakeValue.html(parseInt($($fakeValue).html(), 10) - 1)
            $faktValue.html(parseInt($($faktValue).html(), 10) + 1)
            break;
    }
}
