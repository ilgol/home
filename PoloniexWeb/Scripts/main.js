function is_child_of(parent, child) {
    if (child != null) {
        while (child.parentNode) {
            if ((child = child.parentNode) == parent) {
                return true;
            }
        }
    }
    return false;
}

function fixOnMouseOut(element, event, JavaScript_code) {
    var current_mouse_target = null;
    if (event.toElement) {
        current_mouse_target = event.toElement;
    } else if (event.relatedTarget) {
        current_mouse_target = event.relatedTarget;
    }
    if (!is_child_of(element, current_mouse_target) && element != current_mouse_target) {
        eval(JavaScript_code);
    }
}

document.querySelector('.toggle-button').addEventListener('click', function () {
    slideout.toggle();
});

function hideblock(element) {
    $(element).hide("slow");
}

var slideout;

function slideoutclose(eve) {
    eve.preventDefault();
    slideout.close();
}

$(window).scroll(function () {
    if ($(this).scrollTop() > window.innerHeight) {
        $('.back-to-down').fadeIn();
        $('.back-to-up').fadeIn();
    } else {
        $('.back-to-down').fadeOut();
        $('.back-to-up').fadeOut();
    }
});
$('.back-to-down').click(function () {
    $('body,html').animate({
        scrollTop: $(document).height()
    }, 400);
    return false;
});
$('.back-to-up').click(function () {
    $('body,html').animate({
        scrollTop: 0
    }, 400);
    return false;
});
var idleTimer = null;
var idleState = false;
var idleWait = 2000;
var idleTimermob = null;
var idleStatemob = false;
var idleWaitmob = 2000;
$(document).ready(function () {
    slideout = new Slideout({
        'panel': document.getElementById('panel'),
        'menu': document.getElementById('menu-tablet'),
        'padding': 250,
        'tolerance': 70
    });
    slideout.on('beforeopen', function () {
        this.panel.classList.add('slideout-overflow-open');
    }).on('open', function () {
        this.panel.addEventListener('click', slideoutclose);
    }).on('beforeclose', function () {
        this.panel.classList.remove('slideout-overflow-open');
        this.panel.removeEventListener('click', slideoutclose);
    });
    $('*').bind('mousemove touchmove keydown tap scroll scrollstart', function () {

        clearTimeout(idleTimer);

        if (idleState == true) {
            $('.back-to-up').fadeIn();
            $('.back-to-down').fadeIn();
        }

        idleState = false;

        idleTimer = setTimeout(function () { $('.back-to-up').fadeOut(); $('.back-to-down').fadeOut(); idleState = true; }, idleWait);
    });
    $('*').bind('touchmove tap scrollstart', function () {

        clearTimeout(idleTimermob);

        if (idleStatemob == true) {
            $('.back-to-up').fadeIn();
            $('.back-to-down').fadeIn();
        }

        idleStatemob = false;

        idleTimermob = setTimeout(function () { $('.back-to-up').fadeOut(); $('.back-to-down').fadeOut(); idleStatemob = true; }, idleWaitmob);
    });
});