﻿$(function () {
    $(document).on("click", ".deleteWish", function (e) {
        e.preventDefault();
        let Url = $(this).attr("href")

        fetch(Url)
            .then(response => {
                window.location.reload(true);
            })
    })
})