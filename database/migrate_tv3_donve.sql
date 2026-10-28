-- ==============================================================
-- ĐỀ TÀI 20: QUẢN LÝ SỰ KIỆN VÀ BÁN VÉ
-- Script migration độc lập của TV3 (Tuần 2):
-- Bổ sung MaDonGoc, ThoiGianGiu, Filtered Unique Index
-- và Dữ liệu mẫu tự kiểm thử nghiệp vụ Hủy/Đổi vé.
-- ==============================================================

USE QuanLySuKienVaBanVe;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- 1. ALTER DonVe ADD MaDonGoc INT NULL + Khóa ngoại tự tham chiếu
IF COL_LENGTH('dbo.DonVe', 'MaDonGoc') IS NULL
BEGIN
    ALTER TABLE dbo.DonVe ADD MaDonGoc INT NULL;
    PRINT N'Đã thêm cột MaDonGoc vào bảng DonVe.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_DonVe_DonVeGoc')
BEGIN
    ALTER TABLE dbo.DonVe
    ADD CONSTRAINT FK_DonVe_DonVeGoc FOREIGN KEY (MaDonGoc) REFERENCES dbo.DonVe(MaDon);
    PRINT N'Đã thêm ràng buộc FK_DonVe_DonVeGoc.';
END
GO

-- 2. ALTER Ghe ADD ThoiGianGiu DATETIME NULL
IF COL_LENGTH('dbo.Ghe', 'ThoiGianGiu') IS NULL
BEGIN
    ALTER TABLE dbo.Ghe ADD ThoiGianGiu DATETIME NULL;
    PRINT N'Đã thêm cột ThoiGianGiu vào bảng Ghe.';
END
GO

-- 3. Filtered Unique Index trên ChiTietVe(MaGhe) WHERE TrangThai = N'HopLe'
-- Ngăn chặn bán trùng 1 ghế trên 2 đơn hợp lệ tại cùng một thời điểm,
-- nhưng vẫn cho phép bán lại ghế đó khi vé cũ đã 'DaHuy' hoặc 'DaDoi'.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_ChiTietVe_Ghe_HopLe' AND object_id = OBJECT_ID('dbo.ChiTietVe'))
BEGIN
    CREATE UNIQUE INDEX UQ_ChiTietVe_Ghe_HopLe
    ON dbo.ChiTietVe(MaGhe)
    WHERE TrangThai = N'HopLe';
    PRINT N'Đã tạo Filtered Unique Index UQ_ChiTietVe_Ghe_HopLe.';
END
GO

-- 4. DỮ LIỆU TEST TỐI THIỂU CHO TV3 TỰ KIỂM THỬ (IDEMPOTENT)
-- Đảm bảo có tài khoản Quản lý (admin) và Nhân viên
IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE TenDangNhap = N'admin')
BEGIN
    INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThai)
    VALUES (N'admin', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'QuanLy', 1);
END
GO

DECLARE @MaTK_Admin INT = (SELECT TOP 1 MaTK FROM TaiKhoan WHERE TenDangNhap = N'admin');

-- Khach hang test TV3 (UPSERT de luon dam bao ten tieng Viet co dau chuan xac)
IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE SDT = '0911222333')
    INSERT INTO KhachHang (TenKH, SDT, Email) VALUES (N'Lê Văn Test Hủy (>=24h)', '0911222333', 'test.huy24h@example.com');
ELSE
    UPDATE KhachHang SET TenKH = N'Lê Văn Test Hủy (>=24h)', Email = 'test.huy24h@example.com' WHERE SDT = '0911222333';

IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE SDT = '0944555666')
    INSERT INTO KhachHang (TenKH, SDT, Email) VALUES (N'Phạm Thị Test Đổi (18h)', '0944555666', 'test.doi18h@example.com');
ELSE
    UPDATE KhachHang SET TenKH = N'Phạm Thị Test Đổi (18h)', Email = 'test.doi18h@example.com' WHERE SDT = '0944555666';

IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE SDT = '0977888999')
    INSERT INTO KhachHang (TenKH, SDT, Email) VALUES (N'Hoàng Văn Test Chặn (<12h)', '0977888999', 'test.chan12h@example.com');
ELSE
    UPDATE KhachHang SET TenKH = N'Hoàng Văn Test Chặn (<12h)', Email = 'test.chan12h@example.com' WHERE SDT = '0977888999';
GO

-- Su kien test TV3 (UPSERT)
IF NOT EXISTS (SELECT 1 FROM SuKien WHERE TenSK LIKE '%TV3-Test%')
    INSERT INTO SuKien (TenSK, LoaiSK, MoTa, TrangThai)
    VALUES (N'[TV3-Test] Đêm Nhạc Acoustic Mùa Thu', N'Ca nhạc', N'Sự kiện test độc lập các ca hủy và đổi vé của TV3', N'DangDien');
ELSE
    UPDATE SuKien 
    SET TenSK = N'[TV3-Test] Đêm Nhạc Acoustic Mùa Thu',
        LoaiSK = N'Ca nhạc',
        MoTa = N'Sự kiện test độc lập các ca hủy và đổi vé của TV3'
    WHERE TenSK LIKE '%TV3-Test%';
GO


DECLARE @MaSK_Test INT = (SELECT TOP 1 MaSK FROM SuKien WHERE TenSK LIKE '%TV3-Test%');

-- Suat 1: Cach hien tai +72 gio (> 24h, du dieu kien ca huy va doi)
IF NOT EXISTS (SELECT 1 FROM SuatDien WHERE MaSK = @MaSK_Test AND DiaDiem LIKE '%72h%')
    INSERT INTO SuatDien (MaSK, ThoiGianBatDau, ThoiGianKetThuc, DiaDiem, TrangThai)
    VALUES (@MaSK_Test, DATEADD(HOUR, 72, GETDATE()), DATEADD(HOUR, 75, GETDATE()), N'Khán phòng A (Suất 72h - Đủ điều kiện)', N'MoCuaBanVe');
ELSE
    UPDATE SuatDien 
    SET DiaDiem = N'Khán phòng A (Suất 72h - Đủ điều kiện)',
        ThoiGianBatDau = DATEADD(HOUR, 72, GETDATE()),
        ThoiGianKetThuc = DATEADD(HOUR, 75, GETDATE())
    WHERE MaSK = @MaSK_Test AND DiaDiem LIKE '%72h%';

-- Suat 2: Cach hien tai +18 gio (12h <= tg < 24h: Chan huy nhung CHO DOI)
IF NOT EXISTS (SELECT 1 FROM SuatDien WHERE MaSK = @MaSK_Test AND DiaDiem LIKE '%18h%')
    INSERT INTO SuatDien (MaSK, ThoiGianBatDau, ThoiGianKetThuc, DiaDiem, TrangThai)
    VALUES (@MaSK_Test, DATEADD(HOUR, 18, GETDATE()), DATEADD(HOUR, 21, GETDATE()), N'Khán phòng B (Suất 18h - Cho đổi, chặn hủy)', N'MoCuaBanVe');
ELSE
    UPDATE SuatDien 
    SET DiaDiem = N'Khán phòng B (Suất 18h - Cho đổi, chặn hủy)',
        ThoiGianBatDau = DATEADD(HOUR, 18, GETDATE()),
        ThoiGianKetThuc = DATEADD(HOUR, 21, GETDATE())
    WHERE MaSK = @MaSK_Test AND DiaDiem LIKE '%18h%';

-- Suat 3: Cach hien tai +6 gio (< 12h: CHAN CA HUY LAN DOI)
IF NOT EXISTS (SELECT 1 FROM SuatDien WHERE MaSK = @MaSK_Test AND DiaDiem LIKE '%6h%')
    INSERT INTO SuatDien (MaSK, ThoiGianBatDau, ThoiGianKetThuc, DiaDiem, TrangThai)
    VALUES (@MaSK_Test, DATEADD(HOUR, 6, GETDATE()), DATEADD(HOUR, 9, GETDATE()), N'Khán phòng C (Suất 6h - Chặn toàn bộ)', N'MoCuaBanVe');
