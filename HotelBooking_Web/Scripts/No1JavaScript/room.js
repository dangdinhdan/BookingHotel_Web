/* ========== ROOM.JS ==========
   Trang chi tiết phòng — HotelOne
   Hiển thị thông tin 1 phòng theo ID trong URL
================================= */

document.getElementById("year").textContent = new Date().getFullYear();

const params = new URLSearchParams(window.location.search);
const roomId = Number(params.get("id"));

const section = document.getElementById("roomDetail");
const room = ROOMS.find(r => r.id === roomId);

if (!room) {
    section.innerHTML = `
    <div style="background:white;padding:40px;border-radius:10px;text-align:center">
      <h2>Không tìm thấy phòng!</h2>
      <p class="muted">Vui lòng quay lại <a href="rooms.html">danh sách phòng</a>.</p>
    </div>`;
} else {
    section.innerHTML = `
    <div class="room-detail">
      <div class="room-images">
        <img src="${room.img}" alt="${room.title}" class="room-main-img">
      </div>

      <div class="room-info">
        <h2>${room.title}</h2>
        <div class="muted">${room.beds} beds • ${room.guests} khách • ⭐ ${room.rating}</div>

        <p style="margin-top:10px">${room.description}</p>

        <div style="margin-top:14px">
          <b>Tiện nghi:</b><br>
          ${room.amenities.map(a => `<span class="pill">${a}</span>`).join("")}
        </div>

        <div class="price" style="font-size:22px;margin-top:18px">
          ${new Intl.NumberFormat("en-US", { style: "currency", currency: "USD" }).format(room.price)} <span class="muted">/đêm</span>
        </div>

        <div style="display:flex;gap:10px;margin-top:20px">
          <button class="btn btn-primary" id="bookNowBtn">Đặt phòng ngay</button>
          <button class="btn btn-ghost" onclick="window.history.back()">Quay lại</button>
        </div>
      </div>
    </div>
  `;

    document.getElementById("bookNowBtn").addEventListener("click", () => {
        openRoomModal(roomId, true);
    });
}
