$(document).ready(function () {

    tinymce.init({
        selector: "#topicDescription",
        encoding: "xml"
    });

    //Show reply form
    $(document).on('click',
        '#addReplyBox',
        function (event) {
            event.stopPropagation();
            event.preventDefault();
            $(this).closest('.comment-box').find('.addPostReply').fadeToggle();
        });

    //Get posts for provided topicID (on open)
    if ($("#SingleTopic").length > 0 && $("#SingleTopic").hasClass('first')) {
        $("#SingleTopic").removeClass('first');
        showPosts(0);
    }

    //Add Post
    $('.frmAddPostComment').on('submit', function (event) {
        event.preventDefault();
        var $comment = $('#frmCommentText');
        var $topicID = $('.frmTopicID').val();
        var $template = $('#forumPostTemplate').html();
        var $commentList = $("#commentsList");

        ajaxAddPost($comment, $topicID, 0, $template, $commentList)
    });

    //Add Reply
    $(document).on('submit', '.frmAddPostReply', function (event) {
        event.preventDefault();

        var $post = $(this).closest('.addPostReply').find('.message');
        var $topicID = $('.frmTopicID').val();
        var $parentID = $(this).find('.postID').val();
        var $template = $('#replyPostPattern').html();
        var $replyList = $(this).closest('.singleComment').find('.reply-list');

        ajaxAddPost($post, $topicID, $parentID, $template, $replyList)
    });

    //Report Post
    $(document).ready(function () {
        $(document).on('click', '.reportPost', function (event) {
            event.preventDefault();
            //event.stopPropagation();
            var $Post = $(this);
            var PostID = $Post.data('postid')

            $.ajax({
                url: '/ForumPost/Report',
                type: 'POST',
                //timeout: 3000,
                data: {
                    postID: PostID,
                    ToReport: true
                },
                success: function (response) {
                    if (response.Success == true) {
                        eventNotification('Zgłaszanie komentarza.', 'Komentarz został zgłoszony do moderacji.')
                    }
                    else {
                        eventNotification('Zgłaszanie komentarza.', response.errMessage, 'negative')
                    }
                },
                error: function () {
                    eventNotification('Zgłaszanie komentarza.', 'Wystąpił błąd', 'negative')
                }
            });
        });
    });

    //Remove post
    $(document).on('click', '.deletePost', function (event) {
        event.preventDefault();
        //event.stopPropagation();
        var $entComm = $(this);
        var PostID = $entComm.data('postid')

        $.ajax({
            url: '/ForumPost/Remove',
            type: 'POST',
            //timeout: 3000,
            data: {
                postID: PostID
            },
            success: function (response) {
                if (response.success == true) {
                    $entComm.closest(".singleComment").fadeOut(600, function () {
                        $entComm.closest(".singleComment").remove();
                    });

                }
                else {
                    eventNotification(response.errHeader, response.errMessage, 'negative')
                }

            },
            error: function () {
                eventNotification('Usuwanie komentarzy.', 'Wystąpił błąd podczas usuwania komentarza.', 'negative')
            }
        });
    });
});

function ajaxAddPost($comment, topicID, parentID, $template, $wherePrepend) {
    $.ajax({
        url: '/ForumPost/Add',
        type: 'POST',
        //timeout: 3000,
        data: {
            TopicID: topicID,
            CommentText: $comment.val(),            
            ParentID: parentID
        },
        success: function (response) {
            if (response.success == true) {
                fulfillPostCommentTemplate(response.postID, response.post, response.dateAdd, response.userName, response.userLogoLink, $template, $wherePrepend, $comment,  parentID);
            } else {
                eventNotification(response.errHeader, response.errMessage, 'negative');
            }
        },
        error: function () {
            eventNotification('Dodawanie komentarza.', 'Wystąpił błąd podczas dodawania komentarza.', 'negative')
        }
    });
};

function fulfillPostCommentTemplate(postID, post, date, userN, userL, $template, $wherePrepend, $comment, parentID) {
    var commentVariables =
    {
        PostID: postID,
        Post: post,
        DateAdd: date,
        UserName: userN,
        UserLogo: userL
        //,
        //CommentFaktV: 0,
        //CommentFakeV: 0,
        //class1: "outline",
        //class2: "outline",
        //positiveCount: repPcom,
        //allCount: repAcom,
        //reputationPoints: repPoins
    };
    var html = Mustache.to_html($template, commentVariables);

    if (parentID != 0 && $wherePrepend.is(":hidden")) {
        $wherePrepend.fadeToggle();
    }

    $('#noResultTab').addClass("hidden");
    $('#sortingTab').removeClass("hidden");

    $comment.val("");
    $(html).hide().prependTo($wherePrepend).fadeIn('slow');
};

function ajaxAddTopic($catID, $sub, $desc) {
    $.ajax({
        url: '/Forum/Add',
        type: 'POST',
        //timeout: 3000,
        data: {
            catID: $catID,
            sub: $sub,
            desc: $desc
        },
        success: function (response) {
            if (response.success == true) {
                eventNotification(response.errHeader, response.errMessage);
            } else {
                eventNotification(response.errHeader, response.errMessage, 'negative');
            }
        },
        error: function () {
            eventNotification('Dodawanie komentarza.', 'Wystąpił błąd podczas dodawania komentarza.', 'negative')
        }
    });
};

function showPosts(Filter) {
    $('#loadingImage').removeClass("hidden");

    var $template = $('#forumPostTemplate').html();
    var $commentList = $("#commentsList");

    function fulfillCommTemplate(postID, post, date, userN, userL) {

        //var repValue = "";
        //if (repV != 0) {
        //    repValue = "odpowiedzi: " + repV;
        //}

        //var c1 = "outline";
        //var c2 = "outline";

        //if (commV == 0) {
        //    c2 = "";
        //}
        //else if (commV == 1) {
        //    c1 = "";
        //}

        var replyVariables =
        {
            PostID: postID,
            Post: post,
            DateAdd: date,
            UserName: userN,
            UserLogo: userL
        };
        var html = Mustache.to_html($template, replyVariables);
        //$comment.val("");
        $(html).hide().prependTo($commentList).fadeIn('fast');
    }

    var topicID = $(".frmTopicID").val();


    $.ajax({
        url: '/ForumPost/Get',
        type: "GET",
        data: {
            TopicID: topicID,
            filter: Filter
        },
        success: function (result) {

            $('#sortingTab').addClass("hidden");
            $('#noResultTab').addClass("hidden");
            $('#loadingImage').removeClass("hidden");

            $.each(result, function (key, val) {
                fulfillCommTemplate(val.postID, val.post, val.dateAdd, val.userName, val.userLogoLink)
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
            eventNotification('Pobieranie komentarzy.', 'Wystąpił błąd podczas pobierania komentarzy.', 'negative')
        }
    });
};
