using System;
using System.Data;
using System.Data.SqlClient;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.DAL
{
    public class KhuVucDAL
    {
        private static bool _schemaEnsured = false;
        private static readonly object _schemaLock = new object();

        public static void EnsureSchemaAndSyncSeats()
        {
            if (_schemaEnsured) return;
            lock (_schemaLock)
            {
                if (_schemaEnsured) return;
                try
                {
                    using (SqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();

                        // 1. Thêm cột MaVung nếu chưa có
                        string checkColSql = @"
                            IF COL_LENGTH('KhuVuc', 'MaVung') IS NULL
                            BEGIN
                                ALTER TABLE KhuVuc ADD MaVung VARCHAR(20) NULL;
                            END";
                        using (SqlCommand cmd = new SqlCommand(checkColSql, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        // 2. Sửa lỗi gán sai trước đó và gán MaVung cho dữ liệu mẫu cũ theo tên cụ thể
                        string updateMaVungSql = @"
                            -- Sửa lỗi những bản ghi VIP B từng bị gán nhầm thành VIP_A do điều kiện %Khu VIP% cũ
                            UPDATE KhuVuc SET MaVung = 'VIP_B' WHERE MaVung = 'VIP_A' AND (TenKV LIKE '%VIP B%' OR TenKV LIKE '%VIP_B%');

                            -- Khớp tên cụ thể và chính xác trước; dữ liệu không rõ ràng giữ nguyên NULL để người dùng tự cấu hình
                            UPDATE KhuVuc SET MaVung = 'VIP_B' WHERE MaVung IS NULL AND (TenKV LIKE '%VIP B%' OR TenKV LIKE '%VIP_B%');
                            UPDATE KhuVuc SET MaVung = 'VIP_A' WHERE MaVung IS NULL AND (TenKV LIKE '%VIP A%' OR TenKV LIKE '%VIP_A%');
                            UPDATE KhuVuc SET MaVung = 'GA_B'  WHERE MaVung IS NULL AND (TenKV LIKE '%GA B%'  OR TenKV LIKE '%GA_B%');
                            UPDATE KhuVuc SET MaVung = 'GA_A'  WHERE MaVung IS NULL AND (TenKV LIKE '%GA A%'  OR TenKV LIKE '%GA_A%');
                            UPDATE KhuVuc SET MaVung = 'CAT'   WHERE MaVung IS NULL AND (TenKV LIKE '%CAT %'  OR TenKV LIKE '%CAT_%' OR TenKV = 'CAT');
                        ";
                        using (SqlCommand cmd = new SqlCommand(updateMaVungSql, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        // 3. Đồng bộ ghế bị thiếu cho các khu vực mẫu cũ
                        string syncMissingSeatsSql = @"
                            DECLARE @MaKV INT, @TongSoGhe INT, @CurrentGheCount INT;
                            DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
                                SELECT kv.MaKV, kv.TongSoGhe, ISNULL(COUNT(g.MaGhe), 0)
                                FROM KhuVuc kv
                                LEFT JOIN Ghe g ON kv.MaKV = g.MaKV
                                GROUP BY kv.MaKV, kv.TongSoGhe
                                HAVING ISNULL(COUNT(g.MaGhe), 0) < kv.TongSoGhe;

                            OPEN cur;
                            FETCH NEXT FROM cur INTO @MaKV, @TongSoGhe, @CurrentGheCount;
                            WHILE @@FETCH_STATUS = 0
                            BEGIN
                                DECLARE @MissingCount INT = @TongSoGhe - @CurrentGheCount;
                                DECLARE @ColPerRow INT;
                                SELECT @ColPerRow = ISNULL(MAX(Cot), 10) FROM Ghe WHERE MaKV = @MaKV;
                                IF @ColPerRow IS NULL OR @ColPerRow < 10
                                    SET @ColPerRow = 10;

                                WITH Numbers AS (
                                    SELECT TOP (10000) (ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1) AS Idx
                                    FROM sys.all_columns a CROSS JOIN sys.all_columns b
                                ),
                                Candidates AS (
                                    SELECT 
                                        @MaKV AS MaKV,
                                        CASE 
                                            WHEN (Idx / @ColPerRow) < 26 THEN CHAR(65 + (Idx / @ColPerRow))
                                            ELSE CHAR(65 + ((Idx / @ColPerRow) / 26 - 1)) + CHAR(65 + ((Idx / @ColPerRow) % 26))
                                        END AS Hang,
                                        (Idx % @ColPerRow) + 1 AS Cot,
                                        Idx
                                    FROM Numbers
                                ),
                                MissingCandidates AS (
                                    SELECT c.MaKV, c.Hang, c.Cot, c.Idx
                                    FROM Candidates c
                                    WHERE NOT EXISTS (
                                        SELECT 1 FROM Ghe g 
                                        WHERE g.MaKV = @MaKV AND g.Hang = c.Hang AND g.Cot = c.Cot
                                    )
                                )
                                INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai)
                                SELECT TOP (@MissingCount) mc.MaKV, mc.Hang, mc.Cot, N'Trong'
                                FROM MissingCandidates mc
                                ORDER BY mc.Idx ASC;

                                FETCH NEXT FROM cur INTO @MaKV, @TongSoGhe, @CurrentGheCount;
                            END;
                            CLOSE cur;
                            DEALLOCATE cur;";

                        using (SqlCommand cmd = new SqlCommand(syncMissingSeatsSql, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    _schemaEnsured = true;
                }
                catch
                {
                    // Tránh crash nếu không có quyền DDL, lần sau thử lại
                }
            }
        }

        public static DataTable LayDanhSachTheoSuat(int maSuat)
        {
            EnsureSchemaAndSyncSeats();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
                    SELECT kv.MaKV, kv.MaSuat, ISNULL(kv.MaVung, '') AS MaVung, kv.TenKV, kv.HangVe, kv.TongSoGhe, kv.GiaVe, kv.ChoPhepBanVe,
                           (SELECT COUNT(*) FROM Ghe g WHERE g.MaKV = kv.MaKV AND g.TrangThai = N'DaBan') as DaBan,
                           (SELECT COUNT(*) FROM Ghe g WHERE g.MaKV = kv.MaKV AND g.TrangThai = N'DangGiu') as DangGiu,
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
            EnsureSchemaAndSyncSeats();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
                    SELECT kv.MaKV, kv.MaSuat, ISNULL(kv.MaVung, '') AS MaVung, kv.TenKV, kv.HangVe, kv.TongSoGhe, kv.GiaVe, kv.ChoPhepBanVe,
                           (SELECT COUNT(*) FROM Ghe g WHERE g.MaKV = kv.MaKV AND g.TrangThai = N'DaBan') as DaBan,
                           (SELECT COUNT(*) FROM Ghe g WHERE g.MaKV = kv.MaKV AND g.TrangThai = N'DangGiu') as DangGiu,
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
                                MaVung = reader["MaVung"]?.ToString() ?? "",
                                TenKV = reader["TenKV"].ToString(),
                                HangVe = reader["HangVe"].ToString(),
                                TongSoGhe = Convert.ToInt32(reader["TongSoGhe"]),
                                GiaVe = Convert.ToDecimal(reader["GiaVe"]),
                                ChoPhepBanVe = Convert.ToBoolean(reader["ChoPhepBanVe"]),
                                DaBan = Convert.ToInt32(reader["DaBan"]),
                                DangGiu = Convert.ToInt32(reader["DangGiu"]),
                                ConLai = Convert.ToInt32(reader["ConLai"])
                            };
                        }
                    }
                }
            }
            return null;
        }

        public static bool KiemTraMaVungTonTai(int maSuat, string maVung, int excludeMaKV = 0)
        {
            if (string.IsNullOrWhiteSpace(maVung)) return false;
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"SELECT COUNT(*) FROM KhuVuc 
                                 WHERE MaSuat = @MaSuat AND MaVung = @MaVung AND MaKV <> @ExcludeMaKV";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaSuat", maSuat);
                    cmd.Parameters.AddWithValue("@MaVung", maVung.Trim());
                    cmd.Parameters.AddWithValue("@ExcludeMaKV", excludeMaKV);
                    conn.Open();
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        public static int Them(KhuVucDTO kv)
        {
            EnsureSchemaAndSyncSeats();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        const string sqlKV = @"
                            INSERT INTO KhuVuc (MaSuat, MaVung, TenKV, HangVe, GiaVe, TongSoGhe, ChoPhepBanVe)
                            OUTPUT INSERTED.MaKV
                            VALUES (@MaSuat, @MaVung, @TenKV, @HangVe, @GiaVe, @TongSoGhe, @ChoPhepBanVe)";

                        int newMaKV;
                        using (SqlCommand cmdKV = new SqlCommand(sqlKV, conn, trans))
                        {
                            cmdKV.Parameters.AddWithValue("@MaSuat", kv.MaSuat);
                            cmdKV.Parameters.AddWithValue("@MaVung", (object)kv.MaVung ?? DBNull.Value);
                            cmdKV.Parameters.AddWithValue("@TenKV", kv.TenKV);
                            cmdKV.Parameters.AddWithValue("@HangVe", kv.HangVe ?? "VIP");
                            cmdKV.Parameters.AddWithValue("@GiaVe", kv.GiaVe);
                            cmdKV.Parameters.AddWithValue("@TongSoGhe", kv.TongSoGhe);
                            cmdKV.Parameters.AddWithValue("@ChoPhepBanVe", kv.ChoPhepBanVe);
                            newMaKV = (int)cmdKV.ExecuteScalar();
                        }

                        // Sinh ghế đa hàng chuẩn A, B, C... (10 ghế mỗi hàng; hỗ trợ mã hàng mở rộng AA, AB... khi vượt quá 260 ghế)
                        int colPerRow = 10;
                        if (kv.TongSoGhe > 260)
                            colPerRow = (int)Math.Ceiling(kv.TongSoGhe / 26.0);

                        const string sqlGhe = @"
                            WITH Nbrs AS (
                                SELECT TOP (@TongSoGhe) (ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1) AS Idx
                                FROM sys.all_columns a CROSS JOIN sys.all_columns b
                            )
                            INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai)
                            SELECT @MaKV,
                                   CASE 
                                       WHEN (Idx / @ColPerRow) < 26 THEN CHAR(65 + (Idx / @ColPerRow))
                                       ELSE CHAR(65 + ((Idx / @ColPerRow) / 26 - 1)) + CHAR(65 + ((Idx / @ColPerRow) % 26))
                                   END,
                                   (Idx % @ColPerRow) + 1,
                                   N'Trong'
                            FROM Nbrs";

                        using (SqlCommand cmdGhe = new SqlCommand(sqlGhe, conn, trans))
                        {
                            cmdGhe.Parameters.AddWithValue("@MaKV", newMaKV);
                            cmdGhe.Parameters.AddWithValue("@TongSoGhe", kv.TongSoGhe);
                            cmdGhe.Parameters.AddWithValue("@ColPerRow", colPerRow);
                            cmdGhe.ExecuteNonQuery();
                        }

                        // Kiểm tra tổng ghế thực tế trong CSDL trước khi commit transaction
                        int finalCount = 0;
                        using (SqlCommand cmdVerify = new SqlCommand("SELECT COUNT(*) FROM Ghe WHERE MaKV = @MaKV", conn, trans))
                        {
                            cmdVerify.Parameters.AddWithValue("@MaKV", newMaKV);
                            finalCount = (int)cmdVerify.ExecuteScalar();
                        }
                        if (finalCount != kv.TongSoGhe)
                        {
                            trans.Rollback();
                            throw new InvalidOperationException($"Lỗi sinh ghế: Chỉ sinh được {finalCount}/{kv.TongSoGhe} ghế!");
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

        public static bool Sua(KhuVucDTO kv, out string errorMessage)
        {
            errorMessage = "";
            EnsureSchemaAndSyncSeats();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Lấy thông tin sức chứa hiện tại và số ghế trong CSDL
                        int oldTongSoGhe = 0;
                        using (SqlCommand cmdCheck = new SqlCommand("SELECT TongSoGhe FROM KhuVuc WHERE MaKV = @MaKV", conn, trans))
                        {
                            cmdCheck.Parameters.AddWithValue("@MaKV", kv.MaKV);
                            object result = cmdCheck.ExecuteScalar();
                            if (result == null)
                            {
                                errorMessage = "Khu vực không tồn tại.";
                                trans.Rollback();
                                return false;
                            }
                            oldTongSoGhe = Convert.ToInt32(result);
                        }

                        // 2. Cập nhật thông tin KhuVuc
                        const string sqlUpdateKV = @"
                            UPDATE KhuVuc
                            SET MaVung = @MaVung, TenKV = @TenKV, HangVe = @HangVe, GiaVe = @GiaVe, 
                                TongSoGhe = @TongSoGhe, ChoPhepBanVe = @ChoPhepBanVe
                            WHERE MaKV = @MaKV";

                        using (SqlCommand cmdUpdate = new SqlCommand(sqlUpdateKV, conn, trans))
                        {
                            cmdUpdate.Parameters.AddWithValue("@MaVung", (object)kv.MaVung ?? DBNull.Value);
                            cmdUpdate.Parameters.AddWithValue("@TenKV", kv.TenKV);
                            cmdUpdate.Parameters.AddWithValue("@HangVe", kv.HangVe ?? "VIP");
                            cmdUpdate.Parameters.AddWithValue("@GiaVe", kv.GiaVe);
                            cmdUpdate.Parameters.AddWithValue("@TongSoGhe", kv.TongSoGhe);
                            cmdUpdate.Parameters.AddWithValue("@ChoPhepBanVe", kv.ChoPhepBanVe);
                            cmdUpdate.Parameters.AddWithValue("@MaKV", kv.MaKV);
                            cmdUpdate.ExecuteNonQuery();
                        }

                        // 3. Nếu Sức chứa thay đổi -> xử lý đồng bộ ghế an toàn
                        if (kv.TongSoGhe != oldTongSoGhe)
                        {
                            int soVeDaPhatHanh = 0;
                            using (SqlCommand cmdVe = new SqlCommand(
                                "SELECT COUNT(*) FROM ChiTietVe ctv INNER JOIN Ghe g ON ctv.MaGhe = g.MaGhe WHERE g.MaKV = @MaKV",
                                conn, trans))
                            {
                                cmdVe.Parameters.AddWithValue("@MaKV", kv.MaKV);
                                soVeDaPhatHanh = (int)cmdVe.ExecuteScalar();
                            }

                            int soGheDangBanHoacGiu = 0;
                            using (SqlCommand cmdTrangThai = new SqlCommand(
                                "SELECT COUNT(*) FROM Ghe WHERE MaKV = @MaKV AND TrangThai IN (N'DaBan', N'DangGiu')",
                                conn, trans))
                            {
                                cmdTrangThai.Parameters.AddWithValue("@MaKV", kv.MaKV);
                                soGheDangBanHoacGiu = (int)cmdTrangThai.ExecuteScalar();
                            }

                            // Giữ ổn định số cột mỗi hàng theo cấu trúc ghế hiện có (tránh làm lệch vị trí khi tăng/giảm sức chứa)
                            int maxExistingCot = 0;
                            using (SqlCommand cmdMaxCol = new SqlCommand("SELECT ISNULL(MAX(Cot), 0) FROM Ghe WHERE MaKV = @MaKV", conn, trans))
                            {
                                cmdMaxCol.Parameters.AddWithValue("@MaKV", kv.MaKV);
                                maxExistingCot = Convert.ToInt32(cmdMaxCol.ExecuteScalar());
                            }
                            int colPerRow = maxExistingCot > 0 ? maxExistingCot : 10;
                            if (maxExistingCot == 0 && kv.TongSoGhe > 260)
                                colPerRow = (int)Math.Ceiling(kv.TongSoGhe / 26.0);

                            // Trường hợp 1: Khu vực hoàn toàn chưa có vé và chưa có ghế đang giữ -> Tái tạo sạch ma trận ghế
                            if (soVeDaPhatHanh == 0 && soGheDangBanHoacGiu == 0)
                            {
                                using (SqlCommand cmdDel = new SqlCommand("DELETE FROM Ghe WHERE MaKV = @MaKV", conn, trans))
                                {
                                    cmdDel.Parameters.AddWithValue("@MaKV", kv.MaKV);
                                    cmdDel.ExecuteNonQuery();
                                }

                                const string sqlRebuild = @"
                                    WITH Nbrs AS (
                                        SELECT TOP (@TongSoGhe) (ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1) AS Idx
                                        FROM sys.all_columns a CROSS JOIN sys.all_columns b
                                    )
                                    INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai)
                                    SELECT @MaKV,
                                           CASE 
                                               WHEN (Idx / @ColPerRow) < 26 THEN CHAR(65 + (Idx / @ColPerRow))
                                               ELSE CHAR(65 + ((Idx / @ColPerRow) / 26 - 1)) + CHAR(65 + ((Idx / @ColPerRow) % 26))
                                           END,
                                           (Idx % @ColPerRow) + 1,
                                           N'Trong'
                                    FROM Nbrs";

                                using (SqlCommand cmdGen = new SqlCommand(sqlRebuild, conn, trans))
                                {
                                    cmdGen.Parameters.AddWithValue("@MaKV", kv.MaKV);
                                    cmdGen.Parameters.AddWithValue("@TongSoGhe", kv.TongSoGhe);
                                    cmdGen.Parameters.AddWithValue("@ColPerRow", colPerRow);
                                    cmdGen.ExecuteNonQuery();
                                }
                            }
                            else if (kv.TongSoGhe > oldTongSoGhe)
                            {
                                // Trường hợp 2: Tăng sức chứa khi đã có vé/ghế giữ -> Sinh bổ sung đủ các vị trí ghế còn thiếu (kể cả có khoảng trống)
                                int currentCount = 0;
                                using (SqlCommand cmdCount = new SqlCommand("SELECT COUNT(*) FROM Ghe WHERE MaKV = @MaKV", conn, trans))
                                {
                                    cmdCount.Parameters.AddWithValue("@MaKV", kv.MaKV);
                                    currentCount = (int)cmdCount.ExecuteScalar();
                                }

                                int missingCount = kv.TongSoGhe - currentCount;
                                if (missingCount > 0)
                                {
                                    const string sqlAddMore = @"
                                        WITH Numbers AS (
                                            SELECT TOP (10000) (ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1) AS Idx
                                            FROM sys.all_columns a CROSS JOIN sys.all_columns b
                                        ),
                                        Candidates AS (
                                            SELECT 
                                                @MaKV AS MaKV,
                                                CASE 
                                                    WHEN (Idx / @ColPerRow) < 26 THEN CHAR(65 + (Idx / @ColPerRow))
                                                    ELSE CHAR(65 + ((Idx / @ColPerRow) / 26 - 1)) + CHAR(65 + ((Idx / @ColPerRow) % 26))
                                                END AS Hang,
                                                (Idx % @ColPerRow) + 1 AS Cot,
                                                Idx
                                            FROM Numbers
                                        ),
                                        MissingCandidates AS (
                                            SELECT c.MaKV, c.Hang, c.Cot, c.Idx
                                            FROM Candidates c
                                            WHERE NOT EXISTS (
                                                SELECT 1 FROM Ghe g 
                                                WHERE g.MaKV = @MaKV AND g.Hang = c.Hang AND g.Cot = c.Cot
                                            )
                                        )
                                        INSERT INTO Ghe (MaKV, Hang, Cot, TrangThai)
                                        SELECT TOP (@MissingCount) mc.MaKV, mc.Hang, mc.Cot, N'Trong'
                                        FROM MissingCandidates mc
                                        ORDER BY mc.Idx ASC;";

                                    using (SqlCommand cmdAdd = new SqlCommand(sqlAddMore, conn, trans))
                                    {
                                        cmdAdd.Parameters.AddWithValue("@MaKV", kv.MaKV);
                                        cmdAdd.Parameters.AddWithValue("@MissingCount", missingCount);
                                        cmdAdd.Parameters.AddWithValue("@ColPerRow", colPerRow);
                                        cmdAdd.ExecuteNonQuery();
                                    }
                                }
                            }
                            else
                            {
                                // Trường hợp 3: Giảm sức chứa -> Chỉ xóa ghế trống chưa từng có vé
                                int delta = oldTongSoGhe - kv.TongSoGhe;

                                int safeCount = 0;
                                using (SqlCommand cmdSafe = new SqlCommand(@"
                                    SELECT COUNT(*) FROM Ghe g
                                    WHERE g.MaKV = @MaKV 
                                      AND g.TrangThai = N'Trong'
                                      AND NOT EXISTS (SELECT 1 FROM ChiTietVe ctv WHERE ctv.MaGhe = g.MaGhe)", conn, trans))
                                {
                                    cmdSafe.Parameters.AddWithValue("@MaKV", kv.MaKV);
                                    safeCount = (int)cmdSafe.ExecuteScalar();
                                }

                                if (safeCount < delta)
                                {
                                    errorMessage = $"Không thể giảm sức chứa xuống {kv.TongSoGhe}! Khu vực này có {soGheDangBanHoacGiu} ghế đang giữ/đã bán và {soVeDaPhatHanh} vé có lịch sử, chỉ có thể giảm tối đa {safeCount} ghế trống an toàn.";
                                    trans.Rollback();
                                    return false;
                                }

                                const string sqlDeleteSafe = @"
                                    DELETE FROM Ghe
                                    WHERE MaGhe IN (
                                        SELECT TOP (@Delta) g.MaGhe
                                        FROM Ghe g
                                        WHERE g.MaKV = @MaKV 
                                          AND g.TrangThai = N'Trong'
                                          AND NOT EXISTS (SELECT 1 FROM ChiTietVe ctv WHERE ctv.MaGhe = g.MaGhe)
                                        ORDER BY g.Hang DESC, g.Cot DESC
                                    )";

                                using (SqlCommand cmdDelSafe = new SqlCommand(sqlDeleteSafe, conn, trans))
                                {
                                    cmdDelSafe.Parameters.AddWithValue("@MaKV", kv.MaKV);
                                    cmdDelSafe.Parameters.AddWithValue("@Delta", delta);
                                    cmdDelSafe.ExecuteNonQuery();
                                }
                            }
                        }

                        // 4. Kiểm tra đối soát tổng ghế thực tế trong CSDL trước khi commit transaction
                        int finalGheCount = 0;
                        using (SqlCommand cmdVerify = new SqlCommand("SELECT COUNT(*) FROM Ghe WHERE MaKV = @MaKV", conn, trans))
                        {
                            cmdVerify.Parameters.AddWithValue("@MaKV", kv.MaKV);
                            finalGheCount = (int)cmdVerify.ExecuteScalar();
                        }

                        if (finalGheCount != kv.TongSoGhe)
                        {
                            errorMessage = $"Lỗi đồng bộ ghế: Số ghế thực tế trong CSDL ({finalGheCount}) không khớp với sức chứa yêu cầu ({kv.TongSoGhe})!";
                            trans.Rollback();
                            return false;
                        }

                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        errorMessage = "Lỗi khi cập nhật khu vực: " + ex.Message;
                        return false;
                    }
                }
            }
        }

        public static bool Sua(KhuVucDTO kv)
        {
            return Sua(kv, out _);
        }

        public static bool Xoa(int maKV)
        {
            EnsureSchemaAndSyncSeats();
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
