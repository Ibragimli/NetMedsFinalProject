$(function () {
    $(document).on("click", ".addBasket", function (e) {
        e.preventDefault();
        let url = $(this).attr("href")
        fetch(url)
            .then(response => response.json())
            .then(data => {
                $(".DownLeft .salePrice").text(data.basketItems.length)
                $(".DownLeft  .savePrice").text("£" + data.totalAmount)

                $('.basketHoverMiddle').html("");
                for (var i = 0; i < data.basketItems.length; i++) {
                    let element = `
                                        <div class="row">
                                            <div class="itemBasket">
                                                <div class="basketName">
                                                    <a asp-action="detail" asp-controller="product" asp-route-productId="`+ data.basketItems[i].productId + `">
                                                        `+ data.basketItems[i].name + `
                                                    </a>
                                                </div>
                                                <div class="basketPriceCount">`+ data.basketItems[i].price + `x` + data.basketItems[i].count + `</div>
                                            </div>
                                        </div>`
                    $('.basketHoverMiddle').append($(element));
                }
            })
    })
})