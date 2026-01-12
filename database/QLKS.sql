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
	PhongID INT NOT NULL REFERENCES tbl_Phong(PhongID),
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


INSERT INTO tbl_VaiTro(VaiTro) values ('admin'),('customer')	
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
INSERT INTO tbl_DatPhong(TaiKhoanID, PhongID, NgayNhanPhong, NgayTraPhong, SoLuongNguoi, TongTien, TrangThai, GhiChu) VALUES
(2, 1, '2025-12-12', '2025-12-14', 1, 1000000, N'Pending', N'Customer requested low floor'),
(3, 2, '2025-12-09', '2025-12-12', 2, 1500000, N'Checkin', NULL),
(4, 3, '2025-12-20', '2025-12-22', 3, 2200000, N'Pending', NULL),
(5, 4, '2025-12-05', '2025-12-06', 2, 2000000, N'Checkout', N'Paid online'),

(2, 1, '2025-12-12', '2025-12-13', 1, 600000, 'Pending', NULL),
(3, 2, '2025-12-14', '2025-12-16', 2, 1200000, 'Pending', NULL),
(4, 3, '2025-12-15', '2025-12-17', 3, 1800000, 'Pending', NULL),
(5, 4, '2025-12-18', '2025-12-19', 2, 700000, 'Pending', NULL),
(2, 1, '2025-12-20', '2025-12-22', 1, 1300000, 'Pending', NULL),

(3, 2, '2025-12-22', '2025-12-23', 2, 800000, 'Pending', NULL),
(4, 3, '2025-12-24', '2025-12-26', 3, 1600000, 'Pending', NULL),
(5, 4, '2025-12-25', '2025-12-26', 1, 550000, 'Pending', NULL),
(2, 1, '2025-12-27', '2025-12-29', 2, 1400000, 'Pending', NULL),
(3, 2, '2025-12-28', '2025-12-30', 1, 650000, 'Pending', NULL),

(4, 3, '2026-01-02', '2026-01-04', 2, 1500000, 'Pending', NULL),
(5, 4, '2026-01-03', '2026-01-05', 1, 600000, 'Pending', NULL),
(2, 1, '2026-01-06', '2026-01-08', 3, 2000000, 'Pending', NULL),
(3, 2, '2026-01-08', '2026-01-09', 2, 750000, 'Pending', NULL),
(4, 3, '2026-01-10', '2026-01-11', 2, 720000, 'Pending', NULL),

(5, 4, '2026-01-12', '2026-01-14', 3, 1700000, 'Pending', NULL),
(2, 1, '2026-01-14', '2026-01-15', 1, 600000, 'Pending', NULL),
(3, 2, '2026-01-16', '2026-01-18', 2, 1300000, 'Pending', NULL),
(4, 3, '2026-01-18', '2026-01-19', 1, 580000, 'Pending', NULL),
(5, 4, '2026-01-19', '2026-01-20', 2, 950000, 'Pending', NULL);
go
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
	DP.isDelete
FROM tbl_DatPhong DP
JOIN tbl_Phong P on P.PhongID= DP.PhongID
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



CREATE OR ALTER PROCEDURE sp_TimPhong
    @NgayNhanPhong DATETIME2,
    @NgayTraPhong DATETIME2,
    @SucChuaToiDa INT = NULL      -- Tùy chọn: sức chứa
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.PhongID,
        p.SoPhong,
		p.LoaiPhongID,
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
            FROM tbl_DatPhong dp
            WHERE 
                dp.PhongID = p.PhongID -- Chỉ kiểm tra cho phòng hiện tại
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


	



CREATE OR ALTER PROCEDURE sp_BaoCaoDoanhThuDatPhong
    @Thang INT = NULL,
    @Nam INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        MONTH(dp.NgayTraPhong) AS Thang,
        YEAR(dp.NgayTraPhong) AS Nam,
        COUNT(DISTINCT dp.DatPhongID) AS SoLuotDat,
        -- Nếu giao dịch null thì tính là 0
        ISNULL(SUM(gd.SoTien), 0) AS DoanhThu	
    FROM tbl_GiaoDich gd
    JOIN tbl_DatPhong dp ON gd.DatPhongID = dp.DatPhongID
    WHERE
        -- Điều kiện lọc theo trạng thái
        (gd.TrangThai = 'Paid' OR gd.TrangThai = 'Success')
        AND (dp.isDelete = 0 OR dp.isDelete IS NULL)
        
        -- Điều kiện lọc thời gian
        AND (@Nam IS NULL OR YEAR(dp.NgayTraPhong) = @Nam)
        AND (@Thang IS NULL OR MONTH(dp.NgayTraPhong) = @Thang)
    GROUP BY
        YEAR(dp.NgayTraPhong),
        MONTH(dp.NgayTraPhong)
    ORDER BY
        Nam, Thang;
END
GO





