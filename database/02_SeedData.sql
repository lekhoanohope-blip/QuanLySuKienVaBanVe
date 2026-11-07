-- ============================================================================
-- ĐỀ TÀI 20: QUẢN LÝ SỰ KIỆN VÀ BÁN VÉ
-- File: 02_SeedData.sql
-- Mục đích: Khởi tạo dữ liệu mẫu đa dạng (Tài khoản, 5 Sự kiện, 11 Suất diễn,
--           Sơ đồ Khu vực chuẩn Ticketbox, Ma trận Ghế, 16 Khách hàng & Đơn vé).
-- Lưu ý: Chạy sau 01_Schema.sql. Script có tính idempotent (chạy lại không trùng).
-- ============================================================================

USE QuanLySuKienVaBanVe;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
-- Các SET option bắt buộc khi thao tác bảng có cột PERSISTED và Filtered Index
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET ARITHABORT ON;
SET NUMERIC_ROUNDABORT OFF;
GO

-- ============================================================================
-- 1. TÀI KHOẢN HỆ THỐNG (Mật khẩu mặc định của tất cả tài khoản mẫu: 123456)
--    SHA-256("123456") = 8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92
-- ============================================================================
DECLARE @DefaultHash NVARCHAR(255) = N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92';

MERGE dbo.TaiKhoan AS target
USING (VALUES
    (N'admin',          @DefaultHash, N'QuanLy',   CAST(1 AS BIT)),
    (N'staff',          @DefaultHash, N'NhanVien', CAST(1 AS BIT)),
    (N'quanly_sukien',  @DefaultHash, N'QuanLy',   CAST(1 AS BIT)),
    (N'nhanvien_quay1', @DefaultHash, N'NhanVien', CAST(1 AS BIT)),
    (N'nhanvien_khoa',  @DefaultHash, N'NhanVien', CAST(0 AS BIT)) -- Tài khoản khóa để kiểm thử phân quyền
) AS src (TenDangNhap, MatKhau, VaiTro, TrangThai)
ON target.TenDangNhap = src.TenDangNhap
WHEN NOT MATCHED THEN
    INSERT (TenDangNhap, MatKhau, VaiTro, TrangThai)
    VALUES (src.TenDangNhap, src.MatKhau, src.VaiTro, src.TrangThai);
GO

-- ============================================================================
-- 2. KHÁCH HÀNG MẪU (16 khách hàng đa dạng khu vực, email sinh viên/cá nhân/vãng lai)
-- ============================================================================
MERGE dbo.KhachHang AS target
USING (VALUES
    (N'Nguyễn Minh Khôi',     '0901234567', N'khoi.nguyen@gmail.com'),
    (N'Trần Ngọc Bảo Trâm',   '0987654321', N'tram.tran@hcmue.edu.vn'),
    (N'Lê Hoàng Nam',         '0912345678', N'nam.lehoang@outlook.com'),
    (N'Phạm Mai Phương',      '0938112233', N'phuong.pham@gmail.com'),
    (N'Võ Quốc Huy',          '0977889900', N'huy.vo@student.hcmue.edu.vn'),
    (N'Đặng Thùy Linh',       '0909445566', N'linh.dang@company.vn'),
    (N'Hoàng Gia Bảo',        '0389123456', N'giabao.hoang@gmail.com'),
    (N'Bùi Thanh Hà',         '0703556677', N'thanhha.bui@yahoo.com'),
    (N'Đỗ Tuấn Kiệt',         '0868990011', N'kiet.do@techcorp.vn'),
    (N'Ngô Phương Thảo',      '0945667788', N'thao.ngo@hcmue.edu.vn'),
    (N'Dương Nhật Minh',      '0962334455', N'nhatminh.duong@gmail.com'),
    (N'Lý Khánh Vy',          '0933778899', N'khanhvy.ly@gmail.com'),
    (N'Trương Quốc Anh',      '0918223344', NULL), -- Khách mua trực tiếp tại quầy không để lại email
    (N'Phan Thanh Tùng',      '0971554433', N'tung.phan@icloud.com'),
    (N'Huỳnh Ngọc Ánh',       '0898112244', N'ngocanh.huynh@gmail.com'),
    (N'Mai Xuân Bách',        '0356778899', NULL)
) AS src (TenKH, SDT, Email)
ON target.SDT = src.SDT
WHEN NOT MATCHED THEN
    INSERT (TenKH, SDT, Email)
    VALUES (src.TenKH, src.SDT, src.Email);
GO

