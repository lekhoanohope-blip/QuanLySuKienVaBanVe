using System;
using System.Data;
using System.Data.SqlClient;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.DAL
{
    /// <summary>
    /// Lớp truy xuất dữ liệu Đơn vé qua ADO.NET (Database.cs).
    /// Hỗ trợ cả truy vấn độc lập và truy vấn trong SqlTransaction.
    /// </summary>
    public class DonVeDAL
    {
        public static DataTable LayDanhSachDonVe()
        {
            string query = @"
                SELECT dv.MaDon, dv.MaDonGoc, dv.MaKH, kh.TenKH, kh.SDT,
                       dv.MaSuat, sd.MaSK, sk.TenSK, sd.ThoiGianBatDau, sd.DiaDiem,
                       dv.MaTK, tk.TenDangNhap AS TenNV,
                       dv.NgayDat, dv.TongTien, dv.TrangThai
                FROM DonVe dv
                INNER JOIN KhachHang kh ON dv.MaKH = kh.MaKH
                INNER JOIN SuatDien sd ON dv.MaSuat = sd.MaSuat
                INNER JOIN SuKien sk ON sd.MaSK = sk.MaSK
                INNER JOIN TaiKhoan tk ON dv.MaTK = tk.MaTK
                ORDER BY dv.NgayDat DESC, dv.MaDon DESC;";
            return Database.ExecuteQuery(query);
        }

        public static DataTable TimKiemDonVe(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
            {
                return LayDanhSachDonVe();
            }

            string clean = tuKhoa.Trim();
            if (clean.StartsWith("#"))
            {
                clean = clean.Substring(1).Trim();
            }

            int maDon = 0;
            int.TryParse(clean, out maDon);

            string query = @"
                SELECT dv.MaDon, dv.MaDonGoc, dv.MaKH, kh.TenKH, kh.SDT,
                       dv.MaSuat, sd.MaSK, sk.TenSK, sd.ThoiGianBatDau, sd.DiaDiem,
                       dv.MaTK, tk.TenDangNhap AS TenNV,
                       dv.NgayDat, dv.TongTien, dv.TrangThai
                FROM DonVe dv
                INNER JOIN KhachHang kh ON dv.MaKH = kh.MaKH
                INNER JOIN SuatDien sd ON dv.MaSuat = sd.MaSuat
                INNER JOIN SuKien sk ON sd.MaSK = sk.MaSK
                INNER JOIN TaiKhoan tk ON dv.MaTK = tk.MaTK
                WHERE (@MaDon > 0 AND dv.MaDon = @MaDon)
                   OR (kh.SDT LIKE @TienTo + '%')
                   OR (sk.TenSK LIKE '%' + @TuKhoa + '%')
                ORDER BY dv.NgayDat DESC, dv.MaDon DESC;";

            SqlParameter[] parameters = {
                new SqlParameter("@MaDon", SqlDbType.Int) { Value = maDon },
                new SqlParameter("@TienTo", SqlDbType.VarChar, 20) { Value = clean },
                new SqlParameter("@TuKhoa", SqlDbType.NVarChar, 200) { Value = clean }
            };

            return Database.ExecuteQuery(query, parameters);
        }

        public static DataRow LayThongTinDonVeVaSuatDien(int maDon)
        {
            string query = @"
                SELECT dv.MaDon, dv.MaDonGoc, dv.MaKH, kh.TenKH, kh.SDT,
                       dv.MaSuat, sd.MaSK, sk.TenSK, sd.ThoiGianBatDau, sd.ThoiGianKetThuc, sd.DiaDiem,
                       sd.TrangThai AS TrangThaiSuat,
                       dv.MaTK, tk.TenDangNhap AS TenNV,
                       dv.NgayDat, dv.TongTien, dv.TrangThai
                FROM DonVe dv
                INNER JOIN KhachHang kh ON dv.MaKH = kh.MaKH
                INNER JOIN SuatDien sd ON dv.MaSuat = sd.MaSuat
                INNER JOIN SuKien sk ON sd.MaSK = sk.MaSK
                INNER JOIN TaiKhoan tk ON dv.MaTK = tk.MaTK
                WHERE dv.MaDon = @MaDon;";

            SqlParameter[] parameters = {
                new SqlParameter("@MaDon", SqlDbType.Int) { Value = maDon }
            };

            DataTable dt = Database.ExecuteQuery(query, parameters);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public static DataRow LayThongTinDonVeVaSuatDien(int maDon, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
                SELECT dv.MaDon, dv.MaDonGoc, dv.MaKH, kh.TenKH, kh.SDT,
                       dv.MaSuat, sd.MaSK, sk.TenSK, sd.ThoiGianBatDau, sd.ThoiGianKetThuc, sd.DiaDiem,
                       sd.TrangThai AS TrangThaiSuat,
                       dv.MaTK, tk.TenDangNhap AS TenNV,
                       dv.NgayDat, dv.TongTien, dv.TrangThai
                FROM DonVe dv WITH (UPDLOCK, ROWLOCK)
                INNER JOIN KhachHang kh ON dv.MaKH = kh.MaKH
                INNER JOIN SuatDien sd ON dv.MaSuat = sd.MaSuat
                INNER JOIN SuKien sk ON sd.MaSK = sk.MaSK
                INNER JOIN TaiKhoan tk ON dv.MaTK = tk.MaTK
                WHERE dv.MaDon = @MaDon;";

            SqlParameter[] parameters = {
                new SqlParameter("@MaDon", SqlDbType.Int) { Value = maDon }
            };

            DataTable dt = Database.ExecuteQuery(query, conn, tran, parameters);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public static DataTable LaySuatDienCungSuKien(int maSK)
        {
            string query = @"
                SELECT sd.MaSuat, sd.MaSK, sk.TenSK, sd.ThoiGianBatDau, sd.ThoiGianKetThuc, sd.DiaDiem, sd.TrangThai
                FROM SuatDien sd
                INNER JOIN SuKien sk ON sd.MaSK = sk.MaSK
                WHERE sd.MaSK = @MaSK
                  AND sd.ThoiGianBatDau > GETDATE()
                ORDER BY sd.ThoiGianBatDau ASC;";

            SqlParameter[] parameters = {
                new SqlParameter("@MaSK", SqlDbType.Int) { Value = maSK }
            };

            return Database.ExecuteQuery(query, parameters);
        }

        public static DataTable LayDanhSachGheTrongTheoSuat(int maSuat)
        {
            string query = @"
                SELECT g.MaGhe, g.MaGheHienThi, g.Hang, g.Cot, g.TrangThai,
                       kv.MaKV, kv.TenKV, kv.HangVe, kv.GiaVe
                FROM Ghe g
                INNER JOIN KhuVuc kv ON g.MaKV = kv.MaKV
                WHERE kv.MaSuat = @MaSuat
                  AND g.TrangThai = N'Trong'
                ORDER BY kv.TenKV ASC, g.Hang ASC, g.Cot ASC;";

            SqlParameter[] parameters = {
                new SqlParameter("@MaSuat", SqlDbType.Int) { Value = maSuat }
            };

            return Database.ExecuteQuery(query, parameters);
        }

        public static DataRow LayThongTinGheKemGia(int maGhe, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
                SELECT g.MaGhe, g.MaGheHienThi, g.Hang, g.Cot, g.TrangThai,
                       kv.MaKV, kv.TenKV, kv.HangVe, kv.GiaVe,
                       sd.MaSuat, sd.MaSK, sd.ThoiGianBatDau, sd.TrangThai AS TrangThaiSuat
                FROM Ghe g WITH (UPDLOCK, ROWLOCK)
                INNER JOIN KhuVuc kv ON g.MaKV = kv.MaKV
                INNER JOIN SuatDien sd ON kv.MaSuat = sd.MaSuat
                WHERE g.MaGhe = @MaGhe;";

            SqlParameter[] parameters = {
                new SqlParameter("@MaGhe", SqlDbType.Int) { Value = maGhe }
            };

            DataTable dt = Database.ExecuteQuery(query, conn, tran, parameters);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public static int Them(DonVeDTO don)
        {
            string query = @"
                INSERT INTO DonVe (MaKH, MaSuat, MaTK, MaDonGoc, NgayDat, TongTien, TrangThai)
                OUTPUT INSERTED.MaDon
                VALUES (@MaKH, @MaSuat, @MaTK, @MaDonGoc, @NgayDat, @TongTien, @TrangThai);";

            SqlParameter[] parameters = {
                new SqlParameter("@MaKH", SqlDbType.Int) { Value = don.MaKH },
                new SqlParameter("@MaSuat", SqlDbType.Int) { Value = don.MaSuat },
                new SqlParameter("@MaTK", SqlDbType.Int) { Value = don.MaTK },
                new SqlParameter("@MaDonGoc", SqlDbType.Int) { Value = (object)don.MaDonGoc ?? DBNull.Value },
                new SqlParameter("@NgayDat", SqlDbType.DateTime) { Value = don.NgayDat },
                new SqlParameter("@TongTien", SqlDbType.Decimal) { Value = don.TongTien },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar, 30) { Value = don.TrangThai ?? TrangThaiDonVe.DaThanhToan }
            };

            object result = Database.ExecuteScalar(query, parameters);
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        public static int Them(DonVeDTO don, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
                INSERT INTO DonVe (MaKH, MaSuat, MaTK, MaDonGoc, NgayDat, TongTien, TrangThai)
                OUTPUT INSERTED.MaDon
                VALUES (@MaKH, @MaSuat, @MaTK, @MaDonGoc, @NgayDat, @TongTien, @TrangThai);";

            SqlParameter[] parameters = {
                new SqlParameter("@MaKH", SqlDbType.Int) { Value = don.MaKH },
                new SqlParameter("@MaSuat", SqlDbType.Int) { Value = don.MaSuat },
                new SqlParameter("@MaTK", SqlDbType.Int) { Value = don.MaTK },
                new SqlParameter("@MaDonGoc", SqlDbType.Int) { Value = (object)don.MaDonGoc ?? DBNull.Value },
                new SqlParameter("@NgayDat", SqlDbType.DateTime) { Value = don.NgayDat },
                new SqlParameter("@TongTien", SqlDbType.Decimal) { Value = don.TongTien },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar, 30) { Value = don.TrangThai ?? TrangThaiDonVe.DaThanhToan }
            };

            object result = Database.ExecuteScalar(query, conn, tran, parameters);
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        public static int CapNhatTrangThai(int maDon, string trangThaiMoi, SqlConnection conn, SqlTransaction tran)
        {
            string query = "UPDATE DonVe SET TrangThai = @TrangThai WHERE MaDon = @MaDon;";
            SqlParameter[] parameters = {
                new SqlParameter("@TrangThai", SqlDbType.NVarChar, 30) { Value = trangThaiMoi },
                new SqlParameter("@MaDon", SqlDbType.Int) { Value = maDon }
            };
            return Database.ExecuteNonQuery(query, conn, tran, parameters);
        }

        public static int CapNhatTongTien(int maDon, decimal tongTienMoi, SqlConnection conn, SqlTransaction tran)
        {
            string query = "UPDATE DonVe SET TongTien = @TongTien WHERE MaDon = @MaDon;";
            SqlParameter[] parameters = {
                new SqlParameter("@TongTien", SqlDbType.Decimal) { Value = tongTienMoi },
                new SqlParameter("@MaDon", SqlDbType.Int) { Value = maDon }
            };
            return Database.ExecuteNonQuery(query, conn, tran, parameters);
        }

        /// <summary>
        /// Cập nhật trạng thái tất cả ghế trong đơn về 'Trong' hoặc trạng thái chỉ định.
        /// TODO [Tích hợp TV1]: Sau này có thể thay bằng GheDAL.CapNhatTrangThai khi TV1 bàn giao GheDAL.
        /// </summary>
        public static int CapNhatTrangThaiGheTheoDon(int maDon, string trangThaiGhe, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
                UPDATE Ghe
                SET TrangThai = @TrangThaiGhe, ThoiGianGiu = NULL
                WHERE MaGhe IN (SELECT MaGhe FROM ChiTietVe WHERE MaDon = @MaDon);";

            SqlParameter[] parameters = {
                new SqlParameter("@TrangThaiGhe", SqlDbType.NVarChar, 30) { Value = trangThaiGhe },
                new SqlParameter("@MaDon", SqlDbType.Int) { Value = maDon }
            };
            return Database.ExecuteNonQuery(query, conn, tran, parameters);
        }

        /// <summary>
        /// Cập nhật trạng thái của 1 ghế đơn lẻ.
        /// TODO [Tích hợp TV1]: Thay bằng GheDAL.CapNhatTrangThai(maGhe, trangThai, conn, tran) khi TV1 bàn giao GheDAL.
        /// </summary>
        public static int CapNhatTrangThaiGhe(int maGhe, string trangThaiGhe, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
                UPDATE Ghe
                SET TrangThai = @TrangThaiGhe, ThoiGianGiu = NULL
                WHERE MaGhe = @MaGhe;";

            SqlParameter[] parameters = {
                new SqlParameter("@TrangThaiGhe", SqlDbType.NVarChar, 30) { Value = trangThaiGhe },
                new SqlParameter("@MaGhe", SqlDbType.Int) { Value = maGhe }
            };
            return Database.ExecuteNonQuery(query, conn, tran, parameters);
        }

        /// <summary>
        /// Ghi nhận lịch sử thao tác hệ thống vào bảng NhatKy.
        /// TODO [Tích hợp TV1]: Có thể chuyển sang NhatKyDAL khi TV1 hoàn thiện.
        /// </summary>
        public static int GhiNhatKy(int maTK, string hanhDong, string chiTiet, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
                INSERT INTO NhatKy (MaTK, HanhDong, ThoiGian, ChiTiet)
                VALUES (@MaTK, @HanhDong, GETDATE(), @ChiTiet);";

            SqlParameter[] parameters = {
                new SqlParameter("@MaTK", SqlDbType.Int) { Value = maTK },
                new SqlParameter("@HanhDong", SqlDbType.NVarChar, 100) { Value = hanhDong ?? string.Empty },
                new SqlParameter("@ChiTiet", SqlDbType.NVarChar, -1) { Value = (object)chiTiet ?? DBNull.Value }
            };
            return Database.ExecuteNonQuery(query, conn, tran, parameters);
        }
    }
}
