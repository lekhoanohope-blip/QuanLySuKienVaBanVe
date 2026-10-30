using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QuanLySuKienVaBanVe.DAL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.BLL
{
    /// <summary>
    /// Service cung cấp dữ liệu mẫu (Mock Data) độc lập, tự động fallback khi CSDL chưa có dữ liệu
    /// hoặc khi module Admin chưa hoàn thiện, giúp kiểm thử toàn bộ luồng nghiệp vụ Bán vé và Thanh toán.
    /// </summary>
    public static class MockDataService
    {
        public static bool ForceMockData { get; set; } = false;

        #region Kiểm tra kết nối CSDL
        public static bool KiemTraKetNoiCSDL()
        {
            if (ForceMockData) return false;
            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 1. Dữ liệu Sự kiện (SuKien)
        public static DataTable LayDanhSachSuKien()
        {
            if (!ForceMockData && KiemTraKetNoiCSDL())
            {
                try
                {
                    DataTable dt = SuKienBLL.LayDanhSach();
                    if (dt != null && dt.Rows.Count > 0)
                        return dt;
                }
                catch { }
            }

            // Trả về Mock Data chuẩn
            DataTable mock = new DataTable("SuKien");
            mock.Columns.Add("MaSK", typeof(int));
            mock.Columns.Add("TenSK", typeof(string));
            mock.Columns.Add("LoaiSK", typeof(string));
            mock.Columns.Add("MoTa", typeof(string));
            mock.Columns.Add("TrangThai", typeof(string));

            mock.Rows.Add(1, "Live Concert: HCMUE Melody 2026", "Âm nhạc", "Đêm nhạc sinh viên Đại học Sư phạm TP.HCM với sự tham gia của các nghệ sĩ khách mời hàng đầu.", "DangDien");
            mock.Rows.Add(2, "[TV3-Test] Đêm Nhạc Acoustic Mùa Thu", "Live Acoustic", "Chương trình ca nhạc ấm áp, giao lưu acoustic định kỳ tháng 10.", "DangDien");
            mock.Rows.Add(3, "Đại Nhạc Hội: HCMUE Student Fest", "Lễ hội âm nhạc", "Lễ hội chào đón tân sinh viên với sân khấu ngoài trời quy mô lớn.", "DangDien");

            return mock;
        }
        #endregion

        #region 2. Dữ liệu Suất diễn (SuatDien)
        public static DataTable LayDanhSachSuatDien(int maSK)
        {
            if (!ForceMockData && KiemTraKetNoiCSDL())
            {
                try
                {
                    DataTable dt = SuatDienBLL.LayDanhSachTheoSuKien(maSK);
                    if (dt != null && dt.Rows.Count > 0)
                        return dt;
                }
                catch { }
            }

            // Mock Data suất diễn cho sự kiện được chọn
            DataTable mock = new DataTable("SuatDien");
            mock.Columns.Add("MaSuat", typeof(int));
            mock.Columns.Add("MaSK", typeof(int));
            mock.Columns.Add("ThoiGianBatDau", typeof(DateTime));
            mock.Columns.Add("ThoiGianKetThuc", typeof(DateTime));
            mock.Columns.Add("DiaDiem", typeof(string));
            mock.Columns.Add("LoaiSoDo", typeof(int));
            mock.Columns.Add("TenSoDo", typeof(string));
            mock.Columns.Add("TrangThai", typeof(string));
            mock.Columns.Add("HienThiSuat", typeof(string));

            DateTime now = DateTime.Now;

            if (maSK == 1)
            {
                DateTime t1 = new DateTime(now.Year, now.Month, Math.Min(28, now.Day), 19, 30, 0);
                DateTime t2 = t1.AddDays(1).Date.AddHours(20);
                DateTime t3 = t1.AddDays(2).Date.AddHours(18);

                mock.Rows.Add(101, 1, t1, t1.AddHours(3), "Hội trường A - HCMUE (280 An Dương Vương, Q.5)", 1, "Sơ đồ 1 (Chữ T)", "MoCuaBanVe",
                    $"Suất 1: {t1:dd/MM/yyyy HH:mm} - Sơ đồ 1 (Chữ T) - Hội trường A");
                mock.Rows.Add(102, 1, t2, t2.AddHours(3), "Hội trường A - HCMUE (280 An Dương Vương, Q.5)", 2, "Sơ đồ 2 (Hình chữ nhật)", "MoCuaBanVe",
                    $"Suất 2: {t2:dd/MM/yyyy HH:mm} - Sơ đồ 2 (Hình chữ nhật) - Hội trường A");
                mock.Rows.Add(103, 1, t3, t3.AddHours(3), "Hội trường A - HCMUE (280 An Dương Vương, Q.5)", 3, "Sơ đồ 3 (Lưới)", "MoCuaBanVe",
                    $"Suất 3: {t3:dd/MM/yyyy HH:mm} - Sơ đồ 3 (Lưới) - Hội trường A");
            }
            else if (maSK == 2)
            {
                DateTime t1 = new DateTime(now.Year, now.Month, Math.Min(28, now.Day), 20, 0, 0);
                DateTime t2 = t1.AddDays(1).Date.AddHours(19).AddMinutes(30);

                mock.Rows.Add(201, 2, t1, t1.AddHours(2.5), "Phòng Trà Acoustic Riverside (Q.1, TP.HCM)", 2, "Sơ đồ 2 (Hình chữ nhật)", "MoCuaBanVe",
                    $"Suất 1: {t1:dd/MM/yyyy HH:mm} - Sơ đồ 2 (Hình chữ nhật) - Phòng Trà Riverside");
                mock.Rows.Add(202, 2, t2, t2.AddHours(2.5), "Phòng Trà Acoustic Riverside (Q.1, TP.HCM)", 1, "Sơ đồ 1 (Chữ T)", "MoCuaBanVe",
                    $"Suất 2: {t2:dd/MM/yyyy HH:mm} - Sơ đồ 1 (Chữ T) - Phòng Trà Riverside");
            }
            else
            {
                DateTime t1 = new DateTime(now.Year, now.Month, Math.Min(28, now.Day), 18, 30, 0);
                mock.Rows.Add(301, 3, t1, t1.AddHours(4), "Sân khấu ngoài trời - Khuôn viên HCMUE", 3, "Sơ đồ 3 (Lưới)", "MoCuaBanVe",
                    $"Suất 1: {t1:dd/MM/yyyy HH:mm} - Sơ đồ 3 (Lưới) - Sân khấu ngoài trời");
            }

            return mock;
        }
        #endregion

        #region 3. Dữ liệu Khu vực & Giá vé (KhuVuc)
        public static DataTable LayDanhSachKhuVuc(int maSuat, int loaiSoDo)
        {
            if (!ForceMockData && KiemTraKetNoiCSDL())
            {
                try
                {
                    DataTable dt = KhuVucBLL.LayDanhSachTheoSuat(maSuat);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        // Kiểm tra nếu có đầy đủ các cột cần thiết
                        if (dt.Columns.Contains("DaBan") && dt.Columns.Contains("ConLai"))
                            return dt;

                        // Nếu thiếu thì tính toán bổ sung
                        if (!dt.Columns.Contains("DaBan")) dt.Columns.Add("DaBan", typeof(int));
                        if (!dt.Columns.Contains("ConLai")) dt.Columns.Add("ConLai", typeof(int));
                        foreach (DataRow r in dt.Rows)
                        {
                            int tong = r["TongSoGhe"] != DBNull.Value ? Convert.ToInt32(r["TongSoGhe"]) : 50;
                            int daban = r["DaBan"] != DBNull.Value ? Convert.ToInt32(r["DaBan"]) : 0;
                            r["DaBan"] = daban;
                            r["ConLai"] = Math.Max(0, tong - daban);
                        }
                        return dt;
                    }
                }
                catch { }
            }

            // Tạo Mock Data theo đúng loại sơ đồ (1: Chữ T, 2: Hình chữ nhật, 3: Lưới)
            DataTable mock = new DataTable("KhuVuc");
            mock.Columns.Add("MaKV", typeof(int));
            mock.Columns.Add("MaSuat", typeof(int));
            mock.Columns.Add("MaVung", typeof(string));
            mock.Columns.Add("TenKV", typeof(string));
            mock.Columns.Add("HangVe", typeof(string));
            mock.Columns.Add("GiaVe", typeof(decimal));
            mock.Columns.Add("TongSoGhe", typeof(int));
            mock.Columns.Add("DaBan", typeof(int));
            mock.Columns.Add("ConLai", typeof(int));
            mock.Columns.Add("ChoPhepBanVe", typeof(bool));

            int idSeed = maSuat * 10;

            if (loaiSoDo == 1) // Sơ đồ 1: Sân khấu Chữ T
            {
                mock.Rows.Add(idSeed + 1, maSuat, "VIP_A", "VIP A", "VIP", 500000m, 50, 4, 46, true);
                mock.Rows.Add(idSeed + 2, maSuat, "VIP_B", "VIP B", "VIP", 500000m, 50, 2, 48, true);
                mock.Rows.Add(idSeed + 3, maSuat, "GA_A", "GA A", "Phổ thông", 250000m, 100, 10, 90, true);
                mock.Rows.Add(idSeed + 4, maSuat, "GA_B", "GA B", "Phổ thông", 250000m, 100, 5, 95, true);
                mock.Rows.Add(idSeed + 5, maSuat, "CAT", "CAT", "Thường", 150000m, 150, 15, 135, true);
            }
            else if (loaiSoDo == 2) // Sơ đồ 2: Hình chữ nhật
            {
                mock.Rows.Add(idSeed + 1, maSuat, "GA_A", "GA A", "Phổ thông", 250000m, 60, 5, 55, true);
                mock.Rows.Add(idSeed + 2, maSuat, "VIP_B", "VIP B", "VIP", 500000m, 40, 3, 37, true);
                mock.Rows.Add(idSeed + 3, maSuat, "VIP_A", "VIP A", "VIP", 500000m, 60, 6, 54, true);
                mock.Rows.Add(idSeed + 4, maSuat, "VIP_C", "VIP C", "VIP", 500000m, 40, 2, 38, true);
                mock.Rows.Add(idSeed + 5, maSuat, "GA_B", "GA B", "Phổ thông", 250000m, 60, 4, 56, true);
                mock.Rows.Add(idSeed + 6, maSuat, "GA_C", "GA C", "Phổ thông", 250000m, 70, 8, 62, true);
                mock.Rows.Add(idSeed + 7, maSuat, "VIP_D", "VIP D", "VIP", 500000m, 60, 4, 56, true);
                mock.Rows.Add(idSeed + 8, maSuat, "GA_D", "GA D", "Phổ thông", 250000m, 70, 5, 65, true);
                mock.Rows.Add(idSeed + 9, maSuat, "CAT_B", "CAT B", "Thường", 200000m, 80, 10, 70, true);
                mock.Rows.Add(idSeed + 10, maSuat, "GA_E", "GA E", "Phổ thông", 250000m, 60, 3, 57, true);
                mock.Rows.Add(idSeed + 11, maSuat, "CAT_C", "CAT C", "Thường", 200000m, 80, 7, 73, true);
                mock.Rows.Add(idSeed + 12, maSuat, "CAT_A", "CAT A", "Thường", 200000m, 70, 4, 66, true);
            }
            else // Sơ đồ 3: Lưới
            {
                mock.Rows.Add(idSeed + 1, maSuat, "VIP_A", "VIP A", "VIP", 600000m, 40, 4, 36, true);
                mock.Rows.Add(idSeed + 2, maSuat, "VIP_B", "VIP B", "VIP", 600000m, 40, 2, 38, true);
                mock.Rows.Add(idSeed + 3, maSuat, "VIP_C", "VIP C", "VIP", 600000m, 40, 3, 37, true);
                mock.Rows.Add(idSeed + 4, maSuat, "VIP_D", "VIP D", "VIP", 600000m, 40, 5, 35, true);

                mock.Rows.Add(idSeed + 5, maSuat, "GA_A", "GA A", "Phổ thông", 300000m, 50, 6, 44, true);
                mock.Rows.Add(idSeed + 6, maSuat, "GA_B", "GA B", "Phổ thông", 300000m, 50, 5, 45, true);
                mock.Rows.Add(idSeed + 7, maSuat, "GA_C", "GA C", "Phổ thông", 300000m, 50, 4, 46, true);
                mock.Rows.Add(idSeed + 8, maSuat, "GA_D", "GA D", "Phổ thông", 300000m, 50, 7, 43, true);

                mock.Rows.Add(idSeed + 9, maSuat, "GA_E", "GA E", "Phổ thông", 300000m, 50, 3, 47, true);
                mock.Rows.Add(idSeed + 10, maSuat, "GA_F", "GA F", "Phổ thông", 300000m, 50, 2, 48, true);
                mock.Rows.Add(idSeed + 11, maSuat, "GA_G", "GA G", "Phổ thông", 300000m, 50, 4, 46, true);
                mock.Rows.Add(idSeed + 12, maSuat, "GA_H", "GA H", "Phổ thông", 300000m, 50, 5, 45, true);

                mock.Rows.Add(idSeed + 13, maSuat, "CAT_C", "CAT C", "Thường", 200000m, 70, 8, 62, true);
                mock.Rows.Add(idSeed + 14, maSuat, "CAT_D", "CAT D", "Thường", 200000m, 70, 6, 64, true);
                mock.Rows.Add(idSeed + 15, maSuat, "CAT_A", "CAT A", "Thường", 200000m, 70, 9, 61, true);
                mock.Rows.Add(idSeed + 16, maSuat, "CAT_B", "CAT B", "Thường", 200000m, 70, 7, 63, true);
            }

            return mock;
        }
        #endregion

        #region 4. Dữ liệu Khách hàng (KhachHang)
        public static DataTable LayDanhSachKhachHang()
        {
            if (!ForceMockData && KiemTraKetNoiCSDL())
            {
                try
                {
                    DataTable dt = KhachHangBLL.LayDanhSach();
                    if (dt != null && dt.Rows.Count > 0)
                        return dt;
                }
                catch { }
            }

            DataTable mock = new DataTable("KhachHang");
            mock.Columns.Add("MaKH", typeof(int));
            mock.Columns.Add("TenKH", typeof(string));
            mock.Columns.Add("SDT", typeof(string));
            mock.Columns.Add("Email", typeof(string));

            mock.Rows.Add(1, "Nguyễn Văn An", "0901234567", "an.nguyen@example.com");
            mock.Rows.Add(2, "Trần Thị Mai", "0912345678", "mai.tran@example.com");
            mock.Rows.Add(3, "Lê Hoàng Nam", "0987654321", "nam.le@example.com");
            mock.Rows.Add(4, "Phạm Thu Hà", "0934567890", "ha.pham@example.com");
            mock.Rows.Add(5, "Võ Minh Trí", "0971122334", "tri.vo@example.com");

            return mock;
        }
        #endregion

        #region 5. Logic tự động sinh vị trí ghế (Seat Generation Logic)
        /// <summary>
        /// Thuật toán phân bổ ghế tự động theo đặc tả kỹ thuật:
        /// - Sắp xếp theo hàng ngang từ trái sang phải để lấp đầy khu vực.
        /// - Quy tắc đặt tên ghế: [Tên khu vực] [Số thứ tự ghế] (Ví dụ: VIP A-1, CAT B-10).
        /// - Quy tắc ưu tiên: Khách đặt sớm nhất sẽ được xếp gần sân khấu nhất (số thứ tự nhỏ nhất trong các ghế còn trống).
        /// </summary>
        /// 

        public static List<string> SinhViTriGheTuDong(int maKV, string tenKV, int tongSoGhe, int daBan, int soLuongDat)
        {
            List<string> danhSachGheDuocChon = new List<string>();

            // Bắt đầu cấp số ghế từ vị trí (daBan + 1) vì các ghế trước đã được đặt
            int viTriBatDau = daBan + 1;

            for (int i = 0; i < soLuongDat; i++)
            {
                int soThuTuGhe = viTriBatDau + i;
                if (soThuTuGhe > tongSoGhe) break; // Không vượt quá tổng số ghế của khu vực

                // Định dạng chuẩn theo yêu cầu: [Tên khu vực] - [Số thứ tự 2 chữ số]
                string tenGheChuan = $"{tenKV} - {soThuTuGhe:D2}";

                danhSachGheDuocChon.Add(tenGheChuan);
            }

            return danhSachGheDuocChon;
        } // <-- Kiểm tra kỹ phải có đủ dấu đóng ngoặc này cho hàm
        #endregion
    }
}

        /*
        public static List<string> SinhViTriGheTuDong(int maKV, string tenKV, int tongSoGhe, int daBan, int soLuong)
        {
            List<string> danhSachGhe = new List<string>();
            if (soLuong <= 0) return danhSachGhe;

            // 1. Nếu CSDL sẵn sàng, thử tìm danh sách ghế trống thực tế từ bảng Ghe
            if (!ForceMockData && KiemTraKetNoiCSDL())
            {
                try
                {
                    string sql = @"
                        SELECT TOP (@SoLuong) MaGhe, MaGheHienThi, Hang, Cot
                        FROM Ghe
                        WHERE MaKV = @MaKV AND TrangThai = N'Trong'
                        ORDER BY Hang ASC, Cot ASC;";

                    SqlParameter[] pars = {
                        new SqlParameter("@MaKV", SqlDbType.Int) { Value = maKV },
                        new SqlParameter("@SoLuong", SqlDbType.Int) { Value = soLuong }
                    };

                    DataTable dtGhe = Database.ExecuteQuery(sql, pars);
                    if (dtGhe != null && dtGhe.Rows.Count >= soLuong)
                    {
                        foreach (DataRow r in dtGhe.Rows)
                        {
                            string maHienThi = r["MaGheHienThi"]?.ToString();
                            if (!string.IsNullOrWhiteSpace(maHienThi))
                                danhSachGhe.Add($"{tenKV.Trim()} {maHienThi.Trim()}");
                            else
                                danhSachGhe.Add($"{tenKV.Trim()}-{r["Cot"]}");
                        }
                        return danhSachGhe;
                    }
                }
                catch { }
            }

            // 2. Logic sinh tự động (Virtual / Fallback Algorithm)
            // Khách đặt sớm được cấp số thứ tự nhỏ nhất trong các ghế còn trống
            int batDau = Math.Max(1, daBan + 1);
            for (int i = 0; i < soLuong; i++)
            {
                int soGhe = batDau + i;
                if (tongSoGhe > 0 && soGhe > tongSoGhe)
                {
                    soGhe = tongSoGhe; // Chạm trần
                }
                danhSachGhe.Add($"{tenKV.Trim()}-{soGhe}");
            }

            return danhSachGhe;
        }
        #endregion
    }
}
        */