-- ============================================================================
-- 3. NĂM SỰ KIỆN MẪU ĐA DẠNG THỂ LOẠI VÀ TRẠNG THÁI
-- ============================================================================
MERGE dbo.SuKien AS target
USING (VALUES
    (
        N'Live Concert: Vũ Trụ Cò Bay - Phương Mỹ Chi',
        N'Ca nhạc',
        N'Đêm nhạc giao thoa âm hưởng dân gian đương đại và pop/rock hiện đại cùng hệ thống sân khấu chữ T hoành tráng.',
        N'SapDien'
    ),
    (
        N'Anh Trai Say Hi Concert 2026 - Đêm Sài Gòn',
        N'Đại nhạc hội',
        N'Đại nhạc hội quy tụ 30 nghệ sĩ trẻ hàng đầu với sân khấu Arena ngoài trời, pháo hoa và hiệu ứng ánh sáng 3D.',
        N'SapDien'
    ),
    (
        N'Hòa Nhạc Giao Hưởng: Ghibli & Anime Symphony',
        N'Hòa nhạc',
        N'Đêm nhạc thính phòng trình diễn trực tiếp các bản nhạc phim bất hủ của Joe Hisaishi với dàn nhạc giao hưởng 70 nghệ sĩ.',
        N'DangDien'
    ),
    (
        N'Vở Kịch: Ngày Xửa Ngày Xưa 36 - Hậu Duệ Mặt Trời',
        N'Sân khấu',
        N'Vở nhạc kịch thiếu nhi và gia đình đặc sắc với phục trang lộng lẫy, âm nhạc vui nhộn và thông điệp nhân văn.',
        N'SapDien'
    ),
    (
        N'Diễn đàn Công nghệ: Vietnam AI & Cloud Summit 2026',
        N'Hội thảo',
        N'Sự kiện công nghệ thường niên chia sẻ kiến trúc hệ thống lớn, Generative AI và Cloud Native từ các chuyên gia.',
        N'DaKetThuc'
    )
) AS src (TenSK, LoaiSK, MoTa, TrangThai)
ON target.TenSK = src.TenSK
WHEN NOT MATCHED THEN
    INSERT (TenSK, LoaiSK, MoTa, TrangThai)
    VALUES (src.TenSK, src.LoaiSK, src.MoTa, src.TrangThai);
GO

-- ============================================================================
-- 4. SUẤT DIỄN, KHU VỰC (CHUẨN SEATMAP CONCERT TICKETBOX) & MA TRẬN GHẾ
--    Quy ước Ticketbox: Các vùng đối xứng trái/phải (VD: VIP_A & VIP_B, GA_A & GA_B)
--    có cùng Hạng vé, cùng Mức giá và cùng Mã màu hiển thị trên sơ đồ.
-- ============================================================================
BEGIN TRANSACTION;

DECLARE @BaseDate DATETIME = CAST(CAST(GETDATE() AS DATE) AS DATETIME);
DECLARE @MaSK1 INT = (SELECT TOP 1 MaSK FROM dbo.SuKien WHERE TenSK = N'Live Concert: Vũ Trụ Cò Bay - Phương Mỹ Chi');
DECLARE @MaSK2 INT = (SELECT TOP 1 MaSK FROM dbo.SuKien WHERE TenSK = N'Anh Trai Say Hi Concert 2026 - Đêm Sài Gòn');
DECLARE @MaSK3 INT = (SELECT TOP 1 MaSK FROM dbo.SuKien WHERE TenSK = N'Hòa Nhạc Giao Hưởng: Ghibli & Anime Symphony');
DECLARE @MaSK4 INT = (SELECT TOP 1 MaSK FROM dbo.SuKien WHERE TenSK = N'Vở Kịch: Ngày Xửa Ngày Xưa 36 - Hậu Duệ Mặt Trời');
DECLARE @MaSK5 INT = (SELECT TOP 1 MaSK FROM dbo.SuKien WHERE TenSK = N'Diễn đàn Công nghệ: Vietnam AI & Cloud Summit 2026');

-- ----------------------------------------------------------------------------
-- 4.1. Khởi tạo 11 Suất diễn phủ đủ 3 loại sơ đồ (1: Chữ T, 2: Arena, 3: Nhà hát)
-- ----------------------------------------------------------------------------
IF @MaSK1 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.SuatDien WHERE MaSK = @MaSK1)
BEGIN
    INSERT INTO dbo.SuatDien (MaSK, ThoiGianBatDau, ThoiGianKetThuc, DiaDiem, LoaiSoDo, TrangThai) VALUES
        -- Đêm 1 & Đêm 2 tại Nhà thi đấu Phú Thọ (Sơ đồ 1: Sân khấu Chữ T Catwalk)
        (@MaSK1, DATEADD(MINUTE, 19*60+30, DATEADD(DAY, 7, @BaseDate)), DATEADD(MINUTE, 22*60+30, DATEADD(DAY, 7, @BaseDate)), N'Nhà thi đấu Phú Thọ (219 Lý Thường Kiệt, Q.11, TP.HCM)', 1, N'MoCuaBanVe'),
        (@MaSK1, DATEADD(MINUTE, 19*60+30, DATEADD(DAY, 8, @BaseDate)), DATEADD(MINUTE, 22*60+30, DATEADD(DAY, 8, @BaseDate)), N'Nhà thi đấu Phú Thọ (219 Lý Thường Kiệt, Q.11, TP.HCM)', 1, N'MoCuaBanVe');
END

