-- ============================================================================
-- ĐỀ TÀI 20: QUẢN LÝ SỰ KIỆN VÀ BÁN VÉ
-- File: migrations/03_AddSeatHoldAndZoneColor.sql
-- Mục đích: Chuẩn hóa cơ chế giữ ghế bằng token MaPhienGiu, chỉ mục lọc trên Ghe,
--           định dạng mã màu hex (#RRGGBB) và ánh xạ MaVung cho dữ liệu khu vực cũ.
-- Lưu ý: Script idempotent, an toàn khi chạy nhiều lần.
-- ============================================================================

USE QuanLySuKienVaBanVe;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- 1. Đảm bảo các cột giữ ghế tạm thời tồn tại trên bảng Ghe
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

-- Gỡ khóa ngoại phiên giữ ghế cũ (nếu có) vì MaPhienGiu được dùng làm token sở hữu độc lập
IF EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE name = 'FK_Ghe_PhienGiuGhe'
      AND parent_object_id = OBJECT_ID('dbo.Ghe')
)
BEGIN
    ALTER TABLE dbo.Ghe DROP CONSTRAINT FK_Ghe_PhienGiuGhe;
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Ghe_MaPhienGiu' AND object_id = OBJECT_ID('dbo.Ghe')
)
BEGIN
    CREATE INDEX IX_Ghe_MaPhienGiu
        ON dbo.Ghe(MaPhienGiu)
        WHERE MaPhienGiu IS NOT NULL;
END
GO

-- 2. Đảm bảo cột MauSac tồn tại và làm sạch các giá trị không đúng chuẩn #RRGGBB
IF COL_LENGTH('dbo.KhuVuc', 'MauSac') IS NULL
BEGIN
    ALTER TABLE dbo.KhuVuc ADD MauSac VARCHAR(20) NULL;
END
GO

UPDATE dbo.KhuVuc
SET MauSac = NULL
WHERE MauSac IS NOT NULL
  AND MauSac NOT LIKE '#[0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f]';
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_KhuVuc_MauSac')
BEGIN
    ALTER TABLE dbo.KhuVuc WITH CHECK
        ADD CONSTRAINT CK_KhuVuc_MauSac CHECK (
            MauSac IS NULL
            OR MauSac LIKE '#[0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f]'
        );
END
GO

-- 3. Tự động ánh xạ MaVung cho các khu vực cũ có tên nhận diện rõ ràng và không xung đột
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
    WHERE TenKV IN (
        N'Khu VIP Khán Đài Trực Diện', N'[DEMO] Khu VIP',
        N'Khu Thường Tiêu Chuẩn', N'[DEMO] Khu tiêu chuẩn'
    )
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
UPDATE kv
SET MaVung = c.MaVung
FROM dbo.KhuVuc kv
JOIN Candidates c ON c.MaKV = kv.MaKV
WHERE c.NameMatches = 1
  AND c.ZoneMatches = 1
  AND NOT EXISTS (
      SELECT 1 FROM dbo.KhuVuc existing
      WHERE existing.MaSuat = c.MaSuat AND existing.MaVung = c.MaVung
  );
GO
