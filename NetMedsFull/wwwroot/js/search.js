$("#searchBoxInput").click(function () {
    $(".searchBox").show();
    $(".fa-xmark").show();
});
$(".fa-xmark").click(function () {
    $(".searchBox").hide();
    $(".fa-xmark").hide();
});