IF @MaSK2 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.SuatDien WHERE MaSK = @MaSK2)
BEGIN
    INSERT INTO dbo.SuatDien (MaSK, ThoiGianBatDau, ThoiGianKetThuc, DiaDiem, LoaiSoDo, TrangThai) VALUES
        -- 3 đêm đại nhạc hội (Sơ đồ 2: Hình chữ nhật Arena 12 phân khu)
        (@MaSK2, DATEADD(HOUR, 19, DATEADD(DAY, 14, @BaseDate)), DATEADD(HOUR, 23, DATEADD(DAY, 14, @BaseDate)), N'Khu đô thị Vạn Phúc City (TP. Thủ Đức, TP.HCM)', 2, N'MoCuaBanVe'),
        (@MaSK2, DATEADD(HOUR, 19, DATEADD(DAY, 15, @BaseDate)), DATEADD(HOUR, 23, DATEADD(DAY, 15, @BaseDate)), N'Khu đô thị Vạn Phúc City (TP. Thủ Đức, TP.HCM)', 2, N'MoCuaBanVe'),
        (@MaSK2, DATEADD(HOUR, 19, DATEADD(DAY, 21, @BaseDate)), DATEADD(HOUR, 23, DATEADD(DAY, 21, @BaseDate)), N'Sân vận động Quốc gia Mỹ Đình (Nam Từ Liêm, Hà Nội)', 2, N'DongBanVe');
END

IF @MaSK3 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.SuatDien WHERE MaSK = @MaSK3)
BEGIN
    INSERT INTO dbo.SuatDien (MaSK, ThoiGianBatDau, ThoiGianKetThuc, DiaDiem, LoaiSoDo, TrangThai) VALUES
        -- Hòa nhạc thính phòng (Sơ đồ 3: Lưới khán phòng Nhà hát 16 phân khu)
        (@MaSK3, DATEADD(MINUTE, -30, GETDATE()), DATEADD(MINUTE, 150, GETDATE()), N'Nhà hát Thành phố (07 Công Trường Lam Sơn, Q.1, TP.HCM)', 3, N'DangDien'),
        (@MaSK3, DATEADD(MINUTE, 19*60+30, DATEADD(DAY, 5, @BaseDate)), DATEADD(MINUTE, 22*60, DATEADD(DAY, 5, @BaseDate)), N'Nhà hát Thành phố (07 Công Trường Lam Sơn, Q.1, TP.HCM)', 3, N'MoCuaBanVe');
END

IF @MaSK4 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.SuatDien WHERE MaSK = @MaSK4)
BEGIN
    INSERT INTO dbo.SuatDien (MaSK, ThoiGianBatDau, ThoiGianKetThuc, DiaDiem, LoaiSoDo, TrangThai) VALUES
        -- Suất chiều & suất tối cùng ngày (Sơ đồ 1: Chữ T)
        (@MaSK4, DATEADD(HOUR, 15, DATEADD(DAY, 10, @BaseDate)), DATEADD(HOUR, 18, DATEADD(DAY, 10, @BaseDate)), N'Nhà hát Bến Thành (06 Mạc Đĩnh Chi, Q.1, TP.HCM)', 1, N'MoCuaBanVe'),
        (@MaSK4, DATEADD(HOUR, 20, DATEADD(DAY, 10, @BaseDate)), DATEADD(HOUR, 23, DATEADD(DAY, 10, @BaseDate)), N'Nhà hát Bến Thành (06 Mạc Đĩnh Chi, Q.1, TP.HCM)', 1, N'MoCuaBanVe');
END

IF @MaSK5 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.SuatDien WHERE MaSK = @MaSK5)
BEGIN
    INSERT INTO dbo.SuatDien (MaSK, ThoiGianBatDau, ThoiGianKetThuc, DiaDiem, LoaiSoDo, TrangThai) VALUES
        -- Phiên sáng & phiên chiều đã diễn ra (Sơ đồ 2: Hội trường lớn)
        (@MaSK5, DATEADD(MINUTE, 8*60+30,  DATEADD(DAY, -5, @BaseDate)), DATEADD(HOUR, 12, DATEADD(DAY, -5, @BaseDate)), N'Hội trường A - ĐH Sư phạm TP.HCM (280 An Dương Vương, Q.5)', 2, N'DaKetThuc'),
        (@MaSK5, DATEADD(MINUTE, 13*60+30, DATEADD(DAY, -4, @BaseDate)), DATEADD(MINUTE, 17*60+30, DATEADD(DAY, -4, @BaseDate)), N'Hội trường A - ĐH Sư phạm TP.HCM (280 An Dương Vương, Q.5)', 2, N'DaKetThuc');
END

-- ----------------------------------------------------------------------------
-- 4.2. Thiết lập Khu vực & Hạng vé đối xứng màu/giá theo từng Sự kiện
-- ----------------------------------------------------------------------------

