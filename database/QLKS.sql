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
	TrangThai NVARCHAR(50) DEFAULT 'Trong',
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
INSERT INTO tbl_TaiKhoan(HoTen,Email,MatKhau,SoDienThoai,DiaChi,VaiTro) 
values ('Admin','admin@gmail.com','1','0888888888','hn','admin');

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


