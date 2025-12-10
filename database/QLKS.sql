IF NOT EXISTS (SELECT 1 FROM sys.Databases WHERE name = 'QLKS')
    EXEC('CREATE DATABASE QLKS');
GO
USE QLKS
go

CREATE TABLE tbl_VaiTro(
	VaiTro NVARCHAR(50) PRIMARY KEY ,
	Create_at DATETIME2 DEFAULT SYSUTCDATETIME(),
	isDelete BIT DEFAULT 0,
	Delete_at DATETIME2 NULL
);


CREATE TABLE tbl_TaiKhoan(
	TaiKhoanID INT PRIMARY KEY IDENTITY(1,1),
	MaTK AS ('TK' + RIGHT('000000' + CAST(TaiKhoanID AS VARCHAR(10)), 3)) PERSISTED,
	HoTen NVARCHAR(100) NOT NULL,
	Email VARCHAR(100) NOT NULL,
	MatKhau VARCHAR(255),
	SoDienThoai VARCHAR(10),
	DiaChi NVARCHAR(255),
	Create_at DATETIME2 DEFAULT SYSUTCDATETIME(),
	VaiTro NVARCHAR(50) NOT NULL REFERENCES tbl_VaiTro(VaiTro) DEFAULT 'customer',
	Update_at DATETIME2 NULL,
	isDelete BIT DEFAULT 0,
	Delete_at DATETIME2 NULL
)

CREATE TABLE tbl_LoaiPhong(
	LoaiPhongID INT PRIMARY KEY IDENTITY(1,1),
	TenLoaiPhong NVARCHAR(100) not null,
	MoTa NVARCHAR(4000),
	Create_at DATETIME2 DEFAULT SYSUTCDATETIME(),
	isDelete BIT DEFAULT 0,
	Update_at DATETIME2 NULL,
	Delete_at DATETIME2 NULL
);

CREATE TABLE tbl_Phong(
	PhongID INT PRIMARY KEY IDENTITY(1,1),
	SoPhong VARCHAR(10) NOT NULL,
	LoaiPhongID INT NOT NULL REFERENCES tbl_LoaiPhong(LoaiPhongID),
	GiaMoiDem DECIMAL(18,2) NOT NULL CHECK(GiaMoiDem >= 0) DEFAULT 0,
	SucChuaToiDa INT CHECK(SucChuaToiDa > 0) DEFAULT 1,
	MoTa NVARCHAR(4000),
	TrangThai NVARCHAR(50) DEFAULT 'Available',
	HinhAnh VARCHAR(MAX),
	Create_at DATETIME2 DEFAULT SYSUTCDATETIME(),
	isDelete BIT DEFAULT 0,
	Update_at DATETIME2 NULL,
	Delete_at DATETIME2 NULL
);

CREATE TABLE tbl_PhongImages(
	PhongImages INT PRIMARY KEY IDENTITY(1,1),
	PhongID INT NOT NULL REFERENCES tbl_Phong(PhongID),
	Url VARCHAR(MAX),
);

CREATE TABLE tbl_DatPhong(
	DatPhongID INT PRIMARY KEY IDENTITY(1,1),
	TaiKhoanID INT NOT NULL REFERENCES tbl_TaiKhoan(TaiKhoanID),
	NgayDat DATETIME2 DEFAULT SYSUTCDATETIME(),
	NgayNhanPhong DATETIME2 NOT NULL,
	NgayTraPhong DATETIME2 NOT NULL ,
	SoLuongNguoi INT,
	TongTien DECIMAL(18,2),
	TrangThai NVARCHAR(50),
	GhiChu NVARCHAR(2000),
	CONSTRAINT CHK_NgayTraPhong CHECK (NgayTraPhong > NgayNhanPhong),
	isDelete BIT DEFAULT 0, -- nhớ phải check trạng thái đặt phòng
	Cancelled_at DATETIME2 NULL,
	Delete_at DATETIME2 NULL
);

