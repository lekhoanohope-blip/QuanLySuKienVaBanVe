using System;
using System.Data;
using System.Data.SqlClient;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.DAL
{
    /// <summary>
    /// Lớp truy xuất dữ liệu Giao dịch thanh toán qua ADO.NET (Database.cs).
    /// </summary>
    public class ThanhToanDAL
    {
        public static DataTable LayTheoMaDon(int maDon)
        {
            string query = @"
                SELECT MaTT, MaDon, NgayThanhToan, SoTien, PhuongThuc
                FROM ThanhToan
                WHERE MaDon = @MaDon
                ORDER BY NgayThanhToan DESC;";

            SqlParameter[] parameters = {
                new SqlParameter("@MaDon", SqlDbType.Int) { Value = maDon }
            };

            return Database.ExecuteQuery(query, parameters);
        }

        public static int Them(ThanhToanDTO tt)
        {
            string query = @"
                INSERT INTO ThanhToan (MaDon, NgayThanhToan, SoTien, PhuongThuc)
                OUTPUT INSERTED.MaTT
                VALUES (@MaDon, @NgayThanhToan, @SoTien, @PhuongThuc);";

            SqlParameter[] parameters = {
                new SqlParameter("@MaDon", SqlDbType.Int) { Value = tt.MaDon },
                new SqlParameter("@NgayThanhToan", SqlDbType.DateTime) { Value = tt.NgayThanhToan },
                new SqlParameter("@SoTien", SqlDbType.Decimal) { Value = tt.SoTien },
                new SqlParameter("@PhuongThuc", SqlDbType.NVarChar, 30) { Value = tt.PhuongThuc ?? PhuongThucThanhToan.TienMat }
            };

            object result = Database.ExecuteScalar(query, parameters);
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        public static int Them(ThanhToanDTO tt, SqlConnection conn, SqlTransaction tran)
        {
            string query = @"
                INSERT INTO ThanhToan (MaDon, NgayThanhToan, SoTien, PhuongThuc)
                OUTPUT INSERTED.MaTT
                VALUES (@MaDon, @NgayThanhToan, @SoTien, @PhuongThuc);";

            SqlParameter[] parameters = {
                new SqlParameter("@MaDon", SqlDbType.Int) { Value = tt.MaDon },
                new SqlParameter("@NgayThanhToan", SqlDbType.DateTime) { Value = tt.NgayThanhToan },
                new SqlParameter("@SoTien", SqlDbType.Decimal) { Value = tt.SoTien },
                new SqlParameter("@PhuongThuc", SqlDbType.NVarChar, 30) { Value = tt.PhuongThuc ?? PhuongThucThanhToan.TienMat }
            };

            object result = Database.ExecuteScalar(query, conn, tran, parameters);
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }
    }
}
