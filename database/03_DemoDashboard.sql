-- ============================================================================
-- ĐỀ TÀI 20: QUẢN LÝ SỰ KIỆN VÀ BÁN VÉ
-- File: 03_DemoDashboard.sql
-- Mục đích: Bổ sung bộ dữ liệu mô phỏng quy mô lớn (16 sự kiện, 48 suất diễn,
--           doanh thu 7 ngày liên tiếp) phục vụ kiểm thử tải biểu đồ Báo cáo.
-- Lưu ý: Chạy tùy chọn sau 01_Schema.sql và 02_SeedData.sql. Không ghi đè dữ liệu cũ.
-- ============================================================================

USE QuanLySuKienVaBanVe;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
-- Các SET option bắt buộc khi thao tác bảng có cột PERSISTED và Filtered Index
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET ARITHABORT ON;
SET NUMERIC_ROUNDABORT OFF;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @Marker NVARCHAR(50) = N'[DEMO-DASHBOARD-v1]';
    IF EXISTS (SELECT 1 FROM dbo.SuKien WHERE CHARINDEX(@Marker, MoTa) > 0)
    BEGIN
        PRINT N'Dữ liệu mô phỏng Dashboard đã tồn tại. Bỏ qua để không tạo trùng.';
        COMMIT TRANSACTION;
        RETURN;
    END;

    DECLARE @AccountId INT = (
        SELECT TOP (1) MaTK FROM dbo.TaiKhoan
        WHERE TrangThai = 1 ORDER BY CASE WHEN VaiTro = N'QuanLy' THEN 0 ELSE 1 END, MaTK
    );
    IF @AccountId IS NULL
        THROW 51000, N'Cần tối thiểu 1 tài khoản đang hoạt động. Hãy chạy 02_SeedData.sql trước.', 1;

    DECLARE @CustomerId INT = (SELECT TOP (1) MaKH FROM dbo.KhachHang WHERE SDT = '0900002026');
    IF @CustomerId IS NULL
    BEGIN
        INSERT dbo.KhachHang (TenKH, SDT, Email)
        VALUES (N'Khách hàng Mô phỏng Thống kê', '0900002026', N'dashboard.demo@eventticket.vn');
        SET @CustomerId = CONVERT(INT, SCOPE_IDENTITY());
    END;

    DECLARE @Events TABLE (No INT PRIMARY KEY, Title NVARCHAR(190), Category NVARCHAR(100));
    INSERT @Events (No, Title, Category) VALUES
        (1,  N'Live Concert: Sắc Màu Thanh Xuân',                  N'Ca nhạc'),
        (2,  N'Đêm nhạc Acoustic: Chạm Vào Ký Ức',                 N'Ca nhạc'),
        (3,  N'Kịch nói: Những Ngày Không Quên',                   N'Sân khấu'),
        (4,  N'Hội thảo: AI và Tương lai Giáo dục',                N'Hội thảo'),
        (5,  N'Workshop: Thiết kế UI/UX Thực chiến',               N'Workshop'),
        (6,  N'Đại nhạc hội: HCMUE Student Festival 2026',         N'Đại nhạc hội'),
        (7,  N'Giao lưu: Nghề Lập trình và Hành trình Khởi nghiệp', N'Giao lưu'),
        (8,  N'Triển lãm: Sáng tạo số trong đời sống',             N'Triển lãm'),
        (9,  N'Talkshow: Kỹ năng nghề nghiệp thế hệ mới',          N'Talkshow'),
        (10, N'Giai điệu mùa thu - Live Session',                  N'Ca nhạc'),
        (11, N'Lễ hội Văn hóa Việt Nam - Nhật Bản',                N'Lễ hội'),
        (12, N'Hòa nhạc: Hành trình của những ước mơ',             N'Hòa nhạc'),
        (13, N'Sân khấu cuối tuần: Chuyện Người Trẻ',              N'Sân khấu'),
        (14, N'Ngày hội Khoa học và Công nghệ Trẻ',                N'Triển lãm'),
        (15, N'Đêm hài kịch: Cười Cùng Sinh Viên',                 N'Sân khấu'),
        (16, N'Đêm nhạc ngoài trời - Chương trình tạm dừng',       N'Ca nhạc');

    DECLARE @Now DATETIME = GETDATE();
    DECLARE @EventNo INT = 1, @ShowNo INT, @EventId INT, @ShowId INT;
    DECLARE @EventStatus NVARCHAR(30), @ShowStatus NVARCHAR(30), @Venue NVARCHAR(255);
    DECLARE @Start DATETIME, @End DATETIME, @OrderTime DATETIME;
    DECLARE @VipArea INT, @StandardArea INT, @SeatId INT, @OrderId INT;
    DECLARE @VipPrice DECIMAL(18,0), @StandardPrice DECIMAL(18,0), @Price DECIMAL(18,0);
    DECLARE @SeatNo INT, @SoldSeats INT, @Day INT, @TicketNo INT, @TicketCount INT;
    DECLARE @OrderTotal DECIMAL(18,0);

    WHILE @EventNo <= 16
    BEGIN
        SET @EventStatus = CASE
            WHEN @EventNo <= 9  THEN N'SapDien'
            WHEN @EventNo <= 11 THEN N'DangDien'
            WHEN @EventNo <= 15 THEN N'DaKetThuc'
            ELSE N'DaHuy' END;

        INSERT dbo.SuKien (TenSK, LoaiSK, MoTa, TrangThai)
        SELECT N'[DEMO] ' + Title, Category,
               @Marker + N' Bộ dữ liệu mô phỏng phục vụ kiểm thử biểu đồ doanh thu và tỷ lệ lấp đầy.', @EventStatus
        FROM @Events WHERE No = @EventNo;
        SET @EventId = CONVERT(INT, SCOPE_IDENTITY());

        SET @Venue = CASE @EventNo % 5
            WHEN 0 THEN N'Hội trường A - HCMUE'
            WHEN 1 THEN N'Hội trường B - HCMUE'
            WHEN 2 THEN N'Nhà Văn hóa Sinh viên TP.HCM'
            WHEN 3 THEN N'Trung tâm Hội nghị Riverside Palace'
            ELSE N'Sân khấu ngoài trời - Khu đô thị Thủ Thiêm' END;

        SET @ShowNo = 1;
        WHILE @ShowNo <= 3
        BEGIN
            IF @EventNo <= 9
            BEGIN
                SET @Start = DATEADD(HOUR, @EventNo * 6 + (@ShowNo - 1) * 24, @Now);
                SET @ShowStatus = CASE WHEN @EventNo = 9 THEN N'DongBanVe' ELSE N'MoCuaBanVe' END;
            END
            ELSE IF @EventNo <= 11
            BEGIN
                SET @Start = CASE @ShowNo
                    WHEN 1 THEN DATEADD(DAY, -2, @Now)
                    WHEN 2 THEN DATEADD(MINUTE, -30, @Now)
                    ELSE DATEADD(DAY, 2, @Now) END;
                SET @ShowStatus = CASE @ShowNo
                    WHEN 1 THEN N'DaKetThuc'
                    WHEN 2 THEN N'DangDien'
                    ELSE N'MoCuaBanVe' END;
            END
            ELSE IF @EventNo <= 15
            BEGIN
                SET @Start = CASE WHEN @EventNo = 12 AND @ShowNo = 1
                    THEN DATEADD(HOUR, -4, @Now)
                    ELSE DATEADD(DAY, -(1 + (@EventNo - 12) * 2 + @ShowNo), @Now) END;
                SET @ShowStatus = N'DaKetThuc';
            END
            ELSE
            BEGIN
                SET @Start = DATEADD(DAY, 3 + @ShowNo, @Now);
                SET @ShowStatus = N'DongBanVe';
            END;
            SET @End = DATEADD(MINUTE, 120 + ((@EventNo + @ShowNo) % 3) * 30, @Start);

            INSERT dbo.SuatDien (MaSK, ThoiGianBatDau, ThoiGianKetThuc, DiaDiem, LoaiSoDo, TrangThai)
            VALUES (@EventId, @Start, @End, @Venue, 1, @ShowStatus);
            SET @ShowId = CONVERT(INT, SCOPE_IDENTITY());

            SET @VipPrice = 400000 + (@EventNo % 5) * 100000 + @ShowNo * 50000;
            SET @StandardPrice = @VipPrice / 2;

            INSERT dbo.KhuVuc (MaSuat, MaVung, TenKV, HangVe, GiaVe, TongSoGhe, ChoPhepBanVe, MauSac)
            VALUES (@ShowId, 'VIP_A', N'VIP A - Khán Đài Trực Diện', N'VIP', @VipPrice, 20, 1, '#D97706');
            SET @VipArea = CONVERT(INT, SCOPE_IDENTITY());

            INSERT dbo.KhuVuc (MaSuat, MaVung, TenKV, HangVe, GiaVe, TongSoGhe, ChoPhepBanVe, MauSac)
            VALUES (@ShowId, 'CAT', N'CAT - Khán Đài Tiêu Chuẩn', N'Thường', @StandardPrice, 20, 1, '#059669');
            SET @StandardArea = CONVERT(INT, SCOPE_IDENTITY());

            -- Sinh đúng 20 ghế (2 hàng A, B x 10 cột) cho mỗi khu vực
            SET @SeatNo = 1;
            WHILE @SeatNo <= 10
            BEGIN
                INSERT dbo.Ghe (MaKV, Hang, Cot, TrangThai) VALUES
                    (@VipArea,      'A', @SeatNo, N'Trong'),
                    (@VipArea,      'B', @SeatNo, N'Trong'),
                    (@StandardArea, 'A', @SeatNo, N'Trong'),
                    (@StandardArea, 'B', @SeatNo, N'Trong');
                SET @SeatNo += 1;
            END;

            -- Phân bổ đơn vé theo từng ngày trong 7 ngày gần nhất để biểu đồ doanh thu trực quan
            SET @Day = 0;
            SET @SoldSeats = 0;
            WHILE @Day <= CASE WHEN @EventNo = 16 THEN 0 ELSE 6 END
            BEGIN
                SET @OrderTime = DATEADD(HOUR, -1, DATEADD(DAY, -@Day, @Now));
                IF @OrderTime >= @Start
                    SET @OrderTime = DATEADD(HOUR, -(@Day + 1), @Start);
                SET @TicketCount = CASE WHEN @EventNo = 16 THEN 2
                    ELSE 2 + ((@Day + @EventNo + @ShowNo) % 3) END;

                INSERT dbo.DonVe (MaKH, MaSuat, MaTK, NgayDat, TongTien, TrangThai)
                VALUES (@CustomerId, @ShowId, @AccountId, @OrderTime, 0,
                        CASE WHEN @EventNo = 16 THEN N'DaHuy' ELSE N'DaThanhToan' END);
                SET @OrderId = CONVERT(INT, SCOPE_IDENTITY());

                SET @TicketNo = 1;
                SET @OrderTotal = 0;
                WHILE @TicketNo <= @TicketCount
                BEGIN
                    SET @SoldSeats += 1;
                    DECLARE @TargetArea INT = CASE WHEN @SoldSeats <= 20 THEN @VipArea ELSE @StandardArea END;
                    DECLARE @LocalIdx INT = CASE WHEN @SoldSeats <= 20 THEN @SoldSeats - 1 ELSE @SoldSeats - 21 END;

                    SELECT @SeatId = MaGhe FROM dbo.Ghe
                    WHERE MaKV = @TargetArea
                      AND Hang = CHAR(65 + (@LocalIdx / 10))
                      AND Cot  = (@LocalIdx % 10) + 1;

                    SET @Price = CASE WHEN @SoldSeats <= 20 THEN @VipPrice ELSE @StandardPrice END;

                    INSERT dbo.ChiTietVe (MaDon, MaGhe, GiaVe, MaVeCode, TrangThai)
                    VALUES (@OrderId, @SeatId, @Price,
                            'DEMO-DASH-v1-' + CONVERT(VARCHAR(12), @OrderId) + '-' + CONVERT(VARCHAR(2), @TicketNo),
                            CASE WHEN @EventNo = 16 THEN N'DaHuy' ELSE N'HopLe' END);

                    IF @EventNo <> 16
                        UPDATE dbo.Ghe SET TrangThai = N'DaBan' WHERE MaGhe = @SeatId;

                    SET @OrderTotal += @Price;
                    SET @TicketNo += 1;
                END;

                UPDATE dbo.DonVe SET TongTien = @OrderTotal WHERE MaDon = @OrderId;
                IF @EventNo <> 16
                    INSERT dbo.ThanhToan (MaDon, NgayThanhToan, SoTien, PhuongThuc)
                    VALUES (@OrderId, DATEADD(MINUTE, 2, @OrderTime), @OrderTotal,
                            CASE @Day % 3 WHEN 0 THEN N'TienMat' WHEN 1 THEN N'ChuyenKhoan' ELSE N'TheNganHang' END);
                SET @Day += 1;
            END;

            SET @ShowNo += 1;
        END;
        SET @EventNo += 1;
    END;

    COMMIT TRANSACTION;
    PRINT N'Đã khởi tạo thành công bộ dữ liệu mô phỏng Dashboard.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO
