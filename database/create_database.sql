-- ==============================================================
-- DE TAI 20: QUAN LY SU KIEN VA BAN VE
-- Script 01: Tao Database, Cac bang va Rang buoc toan ven
-- ==============================================================

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'QuanLySuKienVaBanVe')
BEGIN
    CREATE DATABASE QuanLySuKienVaBanVe;
END
GO

USE QuanLySuKienVaBanVe;
GO

-- 1. Bang TaiKhoan (Nguoi dung he thong: Admin / Nhan vien)
IF OBJECT_ID('TaiKhoan', 'U') IS NULL
BEGIN
    CREATE TABLE TaiKhoan (
        MaTK INT IDENTITY(1,1) PRIMARY KEY,
        TenDangNhap NVARCHAR(50) NOT NULL CONSTRAINT UQ_TaiKhoan_TenDangNhap UNIQUE,
        MatKhau NVARCHAR(255) NOT NULL,
        VaiTro NVARCHAR(30) NOT NULL CONSTRAINT CK_TaiKhoan_VaiTro CHECK (VaiTro IN (N'QuanLy', N'NhanVien')),
        TrangThai BIT NOT NULL DEFAULT 1
    );
END
GO

-- 2. Bang SuKien (Cac su kien, live show, hoi thao)
IF OBJECT_ID('SuKien', 'U') IS NULL
BEGIN
    CREATE TABLE SuKien (
        MaSK INT IDENTITY(1,1) PRIMARY KEY,
        TenSK NVARCHAR(200) NOT NULL,
        LoaiSK NVARCHAR(100),
        MoTa NVARCHAR(MAX),
        TrangThai NVARCHAR(30) NOT NULL DEFAULT N'SapDien'
            CONSTRAINT CK_SuKien_TrangThai CHECK (TrangThai IN (N'SapDien', N'DangDien', N'DaKetThuc', N'DaHuy'))
    );
END
GO

-- 3. Bang SuatDien (Cac buoi dien cu the cua su kien)
IF OBJECT_ID('SuatDien', 'U') IS NULL
BEGIN
    CREATE TABLE SuatDien (
        MaSuat INT IDENTITY(1,1) PRIMARY KEY,
        MaSK INT NOT NULL CONSTRAINT FK_SuatDien_SuKien FOREIGN KEY REFERENCES SuKien(MaSK),
        ThoiGianBatDau DATETIME NOT NULL,
        ThoiGianKetThuc DATETIME,
        DiaDiem NVARCHAR(255) NOT NULL,
        TrangThai NVARCHAR(30) NOT NULL DEFAULT N'MoCuaBanVe'
            CONSTRAINT CK_SuatDien_TrangThai CHECK (TrangThai IN (N'MoCuaBanVe', N'DongBanVe', N'DangDien', N'DaKetThuc'))
    );
END
GO

-- 4. Bang KhuVuc (Khu vuc khan phong / Hang ve cua tung suat dien)
IF OBJECT_ID('KhuVuc', 'U') IS NULL
BEGIN
    CREATE TABLE KhuVuc (
        MaKV INT IDENTITY(1,1) PRIMARY KEY,
        MaSuat INT NOT NULL CONSTRAINT FK_KhuVuc_SuatDien FOREIGN KEY REFERENCES SuatDien(MaSuat),
        TenKV NVARCHAR(100) NOT NULL,
        HangVe NVARCHAR(50) NOT NULL, -- VIP, Thuong, PhoThong
        GiaVe DECIMAL(18,0) NOT NULL CONSTRAINT CK_KhuVuc_GiaVe CHECK (GiaVe >= 0),
        TongSoGhe INT NOT NULL DEFAULT 0
    );
END
GO

-- 5. Bang Ghe (Cac ghe ngoi chi tiet trong khu vuc)
IF OBJECT_ID('Ghe', 'U') IS NULL
BEGIN
    CREATE TABLE Ghe (
        MaGhe INT IDENTITY(1,1) PRIMARY KEY,
        MaKV INT NOT NULL CONSTRAINT FK_Ghe_KhuVuc FOREIGN KEY REFERENCES KhuVuc(MaKV),
        Hang VARCHAR(10) NOT NULL,
        Cot INT NOT NULL,
        MaGheHienThi AS (Hang + CAST(Cot AS VARCHAR(10))) PERSISTED,
        TrangThai NVARCHAR(30) NOT NULL DEFAULT N'Trong'
            CONSTRAINT CK_Ghe_TrangThai CHECK (TrangThai IN (N'Trong', N'DangGiu', N'DaBan')),
        CONSTRAINT UQ_Ghe_ViTri UNIQUE (MaKV, Hang, Cot)
    );
