using System;
using System.Data;
using System.Data.SqlClient;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.DAL
{
    public class SuatDienDAL
    {
        private static string connectionString;

        public static DataTable LayDanhSach()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"SELECT sd.MaSuat, sd.MaSK, sk.TenSK, sd.ThoiGianBatDau, sd.ThoiGianKetThuc, sd.DiaDiem, sd.TrangThai 
                                 FROM SuatDien sd 
                                 INNER JOIN SuKien sk ON sd.MaSK = sk.MaSK
                                 ORDER BY sd.ThoiGianBatDau DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static SuatDienDTO LayTheoMa(int maSuat)
        {
            SuatDienDTO sd = null;
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT MaSuat, MaSK, ThoiGianBatDau, ThoiGianKetThuc, DiaDiem, TrangThai, LoaiSoDo FROM SuatDien WHERE MaSuat = @MaSuat";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSuat", maSuat);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        sd = new SuatDienDTO
                        {
                            MaSuat = Convert.ToInt32(reader["MaSuat"]),
                            MaSK = Convert.ToInt32(reader["MaSK"]),
                            ThoiGianBatDau = Convert.ToDateTime(reader["ThoiGianBatDau"]),
                            ThoiGianKetThuc = reader["ThoiGianKetThuc"] != DBNull.Value ? Convert.ToDateTime(reader["ThoiGianKetThuc"]) : (DateTime?)null,
                            DiaDiem = reader["DiaDiem"].ToString(),
                            TrangThai = reader["TrangThai"].ToString(),
                            // Kiểm tra DBNull để tránh văng app nếu có dòng dữ liệu cũ chưa có LoaiSoDo
                            LoaiSoDo = reader["LoaiSoDo"] != DBNull.Value ? Convert.ToInt32(reader["LoaiSoDo"]) : 1
                        };
                    }
                }
            }
            return sd;
        }
        public static bool Them(SuatDienDTO sd)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "INSERT INTO SuatDien (MaSK, ThoiGianBatDau, ThoiGianKetThuc, DiaDiem, TrangThai, LoaiSoDo) VALUES (@MaSK, @BatDau, @KetThuc, @DiaDiem, @TrangThai, @LoaiSoDo)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSK", sd.MaSK);
                cmd.Parameters.AddWithValue("@BatDau", sd.ThoiGianBatDau);
                cmd.Parameters.AddWithValue("@KetThuc", (object)sd.ThoiGianKetThuc ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DiaDiem", sd.DiaDiem);
                cmd.Parameters.AddWithValue("@TrangThai", sd.TrangThai);
                cmd.Parameters.AddWithValue("@LoaiSoDo", sd.LoaiSoDo);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static bool Sua(SuatDienDTO sd)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "UPDATE SuatDien SET MaSK = @MaSK, ThoiGianBatDau = @BatDau, ThoiGianKetThuc = @KetThuc, DiaDiem = @DiaDiem, TrangThai = @TrangThai, LoaiSoDo = @LoaiSoDo WHERE MaSuat = @MaSuat";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSK", sd.MaSK);
                cmd.Parameters.AddWithValue("@BatDau", sd.ThoiGianBatDau);
                cmd.Parameters.AddWithValue("@KetThuc", (object)sd.ThoiGianKetThuc ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DiaDiem", sd.DiaDiem);
                cmd.Parameters.AddWithValue("@TrangThai", sd.TrangThai);
                cmd.Parameters.AddWithValue("@LoaiSoDo", sd.LoaiSoDo);
                cmd.Parameters.AddWithValue("@MaSuat", sd.MaSuat);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static bool Xoa(int maSuat)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "DELETE FROM SuatDien WHERE MaSuat = @MaSuat";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSuat", maSuat);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public static int KiemTraCoDonVe(int maSuat)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM DonVe WHERE MaSuat = @MaSuat";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSuat", maSuat);

                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public static int KiemTraCoKhuVuc(int maSuat)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM KhuVuc WHERE MaSuat = @MaSuat";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSuat", maSuat);

                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
    }
}