-- [Sự kiện 1] Vũ Trụ Cò Bay (Sơ đồ 1 - Chữ T: VIP_A=VIP_B, GA_A=GA_B, CAT)
INSERT INTO dbo.KhuVuc (MaSuat, MaVung, TenKV, HangVe, GiaVe, TongSoGhe, ChoPhepBanVe, MauSac)
SELECT sd.MaSuat, z.MaVung, z.TenKV, z.HangVe, z.GiaVe, z.TongSoGhe, 1, z.MauSac
FROM dbo.SuatDien sd
CROSS JOIN (VALUES
    ('VIP_A', N'SVIP A - Cò Bay (Cận Catwalk Trái)',   N'VIP',       1200000, 30, '#D97706'),
    ('VIP_B', N'SVIP B - Cò Bay (Cận Catwalk Phải)',   N'VIP',       1200000, 30, '#D97706'),
    ('GA_A',  N'Fanzone GA A - Đứng Cánh Trái',        N'Phổ thông',  650000, 40, '#0284C7'),
    ('GA_B',  N'Fanzone GA B - Đứng Cánh Phải',        N'Phổ thông',  650000, 40, '#0284C7'),
    ('CAT',   N'Khán Đài Thanh Âm (Standard Seating)', N'Thường',     350000, 50, '#059669')
) AS z (MaVung, TenKV, HangVe, GiaVe, TongSoGhe, MauSac)
WHERE sd.MaSK = @MaSK1
  AND NOT EXISTS (SELECT 1 FROM dbo.KhuVuc kv WHERE kv.MaSuat = sd.MaSuat AND kv.MaVung = z.MaVung);

-- [Sự kiện 2] Anh Trai Say Hi Concert (Sơ đồ 2 - Arena 12 phân khu đối xứng)
INSERT INTO dbo.KhuVuc (MaSuat, MaVung, TenKV, HangVe, GiaVe, TongSoGhe, ChoPhepBanVe, MauSac)
SELECT sd.MaSuat, z.MaVung, z.TenKV, z.HangVe, z.GiaVe, z.TongSoGhe,
       CASE WHEN sd.TrangThai = N'DongBanVe' THEN 0 ELSE 1 END, z.MauSac
FROM dbo.SuatDien sd
CROSS JOIN (VALUES
    ('VIP_A', N'Sky Lounge VVIP (Trực Diện Sân Khấu)', N'VIP',       2500000, 30, '#DC2626'),
    ('VIP_B', N'VIP B - Say Hi Zone Trái',             N'VIP',       1800000, 20, '#D97706'),
    ('VIP_C', N'VIP C - Say Hi Zone Phải',             N'VIP',       1800000, 20, '#D97706'),
    ('VIP_D', N'VIP D - Premier Trung Tâm',            N'VIP',       1500000, 30, '#9333EA'),
    ('GA_A',  N'Fanzone GA A (Sát Sân Khấu Trái)',     N'Phổ thông', 1100000, 30, '#0284C7'),
    ('GA_B',  N'Fanzone GA B (Sát Sân Khấu Phải)',     N'Phổ thông', 1100000, 30, '#0284C7'),
    ('GA_C',  N'Standing GA C - Khu Trái',             N'Phổ thông',  850000, 40, '#0D9488'),
    ('GA_D',  N'Standing GA D - Khu Phải',             N'Phổ thông',  850000, 40, '#0D9488'),
    ('GA_E',  N'Standing GA E - Khu Giữa',             N'Phổ thông',  850000, 30, '#0D9488'),
    ('CAT_B', N'Khán Đài Ngồi CAT B (Cánh Trái)',      N'Thường',     550000, 30, '#4F46E5'),
    ('CAT_C', N'Khán Đài Ngồi CAT C (Cánh Phải)',      N'Thường',     550000, 30, '#4F46E5'),
    ('CAT_A', N'Khán Đài Xa CAT A (Phía Sau)',         N'Thường',     400000, 30, '#64748B')
) AS z (MaVung, TenKV, HangVe, GiaVe, TongSoGhe, MauSac)
WHERE sd.MaSK = @MaSK2
  AND NOT EXISTS (SELECT 1 FROM dbo.KhuVuc kv WHERE kv.MaSuat = sd.MaSuat AND kv.MaVung = z.MaVung);

