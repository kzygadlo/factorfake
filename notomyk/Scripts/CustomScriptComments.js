$(document).ready(function () {

    //Show reply form
    $(document).on('click',
        '#addReplyBox',
        function (event) {
            event.stopPropagation();
            event.preventDefault();
            $(this).closest('.comment-box').find('.addReply').fadeToggle();
        });

    //Add Comment
    $('.frmAddComment').on('submit', function (event) {
        event.preventDefault();
        var $notifBox = $('#SingleNews');
        var $comment = $('#frmCommentText');
        var $newsID = $('.frmNewsID').val();
        var $template = $('#commentPattern').html();
        var $commentList = $("#commentsList");

        ajaxAddComment($comment, $newsID, 0, $template, $commentList, $notifBox)
    });

    //Add Reply
    $(document).on('submit', '.frmAddReply', function (event) {
        event.preventDefault();
        var $notifBox = $(this).closest('.commentBoxJQ');
        var $comment = $(this).closest('.addReply').find('.message');
        var $newsID = $('.frmNewsID').val();
        var $parentID = $(this).find('.commentID').val();
        var $template = $('#replyPattern').html();
        var $replyList = $(this).closest('.singleComment').find('.reply-list');

        ajaxAddComment($comment, $newsID, $parentID, $template, $replyList, $notifBox)
    });

    //Show Replies
    $(document).on('click', '#commentContainerJS', function (event) {
        event.stopPropagation();
        event.preventDefault();

        var $parentID = $(this).data('commentid')
        var $template = $('#replyPattern').html();
        var $replyList = $(this).closest('.singleComment').find('.reply-list');

        if ($replyList.is(":hidden")) {

            var $replyTextButton = $(this).find('u')
            var previousCaption = $replyTextButton.text();
            $replyTextButton.text("...wczytywanie...");

            $.ajax({
                url: '/Comment/GetReplies',
                type: "GET",
                data: { parentID: $parentID },
                success: function (result) {

                    $(this).closest('li').find('.reply-list').fadeToggle();

                    $.each(result, function (key, val) {
                        fulfillReplyTemplate(val.cid, val.com, val.date, val.userL, val.userN, val.faktV, val.fakeV, $template, $replyList, val.positiveCommentsNumber, val.allCommentsNumber, val.reputationPoints, val.voteForComment, val.reportedClass)
                    });
                    $replyTextButton.text(previousCaption);

                    $('.replyFakeVote').popup({
                        on: 'click'
                    });

                    $('.replyFaktVote').popup({
                        on: 'click'
                    });
                },
                error: function () {
                    showNotification('negative', 'Odpowiedzi do komentarzy.', 'Wystąpił błąd podczas pobierania odpowiedzi do komentarzy.', $replyList)
                }
            });

            $replyList.fadeToggle();

        } else {
            $replyList.fadeToggle();

            setTimeout(function () {
                removeComments($replyList);
            }, 500);
        }
    });

    //Remove comment
    $(document).on('click', '.DeleteComment', function (event) {
        event.preventDefault();
        //event.stopPropagation();
        var $entComm = $(this);
        var CommentID = $entComm.data('commentid')

        $.ajax({
            url: '/Comment/Remove',
            type: 'POST',
            //timeout: 3000,
            data: {
                commentID: CommentID
            },
            success: function (response) {


                if (response.success == true && response.childComments == 0) {
                    $entComm.closest(".singleComment").fadeOut(600, function () {
                        $entComm.closest(".singleComment").remove();
                    });

                }
                else if (response.success == true && response.childComments > 0)
                {
                    $entComm.closest(".singleComment").find(".removedMessage").removeClass('hidden');
                    $entComm.closest(".singleComment").find(".basicMessage").addClass('hidden');

                }
                else {
                    showNotification('negative', response.errHeader, response.errMessage, $entComm.closest('.commentBoxJQ'))
                }

            },
            error: function () {
                showNotification('negative', 'Usuwanie komentarza.', 'Wystąpił błąd podczas usuwania komentarza.', $entComm.closest('.commentBoxJQ'))

            }
        });
    });

    //Remove reply
    $(document).on('click', '.DeleteReply', function (event) {
        event.preventDefault();
        //event.stopPropagation();
        var $entComm = $(this);
        var CommentID = $entComm.data('replyid')

        $.ajax({
            url: '/Comment/Remove',
            type: 'POST',
            //timeout: 3000,
            data: {
                commentID: CommentID
            },
            success: function (response) {
                if (response.success == true) {
                    $entComm.closest(".singleReply").fadeOut(600, function () {
                        $entComm.closest(".singleReply").remove();
                    });
                }
                else {
                    showNotification('negative', response.errHeader, response.errMessage, $entComm.closest('.commentBoxJQ'))
                }
            },
            error: function () {
                showNotification('negative', 'Usuwanie komentarza.', 'Wystąpił błąd podczas usuwania komentarza.', $entComm.closest('.commentBoxJQ'))
            }
        });
    });

    //Get comments for provided newsID (on open)
    if ($("#SingleNews").length > 0 && $("#SingleNews").hasClass('first')) {
        $("#SingleNews").removeClass('first');
        showComments(0);
    }

    $(document).on("click", "#commentSort0", function (a) {
        $("#sortingTab").addClass('hidden');
        $commList = $('#commentsList')
        removeComments($commList);
        //show loading during retrieving
        showComments(0);
        a.preventDefault();
    });

    $(document).on("click", "#commentSort1", function (a) {
        $("#sortingTab").addClass('hidden');
        $commList = $('#commentsList')
        removeComments($commList);
        //show loading during retrieving
        showComments(1);
        a.preventDefault();
    });

    $(document).on("click", "#commentSort2", function (a) {
        $("#sortingTab").addClass('hidden');
        $commList = $('#commentsList')
        removeComments($commList);
        //show loading during retrieving
        showComments(2);
        a.preventDefault();
    });
});


