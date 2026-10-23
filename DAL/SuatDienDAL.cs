using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.DAL
{
    public class SuatDienDAL
    {
        private static string connStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        public static DataTable LayDanhSach()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT sd.MaSuat, sd.MaSK, sk.TenSK, sd.ThoiGianBatDau, sd.ThoiGianKetThuc, sd.DiaDiem, sd.TrangThai 
                                 FROM SuatDien sd INNER JOIN SuKien sk ON sd.MaSK = sk.MaSK";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static bool Them(SuatDienDTO sd)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "INSERT INTO SuatDien (MaSK, ThoiGianBatDau, ThoiGianKetThuc, DiaDiem, TrangThai) VALUES (@MaSK, @BatDau, @KetThuc, @DiaDiem, @TrangThai)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSK", sd.MaSK);
                cmd.Parameters.AddWithValue("@BatDau", sd.ThoiGianBatDau);
                cmd.Parameters.AddWithValue("@KetThuc", (object)sd.ThoiGianKetThuc ?? System.DBNull.Value);
                cmd.Parameters.AddWithValue("@DiaDiem", sd.DiaDiem);
                cmd.Parameters.AddWithValue("@TrangThai", sd.TrangThai);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static bool Xoa(int maSuat)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "DELETE FROM SuatDien WHERE MaSuat = @MaSuat";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSuat", maSuat);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}