ELSE
    UPDATE SuatDien 
    SET DiaDiem = N'Khán phòng C (Suất 6h - Chặn toàn bộ)',
        ThoiGianBatDau = DATEADD(HOUR, 6, GETDATE()),
        ThoiGianKetThuc = DATEADD(HOUR, 9, GETDATE())
    WHERE MaSK = @MaSK_Test AND DiaDiem LIKE '%6h%';
GO

-- Khu vuc cho cac suat test
DECLARE @MaSK_Test INT = (SELECT TOP 1 MaSK FROM SuKien WHERE TenSK LIKE '%TV3-Test%');
DECLARE @MaSuat1 INT = (SELECT TOP 1 MaSuat FROM SuatDien WHERE MaSK = @MaSK_Test AND DiaDiem LIKE '%72h%');
DECLARE @MaSuat2 INT = (SELECT TOP 1 MaSuat FROM SuatDien WHERE MaSK = @MaSK_Test AND DiaDiem LIKE '%18h%');
DECLARE @MaSuat3 INT = (SELECT TOP 1 MaSuat FROM SuatDien WHERE MaSK = @MaSK_Test AND DiaDiem LIKE '%6h%');

IF @MaSuat1 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM KhuVuc WHERE MaSuat = @MaSuat1 AND HangVe = N'VIP')
    INSERT INTO KhuVuc (MaSuat, TenKV, HangVe, GiaVe, TongSoGhe) VALUES (@MaSuat1, N'Khu VIP Khán Đài', N'VIP', 500000, 10);
IF @MaSuat1 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM KhuVuc WHERE MaSuat = @MaSuat1 AND HangVe = N'Thuong')
    INSERT INTO KhuVuc (MaSuat, TenKV, HangVe, GiaVe, TongSoGhe) VALUES (@MaSuat1, N'Khu Thường Tiêu Chuẩn', N'Thuong', 250000, 10);
IF @MaSuat1 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM KhuVuc WHERE MaSuat = @MaSuat1 AND HangVe = N'PhoThong')
    INSERT INTO KhuVuc (MaSuat, TenKV, HangVe, GiaVe, TongSoGhe) VALUES (@MaSuat1, N'Khu Phổ Thông', N'PhoThong', 150000, 10);

IF @MaSuat2 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM KhuVuc WHERE MaSuat = @MaSuat2 AND HangVe = N'VIP')
    INSERT INTO KhuVuc (MaSuat, TenKV, HangVe, GiaVe, TongSoGhe) VALUES (@MaSuat2, N'Khu VIP Suất 2', N'VIP', 500000, 10);
IF @MaSuat2 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM KhuVuc WHERE MaSuat = @MaSuat2 AND HangVe = N'Thuong')
    INSERT INTO KhuVuc (MaSuat, TenKV, HangVe, GiaVe, TongSoGhe) VALUES (@MaSuat2, N'Khu Thường Suất 2', N'Thuong', 250000, 10);

IF @MaSuat3 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM KhuVuc WHERE MaSuat = @MaSuat3 AND HangVe = N'Thuong')
    INSERT INTO KhuVuc (MaSuat, TenKV, HangVe, GiaVe, TongSoGhe) VALUES (@MaSuat3, N'Khu Thường Suất 3', N'Thuong', 250000, 10);

-- Cap nhat lai TenKV Unicode chuan
IF @MaSuat1 IS NOT NULL
BEGIN
    UPDATE KhuVuc SET TenKV = N'Khu VIP Khán Đài' WHERE MaSuat = @MaSuat1 AND HangVe = N'VIP';
    UPDATE KhuVuc SET TenKV = N'Khu Thường Tiêu Chuẩn' WHERE MaSuat = @MaSuat1 AND HangVe = N'Thuong';
    UPDATE KhuVuc SET TenKV = N'Khu Phổ Thông' WHERE MaSuat = @MaSuat1 AND HangVe = N'PhoThong';
