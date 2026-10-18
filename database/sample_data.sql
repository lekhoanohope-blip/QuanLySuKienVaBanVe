-- ==============================================================
-- DE TAI 20: QUAN LY SU KIEN VA BAN VE
-- Script 02: Du lieu mau khoi tao va Tai khoan demo
-- ==============================================================

USE QuanLySuKienVaBanVe;
GO

-- 1. Tai khoan khoi tao (Mat khau: 123456)
IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE TenDangNhap = N'admin')
BEGIN
    INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThai)
    VALUES (N'admin', N'123456', N'QuanLy', 1);
END

IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE TenDangNhap = N'staff')
BEGIN
    INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThai)
    VALUES (N'staff', N'123456', N'NhanVien', 1);
END
GO

-- 2. Su kien mau
IF NOT EXISTS (SELECT 1 FROM SuKien WHERE TenSK = N'Live Concert: HCMUE Melody 2026')
BEGIN
    INSERT INTO SuKien (TenSK, LoaiSK, MoTa, TrangThai)
    VALUES (N'Live Concert: HCMUE Melody 2026', N'Ca nhạc', N'Đêm nhạc hội giao lưu sinh viên Trường ĐH Sư phạm TP.HCM', N'SapDien');
END

IF NOT EXISTS (SELECT 1 FROM SuKien WHERE TenSK = N'Hội thảo: Trí tuệ nhân tạo và Tương lai Lập trình')
BEGIN
    INSERT INTO SuKien (TenSK, LoaiSK, MoTa, TrangThai)
    VALUES (N'Hội thảo: Trí tuệ nhân tạo và Tương lai Lập trình', N'Hội thảo', N'Chia sẻ xu hướng công nghệ từ các chuyên gia đầu ngành', N'SapDien');
END
GO

-- 3. Suat dien mau
DECLARE @MaSK1 INT = (SELECT TOP 1 MaSK FROM SuKien WHERE TenSK = N'Live Concert: HCMUE Melody 2026');
IF @MaSK1 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM SuatDien WHERE MaSK = @MaSK1)
BEGIN
    INSERT INTO SuatDien (MaSK, ThoiGianBatDau, ThoiGianKetThuc, DiaDiem, TrangThai)
    VALUES (@MaSK1, DATEADD(DAY, 7, GETDATE()), DATEADD(HOUR, 3, DATEADD(DAY, 7, GETDATE())), N'Hội trường A - HCMUE (280 An Dương Vương, Q.5)', N'MoCuaBanVe');
END
GO

-- 4. Khu vuc va Hang ve mau
DECLARE @MaSuat1 INT = (SELECT TOP 1 MaSuat FROM SuatDien);
IF @MaSuat1 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM KhuVuc WHERE MaSuat = @MaSuat1)
BEGIN
    INSERT INTO KhuVuc (MaSuat, TenKV, HangVe, GiaVe, TongSoGhe)
    VALUES (@MaSuat1, N'Khu VIP Khán Đài Trực Diện', N'VIP', 500000, 20);

    INSERT INTO KhuVuc (MaSuat, TenKV, HangVe, GiaVe, TongSoGhe)
    VALUES (@MaSuat1, N'Khu Thường Tiêu Chuẩn', N'Thuong', 250000, 30);
END
GO

-- 5. Sinh ghe mau cho Khu VIP (2 hang A, B moi hang 10 ghe)
DECLARE @MaKV_VIP INT = (SELECT TOP 1 MaKV FROM KhuVuc WHERE HangVe = N'VIP');
IF @MaKV_VIP IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Ghe WHERE MaKV = @MaKV_VIP)
BEGIN
    DECLARE @c INT = 1;
    WHILE @c <= 10
    BEGIN
        INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_VIP, 'A', @c, N'Trong');
        INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_VIP, 'B', @c, N'Trong');
        SET @c = @c + 1;
    END
END
GO

-- 6. Khach hang mau
IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE SDT = '0901234567')
BEGIN
    INSERT INTO KhachHang (TenKH, SDT, Email)
    VALUES (N'Nguyễn Văn An', '0901234567', 'an.nguyen@example.com');
END

IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE SDT = '0987654321')
BEGIN
    INSERT INTO KhachHang (TenKH, SDT, Email)
    VALUES (N'Trần Thị Bình', '0987654321', 'binh.tran@example.com');
END
GO