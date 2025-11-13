IF NOT EXISTS (SELECT 1 FROM sys.Databases WHERE name = 'QLKS')
    EXEC('CREATE DATABASE QLKS');
GO
USE QLKS
go

CREATE TABLE tbl_VaiTro(
	VaiTroID INT PRIMARY KEY IDENTITY(1,1),
	TenVaiTro NVARCHAR(50),
	Create_at DATETIME2 DEFAULT SYSUTCDATETIME(),
	isDelete BIT DEFAULT 0,
	Delete_at DATETIME2 NULL
);


CREATE TABLE tbl_TaiKhoan(
	TaiKhoanID INT PRIMARY KEY IDENTITY(1,1),
	MaTK AS ('TK' + RIGHT('000000' + CAST(TaiKhoanID AS VARCHAR(10)), 3)) PERSISTED,
	HoTen NVARCHAR(100) NOT NULL,
	Email VARCHAR(100) NOT NULL ,
	MatKhau VARCHAR(255),
	SoDienThoai VARCHAR(10) NOT NULL ,
	DiaChi NVARCHAR(255),
	Create_at DATETIME2 DEFAULT SYSUTCDATETIME(),
	VaiTroID INT NOT NULL REFERENCES tbl_VaiTro(VaiTroID),
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
	CONSTRAINT CHK_NgayTraPhong CHECK (NgayTraPhong > NgayNhanPhong)
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
	NgayThanhToan DATETIME2,
	SoTien DECIMAL(18,2),
	TrangThai NVARCHAR(50)
);
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
	TK.MaTK

FROM tbl_DatPhong DP
JOIN tbl_ChiTietDatPhong CTDP ON DP.DatPhongID=CTDP.DatPhongID
JOIN tbl_Phong P on P.PhongID= CTDP.PhongID
JOIN tbl_TaiKhoan TK on DP.TaiKhoanID =TK.TaiKhoanID 
GO

CREATE OR ALTER VIEW vw_DanhSachTaiKhoan AS
SELECT tk.TaiKhoanID,
		tk.HoTen,
		tk.DiaChi,
		tk.Email,
		vt.TenVaiTro,
		vt.VaiTroID,
		tk.SoDienThoai,
		tk.Create_at,
		tk.isDelete,
		tk.Delete_at,
		tk.Update_at
FROM tbl_TaiKhoan tk
JOIN tbl_VaiTro vt ON tk.VaiTroID= vt.VaiTroID
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
    YEAR(gd.NgayThanhToan) AS Nam,
    MONTH(gd.NgayThanhToan) AS Thang,
    SUM(gd.SoTien) AS TongDoanhThu
FROM tbl_GiaoDich gd
GROUP BY YEAR(gd.NgayThanhToan), MONTH(gd.NgayThanhToan);
go

CREATE or alter PROCEDURE sp_CapNhatTrangThaiPhong
    @PhongID INT,
    @TrangThai NVARCHAR(50)
AS
BEGIN
    UPDATE tbl_Phong
    SET TrangThai = @TrangThai
    WHERE PhongID = @PhongID;
END;
go

/*CREATE PROCEDURE sp_ThanhToan
    @DatPhongID INT,
    @PhuongThuc NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @SoTienThanhToan DECIMAL(18,2);
    DECLARE @TrangThaiHienTai NVARCHAR(50);

    -- Lấy thông tin đơn đặt phòng
    SELECT 
        @SoTienThanhToan = TongTien,
        @TrangThaiHienTai = TrangThai
    FROM tbl_DatPhong
    WHERE DatPhongID = @DatPhongID;

    -- Kiểm tra xem đơn có tồn tại không
    IF @SoTienThanhToan IS NULL
    BEGIN
        RAISERROR(N'Không tìm thấy đơn đặt phòng.', 16, 1);
        RETURN;
    END;

    -- Kiểm tra xem đơn đã thanh toán chưa
    IF @TrangThaiHienTai = N'Đã thanh toán'
    BEGIN
        RAISERROR(N'Đơn này đã được thanh toán trước đó.', 16, 1);
        RETURN;
    END;

    BEGIN TRANSACTION;
    BEGIN TRY
        -- 1. Thêm vào bảng giao dịch với số tiền lấy từ đơn đặt
        INSERT INTO tbl_GiaoDich (DatPhongID, NgayThanhToan, SoTien, TrangThai)
        VALUES (@DatPhongID, SYSUTCDATETIME(), @SoTienThanhToan, N'ThanhCong');

        -- 2. Cập nhật trạng thái đơn đặt phòng
        UPDATE tbl_DatPhong
        SET TrangThai = N'Đã thanh toán'
        WHERE DatPhongID = @DatPhongID;
        
        UPDATE p
        SET p.TrangThai = N'Trống'
        FROM tbl_Phong p
        JOIN tbl_ChiTietDatPhong ctdp ON p.PhongID = ctdp.PhongID
        WHERE ctdp.DatPhongID = @DatPhongID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW; 
    END CATCH
END;
GO
*/


CREATE FUNCTION fn_TinhTongTien(@DatPhongID INT)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @TongTien DECIMAL(18,2);
    SELECT @TongTien = SUM(GiaTaiThoiDiemDat)
    FROM tbl_ChiTietDatPhong
    WHERE DatPhongID = @DatPhongID;
    RETURN ISNULL(@TongTien, 0);
END;
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
        AND p.TrangThai != N'Bảo trì' 
        
        -- Lọc tùy chọn (logic này đã đúng)
        AND (@SucChuaToiDa IS NULL OR p.SucChuaToiDa >= @SucChuaToiDa)

        -- Dùng NOT EXISTS thay cho NOT IN để có hiệu năng tốt hơn
        -- Kiểm tra xem có tồn tại bất kỳ đơn đặt phòng nào trùng lặp không
        AND NOT EXISTS (
            SELECT 1
            FROM tbl_ChiTietDatPhong ctdp
            JOIN tbl_DatPhong dp ON ctdp.DatPhongID = dp.DatPhongID
            WHERE 
                ctdp.PhongID = p.PhongID -- Chỉ kiểm tra cho phòng hiện tại
                AND dp.TrangThai IN (N'Đã đặt', N'Đang ở') -- Các trạng thái đặt phòng hợp lệ
                AND (
                    -- Logic kiểm tra trùng lặp thời gian (đã đúng)
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
            MONTH(NgayThanhToan) AS Thang,
            SUM(SoTien) AS TongDoanhThu
        FROM 
            tbl_GiaoDich
        WHERE 
            TrangThai = N'ThanhCong' -- Chỉ tính giao dịch thành công
            AND YEAR(NgayThanhToan) = @Nam -- Lọc theo năm
        GROUP BY 
            MONTH(NgayThanhToan)
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


