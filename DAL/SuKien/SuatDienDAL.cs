using System;
using System.Data;
using System.Data.SqlClient;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.DAL
{
    /// <summary>Truy xuất dữ liệu bảng SuatDien.</summary>
    public class SuatDienDAL
    {
        /// <summary>Lấy danh sách suất diễn kèm tên sự kiện từ SuatDien JOIN SuKien.</summary>
        public static DataTable LayDanhSach()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"SELECT sd.MaSuat, sd.MaSK, sk.TenSK, sd.ThoiGianBatDau, sd.ThoiGianKetThuc, sd.DiaDiem, sd.TrangThai 
                                 FROM SuatDien sd 
                                 INNER JOIN SuKien sk ON sd.MaSK = sk.MaSK
                                 ORDER BY sd.ThoiGianBatDau DESC";
                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        /// <summary>Lấy các suất diễn theo mã sự kiện kèm cột định dạng hiển thị cho ComboBox.</summary>
        public static DataTable LayDanhSachTheoSuKien(int maSK)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"SELECT MaSuat, MaSK, ThoiGianBatDau, ThoiGianKetThuc, DiaDiem, TrangThai, LoaiSoDo 
                                 FROM SuatDien 
                                 WHERE MaSK = @MaSK 
                                 ORDER BY ThoiGianBatDau";
                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@MaSK", maSK);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (!dt.Columns.Contains("HienThiSuat"))
                    {
                        dt.Columns.Add("HienThiSuat", typeof(string));
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        DateTime tg = Convert.ToDateTime(row["ThoiGianBatDau"]);
                        string diaDiem = row["DiaDiem"]?.ToString() ?? "";
                        string trangThai = row["TrangThai"]?.ToString() ?? "";
                        string ttText = "";
                        if (trangThai == "MoCuaBanVe") ttText = " (Mở bán)";
                        else if (trangThai == "DongBanVe") ttText = " (Đóng bán)";
                        else if (trangThai == "DangDien") ttText = " (Đang diễn)";
                        else if (trangThai == "DaKetThuc") ttText = " (Đã kết thúc)";

                        row["HienThiSuat"] = $"{tg:dd/MM/yyyy · HH:mm} · {diaDiem}{ttText}";
                    }
                    return dt;
                }
            }
        }

        /// <summary>Lấy chi tiết một suất diễn theo mã định danh.</summary>
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

        /// <summary>Thêm mới suất diễn vào bảng SuatDien.</summary>
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

        /// <summary>Cập nhật thông tin suất diễn trong bảng SuatDien.</summary>
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

        /// <summary>Xóa suất diễn khỏi bảng SuatDien.</summary>
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

        /// <summary>Đếm số đơn vé thuộc suất diễn (khóa ngoại DonVe).</summary>
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

        /// <summary>Đếm số khu vực thuộc suất diễn (khóa ngoại KhuVuc).</summary>
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
