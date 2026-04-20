const $stepButtons = $('.step-button');
const $progress = $('#progress');

function setStep(step) {
    // Cập nhật giá trị của thanh tiến độ
    $progress.attr('value', step * 100 / ($stepButtons.length - 1));

    // Cập nhật trạng thái của các nút
    $stepButtons.each(function (index) {
        if (step >= index) {
            $(this).addClass('done');
        } else {
            $(this).removeClass('done');
        }
    });
}