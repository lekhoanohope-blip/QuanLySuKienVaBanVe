using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using QuanLySuKienVaBanVe.DAL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.BLL
{
    public class KetQuaThanhToan
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int MaDon { get; set; }
        public int MaKH { get; set; }
        public decimal TongTien { get; set; }
        public List<ChiTietVeDTO> DanhSachVePhatHanh { get; set; } = new List<ChiTietVeDTO>();

        // Cờ và thông tin phục vụ khi giá vé thay đổi giữa chừng
        public bool CanXacNhanGia { get; set; }
        public decimal TongTienMoi { get; set; }

        public static KetQuaThanhToan Ok(string message, int maDon, int maKH, decimal tongTien, List<ChiTietVeDTO> dsVe)
        {
            return new KetQuaThanhToan
            {
                Success = true,
                Message = message,
                MaDon = maDon,
                MaKH = maKH,
                TongTien = tongTien,
                DanhSachVePhatHanh = dsVe ?? new List<ChiTietVeDTO>()
            };
        }

        public static KetQuaThanhToan Fail(string message)
        {
            return new KetQuaThanhToan
            {
                Success = false,
                Message = message
            };
        }

        public static KetQuaThanhToan GiaThayDoi(decimal giaCu, decimal giaMoi)
        {
            return new KetQuaThanhToan
            {
                Success = false,
                CanXacNhanGia = true,
                TongTienMoi = giaMoi,
                Message = $"Giá vé đã được quản lý cập nhật từ {giaCu:N0} đ thành {giaMoi:N0} đ. Vui lòng xác nhận lại với khách hàng trước khi phát hành vé."
            };
        }
    }

    public class BanVeBLL
    {
#if DEBUG
        /// <summary>
        /// Cờ giả lập lỗi phát sinh ngay trước trans.Commit() để kiểm thử Rollback (TC3).
        /// Chỉ tồn tại trong bản DEBUG.
        /// </summary>
        public static bool SimulateFailureBeforeCommit { get; set; } = false;
#endif

        private static string ChuanHoaPhuongThucThanhToan(string pt)
        {
            if (string.IsNullOrWhiteSpace(pt)) return PhuongThucThanhToan.TienMat;
            string clean = pt.Trim();
            if (clean.IndexOf("ChuyenKhoan", StringComparison.OrdinalIgnoreCase) >= 0)
                return PhuongThucThanhToan.ChuyenKhoan;
            if (clean.IndexOf("The", StringComparison.OrdinalIgnoreCase) >= 0 ||
                clean.IndexOf("ATM", StringComparison.OrdinalIgnoreCase) >= 0 ||
                clean.IndexOf("Visa", StringComparison.OrdinalIgnoreCase) >= 0)
                return PhuongThucThanhToan.TheNganHang;
            return PhuongThucThanhToan.TienMat;
        }

        private static void DongBoThongTinKhach(KhachHangDTO kh, ThongTinDatVeDTO booking, SqlConnection conn, SqlTransaction trans)
        {
            bool doi = false;
            string ten = booking.TenKH?.Trim();
            string email = booking.Email?.Trim();

            // Nếu người dùng xác nhận cho phép cập nhật tên và tên mới khác tên cũ
            if (!string.IsNullOrEmpty(ten) && !string.Equals(ten, kh.TenKH, StringComparison.Ordinal) && booking.ChoPhepCapNhatTen)
            {
                kh.TenKH = ten;
                doi = true;
            }

            // Email được so sánh và cập nhật độc lập với tên
            if (!string.IsNullOrEmpty(email) && !string.Equals(email, kh.Email, StringComparison.OrdinalIgnoreCase))
            {
                kh.Email = email;
                doi = true;
            }

            if (doi)
            {
                KhachHangDAL.Sua(kh, conn, trans);
            }
            booking.TenKH = kh.TenKH;
        }

        private static int XuLyKhachHangTrongTransaction(ThongTinDatVeDTO booking, SqlConnection conn, SqlTransaction trans)
        {
            string sdt = booking.SDT?.Trim() ?? string.Empty;

            // 1. Nếu có MaKH hợp lệ, CHỈ chấp nhận nếu SĐT trong DB khớp chính xác với booking.SDT
            // (Chống lỗi gán nhầm MaKH cũ khi người dùng thay đổi SĐT trên giao diện)
            if (booking.MaKH.HasValue && booking.MaKH.Value > 0)
            {
                KhachHangDTO khExist = KhachHangDAL.LayTheoMa(booking.MaKH.Value, conn, trans);
                if (khExist != null && string.Equals(khExist.SDT?.Trim(), sdt, StringComparison.OrdinalIgnoreCase))
                {
                    DongBoThongTinKhach(khExist, booking, conn, trans);
                    return khExist.MaKH;
                }
            }

            // 2. Tra cứu theo SĐT nếu có
            if (!string.IsNullOrEmpty(sdt))
            {
                KhachHangDTO khExistByPhone = KhachHangDAL.LayTheoSDT(sdt, conn, trans);
                if (khExistByPhone != null)
                {
                    DongBoThongTinKhach(khExistByPhone, booking, conn, trans);
                    return khExistByPhone.MaKH;
                }
            }

            // 3. Nếu chưa có -> Tạo khách hàng mới ngay trong transaction
            string tenKH = !string.IsNullOrWhiteSpace(booking.TenKH) ? booking.TenKH.Trim() : "Khách Lẻ Mua Vé";

            KhachHangDTO khMoi = new KhachHangDTO
            {
                TenKH = tenKH,
                SDT = sdt,
                Email = booking.Email?.Trim()
            };

            int newMaKH = KhachHangDAL.Them(khMoi, conn, trans);
            return newMaKH;
        }

        /// <summary>
        /// Thực thi chuỗi nghiệp vụ Bán vé an toàn tuyệt đối trong 1 SqlTransaction duy nhất:
        /// Bước 0: Upsert thông tin khách hàng trong transaction.
        /// Bước 1: Kiểm tra lại hiệu lực giữ ghế bằng khóa vi-mô WITH (UPDLOCK, ROWLOCK), đối chiếu MaPhienGiu và giờ diễn.
        /// Bước 2: Đối chiếu tổng tiền thực tế với báo giá TongTienDaXacNhan.
        /// Bước 3: INSERT DonVe và lấy MaDon sinh qua OUTPUT INSERTED.MaDon.
        /// Bước 4: Sinh MaVeCode duy nhất (có retry khi trùng) & INSERT ChiTietVe cho từng ghế.
        /// Bước 5: UPDATE Ghe sang trạng thái 'DaBan' theo đúng MaPhienGiu.
        /// Bước 6: INSERT ThanhToan và ghi nhật ký hệ thống.
        /// Cơ chế Fault Tolerance: lỗi ở bất kỳ bước nào, transaction tự động Rollback() sạch sẽ 100%.
        /// </summary>
        public static KetQuaThanhToan XuLyThanhToan(ThongTinDatVeDTO booking, int maTK = 0)
        {
            if (booking == null || booking.DanhSachMaGhe == null || booking.DanhSachMaGhe.Count == 0)
            {
                return KetQuaThanhToan.Fail("Danh sách ghế đặt đang trống. Vui lòng chọn ghế trước khi thanh toán.");
            }

            if (booking.MaPhienGiu == Guid.Empty)
            {
                return KetQuaThanhToan.Fail("Mã phiên giữ ghế không hợp lệ hoặc đã hết hiệu lực.");
            }

            string sdt = booking.SDT?.Trim() ?? string.Empty;
            if (!Regex.IsMatch(sdt, @"^0[0-9]{9}$"))
            {
                return KetQuaThanhToan.Fail("Số điện thoại khách hàng không hợp lệ (phải gồm đúng 10 chữ số bắt đầu bằng số 0).");
            }

            if (maTK <= 0 && PhienDangNhap.DaDangNhap)
            {
                maTK = PhienDangNhap.TaiKhoan.MaTK;
            }
            if (maTK <= 0)
            {
                maTK = 1; // Fallback nhân viên mặc định nếu chạy độc lập
            }

            string phuongThuc = ChuanHoaPhuongThucThanhToan(booking.PhuongThucThanhToan);

            using (SqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        // Bước 0: Upsert Khách hàng an toàn trong Transaction
                        int maKH = XuLyKhachHangTrongTransaction(booking, conn, trans);
                        if (maKH <= 0)
                        {
                            trans.Rollback();
                            return KetQuaThanhToan.Fail("Không thể khởi tạo hoặc xác thực thông tin khách hàng.");
                        }

                        // Bước 1: Kiểm tra lại hiệu lực giữ ghế với khóa dòng (UPDLOCK, ROWLOCK)
                        // Sắp xếp MaGhe tăng dần để khóa theo thứ tự cố định -> Chống Deadlock!
                        List<int> sortedSeatIds = booking.DanhSachMaGhe.Distinct().OrderBy(id => id).ToList();
                        var lockedSeats = new List<(int MaGhe, string MaGheHienThi, decimal GiaVe, int MaSK, int MaSuat, string TenKV, string HangVe)>();
                        decimal tongTienThucTe = 0m;

                        foreach (int maGhe in sortedSeatIds)
                        {
                            DataRow rowGhe = GheDAL.KiemTraVaKhoaGheTrongGiaoDich(maGhe, conn, trans);
                            if (rowGhe == null)
                            {
                                trans.Rollback();
                                return KetQuaThanhToan.Fail($"Ghế mã #{maGhe} không tồn tại trong cơ sở dữ liệu.");
                            }

                            string maGheHienThi = rowGhe["MaGheHienThi"]?.ToString() ?? maGhe.ToString();
                            string trangThai = rowGhe["TrangThai"]?.ToString() ?? string.Empty;
                            int soGiayDaGiu = Convert.ToInt32(rowGhe["SoGiayDaGiu"]);
                            bool choPhepBanVe = Convert.ToBoolean(rowGhe["ChoPhepBanVe"]);
                            string trangThaiSuat = rowGhe["TrangThaiSuat"]?.ToString() ?? string.Empty;
                            int maSuatDB = Convert.ToInt32(rowGhe["MaSuat"]);
                            int maSKDB = Convert.ToInt32(rowGhe["MaSK"]);
                            decimal giaVeDB = Convert.ToDecimal(rowGhe["GiaVe"]);
                            int daBatDau = Convert.ToInt32(rowGhe["DaBatDau"]);
                            Guid phienDb = rowGhe["MaPhienGiu"] == DBNull.Value ? Guid.Empty : (Guid)rowGhe["MaPhienGiu"];

                            if (maSuatDB != booking.MaSuat)
                            {
                                trans.Rollback();
                                return KetQuaThanhToan.Fail($"Ghế {maGheHienThi} không thuộc suất diễn #{booking.MaSuat}.");
                            }

                            if (!choPhepBanVe || trangThaiSuat != "MoCuaBanVe")
                            {
                                trans.Rollback();
                                return KetQuaThanhToan.Fail($"Khu vực hoặc suất diễn của ghế {maGheHienThi} hiện đã đóng bán vé.");
                            }

                            if (daBatDau == 1)
                            {
                                trans.Rollback();
                                return KetQuaThanhToan.Fail($"Suất diễn của ghế {maGheHienThi} đã bắt đầu lúc {Convert.ToDateTime(rowGhe["ThoiGianBatDau"]):dd/MM/yyyy HH:mm}. Không thể tiếp tục bán vé.");
                            }

                            if (trangThai != TrangThaiGhe.DangGiu || phienDb != booking.MaPhienGiu || soGiayDaGiu > GheDAL.ThoiGianGiuToiDaGiay)
                            {
                                trans.Rollback();
                                return KetQuaThanhToan.Fail(
                                    $"Ghế {maGheHienThi} không còn thuộc phiên giữ của bạn (đã hết hạn hoặc quầy khác đã giữ). Vui lòng chọn lại ghế!");
                            }

                            tongTienThucTe += giaVeDB;
                            lockedSeats.Add((maGhe, maGheHienThi, giaVeDB, maSKDB, maSuatDB,
                                rowGhe["TenKV"]?.ToString(), rowGhe["HangVe"]?.ToString()));
                        }

                        // Bước 2: So sánh giá thực tế trong DB với báo giá khách đã xác nhận
                        if (tongTienThucTe != booking.TongTienDaXacNhan)
                        {
                            trans.Rollback();
                            return KetQuaThanhToan.GiaThayDoi(booking.TongTienDaXacNhan, tongTienThucTe);
                        }

                        // Bước 3: INSERT DonVe (Lấy mã đơn vừa sinh qua OUTPUT INSERTED.MaDon)
                        DonVeDTO donMoi = new DonVeDTO
                        {
                            MaKH = maKH,
                            MaSuat = booking.MaSuat,
                            MaTK = maTK,
                            MaDonGoc = null,
                            NgayDat = DateTime.Now,
                            TongTien = tongTienThucTe,
                            TrangThai = TrangThaiDonVe.DaThanhToan
                        };

                        int maDonMoi = DonVeDAL.Them(donMoi, conn, trans);
                        if (maDonMoi <= 0)
                        {
                            trans.Rollback();
                            return KetQuaThanhToan.Fail("Không thể tạo đơn vé trong cơ sở dữ liệu.");
                        }

                        // Bước 4 & Bước 5: Sinh mã vé duy nhất (retry), INSERT ChiTietVe & UPDATE Ghe
                        var dsVePhatHanh = new List<ChiTietVeDTO>();
                        const int MaxRetryMaVe = 5;

                        foreach (var seat in lockedSeats)
                        {
                            int maCT = 0;
                            string maVeCode = string.Empty;

                            for (int retry = 0; retry < MaxRetryMaVe; retry++)
                            {
                                maVeCode = DonVeBLL.SinhMaVeCode(seat.MaSK, seat.MaSuat, seat.MaGheHienThi, conn, trans);
                                ChiTietVeDTO ct = new ChiTietVeDTO
                                {
                                    MaDon = maDonMoi,
                                    MaGhe = seat.MaGhe,
                                    MaGheHienThi = seat.MaGheHienThi,
                                    TenKV = seat.TenKV,
                                    HangVe = seat.HangVe,
                                    GiaVe = seat.GiaVe,
                                    MaVeCode = maVeCode,
                                    TrangThai = TrangThaiChiTietVe.HopLe
                                };

                                try
                                {
                                    maCT = ChiTietVeDAL.Them(ct, conn, trans);
                                    if (maCT > 0)
                                    {
                                        ct.MaCT = maCT;
                                        dsVePhatHanh.Add(ct);
                                        break;
                                    }
                                }
                                catch (SqlException sqlEx) when ((sqlEx.Number == 2601 || sqlEx.Number == 2627)
                                                              && sqlEx.Message.Contains("UQ_ChiTietVe_MaVeCode"))
                                {
                                    if (retry == MaxRetryMaVe - 1)
                                    {
                                        trans.Rollback();
                                        return KetQuaThanhToan.Fail($"Không thể sinh mã vé duy nhất sau {MaxRetryMaVe} lần thử cho ghế {seat.MaGheHienThi}.");
                                    }
                                }
                            }

                            if (maCT <= 0)
                            {
                                trans.Rollback();
                                return KetQuaThanhToan.Fail($"Không thể tạo chi tiết vé cho vị trí {seat.MaGheHienThi}.");
                            }

                            // Bước 5: UPDATE Ghe -> 'DaBan', ThoiGianGiu = NULL (kiểm tra khớp MaPhienGiu)
                            bool okGhe = GheDAL.CapNhatTrangThai(seat.MaGhe, TrangThaiGhe.DaBan, booking.MaPhienGiu, conn, trans);
                            if (!okGhe)
                            {
                                trans.Rollback();
                                return KetQuaThanhToan.Fail($"Không thể cập nhật trạng thái Đã bán cho ghế {seat.MaGheHienThi} (ghế không còn thuộc phiên giữ này).");
                            }
                        }

                        // Bước 6: INSERT ThanhToan & Ghi nhật ký kiểm toán
                        ThanhToanDTO tt = new ThanhToanDTO
                        {
                            MaDon = maDonMoi,
                            NgayThanhToan = DateTime.Now,
                            SoTien = tongTienThucTe,
                            PhuongThuc = phuongThuc
                        };

                        int maTT = ThanhToanDAL.Them(tt, conn, trans);
                        if (maTT <= 0)
                        {
                            trans.Rollback();
                            return KetQuaThanhToan.Fail("Không thể ghi nhận giao dịch thanh toán.");
                        }

                        string dsGheStr = string.Join(", ", lockedSeats.Select(s => s.MaGheHienThi));
                        string chiTietLog = $"Bán vé thành công đơn #{maDonMoi} ({dsVePhatHanh.Count} vé). " +
                                            $"Khách hàng: {booking.TenKH} (SĐT: {booking.SDT}). Ghế: [{dsGheStr}]. Tổng tiền: {tongTienThucTe:N0} đ.";
                        DonVeDAL.GhiNhatKy(maTK, $"Bán vé đơn #{maDonMoi}", chiTietLog, conn, trans);

#if DEBUG
                        // Giả lập lỗi để test Rollback (TC3) - chỉ có trong bản Debug
                        if (SimulateFailureBeforeCommit || DonVeBLL.SimulateFailureBeforeCommit)
                        {
                            throw new InvalidOperationException("Giả lập sự cố ngắt kết nối trước Commit để kiểm thử tính năng Rollback giao dịch sạch sẽ.");
                        }
#endif

                        trans.Commit();

                        return KetQuaThanhToan.Ok(
                            $"Thanh toán thành công đơn vé #{maDonMoi}!\nTổng tiền: {tongTienThucTe:N0} VNĐ ({dsVePhatHanh.Count} vé).\nCác vị trí ghế: {dsGheStr}.",
                            maDonMoi, maKH, tongTienThucTe, dsVePhatHanh);
                    }
                    catch (SqlException sqlEx)
                    {
                        try { trans.Rollback(); } catch { }
                        if (sqlEx.Number == 2601 || sqlEx.Number == 2627)
                        {
                            if (sqlEx.Message.Contains("UQ_ChiTietVe_Ghe_HopLe") || sqlEx.Message.Contains("UQ_ChiTietVe_DonGhe"))
                            {
                                return KetQuaThanhToan.Fail("Xung đột dữ liệu: Một trong các ghế vừa được thanh toán ở giao dịch khác. Toàn bộ thao tác đã được Rollback an toàn.");
                            }
                            return KetQuaThanhToan.Fail("Xung đột dữ liệu khóa duy nhất: " + sqlEx.Message);
                        }
                        return KetQuaThanhToan.Fail("Lỗi cơ sở dữ liệu khi thanh toán (Đã Rollback hoàn toàn): " + sqlEx.Message);
                    }
                    catch (Exception ex)
                    {
                        try { trans.Rollback(); } catch { }
                        return KetQuaThanhToan.Fail("Giao dịch bị hủy và đã Rollback toàn bộ dữ liệu sạch sẽ: " + ex.Message);
                    }
                }
            }
        }
    }
}
