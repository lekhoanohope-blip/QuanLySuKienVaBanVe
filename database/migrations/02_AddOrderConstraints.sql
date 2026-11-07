-- ============================================================================
-- ĐỀ TÀI 20: QUẢN LÝ SỰ KIỆN VÀ BÁN VÉ
-- File: migrations/02_AddOrderConstraints.sql
-- Mục đích: Bổ sung khóa ngoại tự tham chiếu DonVe.MaDonGoc và chỉ mục lọc
--           UQ_ChiTietVe_Ghe_HopLe ngăn bán trùng ghế hiệu lực.
-- Lưu ý: Script idempotent, an toàn khi chạy nhiều lần.
-- ============================================================================

USE QuanLySuKienVaBanVe;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- 1. Đảm bảo cột MaDonGoc tồn tại và gắn khóa ngoại tự tham chiếu trên bảng DonVe
IF COL_LENGTH('dbo.DonVe', 'MaDonGoc') IS NULL
BEGIN
    ALTER TABLE dbo.DonVe ADD MaDonGoc INT NULL;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_DonVe_DonVeGoc')
BEGIN
    ALTER TABLE dbo.DonVe
        ADD CONSTRAINT FK_DonVe_DonVeGoc FOREIGN KEY (MaDonGoc) REFERENCES dbo.DonVe(MaDon);
END
GO

-- 2. Đảm bảo cột ThoiGianGiu tồn tại trên bảng Ghe
IF COL_LENGTH('dbo.Ghe', 'ThoiGianGiu') IS NULL
BEGIN
    ALTER TABLE dbo.Ghe ADD ThoiGianGiu DATETIME NULL;
END
GO

-- 3. Filtered Unique Index chống bán trùng cùng một ghế trên hai vé hợp lệ,
--    đồng thời vẫn cho phép bán lại ghế đó sau khi vé cũ chuyển sang DaHuy/DaDoi
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'UQ_ChiTietVe_Ghe_HopLe' AND object_id = OBJECT_ID('dbo.ChiTietVe')
)
BEGIN
    CREATE UNIQUE INDEX UQ_ChiTietVe_Ghe_HopLe
        ON dbo.ChiTietVe(MaGhe)
        WHERE TrangThai = N'HopLe';
END
GO
