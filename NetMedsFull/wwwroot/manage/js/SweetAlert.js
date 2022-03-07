$(function () {
    $(document).on("click", ".delete-btn", function (e) {
        e.preventDefault();
        let url = $(this).attr("href");
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(url)
                    .then(response => {
                        if (response.ok) {
                            window.location.reload(true)
                        }
                        else {
                            alert("xeta bas verdi")
                        }
                    })
            }
            else {
                console.log("cancel")
            }
        })
    })
    //$(document).on("click", ".delete-imgBtn", function () {
    //    let bookId = $(this).attr("data-book-id")
    //    let Id = $(this).attr("data-id")
    //    console.log(`manage/book/edit/${bookId}/getImage/${Id}`);
    //    fetch(`manage/book/edit/bookId/getImage`)
    //        .then(response => {
    //            console.log(response)
    //            $(".imgBox").remove();
    //        })
    //})

})