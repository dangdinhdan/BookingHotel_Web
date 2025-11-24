/* ========== SEARCHROOMS.JS ==========
   Trang danh sách phòng — HotelOne
   Hiển thị danh sách dựa trên query + bộ lọc
================================= */

// Gắn năm footer
//document.getElementById("year").textContent = new Date().getFullYear();

const SEARCHROOMS = [
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

/* --- Tìm kiếm --- */
document.getElementById("searchBtn").addEventListener("click", () => {
    const guests = Number(document.getElementById("guests").value);
    //const checkin = document.getElementById("checkin").value;
    //const checkout = document.getElementById("checkout").value;

    //if (checkin && checkout && new Date(checkin) > new Date(checkout)) {
    //    alert("Ngày nhận phải trước ngày trả.");
    //    return;
    //}

    const filtered = ROOMS.filter((r) => r.guests >= guests);
    renderRooms(filtered);

    //let label = `${filtered.length} loại phòng`;
    //if (checkin && checkout) label += ` · ${checkin} → ${checkout}`;
    ////resultsInfo.textContent = label;

    document.getElementById("rooms").scrollIntoView({
        behavior: "smooth",
        block: "start",
    });
});