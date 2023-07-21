var CartController = function () {
    this.initialize = function () {
        loadData();
        regsiterEvents();
            
    }
    function regsiterEvents() {
        // Write your JavaScript code.
        $('body').on('click', '.btn-update-cart', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const quantity = $('#txt_quantity_' + id).val();
            updateCart(id, quantity);
        });
        $('body').on('click', '.btn-remove-cart', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            updateCart(id, 0);
        });
    }
    function updateCart(id, quantity) {
        const culture = $('#hidCulture').val();
        $.ajax({
            type: "POST",
            url: '/Cart/UpdateCart',
            data: {
                id: id,
                quantity: quantity
            },
            success: function (res) {
                if (quantity !== 0) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Update successful!',
                        showConfirmButton: false,
                        timer: 1500
                    });
                } else {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Removed!',
                        showConfirmButton: false,
                        timer: 1000
                    });
                }
                $('#lbl_number_items_header').text(res.length);
                loadData();
            },
            error: function (err) {
                console.log(err);
            }
        });
    }

    function loadData() {
        const culture = $('#hidCulture').val();
        $.ajax({
            type: "GET",
            url: '/Cart/GetListCartItem',
            success: function (res) {
                if (res.length === 0) {
                    $('#tbl_cart').hide();
                }
                var html = '';
                var total = 0;

                $.each(res, function (i, item) {
                    var amount = item.price * item.quantity;
                    html += "<tr>"
                        + "<td> <img width=\"60\" src=\"" + $('#hidBaseAddress').val() +"/user-content/"+ item.image + "\" alt=\"\" /></td>"
                        + "<td>" + item.description + "</td>"
                        + "<td><div class=\"input-append\"><input class=\"span1\" style=\"max-width: 34px\" placeholder=\"" + item.quantity + "\" id=\"txt_quantity_" + item.productId + "\" value=\"" + item.quantity + "\"size=\"16\" type=\"text\">"
                        + "<button class=\"btn btn-update-cart\" data-id=\"" + item.productId + "\" type =\"button\"> <i class=\"icon-edit\"></i></button>"
                 
                        + "<button class=\"btn btn-danger btn-remove-cart\" type=\"button\" data-id=\"" + item.productId + "\"><i class=\"icon-remove icon-white\"></i></button>"
                        + "</div>"
                        + "</td>"

                        + "<td>" + numberWithCommas(item.price) + "</td>"
                        + "<td>" + numberWithCommas(amount) + "</td>"
                        + "</tr>";
                    total += amount;
                });
                $('#cart_body').html(html);
                $('#lbl_number_of_items').text(res.length);
                $('#lbl_total').text(numberWithCommas(total));
            }
        });
    }
}