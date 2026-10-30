using System;

namespace QuanLySuKienVaBanVe.DTO
{
    /// <summary>
    /// Đối tượng truyền dữ liệu của bảng KhuVuc (DTO - Data Transfer Object).
    /// </summary>
    public class KhuVucDTO
    {
        public int MaKV { get; set; }
        public int MaSuat { get; set; }
        public string MaVung { get; set; }
        public string TenKV { get; set; }
        public string HangVe { get; set; }
        public decimal GiaVe { get; set; }
        public int TongSoGhe { get; set; }
        public bool ChoPhepBanVe { get; set; } = true;

        // Dữ liệu mở rộng phục vụ hiển thị thống kê / ma trận ghế
        public int DaBan { get; set; }
        public int DangGiu { get; set; }
        public int ConLai { get; set; }
    }
}
