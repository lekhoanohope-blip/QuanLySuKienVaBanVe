using System;
using System.Collections.Generic;

namespace QuanLySuKienVaBanVe.DTO
{
    /// <summary>
    /// DTO mang thông tin đặt vé từ giao diện Bán vé (Staff) sang giao diện Thanh toán.
    /// Chứa toàn bộ thông tin về Sự kiện, Suất diễn, Khu vực, Số lượng vé và Danh sách ghế được cấp tự động.
    /// </summary>
    public class ThongTinDatVeDTO
    {
        // Thông tin sự kiện
        public int MaSK { get; set; }
        public string TenSK { get; set; }

        // Thông tin suất diễn
        public int MaSuat { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public string DiaDiem { get; set; }
        public int LoaiSoDo { get; set; } = 1; // 1: Chữ T, 2: Hình chữ nhật, 3: Lưới

        // Thông tin khu vực & vé
        public int MaKV { get; set; }
        public string MaVung { get; set; }
        public string TenKV { get; set; }
        public string HangVe { get; set; }
        public decimal GiaVe { get; set; }
        public int TongSoGhe { get; set; }
        public int DaBan { get; set; }
        public int ConLai { get; set; }

        // Thông tin đơn đặt
        public int SoLuong { get; set; } = 1;
        public decimal TongTien => GiaVe * SoLuong;

        // Tổng tiền nhân viên đã báo khách và xác nhận tại giao diện
        public decimal TongTienDaXacNhan { get; set; }

        // Mã phiên giữ ghế duy nhất (chống giải phóng hoặc bán nhầm ghế của quầy khác)
        public Guid MaPhienGiu { get; set; } = Guid.Empty;

        // Danh sách mã ghế thực tế trong cơ sở dữ liệu
        public List<int> DanhSachMaGhe { get; set; } = new List<int>();

        // Danh sách vị trí ghế được phân bổ tự động (hiển thị)
        public List<string> DanhSachGhe { get; set; } = new List<string>();

        // Chuỗi hiển thị danh sách ghế (ví dụ: "VIP A - A1, VIP A - A2")
        public string ViTriGheHienThi => (DanhSachGhe != null && DanhSachGhe.Count > 0)
            ? string.Join(", ", DanhSachGhe)
            : "Chưa phân bổ";

        // Thông tin khách hàng đặt vé
        public int? MaKH { get; set; }
        public string TenKH { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public string PhuongThucThanhToan { get; set; } = "TienMat";

        // Cờ cho phép cập nhật tên khách hàng khi SĐT khớp khách cũ trong DB (được xác nhận ở UI)
        public bool ChoPhepCapNhatTen { get; set; } = false;

        // Thời gian giữ ghế còn lại (giây), mặc định 600s = 10 phút
        public int SoGiayConLai { get; set; } = 600;

        // Kết quả sau khi thanh toán thành công
        public int MaDonPhatHanh { get; set; }
        public List<string> DanhSachMaVeCode { get; set; } = new List<string>();
    }
}
