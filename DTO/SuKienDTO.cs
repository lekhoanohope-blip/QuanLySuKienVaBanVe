namespace QuanLySuKienVaBanVe.DTO
{
    public class SuKienDTO
    {
        public int MaSK { get; set; }
        public string TenSK { get; set; }
        public string LoaiSK { get; set; }
        public string MoTa { get; set; }
        public string TrangThai { get; set; }
        public string HinhAnh { get; internal set; }
    }
}