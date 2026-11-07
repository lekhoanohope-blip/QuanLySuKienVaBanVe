-- ============================================================================
-- ĐỀ TÀI 20: QUẢN LÝ SỰ KIỆN VÀ BÁN VÉ
-- File: 01_Schema.sql
-- Mục đích: Khởi tạo CSDL, cấu trúc 10 bảng, ràng buộc toàn vẹn và chỉ mục.
-- Lưu ý: Script có tính idempotent (an toàn khi chạy lại nhiều lần).
-- ============================================================================

USE master;
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'QuanLySuKienVaBanVe')
BEGIN
    CREATE DATABASE QuanLySuKienVaBanVe;
END
GO

USE QuanLySuKienVaBanVe;
GO

-- Bắt buộc bật QUOTED_IDENTIFIER & ANSI_NULLS cho cột tính toán PERSISTED và Filtered Index
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- ============================================================================
-- 1. BẢNG TÀI KHOẢN (Phân quyền Quản lý / Nhân viên)
-- ============================================================================
IF OBJECT_ID('dbo.TaiKhoan', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.TaiKhoan (
        MaTK        INT IDENTITY(1,1) PRIMARY KEY,
        TenDangNhap NVARCHAR(50)  NOT NULL CONSTRAINT UQ_TaiKhoan_TenDangNhap UNIQUE,
        MatKhau     NVARCHAR(255) NOT NULL, -- Băm SHA-256 (64 ký tự hex)
        VaiTro      NVARCHAR(30)  NOT NULL CONSTRAINT CK_TaiKhoan_VaiTro CHECK (VaiTro IN (N'QuanLy', N'NhanVien')),
        TrangThai   BIT           NOT NULL CONSTRAINT DF_TaiKhoan_TrangThai DEFAULT 1
    );
END
GO

-- ============================================================================
-- 2. BẢNG SỰ KIỆN (Danh mục chương trình, concert, sân khấu, hội thảo)
-- ============================================================================
IF OBJECT_ID('dbo.SuKien', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.SuKien (
        MaSK      INT IDENTITY(1,1) PRIMARY KEY,
        TenSK     NVARCHAR(200) NOT NULL,
        LoaiSK    NVARCHAR(100) NULL,
        MoTa      NVARCHAR(MAX) NULL,
        TrangThai NVARCHAR(30)  NOT NULL CONSTRAINT DF_SuKien_TrangThai DEFAULT N'SapDien'
            CONSTRAINT CK_SuKien_TrangThai CHECK (TrangThai IN (N'SapDien', N'DangDien', N'DaKetThuc', N'DaHuy'))
    );
END
GO

-- ============================================================================
-- 3. BẢNG SUẤT DIỄN (Lịch biểu diễn và cấu hình loại sơ đồ sân khấu)
--    LoaiSoDo: 1 = Sơ đồ Chữ T (Catwalk), 2 = Hình chữ nhật (Arena), 3 = Lưới (Nhà hát)
-- ============================================================================
IF OBJECT_ID('dbo.SuatDien', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.SuatDien (
        MaSuat          INT IDENTITY(1,1) PRIMARY KEY,
        MaSK            INT           NOT NULL CONSTRAINT FK_SuatDien_SuKien FOREIGN KEY REFERENCES dbo.SuKien(MaSK),
        ThoiGianBatDau  DATETIME      NOT NULL,
        ThoiGianKetThuc DATETIME      NULL,
        DiaDiem         NVARCHAR(255) NOT NULL,
        LoaiSoDo        INT           NOT NULL CONSTRAINT DF_SuatDien_LoaiSoDo DEFAULT 1
            CONSTRAINT CK_SuatDien_LoaiSoDo CHECK (LoaiSoDo IN (1, 2, 3)),
        TrangThai       NVARCHAR(30)  NOT NULL CONSTRAINT DF_SuatDien_TrangThai DEFAULT N'MoCuaBanVe'
            CONSTRAINT CK_SuatDien_TrangThai CHECK (TrangThai IN (N'MoCuaBanVe', N'DongBanVe', N'DangDien', N'DaKetThuc')),
        CONSTRAINT CK_SuatDien_ThoiGian CHECK (ThoiGianKetThuc IS NULL OR ThoiGianKetThuc > ThoiGianBatDau)
    );
END
GO

-- ============================================================================
-- 4. BẢNG KHU VỰC (Ánh xạ vùng trên sơ đồ ghế, hạng vé, giá vé và màu hiển thị)
--    HangVe chuẩn trên giao diện: N'VIP', N'Phổ thông', N'Thường'
-- ============================================================================
IF OBJECT_ID('dbo.KhuVuc', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.KhuVuc (
        MaKV         INT IDENTITY(1,1) PRIMARY KEY,
        MaSuat       INT            NOT NULL CONSTRAINT FK_KhuVuc_SuatDien FOREIGN KEY REFERENCES dbo.SuatDien(MaSuat),
        MaVung       VARCHAR(20)    NULL,
        TenKV        NVARCHAR(100)  NOT NULL,
        HangVe       NVARCHAR(50)   NOT NULL,
        GiaVe        DECIMAL(18,0)  NOT NULL CONSTRAINT CK_KhuVuc_GiaVe CHECK (GiaVe >= 0),
        TongSoGhe    INT            NOT NULL CONSTRAINT DF_KhuVuc_TongSoGhe DEFAULT 20
            CONSTRAINT CK_KhuVuc_TongSoGhe CHECK (TongSoGhe > 0 AND TongSoGhe <= 10000),
        ChoPhepBanVe BIT            NOT NULL CONSTRAINT DF_KhuVuc_ChoPhepBanVe DEFAULT 1,
        MauSac       VARCHAR(20)    NULL
            CONSTRAINT CK_KhuVuc_MauSac CHECK (MauSac IS NULL OR MauSac LIKE '#[0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f]')
    );
END
GO

-- ============================================================================
-- 5. BẢNG GHẾ (Ma trận ghế chi tiết và cơ chế khóa phiên giữ ghế tạm thời)
--    MaPhienGiu: Token định danh phiên giữ ghế tại quầy (tự giải phóng sau 10 phút)
-- ============================================================================
IF OBJECT_ID('dbo.Ghe', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Ghe (
        MaGhe        INT IDENTITY(1,1) PRIMARY KEY,
        MaKV         INT              NOT NULL CONSTRAINT FK_Ghe_KhuVuc FOREIGN KEY REFERENCES dbo.KhuVuc(MaKV),
        Hang         VARCHAR(10)      NOT NULL,
        Cot          INT              NOT NULL,
        MaGheHienThi AS (Hang + CAST(Cot AS VARCHAR(10))) PERSISTED,
        TrangThai    NVARCHAR(30)     NOT NULL CONSTRAINT DF_Ghe_TrangThai DEFAULT N'Trong'
            CONSTRAINT CK_Ghe_TrangThai CHECK (TrangThai IN (N'Trong', N'DangGiu', N'DaBan')),
        ThoiGianGiu  DATETIME         NULL,
        MaPhienGiu   UNIQUEIDENTIFIER NULL,
        CONSTRAINT UQ_Ghe_ViTri UNIQUE (MaKV, Hang, Cot)
    );
END
GO

-- ============================================================================
-- 6. BẢNG KHÁCH HÀNG (Thông tin người mua vé, định danh duy nhất theo SĐT)
-- ============================================================================
IF OBJECT_ID('dbo.KhachHang', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.KhachHang (
        MaKH  INT IDENTITY(1,1) PRIMARY KEY,
        TenKH NVARCHAR(100) NOT NULL,
        SDT   VARCHAR(20)   NOT NULL CONSTRAINT UQ_KhachHang_SDT UNIQUE,
        Email NVARCHAR(100) NULL
    );
END
GO

-- ============================================================================
-- 7. BẢNG ĐƠN VÉ (Hóa đơn bán vé; MaDonGoc lưu liên kết truy vết khi đổi vé)
-- ============================================================================
IF OBJECT_ID('dbo.DonVe', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.DonVe (
        MaDon     INT IDENTITY(1,1) PRIMARY KEY,
        MaKH      INT           NOT NULL CONSTRAINT FK_DonVe_KhachHang FOREIGN KEY REFERENCES dbo.KhachHang(MaKH),
        MaSuat    INT           NOT NULL CONSTRAINT FK_DonVe_SuatDien FOREIGN KEY REFERENCES dbo.SuatDien(MaSuat),
        MaTK      INT           NOT NULL CONSTRAINT FK_DonVe_TaiKhoan FOREIGN KEY REFERENCES dbo.TaiKhoan(MaTK),
        MaDonGoc  INT           NULL     CONSTRAINT FK_DonVe_DonVeGoc FOREIGN KEY REFERENCES dbo.DonVe(MaDon),
        NgayDat   DATETIME      NOT NULL CONSTRAINT DF_DonVe_NgayDat DEFAULT GETDATE(),
        TongTien  DECIMAL(18,0) NOT NULL CONSTRAINT DF_DonVe_TongTien DEFAULT 0,
        TrangThai NVARCHAR(30)  NOT NULL CONSTRAINT DF_DonVe_TrangThai DEFAULT N'DaThanhToan'
            CONSTRAINT CK_DonVe_TrangThai CHECK (TrangThai IN (N'DaThanhToan', N'DaHuy', N'DaDoi'))
    );
END
GO

-- ============================================================================
-- 8. BẢNG CHI TIẾT VÉ (Từng vé gắn với ghế và mã vạch MaVeCode để xuất vé PDF)
-- ============================================================================
IF OBJECT_ID('dbo.ChiTietVe', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ChiTietVe (
        MaCT      INT IDENTITY(1,1) PRIMARY KEY,
        MaDon     INT           NOT NULL CONSTRAINT FK_ChiTietVe_DonVe FOREIGN KEY REFERENCES dbo.DonVe(MaDon),
        MaGhe     INT           NOT NULL CONSTRAINT FK_ChiTietVe_Ghe FOREIGN KEY REFERENCES dbo.Ghe(MaGhe),
        GiaVe     DECIMAL(18,0) NOT NULL,
        MaVeCode  VARCHAR(50)   NOT NULL CONSTRAINT UQ_ChiTietVe_MaVeCode UNIQUE,
        TrangThai NVARCHAR(20)  NOT NULL CONSTRAINT DF_ChiTietVe_TrangThai DEFAULT N'HopLe'
            CONSTRAINT CK_ChiTietVe_TrangThai CHECK (TrangThai IN (N'HopLe', N'DaHuy')),
        CONSTRAINT UQ_ChiTietVe_DonGhe UNIQUE (MaDon, MaGhe)
    );
END
GO

-- ============================================================================
-- 9. BẢNG THANH TOÁN (Lịch sử thu/hoàn tiền của đơn vé)
-- ============================================================================
IF OBJECT_ID('dbo.ThanhToan', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ThanhToan (
        MaTT          INT IDENTITY(1,1) PRIMARY KEY,
        MaDon         INT           NOT NULL CONSTRAINT FK_ThanhToan_DonVe FOREIGN KEY REFERENCES dbo.DonVe(MaDon),
        NgayThanhToan DATETIME      NOT NULL CONSTRAINT DF_ThanhToan_NgayThanhToan DEFAULT GETDATE(),
        SoTien        DECIMAL(18,0) NOT NULL,
        PhuongThuc    NVARCHAR(30)  NOT NULL
            CONSTRAINT CK_ThanhToan_PhuongThuc CHECK (PhuongThuc IN (N'TienMat', N'ChuyenKhoan', N'TheNganHang'))
    );
END
GO

-- ============================================================================
-- 10. BẢNG NHẬT KÝ (Audit log ghi nhận thao tác quản trị & giao dịch)
-- ============================================================================
IF OBJECT_ID('dbo.NhatKy', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.NhatKy (
        MaNK     INT IDENTITY(1,1) PRIMARY KEY,
        MaTK     INT           NOT NULL CONSTRAINT FK_NhatKy_TaiKhoan FOREIGN KEY REFERENCES dbo.TaiKhoan(MaTK),
        HanhDong NVARCHAR(100) NOT NULL,
        ThoiGian DATETIME      NOT NULL CONSTRAINT DF_NhatKy_ThoiGian DEFAULT GETDATE(),
        ChiTiet  NVARCHAR(MAX) NULL
    );
END
GO

-- ============================================================================
-- ĐỒNG BỘ CỘT & CHỈ MỤC CHO CSDL ĐÃ TỒN TẠI TỪ PHIÊN BẢN CŨ
-- ============================================================================
IF COL_LENGTH('dbo.SuatDien', 'LoaiSoDo') IS NULL
    ALTER TABLE dbo.SuatDien ADD LoaiSoDo INT NOT NULL CONSTRAINT DF_SuatDien_LoaiSoDo DEFAULT 1;
GO

IF COL_LENGTH('dbo.KhuVuc', 'ChoPhepBanVe') IS NULL
    ALTER TABLE dbo.KhuVuc ADD ChoPhepBanVe BIT NOT NULL CONSTRAINT DF_KhuVuc_ChoPhepBanVe DEFAULT 1;
GO

IF COL_LENGTH('dbo.KhuVuc', 'MaVung') IS NULL
    ALTER TABLE dbo.KhuVuc ADD MaVung VARCHAR(20) NULL;
GO

IF COL_LENGTH('dbo.KhuVuc', 'MauSac') IS NULL
    ALTER TABLE dbo.KhuVuc ADD MauSac VARCHAR(20) NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_KhuVuc_TongSoGhe')
BEGIN
    UPDATE dbo.KhuVuc SET TongSoGhe = 1 WHERE TongSoGhe <= 0;
    ALTER TABLE dbo.KhuVuc WITH CHECK
        ADD CONSTRAINT CK_KhuVuc_TongSoGhe CHECK (TongSoGhe > 0 AND TongSoGhe <= 10000);
END
GO

IF COL_LENGTH('dbo.Ghe', 'ThoiGianGiu') IS NULL
    ALTER TABLE dbo.Ghe ADD ThoiGianGiu DATETIME NULL;
GO

IF COL_LENGTH('dbo.Ghe', 'MaPhienGiu') IS NULL
    ALTER TABLE dbo.Ghe ADD MaPhienGiu UNIQUEIDENTIFIER NULL;
GO

IF COL_LENGTH('dbo.DonVe', 'MaDonGoc') IS NULL
    ALTER TABLE dbo.DonVe ADD MaDonGoc INT NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_DonVe_DonVeGoc')
    ALTER TABLE dbo.DonVe ADD CONSTRAINT FK_DonVe_DonVeGoc FOREIGN KEY (MaDonGoc) REFERENCES dbo.DonVe(MaDon);
GO

-- Chỉ mục lọc tăng tốc tra cứu ghế đang giữ theo phiên giao dịch
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Ghe_MaPhienGiu' AND object_id = OBJECT_ID('dbo.Ghe'))
    CREATE INDEX IX_Ghe_MaPhienGiu ON dbo.Ghe(MaPhienGiu) WHERE MaPhienGiu IS NOT NULL;
GO

-- Chỉ mục duy nhất có điều kiện: Chặn bán trùng 1 ghế trên 2 vé hợp lệ cùng lúc,
-- nhưng vẫn cho phép bán lại ghế đó sau khi vé cũ đã chuyển sang 'DaHuy'.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_ChiTietVe_Ghe_HopLe' AND object_id = OBJECT_ID('dbo.ChiTietVe'))
    CREATE UNIQUE INDEX UQ_ChiTietVe_Ghe_HopLe ON dbo.ChiTietVe(MaGhe) WHERE TrangThai = N'HopLe';
GO
