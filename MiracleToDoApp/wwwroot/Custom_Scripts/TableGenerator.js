$(document).ready(function () {

    $.ajax({
        url: '/Steps/BuildStepsTable',
        success: function (result) {
            $('#tableDiv').html(result);
        }
    });

}); 