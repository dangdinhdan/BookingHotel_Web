/* ========== SCRIPT.JS ==========
   Trang chủ HotelOne
   Xử lý hiển thị phòng, tìm kiếm, modal và form đặt phòng
================================= */


/* --- SCROLL MƯỢT THÔNG MINH CHO MVC --- */
document.querySelectorAll('a[href*="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        // Lấy phần đường dẫn (path) và phần ID (hash)
        const currentPath = window.location.pathname; // Ví dụ: "/" hoặc "/Home/Index"
        const linkHref = this.getAttribute('href'); // Ví dụ: "/#rooms"

        // Tách dấu # ra: ["/", "rooms"]
        const [path, hash] = linkHref.split('#');

        // Kiểm tra: Nếu link trỏ về cùng trang hiện tại (hoặc là trang chủ /)
        if ((path === "" || path === "/" || path === currentPath) && hash) {
            const targetElement = document.getElementById(hash);
            if (targetElement) {
                e.preventDefault(); // Ngừng chuyển trang, chỉ cuộn thôi

                if (hash === "top") {
                    window.scrollTo({
                        top: 0,
                        behavior: "smooth"
                    });
                } else {
                    const headerOffset = 80; // Chỉnh số này bằng chiều cao menu của bạn
                    const elementPosition = targetElement.getBoundingClientRect().top;
                    const offsetPosition = elementPosition + window.scrollY - headerOffset;

                    window.scrollTo({
                        top: offsetPosition,
                        behavior: "smooth"
                    });
                }
            }
        }
    });
});

/* --- Modal hiển thị chi tiết phòng + form đặt --- */
const modalRoot = document.getElementById("modalRoot");

function openRoomModal(id, autoOpenBooking) {
    const room = ROOMS.find((r) => r.id === id);
    if (!room) return;

    modalRoot.innerHTML = `
    <div class="modal-backdrop">
      <div class="modal">
        <div style="display:flex;justify-content:space-between;align-items:center;padding:12px 18px;border-bottom:1px solid #eee;">
          <div style="font-weight:700">${room.title}</div>
          <button class="close-btn" id="modalClose">&times;</button>
        </div>

        <div class="modal-body">
          <div class="modal-gallery" style="background-image:url('${room.img}')"></div>
          <div class="modal-right">
            <div style="font-size:20px;font-weight:700">${formatCurrency(room.price)} <span style="color:#777;font-size:13px;">/đêm</span></div>
            <div style="color:#777;margin-top:8px">${room.beds} beds • ${room.guests} khách • ⭐ ${room.rating}</div>

            <div style="margin-top:10px;">
              <b>Tiện nghi:</b> ${room.amenities.join(" • ")}
            </div>

            <p style="margin-top:10px;color:#555">${room.description}</p>
            <hr style="margin:14px 0;border:none;border-top:1px solid #eee" />

            <form id="bookingForm" style="display:flex;flex-direction:column;gap:8px">
              <label>Ngày nhận</label>
              <input type="date" id="b-checkin" required>
              <label>Ngày trả</label>
              <input type="date" id="b-checkout" required>
              <label>Số khách</label>
              <input type="number" id="b-guests" value="1" min="1" max="${room.guests}">
              <label>Họ tên</label>
              <input type="text" id="b-name" placeholder="Nguyễn Văn A" required>
              <label>Email</label>
              <input type="email" id="b-email" placeholder="email@domain.com" required>

              <div style="display:flex;gap:8px;margin-top:6px">
                <button type="submit" class="btn btn-primary" style="flex:1">Xác nhận</button>
                <button type="button" class="btn btn-ghost" id="btnCancel">Hủy</button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>`;

    // Close modal
    document.getElementById("modalClose").addEventListener("click", closeModal);
    document.getElementById("btnCancel").addEventListener("click", closeModal);
    modalRoot
        .querySelector(".modal-backdrop")
        .addEventListener("click", (ev) => {
            if (ev.target.classList.contains("modal-backdrop")) closeModal();
        });

    // Submit booking form
    document
        .getElementById("bookingForm")
        .addEventListener("submit", (ev) => {
            ev.preventDefault();
            const checkin = document.getElementById("b-checkin").value;
            const checkout = document.getElementById("b-checkout").value;
            const guests = Number(document.getElementById("b-guests").value);
            const name = document.getElementById("b-name").value.trim();
            const email = document.getElementById("b-email").value.trim();

            if (new Date(checkin) > new Date(checkout)) {
                alert("Ngày nhận phải trước ngày trả.");
                return;
            }

            const nights = Math.max(
                1,
                Math.round(
                    (new Date(checkout) - new Date(checkin)) / (1000 * 60 * 60 * 24)
                )
            );
            const total = room.price * nights;
            const bookingId = "BK" + Math.random().toString(36).slice(2, 8).toUpperCase();

            closeModal();
            setTimeout(() => {
                alert(
                    `Đặt phòng thành công!\nMã đặt: ${bookingId}\nKhách: ${name}\nTổng: ${formatCurrency(
                        total
                    )}`
                );
            }, 200);
        });
}