END
IF @MaSuat2 IS NOT NULL
BEGIN
    UPDATE KhuVuc SET TenKV = N'Khu VIP Suất 2' WHERE MaSuat = @MaSuat2 AND HangVe = N'VIP';
    UPDATE KhuVuc SET TenKV = N'Khu Thường Suất 2' WHERE MaSuat = @MaSuat2 AND HangVe = N'Thuong';
END
IF @MaSuat3 IS NOT NULL
BEGIN
    UPDATE KhuVuc SET TenKV = N'Khu Thường Suất 3' WHERE MaSuat = @MaSuat3 AND HangVe = N'Thuong';
END
GO

-- Chen tay mot so ghe mau (khong phu thuoc thuat toan sinh ghe cua TV2)
DECLARE @MaSK_Test INT = (SELECT TOP 1 MaSK FROM SuKien WHERE TenSK LIKE '%TV3-Test%');
DECLARE @MaSuat1 INT = (SELECT TOP 1 MaSuat FROM SuatDien WHERE MaSK = @MaSK_Test AND DiaDiem LIKE '%72h%');
DECLARE @MaSuat2 INT = (SELECT TOP 1 MaSuat FROM SuatDien WHERE MaSK = @MaSK_Test AND DiaDiem LIKE '%18h%');
DECLARE @MaSuat3 INT = (SELECT TOP 1 MaSuat FROM SuatDien WHERE MaSK = @MaSK_Test AND DiaDiem LIKE '%6h%');

DECLARE @MaKV_VIP1 INT = (SELECT TOP 1 MaKV FROM KhuVuc WHERE MaSuat = @MaSuat1 AND HangVe = N'VIP');
DECLARE @MaKV_Thuong1 INT = (SELECT TOP 1 MaKV FROM KhuVuc WHERE MaSuat = @MaSuat1 AND HangVe = N'Thuong');
DECLARE @MaKV_PhoThong1 INT = (SELECT TOP 1 MaKV FROM KhuVuc WHERE MaSuat = @MaSuat1 AND HangVe = N'PhoThong');
DECLARE @MaKV_VIP2 INT = (SELECT TOP 1 MaKV FROM KhuVuc WHERE MaSuat = @MaSuat2 AND HangVe = N'VIP');
DECLARE @MaKV_Thuong2 INT = (SELECT TOP 1 MaKV FROM KhuVuc WHERE MaSuat = @MaSuat2 AND HangVe = N'Thuong');
DECLARE @MaKV_Thuong3 INT = (SELECT TOP 1 MaKV FROM KhuVuc WHERE MaSuat = @MaSuat3 AND HangVe = N'Thuong');

-- Ghe Khu VIP Suat 1 (A1 -> A5)
IF @MaKV_VIP1 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Ghe WHERE MaKV = @MaKV_VIP1)
BEGIN
    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_VIP1, 'A', 1, N'Trong');
    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_VIP1, 'A', 2, N'Trong');
    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_VIP1, 'A', 3, N'Trong');
    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_VIP1, 'A', 4, N'Trong');
    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_VIP1, 'A', 5, N'Trong');
END

-- Ghe Khu Thuong Suat 1 (B1 -> B5)
IF @MaKV_Thuong1 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Ghe WHERE MaKV = @MaKV_Thuong1)
BEGIN
    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_Thuong1, 'B', 1, N'Trong');
    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_Thuong1, 'B', 2, N'Trong');
    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_Thuong1, 'B', 3, N'Trong');
    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_Thuong1, 'B', 4, N'Trong');
    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_Thuong1, 'B', 5, N'Trong');
END

-- Ghe Khu Pho Thong Suat 1 (C1 -> C5)
IF @MaKV_PhoThong1 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Ghe WHERE MaKV = @MaKV_PhoThong1)
BEGIN
    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_PhoThong1, 'C', 1, N'Trong');
    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_PhoThong1, 'C', 2, N'Trong');
    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_PhoThong1, 'C', 3, N'Trong');
END

-- Ghe Suat 2
IF @MaKV_VIP2 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Ghe WHERE MaKV = @MaKV_VIP2)
BEGIN
    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_VIP2, 'A', 1, N'Trong');
    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_VIP2, 'A', 2, N'Trong');
END

IF @MaKV_Thuong2 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Ghe WHERE MaKV = @MaKV_Thuong2)
BEGIN
    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_Thuong2, 'B', 1, N'Trong');
    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_Thuong2, 'B', 2, N'Trong');
    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_Thuong2, 'B', 3, N'Trong');
END

-- Ghe Suat 3
IF @MaKV_Thuong3 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Ghe WHERE MaKV = @MaKV_Thuong3)
BEGIN
    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_Thuong3, 'B', 1, N'Trong');
    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai) VALUES (@MaKV_Thuong3, 'B', 2, N'Trong');
END
GO

-- Chen don ve mau de kiem thu
DECLARE @MaSK_Test INT = (SELECT TOP 1 MaSK FROM SuKien WHERE TenSK LIKE '%TV3-Test%');
DECLARE @MaTK_Admin INT = (SELECT TOP 1 MaTK FROM TaiKhoan WHERE TenDangNhap = N'admin');
DECLARE @MaKH1 INT = (SELECT TOP 1 MaKH FROM KhachHang WHERE SDT = '0911222333');
DECLARE @MaKH2 INT = (SELECT TOP 1 MaKH FROM KhachHang WHERE SDT = '0944555666');
DECLARE @MaKH3 INT = (SELECT TOP 1 MaKH FROM KhachHang WHERE SDT = '0977888999');

DECLARE @MaSuat1 INT = (SELECT TOP 1 MaSuat FROM SuatDien WHERE MaSK = @MaSK_Test AND DiaDiem LIKE '%72h%');
DECLARE @MaSuat2 INT = (SELECT TOP 1 MaSuat FROM SuatDien WHERE MaSK = @MaSK_Test AND DiaDiem LIKE '%18h%');
DECLARE @MaSuat3 INT = (SELECT TOP 1 MaSuat FROM SuatDien WHERE MaSK = @MaSK_Test AND DiaDiem LIKE '%6h%');

-- Cap nhat moc thoi gian chuan cho suat test: +72h, +18h, +6h
IF @MaSuat1 IS NOT NULL UPDATE SuatDien SET ThoiGianBatDau = DATEADD(HOUR, 72, GETDATE()), ThoiGianKetThuc = DATEADD(HOUR, 75, GETDATE()) WHERE MaSuat = @MaSuat1;
IF @MaSuat2 IS NOT NULL UPDATE SuatDien SET ThoiGianBatDau = DATEADD(HOUR, 18, GETDATE()), ThoiGianKetThuc = DATEADD(HOUR, 21, GETDATE()) WHERE MaSuat = @MaSuat2;
IF @MaSuat3 IS NOT NULL UPDATE SuatDien SET ThoiGianBatDau = DATEADD(HOUR, 6, GETDATE()), ThoiGianKetThuc = DATEADD(HOUR, 9, GETDATE()) WHERE MaSuat = @MaSuat3;

-- Don dep du lieu don ve test cu cua TV3 de bao dam tinh idempotent tuyet doi khi tai chay kiem thu
IF @MaSK_Test IS NOT NULL
BEGIN
    DELETE FROM ThanhToan WHERE MaDon IN (SELECT MaDon FROM DonVe WHERE MaSuat IN (@MaSuat1, @MaSuat2, @MaSuat3));
    DELETE FROM ChiTietVe WHERE MaDon IN (SELECT MaDon FROM DonVe WHERE MaSuat IN (@MaSuat1, @MaSuat2, @MaSuat3));
    DELETE FROM DonVe WHERE MaDonGoc IS NOT NULL AND MaSuat IN (@MaSuat1, @MaSuat2, @MaSuat3);
    DELETE FROM DonVe WHERE MaSuat IN (@MaSuat1, @MaSuat2, @MaSuat3);
    UPDATE Ghe SET TrangThai = N'Trong', ThoiGianGiu = NULL 
    WHERE MaKV IN (SELECT MaKV FROM KhuVuc WHERE MaSuat IN (@MaSuat1, @MaSuat2, @MaSuat3));
