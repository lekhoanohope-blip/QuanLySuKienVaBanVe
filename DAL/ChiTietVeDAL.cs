using System;
using System.Data;
using System.Data.SqlClient;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.DAL
{
    /// <summary>
    /// Lớp truy xuất dữ liệu Chi tiết vé qua ADO.NET (Database.cs).
    /// </summary>
    public class ChiTietVeDAL
    {
        public static DataTable LayTheoMaDon(int maDon)
        {
            string query = @"
                SELECT ct.MaCT, ct.MaDon, ct.MaGhe, g.MaGheHienThi, g.Hang, g.Cot,
                       kv.MaKV, kv.TenKV, kv.HangVe, ct.GiaVe, ct.MaVeCode, ct.TrangThai,
                       g.TrangThai AS TrangThaiGhe
                FROM ChiTietVe ct
                INNER JOIN Ghe g ON ct.MaGhe = g.MaGhe
                INNER JOIN KhuVuc kv ON g.MaKV = kv.MaKV
                WHERE ct.MaDon = @MaDon
                ORDER BY kv.TenKV ASC, g.Hang ASC, g.Cot ASC;";

            SqlParameter[] parameters = {
                new SqlParameter("@MaDon", SqlDbType.Int) { Value = maDon }
            };

            return Database.ExecuteQuery(query, parameters);
        }

        public static ChiTietVeDTO LayChiTietVeTheoMa(int maCT, SqlConnection conn = null, SqlTransaction tran = null)
        {
            string query = @"
                SELECT ct.MaCT, ct.MaDon, ct.MaGhe, g.MaGheHienThi, g.Hang, g.Cot,
                       kv.MaKV, kv.TenKV, kv.HangVe, ct.GiaVe, ct.MaVeCode, ct.TrangThai
                FROM ChiTietVe ct
                INNER JOIN Ghe g ON ct.MaGhe = g.MaGhe
                INNER JOIN KhuVuc kv ON g.MaKV = kv.MaKV
                WHERE ct.MaCT = @MaCT;";

            SqlParameter[] parameters = {
                new SqlParameter("@MaCT", SqlDbType.Int) { Value = maCT }
            };

            DataTable dt = (conn != null && tran != null)
                ? Database.ExecuteQuery(query, conn, tran, parameters)
                : Database.ExecuteQuery(query, parameters);

            return dt.Rows.Count > 0 ? new ChiTietVeDTO(dt.Rows[0]) : null;
        }

        public static int Them(ChiTietVeDTO ct)
        {
            string query = @"
                INSERT INTO ChiTietVe (MaDon, MaGhe, GiaVe, MaVeCode, TrangThai)
                OUTPUT INSERTED.MaCT
                VALUES (@MaDon, @MaGhe, @GiaVe, @MaVeCode, @TrangThai);";

            SqlParameter[] parameters = {
                new SqlParameter("@MaDon", SqlDbType.Int) { Value = ct.MaDon },
                new SqlParameter("@MaGhe", SqlDbType.Int) { Value = ct.MaGhe },
                new SqlParameter("@GiaVe", SqlDbType.Decimal) { Value = ct.GiaVe },
                new SqlParameter("@MaVeCode", SqlDbType.VarChar, 50) { Value = ct.MaVeCode ?? string.Empty },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar, 20) { Value = ct.TrangThai ?? TrangThaiChiTietVe.HopLe }
            };

            object result = Database.ExecuteScalar(query, parameters);
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        public static int Them(ChiTietVeDTO ct, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
                INSERT INTO ChiTietVe (MaDon, MaGhe, GiaVe, MaVeCode, TrangThai)
                OUTPUT INSERTED.MaCT
                VALUES (@MaDon, @MaGhe, @GiaVe, @MaVeCode, @TrangThai);";

            SqlParameter[] parameters = {
                new SqlParameter("@MaDon", SqlDbType.Int) { Value = ct.MaDon },
                new SqlParameter("@MaGhe", SqlDbType.Int) { Value = ct.MaGhe },
                new SqlParameter("@GiaVe", SqlDbType.Decimal) { Value = ct.GiaVe },
                new SqlParameter("@MaVeCode", SqlDbType.VarChar, 50) { Value = ct.MaVeCode ?? string.Empty },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar, 20) { Value = ct.TrangThai ?? TrangThaiChiTietVe.HopLe }
            };

            object result = Database.ExecuteScalar(query, conn, tran, parameters);
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        public static int CapNhatTrangThaiTheoDon(int maDon, string trangThai, SqlConnection conn, SqlTransaction tran)
        {
            string query = "UPDATE ChiTietVe SET TrangThai = @TrangThai WHERE MaDon = @MaDon;";
            SqlParameter[] parameters = {
                new SqlParameter("@TrangThai", SqlDbType.NVarChar, 20) { Value = trangThai },
                new SqlParameter("@MaDon", SqlDbType.Int) { Value = maDon }
            };
            return Database.ExecuteNonQuery(query, conn, tran, parameters);
        }

        public static int CapNhatTrangThaiTheoMaCT(int maCT, string trangThai, SqlConnection conn, SqlTransaction tran)
        {
            string query = "UPDATE ChiTietVe SET TrangThai = @TrangThai WHERE MaCT = @MaCT;";
            SqlParameter[] parameters = {
                new SqlParameter("@TrangThai", SqlDbType.NVarChar, 20) { Value = trangThai },
                new SqlParameter("@MaCT", SqlDbType.Int) { Value = maCT }
            };
            return Database.ExecuteNonQuery(query, conn, tran, parameters);
        }

        public static int DemSoVeHopLeTheoDon(int maDon, SqlConnection conn, SqlTransaction tran)
        {
            string query = "SELECT COUNT(1) FROM ChiTietVe WHERE MaDon = @MaDon AND TrangThai = N'HopLe';";
            SqlParameter[] parameters = {
                new SqlParameter("@MaDon", SqlDbType.Int) { Value = maDon }
            };
            object result = Database.ExecuteScalar(query, conn, tran, parameters);
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        public static decimal TinhTongTienVeHopLe(int maDon, SqlConnection conn, SqlTransaction tran)
        {
            string query = "SELECT ISNULL(SUM(GiaVe), 0) FROM ChiTietVe WHERE MaDon = @MaDon AND TrangThai = N'HopLe';";
            SqlParameter[] parameters = {
                new SqlParameter("@MaDon", SqlDbType.Int) { Value = maDon }
            };
            object result = Database.ExecuteScalar(query, conn, tran, parameters);
            return result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0m;
        }

        public static bool KiemTraMaVeCodeTonTai(string maVeCode, SqlConnection conn = null, SqlTransaction tran = null)
        {
            string query = "SELECT COUNT(1) FROM ChiTietVe WHERE MaVeCode = @MaVeCode;";
            SqlParameter[] parameters = {
                new SqlParameter("@MaVeCode", SqlDbType.VarChar, 50) { Value = maVeCode ?? string.Empty }
            };
            object result = (conn != null && tran != null)
                ? Database.ExecuteScalar(query, conn, tran, parameters)
                : Database.ExecuteScalar(query, parameters);
            return result != null && result != DBNull.Value && Convert.ToInt32(result) > 0;
        }
    }
}
