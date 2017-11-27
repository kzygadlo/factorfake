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
        var $comment = $('#frmCommentText');
        var $newsID = $('.frmNewsID').val();
        var $template = $('#commentPattern').html();
        var $commentList = $("#commentsList");

        ajaxAddComment($comment, $newsID, 0, $template, $commentList)
    });

    //Add Reply
    $(document).on('submit', '.frmAddReply', function (event) {
        event.preventDefault();

        var $comment = $(this).closest('.addReply').find('.message');
        var $newsID = $('.frmNewsID').val();
        var $parentID = $(this).find('.commentID').val();
        var $template = $('#replyPattern').html();
        var $replyList = $(this).closest('.singleComment').find('.reply-list');

        ajaxAddComment($comment, $newsID, $parentID, $template, $replyList)
    });

    //Show Replies
    $(document).on('click', '#commentContainerJS', function (event) {
        event.stopPropagation();
        event.preventDefault();

        var $parentID = $(this).data('commentid')
        var $template = $('#replyPattern').html();
        var $replyList = $(this).closest('.singleComment').find('.reply-list');

        if ($replyList.is(":hidden")) {

            $.ajax({
                url: '/Comment/GetReplies',
                type: "GET",
                data: { parentID: $parentID },
                success: function (result) {

                    $(this).closest('li').find('.reply-list').fadeToggle();

                    $.each(result, function (key, val) {
                        fulfillTemplate(val.cid, val.com, val.date, val.userL, val.userN, val.faktV, val.fakeV)
                    });

                },
                error: function () {
                    //alert("Nie można wyświetlić komentarzy.");
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
                if (response.Success == false) {
                    //alert(response.ResultMsg);
                }
                else {
                    $entComm.closest(".singleComment").fadeOut(600, function () {
                        $entComm.closest(".singleComment").remove();
                    });
                }

            },
            error: function () {
                //alert("nie mozna usunac komentarza");
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
                if (response.Success == false) {
                    //alert(response.ResultMsg);
                }
                else {
                    $entComm.closest(".singleReply").fadeOut(600, function () {
                        $entComm.closest(".singleReply").remove();
                    });
                }

            },
            error: function () {
                //alert("nie mozna usunac komentarza");
            }
        });
    });

    //Get comments for provided newsID (on open)
    if ($("#SingleNews").length > 0 && $("#SingleNews").hasClass('first')) {
        $("#SingleNews").removeClass('first');
        showComments(0);
    }

    event.preventDefault();

    $('#commentSort0').click(function () {
        $commList = $('#commentsList')
        removeComments($commList);
        //show loading during retrieving
        showComments(0);
    });

    $('#commentSort1').click(function () {
        $commList = $('#commentsList')
        removeComments($commList);
        //show loading during retrieving
        showComments(1);
    });

    $('#commentSort2').click(function () {
        $commList = $('#commentsList')
        removeComments($commList);
        //show loading during retrieving
        showComments(2);
    });

});


function fulfillTemplate(rid, rep, date, logoN, userN, faktV, fakeV) {
    var replyVariables =
    {
        CommentID: rid,
        Comment: rep,
        Date: date,
        LogoName: logoN,
        UserName: userN,
        CommentFaktV: faktV,
        CommentFakeV: fakeV
    };
    var html = Mustache.to_html($template, replyVariables);

    $(html).hide().prependTo($replyList).fadeIn('slow');
};

function ajaxAddComment($comment, newsID, parentID, $template, $wherePrepend) {
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
                fulfillCommentTemplate(response.cid, response.com, response.date, response.userN, response.userL, $template, $wherePrepend, $comment, newsID, parentID);
            } else {
                //alert("nie mozna dodac komentarza [try catch error]");
            }
        },
        error: function () {
            //alert("nie mozna dodac komentarza [ajax error]");
        }
    });

};

function fulfillCommentTemplate(cid, com, date, userN, userL, $template, $wherePrepend, $comment, newsID, parentID) {
    var commentVariables =
    {
        NewsID: newsID,
        CommentID: cid,
        Comment: com,
        Date: date,
        UserName: userN,
        LogoName: userL,
        CommentFaktV: 0,
        CommentFakeV: 0
    };
    var html = Mustache.to_html($template, commentVariables);

    if (parentID != 0 && $wherePrepend.is(":hidden")) {
        $wherePrepend.fadeToggle();
    }

    $comment.val("");
    $(html).hide().prependTo($wherePrepend).fadeIn('slow');
};




function showComments(Filter) {
    $('#loadingImage').removeClass("hidden");

    var $template = $('#commentPattern').html();
    var $commentList = $("#commentsList");

    function fulfillTemplate(nid, cid, com, date, userN, userL, faktV, fakeV, repV) {

        var repValue = "";
        if (repV != 0) {
            repValue = "odpowiedzi: " + repV;
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
            RepliesV: repValue
        };
        var html = Mustache.to_html($template, replyVariables);
        //$comment.val("");
        $(html).hide().prependTo($commentList).fadeIn('fast');
    }

    var NewsID = $(".frmNewsID").val();
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
            $.each(result, function (key, val) {
                fulfillTemplate(NewsID, val.cid, val.com, val.date, val.userN, val.userL, val.faktV, val.fakeV, val.repliesV)
            });

            if (result.length == 0) {
                $('#noResultTab').removeClass("hidden");
            }
            else {
                $('#sortingTab').removeClass("hidden");
            }

            $('#loadingImage').addClass("hidden");
        },
        error: function () {
            //alert("Nie można wyświetlić newsów");
        }
    });
};

//Remove Comments/Replies
function removeComments(node) {
    node.find('li').remove();
};

