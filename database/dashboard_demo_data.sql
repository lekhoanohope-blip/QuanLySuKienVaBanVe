-- Du lieu gia de kiem tra dashboard, khong phai giao dich that.
-- Chay sau create_database.sql va sample_data.sql.
-- Them 16 su kien, moi su kien 3 suat dien va moi suat 30 ghe.
-- Co doanh thu 7 ngay, ghe trong/dang giu/da ban va don huy.
-- Ngay gio duoc tinh theo GETDATE(), giong truy van cua dashboard.
-- Chay lai se bo qua neu bo du lieu [DEMO-DASHBOARD-v1] da ton tai.
-- Khong sua hoac xoa du lieu hien co.

USE QuanLySuKienVaBanVe;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
-- Cac SET option bat buoc khi ghi vao bang Ghe co cot tinh toan PERSISTED.
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET ARITHABORT ON;
SET NUMERIC_ROUNDABORT OFF;

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @Marker NVARCHAR(50) = N'[DEMO-DASHBOARD-v1]';
    IF EXISTS (SELECT 1 FROM dbo.SuKien WHERE CHARINDEX(@Marker, MoTa) > 0)
    BEGIN
        PRINT N'Du lieu demo dashboard da ton tai. Khong them trung.';
        COMMIT TRANSACTION;
        RETURN;
    END;

    DECLARE @AccountId INT = (
        SELECT TOP (1) MaTK FROM dbo.TaiKhoan
        WHERE TrangThai = 1 ORDER BY CASE WHEN VaiTro = N'QuanLy' THEN 0 ELSE 1 END, MaTK
    );
    IF @AccountId IS NULL
        THROW 51000, N'Can tai khoan dang hoat dong. Hay chay sample_data.sql truoc.', 1;

    IF EXISTS (SELECT 1 FROM dbo.KhachHang WHERE SDT = '0000002026')
        THROW 51001, N'SDT demo 0000002026 da duoc su dung. Hay chon SDT demo khac trong script.', 1;

    INSERT dbo.KhachHang (TenKH, SDT, Email)
    VALUES (N'[DEMO] Khách kiểm thử Dashboard', '0000002026', N'dashboard-demo@example.invalid');
    DECLARE @CustomerId INT = CONVERT(INT, SCOPE_IDENTITY());

    DECLARE @Events TABLE (No INT PRIMARY KEY, Title NVARCHAR(190), Category NVARCHAR(100));
    INSERT @Events (No, Title, Category) VALUES
        (1, N'Live Concert: Sắc Màu Thanh Xuân', N'Ca nhạc'),
        (2, N'Đêm nhạc Acoustic: Chạm Vào Ký Ức', N'Ca nhạc'),
        (3, N'Kịch nói: Những Ngày Không Quên', N'Sân khấu'),
        (4, N'Hội thảo: AI và Tương lai Giáo dục', N'Hội thảo'),
        (5, N'Workshop: Thiết kế UI/UX cho người mới bắt đầu', N'Workshop'),
        (6, N'Đại nhạc hội: HCMUE Student Festival 2026', N'Ca nhạc'),
        (7, N'Giao lưu: Nghề Lập trình và Hành trình Khởi nghiệp', N'Giao lưu'),
        (8, N'Triển lãm: Sáng tạo số trong đời sống', N'Triển lãm'),
        (9, N'Talkshow: Kỹ năng nghề nghiệp cho sinh viên thế hệ mới', N'Talkshow'),
        (10, N'Giai điệu mùa thu - Live Session', N'Ca nhạc'),
        (11, N'Lễ hội Văn hóa Việt Nam - Nhật Bản', N'Lễ hội'),
        (12, N'Hòa nhạc: Hành trình của những ước mơ', N'Hòa nhạc'),
        (13, N'Sân khấu cuối tuần: Chuyện Người Trẻ', N'Sân khấu'),
        (14, N'Ngày hội Khoa học và Công nghệ', N'Triển lãm'),
        (15, N'Đêm hài: Cười Cùng Sinh Viên', N'Sân khấu'),
        (16, N'Đêm nhạc ngoài trời - Chương trình đã hủy', N'Ca nhạc');

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
            WHEN @EventNo <= 9 THEN N'SapDien'
            WHEN @EventNo <= 11 THEN N'DangDien'
            WHEN @EventNo <= 15 THEN N'DaKetThuc'
            ELSE N'DaHuy' END;

        INSERT dbo.SuKien (TenSK, LoaiSK, MoTa, TrangThai)
        SELECT N'[DEMO] ' + Title, Category,
               @Marker + N' Dữ liệu giả phục vụ kiểm thử giao diện và thống kê.', @EventStatus
        FROM @Events WHERE No = @EventNo;
        SET @EventId = CONVERT(INT, SCOPE_IDENTITY());

        SET @Venue = CASE @EventNo % 5
            WHEN 0 THEN N'Hội trường A - HCMUE, 280 An Dương Vương'
            WHEN 1 THEN N'Hội trường B - HCMUE, 222 Lê Văn Sỹ'
            WHEN 2 THEN N'Nhà Văn hóa Sinh viên TP.HCM'
            WHEN 3 THEN N'Trung tâm Hội nghị Riverside - Phòng lớn'
            ELSE N'Sân khấu ngoài trời - Khuôn viên HCMUE' END;

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
            SET @End = DATEADD(MINUTE, 90 + ((@EventNo + @ShowNo) % 4) * 30, @Start);

            INSERT dbo.SuatDien (MaSK, ThoiGianBatDau, ThoiGianKetThuc, DiaDiem, TrangThai)
            VALUES (@EventId, @Start, @End, @Venue, @ShowStatus);
            SET @ShowId = CONVERT(INT, SCOPE_IDENTITY());

            SET @VipPrice = 300000 + (@EventNo % 5) * 75000 + @ShowNo * 25000;
            SET @StandardPrice = @VipPrice / 2;
            INSERT dbo.KhuVuc (MaSuat, TenKV, HangVe, GiaVe, TongSoGhe)
            VALUES (@ShowId, N'[DEMO] Khu VIP', N'VIP', @VipPrice, 15);
            SET @VipArea = CONVERT(INT, SCOPE_IDENTITY());
            INSERT dbo.KhuVuc (MaSuat, TenKV, HangVe, GiaVe, TongSoGhe)
            VALUES (@ShowId, N'[DEMO] Khu tiêu chuẩn', N'Thuong', @StandardPrice, 15);
            SET @StandardArea = CONVERT(INT, SCOPE_IDENTITY());

            SET @SeatNo = 1;
            WHILE @SeatNo <= 15
            BEGIN
                INSERT dbo.Ghe (MaKV, Hang, Cot, TrangThai)
                VALUES (@VipArea, 'A', @SeatNo, N'Trong'), (@StandardArea, 'B', @SeatNo, N'Trong');
                SET @SeatNo += 1;
            END;

            SET @Day = 0;
            SET @SoldSeats = 0;
            -- Moi ngay mot don, so ve va gia khac nhau de bieu do co nhieu muc doanh thu.
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
                    SELECT @SeatId = MaGhe FROM dbo.Ghe
                    WHERE MaKV = CASE WHEN @SoldSeats <= 15 THEN @VipArea ELSE @StandardArea END
                      AND Cot = CASE WHEN @SoldSeats <= 15 THEN @SoldSeats ELSE @SoldSeats - 15 END;
                    SET @Price = CASE WHEN @SoldSeats <= 15 THEN @VipPrice ELSE @StandardPrice END;

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

            IF @EventNo <> 16 AND @ShowStatus <> N'DaKetThuc'
                UPDATE dbo.Ghe SET TrangThai = N'DangGiu'
                WHERE MaKV = @StandardArea AND Cot BETWEEN @SoldSeats - 15 + 1 AND @SoldSeats - 15 + 3;

            SET @ShowNo += 1;
        END;
        SET @EventNo += 1;
    END;

    COMMIT TRANSACTION;

    SELECT N'DEMO events' AS Dataset, COUNT(*) AS Total
    FROM dbo.SuKien WHERE CHARINDEX(@Marker, MoTa) > 0
    UNION ALL
    SELECT N'DEMO performances', COUNT(*) FROM dbo.SuatDien sd
    JOIN dbo.SuKien sk ON sk.MaSK = sd.MaSK WHERE CHARINDEX(@Marker, sk.MoTa) > 0
    UNION ALL
    SELECT N'DEMO seats', COUNT(*) FROM dbo.Ghe g
    JOIN dbo.KhuVuc kv ON kv.MaKV = g.MaKV JOIN dbo.SuatDien sd ON sd.MaSuat = kv.MaSuat
    JOIN dbo.SuKien sk ON sk.MaSK = sd.MaSK WHERE CHARINDEX(@Marker, sk.MoTa) > 0
    UNION ALL
    SELECT N'DEMO orders', COUNT(*) FROM dbo.DonVe dv
    JOIN dbo.SuatDien sd ON sd.MaSuat = dv.MaSuat JOIN dbo.SuKien sk ON sk.MaSK = sd.MaSK
    WHERE CHARINDEX(@Marker, sk.MoTa) > 0
    UNION ALL
    SELECT N'DEMO tickets', COUNT(*) FROM dbo.ChiTietVe WHERE MaVeCode LIKE 'DEMO-DASH-v1-%';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