END

DECLARE @MaKV_VIP1 INT = (SELECT TOP 1 MaKV FROM KhuVuc WHERE MaSuat = @MaSuat1 AND HangVe = N'VIP');
DECLARE @MaKV_Thuong1 INT = (SELECT TOP 1 MaKV FROM KhuVuc WHERE MaSuat = @MaSuat1 AND HangVe = N'Thuong');
DECLARE @MaKV_PhoThong1 INT = (SELECT TOP 1 MaKV FROM KhuVuc WHERE MaSuat = @MaSuat1 AND HangVe = N'PhoThong');
DECLARE @MaKV_VIP2 INT = (SELECT TOP 1 MaKV FROM KhuVuc WHERE MaSuat = @MaSuat2 AND HangVe = N'VIP');
DECLARE @MaKV_Thuong2 INT = (SELECT TOP 1 MaKV FROM KhuVuc WHERE MaSuat = @MaSuat2 AND HangVe = N'Thuong');
DECLARE @MaKV_Thuong3 INT = (SELECT TOP 1 MaKV FROM KhuVuc WHERE MaSuat = @MaSuat3 AND HangVe = N'Thuong');

-- Don 1: Ban 1 ve VIP A1 (500.000d) cho Suat 1 (+72h) -> Dung de test Huy hop le hoac Doi ve
DECLARE @Ghe_A1 INT = (SELECT TOP 1 MaGhe FROM Ghe WHERE MaKV = @MaKV_VIP1 AND Hang = 'A' AND Cot = 1);
IF @Ghe_A1 IS NOT NULL
BEGIN
    INSERT INTO DonVe (MaKH, MaSuat, MaTK, MaDonGoc, NgayDat, TongTien, TrangThai)
    VALUES (@MaKH1, @MaSuat1, @MaTK_Admin, NULL, GETDATE(), 500000, N'DaThanhToan');
    
    DECLARE @MaDon1 INT = SCOPE_IDENTITY();
    
    INSERT INTO ChiTietVe (MaDon, MaGhe, GiaVe, MaVeCode, TrangThai)
    VALUES (@MaDon1, @Ghe_A1, 500000, 'TV3-TEST-72H-A1', N'HopLe');
    
    UPDATE Ghe SET TrangThai = N'DaBan', ThoiGianGiu = NULL WHERE MaGhe = @Ghe_A1;
    
    INSERT INTO ThanhToan (MaDon, NgayThanhToan, SoTien, PhuongThuc)
    VALUES (@MaDon1, GETDATE(), 500000, N'TienMat');
END

-- Don 2: Ban 1 ve Thuong B1 (250.000d) cho Suat 2 (+18h) -> Dung de test CHAN HUY (<24h) nhung CHO DOI (>=12h)
DECLARE @Ghe_B1_S2 INT = (SELECT TOP 1 MaGhe FROM Ghe WHERE MaKV = @MaKV_Thuong2 AND Hang = 'B' AND Cot = 1);
IF @Ghe_B1_S2 IS NOT NULL
BEGIN
    INSERT INTO DonVe (MaKH, MaSuat, MaTK, MaDonGoc, NgayDat, TongTien, TrangThai)
    VALUES (@MaKH2, @MaSuat2, @MaTK_Admin, NULL, GETDATE(), 250000, N'DaThanhToan');
    
    DECLARE @MaDon2 INT = SCOPE_IDENTITY();
    
    INSERT INTO ChiTietVe (MaDon, MaGhe, GiaVe, MaVeCode, TrangThai)
    VALUES (@MaDon2, @Ghe_B1_S2, 250000, 'TV3-TEST-18H-B1', N'HopLe');
    
    UPDATE Ghe SET TrangThai = N'DaBan', ThoiGianGiu = NULL WHERE MaGhe = @Ghe_B1_S2;
    
    INSERT INTO ThanhToan (MaDon, NgayThanhToan, SoTien, PhuongThuc)
    VALUES (@MaDon2, GETDATE(), 250000, N'ChuyenKhoan');
