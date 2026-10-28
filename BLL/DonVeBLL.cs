using System;
using System.Data;
using System.Data.SqlClient;
using QuanLySuKienVaBanVe.DAL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.BLL
{
    /// <summary>
    /// Đối tượng chứa kết quả trả về từ tầng nghiệp vụ DonVeBLL.
    /// Không để GUI phải phân tích chuỗi lỗi thô.
    /// </summary>
    public class DonVeResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int MaDon { get; set; }
        public int? MaDonMoi { get; set; }
        public string MaVeCodeMoi { get; set; }
        public decimal ChenhLech { get; set; }

        public static DonVeResult Ok(string message = "", int maDon = 0, int? maDonMoi = null, string maVeCodeMoi = "", decimal chenhLech = 0m)
        {
            return new DonVeResult
            {
                Success = true,
                Message = message,
                MaDon = maDon,
                MaDonMoi = maDonMoi,
                MaVeCodeMoi = maVeCodeMoi ?? string.Empty,
                ChenhLech = chenhLech
            };
        }

        public static DonVeResult Fail(string message, int maDon = 0)
        {
            return new DonVeResult
            {
                Success = false,
                Message = message,
                MaDon = maDon,
                MaDonMoi = null,
                MaVeCodeMoi = string.Empty,
                ChenhLech = 0m
            };
        }
    }

    /// <summary>
    /// Lớp xử lý nghiệp vụ Đơn vé (BLL)
    /// Đảm bảo quy tắc hủy vé (>= 24h), đổi vé (>= 12h, lưu MaDonGoc, xử lý chênh lệch)
    /// và bảo toàn tính toàn vẹn với SqlTransaction.
    /// </summary>
    public class DonVeBLL
    {
        private static readonly Random RandomGenerator = new Random();

        /// <summary>
        /// Cờ giả lập lỗi phát sinh ngay trước lệnh Commit của transaction.
        /// Sử dụng cho kịch bản kiểm thử (TC3 & KT6) để chứng minh rollback sạch sẽ.
        /// </summary>
        public static bool SimulateFailureBeforeCommit { get; set; }

        public static DataTable LayDanhSachDonVe()
        {
            return DonVeDAL.LayDanhSachDonVe();
        }

        public static DataRow LayThongTinDonVeVaSuatDien(int maDon)
        {
            if (maDon <= 0) return null;
            return DonVeDAL.LayThongTinDonVeVaSuatDien(maDon);
        }

        public static DataTable TimKiemDonVe(string tuKhoa)
        {
            return DonVeDAL.TimKiemDonVe(tuKhoa);
        }

        public static DataTable LayChiTietVeTheoDon(int maDon)
        {
            if (maDon <= 0) return new DataTable();
            return ChiTietVeDAL.LayTheoMaDon(maDon);
        }

        public static DataTable LayThanhToanTheoDon(int maDon)
        {
            if (maDon <= 0) return new DataTable();
            return ThanhToanDAL.LayTheoMaDon(maDon);
        }

        public static DataTable LaySuatDienCungSuKien(int maSK)
        {
            if (maSK <= 0) return new DataTable();
            return DonVeDAL.LaySuatDienCungSuKien(maSK);
        }

        public static DataTable LayDanhSachGheTrongTheoSuat(int maSuat)
        {
            if (maSuat <= 0) return new DataTable();
            return DonVeDAL.LayDanhSachGheTrongTheoSuat(maSuat);
        }

        /// <summary>
        /// Kiểm tra quy định hủy vé:
        /// 1. Đơn vé phải tồn tại và đang ở trạng thái 'DaThanhToan'.
        /// 2. Suất diễn chưa diễn ra.
        /// 3. Thời điểm yêu cầu hủy phải TRƯỚC giờ diễn ít nhất 24 GIỜ.
        /// </summary>
        public static DonVeResult KiemTraDieuKienHuy(int maDon)
        {
            if (maDon <= 0)
            {
                return DonVeResult.Fail("Mã đơn vé không hợp lệ.", maDon);
            }

            DataRow row = DonVeDAL.LayThongTinDonVeVaSuatDien(maDon);
            if (row == null)
            {
                return DonVeResult.Fail($"Không tìm thấy đơn vé #{maDon} trên hệ thống.", maDon);
            }

            string trangThai = row["TrangThai"].ToString();
            if (trangThai != TrangThaiDonVe.DaThanhToan)
            {
                string moTaTrangThai = trangThai == TrangThaiDonVe.DaHuy ? "Đã hủy" : trangThai == TrangThaiDonVe.DaDoi ? "Đã đổi" : trangThai;
                return DonVeResult.Fail($"Đơn vé #{maDon} đang ở trạng thái “{moTaTrangThai}”. Chỉ được hủy đơn vé đang có hiệu lực (“Đã thanh toán”).", maDon);
            }

            DateTime thoiGianBatDau = Convert.ToDateTime(row["ThoiGianBatDau"]);
            TimeSpan thoiGianConLai = thoiGianBatDau - DateTime.Now;

            if (thoiGianConLai.TotalSeconds <= 0)
            {
                return DonVeResult.Fail($"Suất diễn đã bắt đầu lúc {thoiGianBatDau:dd/MM/yyyy HH:mm}. Không thể hủy vé cho suất diễn đã hoặc đang diễn ra.", maDon);
            }

            if (thoiGianConLai.TotalHours < 24.0)
            {
                return DonVeResult.Fail(
                    $"Không đủ điều kiện hủy vé: Suất diễn bắt đầu lúc {thoiGianBatDau:dd/MM/yyyy HH:mm} (còn {thoiGianConLai.TotalHours:F1} giờ nữa).\nTheo quy định, yêu cầu hủy vé phải được thực hiện trước giờ diễn ít nhất 24 giờ.",
                    maDon);
            }

            return DonVeResult.Ok("Đơn vé đủ điều kiện hủy theo quy định.", maDon);
        }

        /// <summary>
        /// Kiểm tra quy định đổi vé:
        /// 1. Đơn vé phải tồn tại và đang ở trạng thái 'DaThanhToan'.
        /// 2. Suất diễn chưa diễn ra.
        /// 3. Thời điểm yêu cầu đổi phải TRƯỚC giờ diễn ít nhất 12 GIỜ.
        /// </summary>
        public static DonVeResult KiemTraDieuKienDoi(int maDon)
        {
            if (maDon <= 0)
            {
                return DonVeResult.Fail("Mã đơn vé không hợp lệ.", maDon);
            }

            DataRow row = DonVeDAL.LayThongTinDonVeVaSuatDien(maDon);
            if (row == null)
            {
                return DonVeResult.Fail($"Không tìm thấy đơn vé #{maDon} trên hệ thống.", maDon);
            }

            string trangThai = row["TrangThai"].ToString();
            if (trangThai != TrangThaiDonVe.DaThanhToan)
            {
                string moTaTrangThai = trangThai == TrangThaiDonVe.DaHuy ? "Đã hủy" : trangThai == TrangThaiDonVe.DaDoi ? "Đã đổi" : trangThai;
                return DonVeResult.Fail($"Đơn vé #{maDon} đang ở trạng thái “{moTaTrangThai}”. Chỉ được đổi đơn vé đang có hiệu lực (“Đã thanh toán”).", maDon);
            }

            DateTime thoiGianBatDau = Convert.ToDateTime(row["ThoiGianBatDau"]);
            TimeSpan thoiGianConLai = thoiGianBatDau - DateTime.Now;

            if (thoiGianConLai.TotalSeconds <= 0)
            {
                return DonVeResult.Fail($"Suất diễn đã bắt đầu lúc {thoiGianBatDau:dd/MM/yyyy HH:mm}. Không thể đổi vé cho suất diễn đã hoặc đang diễn ra.", maDon);
            }

            if (thoiGianConLai.TotalHours < 12.0)
            {
                return DonVeResult.Fail(
                    $"Không đủ điều kiện đổi vé: Suất diễn bắt đầu lúc {thoiGianBatDau:dd/MM/yyyy HH:mm} (còn {thoiGianConLai.TotalHours:F1} giờ nữa).\nTheo quy định, yêu cầu đổi vé phải được thực hiện trước giờ diễn ít nhất 12 giờ.",
                    maDon);
            }

            return DonVeResult.Ok("Đơn vé đủ điều kiện đổi theo quy định.", maDon);
        }

        /// <summary>
        /// Tính tiền chênh lệch khi đổi vé (Delta = GiaVeMoi - GiaVeCu).
        /// Delta > 0: Khách trả thêm.
        /// Delta = 0: Đổi ngang không thu thêm.
        /// Delta &lt; 0: Hoàn lại tiền cho khách (theo Phương án A).
        /// </summary>
        public static decimal TinhChenhLech(decimal giaCu, decimal giaMoi)
        {
            return giaMoi - giaCu;
        }

        /// <summary>
        /// Sinh mã vé duy nhất theo cấu trúc chuẩn: SK{MaSK:D2}-SD{MaSuat:D2}-{MaGheHienThi}-{Random4}
        /// Đảm bảo không bị trùng lặp trong bảng ChiTietVe.
        /// </summary>
        public static string SinhMaVeCode(int maSK, int maSuat, string maGheHienThi, SqlConnection conn = null, SqlTransaction tran = null)
        {
            string code;
            int attempts = 0;
            do
            {
                int randomNum = RandomGenerator.Next(1000, 9999);
                code = string.Format("SK{0:D2}-SD{1:D2}-{2}-{3}", maSK, maSuat, (maGheHienThi ?? "SEAT").Trim().ToUpper(), randomNum);
                attempts++;
                if (attempts > 50)
                {
                    code = string.Format("SK{0:D2}-SD{1:D2}-{2}-{3}-{4}", maSK, maSuat, (maGheHienThi ?? "SEAT").Trim().ToUpper(), DateTime.Now.Ticks % 10000, randomNum);
                    break;
                }
            } while (ChiTietVeDAL.KiemTraMaVeCodeTonTai(code, conn, tran));

            return code;
        }

        /// <summary>
        /// Thực thi hủy đơn vé an toàn trong 1 SqlTransaction nguyên tử:
        /// 1. Kiểm tra lại điều kiện hủy với khóa dòng.
        /// 2. UPDATE DonVe -> 'DaHuy'.
        /// 3. UPDATE ChiTietVe -> 'DaHuy'.
        /// 4. UPDATE Ghe -> 'Trong', ThoiGianGiu = NULL.
        /// 5. INSERT NhatKy kiểm toán.
        /// Lỗi bất kỳ lập tức Rollback và ném ngoại lệ gốc, không nuốt lỗi.
        /// </summary>
        public static DonVeResult HuyVe(int maDon, int maTK = 0)
        {
            if (maTK <= 0 && PhienDangNhap.DaDangNhap)
            {
                maTK = PhienDangNhap.TaiKhoan.MaTK;
            }

            DonVeResult check = KiemTraDieuKienHuy(maDon);
            if (!check.Success)
            {
                return check;
            }

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Khóa và kiểm tra lại trạng thái đơn vé trong transaction
                        DataRow row = DonVeDAL.LayThongTinDonVeVaSuatDien(maDon, conn, tran);
                        if (row == null)
                        {
                            tran.Rollback();
                            return DonVeResult.Fail($"Không tìm thấy đơn vé #{maDon} trong phiên giao dịch.", maDon);
                        }

                        if (row["TrangThai"].ToString() != TrangThaiDonVe.DaThanhToan)
                        {
                            tran.Rollback();
                            return DonVeResult.Fail($"Đơn vé #{maDon} đã bị thay đổi trạng thái bởi một phiên làm việc khác.", maDon);
                        }

                        DateTime thoiGianBatDau = Convert.ToDateTime(row["ThoiGianBatDau"]);
                        if ((thoiGianBatDau - DateTime.Now).TotalHours < 24.0)
                        {
                            tran.Rollback();
                            return DonVeResult.Fail("Thời gian đến giờ diễn hiện đã dưới 24 giờ, không thể hủy vé.", maDon);
                        }

                        // 2. Chuyển trạng thái đơn vé sang 'DaHuy'
                        DonVeDAL.CapNhatTrangThai(maDon, TrangThaiDonVe.DaHuy, conn, tran);

                        // 3. Giải phóng ghế của các vé đang 'HopLe' trong đơn về trạng thái 'Trong'
                        // (Thực hiện trước khi đổi trạng thái ChiTietVe sang 'DaHuy' để chỉ giải phóng đúng ghế còn hiệu lực, tránh giải phóng ghế đã đổi và bán lại)
                        DonVeDAL.CapNhatTrangThaiGheTheoDon(maDon, "Trong", conn, tran);

                        // 4. Chuyển trạng thái tất cả vé trong đơn sang 'DaHuy'
                        ChiTietVeDAL.CapNhatTrangThaiTheoDon(maDon, TrangThaiChiTietVe.DaHuy, conn, tran);

                        // 5. Ghi nhật ký kiểm toán hệ thống
                        if (maTK > 0)
                        {
                            string chiTiet = $"Hủy đơn vé #{maDon} của khách hàng {row["TenKH"]} (SĐT: {row["SDT"]}), tổng tiền: {Convert.ToDecimal(row["TongTien"]):N0} đ. Toàn bộ ghế trong đơn đã được trả về trạng thái Trống.";
                            DonVeDAL.GhiNhatKy(maTK, $"Hủy đơn vé #{maDon}", chiTiet, conn, tran);
                        }

                        // Kiểm tra cờ giả lập lỗi để test rollback
                        if (SimulateFailureBeforeCommit)
                        {
                            throw new InvalidOperationException("Giả lập lỗi hệ thống trước Commit để chứng minh Rollback giao dịch sạch sẽ.");
                        }

                        tran.Commit();
                        return DonVeResult.Ok($"Đã hủy thành công đơn vé #{maDon}. Toàn bộ ghế trong đơn đã được giải phóng về trạng thái Trống.", maDon);
                    }
                    catch (Exception ex)
                    {
                        try { tran.Rollback(); } catch { }
                        throw new ApplicationException($"Lỗi giao dịch khi hủy đơn vé #{maDon}: {ex.Message}", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Thực thi đổi vé an toàn trong 1 SqlTransaction nguyên tử theo mục 5.4 tài liệu bàn giao:
        /// - Áp dụng Cách 2: Nếu đơn gốc có nhiều vé, chỉ hủy vé được chọn và giảm tổng tiền đơn cũ.
        ///   Đơn cũ chỉ chuyển 'DaDoi' khi toàn bộ vé trong đơn đều đã hủy/đổi.
        /// - Áp dụng Phương án A: Khi đổi sang vé rẻ hơn (Delta &lt; 0), ghi nhận ThanhToan.SoTien = Delta (số âm)
        ///   để ghi nhận khoản tiền hoàn lại cho khách.
        /// </summary>
        public static DonVeResult DoiVe(int maDonCu, int maCTCu, int maGheMoi, int maTK = 0, string phuongThuc = "TienMat")
        {
            if (maTK <= 0 && PhienDangNhap.DaDangNhap)
            {
                maTK = PhienDangNhap.TaiKhoan.MaTK;
            }

            DonVeResult checkDoi = KiemTraDieuKienDoi(maDonCu);
            if (!checkDoi.Success)
            {
                return checkDoi;
            }

            if (maCTCu <= 0 || maGheMoi <= 0)
            {
                return DonVeResult.Fail("Thông tin vé cũ hoặc ghế mới không hợp lệ.");
            }

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Đọc và khóa thông tin đơn vé cũ
                        DataRow rowDonCu = DonVeDAL.LayThongTinDonVeVaSuatDien(maDonCu, conn, tran);
                        if (rowDonCu == null)
                        {
                            tran.Rollback();
                            return DonVeResult.Fail($"Không tìm thấy đơn vé #{maDonCu}.", maDonCu);
                        }

                        if (rowDonCu["TrangThai"].ToString() != TrangThaiDonVe.DaThanhToan)
                        {
                            tran.Rollback();
                            return DonVeResult.Fail($"Đơn vé #{maDonCu} đã thay đổi trạng thái, không thể tiếp tục đổi.", maDonCu);
                        }

                        DateTime thoiGianBatDauCu = Convert.ToDateTime(rowDonCu["ThoiGianBatDau"]);
                        if ((thoiGianBatDauCu - DateTime.Now).TotalHours < 12.0)
                        {
                            tran.Rollback();
                            return DonVeResult.Fail("Thời gian đến giờ diễn của đơn vé cũ hiện đã dưới 12 giờ, không thể đổi.", maDonCu);
                        }

                        int maKH = Convert.ToInt32(rowDonCu["MaKH"]);
                        int maSKCu = Convert.ToInt32(rowDonCu["MaSK"]);

                        // 2. Đọc thông tin vé cũ cần đổi
                        ChiTietVeDTO veCu = ChiTietVeDAL.LayChiTietVeTheoMa(maCTCu, conn, tran);
                        if (veCu == null || veCu.MaDon != maDonCu)
                        {
                            tran.Rollback();
                            return DonVeResult.Fail($"Vé #{maCTCu} không thuộc đơn vé #{maDonCu}.", maDonCu);
                        }

                        if (veCu.TrangThai != TrangThaiChiTietVe.HopLe)
                        {
                            tran.Rollback();
                            return DonVeResult.Fail($"Vé #{maCTCu} đang ở trạng thái '{veCu.TrangThai}', không thể đổi.", maDonCu);
                        }

                        int maGheCu = veCu.MaGhe;
                        decimal giaVeCu = veCu.GiaVe;

                        if (maGheMoi == maGheCu)
                        {
                            tran.Rollback();
                            return DonVeResult.Fail("Vị trí ghế mới trùng với vị trí ghế cũ đang đổi.", maDonCu);
                        }

                        // 3. Đọc và khóa ghế mới WITH (UPDLOCK, ROWLOCK)
                        DataRow rowGheMoi = DonVeDAL.LayThongTinGheKemGia(maGheMoi, conn, tran);
                        if (rowGheMoi == null)
                        {
                            tran.Rollback();
                            return DonVeResult.Fail("Không tìm thấy thông tin vị trí ghế mới trong hệ thống.", maDonCu);
                        }

                        string trangThaiGheMoi = rowGheMoi["TrangThai"].ToString();
                        if (trangThaiGheMoi != "Trong")
                        {
                            tran.Rollback();
                            return DonVeResult.Fail($"Vị trí ghế mới ({rowGheMoi["MaGheHienThi"]}) hiện không còn trống (trạng thái: {trangThaiGheMoi}). Vui lòng chọn ghế khác.", maDonCu);
                        }

                        int maSKMoi = Convert.ToInt32(rowGheMoi["MaSK"]);
                        int maSuatMoi = Convert.ToInt32(rowGheMoi["MaSuat"]);
                        decimal giaVeMoi = Convert.ToDecimal(rowGheMoi["GiaVe"]);
                        string maGheHienThiMoi = rowGheMoi["MaGheHienThi"].ToString();
                        DateTime thoiGianBatDauMoi = Convert.ToDateTime(rowGheMoi["ThoiGianBatDau"]);

                        // Quy tắc: Chỉ đổi sang suất diễn của CÙNG sự kiện
                        if (maSKMoi != maSKCu)
                        {
                            tran.Rollback();
                            return DonVeResult.Fail("Theo quy định, chỉ được đổi vé sang suất diễn thuộc cùng sự kiện.", maDonCu);
                        }

                        if (thoiGianBatDauMoi <= DateTime.Now)
                        {
                            tran.Rollback();
                            return DonVeResult.Fail("Suất diễn của vị trí ghế mới đã bắt đầu hoặc kết thúc.", maDonCu);
                        }

                        string trangThaiSuatMoi = rowGheMoi["TrangThaiSuat"]?.ToString() ?? string.Empty;
                        if (trangThaiSuatMoi != "MoCuaBanVe")
                        {
                            tran.Rollback();
                            return DonVeResult.Fail("Suất diễn của vị trí ghế mới đã đóng bán hoặc không trong trạng thái mở bán vé.", maDonCu);
                        }

                        // 4. Tính chênh lệch
                        decimal delta = TinhChenhLech(giaVeCu, giaVeMoi);

                        // 5. Bước 1: UPDATE ChiTietVe cũ -> 'DaHuy'
                        ChiTietVeDAL.CapNhatTrangThaiTheoMaCT(maCTCu, TrangThaiChiTietVe.DaHuy, conn, tran);

                        // 6. Bước 2: UPDATE Ghe cũ -> 'Trong', ThoiGianGiu = NULL
                        DonVeDAL.CapNhatTrangThaiGhe(maGheCu, "Trong", conn, tran);

                        // 7. Bước 3: INSERT DonVe mới (lưu MaDonGoc = MaDonCu)
                        DonVeDTO donMoi = new DonVeDTO
                        {
                            MaKH = maKH,
                            MaSuat = maSuatMoi,
                            MaTK = maTK > 0 ? maTK : Convert.ToInt32(rowDonCu["MaTK"]),
                            MaDonGoc = maDonCu,
                            NgayDat = DateTime.Now,
                            TongTien = giaVeMoi,
                            TrangThai = TrangThaiDonVe.DaThanhToan
                        };
                        int maDonMoi = DonVeDAL.Them(donMoi, conn, tran);
                        if (maDonMoi <= 0)
                        {
                            tran.Rollback();
                            return DonVeResult.Fail("Không thể tạo đơn vé mới cho giao dịch đổi vé.", maDonCu);
                        }

                        // 8. Bước 4: Sinh mã vé mới duy nhất và INSERT ChiTietVe mới
                        string maVeCodeMoi = SinhMaVeCode(maSKMoi, maSuatMoi, maGheHienThiMoi, conn, tran);
                        ChiTietVeDTO ctMoi = new ChiTietVeDTO
                        {
                            MaDon = maDonMoi,
                            MaGhe = maGheMoi,
                            GiaVe = giaVeMoi,
                            MaVeCode = maVeCodeMoi,
                            TrangThai = TrangThaiChiTietVe.HopLe
                        };
                        int maCTMoi = ChiTietVeDAL.Them(ctMoi, conn, tran);
                        if (maCTMoi <= 0)
                        {
                            tran.Rollback();
                            return DonVeResult.Fail("Không thể lưu chi tiết vé mới trong giao dịch đổi vé.", maDonCu);
                        }

                        // 9. Bước 5: UPDATE Ghe mới -> 'DaBan'
                        DonVeDAL.CapNhatTrangThaiGhe(maGheMoi, "DaBan", conn, tran);

                        // 10. Bước 6: INSERT ThanhToan ghi nhận số tiền chênh lệch
                        // Phương án A: Nếu delta < 0 (đổi sang ghế rẻ hơn), lưu SoTien = delta (số âm biểu thị khoản hoàn tiền)
                        ThanhToanDTO ttMoi = new ThanhToanDTO
                        {
                            MaDon = maDonMoi,
                            NgayThanhToan = DateTime.Now,
                            SoTien = delta,
                            PhuongThuc = phuongThuc ?? PhuongThucThanhToan.TienMat
                        };
                        ThanhToanDAL.Them(ttMoi, conn, tran);

                        // 11. Bước 7: Xử lý trạng thái và tổng tiền của đơn cũ (Cách 2)
                        int soVeHopLeConLai = ChiTietVeDAL.DemSoVeHopLeTheoDon(maDonCu, conn, tran);
                        if (soVeHopLeConLai == 0)
                        {
                            // Đơn cũ không còn vé hợp lệ nào -> chuyển DaDoi
                            DonVeDAL.CapNhatTrangThai(maDonCu, TrangThaiDonVe.DaDoi, conn, tran);
                        }
                        else
                        {
                            // Đơn cũ vẫn còn vé khác -> cập nhật lại TongTien để khớp với các vé hợp lệ còn lại
                            decimal tongTienConLai = ChiTietVeDAL.TinhTongTienVeHopLe(maDonCu, conn, tran);
                            DonVeDAL.CapNhatTongTien(maDonCu, tongTienConLai, conn, tran);
                        }

                        // 12. Bước 8: Ghi nhật ký kiểm toán hệ thống
                        if (maTK > 0)
                        {
                            string chiTietLog = string.Format(
                                "Đổi vé #{0} (ghế cũ: {1}, giá: {2:N0} đ) từ đơn #{3} sang đơn mới #{4} (ghế mới: {5}, giá: {6:N0} đ). Tiền chênh lệch: {7:N0} đ (Phương thức: {8}).",
                                maCTCu, veCu.MaGheHienThi, giaVeCu, maDonCu, maDonMoi, maGheHienThiMoi, giaVeMoi, delta, phuongThuc);
                            DonVeDAL.GhiNhatKy(maTK, $"Đổi vé #{maCTCu} (Đơn #{maDonCu} -> #{maDonMoi})", chiTietLog, conn, tran);
                        }

                        // Kiểm tra cờ giả lập lỗi
                        if (SimulateFailureBeforeCommit)
                        {
                            throw new InvalidOperationException("Giả lập lỗi hệ thống trước Commit để chứng minh Rollback giao dịch sạch sẽ.");
                        }

                        tran.Commit();

                        string thongBao = delta > 0
                            ? string.Format("Đổi vé thành công! Tạo đơn mới #{0} (Mã vé: {1}).\nKhách hàng đã thanh toán thêm: {2:N0} VNĐ.", maDonMoi, maVeCodeMoi, delta)
                            : delta < 0
                            ? string.Format("Đổi vé thành công! Tạo đơn mới #{0} (Mã vé: {1}).\nSố tiền hoàn lại cho khách hàng: {2:N0} VNĐ.", maDonMoi, maVeCodeMoi, Math.Abs(delta))
                            : string.Format("Đổi vé ngang giá thành công! Tạo đơn mới #{0} (Mã vé: {1}).", maDonMoi, maVeCodeMoi);

                        return DonVeResult.Ok(thongBao, maDonCu, maDonMoi, maVeCodeMoi, delta);
                    }
                    catch (Exception ex)
                    {
                        try { tran.Rollback(); } catch { }
                        throw new ApplicationException($"Lỗi giao dịch khi đổi vé đơn #{maDonCu}: {ex.Message}", ex);
                    }
                }
            }
        }
    }
}
