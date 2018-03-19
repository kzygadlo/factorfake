$(document).ready(function () {

    if ($(window).width() >= 992) {
        $('#ManipulatedContainer > button').hover(
            function () {
                $(this).closest('div').find('.label').toggleClass('BGgreyColorLightToogle');
            }
        );

        $('#FaktContainer > button').hover(
            function () {
                $(this).closest('div').find('.label').toggleClass('BGgreenColorLightToogle');
            }
        );

        $('#FakeContainer > button').hover(
            function () {
                $(this).closest('div').find('.label').toggleClass('BGredColorLightToogle');
            }
        );
    }

    $('#FaktContainer > button').popup({
        on: 'click'
    });

    $('#FakeContainer > button').popup({
        on: 'click'
    });

    $('#ManipulatedContainer > button').popup({
        on: 'click'
    });

    
    $(document).on('click', '#FaktContainer .button', function () {
        newsVoting(1);
    });

    $(document).on('click', '#ManipulatedContainer .button', function () {
        newsVoting(2);
    });

    $(document).on('click', '#FakeContainer .button', function () {
        newsVoting(-1);
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
    var $newsID = $('.frmNewsID').val();
    ajaxNewsVoteRequest(whatVote, $newsID)
};

function commentsVoting(whatVote, $this, $notifBox) {

    var $commentID = $this.find('#commentContainerJS').data('commentid');
    var $fakt = $this.find(".commentFaktVote").find('i');
    var $fake = $this.find(".commentFakeVote").find('i');

    var $faktValue = $this.find(".commentFaktValue")
    var $fakeValue = $this.find(".commentFakeValue")

    var $faktClass = "green";
    var $fakeClass = "red";

    ajaxVoteRequest(whatVote, $commentID, $fakt, $fake, $faktValue, $fakeValue, $faktClass, $fakeClass, $notifBox)
};

function replyVoting(whatVote, $this, $notifBox) {

    var $commentID = $this.find('.DeleteReply').data('replyid');
    var $fakt = $this.find(".replyFaktVote").find('i');
    var $fake = $this.find(".replyFakeVote").find('i');

    var $faktValue = $this.find(".replyFaktValue")
    var $fakeValue = $this.find(".replyFakeValue")

    var $faktClass = "green";
    var $fakeClass = "red";

    ajaxVoteRequest(whatVote, $commentID, $fakt, $fake, $faktValue, $fakeValue, $faktClass, $fakeClass, $notifBox)
};

function ajaxVoteRequest(whatVote, itemID, $fakt, $fake, $faktValue, $fakeValue, $faktClass, $fakeClass, $notifBox) {

    $.ajax({
        url: '/Voting/Vote',
        type: 'POST',
        //timeout: 3000,
        data: {
            whatVote: whatVote,
            ID: itemID
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
};

function ajaxNewsVoteRequest(whatVote, itemID) {

    var $notifBox = $('#SingleNews');

    $.ajax({
        url: '/VotingNews/Vote',
        type: 'POST',
        //timeout: 3000,
        data: {
            whatVote: whatVote,
            ID: itemID
        },
        success: function (response) {
            votingNewsAction(response.result, response.faktVote, response.fakeVote, response.manipulatedVote);
        },
        error: function () {
            showNotification('negative', 'Głosowanie.', 'Wystąpił błąd podczas głosowania.', $notifBox)
        }
    });
};

function votingNewsAction(whatVote, $faktValue, $fakeValue, $manipulatedValues) {

    var faktClassName = 'BGgreenColorLight';
    var fakeClassName = 'BGredColorLight';
    var manipulatedClassName = 'BGgreyColorLight';

    var $fakt = $('#FaktContainer').find('div');
    var $fake = $('#FakeContainer').find('div');
    var $manipulated = $('#ManipulatedContainer').find('div');

    $fakt.text($faktValue);
    $fake.text($fakeValue);
    $manipulated.text($manipulatedValues);

    switch (whatVote) {

        case -1:
            $fakt.removeClass(faktClassName);
            $fake.addClass(fakeClassName);
            $manipulated.removeClass(manipulatedClassName);
            break;
        case 0:
            $fakt.removeClass(faktClassName);
            $fake.removeClass(fakeClassName);
            $manipulated.removeClass(manipulatedClassName);
            break;

        case 1:
            $fakt.addClass(faktClassName);
            $fake.removeClass(fakeClassName);
            $manipulated.removeClass(manipulatedClassName);
            break;
        case 2:
            $fakt.removeClass(faktClassName);
            $fake.removeClass(fakeClassName);
            $manipulated.addClass(manipulatedClassName);
            break;
    }
}

