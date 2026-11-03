using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLySuKienVaBanVe.DAL
{
    /// <summary>
    /// Truy vấn báo cáo / thống kê. Mọi câu truy vấn đều dùng tham số, không nối chuỗi.
    ///
    /// QUY ƯỚC MẶC ĐỊNH (nhóm có thể đổi, xem README_BaoCao.txt):
    /// - Doanh thu = SUM(ThanhToan.SoTien) của các đơn có TrangThai IN ('DaThanhToan', 'DaDoi'),
    ///   lọc theo ThanhToan.NgayThanhToan. Tiền hoàn (SoTien âm) tự trừ vào tổng.
    ///   Đơn 'DaDoi' phải tính vì sau khi đổi vé, tiền gốc vẫn nằm ở đơn cũ (đơn mới
    ///   chỉ ghi khoản chênh lệch; đổi ngang giá thì đơn mới không có dòng thanh toán).
    /// - Vé đã bán = ChiTietVe có TrangThai = 'HopLe' thuộc đơn 'DaThanhToan',
    ///   lọc theo DonVe.NgayDat.
    /// - Tỷ lệ lấp đầy = số ghế DaBan / tổng số ghế thật trong bảng Ghe
    ///   (NULLIF chống chia 0; cột KhuVuc.TongSoGhe đã có CHECK &gt; 0).
    /// - maSK = 0 nghĩa là tất cả sự kiện, maSuat = 0 nghĩa là tất cả suất diễn.
    /// </summary>
    public static class BaoCaoDAL
    {
        private static SqlParameter[] ThamSoLoc(int maSK, int maSuat, DateTime tuNgay, DateTime denNgay)
        {
            // Lọc đến hết ngày kết thúc: NgayThanhToan/NgayDat < 00:00 ngày hôm sau.
            DateTime denSau = denNgay.Date.AddDays(1);
            return new[]
            {
                new SqlParameter("@maSK", SqlDbType.Int) { Value = maSK },
                new SqlParameter("@maSuat", SqlDbType.Int) { Value = maSuat },
                new SqlParameter("@tu", SqlDbType.DateTime) { Value = tuNgay.Date },
                new SqlParameter("@denSau", SqlDbType.DateTime) { Value = denSau }
            };
        }

        /// <summary>Doanh thu + số vé theo từng sự kiện (cho biểu đồ cột/tròn cũ, vẫn giữ để dùng khi cần).</summary>
        public static DataTable DoanhThuTheoSuKien(int maSK, int maSuat, DateTime tuNgay, DateTime denNgay)
        {
            // Gom tiền và vé theo MaSuat trước (CTE) để tránh nhân tổng khi JOIN 1-n.
            const string sql = @"
WITH Pay AS (
    SELECT dv.MaSuat, SUM(tt.SoTien) AS Tien
    FROM DonVe dv
    INNER JOIN ThanhToan tt ON tt.MaDon = dv.MaDon
    WHERE dv.TrangThai IN (N'DaThanhToan', N'DaDoi')
      AND tt.NgayThanhToan >= @tu AND tt.NgayThanhToan < @denSau
    GROUP BY dv.MaSuat
),
Tick AS (
    SELECT dv.MaSuat, COUNT(ct.MaCT) AS SoVe, COUNT(DISTINCT dv.MaDon) AS SoDon
    FROM DonVe dv
    INNER JOIN ChiTietVe ct ON ct.MaDon = dv.MaDon
    WHERE dv.TrangThai = N'DaThanhToan' AND ct.TrangThai = N'HopLe'
      AND dv.NgayDat >= @tu AND dv.NgayDat < @denSau
    GROUP BY dv.MaSuat
)
SELECT sk.MaSK, sk.TenSK,
       COALESCE(SUM(t.SoDon), 0) AS SoDon,
       COALESCE(SUM(t.SoVe), 0) AS SoVe,
       COALESCE(SUM(p.Tien), 0) AS DoanhThu
FROM SuKien sk
LEFT JOIN SuatDien sd ON sd.MaSK = sk.MaSK
LEFT JOIN Pay p ON p.MaSuat = sd.MaSuat
LEFT JOIN Tick t ON t.MaSuat = sd.MaSuat
WHERE (@maSK = 0 OR sk.MaSK = @maSK)
  AND (@maSuat = 0 OR sd.MaSuat = @maSuat)
GROUP BY sk.MaSK, sk.TenSK
ORDER BY DoanhThu DESC";
            return Database.ExecuteQuery(sql, ThamSoLoc(maSK, maSuat, tuNgay, denNgay));
        }

        /// <summary>
        /// Doanh thu theo loại vé / khu vực (cho biểu đồ tròn).
        /// Dùng SUM(ChiTietVe.GiaVe) = giá mặt vé, tránh nhân tổng tiền thanh toán
        /// khi một đơn có nhiều vé. Với dữ liệu demo (không hoàn từng phần) thì
        /// tổng này bằng tổng tiền thanh toán.
        /// </summary>
        public static DataTable DoanhThuTheoLoaiVe(int maSK, int maSuat, DateTime tuNgay, DateTime denNgay)
        {
            const string sql = @"
SELECT kv.HangVe,
       COUNT(ct.MaCT) AS SoVe,
       COALESCE(SUM(ct.GiaVe), 0) AS DoanhThu
FROM KhuVuc kv
INNER JOIN Ghe g ON g.MaKV = kv.MaKV
INNER JOIN ChiTietVe ct ON ct.MaGhe = g.MaGhe AND ct.TrangThai = N'HopLe'
INNER JOIN DonVe dv ON dv.MaDon = ct.MaDon AND dv.TrangThai = N'DaThanhToan'
WHERE dv.NgayDat >= @tu AND dv.NgayDat < @denSau
  AND (@maSK = 0 OR kv.MaSuat IN (SELECT MaSuat FROM SuatDien WHERE MaSK = @maSK))
  AND (@maSuat = 0 OR kv.MaSuat = @maSuat)
GROUP BY kv.HangVe
ORDER BY DoanhThu DESC";
            return Database.ExecuteQuery(sql, ThamSoLoc(maSK, maSuat, tuNgay, denNgay));
        }

        /// <summary>
        /// Doanh thu theo thời gian. cheDo: "Ngay" (yyyy-MM-dd), "Thang" (yyyy-MM), "Nam" (yyyy).
        /// Cột Nhan trả về chuỗi để vẽ biểu đồ thẳng, không cần định dạng thêm.
        /// </summary>
        public static DataTable DoanhThuTheoThoiGian(int maSK, int maSuat, DateTime tuNgay, DateTime denNgay, string cheDo)
        {
            string nhom;
            switch ((cheDo ?? "").Trim().ToLowerInvariant())
            {
                case "thang":
                    nhom = "CONVERT(varchar(7), tt.NgayThanhToan, 120)";
                    break;
                case "nam":
                    nhom = "CONVERT(varchar(4), YEAR(tt.NgayThanhToan))";
                    break;
                default:
                    nhom = "CONVERT(varchar(10), tt.NgayThanhToan, 120)";
                    break;
            }

            string sql = @"
SELECT " + nhom + @" AS Nhan,
       COUNT(DISTINCT dv.MaDon) AS SoDon,
       COALESCE(SUM(tt.SoTien), 0) AS DoanhThu
FROM DonVe dv
INNER JOIN ThanhToan tt ON tt.MaDon = dv.MaDon
WHERE dv.TrangThai IN (N'DaThanhToan', N'DaDoi')
  AND tt.NgayThanhToan >= @tu AND tt.NgayThanhToan < @denSau
  AND (@maSuat = 0 OR dv.MaSuat = @maSuat)
  AND (@maSK = 0 OR dv.MaSuat IN (SELECT MaSuat FROM SuatDien WHERE MaSK = @maSK))
GROUP BY " + nhom + @"
ORDER BY Nhan";
            return Database.ExecuteQuery(sql, ThamSoLoc(maSK, maSuat, tuNgay, denNgay));
        }

        /// <summary>
        /// Tỷ lệ lấp đầy ghế theo từng khu vực (cho bảng chi tiết thứ hai).
        /// Chỉ lấy khu vực thuộc suất diễn BẮT ĐẦU trong khoảng lọc, khớp với
        /// KPI và bảng đơn vé trên cùng màn hình (giống TyLeLapDayBinhQuan).
        /// </summary>
        public static DataTable TyLeLapDayTheoKhuVuc(int maSK, int maSuat, DateTime tuNgay, DateTime denNgay)
        {
            const string sql = @"
SELECT kv.TenKV AS [Khu vực], kv.HangVe AS [Hạng vé],
       kv.TongSoGhe AS [Ghế thiết kế],
       COUNT(g.MaGhe) AS [Tổng ghế],
       COALESCE(SUM(CASE WHEN g.TrangThai = N'DaBan' THEN 1 ELSE 0 END), 0) AS [Đã bán],
       COALESCE(SUM(CASE WHEN g.TrangThai = N'DangGiu' THEN 1 ELSE 0 END), 0) AS [Đang giữ],
       COUNT(g.MaGhe)
         - COALESCE(SUM(CASE WHEN g.TrangThai = N'DaBan' THEN 1 ELSE 0 END), 0)
         - COALESCE(SUM(CASE WHEN g.TrangThai = N'DangGiu' THEN 1 ELSE 0 END), 0) AS [Còn trống],
       CAST(100.0 * COALESCE(SUM(CASE WHEN g.TrangThai = N'DaBan' THEN 1 ELSE 0 END), 0)
            / NULLIF(COUNT(g.MaGhe), 0) AS DECIMAL(5, 2)) AS [Tỷ lệ lấp đầy (%)]
FROM KhuVuc kv
INNER JOIN SuatDien sd ON sd.MaSuat = kv.MaSuat
LEFT JOIN Ghe g ON g.MaKV = kv.MaKV
WHERE (@maSK = 0 OR sd.MaSK = @maSK)
  AND (@maSuat = 0 OR kv.MaSuat = @maSuat)
  AND sd.ThoiGianBatDau >= @tu AND sd.ThoiGianBatDau < @denSau
GROUP BY kv.MaKV, kv.TenKV, kv.HangVe, kv.TongSoGhe
ORDER BY kv.TenKV";
            return Database.ExecuteQuery(sql, ThamSoLoc(maSK, maSuat, tuNgay, denNgay));
        }

        /// <summary>
        /// Tỷ lệ lấp đầy bình quân của các khu vực thuộc suất diễn BẮT ĐẦU trong khoảng lọc.
        /// Trả về null khi không có ghế nào (để BLL hiển thị dấu gạch ngang thay vì 0%).
        /// </summary>
        public static decimal? TyLeLapDayBinhQuan(int maSK, int maSuat, DateTime tuNgay, DateTime denNgay)
        {
            const string sql = @"
SELECT CAST(100.0 * SUM(CASE WHEN g.TrangThai = N'DaBan' THEN 1 ELSE 0 END)
     / NULLIF(COUNT(g.MaGhe), 0) AS DECIMAL(5, 2))
FROM KhuVuc kv
INNER JOIN SuatDien sd ON sd.MaSuat = kv.MaSuat
LEFT JOIN Ghe g ON g.MaKV = kv.MaKV
WHERE (@maSK = 0 OR sd.MaSK = @maSK)
  AND (@maSuat = 0 OR kv.MaSuat = @maSuat)
  AND sd.ThoiGianBatDau >= @tu AND sd.ThoiGianBatDau < @denSau";
            object kq = Database.ExecuteScalar(sql, ThamSoLoc(maSK, maSuat, tuNgay, denNgay));
            if (kq == null || kq == DBNull.Value) return null;
            return Convert.ToDecimal(kq);
        }

        /// <summary>Danh sách đơn vé trong khoảng lọc (cho bảng chi tiết).</summary>
        public static DataTable DanhSachDonVe(int maSK, int maSuat, DateTime tuNgay, DateTime denNgay)
        {
            const string sql = @"
SELECT dv.MaDon AS [Mã đơn], dv.NgayDat AS [Ngày đặt],
       kh.TenKH AS [Khách hàng], kh.SDT AS [SĐT],
       sk.TenSK AS [Sự kiện], sd.ThoiGianBatDau AS [Suất diễn],
       (SELECT COUNT(*) FROM ChiTietVe ct WHERE ct.MaDon = dv.MaDon AND (dv.TrangThai <> N'DaThanhToan' OR ct.TrangThai = N'HopLe')) AS [Số vé],
       dv.TongTien AS [Tổng tiền],
       CASE dv.TrangThai
           WHEN N'DaThanhToan' THEN N'Đã thanh toán'
           WHEN N'DaHuy' THEN N'Đã hủy'
           ELSE N'Đã đổi'
       END AS [Trạng thái]
FROM DonVe dv
INNER JOIN KhachHang kh ON kh.MaKH = dv.MaKH
INNER JOIN SuatDien sd ON sd.MaSuat = dv.MaSuat
INNER JOIN SuKien sk ON sk.MaSK = sd.MaSK
WHERE dv.NgayDat >= @tu AND dv.NgayDat < @denSau
  AND (@maSK = 0 OR sk.MaSK = @maSK)
  AND (@maSuat = 0 OR dv.MaSuat = @maSuat)
ORDER BY dv.NgayDat DESC";
            return Database.ExecuteQuery(sql, ThamSoLoc(maSK, maSuat, tuNgay, denNgay));
        }

        /// <summary>Danh sách khách tham dự của một suất diễn (in ấn): một dòng = một vé.</summary>
        public static DataTable DanhSachKhachTheoSuat(int maSuat)
        {
            const string sql = @"
SELECT kh.TenKH, kh.SDT, kh.Email,
       g.MaGheHienThi, kv.TenKV, kv.HangVe, ct.MaVeCode,
       sk.TenSK, sd.ThoiGianBatDau, sd.DiaDiem
FROM ChiTietVe ct
INNER JOIN DonVe dv ON dv.MaDon = ct.MaDon
INNER JOIN KhachHang kh ON kh.MaKH = dv.MaKH
INNER JOIN Ghe g ON g.MaGhe = ct.MaGhe
INNER JOIN KhuVuc kv ON kv.MaKV = g.MaKV
INNER JOIN SuatDien sd ON sd.MaSuat = dv.MaSuat
INNER JOIN SuKien sk ON sk.MaSK = sd.MaSK
WHERE dv.MaSuat = @maSuat
  AND ct.TrangThai = N'HopLe' AND dv.TrangThai = N'DaThanhToan'
ORDER BY kv.TenKV, g.Hang, g.Cot";
            SqlParameter[] prm =
            {
                new SqlParameter("@maSuat", SqlDbType.Int) { Value = maSuat }
            };
            return Database.ExecuteQuery(sql, prm);
        }

        /// <summary>
        /// Lấy 3 chỉ số KPI (Doanh thu, Vé đã bán, Tỷ lệ lấp đầy bình quân)
        /// trong 1 truy vấn duy nhất thay vì gọi 3 lần ExecuteScalar riêng lẻ.
        /// </summary>
        public static void LayChiSoKpi(int maSK, int maSuat, DateTime tuNgay, DateTime denNgay,
            out decimal doanhThu, out int soVe, out decimal? lapDay)
        {
            const string sql = @"
SELECT
   (SELECT COALESCE(SUM(tt.SoTien), 0)
    FROM DonVe dv
    INNER JOIN ThanhToan tt ON tt.MaDon = dv.MaDon
    WHERE dv.TrangThai IN (N'DaThanhToan', N'DaDoi')
      AND tt.NgayThanhToan >= @tu AND tt.NgayThanhToan < @denSau
      AND (@maSuat = 0 OR dv.MaSuat = @maSuat)
      AND (@maSK = 0 OR dv.MaSuat IN (SELECT MaSuat FROM SuatDien WHERE MaSK = @maSK))) AS DoanhThu,
   (SELECT COUNT(*)
    FROM DonVe dv
    INNER JOIN ChiTietVe ct ON ct.MaDon = dv.MaDon
    WHERE dv.TrangThai = N'DaThanhToan' AND ct.TrangThai = N'HopLe'
      AND dv.NgayDat >= @tu AND dv.NgayDat < @denSau
      AND (@maSuat = 0 OR dv.MaSuat = @maSuat)
      AND (@maSK = 0 OR dv.MaSuat IN (SELECT MaSuat FROM SuatDien WHERE MaSK = @maSK))) AS SoVe,
   (SELECT CAST(100.0 * SUM(CASE WHEN g.TrangThai = N'DaBan' THEN 1 ELSE 0 END)
         / NULLIF(COUNT(g.MaGhe), 0) AS DECIMAL(5, 2))
    FROM KhuVuc kv
    INNER JOIN SuatDien sd ON sd.MaSuat = kv.MaSuat
    LEFT JOIN Ghe g ON g.MaKV = kv.MaKV
    WHERE (@maSK = 0 OR sd.MaSK = @maSK)
      AND (@maSuat = 0 OR kv.MaSuat = @maSuat)
      AND sd.ThoiGianBatDau >= @tu AND sd.ThoiGianBatDau < @denSau) AS LapDay";

            DataTable dt = Database.ExecuteQuery(sql, ThamSoLoc(maSK, maSuat, tuNgay, denNgay));
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                doanhThu = r["DoanhThu"] != DBNull.Value ? Convert.ToDecimal(r["DoanhThu"]) : 0m;
                soVe = r["SoVe"] != DBNull.Value ? Convert.ToInt32(r["SoVe"]) : 0;
                lapDay = r["LapDay"] != DBNull.Value ? (decimal?)Convert.ToDecimal(r["LapDay"]) : null;
            }
            else
            {
                doanhThu = 0m;
                soVe = 0;
                lapDay = null;
            }
        }

        /// <summary>Tổng doanh thu (KPI). Dùng ExecuteScalar, NULL thành 0.</summary>
        public static decimal TongDoanhThu(int maSK, int maSuat, DateTime tuNgay, DateTime denNgay)
        {
            const string sql = @"
SELECT COALESCE(SUM(tt.SoTien), 0)
FROM DonVe dv
INNER JOIN ThanhToan tt ON tt.MaDon = dv.MaDon
WHERE dv.TrangThai IN (N'DaThanhToan', N'DaDoi')
  AND tt.NgayThanhToan >= @tu AND tt.NgayThanhToan < @denSau
  AND (@maSuat = 0 OR dv.MaSuat = @maSuat)
  AND (@maSK = 0 OR dv.MaSuat IN (SELECT MaSuat FROM SuatDien WHERE MaSK = @maSK))";
            object kq = Database.ExecuteScalar(sql, ThamSoLoc(maSK, maSuat, tuNgay, denNgay));
            return kq == null || kq == DBNull.Value ? 0m : Convert.ToDecimal(kq);
        }

        /// <summary>Tổng số vé đã bán (KPI).</summary>
        public static int TongVeDaBan(int maSK, int maSuat, DateTime tuNgay, DateTime denNgay)
        {
            const string sql = @"
SELECT COUNT(*)
FROM DonVe dv
INNER JOIN ChiTietVe ct ON ct.MaDon = dv.MaDon
WHERE dv.TrangThai = N'DaThanhToan' AND ct.TrangThai = N'HopLe'
  AND dv.NgayDat >= @tu AND dv.NgayDat < @denSau
  AND (@maSuat = 0 OR dv.MaSuat = @maSuat)
  AND (@maSK = 0 OR dv.MaSuat IN (SELECT MaSuat FROM SuatDien WHERE MaSK = @maSK))";
            object kq = Database.ExecuteScalar(sql, ThamSoLoc(maSK, maSuat, tuNgay, denNgay));
            return kq == null || kq == DBNull.Value ? 0 : Convert.ToInt32(kq);
        }
    }
}
