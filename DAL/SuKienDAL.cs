using System;
using System.Data;
using System.Data.SqlClient;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.DAL
{
    public class SuKienDAL
    {
        public static DataTable LayDanhSach()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT MaSK, TenSK, LoaiSK, MoTa, TrangThai FROM SuKien ORDER BY MaSK DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static SuKienDTO LayTheoMa(int maSK)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT MaSK, TenSK, LoaiSK, MoTa, TrangThai FROM SuKien WHERE MaSK = @MaSK";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSK", maSK);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new SuKienDTO
                        {
                            MaSK = Convert.ToInt32(reader["MaSK"]),
                            TenSK = reader["TenSK"]?.ToString(),
                            LoaiSK = reader["LoaiSK"]?.ToString(),
                            MoTa = reader["MoTa"]?.ToString(),
                            TrangThai = reader["TrangThai"]?.ToString()
                        };
                    }
                }
            }
            return null;
        }

        public static bool Them(SuKienDTO sk)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "INSERT INTO SuKien (TenSK, LoaiSK, MoTa, TrangThai) VALUES (@TenSK, @LoaiSK, @MoTa, @TrangThai)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenSK", sk.TenSK);
                cmd.Parameters.AddWithValue("@LoaiSK", (object)sk.LoaiSK ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MoTa", (object)sk.MoTa ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TrangThai", sk.TrangThai);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static bool Sua(SuKienDTO sk)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "UPDATE SuKien SET TenSK = @TenSK, LoaiSK = @LoaiSK, MoTa = @MoTa, TrangThai = @TrangThai WHERE MaSK = @MaSK";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TenSK", sk.TenSK);
                cmd.Parameters.AddWithValue("@LoaiSK", (object)sk.LoaiSK ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MoTa", (object)sk.MoTa ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TrangThai", sk.TrangThai);
                cmd.Parameters.AddWithValue("@MaSK", sk.MaSK);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static bool Xoa(int maSK)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
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
            using (SqlConnection conn = DatabaseHelper.GetConnection())
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