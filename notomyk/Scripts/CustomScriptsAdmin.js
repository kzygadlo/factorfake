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

            {
                "data": "Id", "width": "50px", "render": function (data) {
                    return '<a class="" href="/AdminUserTable/Save/' + data + '">Edit</a>';
                }
            }
        ]
    })

    var adminSettings = $('#AdminSettings').DataTable({
        "ajax": {
            "url": '/AdminSettings/GetSettings/',
            "type": "get",
            "data": function (d) {
                d.type = $('#ifGlobal').val();
            },
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


    //$('.tablecontainer').on('click', 'a.popup', function (e) {
    //    e.preventDefault();
    //    OpenPopup($(this).attr('href'));
    //})
    //function OpenPopup(pageUrl) {
    //    var $pageContent = $('<div/>');
    //    $pageContent.load(pageUrl, function () {
    //        $('#popupForm', $pageContent).removeData('validator');
    //        $('#popupForm', $pageContent).removeData('unobtrusiveValidation');
    //        $.validator.unobtrusive.parse('form');

    //    });

    //    $dialog = $('<div class="popupWindow" style="overflow:auto"></div>')
    //        .html($pageContent)
    //        .dialog({
    //            draggable: false,
    //            autoOpen: false,
    //            resizable: false,
    //            model: true,
    //            title: 'Popup Dialog',
    //            height: 550,
    //            width: 600,
    //            close: function () {
    //                $dialog.dialog('destroy').remove();
    //            }
    //        })

    //    $('.popupWindow').on('submit', '#popupForm', function (e) {
    //        var url = $('#popupForm')[0].action;
    //        $.ajax({
    //            type: "POST",
    //            url: url,
    //            data: $('#popupForm').serialize(),
    //            success: function (data) {
    //                if (data.status) {
    //                    $dialog.dialog('close');
    //                    oTable.ajax.reload();
    //                }
    //            }
    //        })

    //        e.preventDefault();
    //    })
    //    $dialog.dialog('open');
    //}
})