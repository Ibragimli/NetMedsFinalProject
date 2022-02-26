$(function () {
    $(document).on("click", ".addBasket", function (e) {
        e.preventDefault();
        let url = $(this).attr("href")
        fetch(url)
            .then(response => response.json())
            .then(data => {
                $(".basketHoverUp .basketLentgh").text(data.basketItems.length + "item(s)")
                $(".DownLeft  .salePrice").text("£" + data.totalAmount)
                $(".DownLeft #savePrice").text("£" + data.totalSave)

                $('#basketHoverMiddle').html("");
                for (var i = 0; i < data.basketItems.length; i++) {
                    let element = `
                                            <div class="itemBasket">
                                                <div class="basketName">
                                                    <a asp-action="detail" asp-controller="product" asp-route-productId=`+ data.basketItems[i].productId + `>
                                                        `+ data.basketItems[i].name + `
                                                    </a>
                                                </div>
                                                <div class="basketPriceCount">`+ data.basketItems[i].price + `x` + data.basketItems[i].count + `</div>
                                            </div>`
                    $('#basketHoverMiddle').append($(element));
                }
            })
    })
})