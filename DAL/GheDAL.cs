using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.DAL
{
    public class GheDAL
    {
        public const int ThoiGianGiuToiDaGiay = 600; // 10 phút = 600 giây

        private static bool _schemaEnsured = false;
        private static readonly object _schemaLock = new object();

        /// <summary>
        /// Đảm bảo bảng Ghe có cột ThoiGianGiu (Idempotent).
        /// </summary>
        public static void EnsureSchema()
        {
            if (_schemaEnsured) return;
            lock (_schemaLock)
            {
                if (_schemaEnsured) return;
                try
                {
                    using (SqlConnection conn = Database.GetConnection())
                    {
                        conn.Open();
                        string sql = @"
                            IF COL_LENGTH('dbo.Ghe', 'ThoiGianGiu') IS NULL
                            BEGIN
                                ALTER TABLE dbo.Ghe ADD ThoiGianGiu DATETIME NULL;
                            END
                            IF COL_LENGTH('dbo.Ghe', 'MaPhienGiu') IS NULL
                            BEGIN
                                ALTER TABLE dbo.Ghe ADD MaPhienGiu UNIQUEIDENTIFIER NULL;
                            END";
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    _schemaEnsured = true;
                }
                catch
                {
                    // Tránh crash nếu không có quyền DDL
                }
            }
        }

        /// <summary>
        /// Tự động giải phóng các ghế đang giữ quá hạn (> 10 phút) trong khu vực về 'Trong'.
        /// </summary>
        public static int GiaiPhongGheQuaHanTheoKhuVuc(int maKV)
        {
            EnsureSchema();
            string query = @"
                UPDATE Ghe
                SET TrangThai = N'Trong', ThoiGianGiu = NULL, MaPhienGiu = NULL
                WHERE MaKV = @MaKV
                  AND TrangThai = N'DangGiu'
                  AND (ThoiGianGiu IS NULL OR DATEDIFF(SECOND, ThoiGianGiu, GETDATE()) > @MaxSeconds);";

            SqlParameter[] parameters = {
                new SqlParameter("@MaKV", SqlDbType.Int) { Value = maKV },
                new SqlParameter("@MaxSeconds", SqlDbType.Int) { Value = ThoiGianGiuToiDaGiay }
            };
            return Database.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Lấy danh sách ghế của một khu vực (đã tự động nhả ghế quá hạn).
        /// </summary>
        public static DataTable LayDanhSachTheoKhuVuc(int maKV)
        {
            EnsureSchema();
            GiaiPhongGheQuaHanTheoKhuVuc(maKV);

            string query = @"
                SELECT g.MaGhe, g.MaKV, g.Hang, g.Cot, g.MaGheHienThi, g.TrangThai, g.ThoiGianGiu,
                       kv.TenKV, kv.HangVe, kv.GiaVe, kv.ChoPhepBanVe
                FROM Ghe g
                INNER JOIN KhuVuc kv ON g.MaKV = kv.MaKV
                WHERE g.MaKV = @MaKV
                ORDER BY LEN(g.Hang) ASC, g.Hang ASC, g.Cot ASC;";

            SqlParameter[] parameters = {
                new SqlParameter("@MaKV", SqlDbType.Int) { Value = maKV }
            };
            return Database.ExecuteQuery(query, parameters);
        }

        /// <summary>
        /// Giữ ghế tạm thời bằng Transaction ngắn + khóa dòng vi-mô WITH (UPDLOCK, ROWLOCK).
        /// </summary>
        public static bool GiuGheTamThoi(int maGhe, out string errorMessage)
        {
            EnsureSchema();
            errorMessage = string.Empty;

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        string selectSql = @"
                            SELECT g.MaGhe, g.MaGheHienThi, g.TrangThai, g.ThoiGianGiu,
                                   kv.ChoPhepBanVe, sd.MaSuat, sd.TrangThai AS TrangThaiSuat, sd.ThoiGianBatDau
                            FROM Ghe g WITH (UPDLOCK, ROWLOCK)
                            INNER JOIN KhuVuc kv ON g.MaKV = kv.MaKV
                            INNER JOIN SuatDien sd ON kv.MaSuat = sd.MaSuat
                            WHERE g.MaGhe = @MaGhe;";

                        SqlParameter[] selectParams = {
                            new SqlParameter("@MaGhe", SqlDbType.Int) { Value = maGhe }
                        };

                        DataTable dt = Database.ExecuteQuery(selectSql, conn, trans, selectParams);
                        if (dt.Rows.Count == 0)
                        {
                            trans.Rollback();
                            errorMessage = "Không tìm thấy vị trí ghế trong cơ sở dữ liệu.";
                            return false;
                        }

                        DataRow row = dt.Rows[0];
                        string maGheHienThi = row["MaGheHienThi"]?.ToString() ?? maGhe.ToString();
                        string trangThai = row["TrangThai"]?.ToString() ?? string.Empty;
                        bool choPhepBanVe = row["ChoPhepBanVe"] == DBNull.Value || Convert.ToBoolean(row["ChoPhepBanVe"]);
                        string trangThaiSuat = row["TrangThaiSuat"]?.ToString() ?? string.Empty;
                        DateTime thoiGianBatDau = Convert.ToDateTime(row["ThoiGianBatDau"]);

                        if (!choPhepBanVe || trangThaiSuat != "MoCuaBanVe" || thoiGianBatDau <= DateTime.Now)
                        {
                            trans.Rollback();
                            errorMessage = $"Khu vực hoặc suất diễn của ghế {maGheHienThi} hiện không mở bán vé.";
                            return false;
                        }

                        bool isExpiredHold = false;
                        if (trangThai == TrangThaiGhe.DangGiu)
                        {
                            if (row["ThoiGianGiu"] == DBNull.Value)
                            {
                                isExpiredHold = true;
                            }
                            else
                            {
                                DateTime thoiGianGiu = Convert.ToDateTime(row["ThoiGianGiu"]);
                                isExpiredHold = (DateTime.Now - thoiGianGiu).TotalSeconds > ThoiGianGiuToiDaGiay;
                            }
                        }

                        if (trangThai == TrangThaiGhe.DaBan)
                        {
                            trans.Rollback();
                            errorMessage = $"Ghế {maGheHienThi} vừa được bán ở một quầy giao dịch khác!";
                            return false;
                        }

                        if (trangThai == TrangThaiGhe.DangGiu && !isExpiredHold)
                        {
                            trans.Rollback();
                            errorMessage = $"Ghế {maGheHienThi} đang được giữ tạm thời bởi quầy khác!";
                            return false;
                        }

                        int maSuat = Convert.ToInt32(row["MaSuat"]);
                        Guid phienGiuId = Guid.NewGuid();

                        string checkPhienSql = @"
                            IF OBJECT_ID('dbo.PhienGiuGhe') IS NOT NULL
                            BEGIN
                                INSERT INTO PhienGiuGhe (MaPhienGiu, MaSuat, MaTK, BatDauUtc, HetHanUtc, TrangThai)
                                VALUES (@MaPhienGiu, @MaSuat, 1, SYSUTCDATETIME(), DATEADD(SECOND, 600, SYSUTCDATETIME()), N'HoatDong');
                            END";
                        using (SqlCommand cmdPhien = new SqlCommand(checkPhienSql, conn, trans))
                        {
                            cmdPhien.Parameters.AddWithValue("@MaPhienGiu", phienGiuId);
                            cmdPhien.Parameters.AddWithValue("@MaSuat", maSuat);
                            cmdPhien.ExecuteNonQuery();
                        }

                        string updateSql = @"
                            UPDATE Ghe
                            SET TrangThai = N'DangGiu',
                                ThoiGianGiu = GETDATE(),
                                MaPhienGiu = @MaPhienGiu
                            WHERE MaGhe = @MaGhe;";

                        SqlParameter[] updateParams = {
                            new SqlParameter("@MaPhienGiu", SqlDbType.UniqueIdentifier) { Value = phienGiuId },
                            new SqlParameter("@MaGhe", SqlDbType.Int) { Value = maGhe }
                        };

                        int affected = Database.ExecuteNonQuery(updateSql, conn, trans, updateParams);
                        if (affected <= 0)
                        {
                            trans.Rollback();
                            errorMessage = $"Không thể khóa giữ ghế {maGheHienThi}.";
                            return false;
                        }

                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        try { trans.Rollback(); } catch { }
                        errorMessage = "Lỗi khi giữ ghế: " + ex.Message;
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Thuật toán phân bổ và giữ ghế tự động theo quy tắc nghiệp vụ:
        /// "Thanh toán càng sớm, ghế càng gần sân khấu: Hệ thống sẽ tự động xếp chỗ ưu tiên
        /// từ các hàng ghế đầu tiên (gần sân khấu nhất) lùi dần về sau theo thứ tự thời gian giao dịch".
        /// Sử dụng khóa vi-mô WITH (UPDLOCK, ROWLOCK) để chống chọn trùng giữa các quầy.
        /// </summary>
        public static bool GiuGheTuDongTheoKhuVuc(int maKV, int soLuong, out List<GheDTO> dsGheGiu, out string errorMessage)
        {
            EnsureSchema();
            dsGheGiu = new List<GheDTO>();
            errorMessage = string.Empty;

            if (soLuong <= 0)
            {
                errorMessage = "Số lượng vé yêu cầu phải lớn hơn 0.";
                return false;
            }

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        // 1. Dọn dẹp ghế quá hạn trong khu vực trước khi chọn
                        string cleanSql = @"
                            UPDATE Ghe
                            SET TrangThai = N'Trong', ThoiGianGiu = NULL, MaPhienGiu = NULL
                            WHERE MaKV = @MaKV
                              AND TrangThai = N'DangGiu'
                              AND (ThoiGianGiu IS NULL OR DATEDIFF(SECOND, ThoiGianGiu, GETDATE()) > @MaxSeconds);";

                        using (SqlCommand cmdClean = new SqlCommand(cleanSql, conn, trans))
                        {
                            cmdClean.Parameters.AddWithValue("@MaKV", maKV);
                            cmdClean.Parameters.AddWithValue("@MaxSeconds", ThoiGianGiuToiDaGiay);
                            cmdClean.ExecuteNonQuery();
                        }

                        // 2. Đọc và đặt khóa UPDLOCK, ROWLOCK trên TOP (@SoLuong) ghế gần sân khấu nhất
                        // Sắp xếp theo hàng A -> Z (độ dài hàng và chữ cái), sau đó theo số cột 1..N
                        string selectSql = @"
                            SELECT TOP (@SoLuong)
                                   g.MaGhe, g.MaKV, g.Hang, g.Cot, g.MaGheHienThi, g.TrangThai, g.ThoiGianGiu,
                                   kv.TenKV, kv.HangVe, kv.GiaVe, kv.ChoPhepBanVe,
                                   sd.MaSuat, sd.TrangThai AS TrangThaiSuat, sd.ThoiGianBatDau
                            FROM Ghe g WITH (UPDLOCK, ROWLOCK)
                            INNER JOIN KhuVuc kv WITH (UPDLOCK, ROWLOCK) ON g.MaKV = kv.MaKV
                            INNER JOIN SuatDien sd ON kv.MaSuat = sd.MaSuat
                            WHERE g.MaKV = @MaKV
                              AND g.TrangThai = N'Trong'
                            ORDER BY LEN(g.Hang) ASC, g.Hang ASC, g.Cot ASC;";

                        DataTable dt = new DataTable();
                        using (SqlCommand cmdSelect = new SqlCommand(selectSql, conn, trans))
                        {
                            cmdSelect.Parameters.AddWithValue("@SoLuong", soLuong);
                            cmdSelect.Parameters.AddWithValue("@MaKV", maKV);
                            using (SqlDataAdapter da = new SqlDataAdapter(cmdSelect))
                            {
                                da.Fill(dt);
                            }
                        }

                        if (dt.Rows.Count < soLuong)
                        {
                            trans.Rollback();
                            errorMessage = $"Khu vực này hiện chỉ còn {dt.Rows.Count} ghế trống, không đủ {soLuong} vé theo yêu cầu.";
                            return false;
                        }

                        // Kiểm tra tính hợp lệ của khu vực và suất diễn
                        DataRow firstRow = dt.Rows[0];
                        bool choPhepBanVe = firstRow["ChoPhepBanVe"] == DBNull.Value || Convert.ToBoolean(firstRow["ChoPhepBanVe"]);
                        string trangThaiSuat = firstRow["TrangThaiSuat"]?.ToString() ?? string.Empty;
                        DateTime thoiGianBatDau = Convert.ToDateTime(firstRow["ThoiGianBatDau"]);

                        if (!choPhepBanVe || trangThaiSuat != "MoCuaBanVe" || thoiGianBatDau <= DateTime.Now)
                        {
                            trans.Rollback();
                            errorMessage = "Khu vực hoặc suất diễn này hiện đã đóng bán vé.";
                            return false;
                        }

                        // 3. Khóa giữ các ghế đã chọn sang 'DangGiu' kèm ThoiGianGiu = GETDATE()
                        List<int> seatIds = new List<int>();
                        foreach (DataRow r in dt.Rows)
                        {
                            int id = Convert.ToInt32(r["MaGhe"]);
                            seatIds.Add(id);
                            dsGheGiu.Add(new GheDTO(r));
                        }

                        int maSuat = Convert.ToInt32(firstRow["MaSuat"]);
                        Guid phienGiuId = Guid.NewGuid();

                        string checkPhienSql = @"
                            IF OBJECT_ID('dbo.PhienGiuGhe') IS NOT NULL
                            BEGIN
                                INSERT INTO PhienGiuGhe (MaPhienGiu, MaSuat, MaTK, BatDauUtc, HetHanUtc, TrangThai)
                                VALUES (@MaPhienGiu, @MaSuat, 1, SYSUTCDATETIME(), DATEADD(SECOND, @MaxSeconds, SYSUTCDATETIME()), N'HoatDong');
                            END";
                        using (SqlCommand cmdPhien = new SqlCommand(checkPhienSql, conn, trans))
                        {
                            cmdPhien.Parameters.AddWithValue("@MaPhienGiu", phienGiuId);
                            cmdPhien.Parameters.AddWithValue("@MaSuat", maSuat);
                            cmdPhien.Parameters.AddWithValue("@MaxSeconds", ThoiGianGiuToiDaGiay);
                            cmdPhien.ExecuteNonQuery();
                        }

                        string inClause = string.Join(",", seatIds);
                        string updateSql = $@"
                            UPDATE Ghe
                            SET TrangThai = N'DangGiu',
                                ThoiGianGiu = GETDATE(),
                                MaPhienGiu = @MaPhienGiu
                            WHERE MaGhe IN ({inClause});";

                        using (SqlCommand cmdUpdate = new SqlCommand(updateSql, conn, trans))
                        {
                            cmdUpdate.Parameters.AddWithValue("@MaPhienGiu", phienGiuId);
                            int affected = cmdUpdate.ExecuteNonQuery();
                            if (affected != soLuong)
                            {
                                trans.Rollback();
                                errorMessage = "Không thể giữ đủ số lượng ghế yêu cầu.";
                                return false;
                            }
                        }

                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        try { trans.Rollback(); } catch { }
                        errorMessage = "Lỗi khi phân bổ ghế: " + ex.Message;
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Giải phóng 1 ghế đang giữ tạm về 'Trong'.
        /// </summary>
        public static bool GiaiPhongGhe(int maGhe)
        {
            EnsureSchema();
            string query = @"
                UPDATE Ghe
                SET TrangThai = N'Trong', ThoiGianGiu = NULL, MaPhienGiu = NULL
                WHERE MaGhe = @MaGhe AND TrangThai = N'DangGiu';";

            SqlParameter[] parameters = {
                new SqlParameter("@MaGhe", SqlDbType.Int) { Value = maGhe }
            };
            return Database.ExecuteNonQuery(query, parameters) > 0;
        }

        /// <summary>
        /// Giải phóng danh sách ghế đang giữ về 'Trong'.
        /// </summary>
        /// <summary>
        /// Lấy số giây còn lại tối thiểu của danh sách ghế đang giữ dựa trên đồng hồ của SQL Server (GETDATE).
        /// Nếu không có ghế nào hoặc ghế đã quá hạn / đã bán thì trả về 0.
        /// </summary>
        public static int LayGiayConLaiGiu(IEnumerable<int> dsMaGhe)
        {
            if (dsMaGhe == null) return 0;
            List<int> validIds = dsMaGhe.Where(id => id > 0).ToList();
            if (validIds.Count == 0) return 0;

            EnsureSchema();
            string inClause = string.Join(",", validIds);
            string query = $@"
                SELECT ISNULL(MIN({ThoiGianGiuToiDaGiay} - DATEDIFF(SECOND, ThoiGianGiu, GETDATE())), 0)
                FROM Ghe
                WHERE MaGhe IN ({inClause})
                  AND TrangThai = N'DangGiu'
                  AND ThoiGianGiu IS NOT NULL;";

            object result = Database.ExecuteScalar(query);
            if (result == null || result == DBNull.Value) return 0;
            int remaining = Convert.ToInt32(result);
            return remaining > 0 ? remaining : 0;
        }

        public static int GiaiPhongDanhSachGhe(IEnumerable<int> dsMaGhe)
        {
            if (dsMaGhe == null) return 0;
            List<int> validIds = dsMaGhe.Where(id => id > 0).ToList();
            if (validIds.Count == 0) return 0;

            EnsureSchema();
            string inClause = string.Join(",", validIds);
            string query = $@"
                UPDATE Ghe
                SET TrangThai = N'Trong', ThoiGianGiu = NULL, MaPhienGiu = NULL
                WHERE MaGhe IN ({inClause}) AND TrangThai = N'DangGiu';";

            return Database.ExecuteNonQuery(query);
        }

        /// <summary>
        /// Kiểm tra và khóa dòng ghế bằng WITH (UPDLOCK, ROWLOCK) bên trong SqlTransaction thanh toán.
        /// </summary>
        public static DataRow KiemTraVaKhoaGheTrongGiaoDich(int maGhe, SqlConnection conn, SqlTransaction trans)
        {
            string query = @"
                SELECT g.MaGhe, g.MaGheHienThi, g.Hang, g.Cot, g.TrangThai, g.ThoiGianGiu,
                       CASE WHEN g.ThoiGianGiu IS NULL THEN 999999
                            ELSE DATEDIFF(SECOND, g.ThoiGianGiu, GETDATE())
                       END AS SoGiayDaGiu,
                       kv.MaKV, kv.TenKV, kv.HangVe, kv.GiaVe, kv.ChoPhepBanVe,
                       sd.MaSuat, sd.MaSK, sd.ThoiGianBatDau, sd.TrangThai AS TrangThaiSuat
                FROM Ghe g WITH (UPDLOCK, ROWLOCK)
                INNER JOIN KhuVuc kv WITH (UPDLOCK, ROWLOCK) ON g.MaKV = kv.MaKV
                INNER JOIN SuatDien sd ON kv.MaSuat = sd.MaSuat
                WHERE g.MaGhe = @MaGhe;";

            SqlParameter[] parameters = {
                new SqlParameter("@MaGhe", SqlDbType.Int) { Value = maGhe }
            };

            DataTable dt = Database.ExecuteQuery(query, conn, trans, parameters);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        /// <summary>
        /// Cập nhật trạng thái ghế trong cùng SqlTransaction.
        /// </summary>
        public static bool CapNhatTrangThai(int maGhe, string trangThai, SqlConnection conn, SqlTransaction trans)
        {
            string query = @"
                UPDATE Ghe
                SET TrangThai = @TrangThai,
                    ThoiGianGiu = CASE WHEN @TrangThai = N'DangGiu' THEN GETDATE() ELSE NULL END,
                    MaPhienGiu = CASE WHEN @TrangThai = N'DangGiu' THEN NEWID() ELSE NULL END
                WHERE MaGhe = @MaGhe;";

            SqlParameter[] parameters = {
                new SqlParameter("@TrangThai", SqlDbType.NVarChar, 30) { Value = trangThai },
                new SqlParameter("@MaGhe", SqlDbType.Int) { Value = maGhe }
            };
            return Database.ExecuteNonQuery(query, conn, trans, parameters) > 0;
        }
    }
}
