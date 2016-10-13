
$(document).ready(function () {

    $('.fixedElement').stickThis({
        top: 50
    });



    if (document.getElementById('waypoint') != null) {
        var waypoint = new Waypoint({
            element: document.getElementById('waypoint'),
            handler: function (direction) {
                if (direction == 'down') {
                    $('.cloned').css('visibility', 'hidden');
                    $('.original').css('visibility', 'hidden');
                }
                else {
                    $('.cloned').css('visibility', 'visible');
                    $('.original').css('visibility', 'visible');
                }

            }
        })
    }

});
