using System;
using System.Data;
using System.Data.SqlClient;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.DAL
{
    /// <summary>
    /// Lớp truy xuất dữ liệu Khách hàng qua ADO.NET (Database.cs).
    /// </summary>
    public class KhachHangDAL
    {
        public static DataTable LayDanhSach()
        {
            string query = "SELECT MaKH, TenKH, SDT, Email FROM KhachHang ORDER BY MaKH DESC";
            return Database.ExecuteQuery(query);
        }

        public static KhachHangDTO LayTheoMa(int maKH)
        {
            string query = "SELECT MaKH, TenKH, SDT, Email FROM KhachHang WHERE MaKH = @MaKH";
            SqlParameter[] parameters = {
                new SqlParameter("@MaKH", SqlDbType.Int) { Value = maKH }
            };

            DataTable dt = Database.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
            {
                return new KhachHangDTO(dt.Rows[0]);
            }
            return null;
        }

        public static DataTable TimKiemTheoSDT(string tienTo)
        {
            if (string.IsNullOrWhiteSpace(tienTo))
            {
                return LayDanhSach();
            }

            string query = "SELECT MaKH, TenKH, SDT, Email FROM KhachHang WHERE SDT LIKE @TienTo + '%' ORDER BY MaKH DESC";
            SqlParameter[] parameters = {
                new SqlParameter("@TienTo", SqlDbType.VarChar, 20) { Value = tienTo.Trim() }
            };

            return Database.ExecuteQuery(query, parameters);
        }

        public static int Them(KhachHangDTO kh)
        {
            string query = "INSERT INTO KhachHang (TenKH, SDT, Email) OUTPUT INSERTED.MaKH VALUES (@TenKH, @SDT, @Email)";
            SqlParameter[] parameters = {
                new SqlParameter("@TenKH", SqlDbType.NVarChar, 100) { Value = kh.TenKH ?? string.Empty },
                new SqlParameter("@SDT", SqlDbType.VarChar, 20) { Value = kh.SDT ?? string.Empty },
                new SqlParameter("@Email", SqlDbType.NVarChar, 100) { Value = string.IsNullOrWhiteSpace(kh.Email) ? (object)DBNull.Value : kh.Email }
            };

            object result = Database.ExecuteScalar(query, parameters);
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        public static int Them(KhachHangDTO kh, SqlConnection conn, SqlTransaction tran)
        {
            string query = "INSERT INTO KhachHang (TenKH, SDT, Email) OUTPUT INSERTED.MaKH VALUES (@TenKH, @SDT, @Email)";
            SqlParameter[] parameters = {
                new SqlParameter("@TenKH", SqlDbType.NVarChar, 100) { Value = kh.TenKH ?? string.Empty },
                new SqlParameter("@SDT", SqlDbType.VarChar, 20) { Value = kh.SDT ?? string.Empty },
                new SqlParameter("@Email", SqlDbType.NVarChar, 100) { Value = string.IsNullOrWhiteSpace(kh.Email) ? (object)DBNull.Value : kh.Email }
            };

            object result = Database.ExecuteScalar(query, conn, tran, parameters);
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        public static int Sua(KhachHangDTO kh)
        {
            string query = "UPDATE KhachHang SET TenKH = @TenKH, SDT = @SDT, Email = @Email WHERE MaKH = @MaKH";
            SqlParameter[] parameters = {
                new SqlParameter("@TenKH", SqlDbType.NVarChar, 100) { Value = kh.TenKH ?? string.Empty },
                new SqlParameter("@SDT", SqlDbType.VarChar, 20) { Value = kh.SDT ?? string.Empty },
                new SqlParameter("@Email", SqlDbType.NVarChar, 100) { Value = string.IsNullOrWhiteSpace(kh.Email) ? (object)DBNull.Value : kh.Email },
                new SqlParameter("@MaKH", SqlDbType.Int) { Value = kh.MaKH }
            };

            return Database.ExecuteNonQuery(query, parameters);
        }

        public static int Xoa(int maKH)
        {
            string query = "DELETE FROM KhachHang WHERE MaKH = @MaKH";
            SqlParameter[] parameters = {
                new SqlParameter("@MaKH", SqlDbType.Int) { Value = maKH }
            };

            return Database.ExecuteNonQuery(query, parameters);
        }

        public static bool KiemTraTrungSDT(string sdt, int excludeMaKH = 0)
        {
            string query = "SELECT COUNT(1) FROM KhachHang WHERE SDT = @SDT AND (@ExcludeMaKH = 0 OR MaKH != @ExcludeMaKH)";
            SqlParameter[] parameters = {
                new SqlParameter("@SDT", SqlDbType.VarChar, 20) { Value = sdt?.Trim() ?? string.Empty },
                new SqlParameter("@ExcludeMaKH", SqlDbType.Int) { Value = excludeMaKH }
            };

            object result = Database.ExecuteScalar(query, parameters);
            return result != null && result != DBNull.Value && Convert.ToInt32(result) > 0;
        }

        public static bool KiemTraKhachHangCoDonVe(int maKH)
        {
            string query = "SELECT COUNT(1) FROM DonVe WHERE MaKH = @MaKH";
            SqlParameter[] parameters = {
                new SqlParameter("@MaKH", SqlDbType.Int) { Value = maKH }
            };

            object result = Database.ExecuteScalar(query, parameters);
            return result != null && result != DBNull.Value && Convert.ToInt32(result) > 0;
        }
    }
}