-- [Sự kiện 3] Ghibli & Anime Symphony (Sơ đồ 3 - Khán phòng Nhà hát 16 phân khu)
INSERT INTO dbo.KhuVuc (MaSuat, MaVung, TenKV, HangVe, GiaVe, TongSoGhe, ChoPhepBanVe, MauSac)
SELECT sd.MaSuat, z.MaVung, z.TenKV, z.HangVe, z.GiaVe, z.TongSoGhe, 1, z.MauSac
FROM dbo.SuatDien sd
CROSS JOIN (VALUES
    ('VIP_B', N'Diamond Orchestra B (Trung Tâm)',      N'VIP',       1500000, 20, '#DC2626'),
    ('VIP_C', N'Diamond Orchestra C (Trung Tâm)',      N'VIP',       1500000, 20, '#DC2626'),
    ('VIP_A', N'Gold Orchestra A (Cánh Trái)',         N'VIP',       1200000, 20, '#D97706'),
    ('VIP_D', N'Gold Orchestra D (Cánh Phải)',         N'VIP',       1200000, 20, '#D97706'),
    ('GA_A',  N'Stalls Hạng Nhất - Khu A',             N'Phổ thông',  850000, 20, '#0284C7'),
    ('GA_B',  N'Stalls Hạng Nhất - Khu B',             N'Phổ thông',  850000, 20, '#0284C7'),
    ('GA_C',  N'Stalls Hạng Nhất - Khu C',             N'Phổ thông',  850000, 20, '#0284C7'),
    ('GA_D',  N'Stalls Hạng Nhất - Khu D',             N'Phổ thông',  850000, 20, '#0284C7'),
    ('GA_E',  N'Parterre Trung Tâm - Khu E',           N'Phổ thông',  650000, 20, '#0D9488'),
    ('GA_F',  N'Parterre Trung Tâm - Khu F',           N'Phổ thông',  650000, 20, '#0D9488'),
    ('GA_G',  N'Parterre Trung Tâm - Khu G',           N'Phổ thông',  650000, 20, '#0D9488'),
    ('GA_H',  N'Parterre Trung Tâm - Khu H',           N'Phổ thông',  650000, 20, '#0D9488'),
    ('CAT_C', N'Lô Ban Công Trái (Opera Box L)',       N'Thường',     450000, 20, '#7C3AED'),
    ('CAT_D', N'Lô Ban Công Phải (Opera Box R)',       N'Thường',     450000, 20, '#7C3AED'),
    ('CAT_A', N'Lầu 2 Khán Đài A (Upper Circle)',      N'Thường',     300000, 30, '#475569'),
    ('CAT_B', N'Lầu 2 Khán Đài B (Upper Circle)',      N'Thường',     300000, 30, '#475569')
) AS z (MaVung, TenKV, HangVe, GiaVe, TongSoGhe, MauSac)
WHERE sd.MaSK = @MaSK3
  AND NOT EXISTS (SELECT 1 FROM dbo.KhuVuc kv WHERE kv.MaSuat = sd.MaSuat AND kv.MaVung = z.MaVung);

-- [Sự kiện 4] Kịch Ngày Xửa Ngày Xưa 36 (Sơ đồ 1 - Chữ T)
INSERT INTO dbo.KhuVuc (MaSuat, MaVung, TenKV, HangVe, GiaVe, TongSoGhe, ChoPhepBanVe, MauSac)
SELECT sd.MaSuat, z.MaVung, z.TenKV, z.HangVe, z.GiaVe, z.TongSoGhe, 1, z.MauSac
FROM dbo.SuatDien sd
CROSS JOIN (VALUES
    ('VIP_A', N'Ghế Hoàng Gia VIP A (Trái)',           N'VIP',        700000, 20, '#E11D48'),
    ('VIP_B', N'Ghế Hoàng Gia VIP B (Phải)',           N'VIP',        700000, 20, '#E11D48'),
    ('GA_A',  N'Khán Phòng Gia Đình A (Cánh Trái)',    N'Phổ thông',  450000, 30, '#2563EB'),
    ('GA_B',  N'Khán Phòng Gia Đình B (Cánh Phải)',    N'Phổ thông',  450000, 30, '#2563EB'),
    ('CAT',   N'Ban Công Thiếu Nhi (Lầu 1)',           N'Thường',     250000, 40, '#8B5CF6')
) AS z (MaVung, TenKV, HangVe, GiaVe, TongSoGhe, MauSac)
WHERE sd.MaSK = @MaSK4
  AND NOT EXISTS (SELECT 1 FROM dbo.KhuVuc kv WHERE kv.MaSuat = sd.MaSuat AND kv.MaVung = z.MaVung);

-- [Sự kiện 5] Hội thảo Vietnam AI & Cloud Summit (Sơ đồ 2 - Cấu hình 9 phân khu Hội trường)
INSERT INTO dbo.KhuVuc (MaSuat, MaVung, TenKV, HangVe, GiaVe, TongSoGhe, ChoPhepBanVe, MauSac)
SELECT sd.MaSuat, z.MaVung, z.TenKV, z.HangVe, z.GiaVe, z.TongSoGhe, 0, z.MauSac
FROM dbo.SuatDien sd
CROSS JOIN (VALUES
    ('VIP_A', N'Speaker & Delegate VIP (Hàng Đầu)',    N'VIP',        900000, 20, '#D97706'),
    ('VIP_B', N'Business Pass B (Cánh Trái)',          N'VIP',        700000, 20, '#CA8A04'),
    ('VIP_C', N'Business Pass C (Cánh Phải)',          N'VIP',        700000, 20, '#CA8A04'),
    ('GA_C',  N'Developer Pass - Khu Trái',            N'Phổ thông',  400000, 30, '#0284C7'),
    ('GA_E',  N'Developer Pass - Khu Giữa',            N'Phổ thông',  400000, 30, '#0284C7'),
    ('GA_D',  N'Developer Pass - Khu Phải',            N'Phổ thông',  400000, 30, '#0284C7'),
    ('CAT_B', N'Student Pass - Cánh Trái',             N'Thường',     150000, 30, '#059669'),
    ('CAT_A', N'Student Pass - Trung Tâm',             N'Thường',     150000, 30, '#059669'),
    ('CAT_C', N'Student Pass - Cánh Phải',             N'Thường',     150000, 30, '#059669')
) AS z (MaVung, TenKV, HangVe, GiaVe, TongSoGhe, MauSac)
WHERE sd.MaSK = @MaSK5
  AND NOT EXISTS (SELECT 1 FROM dbo.KhuVuc kv WHERE kv.MaSuat = sd.MaSuat AND kv.MaVung = z.MaVung);