function fulfillReplyTemplate(rid, rep, date, logoN, userN, faktV, fakeV, $template, $replyList, repPcom, repAcom, repPoins, commV, reportClass) {

    var c1 = "outline";
    var c2 = "outline";

    if (commV == 0) {
        c2 = "";
    }
    else if (commV == 1) {
        c1 = "";
    }

    var replyVariables =
    {
        CommentID: rid,
        Comment: rep,
        Date: date,
        LogoName: logoN,
        UserName: userN,
        CommentFaktV: faktV,
        CommentFakeV: fakeV,
        class1: c1,
        class2: c2,
        positiveCount: repPcom,
        allCount: repAcom,
        reputationPoints: repPoins,
        ReportedClass: reportClass
    };
    var html = Mustache.to_html($template, replyVariables);

    $(html).hide().prependTo($replyList).fadeIn('slow');
};

function ajaxAddComment($comment, newsID, parentID, $template, $wherePrepend, $whereAppendNotif) {
    $.ajax({
        url: '/Comment/Add',
        type: 'POST',
        //timeout: 3000,
        data: {
            CommentText: $comment.val(),
            NewsID: newsID,
            parentID: parentID
        },
        success: function (response) {
            if (response.success == true) {
                fulfillCommentTemplate(response.cid, response.com, response.date, response.userN, response.userL, $template, $wherePrepend, $comment, newsID, parentID, response.positiveCommentsNumber, response.allCommentsNumber, response.reputationPoints);
            } else {

                showNotification('negative', response.errHeader, response.errMessage, $whereAppendNotif)
            }
        },
        error: function () {
            showNotification('negative', 'Dodawanie komentarza.', 'Wystąpił błąd podczas dodawania komentarza.', $whereAppendNotif)
        }
    });
};

function fulfillCommentTemplate(cid, com, date, userN, userL, $template, $wherePrepend, $comment, newsID, parentID, repPcom, repAcom, repPoins) {
    var commentVariables =
    {
        NewsID: newsID,
        CommentID: cid,
        Comment: com,
        Date: date,
        UserName: userN,
        LogoName: userL,
        CommentFaktV: 0,
        CommentFakeV: 0,
        class1: "outline",
        class2: "outline",
        positiveCount: repPcom,
        allCount: repAcom,
        reputationPoints: repPoins,
        ReportedClass: "hidden",
        CommentBasicClass: "",
        CommentRemovedClass: "hidden"
    };
    var html = Mustache.to_html($template, commentVariables);

    if (parentID != 0 && $wherePrepend.is(":hidden")) {
        $wherePrepend.fadeToggle();
    }    

    $('#noResultTab').addClass("hidden");
    $('#sortingTab').removeClass("hidden");

    $comment.val("");

    if (parentID == 0) {
        $(html).hide().prependTo($wherePrepend).fadeIn('slow');
    }
    else {
        $(html).hide().appendTo($wherePrepend).fadeIn('slow');
    }

    $("#frmReplyText").MaxLength({
        MaxLength: 3000,
        CharacterCountControl: $('.charCounter')
    });
    
};

