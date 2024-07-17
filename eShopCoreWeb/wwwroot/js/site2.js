var SiteController = function () {
    this.initialize = function () {
        regsiterEvents();
        loadCart();
        registerEvent2();
    }
    function loadCart() {
        const culture = $('#hidCulture').val();
        $.ajax({
            type: "GET",
            url: "/" + culture + '/Cart/GetListCartItem',
            success: function (res) {
                $('#lbl_number_items_header').text(res.length);
            }
        });
    }
    function regsiterEvents() {
        $('body').on('click', '.btn-add-cart', function (e) {
            e.preventDefault();
            const culture = $('#hidCulture').val();
            const id = $(this).data('id');
            $.ajax({
                type: "POST",
                url: "/" + culture + '/Cart/AddToCart',
                data: {
                    id: id,
                    languageId: culture,
                    cartQuantity: 1
                },
                success: function (res) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Add to cart successful!',
                        showConfirmButton: false,
                        timer: 1000
                    });
                    $('#lbl_number_items_header').text(res.length);
                },
                error: function (err) {
                    console.log(err);
                }
            });
        });
    }
    function registerEvent2(){
        $(document).ready(function () {
            // Xử lý sự kiện submit form
            $("#addToCartForm").on("submit", function (event) {
                // Ngăn chặn hành vi mặc định khi submit form
                event.preventDefault();

                // Lấy giá trị của số lượng sản phẩm
                const quantity = $("#cartProductQuantity").val();

                // Kiểm tra số lượng sản phẩm hợp lệ (khác 0 và là số nguyên dương)
                if (quantity <= 0 || !Number.isInteger(+quantity)) {
                    alert("Invalid quantity. Please enter a positive integer value.");
                    return;
                }

                // Lấy giá trị của các trường khác (productId, culture)
                const culture = $('#hidCulture').val();
                const id = $('#hidProductId').val();

                // Thực hiện yêu cầu Ajax để thêm sản phẩm vào giỏ hàng
                $.ajax({
                    type: "POST",
                    url: "/" + culture + '/Cart/AddToCart',
                    data: {
                        id: id,
                        languageId: culture,
                        cartQuantity: quantity
                    },
                    success: function (res) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Add to cart successful!',
                            showConfirmButton: false,
                            timer: 1000
                        });
                        $('#lbl_number_items_header').text(res.length);
                    },
                    error: function (err) {
                        console.log(err);
                    }
                });
            });
        });
    }
}


function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}