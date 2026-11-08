namespace QuanLySuKienVaBanVe.DTO
{
    /// <summary>Đối tượng truyền dữ liệu của bảng SuKien.</summary>
    public class SuKienDTO
    {
        public int MaSK { get; set; }
        public string TenSK { get; set; }
        public string LoaiSK { get; set; }
        public string MoTa { get; set; }
        public string TrangThai { get; set; }   // "SapDien" | "DangDien" | "DaKetThuc"
        public string HinhAnh { get; internal set; }
    }
}
