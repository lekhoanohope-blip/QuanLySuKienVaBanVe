using System;
using System.Collections.Generic;

namespace QuanLySuKienVaBanVe.DTO
{
    /// <summary>
    /// DTO mang thông tin đặt vé từ giao diện Bán vé (Staff) sang giao diện Thanh toán.
    /// </summary>
    public class ThongTinDatVeDTO
    {
        public int MaSK { get; set; }
        public string TenSK { get; set; }

        public int MaSuat { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public string DiaDiem { get; set; }
        public int LoaiSoDo { get; set; } = 1; // 1: Chữ T, 2: Hình chữ nhật, 3: Lưới

        public int MaKV { get; set; }
        public string MaVung { get; set; }
        public string TenKV { get; set; }
        public string HangVe { get; set; }
        public decimal GiaVe { get; set; }
        public int TongSoGhe { get; set; }
        public int DaBan { get; set; }
        public int ConLai { get; set; }

        public int SoLuong { get; set; } = 1;
        public decimal TongTien => GiaVe * SoLuong;

        /// <summary>Tổng tiền nhân viên đã báo khách tại màn hình chọn vé (đối chiếu chống đổi giá giữa chừng).</summary>
        public decimal TongTienDaXacNhan { get; set; }

        /// <summary>Mã phiên giữ ghế duy nhất (chống giải phóng hoặc bán nhầm ghế của quầy khác).</summary>
        public Guid MaPhienGiu { get; set; } = Guid.Empty;

        public List<int> DanhSachMaGhe { get; set; } = new List<int>();
        public List<string> DanhSachGhe { get; set; } = new List<string>();

        public string ViTriGheHienThi => (DanhSachGhe != null && DanhSachGhe.Count > 0)
            ? string.Join(", ", DanhSachGhe)
            : "Chưa phân bổ";

        public int? MaKH { get; set; }
        public string TenKH { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public string PhuongThucThanhToan { get; set; } = "TienMat";

        /// <summary>Cho phép ghi đè tên khách hàng khi SĐT khớp khách cũ trong CSDL.</summary>
        public bool ChoPhepCapNhatTen { get; set; } = false;

        public int SoGiayConLai { get; set; } = 600;
        public int MaDonPhatHanh { get; set; }
        public List<string> DanhSachMaVeCode { get; set; } = new List<string>();
    }
}