CREATE TABLE tbl_ChiTietDatPhong(
	ChiTietDatPhongID INT PRIMARY KEY IDENTITY(1,1),
	DatPhongID INT NOT NULL REFERENCES tbl_DatPhong(DatPhongID),
	PhongID INT NOT NULL REFERENCES tbl_Phong(PhongID),
	GiaTaiThoiDiemDat DECIMAL(18,2)
);

CREATE TABLE tbl_GiaoDich(
	GiaoDichID INT PRIMARY KEY IDENTITY(1,1),
	DatPhongID INT NOT NULL REFERENCES tbl_DatPhong(DatPhongID),
	SoTien DECIMAL(18,2),
	TrangThai NVARCHAR(50),
	PhuongThuc NVARCHAR(50),
	Create_at DATETIME2 DEFAULT SYSUTCDATETIME(),
	Update_at DATETIME2 NULL,
	Delete_at DATETIME2 NULL
);
go


INSERT INTO tbl_VaiTro(VaiTro) values ('admin')
go
INSERT INTO tbl_VaiTro(VaiTro) values ('customer')
go
INSERT INTO tbl_TaiKhoan(HoTen,Email,MatKhau,SoDienThoai,DiaChi,VaiTro) values
('Admin','admin@gmail.com','1','0888888888','hn','admin'),
('Nguyen Van A', 'user1@gmail.com', '1', '0911122233', N'Hà Nội', 'customer'),
('Tran Thi B', 'thib@gmail.com', '123456', '0933445566', N'Hồ Chí Minh', 'customer'),
('Le Van C', 'vanc@gmail.com', '123456', '0988877665', N'Đà Nẵng', 'customer'),
('Pham Thi D', 'thid@gmail.com', '123456', '0909090909', N'Hải Phòng', 'customer');
go

INSERT INTO tbl_LoaiPhong(TenLoaiPhong, MoTa) VALUES
(N'Phòng đơn', N'Phòng 1 giường đơn, phù hợp cho 1 người.'),
(N'Phòng đôi', N'Phòng 1 giường đôi hoặc 2 giường đơn cho 2 người.'),
(N'Phòng gia đình', N'Phòng rộng cho gia đình 3-4 người.'),
(N'Phòng VIP', N'Phòng cao cấp, đầy đủ tiện nghi, view đẹp.');
go

INSERT INTO tbl_Phong(SoPhong, LoaiPhongID, GiaMoiDem, SucChuaToiDa, MoTa, TrangThai, HinhAnh) VALUES
('101', 1, 500000, 1, N'Phòng đơn tiêu chuẩn', 'Available', NULL),
('102', 1, 520000, 1, N'Phòng đơn có cửa sổ', 'Available', NULL),
('201', 2, 750000, 2, N'Phòng đôi tiêu chuẩn', 'Available', NULL),
('202', 2, 780000, 2, N'Phòng đôi hướng phố', 'Available', NULL),
('301', 3, 1100000, 4, N'Phòng gia đình 4 người', 'Available', NULL),
('401', 4, 2000000, 2, N'Phòng VIP view biển', 'Available', NULL);
go
INSERT INTO tbl_PhongImages(PhongID, Url) VALUES
(1, 'room101_1.jpg'),
(1, 'room101_2.jpg'),
(3, 'room201_1.jpg'),
(5, 'room301_1.jpg'),
(6, 'room401_1.jpg'),
(6, 'room401_2.jpg');
go
INSERT INTO tbl_DatPhong(TaiKhoanID, NgayNhanPhong, NgayTraPhong, SoLuongNguoi, TongTien, TrangThai, GhiChu) VALUES
(2, '2025-12-12 14:00:00', '2025-12-13 12:00:00', 1, 1000000, N'Pending', N'Customer requested low floor');
(3, '09/12/2025', '12/12/2025', 2, 1500000, N'Checkin', NULL),
(4, '20/12/2025', '22/12/2025', 3, 2200000, N'Pending', NULL),
(5, '05/12/2025', '06/12/2025', 2, 2000000, N'Checkout', N'Paid online'),
(2, '12/12/2025', '13/12/2025', 1, 600000, 'Pending', NULL),
(3, '14/12/2025', '16/12/2025', 2, 1200000, 'Pending', NULL),
(4, '15/12/2025', '17/12/2025', 3, 1800000, 'Pending', NULL),
(5, '18/12/2025', '19/12/2025', 2, 700000, 'Pending', NULL),
(2, '20/12/2025', '22/12/2025', 1, 1300000, 'Pending', NULL),

