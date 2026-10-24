using System;

namespace QuanLySuKienVaBanVe.DTO
{
    public class SuatDienDTO
    {
        public int MaSuat { get; set; }
        public int MaSK { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
        public string DiaDiem { get; set; }
        public string TrangThai { get; set; }
    }
}