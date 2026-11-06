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

-- 3. Chỉ ánh xạ tên nhận diện được; không suy ra vị trí vật lý từ ID hoặc hạng vé.
-- Tên mơ hồ, mã bị trùng và cấu hình có sẵn được giữ nguyên để Admin kiểm tra.
;WITH Names AS (
    SELECT MaKV, MaSuat, TenKV,
           N' ' + UPPER(REPLACE(LTRIM(RTRIM(TenKV)), N'_', N' ')) + N' ' AS NormalizedName
    FROM dbo.KhuVuc
    WHERE NULLIF(LTRIM(RTRIM(MaVung)), '') IS NULL
), Matches AS (
    SELECT n.MaKV, n.MaSuat, z.MaVung
    FROM Names n
    CROSS JOIN (VALUES
        ('VIP_A'), ('VIP_B'), ('VIP_C'), ('VIP_D'),
        ('GA_A'), ('GA_B'), ('GA_C'), ('GA_D'), ('GA_E'), ('GA_F'), ('GA_G'), ('GA_H'),
        ('CAT'), ('CAT_A'), ('CAT_B'), ('CAT_C'), ('CAT_D')
    ) z(MaVung)
    WHERE CHARINDEX(N' ' + REPLACE(z.MaVung, '_', ' ') + N' ', n.NormalizedName) > 0
    UNION
    SELECT MaKV, MaSuat,
           CASE WHEN TenKV IN (N'Khu VIP Khán Đài Trực Diện', N'[DEMO] Khu VIP')
                THEN 'VIP_A' ELSE 'CAT' END
    FROM Names
    WHERE TenKV IN (N'Khu VIP Khán Đài Trực Diện', N'[DEMO] Khu VIP',
                    N'Khu Thường Tiêu Chuẩn', N'[DEMO] Khu tiêu chuẩn')
), Ranked AS (
    SELECT *, DENSE_RANK() OVER (PARTITION BY MaKV ORDER BY LEN(MaVung) DESC) AS Specificity
    FROM Matches
), NamedCandidates AS (
    SELECT MaKV, MaSuat, MaVung,
           COUNT(*) OVER (PARTITION BY MaKV) AS NameMatches
    FROM Ranked WHERE Specificity = 1
), Candidates AS (
    SELECT *, COUNT(*) OVER (PARTITION BY MaSuat, MaVung) AS ZoneMatches
    FROM NamedCandidates WHERE NameMatches = 1
)
UPDATE kv SET MaVung = c.MaVung
FROM dbo.KhuVuc kv JOIN Candidates c ON c.MaKV = kv.MaKV
WHERE c.NameMatches = 1 AND c.ZoneMatches = 1
  AND NOT EXISTS (
      SELECT 1 FROM dbo.KhuVuc existing
      WHERE existing.MaSuat = c.MaSuat AND existing.MaVung = c.MaVung
  );
GO

PRINT N'=== HOÀN TẤT MIGRATION BÁN VÉ TV3 THÀNH CÔNG ===';
GO
