namespace QuanLySuKienVaBanVe.DTO
{
    /// <summary>Hai vai trò hợp lệ trong cột TaiKhoan.VaiTro (khớp script SQL).</summary>
    public static class VaiTro
    {
        public const string QuanLy = "QuanLy";
        public const string NhanVien = "NhanVien";
    }

    /// <summary>Đối tượng truyền dữ liệu của bảng TaiKhoan (chỉ có thuộc tính, không có logic).</summary>
    public class TaiKhoanDTO
    {
        public int MaTK { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }     // chuỗi băm SHA-256 (hoặc plaintext nếu script demo chưa băm)
        public string VaiTro { get; set; }      // "QuanLy" | "NhanVien"
        public bool TrangThai { get; set; }     // true = hoạt động, false = bị khóa
    }
}
