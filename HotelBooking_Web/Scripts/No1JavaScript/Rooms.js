/* ========== ROOMS.JS ==========
   Trang danh sách phòng — HotelOne
   Hiển thị danh sách dựa trên query + bộ lọc
================================= */

// Gắn năm footer
document.getElementById("year").textContent = new Date().getFullYear();

/* --- Lấy dữ liệu từ URL --- */
const params = new URLSearchParams(window.location.search);
const checkin = params.get("checkin");
const checkout = params.get("checkout");
const guests = Number(params.get("guests")) || 1;

const summary = document.getElementById("searchSummary");
summary.textContent = checkin && checkout
    ? `Kết quả cho ${guests} khách · ${checkin} → ${checkout}`
    : "Tất cả các loại phòng";

/* --- Hiển thị danh sách mặc định --- */
renderRooms(ROOMS);

/* --- Lọc phòng theo giá và khách --- */
document.getElementById("filterBtn").addEventListener("click", () => {
    const maxPrice = Number(document.getElementById("maxPrice").value);
    const guestFilter = Number(document.getElementById("filterGuests").value);

    let filtered = [...ROOMS];

    if (maxPrice > 0) {
        filtered = filtered.filter((r) => r.price <= maxPrice);
    }
    if (guestFilter > 0) {
        filtered = filtered.filter((r) => r.guests >= guestFilter);
    }

    renderRooms(filtered);

    const labelParts = [];
    if (maxPrice) labelParts.push(`giá ≤ ${maxPrice}$`);
    if (guestFilter) labelParts.push(`${guestFilter}+ khách`);
    summary.textContent = labelParts.length
        ? `Lọc theo ${labelParts.join(", ")}`
        : "Tất cả các loại phòng";
});
