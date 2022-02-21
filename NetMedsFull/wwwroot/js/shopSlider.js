$(function () {
    $('.owl-carousel-shop').owlCarousel({
        loop: true,
        autoplay: true,
        autoplayTimeout: 2000,
        autoplayHoverPause: true,
        nav: false,
        responsive: {
            0:{
                items:1
            },
            600:{
                items:1
            },
            1000:{
                items:1
            }
        }
    })
})