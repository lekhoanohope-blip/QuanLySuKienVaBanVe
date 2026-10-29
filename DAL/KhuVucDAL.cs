using System;
using System.Data;
using System.Data.SqlClient;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.DAL
{
    public class KhuVucDAL
    {
        public static DataTable LayDanhSachTheoSuat(int maSuat)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
                    SELECT kv.MaKV, kv.TenKV, kv.HangVe, kv.TongSoGhe, kv.GiaVe, kv.ChoPhepBanVe,
                           (SELECT COUNT(*) FROM Ghe g WHERE g.MaKV = kv.MaKV AND g.TrangThai = N'DaBan') as DaBan,
                           (SELECT COUNT(*) FROM Ghe g WHERE g.MaKV = kv.MaKV AND g.TrangThai = N'Trong') as ConLai
                    FROM KhuVuc kv
                    WHERE kv.MaSuat = @MaSuat
                    ORDER BY kv.MaKV";

                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@MaSuat", maSuat);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public static KhuVucDTO LayTheoMa(int maKV)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
                    SELECT kv.MaKV, kv.MaSuat, kv.TenKV, kv.HangVe, kv.TongSoGhe, kv.GiaVe, kv.ChoPhepBanVe,
                           (SELECT COUNT(*) FROM Ghe g WHERE g.MaKV = kv.MaKV AND g.TrangThai = N'DaBan') as DaBan,
                           (SELECT COUNT(*) FROM Ghe g WHERE g.MaKV = kv.MaKV AND g.TrangThai = N'Trong') as ConLai
                    FROM KhuVuc kv
                    WHERE kv.MaKV = @MaKV";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaKV", maKV);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new KhuVucDTO
                            {
                                MaKV = Convert.ToInt32(reader["MaKV"]),
                                MaSuat = Convert.ToInt32(reader["MaSuat"]),
                                TenKV = reader["TenKV"].ToString(),
                                HangVe = reader["HangVe"].ToString(),
                                TongSoGhe = Convert.ToInt32(reader["TongSoGhe"]),
                                GiaVe = Convert.ToDecimal(reader["GiaVe"]),
                                ChoPhepBanVe = Convert.ToBoolean(reader["ChoPhepBanVe"]),
                                DaBan = Convert.ToInt32(reader["DaBan"]),
                                ConLai = Convert.ToInt32(reader["ConLai"])
                            };
                        }
                    }
                }
            }
            return null;
        }

        public static int Them(KhuVucDTO kv)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        const string sqlKV = @"
                            INSERT INTO KhuVuc (MaSuat, TenKV, HangVe, GiaVe, TongSoGhe, ChoPhepBanVe)
                            OUTPUT INSERTED.MaKV
                            VALUES (@MaSuat, @TenKV, @HangVe, @GiaVe, @TongSoGhe, @ChoPhepBanVe)";

                        int newMaKV;
                        using (SqlCommand cmdKV = new SqlCommand(sqlKV, conn, trans))
                        {
                            cmdKV.Parameters.AddWithValue("@MaSuat", kv.MaSuat);
                            cmdKV.Parameters.AddWithValue("@TenKV", kv.TenKV);
                            cmdKV.Parameters.AddWithValue("@HangVe", kv.HangVe ?? "VIP");
                            cmdKV.Parameters.AddWithValue("@GiaVe", kv.GiaVe);
                            cmdKV.Parameters.AddWithValue("@TongSoGhe", kv.TongSoGhe);
                            cmdKV.Parameters.AddWithValue("@ChoPhepBanVe", kv.ChoPhepBanVe);
                            newMaKV = (int)cmdKV.ExecuteScalar();
                        }

                        // Tối ưu chèn ghế set-based trong cùng connection và transaction, đảm bảo nguyên tử, đúng số ghế và vị trí
                        const string sqlGhe = @"
                            WITH Nbrs AS (
                                SELECT TOP (@TongSoGhe) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Cot
                                FROM sys.all_columns a CROSS JOIN sys.all_columns b
                            )
                            INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai)
                            SELECT @MaKV, 'A', Cot, N'Trong'
                            FROM Nbrs";

                        using (SqlCommand cmdGhe = new SqlCommand(sqlGhe, conn, trans))
                        {
                            cmdGhe.Parameters.AddWithValue("@MaKV", newMaKV);
                            cmdGhe.Parameters.AddWithValue("@TongSoGhe", kv.TongSoGhe);
                            cmdGhe.ExecuteNonQuery();
                        }

                        trans.Commit();
                        return newMaKV;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public static bool Sua(KhuVucDTO kv)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                const string sql = @"
                    UPDATE KhuVuc
                    SET TenKV = @TenKV, HangVe = @HangVe, GiaVe = @GiaVe, ChoPhepBanVe = @ChoPhepBanVe
                    WHERE MaKV = @MaKV";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TenKV", kv.TenKV);
                    cmd.Parameters.AddWithValue("@HangVe", kv.HangVe ?? "VIP");
                    cmd.Parameters.AddWithValue("@GiaVe", kv.GiaVe);
                    cmd.Parameters.AddWithValue("@ChoPhepBanVe", kv.ChoPhepBanVe);
                    cmd.Parameters.AddWithValue("@MaKV", kv.MaKV);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static bool Xoa(int maKV)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmdGhe = new SqlCommand("DELETE FROM Ghe WHERE MaKV = @MaKV", conn, trans))
                        {
                            cmdGhe.Parameters.AddWithValue("@MaKV", maKV);
                            cmdGhe.ExecuteNonQuery();
                        }

                        using (SqlCommand cmdKV = new SqlCommand("DELETE FROM KhuVuc WHERE MaKV = @MaKV", conn, trans))
                        {
                            cmdKV.Parameters.AddWithValue("@MaKV", maKV);
                            int affected = cmdKV.ExecuteNonQuery();

                            trans.Commit();
                            return affected > 0;
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Đếm số vé (kể cả vé đã hủy trong ChiTietVe) tham chiếu tới các ghế thuộc khu vực.
        /// </summary>
        public static int DemSoVeThamChieu(int maKV)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM ChiTietVe ctv
                    INNER JOIN Ghe g ON ctv.MaGhe = g.MaGhe
                    WHERE g.MaKV = @MaKV";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaKV", maKV);
                    conn.Open();
                    return (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}