function showComments(Filter) {
    $('#loadingImage').removeClass("hidden");

    var $template = $('#commentPattern').html();
    var $commentList = $("#commentsList");
    var $commentsContainer = $('.comments-container');

    function fulfillCommTemplate(nid, cid, com, date, userN, userL, faktV, fakeV, repV, commV, repPcom, repAcom, repPoins, basicClass, removedClass, reportClass) {

        var repValue = "";
        if (repV != 0) {
            repValue = "odpowiedzi: " + repV;
        }

        var c1 = "outline";
        var c2 = "outline";

        if (commV == 0) {
            c2 = "";
        }
        else if (commV == 1) {
            c1 = "";
        }

        var replyVariables =
        {
            NewsID: nid,
            CommentID: cid,
            Comment: com,
            Date: date,
            UserName: userN,
            LogoName: userL,
            CommentFaktV: faktV,
            CommentFakeV: fakeV,
            RepliesV: repValue,
            class1: c1,
            class2: c2,
            positiveCount: repPcom,
            allCount: repAcom,
            reputationPoints: repPoins,
            ReportedClass: reportClass,
            CommentBasicClass: basicClass,
            CommentRemovedClass: removedClass

        };
        var html = Mustache.to_html($template, replyVariables);
        //$comment.val("");
        $(html).hide().prependTo($commentList).fadeIn('fast');
    }

    var NewsID = $(".frmNewsID").val();

    $commentsContainer.addClass("hidden");

    $.ajax({
        url: '/Comment/Get',
        type: "GET",
        data: {
            newsID: NewsID,
            filter: Filter
        },
        success: function (result) {

            $('#sortingTab').addClass("hidden");
            $('#noResultTab').addClass("hidden");
            $('#loadingImage').removeClass("hidden");
            
;            $.each(result, function (key, val) {
                fulfillCommTemplate(NewsID, val.cid, val.com, val.date, val.userN, val.userL, val.faktV, val.fakeV, val.repliesV, val.voteForComment, val.positiveCommentsNumber, val.allCommentsNumber, val.reputationPoints, val.commentBasicClass, val.commentRemovedClass, val.reportedClass)
            });

            $('.commentFakeVote').popup({
                on: 'click'
            });

            $('.commentFaktVote').popup({
                on: 'click'
            });


            $("#frmReplyText").MaxLength({
                MaxLength: 3000,
                CharacterCountControl: $('.charCounter')
            });

            if (result.length == 0) {
                $('#noResultTab').removeClass("hidden");
            }
            else {
                $('#sortingTab').removeClass("hidden");
            }

            $('#loadingImage').addClass("hidden");
            $commentsContainer.removeClass("hidden");
        },
        error: function () {
            showNotification('negative', 'Pobieranie komentarzy.', 'Wystąpił błąd podczas pobierania komentarzy.', $commentList)
        }
    });
};

//Remove Comments/Replies
function removeComments(node) {
    node.find('li').remove();
};

//Report Comment
$(document).ready(function () {
    $(document).on('click', '.reportComment', function (event) {
        event.preventDefault();
        //event.stopPropagation();
        var $Comment = $(this);
        var CommentID = $Comment.data('commentid')

        $.ajax({
            url: '/Comment/Report',
            type: 'POST',
            //timeout: 3000,
            data: {
                commentID: CommentID,
                ToReport: true
            },
            success: function (response) {
                if (response.Success == true) {
                    if ($Comment.closest('.comment-box').find('.reportSpan').hasClass('hidden')) {
                        showNotification('positive', 'Zgłaszanie komentarza.', 'Komentarz został zgłoszony do moderacji.', $Comment.closest('.commentBoxJQ'));
                        $Comment.closest('.comment-box').find('.reportSpan').removeClass('hidden');
                    }
                    else {
                        showNotification('negative', 'Zgłaszanie komentarza.', 'Komentarz został już zgłoszony.', $Comment.closest('.commentBoxJQ'));
                    }
                     
                }
                else {
                    
                    showNotification('negative', 'Zgłaszanie komentarza.', response.errMessage, $Comment.closest('.commentBoxJQ'))
                }
            },
            error: function () {


                showNotification('negative', 'Zgłaszanie komentarza.', 'Wystąpił błąd', $Comment.closest('.commentBoxJQ'))
            }
        });
    });
});