/* --- Đóng modal --- */
function closeModal() {
    modalRoot.innerHTML = "";
}

/* --- ESC để đóng --- */
document.addEventListener("keydown", (e) => {
    if (e.key === "Escape") closeModal();
});

/* chuyen sang trang phong */
//document.getElementById("viewAllBtn").onclick = () => {
//    window.location.href = "/Room/Rooms.cshtml";
//};

document.addEventListener("DOMContentLoaded", () => {
    const btn = document.getElementById("viewAllBtn");
    if (btn) {
        btn.addEventListener("click", () => {
            window.location.href = "/Rooms/Rooms";
        });
    }
});
console.log("JS LOADED");

//document.getElementById("viewAllBtn")?.addEventListener("click", () => {
//    console.log("BTN CLICKED");
//    window.location.href = "/Rooms/SearchRooms";
//});

document.getElementById("viewAllBtn")?.addEventListener("click", () => {
    //console.log("BTN CLICKED");

    //// 1. Lấy giá trị từ các input
    //var checkinDate = document.getElementById("checkin").value;
    //var checkoutDate = document.getElementById("checkout").value;
    //var guestCount = document.getElementById("guests").value;

    //// 2. Xây dựng URL đích với các tham số (query string)
    //// Tên tham số (checkin, checkout, guests) phải khớp với Action trong Controller
    //var targetUrl = `/Rooms/SearchRooms?checkin=${checkinDate}&checkout=${checkoutDate}&guests=${guestCount}`;

    // 3. Điều hướng trang
    window.location.href = "/Rooms/SearchRooms";
});


document.addEventListener('DOMContentLoaded', function () {
    const track = document.getElementById('sliderTrack');
    
    const slides = track.querySelectorAll('.slide');

    // Tổng số slide (Bao gồm cả bản sao)
    const totalSlides = slides.length;
    let currentIndex = 0;
    const transitionTime = 800; // 800ms (0.8 giây) - Khớp với CSS transition nếu có
    const intervalTime = 4000;  // 4 giây chuyển ảnh 1 lần

    function runSlider() {
        // 1. Tăng index để chuyển sang slide tiếp theo
        currentIndex++;

        // Bật lại hiệu ứng chuyển động (vì có thể nó đã bị tắt ở bước reset)
        track.style.transition = `transform ${transitionTime}ms ease-in-out`;
        track.style.transform = `translateX(-${currentIndex * 100}%)`;

        // 2. Kiểm tra: Nếu đang ở slide cuối cùng (Slide Clone)
        if (currentIndex === totalSlides - 1) {
            // Đợi cho hiệu ứng trượt hoàn tất (sau 800ms)
            setTimeout(() => {
                // Tắt hiệu ứng chuyển động để người dùng không thấy cảnh tua lại
                track.style.transition = 'none';

                // Reset về slide đầu tiên (Slide 0)
                currentIndex = 0;
                track.style.transform = `translateX(0)`;

            }, transitionTime);
        }
    }

    // Chạy tự động
    setInterval(runSlider, intervalTime);
});