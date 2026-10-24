using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using QuanLySuKienVaBanVe.DAL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.BLL
{
    // BLL KHÔNG chứa chuỗi hiển thị: chỉ trả mã kết quả, GUI tự đổi sang tiếng Việt/Anh.
    public enum KetQuaDangNhap { ThanhCong, ThieuThongTin, SaiThongTin, TaiKhoanBiKhoa, TamKhoa, LoiHeThong }
    public enum KetQuaDangKy { ThanhCong, TenKhongHopLe, MatKhauNgan, XacNhanKhongKhop, TrungTen, LoiHeThong }

    /// <summary>Nghiệp vụ tài khoản: kiểm tra đầu vào, băm mật khẩu, chống dò mật khẩu.</summary>
    public static class TaiKhoanBLL
    {
        private const int SoLanSaiToiDa = 5;                                // sai 5 lần thì khóa tạm
        private static readonly TimeSpan ThoiGianKhoa = TimeSpan.FromSeconds(60);

        private class LanSai { public int SoLan; public DateTime KhoaDen = DateTime.MinValue; }
        private static readonly Dictionary<string, LanSai> dsSai = new Dictionary<string, LanSai>();
        private static readonly object khoa = new object();

        /// <summary>Thông điệp lỗi kỹ thuật gần nhất (dùng để debug, không hiện cho người dùng cuối).</summary>
        public static string ChiTietLoi { get; private set; }
        /// <summary>Số giây còn lại khi kết quả là TamKhoa.</summary>
        public static int GiayKhoaConLai { get; private set; }

        public static KetQuaDangNhap DangNhap(string tenDangNhap, string matKhau, out TaiKhoanDTO taiKhoan)
        {
            taiKhoan = null;
            tenDangNhap = (tenDangNhap ?? "").Trim();
            if (tenDangNhap.Length == 0 || string.IsNullOrEmpty(matKhau))
                return KetQuaDangNhap.ThieuThongTin;

            string key = tenDangNhap.ToLowerInvariant();

            // 1. Đang bị khóa tạm?
            lock (khoa)
            {
                LanSai ls;
                if (dsSai.TryGetValue(key, out ls) && ls.KhoaDen > DateTime.Now)
                {
                    GiayKhoaConLai = (int)Math.Ceiling((ls.KhoaDen - DateTime.Now).TotalSeconds);
                    return KetQuaDangNhap.TamKhoa;
                }
            }

            // 2. Lấy tài khoản từ CSDL
            TaiKhoanDTO tk;
            try { tk = TaiKhoanDAL.LayTheoTenDangNhap(tenDangNhap); }
            catch (Exception ex) { ChiTietLoi = ex.Message; return KetQuaDangNhap.LoiHeThong; }

            // 3. Sai tên hoặc sai mật khẩu -> cùng một thông báo (không lộ tên nào tồn tại)
            if (tk == null || !KiemTraMatKhau(matKhau, tk.MatKhau))
            {
                if (GhiNhanSai(key)) return KetQuaDangNhap.TamKhoa;
                return KetQuaDangNhap.SaiThongTin;
            }

            // 4. Đúng mật khẩu nhưng tài khoản bị Admin khóa
            if (!tk.TrangThai) return KetQuaDangNhap.TaiKhoanBiKhoa;

            XoaSai(key);
            tk.MatKhau = null;      // không giữ chuỗi băm trong bộ nhớ phiên
            taiKhoan = tk;
            return KetQuaDangNhap.ThanhCong;
        }

        public static KetQuaDangKy DangKy(string tenDangNhap, string matKhau, string xacNhan)
        {
            tenDangNhap = (tenDangNhap ?? "").Trim();
            if (!Regex.IsMatch(tenDangNhap, @"^[A-Za-z0-9_.]{4,30}$")) return KetQuaDangKy.TenKhongHopLe;
            if (matKhau == null || matKhau.Length < 6) return KetQuaDangKy.MatKhauNgan;
            if (matKhau != xacNhan) return KetQuaDangKy.XacNhanKhongKhop;

            try
            {
                if (TaiKhoanDAL.TonTai(tenDangNhap)) return KetQuaDangKy.TrungTen;

                bool ok = TaiKhoanDAL.Them(new TaiKhoanDTO
                {
                    TenDangNhap = tenDangNhap,
                    MatKhau = BamMatKhau(matKhau),
                    VaiTro = VaiTro.NhanVien,       // tự đăng ký luôn là Nhân viên, KHÔNG BAO GIỜ là Quản lý
                    TrangThai = true
                });
                return ok ? KetQuaDangKy.ThanhCong : KetQuaDangKy.LoiHeThong;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601) return KetQuaDangKy.TrungTen; // vi phạm UNIQUE
                ChiTietLoi = ex.Message;
                return KetQuaDangKy.LoiHeThong;
            }
            catch (Exception ex)
            {
                ChiTietLoi = ex.Message;
                return KetQuaDangKy.LoiHeThong;
            }
        }

        // ---------------- Băm / kiểm tra mật khẩu ----------------

        /// <summary>SHA-256 dạng hex thường (64 ký tự). VD "123456" -> 8d969eef...dc6c92</summary>
        public static string BamMatKhau(string matKhau)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] h = sha.ComputeHash(Encoding.UTF8.GetBytes(matKhau));
                StringBuilder sb = new StringBuilder(64);
                foreach (byte b in h) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        private static bool KiemTraMatKhau(string nhap, string luu)
        {
            if (string.IsNullOrEmpty(luu)) return false;
            if (luu.Length == 64 && Regex.IsMatch(luu, "^[0-9a-fA-F]{64}$"))
                return string.Equals(BamMatKhau(nhap), luu, StringComparison.OrdinalIgnoreCase);
            // Tương thích tạm với script demo chưa băm (plaintext). Khi TV2 đã băm 2 tài khoản demo
            // thì có thể xóa dòng dưới để chỉ còn chấp nhận mật khẩu đã băm.
            return luu == nhap;
        }

        // ---------------- Chống dò mật khẩu (bộ nhớ, reset khi tắt app) ----------------

        private static bool GhiNhanSai(string key)
        {
            lock (khoa)
            {
                LanSai ls;
                if (!dsSai.TryGetValue(key, out ls)) { ls = new LanSai(); dsSai[key] = ls; }
                ls.SoLan++;
                if (ls.SoLan >= SoLanSaiToiDa)
                {
                    ls.SoLan = 0;
                    ls.KhoaDen = DateTime.Now + ThoiGianKhoa;
                    GiayKhoaConLai = (int)ThoiGianKhoa.TotalSeconds;
                    return true;
                }
                return false;
            }
        }

        private static void XoaSai(string key)
        {
            lock (khoa) { dsSai.Remove(key); }
        }
    }
}
