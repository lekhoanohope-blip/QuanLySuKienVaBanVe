using System;

namespace QuanLySuKienVaBanVe.DTO
{
    /// <summary>Đối tượng truyền dữ liệu của bảng SuatDien.</summary>
    public class SuatDienDTO
    {
        public int MaSuat { get; set; }
        public int MaSK { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
        public string DiaDiem { get; set; }
        public string TrangThai { get; set; }   // "MoCuaBanVe" | "DongBanVe" | "DangDien" | "DaKetThuc"
        public int LoaiSoDo { get; set; } = 1;  // 1: Chữ T, 2: Hình chữ nhật, 3: Lưới
    }
}
