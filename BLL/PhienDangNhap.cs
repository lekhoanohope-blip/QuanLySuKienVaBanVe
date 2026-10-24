using System;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.BLL
{
    /// <summary>
    /// Phiên đăng nhập hiện tại, dùng chung toàn ứng dụng.
    /// Form nào cần biết "ai đang đăng nhập" (VD: DonVe.MaTK khi bán vé, NhatKy) đều đọc ở đây:
    ///     int maTK = PhienDangNhap.TaiKhoan.MaTK;
    /// </summary>
    public static class PhienDangNhap
    {
        public static TaiKhoanDTO TaiKhoan { get; private set; }
        public static DateTime ThoiDiemDangNhap { get; private set; }

        public static bool DaDangNhap { get { return TaiKhoan != null; } }
        public static bool LaQuanLy { get { return TaiKhoan != null && TaiKhoan.VaiTro == VaiTro.QuanLy; } }

        public static void BatDau(TaiKhoanDTO tk)
        {
            TaiKhoan = tk;
            ThoiDiemDangNhap = DateTime.Now;
        }

        public static void KetThuc()
        {
            TaiKhoan = null;
        }
    }
}