END
GO

-- 6. Bang KhachHang (Thong tin nguoi mua ve)
IF OBJECT_ID('KhachHang', 'U') IS NULL
BEGIN
    CREATE TABLE KhachHang (
        MaKH INT IDENTITY(1,1) PRIMARY KEY,
        TenKH NVARCHAR(100) NOT NULL,
        SDT VARCHAR(20) NOT NULL CONSTRAINT UQ_KhachHang_SDT UNIQUE,
        Email NVARCHAR(100)
    );
END
GO

-- 7. Bang DonVe (Don dat ve / Hoa don ban ve)
IF OBJECT_ID('DonVe', 'U') IS NULL
BEGIN
    CREATE TABLE DonVe (
        MaDon INT IDENTITY(1,1) PRIMARY KEY,
        MaKH INT NOT NULL CONSTRAINT FK_DonVe_KhachHang FOREIGN KEY REFERENCES KhachHang(MaKH),
        MaSuat INT NOT NULL CONSTRAINT FK_DonVe_SuatDien FOREIGN KEY REFERENCES SuatDien(MaSuat),
        MaTK INT NOT NULL CONSTRAINT FK_DonVe_TaiKhoan FOREIGN KEY REFERENCES TaiKhoan(MaTK),
        NgayDat DATETIME NOT NULL DEFAULT GETDATE(),
        TongTien DECIMAL(18,0) NOT NULL DEFAULT 0,
        TrangThai NVARCHAR(30) NOT NULL DEFAULT N'DaThanhToan'
            CONSTRAINT CK_DonVe_TrangThai CHECK (TrangThai IN (N'DaThanhToan', N'DaHuy', N'DaDoi'))
    );
END
GO

-- 8. Bang ChiTietVe (Tung ve cua tung ghe trong don)
IF OBJECT_ID('ChiTietVe', 'U') IS NULL
BEGIN
    CREATE TABLE ChiTietVe (
        MaCT INT IDENTITY(1,1) PRIMARY KEY,
        MaDon INT NOT NULL CONSTRAINT FK_ChiTietVe_DonVe FOREIGN KEY REFERENCES DonVe(MaDon),
        MaGhe INT NOT NULL CONSTRAINT FK_ChiTietVe_Ghe FOREIGN KEY REFERENCES Ghe(MaGhe),
        GiaVe DECIMAL(18,0) NOT NULL,
        MaVeCode VARCHAR(50) NOT NULL CONSTRAINT UQ_ChiTietVe_MaVeCode UNIQUE,
        TrangThai NVARCHAR(20) NOT NULL DEFAULT N'HopLe'
            CONSTRAINT CK_ChiTietVe_TrangThai CHECK (TrangThai IN (N'HopLe', N'DaHuy')),
        CONSTRAINT UQ_ChiTietVe_DonGhe UNIQUE (MaDon, MaGhe)
    );
END
GO

-- 9. Bang ThanhToan (Giao dich thanh toan)
IF OBJECT_ID('ThanhToan', 'U') IS NULL
BEGIN
    CREATE TABLE ThanhToan (
        MaTT INT IDENTITY(1,1) PRIMARY KEY,
        MaDon INT NOT NULL CONSTRAINT FK_ThanhToan_DonVe FOREIGN KEY REFERENCES DonVe(MaDon),
        NgayThanhToan DATETIME NOT NULL DEFAULT GETDATE(),
        SoTien DECIMAL(18,0) NOT NULL,
        PhuongThuc NVARCHAR(30) NOT NULL
            CONSTRAINT CK_ThanhToan_PhuongThuc CHECK (PhuongThuc IN (N'TienMat', N'ChuyenKhoan', N'TheNganHang'))
    );
END
GO

-- 10. Bang NhatKy (Audit log thao tac nguoi dung)
IF OBJECT_ID('NhatKy', 'U') IS NULL
BEGIN
    CREATE TABLE NhatKy (
        MaNK INT IDENTITY(1,1) PRIMARY KEY,
        MaTK INT NOT NULL CONSTRAINT FK_NhatKy_TaiKhoan FOREIGN KEY REFERENCES TaiKhoan(MaTK),
        HanhDong NVARCHAR(100) NOT NULL,
        ThoiGian DATETIME NOT NULL DEFAULT GETDATE(),
        ChiTiet NVARCHAR(MAX)
    );
END
GO