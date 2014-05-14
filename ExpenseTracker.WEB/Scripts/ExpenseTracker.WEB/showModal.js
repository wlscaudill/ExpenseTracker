function loginShowModal(title, url, finishedUrl, resizable, minWidth, minHeight, maxWidth, maxHeight) {
    var intWidth = $(window).width();
    var intHeight = $(window).height();

    var xCenter = intWidth / 2;
    // offset slightly to top
    var yCenter = (intHeight * 0.9) / 2;

    xCenter = xCenter - (maxWidth / 2);
    yCenter = yCenter - (maxHeight / 2);

    var win = $("<div></div>");

    var height = maxHeight;

    if (resizable == "true") {
        height = "auto";
    }

    win.dialog({
        position: [xCenter, yCenter],
        title: title,
        modal: true,
        resizable: resizable,
        minWidth: minWidth,
        minHeight: minHeight,
        maxWidth: maxWidth,
        maxHeight: maxHeight,
        height: height,
        show: 'fade',
        hide: 'fade',
        closeOnEscape: false,
        open: function () {
            win.block();
            win.load(url, null, function () { win.unblock(); });
        },
        close: function () {
            $("#mainDiv").block();
            $("#mainDiv").load(finishedUrl, null, function () { $("#mainDiv").unblock(); });
            $('#loginModal').remove();
        },
    });

    var buttons = [];
    win.dialog("option", "buttons", buttons);
    win.attr('id', 'loginModal');
    $(".ui-dialog-titlebar-close").hide(); // hide close button
    win.show();
}


function editExpenseShowModal(title, url, finishedUrl, resizable, minWidth, minHeight, maxWidth, maxHeight) {
    var intWidth = $(window).width();
    var intHeight = $(window).height();

    var xCenter = intWidth / 2;
    // offset slightly to top
    var yCenter = (intHeight * 0.9) / 2;

    xCenter = xCenter - (maxWidth / 2);
    yCenter = yCenter - (maxHeight / 2);

    var win = $("<div></div>");

    var height = maxHeight;

    if (resizable == "true") {
        height = "auto";
    }

    win.dialog({
        position: [xCenter, yCenter],
        title: title,
        modal: true,
        resizable: resizable,
        minWidth: minWidth,
        minHeight: minHeight,
        maxWidth: maxWidth,
        maxHeight: maxHeight,
        height: height,
        show: 'fade',
        hide: 'fade',
        close: function () {
            $("#mainDiv").block();
            $("#mainDiv").load(finishedUrl, null, function () { $("#mainDiv").unblock(); });
            $('#editModal').remove();
        },
        open: function () {
            win.block();
            win.load(url, null, function () { win.unblock(); });
        }
    });

    var buttons = [];
    win.dialog("option", "buttons", buttons);
    win.attr('id', 'editModal');
    win.show();
}



function printReportShowModal(title, url, finishedUrl, resizable, minWidth, minHeight, maxWidth, maxHeight) {
    var intWidth = $(window).width();
    var intHeight = $(window).height();

    var xCenter = intWidth / 2;
    // offset slightly to top
    var yCenter = (intHeight * 0.9) / 2;

    xCenter = xCenter - (maxWidth / 2);
    yCenter = yCenter - (maxHeight / 2);

    var win = $("<div></div>");

    var height = maxHeight;

    if (resizable == "true") {
        height = "auto";
    }

    win.dialog({
        position: [xCenter, yCenter],
        title: title,
        modal: true,
        resizable: resizable,
        minWidth: minWidth,
        minHeight: minHeight,
        maxWidth: maxWidth,
        maxHeight: maxHeight,
        height: height,
        show: 'fade',
        hide: 'fade',
        close: function () {
            $("#mainDiv").block();
            $("#mainDiv").load(finishedUrl, null, function () { $("#mainDiv").unblock(); });
            $('#reportModal').remove();
        },
        open: function () {
            win.block();
            win.load(url, null, function () { win.unblock(); });
        }
    });

    var buttons = [];
    win.dialog("option", "buttons", buttons);
    win.attr('id', 'reportModal');
    win.show();
}


function deleteExpenseShowModalConfirm(title, content, finishedUrl, deleteServiceCallUrl, successPrefix) {
    var maxWidth = 300;
    var maxHeight = 200;

    var intWidth = $(window).width();
    var intHeight = $(window).height();

    var xCenter = intWidth / 2;
    // offset slightly to top
    var yCenter = (intHeight * 0.9) / 2;

    var minWidth = maxWidth;
    var minHeight = maxHeight;

    xCenter = xCenter - (maxWidth / 2);
    yCenter = yCenter - (maxHeight / 2);


    var win = $("<div>" + content + "</div>");
    win.dialog({
        position: [xCenter, yCenter],
        title: title,
        modal: true,
        maxWidth: maxWidth,
        maxHeight: maxHeight,
        minWidth: minWidth,
        minHeight: minHeight,
        show: 'fade',
        hide: 'fade',
        close: function () {
            $('#confirmDeleteExpenseModal').remove();
        },
        buttons: {
            "Confirm": function () {
                $.ajax({
                    url: deleteServiceCallUrl,
                    success: function (data) {
                        if (data.indexOf(successPrefix) != -1) {
                            $("#mainDiv").block();
                            $("#mainDiv").load(finishedUrl, null, function () { $("#mainDiv").unblock(); });
                            $('#confirmDeleteExpenseModal').dialog("close");
                        } else {
                            alert('Failed to delete: ' + data);
                        }
                    },
                    error: function(data) {
                        alert('Failed to delete, please try again.');
                    }
                });
            },
            "Cancel": function() {
                $('#confirmDeleteExpenseModal').dialog("close");
            }
        }
    });
    
    win.attr('id', 'confirmDeleteExpenseModal');
    win.show();

}