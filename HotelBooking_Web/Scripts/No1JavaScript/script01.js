/* ========== SCRIPT.JS ==========
   Trang chủ HotelOne
   Xử lý hiển thị phòng, tìm kiếm, modal và form đặt phòng
================================= */

/* --- Mock dữ liệu phòng --- */
const ROOMS = [
    {
        id: 1,
        title: "Superior Sea View",
        price: 120,
        beds: 2,
        guests: 3,
        rating: 3.6,
        img: "https://images.unsplash.com/photo-1501117716987-c8e7b7b3b6b3?q=80&w=1200&auto=format&fit=crop",
        amenities: ["WiFi", "Breakfast", "AC"],
        description: "Phòng hướng biển, có ban công, view đẹp.",
    },
    {
        id: 2,
        title: "Deluxe King Room",
        price: 150,
        beds: 1,
        guests: 2,
        rating: 3.6,
        img: "https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?q=80&w=1200&auto=format&fit=crop",
        amenities: ["WiFi", "Pool", "Gym"],
        description: "Phòng rộng với giường king, thích hợp cho cặp đôi.",
    },
    {
        id: 3,
        title: "Family Suite",
        price: 200,
        beds: 3,
        guests: 5,
        rating: 3.6,
        img: "https://images.unsplash.com/photo-1554995207-c18c203602cb?q=80&w=1200&auto=format&fit=crop",
        amenities: ["Kitchen", "Parking", "Breakfast"],
        description: "Suite cho gia đình, phòng khách tách biệt.",
    },
    {
        id: 4,
        title: "Budget Single",
        price: 45,
        beds: 1,
        guests: 1,
        rating: 3.6,
        img: "https://images.unsplash.com/photo-1505691723518-36a0f3d9d6b2?q=80&w=1200&auto=format&fit=crop",
        amenities: ["WiFi"],
        description: "Phòng giá rẻ, gọn nhẹ, thích hợp ở 1 người.",
    },
    {
        id: 5,
        title: "Executive Room",
        price: 180,
        beds: 2,
        guests: 3,
        rating: 3.6,
        img: "https://images.unsplash.com/photo-1560343090-f0409e92791a?q=80&w=1200&auto=format&fit=crop",
        amenities: ["WiFi", "Workspace", "AC"],
        description: "Phòng dành cho doanh nhân, có bàn làm việc.",
    },
];

///* --- Gắn năm hiện tại vào footer --- */
//document.getElementById("year").textContent = new Date().getFullYear();

/* --- Hàm hiển thị danh sách phòng --- */
const roomsGrid = document.getElementById("roomsGrid");
const resultsInfo = document.getElementById("resultsInfo");

function formatCurrency(num) {
    return new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "USD",
        maximumFractionDigits: 0,
    }).format(num);
}

function renderRooms(list) {
    roomsGrid.innerHTML = "";

    if (list.length === 0) {
        roomsGrid.innerHTML =
            '<div style="grid-column:1/-1;padding:24px;background:white;border-radius:10px;text-align:center">Không tìm thấy phòng phù hợp.</div>';
        resultsInfo.textContent = "0 kết quả";
        return;
    }

    resultsInfo.textContent = `${list.length} loại phòng`;

    list.forEach((room) => {
        const card = document.createElement("article");
        card.className = "card";
        card.innerHTML = `
      <div class="thumb" style="background-image:url('${room.img}')"></div>
      <div class="card-body">
        <div class="room-title">${room.title}</div>
        <div class="room-meta">${room.beds} beds • ${room.guests} khách • ⭐ ${room.rating}</div>
        <div>${room.amenities
                .slice(0, 3)
                .map((a) => `<span class="pill">${a}</span>`)
                .join("")}</div>
        <div class="room-bottom">
          <div class="price">${formatCurrency(room.price)} / đêm</div>
          <div style="display:flex;gap:8px">
            <button class="btn btn-ghost" data-id="${room.id}" data-action="view">Xem</button>
            <button class="btn btn-primary" data-id="${room.id}" data-action="book">Đặt</button>
          </div>
        </div>
      </div>`;
        roomsGrid.appendChild(card);
    });
}

/* --- Hiển thị phòng mặc định --- */
renderRooms(ROOMS);

/* --- Tìm kiếm --- */
document.getElementById("searchBtn").addEventListener("click", () => {
    const guests = Number(document.getElementById("guests").value);
    const checkin = document.getElementById("checkin").value;
    const checkout = document.getElementById("checkout").value;

    if (checkin && checkout && new Date(checkin) > new Date(checkout)) {
        alert("Ngày nhận phải trước ngày trả.");
        return;
    }

    const filtered = ROOMS.filter((r) => r.guests >= guests);
    renderRooms(filtered);

    let label = `${filtered.length} loại phòng`;
    if (checkin && checkout) label += ` · ${checkin} → ${checkout}`;
    resultsInfo.textContent = label;

    document.getElementById("rooms").scrollIntoView({
        behavior: "smooth",
        block: "start",
    });
});

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

roomsGrid.addEventListener("click", (e) => {
    const btn = e.target.closest("button");
    if (!btn) return;
    const action = btn.dataset.action;
    const id = Number(btn.dataset.id);

    if (action === "view") openRoomModal(id, false);
    if (action === "book") openRoomModal(id, true);
});

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
