$(document).ready(function () {

    tinymce.init({
        selector: "#topicDescription",
        encoding: "xml"
    });

    $('.rTopic').click(function () {
        $('.forumList .post').removeClass('activePane');
        $('.rightTable a').removeClass('activePane');
        $(this).closest('.post').addClass('activePane');
        //$('#editTags').addClass('hidden');
    });


    //Show PostReplies
    $(document).on('click', '#PostContainerJS', function (event) {
        event.stopPropagation();
        event.preventDefault();

        var $parentID = $(this).data('postid')
        var $template = $('#replyPostPattern').html();
        var $replyList = $(this).closest('.singleComment').find('.reply-list');

        if ($replyList.is(":hidden")) {

            $.ajax({
                url: '/ForumPost/GetReplies',
                type: "GET",
                data: { parentID: $parentID },
                success: function (result) {

                    $(this).closest('li').find('.reply-list').fadeToggle();

                    $.each(result, function (key, val) {
                        fulfillPostReplyTemplate(val.postID, val.post, val.dateAdd, val.logoName, val.userName, $template, $replyList)
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
        var $notifBox = $('#SingleTopic');
        var $comment = $('#frmCommentText');
        var $topicID = $('.frmTopicID').val();
        var $template = $('#forumPostTemplate').html();
        var $commentList = $("#commentsList");

        ajaxAddPost($comment, $topicID, 0, $template, $commentList, $notifBox)
    });

    //Add Reply
    $(document).on('submit', '.frmAddPostReply', function (event) {
        event.preventDefault();
        var $notifBox = $(this).closest('.commentBoxJQ');
        var $post = $(this).closest('.addPostReply').find('.message');
        var $topicID = $('.frmTopicID').val();
        var $parentID = $(this).find('.postID').val();
        var $template = $('#replyPostPattern').html();
        var $replyList = $(this).closest('.singleComment').find('.reply-list');

        ajaxAddPost($post, $topicID, $parentID, $template, $replyList, $notifBox)
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
                        if ($Post.closest('.comment-box').find('.reportSpan').hasClass('hidden')) {
                            showNotification('positive', 'Zgłaszanie komentarza.', 'Komentarz został zgłoszony do moderacji.', $Post.closest('.commentBoxJQ'))
                            $Post.closest('.comment-box').find('.reportSpan').removeClass('hidden');
                        }
                        else {
                            showNotification('negative', 'Zgłaszanie komentarza.', 'Komentarz został już zgłoszony.', $Post.closest('.commentBoxJQ'))
                        }
                        
                    }
                    else {
                        showNotification('negative', 'Zgłaszanie komentarza.', response.errMessage, $Post.closest('.commentBoxJQ'))
                    }
                },
                error: function () {
                    showNotification('negative', 'Zgłaszanie komentarza.', 'Wystąpił błąd', $Post.closest('.commentBoxJQ'))
                }
            });
        });
    });

    //Remove reply
    $(document).on('click', '.deletePostReply', function (event) {
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
                if (response.success == true && response.childComments == 0) {

                    $entComm.closest(".singleComment").fadeOut(600, function () {
                        $entComm.closest(".singleComment").remove();
                    });

                }
                else if (response.success == true && response.childComments > 0) {
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
});

function fulfillPostReplyTemplate(postID, post, dateAdd, logoName, userName, $template, $replyList) {
    var replyVariables =
    {
        PostID: postID,
        Post: post,
        Date: dateAdd,
        LogoName: logoName,
        UserName: userName,
        //CommentFaktV: faktV,
        //CommentFakeV: fakeV,
        //class1: "outline",
        //class2: "outline",
        //positiveCount: repPcom,
        //allCount: repAcom,
        //reputationPoints: repPoins
    };
    var html = Mustache.to_html($template, replyVariables);

    $(html).hide().prependTo($replyList).fadeIn('slow');
};

function ajaxAddPost($comment, topicID, parentID, $template, $wherePrepend, $whereAppendNotif) {

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
                fulfillPostCommentTemplateForNewPost(response.postID, response.post, response.dateAdd, response.userName, response.userLogoLink, $template, $wherePrepend, $comment, parentID);
            } else {
                showNotification('negative', response.errHeader, response.errMessage, $whereAppendNotif)
            }
        },
        error: function () {
            showNotification('negative', 'Dodawanie komentarza.', 'Wystąpił błąd podczas dodawania komentarza.', $whereAppendNotif)
        }
    });
};

function fulfillPostCommentTemplateForNewPost(postID, post, date, userN, userL, $template, $wherePrepend, $comment, parentID, basicClass, removedClass, reportClass) {
    var commentVariables =
    {
        PostID: postID,
        Post: post,
        Date: date,
        UserName: userN,
        LogoName: userL,
        ReportedClass: "hidden",
        PostBasicClass: "",
        PostRemovedClass: "hidden"
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
    //$('#sortingTab').removeClass("hidden");

    $comment.val("");

    if (parentID == 0) {
        $(html).hide().prependTo($wherePrepend).fadeIn('slow');
    }
    else {
        $(html).hide().appendTo($wherePrepend).fadeIn('slow');
    }
    
};


function showPosts(Filter) {
    $('#loadingImage').removeClass("hidden");

    var $template = $('#forumPostTemplate').html();
    var $commentList = $("#commentsList");

    function fulfillCommTemplate(postID, post, date, userN, userL, repV, basicClass, removedClass, reportClass) {

        var repValue = "";
        if (repV != 0) {
            repValue = "odpowiedzi: " + repV;
        }

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
            Date: date,
            UserName: userN,
            LogoName: userL,
            RepliesV: repValue,
            ReportedClass: reportClass,
            PostBasicClass: basicClass,
            PostRemovedClass: removedClass
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
                fulfillCommTemplate(val.postID, val.post, val.dateAdd, val.userName, val.userLogoLink, val.repliesNumber, val.commentBasicClass, val.commentRemovedClass, val.reportedClass)
            });

            if (result.length == 0) {
                $('#noResultTab').removeClass("hidden");
            }
            else {
                //$('#sortingTab').removeClass("hidden");
            }

            $("#frmReplyText").MaxLength({
                MaxLength: 3000,
                CharacterCountControl: $('.charCounter')
            });

            $('#loadingImage').addClass("hidden");
        },
        error: function () {
            showNotification('negative', 'Pobieranie komentarzy.', 'Wystąpił błąd podczas pobierania komentarzy.', $commentList)
        }
    });
};
