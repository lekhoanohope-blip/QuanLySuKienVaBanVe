using System.Data;
using System.Data.SqlClient;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.DAL
{
    /// <summary>
    /// Truy cập bảng TaiKhoan. Chỉ chứa SQL, KHÔNG chứa quy tắc nghiệp vụ.
    /// Dùng SqlParameter cho mọi giá trị người dùng nhập để chống SQL Injection.
    /// Chỉ phụ thuộc Database.GetConnection() của TV3 (trả về SqlConnection CHƯA mở).
    /// </summary>
    public static class TaiKhoanDAL
    {
        public static TaiKhoanDTO LayTheoTenDangNhap(string tenDangNhap)
        {
            const string sql =
                "SELECT MaTK, TenDangNhap, MatKhau, VaiTro, TrangThai " +
                "FROM TaiKhoan WHERE TenDangNhap = @ten";

            using (SqlConnection conn = Database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@ten", SqlDbType.NVarChar, 50).Value = tenDangNhap;
                conn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    if (!rd.Read()) return null;
                    return new TaiKhoanDTO
                    {
                        MaTK = rd.GetInt32(0),
                        TenDangNhap = rd.GetString(1),
                        MatKhau = rd.GetString(2),
                        VaiTro = rd.GetString(3),
                        TrangThai = rd.IsDBNull(4) || rd.GetBoolean(4)
                    };
                }
            }
        }

        public static bool TonTai(string tenDangNhap)
        {
            const string sql = "SELECT COUNT(1) FROM TaiKhoan WHERE TenDangNhap = @ten";
            using (SqlConnection conn = Database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@ten", SqlDbType.NVarChar, 50).Value = tenDangNhap;
                conn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public static bool Them(TaiKhoanDTO tk)
        {
            const string sql =
                "INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro, TrangThai) " +
                "VALUES (@ten, @mk, @vt, @tt)";

            using (SqlConnection conn = Database.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@ten", SqlDbType.NVarChar, 50).Value = tk.TenDangNhap;
                cmd.Parameters.Add("@mk", SqlDbType.NVarChar, 255).Value = tk.MatKhau;
                cmd.Parameters.Add("@vt", SqlDbType.NVarChar, 30).Value = tk.VaiTro;
                cmd.Parameters.Add("@tt", SqlDbType.Bit).Value = tk.TrangThai;
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
