$(function () {
    $('.get-action').css("cursor", "pointer");
    $('.get-action').on('click', function () {
        var url = $(this).data('url');
        window.location = url;
    });
});