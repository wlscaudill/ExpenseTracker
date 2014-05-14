function loadDivWithBlock(id, url) {
    $("#"+id).block();
    $("#" + id).load(url, null, function () { $("#" + id).unblock(); });
}

function loadDivWithBlockTransparent(id, url) {
    $("#" + id).block({
        message: '',
        overlayCSS: {
            opacity: 0,
        },
    });
    
    $("#" + id).load(url, null, function () { $("#" + id).unblock(); });
}