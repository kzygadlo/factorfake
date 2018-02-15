$(document).ready(function () {

    var adminUserTable = $('#AdminUserTable').DataTable({
        "ajax": {
            "url": '/AdminUserTable/GetUsers',
            "type": "get",
            "datatype": "json"
        },
        "columns": [
            { "data": "UserName", "autoWidth": true },
            { "data": "Email", "autoWidth": true },
            { "data": "EmailConfirmed", "autoWidth": true },
            { "data": "RoleName", "autoWidth": true },
            { "data": "LastActivity", "autoWidth": true },
            { "data": "BanTo", "autoWidth": true },
            {
                "data": "Comm", "width": "50px", "render": function (data) {
                    var result = data.split(';')
                    return '<a class="" href="/AdminComments/IndexUserID/' + result[0] + '">' + result[1] + '</a>';
                }
            },
            {
                "data": "News", "width": "50px", "render": function (data) {
                    var result = data.split(';')
                    return '<a class="" href="/AdminNews/IndexUserID/' + result[0] + '">' + result[1] + '</a>';
                }
            },
            {
                "data": "Id", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminUserTable/Save/' + data + '">Edit</a>';
                }
            }
        ]
    })

    var adminSettings = $('#AdminSettingsTable').DataTable({
        "ajax": {
            "url": '/AdminSettings/GetSettings/',
            "type": "get",
            "datatype": "json"
        },
        "columns": [
            { "data": "Key", "autoWidth": true },
            { "data": "Value", "autoWidth": true },
            { "data": "Description", "autoWidth": true },

            {
                "data": "ID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminSettings/Save/' + data + '">Edit</a>';
                }
            }
        ]
    })

    var adminSettingsGlobal = $('#AdminSettingsGlobalTable').DataTable({
        "ajax": {
            "url": '/AdminSettingsGlobal/GetSettings/',
            "type": "get",
            "datatype": "json"
        },
        "columns": [
            { "data": "Key", "autoWidth": true },
            { "data": "Value", "autoWidth": true },
            { "data": "Description", "autoWidth": true },
            {
                "data": "ID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminSettingsGlobal/Save/' + data + '">Edit</a>';
                }
            }
        ]
    })

    var adminNewspapers = $('#AdminNewspaperTable').DataTable({
        "ajax": {
            "url": '/AdminNewspapers/GetNewspapers/',
            "type": "get",
            "datatype": "json"
        },
        "columns": [
            { "data": "tbl_NewspaperID", "autoWidth": true },
            { "data": "NewspaperName", "autoWidth": true },
            { "data": "NewspaperLink", "autoWidth": true },
            { "data": "NewspaperIconLink", "autoWidth": true },
            { "data": "IsActive", "autoWidth": true },
            {
                "data": "News", "width": "50px", "render": function (data) {
                    var result = data.split(';')
                    return '<a class="" href="/AdminNews/IndexNewspaperID/' + result[0] + '">' + result[1] + '</a>';
                }
            },
            {
                "data": "tbl_NewspaperID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminNewspapers/Save/' + data + '">Edit</a>';
                }
            },
            {
                "data": "tbl_NewspaperID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminNewspapers/Remove/' + data + '">Del</a>';
                }
            }
        ]
    })


    var adminComments = $('#AdminCommentTable').DataTable({
        "ajax": {
            "url": '/AdminComments/GetComments',
            "type": "get",
            "data": function (d) {
                d.newsID = $('#newsID').val();
                d.userID = $('#userID').val();
                d.parentID = $('#parentID').val();
            },
            "datatype": "json"
        },
        "columnDefs": [
            { "className": "breakLine350", "targets": [1] }
        ],
        "columns": [

            { "data": "UserName", "autoWidth": true },
            { "data": "Comment", "autoWidth": false },
            { "data": "Fakt", "autoWidth": true },
            { "data": "Fake", "autoWidth": true },
            { "data": "IsReported", "autoWidth": true },
            { "data": "IsActive", "autoWidth": true },
            { "data": "Date", "autoWidth": true },
            {
                "data": "Child", "width": "50px", "render": function (data) {
                    var result = data.split(';')
                    return '<a class="" href="/AdminComments/Index/' + result[0] + '">' + result[1] + '</a>';
                }
            },

            {
                "data": "tbl_CommentID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminComments/Save/' + data + '">Edit</a>';
                }
            }
        ]
    })

    var adminTags = $('#AdminTagsTable').DataTable({
        "ajax": {
            "url": '/AdminTag/GetTags',
            "type": "get",
            "datatype": "json"
        },
        "columns": [
            { "data": "TagName", "autoWidth": true },
            { "data": "HowManyTimesUsed", "autoWidth": true },
            //{ "data": "IsActive", "autoWidth": true },
            //{ "data": "Comment", "width": "100px" },

            {
                "data": "ID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminTag/Save/' + data + '">Edit</a>';
                }
            },
            {
                "data": "ID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminTag/Save/' + data + '">Del</a>';
                }
            }
        ]
    })

    var adminNewses = $('#AdminNewsesTable').DataTable({
        "ajax": {
            "url": '/AdminNews/GetNewses',
            "type": "get",
            "data": function (d) {
                d.newspaperID = $('#newspaperID').val();
                d.userID = $('#userID').val();
            },
            "datatype": "json"
        },
        "columns": [
            {
                "data": "tbl_NewsID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/Main/News/' + data + '">' + data + '</a>';
                }
            },
            { "data": "NewspaperName", "autoWidth": true },
            { "data": "Title", "autoWidth": true },
            { "data": "IsReported", "autoWidth": true },
            { "data": "IsActive", "autoWidth": true },
            { "data": "DateAdd", "autoWidth": true },
            { "data": "Visitors", "autoWidth": true },
            { "data": "faktValue", "autoWidth": true },
            { "data": "fakeValue", "autoWidth": true },

            {
                "data": "Comm", "width": "50px", "render": function (data) {
                    var result = data.split(';')
                    return '<a class="" href="/AdminComments/IndexNewsID/' + result[0] + '">' + result[1] + '</a>';
                }
            },
            {
                "data": "tbl_NewsID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminNews/Save/' + data + '">Edit</a>';
                }
            }
        ]
    })



    var adminForumCategories = $('#AdminForumCategoriesTable').DataTable({
        "ajax": {
            "url": '/AdminForumCategory/GetForumCategories',
            "type": "get",
            "datatype": "json"
        },
        "columns": [
            { "data": "CategoryName", "autoWidth": true },
            { "data": "IconClass", "autoWidth": true },
            { "data": "Order", "autoWidth": true },
            { "data": "HowManyTopics", "autoWidth": true },

            {
                "data": "ID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminForumCategory/Save/' + data + '">Edit</a>';
                }
            },
            {
                "data": "ID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminForumCategory/Save/' + data + '">Remove</a>';
                }
            }
        ]
    })


    var adminForumPosts = $('#AdminForumPostsTable').DataTable({
        "ajax": {
            "url": '/AdminForumPost/GetForumPosts',
            "type": "get",
            "datatype": "json"
        },
        "columns": [
            { "data": "UserName", "autoWidth": true },
            { "data": "Content", "autoWidth": true },
            { "data": "IsReported", "autoWidth": true },

            {
                "data": "ID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminForumPost/Save/' + data + '">Edit</a>';
                }
            },
            {
                "data": "ID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminForumPost/Save/' + data + '">Remove</a>';
                }
            }
        ]
    })


    var adminForumTopics = $('#AdminForumTopicsTable').DataTable({
        "ajax": {
            "url": '/AdminForumTopic/GetForumTopics',
            "type": "get",
            "datatype": "json"
        },
        "columns": [
            { "data": "UserName", "autoWidth": true },
            { "data": "Description", "autoWidth": true },
            { "data": "IsActive", "autoWidth": true },

            {
                "data": "ID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminForumTopic/Save/' + data + '">Edit</a>';
                }
            },
            {
                "data": "ID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminForumTopic/Save/' + data + '">Remove</a>';
                }
            }
        ]
    })

    var adminVoteNews = $('#AdminNewsVotesTable').DataTable({
        "ajax": {
            "url": '/AdminVotingNews/GetNewsVotes',
            "type": "get",
            "datatype": "json"
        },
        "columns": [
            { "data": "UserName", "autoWidth": true },
            { "data": "tbl_NewsID", "autoWidth": true },
            { "data": "Vote", "autoWidth": true },

            {
                "data": "VoteLogID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminVotingNews/Save/' + data + '">Edit</a>';
                }
            },
            {
                "data": "VoteLogID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminVotingNews/Save/' + data + '">Remove</a>';
                }
            }
        ]
    })


    var adminVoteComments = $('#AdminCommentsVotesTable').DataTable({
        "ajax": {
            "url": '/AdminVotingComment/GetCommentVotes',
            "type": "get",
            "datatype": "json"
        },
        "columns": [
            { "data": "UserName", "autoWidth": true },
            { "data": "tbl_CommentID", "autoWidth": true },
            { "data": "Vote", "autoWidth": true },

            {
                "data": "VoteCommentLogID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminVotingComment/Save/' + data + '">Edit</a>';
                }
            },
            {
                "data": "VoteCommentLogID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminVotingComment/Save/' + data + '">Remove</a>';
                }
            }
        ]
    })


    var adminBlackList = $('#AdminBlackListTable').DataTable({
        "ajax": {
            "url": '/AdminBlackList/GetBlackList',
            "type": "get",
            "datatype": "json"
        },
        "columns": [
            { "data": "url", "autoWidth": true },

            {
                "data": "ID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminBlackList/Save/' + data + '">Edit</a>';
                }
            },
            {
                "data": "ID", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminBlackList/Remove/' + data + '">Remove</a>';
                }
            }
        ]
    })


})