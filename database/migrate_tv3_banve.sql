-- ==============================================================
-- ĐỀ TÀI 20: QUẢN LÝ SỰ KIỆN VÀ BÁN VÉ
-- Script migration độc lập của TV3 (Bán vé):
-- Bổ sung ThoiGianGiu, MaPhienGiu, MauSac, chuẩn hóa mã vùng & màu sắc
-- ==============================================================

USE QuanLySuKienVaBanVe;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- 1. Bổ sung cột ThoiGianGiu và MaPhienGiu vào bảng Ghe
IF COL_LENGTH('dbo.Ghe', 'ThoiGianGiu') IS NULL
BEGIN
    ALTER TABLE dbo.Ghe ADD ThoiGianGiu DATETIME NULL;
    PRINT N'Đã thêm cột ThoiGianGiu vào bảng Ghe.';
END
GO

IF COL_LENGTH('dbo.Ghe', 'MaPhienGiu') IS NULL
BEGIN
    ALTER TABLE dbo.Ghe ADD MaPhienGiu UNIQUEIDENTIFIER NULL;
    PRINT N'Đã thêm cột MaPhienGiu vào bảng Ghe.';
END
GO

-- MaPhienGiu là token sở hữu lượt giữ ghế, không còn là khóa ngoại tới bảng phiên.
-- Chỉ gỡ ràng buộc cũ của Ghe; giữ nguyên dữ liệu phiên và liên kết lịch sử của DonVe.
IF EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE name = 'FK_Ghe_PhienGiuGhe'
      AND parent_object_id = OBJECT_ID('dbo.Ghe')
      AND referenced_object_id = OBJECT_ID('dbo.PhienGiuGhe')
)
BEGIN
    ALTER TABLE dbo.Ghe DROP CONSTRAINT FK_Ghe_PhienGiuGhe;
    PRINT N'Đã gỡ khóa ngoại phiên giữ ghế cũ; MaPhienGiu được dùng làm token sở hữu.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Ghe_MaPhienGiu' AND object_id = OBJECT_ID('dbo.Ghe'))
BEGIN
    CREATE INDEX IX_Ghe_MaPhienGiu ON dbo.Ghe(MaPhienGiu) WHERE MaPhienGiu IS NOT NULL;
    PRINT N'Đã tạo chỉ mục IX_Ghe_MaPhienGiu.';
END
GO

-- 2. Bổ sung cột MauSac vào bảng KhuVuc và dọn màu không hợp lệ
IF COL_LENGTH('dbo.KhuVuc', 'MauSac') IS NULL
BEGIN
    ALTER TABLE dbo.KhuVuc ADD MauSac VARCHAR(20) NULL;
    PRINT N'Đã thêm cột MauSac vào bảng KhuVuc.';
END
GO

UPDATE dbo.KhuVuc
SET MauSac = NULL
WHERE MauSac IS NOT NULL
  AND MauSac NOT LIKE '#[0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f]';
GO

-- 3. Gán MaVung cho các khu vực dữ liệu mẫu chưa được ánh xạ
;WITH x AS (
    SELECT MaKV, HangVe,
           ROW_NUMBER() OVER (PARTITION BY MaSuat, HangVe ORDER BY MaKV) AS rn
    FROM dbo.KhuVuc
    WHERE MaVung IS NULL OR MaVung = ''
)
UPDATE kv
SET MaVung = CASE
    WHEN x.HangVe = N'VIP'      AND x.rn = 1 THEN 'VIP_A'
    WHEN x.HangVe = N'VIP'      AND x.rn = 2 THEN 'VIP_B'
    WHEN x.HangVe = N'PhoThong' AND x.rn = 1 THEN 'GA_A'
    WHEN x.HangVe = N'PhoThong' AND x.rn = 2 THEN 'GA_B'
    WHEN x.HangVe = N'Thuong'   AND x.rn = 1 THEN 'CAT'
END
FROM dbo.KhuVuc kv
JOIN x ON x.MaKV = kv.MaKV
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.KhuVuc k2
    WHERE k2.MaSuat = kv.MaSuat
      AND k2.MaVung = CASE
          WHEN x.HangVe = N'VIP'      AND x.rn = 1 THEN 'VIP_A'
          WHEN x.HangVe = N'VIP'      AND x.rn = 2 THEN 'VIP_B'
          WHEN x.HangVe = N'PhoThong' AND x.rn = 1 THEN 'GA_A'
          WHEN x.HangVe = N'PhoThong' AND x.rn = 2 THEN 'GA_B'
          WHEN x.HangVe = N'Thuong'   AND x.rn = 1 THEN 'CAT'
      END
);
GO

PRINT N'=== HOÀN TẤT MIGRATION BÁN VÉ TV3 THÀNH CÔNG ===';
GO
