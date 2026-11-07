-- ============================================================================
-- ĐỀ TÀI 20: QUẢN LÝ SỰ KIỆN VÀ BÁN VÉ
-- File: migrations/01_AddSchemaColumns.sql
-- Mục đích: Bổ sung các cột mở rộng (LoaiSoDo, ChoPhepBanVe, MaVung, MauSac,
--           ThoiGianGiu, MaPhienGiu, MaDonGoc) cho CSDL từ phiên bản cũ.
-- Lưu ý: Script idempotent, an toàn khi chạy nhiều lần.
-- ============================================================================

USE QuanLySuKienVaBanVe;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- 1. Bổ sung cột LoaiSoDo (1: Chữ T, 2: Hình chữ nhật, 3: Lưới) vào bảng SuatDien
IF COL_LENGTH('dbo.SuatDien', 'LoaiSoDo') IS NULL
BEGIN
    ALTER TABLE dbo.SuatDien ADD LoaiSoDo INT NOT NULL CONSTRAINT DF_SuatDien_LoaiSoDo DEFAULT 1;
END
GO

-- 2. Bổ sung các cột cấu hình sơ đồ vào bảng KhuVuc
IF COL_LENGTH('dbo.KhuVuc', 'ChoPhepBanVe') IS NULL
BEGIN
    ALTER TABLE dbo.KhuVuc ADD ChoPhepBanVe BIT NOT NULL CONSTRAINT DF_KhuVuc_ChoPhepBanVe DEFAULT 1;
END
GO

IF COL_LENGTH('dbo.KhuVuc', 'MaVung') IS NULL
BEGIN
    ALTER TABLE dbo.KhuVuc ADD MaVung VARCHAR(20) NULL;
END
GO

IF COL_LENGTH('dbo.KhuVuc', 'MauSac') IS NULL
BEGIN
    ALTER TABLE dbo.KhuVuc ADD MauSac VARCHAR(20) NULL;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_KhuVuc_TongSoGhe')
BEGIN
    UPDATE dbo.KhuVuc SET TongSoGhe = 1 WHERE TongSoGhe <= 0;
    ALTER TABLE dbo.KhuVuc WITH CHECK
        ADD CONSTRAINT CK_KhuVuc_TongSoGhe CHECK (TongSoGhe > 0 AND TongSoGhe <= 10000);
END
GO

-- 3. Bổ sung cột giữ ghế tạm thời vào bảng Ghe
IF COL_LENGTH('dbo.Ghe', 'ThoiGianGiu') IS NULL
BEGIN
    ALTER TABLE dbo.Ghe ADD ThoiGianGiu DATETIME NULL;
END
GO

IF COL_LENGTH('dbo.Ghe', 'MaPhienGiu') IS NULL
BEGIN
    ALTER TABLE dbo.Ghe ADD MaPhienGiu UNIQUEIDENTIFIER NULL;
END
GO

-- 4. Bổ sung cột MaDonGoc phục vụ truy vết đổi vé vào bảng DonVe
IF COL_LENGTH('dbo.DonVe', 'MaDonGoc') IS NULL
BEGIN
    ALTER TABLE dbo.DonVe ADD MaDonGoc INT NULL;
END
GO
