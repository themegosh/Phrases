function showNotification(title, message, type) {

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "positionClass": "toast-bottom-right",
        "onclick": null,
        "showDuration": "1000",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    toastr[type](message, title);

}


window.onerror = function myErrorHandler(errorMsg, url, lineNumber) {
    
    $.ajax({
        method: "POST",
        url: '/tts/logthing/',
        data: {
            errorMsg: errorMsg,
            url: url,
            lineNumber: lineNumber
        }
    }).done(function () {
        console.log("Error logged.");
    });

    return false;
}