(3, '22/12/2025', '23/12/2025', 2, 800000, 'Pending', NULL),
(4, '24/12/2025', '26/12/2025', 3, 1600000, 'Pending', NULL),
(5, '25/12/2025', '26/12/2025', 1, 550000, 'Pending', NULL),
(2, '27/12/2025', '29/12/2025', 2, 1400000, 'Pending', NULL),
(3, '28/12/2025', '30/12/2025', 1, 650000, 'Pending', NULL),

(4, '02/01/2026', '04/01/2026', 2, 1500000, 'Pending', NULL),
(5, '03/01/2026', '05/01/2026', 1, 600000, 'Pending', NULL),
(2, '06/01/2026', '08/01/2026', 3, 2000000, 'Pending', NULL),
(3, '08/01/2026', '09/01/2026', 2, 750000, 'Pending', NULL),
(4, '10/01/2026', '11/01/2026', 2, 720000, 'Pending', NULL),

(5, '12/01/2026', '14/01/2026', 3, 1700000, 'Pending', NULL),
(2, '14/01/2026', '15/01/2026', 1, 600000, 'Pending', NULL),
(3, '16/01/2026', '18/01/2026', 2, 1300000, 'Pending', NULL),
(4, '18/01/2026', '19/01/2026', 1, 580000, 'Pending', NULL),
(5, '19/01/2026', '20/01/2026', 2, 950000, 'Pending', NULL);


INSERT INTO tbl_ChiTietDatPhong(DatPhongID, PhongID, GiaTaiThoiDiemDat) VALUES
(1, 1, 500000),
(2, 3, 750000),
(3, 5, 1100000),
(4, 6, 2000000);

INSERT INTO tbl_GiaoDich(DatPhongID, SoTien, TrangThai, PhuongThuc) VALUES
(1, 500000, N'Unpaid', N'Tiền mặt'),
(2, 750000, N'Unpaid', N'Chuyển khoản'),
(3, 1100000, N'Unpaid', N'MoMo'),
(4, 2000000, N'Paid', N'VNPAY');
go



CREATE OR ALTER VIEW vw_DanhSachDatPhong AS
SELECT DP.DatPhongID,
	DP.NgayDat,
	DP.NgayNhanPhong,
	DP.NgayTraPhong,
	DP.SoLuongNguoi,
	DP.TongTien,
	DP.TrangThai,
	dp.GhiChu,
	P.SoPhong,
	P.PhongID,
	DP.TaiKhoanID,
	TK.MaTK,
	Tk.HoTen,
	DP.isDelete,
	CTDP.GiaTaiThoiDiemDat
FROM tbl_DatPhong DP
JOIN tbl_ChiTietDatPhong CTDP ON DP.DatPhongID=CTDP.DatPhongID
JOIN tbl_Phong P on P.PhongID= CTDP.PhongID
JOIN tbl_TaiKhoan TK on DP.TaiKhoanID =TK.TaiKhoanID
GO


CREATE OR ALTER VIEW vw_DanhSachPhong AS
SELECT P.PhongID,
		P.SoPhong,
		P.GiaMoiDem,
		P.MoTa,
		P.HinhAnh,
		p.SucChuaToiDa,
		P.Create_at,
		P.isDelete,
		P.Delete_at,
		P.Update_at,
		LP.TenLoaiPhong

