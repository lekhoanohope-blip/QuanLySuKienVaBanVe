using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.DAL
{
    public class SuKienDAL
    {
        private static string connStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        public static DataTable LayDanhSach()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM SuKien";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static bool Them(SuKienDTO sk)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "INSERT INTO SuKien (TenSK, LoaiSK, MoTa, TrangThai) VALUES (@TenSK, @LoaiSK, @MoTa, @TrangThai)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenSK", sk.TenSK);
                cmd.Parameters.AddWithValue("@LoaiSK", sk.LoaiSK);
                cmd.Parameters.AddWithValue("@MoTa", sk.MoTa);
                cmd.Parameters.AddWithValue("@TrangThai", sk.TrangThai);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static bool Sua(SuKienDTO sk)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "UPDATE SuKien SET TenSK = @TenSK, LoaiSK = @LoaiSK, MoTa = @MoTa, TrangThai = @TrangThai WHERE MaSK = @MaSK";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenSK", sk.TenSK);
                cmd.Parameters.AddWithValue("@LoaiSK", sk.LoaiSK);
                cmd.Parameters.AddWithValue("@MoTa", sk.MoTa);
                cmd.Parameters.AddWithValue("@TrangThai", sk.TrangThai);
                cmd.Parameters.AddWithValue("@MaSK", sk.MaSK);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static bool Xoa(int maSK)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "DELETE FROM SuKien WHERE MaSK = @MaSK";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSK", maSK);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static int KiemTraCoSuatDien(int maSK)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT COUNT(*) FROM SuatDien WHERE MaSK = @MaSK";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSK", maSK);

                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
    }
}