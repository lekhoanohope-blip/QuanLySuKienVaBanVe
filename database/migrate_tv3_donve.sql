-- ==============================================================
-- ĐỀ TÀI 20: QUẢN LÝ SỰ KIỆN VÀ BÁN VÉ
-- Script migration độc lập của TV3 (Tuần 2):
-- Bổ sung MaDonGoc, ThoiGianGiu, Filtered Unique Index
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

PRINT N'=== HOÀN TẤT MIGRATION CẤU TRÚC TV3 THÀNH CÔNG ===';
GO
