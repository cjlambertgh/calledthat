$(function () {
    $(".toggle").bootstrapSwitch();

    $('input.toggle').on('change', function () {
        $('input.toggle').not(this).prop('checked', false);
    });

    $('input.toggle.banker').on('switchChange.bootstrapSwitch', function (event, state) {
        $('input.toggle.banker').not($(this)).bootstrapSwitch('state', false);
    });

    $('input.toggle.double').on('switchChange.bootstrapSwitch', function (event, state) {
        $('input.toggle.double').not($(this)).bootstrapSwitch('state', false);
    });
});
