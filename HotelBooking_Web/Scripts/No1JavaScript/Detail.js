
   var slideIndex = 1;

        // TỰ ĐỘNG CHẠY KHI TRANG LOAD XONG
        $(document).ready(function () {
            // 1. Lấy ID từ input ẩn
            var roomId = $('#currentRoomId').val();

            // 2. Gọi AJAX ngay lập tức
            loadRoomData(roomId);
        });

        function loadRoomData(id) {
            $.ajax({
                url: '/Rooms/GetRoomDetailJson',
                type: 'GET',
                data: { id: id },
                dataType: 'json',
                success: function (response) {
                    if (response.success) {
                        // Đổ dữ liệu text
                        $('#lblTenPhong').text(response.LoaiPhong + ' - Số: ' + response.TenPhong);
                        $('#lblLoaiPhong').text(response.LoaiPhong);
                        $('#lblGia').text(response.Gia.toLocaleString('vi-VN'));
                        $('#lblMoTa').html(response.MoTa);

                        // Đổ dữ liệu ảnh slide
                        var html = '';
                        $.each(response.DanhSachAnh, function (i, url) {
                            html += `<div class="my-slide fade"><img src="${url}" class="slide-img"></div>`;
                        });
                        $('#slider-wrapper').html(html);

                        // Khởi động slide
                        showSlides(1);
                    } else {
                        alert("Không tìm thấy thông tin phòng này!");
                    }
                },
                error: function () {
                    alert("Lỗi tải dữ liệu phòng.");
                }
            });
        }

    // --- LOGIC SLIDER (GIỮ NGUYÊN) ---
    var slideIndex = 1;

    function plusSlides(n) {
        showSlides(slideIndex += n);
    }

    function showSlides(n) {
        var i;
        var slides = document.getElementsByClassName("my-slide");
        
        // Nếu AJAX chưa chạy xong thì slides.length = 0, thoát hàm để tránh lỗi
        if (!slides || slides.length === 0) return;

        if (n > slides.length) { slideIndex = 1 }
        if (n < 1) { slideIndex = slides.length }
        
        for (i = 0; i < slides.length; i++) {
            slides[i].style.display = "none";
        }
        slides[slideIndex - 1].style.display = "block";
    }