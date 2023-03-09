var isLoadingShow = true;

$(document).ready(function () {
    $(document).ajaxStart(function (event, request, settings) {
        if (isLoadingShow) {
            $(".loading").css("display", "flex");
        }
        else {
            isLoadingShow = true;
        }
    });
    $(document).ajaxComplete(function () {
        $(".loading").css("display", "none");
    });
    $(document).ajaxStop(function () {
        $(".loading").css("display", "none");
    });

    $("a").on("click", function () {
        if ($(this).attr("href") == undefined || $(this).attr("href") == "" || $(this).attr("href") == "javascript:void(0)" || $(this).attr("href") == "javascript:void(0);" || $(this).attr("href").indexOf("#") == 0 ||
            $(this).attr("target") == "_blank" || $(this).hasClass("no-loading")) {
            return;
        }
        $(".loading").css("display", "flex");
        setTimeout(() => { $(".loading").css("display", "none") }, 6000);

    });


});

window.onload = function () {
    $(".loading").css("display", "none");
};