-- ----------------------------------------------------------------------------
-- 4.3. Tự động sinh ma trận ghế (10 ghế/hàng: A1..A10, B1..B10...) cho mọi Khu vực
-- ----------------------------------------------------------------------------
;WITH Numbers AS (
    SELECT TOP (500) (ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1) AS Idx
    FROM sys.all_columns
)
INSERT INTO dbo.Ghe (MaKV, Hang, Cot, TrangThai)
SELECT
    kv.MaKV,
    CASE
        WHEN (n.Idx / 10) < 26 THEN CHAR(65 + (n.Idx / 10))
        ELSE CHAR(65 + ((n.Idx / 10) / 26 - 1)) + CHAR(65 + ((n.Idx / 10) % 26))
    END AS Hang,
    (n.Idx % 10) + 1 AS Cot,
    N'Trong'
FROM dbo.KhuVuc kv
JOIN Numbers n ON n.Idx < kv.TongSoGhe
WHERE NOT EXISTS (SELECT 1 FROM dbo.Ghe g WHERE g.MaKV = kv.MaKV);

COMMIT TRANSACTION;
GO

-- ============================================================================
-- 5. ĐƠN VÉ, CHI TIẾT VÉ & THANH TOÁN MẪU (Trải đều 7 ngày gần nhất)
--    Bao gồm: Đơn hợp lệ (DaThanhToan), Đơn hủy (DaHuy) và Đơn đổi vé (DaDoi -> Đơn mới)
-- ============================================================================
IF NOT EXISTS (
    SELECT 1 FROM dbo.DonVe d
    INNER JOIN dbo.SuatDien sd ON d.MaSuat = sd.MaSuat
    INNER JOIN dbo.SuKien sk ON sd.MaSK = sk.MaSK
    WHERE sk.TenSK = N'Live Concert: Vũ Trụ Cò Bay - Phương Mỹ Chi'
)
BEGIN
    BEGIN TRANSACTION;

    DECLARE @StaffId INT = ISNULL(
        (SELECT TOP 1 MaTK FROM dbo.TaiKhoan WHERE TenDangNhap = N'staff'),
        (SELECT TOP 1 MaTK FROM dbo.TaiKhoan ORDER BY MaTK)
    );
    DECLARE @AdminId INT = ISNULL(
        (SELECT TOP 1 MaTK FROM dbo.TaiKhoan WHERE TenDangNhap = N'admin'),
        @StaffId
    );

    -- Bảng kịch bản các giao dịch mẫu
    DECLARE @SeedOrders TABLE (
        RowNo        INT PRIMARY KEY,
        SDT          VARCHAR(20),
        TenSK        NVARCHAR(200),
        ShowOrder    INT,          -- Suất thứ mấy của sự kiện (1 hoặc 2)
        MaVung       VARCHAR(20),
        Hang         VARCHAR(10),
        TuCot        INT,
        SoLuongGhe   INT,
        DaysAgo      INT,
        PhuongThuc   NVARCHAR(30),
        LoaiGiaoDich VARCHAR(20)   -- 'NORMAL', 'CANCELLED'
    );

    INSERT INTO @SeedOrders VALUES
        (1,  '0901234567', N'Live Concert: Vũ Trụ Cò Bay - Phương Mỹ Chi',           1, 'VIP_A', 'A', 1, 2, 6, N'ChuyenKhoan', 'NORMAL'),
        (2,  '0987654321', N'Live Concert: Vũ Trụ Cò Bay - Phương Mỹ Chi',           1, 'GA_A',  'A', 1, 3, 5, N'TienMat',     'NORMAL'),
        (3,  '0912345678', N'Live Concert: Vũ Trụ Cò Bay - Phương Mỹ Chi',           2, 'VIP_B', 'A', 3, 2, 2, N'TheNganHang', 'NORMAL'),
        (4,  '0938112233', N'Anh Trai Say Hi Concert 2026 - Đêm Sài Gòn',            1, 'VIP_A', 'A', 5, 2, 4, N'ChuyenKhoan', 'NORMAL'),
        (5,  '0977889900', N'Anh Trai Say Hi Concert 2026 - Đêm Sài Gòn',            1, 'VIP_B', 'A', 1, 2, 3, N'ChuyenKhoan', 'NORMAL'),
        (6,  '0909445566', N'Anh Trai Say Hi Concert 2026 - Đêm Sài Gòn',            1, 'GA_C',  'B', 1, 4, 1, N'TienMat',     'NORMAL'),
        (7,  '0389123456', N'Hòa Nhạc Giao Hưởng: Ghibli & Anime Symphony',          1, 'VIP_B', 'A', 4, 2, 3, N'TheNganHang', 'NORMAL'),
        (8,  '0703556677', N'Hòa Nhạc Giao Hưởng: Ghibli & Anime Symphony',          2, 'GA_A',  'A', 1, 2, 0, N'ChuyenKhoan', 'NORMAL'),
        (9,  '0868990011', N'Vở Kịch: Ngày Xửa Ngày Xưa 36 - Hậu Duệ Mặt Trời',      1, 'VIP_A', 'A', 1, 3, 2, N'TienMat',     'NORMAL'),
        (10, '0945667788', N'Diễn đàn Công nghệ: Vietnam AI & Cloud Summit 2026',    1, 'VIP_A', 'A', 1, 2, 6, N'ChuyenKhoan', 'NORMAL'),
        (11, '0962334455', N'Diễn đàn Công nghệ: Vietnam AI & Cloud Summit 2026',    1, 'GA_E',  'A', 1, 3, 5, N'TienMat',     'NORMAL'),
        -- Đơn đã hủy (Ghế vẫn giữ trạng thái 'Trong' để có thể bán lại)
        (12, '0933778899', N'Live Concert: Vũ Trụ Cò Bay - Phương Mỹ Chi',           1, 'CAT',   'A', 1, 2, 1, N'ChuyenKhoan', 'CANCELLED');

    DECLARE @i INT = 1;
    WHILE @i <= 12
    BEGIN
        DECLARE @SDT VARCHAR(20), @TenSK NVARCHAR(200), @ShowOrd INT, @MaVung VARCHAR(20),
                @Hang VARCHAR(10), @TuCot INT, @SL INT, @DaysAgo INT, @PT NVARCHAR(30), @Loai VARCHAR(20);

        SELECT @SDT = SDT, @TenSK = TenSK, @ShowOrd = ShowOrder, @MaVung = MaVung,
               @Hang = Hang, @TuCot = TuCot, @SL = SoLuongGhe, @DaysAgo = DaysAgo,
               @PT = PhuongThuc, @Loai = LoaiGiaoDich
        FROM @SeedOrders WHERE RowNo = @i;

        DECLARE @MaKH INT = (SELECT TOP 1 MaKH FROM dbo.KhachHang WHERE SDT = @SDT);
        DECLARE @MaSK INT = (SELECT TOP 1 MaSK FROM dbo.SuKien WHERE TenSK = @TenSK);
        DECLARE @MaSuat INT = (
            SELECT MaSuat FROM (
                SELECT MaSuat, ROW_NUMBER() OVER (ORDER BY MaSuat) AS rn
                FROM dbo.SuatDien WHERE MaSK = @MaSK
            ) t WHERE t.rn = @ShowOrd
        );
        DECLARE @MaKV INT = (SELECT TOP 1 MaKV FROM dbo.KhuVuc WHERE MaSuat = @MaSuat AND MaVung = @MaVung);
        DECLARE @GiaVe DECIMAL(18,0) = (SELECT GiaVe FROM dbo.KhuVuc WHERE MaKV = @MaKV);

        IF @MaKH IS NOT NULL AND @MaKV IS NOT NULL
        BEGIN
            DECLARE @NgayDat DATETIME = DATEADD(MINUTE, -(@i * 17), DATEADD(DAY, -@DaysAgo, GETDATE()));
            DECLARE @TongTien DECIMAL(18,0) = @GiaVe * @SL;
            DECLARE @TrangThaiDon NVARCHAR(30) = CASE WHEN @Loai = 'CANCELLED' THEN N'DaHuy' ELSE N'DaThanhToan' END;
            DECLARE @TrangThaiVe  NVARCHAR(20) = CASE WHEN @Loai = 'CANCELLED' THEN N'DaHuy' ELSE N'HopLe' END;

            INSERT INTO dbo.DonVe (MaKH, MaSuat, MaTK, NgayDat, TongTien, TrangThai)
            VALUES (@MaKH, @MaSuat, @StaffId, @NgayDat, @TongTien, @TrangThaiDon);
            DECLARE @MaDon INT = CONVERT(INT, SCOPE_IDENTITY());

            INSERT INTO dbo.ChiTietVe (MaDon, MaGhe, GiaVe, MaVeCode, TrangThai)
            SELECT
                @MaDon,
                g.MaGhe,
                @GiaVe,
                'SK' + RIGHT('00' + CAST(@MaSK AS VARCHAR(5)), 2)
                + '-SD' + RIGHT('00' + CAST(@MaSuat AS VARCHAR(5)), 2)
                + '-' + g.MaGheHienThi
                + '-' + RIGHT('0000' + CAST(1000 + (@MaDon * 13) + g.Cot AS VARCHAR(10)), 4),
                @TrangThaiVe
            FROM dbo.Ghe g
            WHERE g.MaKV = @MaKV AND g.Hang = @Hang AND g.Cot BETWEEN @TuCot AND (@TuCot + @SL - 1);

            IF @Loai = 'NORMAL'
            BEGIN
                UPDATE dbo.Ghe
                SET TrangThai = N'DaBan'
                WHERE MaKV = @MaKV AND Hang = @Hang AND Cot BETWEEN @TuCot AND (@TuCot + @SL - 1);

                INSERT INTO dbo.ThanhToan (MaDon, NgayThanhToan, SoTien, PhuongThuc)
                VALUES (@MaDon, DATEADD(MINUTE, 1, @NgayDat), @TongTien, @PT);
            END
        END

        SET @i += 1;
    END

    -- Kịch bản mẫu Đổi vé: Khách hàng Dương Nhật Minh đổi 1 vé Thường (CAT) lên VIP_B trong Vũ Trụ Cò Bay
    DECLARE @KhDoi INT = (SELECT TOP 1 MaKH FROM dbo.KhachHang WHERE SDT = '0962334455');
    DECLARE @SkCoBay INT = (SELECT TOP 1 MaSK FROM dbo.SuKien WHERE TenSK = N'Live Concert: Vũ Trụ Cò Bay - Phương Mỹ Chi');
    DECLARE @SuatCoBay INT = (SELECT TOP 1 MaSuat FROM dbo.SuatDien WHERE MaSK = @SkCoBay ORDER BY MaSuat);
    DECLARE @KvCu INT = (SELECT TOP 1 MaKV FROM dbo.KhuVuc WHERE MaSuat = @SuatCoBay AND MaVung = 'CAT');
    DECLARE @KvMoi INT = (SELECT TOP 1 MaKV FROM dbo.KhuVuc WHERE MaSuat = @SuatCoBay AND MaVung = 'VIP_B');
    DECLARE @GheCu INT = (SELECT TOP 1 MaGhe FROM dbo.Ghe WHERE MaKV = @KvCu AND Hang = 'B' AND Cot = 1 AND TrangThai = N'Trong');
    DECLARE @GheMoi INT = (SELECT TOP 1 MaGhe FROM dbo.Ghe WHERE MaKV = @KvMoi AND Hang = 'B' AND Cot = 1 AND TrangThai = N'Trong');

    IF @KhDoi IS NOT NULL AND @GheCu IS NOT NULL AND @GheMoi IS NOT NULL
    BEGIN
        DECLARE @GiaCu DECIMAL(18,0) = (SELECT GiaVe FROM dbo.KhuVuc WHERE MaKV = @KvCu);
        DECLARE @GiaMoi DECIMAL(18,0) = (SELECT GiaVe FROM dbo.KhuVuc WHERE MaKV = @KvMoi);
        DECLARE @TgGoc DATETIME = DATEADD(DAY, -2, GETDATE());
        DECLARE @TgDoi DATETIME = DATEADD(DAY, -1, GETDATE());

        -- Đơn gốc (Đã chuyển sang trạng thái DaDoi, ghế cũ giải phóng về Trong)
        INSERT INTO dbo.DonVe (MaKH, MaSuat, MaTK, NgayDat, TongTien, TrangThai)
        VALUES (@KhDoi, @SuatCoBay, @StaffId, @TgGoc, @GiaCu, N'DaDoi');
        DECLARE @MaDonCu INT = CONVERT(INT, SCOPE_IDENTITY());

        DECLARE @PrefixCoBay VARCHAR(20) = 'SK' + RIGHT('00' + CAST(@SkCoBay AS VARCHAR(5)), 2)
                                         + '-SD' + RIGHT('00' + CAST(@SuatCoBay AS VARCHAR(5)), 2);

        INSERT INTO dbo.ChiTietVe (MaDon, MaGhe, GiaVe, MaVeCode, TrangThai)
        VALUES (@MaDonCu, @GheCu, @GiaCu, @PrefixCoBay + '-B1-' + RIGHT('0000' + CAST(8000 + @MaDonCu AS VARCHAR(10)), 4), N'DaHuy');

        INSERT INTO dbo.ThanhToan (MaDon, NgayThanhToan, SoTien, PhuongThuc)
        VALUES (@MaDonCu, @TgGoc, @GiaCu, N'TienMat');

        -- Đơn mới sau khi nâng hạng lên VIP_B (Tham chiếu MaDonGoc = @MaDonCu)
        INSERT INTO dbo.DonVe (MaKH, MaSuat, MaTK, MaDonGoc, NgayDat, TongTien, TrangThai)
        VALUES (@KhDoi, @SuatCoBay, @StaffId, @MaDonCu, @TgDoi, @GiaMoi, N'DaThanhToan');
        DECLARE @MaDonMoi INT = CONVERT(INT, SCOPE_IDENTITY());

        INSERT INTO dbo.ChiTietVe (MaDon, MaGhe, GiaVe, MaVeCode, TrangThai)
        VALUES (@MaDonMoi, @GheMoi, @GiaMoi, @PrefixCoBay + '-B1-' + RIGHT('0000' + CAST(8000 + @MaDonMoi AS VARCHAR(10)), 4), N'HopLe');

        UPDATE dbo.Ghe SET TrangThai = N'DaBan' WHERE MaGhe = @GheMoi;

        INSERT INTO dbo.ThanhToan (MaDon, NgayThanhToan, SoTien, PhuongThuc)
        VALUES (@MaDonMoi, @TgDoi, @GiaMoi - @GiaCu, N'ChuyenKhoan');

        INSERT INTO dbo.NhatKy (MaTK, HanhDong, ThoiGian, ChiTiet)
        VALUES (@AdminId, N'Khởi tạo dữ liệu mẫu', GETDATE(), N'Đã nạp thành công bộ dữ liệu chuẩn 5 sự kiện, sơ đồ ghế và giao dịch mẫu.');
    END

    COMMIT TRANSACTION;
END
GO