FROM tbl_Phong P
JOIN tbl_LoaiPhong LP ON P.LoaiPhongID= LP.LoaiPhongID
GO
CREATE or alter VIEW vw_ThongKeDoanhThu AS
SELECT 
    YEAR(gd.Create_at) AS Nam,
    MONTH(gd.Create_at) AS Thang,
    SUM(gd.SoTien) AS TongDoanhThu
FROM tbl_GiaoDich gd
GROUP BY YEAR(gd.Create_at), MONTH(gd.Create_at);
go


CREATE OR ALTER PROCEDURE sp_TimPhongTrong
    @NgayNhanPhong DATETIME2,
    @NgayTraPhong DATETIME2,
    @SucChuaToiDa INT = NULL      -- Tùy chọn: sức chứa
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.PhongID,
        p.SoPhong,
        lp.TenLoaiPhong,
        p.GiaMoiDem,
        p.SucChuaToiDa,
        p.TrangThai,
        p.MoTa,
        p.HinhAnh
    FROM tbl_Phong p
    INNER JOIN tbl_LoaiPhong lp ON p.LoaiPhongID = lp.LoaiPhongID
    WHERE 
        p.isDelete = 0
        AND lp.isDelete = 0
        -- Chỉ loại bỏ các phòng không thể phục vụ (ví dụ: đang bảo trì) 
        
        -- Lọc tùy chọn 
        AND (@SucChuaToiDa IS NULL OR p.SucChuaToiDa >= @SucChuaToiDa)

        -- Kiểm tra xem có tồn tại bất kỳ đơn đặt phòng nào trùng lặp không
        AND NOT EXISTS (
            SELECT 1
            FROM tbl_ChiTietDatPhong ctdp
            JOIN tbl_DatPhong dp ON ctdp.DatPhongID = dp.DatPhongID
            WHERE 
                ctdp.PhongID = p.PhongID -- Chỉ kiểm tra cho phòng hiện tại
                AND dp.TrangThai IN (N'Pending', N'Checkin') -- Các trạng thái đặt phòng hợp lệ
                AND (
                    -- Logic kiểm tra trùng lặp thời gian
                    @NgayNhanPhong < dp.NgayTraPhong AND
                    @NgayTraPhong > dp.NgayNhanPhong
                )
        )
    ORDER BY p.GiaMoiDem ASC;
END;
GO


	


CREATE or alter PROCEDURE sp_BaoCaoDoanhThuTheoThang
    @Nam INT  -- Tham số đầu vào là năm cần xem báo cáo
AS
BEGIN
    SET NOCOUNT ON;

    -- Sử dụng Common Table Expression (CTE) để tạo ra 12 tháng
    -- Điều này đảm bảo tất cả các tháng đều xuất hiện, kể cả khi không có doanh thu
    ;WITH TatCaCacThang AS (
        SELECT 1 AS Thang
        UNION ALL SELECT 2
        UNION ALL SELECT 3
        UNION ALL SELECT 4
        UNION ALL SELECT 5
        UNION ALL SELECT 6
        UNION ALL SELECT 7
        UNION ALL SELECT 8
        UNION ALL SELECT 9
        UNION ALL SELECT 10
        UNION ALL SELECT 11
        UNION ALL SELECT 12
    ),
    -- CTE thứ hai để tính toán doanh thu thực tế theo tháng
    DoanhThuThucTe AS (
        SELECT 
            MONTH(Create_at) AS Thang,
            SUM(SoTien) AS TongDoanhThu
        FROM 
            tbl_GiaoDich
        WHERE 
            TrangThai = N'ThanhCong' -- Chỉ tính giao dịch thành công
            AND YEAR(Create_at) = @Nam -- Lọc theo năm
        GROUP BY 
            MONTH(Create_at)
    )
    -- Tham gia 2 bảng CTE để có kết quả cuối cùng
    SELECT 
        m.Thang,
        ISNULL(dt.TongDoanhThu, 0) AS TongDoanhThu
    FROM 
        TatCaCacThang m
    LEFT JOIN 
        DoanhThuThucTe dt ON m.Thang = dt.Thang
    ORDER BY 
        m.Thang;
END;
GO