END

-- Don 3: Ban 1 ve Thuong B1 (250.000d) cho Suat 3 (+6h) -> Dung de test CHAN CA HUY LAN DOI (<12h)
DECLARE @Ghe_B1_S3 INT = (SELECT TOP 1 MaGhe FROM Ghe WHERE MaKV = @MaKV_Thuong3 AND Hang = 'B' AND Cot = 1);
IF @Ghe_B1_S3 IS NOT NULL
BEGIN
    INSERT INTO DonVe (MaKH, MaSuat, MaTK, MaDonGoc, NgayDat, TongTien, TrangThai)
    VALUES (@MaKH3, @MaSuat3, @MaTK_Admin, NULL, GETDATE(), 250000, N'DaThanhToan');
    
    DECLARE @MaDon3 INT = SCOPE_IDENTITY();
    
    INSERT INTO ChiTietVe (MaDon, MaGhe, GiaVe, MaVeCode, TrangThai)
    VALUES (@MaDon3, @Ghe_B1_S3, 250000, 'TV3-TEST-06H-B1', N'HopLe');
    
    UPDATE Ghe SET TrangThai = N'DaBan', ThoiGianGiu = NULL WHERE MaGhe = @Ghe_B1_S3;
    
    INSERT INTO ThanhToan (MaDon, NgayThanhToan, SoTien, PhuongThuc)
    VALUES (@MaDon3, GETDATE(), 250000, N'TheNganHang');
END

-- Don 4 (Multi-ticket): Ban 2 ve VIP (A2, A3 tong 1.000.000d) cho Suat 1 de test doi 1 ve trong don nhieu ve (Cach 2)
DECLARE @Ghe_A2 INT = (SELECT TOP 1 MaGhe FROM Ghe WHERE MaKV = @MaKV_VIP1 AND Hang = 'A' AND Cot = 2);
DECLARE @Ghe_A3 INT = (SELECT TOP 1 MaGhe FROM Ghe WHERE MaKV = @MaKV_VIP1 AND Hang = 'A' AND Cot = 3);
IF @Ghe_A2 IS NOT NULL AND @Ghe_A3 IS NOT NULL
BEGIN
    INSERT INTO DonVe (MaKH, MaSuat, MaTK, MaDonGoc, NgayDat, TongTien, TrangThai)
    VALUES (@MaKH1, @MaSuat1, @MaTK_Admin, NULL, GETDATE(), 1000000, N'DaThanhToan');
    
    DECLARE @MaDon4 INT = SCOPE_IDENTITY();
    
    INSERT INTO ChiTietVe (MaDon, MaGhe, GiaVe, MaVeCode, TrangThai)
    VALUES (@MaDon4, @Ghe_A2, 500000, 'TV3-TEST-MULTI-A2', N'HopLe');
    INSERT INTO ChiTietVe (MaDon, MaGhe, GiaVe, MaVeCode, TrangThai)
    VALUES (@MaDon4, @Ghe_A3, 500000, 'TV3-TEST-MULTI-A3', N'HopLe');
    
    UPDATE Ghe SET TrangThai = N'DaBan', ThoiGianGiu = NULL WHERE MaGhe IN (@Ghe_A2, @Ghe_A3);
    
    INSERT INTO ThanhToan (MaDon, NgayThanhToan, SoTien, PhuongThuc)
    VALUES (@MaDon4, GETDATE(), 1000000, N'TienMat');
END
GO
GO

PRINT N'=== HOÀN TẤT MIGRATION VÀ KHỞI TẠO DỮ LIỆU TEST TV3 THÀNH CÔNG ===';
GO
