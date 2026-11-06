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
        /// Đảm bảo bảng Ghe có cột ThoiGianGiu và MaPhienGiu (Idempotent).
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

                        // Xác nhận lại rằng cả hai cột đã thực sự tồn tại
                        string verifySql = @"
                            SELECT CASE WHEN COL_LENGTH('dbo.Ghe', 'ThoiGianGiu') IS NOT NULL 
                                         AND COL_LENGTH('dbo.Ghe', 'MaPhienGiu') IS NOT NULL THEN 1 ELSE 0 END;";
                        using (SqlCommand cmdVerify = new SqlCommand(verifySql, conn))
                        {
                            object ok = cmdVerify.ExecuteScalar();
                            if (ok == null || Convert.ToInt32(ok) != 1)
                            {
                                throw new InvalidOperationException("Thiếu cột Ghe.ThoiGianGiu hoặc Ghe.MaPhienGiu. Vui lòng thực thi migration database/migrate_tv3_banve.sql!");
                            }
                        }
                    }
                    _schemaEnsured = true;
                }
                catch (SqlException sqlEx)
                {
                    throw new InvalidOperationException("Không thể tự động cập nhật cấu trúc bảng Ghe (thiếu quyền DDL). Vui lòng chạy database/migrate_tv3_banve.sql: " + sqlEx.Message, sqlEx);
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
        /// Tự động giải phóng các ghế đang giữ quá hạn (> 10 phút) của toàn bộ suất diễn về 'Trong'.
        /// </summary>
        public static int GiaiPhongGheQuaHanTheoSuat(int maSuat)
        {
            EnsureSchema();
            string query = @"
                UPDATE g
                SET g.TrangThai = N'Trong', g.ThoiGianGiu = NULL, g.MaPhienGiu = NULL
                FROM Ghe g
                INNER JOIN KhuVuc kv ON g.MaKV = kv.MaKV
                WHERE kv.MaSuat = @MaSuat
                  AND g.TrangThai = N'DangGiu'
                  AND (g.ThoiGianGiu IS NULL OR DATEDIFF(SECOND, g.ThoiGianGiu, GETDATE()) > @MaxSeconds);";

            SqlParameter[] parameters = {
                new SqlParameter("@MaSuat", SqlDbType.Int) { Value = maSuat },
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
                SELECT g.MaGhe, g.MaKV, g.Hang, g.Cot, g.MaGheHienThi, g.TrangThai, g.ThoiGianGiu, g.MaPhienGiu,
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
        /// Trả về mã phiên giữ ghế maPhienGiu nếu thành công.
        /// </summary>
        public static bool GiuGheTamThoi(int maGhe, out Guid maPhienGiu, out string errorMessage)
        {
            EnsureSchema();
            errorMessage = string.Empty;
            maPhienGiu = Guid.Empty;

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        string selectSql = @"
                            SELECT g.MaGhe, g.MaGheHienThi, g.TrangThai, g.ThoiGianGiu, g.MaPhienGiu,
                                   kv.ChoPhepBanVe, sd.MaSuat, sd.TrangThai AS TrangThaiSuat,
                                   CASE WHEN sd.ThoiGianBatDau <= GETDATE() THEN 1 ELSE 0 END AS DaBatDau,
                                   CASE WHEN g.ThoiGianGiu IS NULL OR DATEDIFF(SECOND, g.ThoiGianGiu, GETDATE()) > @MaxSeconds
                                        THEN 1 ELSE 0 END AS GiuQuaHan
                            FROM Ghe g WITH (UPDLOCK, ROWLOCK)
                            INNER JOIN KhuVuc kv ON g.MaKV = kv.MaKV
                            INNER JOIN SuatDien sd ON kv.MaSuat = sd.MaSuat
                            WHERE g.MaGhe = @MaGhe;";

                        SqlParameter[] selectParams = {
                            new SqlParameter("@MaGhe", SqlDbType.Int) { Value = maGhe },
                            new SqlParameter("@MaxSeconds", SqlDbType.Int) { Value = ThoiGianGiuToiDaGiay }
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
                        bool daBatDau = Convert.ToInt32(row["DaBatDau"]) == 1;

                        if (!choPhepBanVe || trangThaiSuat != "MoCuaBanVe" || daBatDau)
                        {
                            trans.Rollback();
                            errorMessage = $"Khu vực hoặc suất diễn của ghế {maGheHienThi} hiện không mở bán vé.";
                            return false;
                        }

                        bool isExpiredHold = Convert.ToInt32(row["GiuQuaHan"]) == 1;

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

                        Guid phienGiuId = Guid.NewGuid();

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
                        maPhienGiu = phienGiuId;
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
        /// Thuật toán phân bổ và giữ ghế tự động theo quy tắc:
        /// Ưu tiên liền kề cùng hàng, sau đó cùng hàng, cuối cùng các ghế gần sân khấu nhất.
        /// Sử dụng khóa vi-mô WITH (UPDLOCK, ROWLOCK) để chống chọn trùng giữa các quầy.
        /// Trả về mã phiên giữ ghế maPhienGiu duy nhất.
        /// </summary>
        public static bool GiuGheTuDongTheoKhuVuc(int maKV, int soLuong, out List<GheDTO> dsGheGiu, out Guid maPhienGiu, out string errorMessage)
        {
            EnsureSchema();
            dsGheGiu = new List<GheDTO>();
            errorMessage = string.Empty;
            maPhienGiu = Guid.Empty;

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

                        // 2. Khóa danh sách ghế trống để chọn dãy liền kề trong cùng transaction.
                        string selectSql = @"
                            SELECT
                                   g.MaGhe, g.MaKV, g.Hang, g.Cot, g.MaGheHienThi, g.TrangThai, g.ThoiGianGiu,
                                   kv.TenKV, kv.HangVe, kv.GiaVe, kv.ChoPhepBanVe,
                                   sd.MaSuat, sd.TrangThai AS TrangThaiSuat,
                                   CASE WHEN sd.ThoiGianBatDau <= GETDATE() THEN 1 ELSE 0 END AS DaBatDau
                            FROM Ghe g WITH (UPDLOCK, ROWLOCK)
                            INNER JOIN KhuVuc kv WITH (UPDLOCK, ROWLOCK) ON g.MaKV = kv.MaKV
                            INNER JOIN SuatDien sd ON kv.MaSuat = sd.MaSuat
                            WHERE g.MaKV = @MaKV
                              AND g.TrangThai = N'Trong'
                            ORDER BY LEN(g.Hang) ASC, g.Hang ASC, g.Cot ASC;";

                        DataTable dt = new DataTable();
                        using (SqlCommand cmdSelect = new SqlCommand(selectSql, conn, trans))
                        {
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
                        bool daBatDau = Convert.ToInt32(firstRow["DaBatDau"]) == 1;

                        if (!choPhepBanVe || trangThaiSuat != "MoCuaBanVe" || daBatDau)
                        {
                            trans.Rollback();
                            errorMessage = "Khu vực hoặc suất diễn này hiện đã đóng bán vé.";
                            return false;
                        }

                        // 3. Khóa giữ các ghế đã chọn sang 'DangGiu' kèm ThoiGianGiu = GETDATE() và MaPhienGiu
                        List<int> seatIds = new List<int>();
                        foreach (DataRow r in ChonGheUuTien(dt, soLuong))
                        {
                            int id = Convert.ToInt32(r["MaGhe"]);
                            seatIds.Add(id);
                            dsGheGiu.Add(new GheDTO(r));
                        }

                        Guid phienGiuId = Guid.NewGuid();

                        var updateParams = new List<SqlParameter>();
                        var inParams = new List<string>();
                        for (int i = 0; i < seatIds.Count; i++)
                        {
                            string p = "@id" + i;
                            inParams.Add(p);
                            updateParams.Add(new SqlParameter(p, SqlDbType.Int) { Value = seatIds[i] });
                        }
                        updateParams.Add(new SqlParameter("@MaPhienGiu", SqlDbType.UniqueIdentifier) { Value = phienGiuId });

                        string updateSql = $@"
                            UPDATE Ghe
                            SET TrangThai = N'DangGiu',
                                ThoiGianGiu = GETDATE(),
                                MaPhienGiu = @MaPhienGiu
                            WHERE MaGhe IN ({string.Join(",", inParams)});";

                        using (SqlCommand cmdUpdate = new SqlCommand(updateSql, conn, trans))
                        {
                            cmdUpdate.Parameters.AddRange(updateParams.ToArray());
                            int affected = cmdUpdate.ExecuteNonQuery();
                            if (affected != soLuong)
                            {
                                trans.Rollback();
                                errorMessage = "Không thể giữ đủ số lượng ghế yêu cầu.";
                                return false;
                            }
                        }

                        trans.Commit();
                        maPhienGiu = phienGiuId;
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
        /// Danh sách đầu vào đã được SQL sắp theo LEN(Hang), Hang, Cot (gần sân khấu trước).
        /// </summary>
        private static List<DataRow> ChonGheUuTien(DataTable gheTrong, int soLuong)
        {
            var cacHang = gheTrong.AsEnumerable().GroupBy(r => r.Field<string>("Hang")).ToList();
            foreach (var hang in cacHang)
            {
                var ghe = hang.ToList();
                int lienTiep = 0;
                for (int i = 0; i < ghe.Count; i++)
                {
                    lienTiep = i > 0 && ghe[i].Field<int>("Cot") == ghe[i - 1].Field<int>("Cot") + 1
                        ? lienTiep + 1 : 1;
                    if (lienTiep == soLuong)
                        return ghe.Skip(i - soLuong + 1).Take(soLuong).ToList();
                }
            }

            var cungHang = cacHang.FirstOrDefault(hang => hang.Count() >= soLuong);
            if (cungHang != null) return cungHang.Take(soLuong).ToList();
            return gheTrong.AsEnumerable().Take(soLuong).ToList();
        }

        /// <summary>
        /// Giải phóng 1 ghế đang giữ tạm về 'Trong' (chỉ giải phóng nếu thuộc đúng phiên giữ).
        /// </summary>
        public static bool GiaiPhongGhe(int maGhe, Guid maPhienGiu)
        {
            if (maGhe <= 0 || maPhienGiu == Guid.Empty) return false;
            EnsureSchema();
            string query = @"
                UPDATE Ghe
                SET TrangThai = N'Trong', ThoiGianGiu = NULL, MaPhienGiu = NULL
                WHERE MaGhe = @MaGhe AND TrangThai = N'DangGiu' AND MaPhienGiu = @MaPhienGiu;";

            SqlParameter[] parameters = {
                new SqlParameter("@MaGhe", SqlDbType.Int) { Value = maGhe },
                new SqlParameter("@MaPhienGiu", SqlDbType.UniqueIdentifier) { Value = maPhienGiu }
            };
            return Database.ExecuteNonQuery(query, parameters) > 0;
        }

        /// <summary>
        /// Lấy số giây còn lại tối thiểu của danh sách ghế đang giữ dựa trên đồng hồ SQL Server (GETDATE).
        /// Chỉ trả về thời gian nếu TOÀN BỘ danh sách ghế vẫn thuộc đúng phiên giữ maPhienGiu.
        /// </summary>
        public static int LayGiayConLaiGiu(IEnumerable<int> dsMaGhe, Guid maPhienGiu)
        {
            if (dsMaGhe == null || maPhienGiu == Guid.Empty) return 0;
            List<int> validIds = dsMaGhe.Where(id => id > 0).ToList();
            if (validIds.Count == 0) return 0;

            EnsureSchema();
            var parameters = new List<SqlParameter>();
            var inParams = new List<string>();
            for (int i = 0; i < validIds.Count; i++)
            {
                string p = "@id" + i;
                inParams.Add(p);
                parameters.Add(new SqlParameter(p, SqlDbType.Int) { Value = validIds[i] });
            }
            parameters.Add(new SqlParameter("@MaPhienGiu", SqlDbType.UniqueIdentifier) { Value = maPhienGiu });

            string query = $@"
                SELECT COUNT(1) AS SoGheKhopPhien,
                       ISNULL(MIN({ThoiGianGiuToiDaGiay} - DATEDIFF(SECOND, ThoiGianGiu, GETDATE())), 0) AS GiayConLai
                FROM Ghe
                WHERE MaGhe IN ({string.Join(",", inParams)})
                  AND TrangThai = N'DangGiu'
                  AND MaPhienGiu = @MaPhienGiu
                  AND ThoiGianGiu IS NOT NULL;";

            DataTable dt = Database.ExecuteQuery(query, parameters.ToArray());
            if (dt == null || dt.Rows.Count == 0) return 0;

            int soGheKhop = Convert.ToInt32(dt.Rows[0]["SoGheKhopPhien"]);
            if (soGheKhop < validIds.Count) return 0; // Một trong các ghế đã bị mất phiên hoặc giải phóng

            int remaining = Convert.ToInt32(dt.Rows[0]["GiayConLai"]);
            return remaining > 0 ? remaining : 0;
        }

        /// <summary>
        /// Giải phóng danh sách ghế đang giữ về 'Trong' (chỉ giải phóng nếu thuộc đúng phiên giữ maPhienGiu).
        /// </summary>
        public static int GiaiPhongDanhSachGhe(IEnumerable<int> dsMaGhe, Guid maPhienGiu)
        {
            if (dsMaGhe == null || maPhienGiu == Guid.Empty) return 0;
            List<int> validIds = dsMaGhe.Where(id => id > 0).ToList();
            if (validIds.Count == 0) return 0;

            EnsureSchema();
            var parameters = new List<SqlParameter>();
            var inParams = new List<string>();
            for (int i = 0; i < validIds.Count; i++)
            {
                string p = "@id" + i;
                inParams.Add(p);
                parameters.Add(new SqlParameter(p, SqlDbType.Int) { Value = validIds[i] });
            }
            parameters.Add(new SqlParameter("@MaPhienGiu", SqlDbType.UniqueIdentifier) { Value = maPhienGiu });

            string query = $@"
                UPDATE Ghe
                SET TrangThai = N'Trong', ThoiGianGiu = NULL, MaPhienGiu = NULL
                WHERE MaGhe IN ({string.Join(",", inParams)})
                  AND TrangThai = N'DangGiu'
                  AND MaPhienGiu = @MaPhienGiu;";

            return Database.ExecuteNonQuery(query, parameters.ToArray());
        }

        /// <summary>
        /// Kiểm tra và khóa dòng ghế bằng WITH (UPDLOCK, ROWLOCK) bên trong SqlTransaction thanh toán.
        /// Trả về các trường thông tin ghế, MaPhienGiu và cờ DaBatDau tính theo GETDATE() SQL.
        /// </summary>
        public static DataRow KiemTraVaKhoaGheTrongGiaoDich(int maGhe, SqlConnection conn, SqlTransaction trans)
        {
            string query = @"
                SELECT g.MaGhe, g.MaGheHienThi, g.Hang, g.Cot, g.TrangThai, g.ThoiGianGiu, g.MaPhienGiu,
                       CASE WHEN g.ThoiGianGiu IS NULL THEN 999999
                            ELSE DATEDIFF(SECOND, g.ThoiGianGiu, GETDATE())
                       END AS SoGiayDaGiu,
                       kv.MaKV, kv.TenKV, kv.HangVe, kv.GiaVe, kv.ChoPhepBanVe,
                       sd.MaSuat, sd.MaSK, sd.ThoiGianBatDau, sd.TrangThai AS TrangThaiSuat,
                       CASE WHEN sd.ThoiGianBatDau <= GETDATE() THEN 1 ELSE 0 END AS DaBatDau
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
        /// Chỉ cho phép chuyển sang DaBan nếu ghế thuộc đúng phiên giữ maPhienGiu.
        /// </summary>
        public static bool CapNhatTrangThai(int maGhe, string trangThai, Guid? maPhienGiu, SqlConnection conn, SqlTransaction trans)
        {
            if ((trangThai == TrangThaiGhe.DaBan || trangThai == TrangThaiGhe.DangGiu)
                && (!maPhienGiu.HasValue || maPhienGiu.Value == Guid.Empty)) return false;

            string query = @"
                UPDATE Ghe
                SET TrangThai = @TrangThai,
                    ThoiGianGiu = CASE WHEN @TrangThai = N'DangGiu' THEN GETDATE() ELSE NULL END,
                    MaPhienGiu = CASE WHEN @TrangThai = N'DangGiu' THEN @MaPhienGiu ELSE NULL END
                WHERE MaGhe = @MaGhe
                  AND (@MaPhienGiu IS NULL OR MaPhienGiu = @MaPhienGiu OR @TrangThai = N'DangGiu')
                  AND (@TrangThai <> N'DaBan' OR TrangThai = N'DangGiu');";

            SqlParameter[] parameters = {
                new SqlParameter("@TrangThai", SqlDbType.NVarChar, 30) { Value = trangThai },
                new SqlParameter("@MaGhe", SqlDbType.Int) { Value = maGhe },
                new SqlParameter("@MaPhienGiu", SqlDbType.UniqueIdentifier) { Value = (object)maPhienGiu ?? DBNull.Value }
            };
            return Database.ExecuteNonQuery(query, conn, trans, parameters) > 0;
        }
    }
}
