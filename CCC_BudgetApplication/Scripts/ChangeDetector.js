$(document).ready(function () {
    var _isDirty = false;
    $(':input').change(function () {
        _isDirty = true;
    });

    $('.cancel-btn').on("click", function () {
        _isDirty = false;
    });
    $('.save-btn').on("click", function () {
        _isDirty = false;
    });
    $('.emp-save-btn').on("click", function () {
        _isDirty = false;
    });

    window.onbeforeunload = function () {
        if (_isDirty) {
            return 'You have unsaved changes! Would you like to continue anyways?';
        }
    }
});