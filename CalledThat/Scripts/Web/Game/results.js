$(function () {
    $('.demo2').bootpag({
        total: @Model.TotalGameweeks,
    page: "@Model.Gameweek",
    maxVisible: 10
        }).on('page', function (event, num) {
        $.ajax({
            url: "@Url.Action("Results", "Game")",
            data: { week: num, playerId: "@Model.PlayerId" },
            success: function (data) {
                $('#results-container').html(data);
                $('#gameweek-number').html(num);
            }
        });
    });
    });