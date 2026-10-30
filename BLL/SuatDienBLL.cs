using System;
using System.Data;
using QuanLySuKienVaBanVe.DAL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.BLL
{
    public class SuatDienBLL
    {
        public static DataTable LayDanhSach() => SuatDienDAL.LayDanhSach();

        public static SuatDienDTO LayTheoMa(int maSuat) => SuatDienDAL.LayTheoMa(maSuat);

        public static DataTable LayDanhSachTheoSuKien(int maSK) => SuatDienDAL.LayDanhSachTheoSuKien(maSK);

        public static string Them(SuatDienDTO sd)
        {
            if (sd == null) return "Dữ liệu suất diễn không hợp lệ!";
            if (sd.MaSK <= 0) return "Vui lòng chọn sự kiện!";
            if (string.IsNullOrWhiteSpace(sd.DiaDiem)) return "Địa điểm không được để trống!";
            if (sd.ThoiGianKetThuc.HasValue && sd.ThoiGianKetThuc <= sd.ThoiGianBatDau)
                return "Thời gian kết thúc phải lớn hơn thời gian bắt đầu!";
            if (string.IsNullOrWhiteSpace(sd.TrangThai)) sd.TrangThai = "MoCuaBanVe";

            try
            {
                return SuatDienDAL.Them(sd) ? "Success" : "Thêm suất diễn thất bại!";
            }
            catch (Exception ex)
            {
                return "Lỗi CSDL khi thêm suất diễn: " + ex.Message;
            }
        }

        public static string Sua(SuatDienDTO sd)
        {
            if (sd == null || sd.MaSuat <= 0) return "Mã suất diễn không hợp lệ!";
            if (sd.MaSK <= 0) return "Vui lòng chọn sự kiện!";
            if (string.IsNullOrWhiteSpace(sd.DiaDiem)) return "Địa điểm không được để trống!";
            if (sd.ThoiGianKetThuc.HasValue && sd.ThoiGianKetThuc <= sd.ThoiGianBatDau)
                return "Thời gian kết thúc phải lớn hơn thời gian bắt đầu!";

            try
            {
                return SuatDienDAL.Sua(sd) ? "Success" : "Sửa suất diễn thất bại!";
            }
            catch (Exception ex)
            {
                return "Lỗi CSDL khi cập nhật suất diễn: " + ex.Message;
            }
        }

        public static string Xoa(int maSuat)
        {
            try
            {
                if (SuatDienDAL.KiemTraCoDonVe(maSuat) > 0)
                {
                    return "Không thể xóa! Suất diễn này đã có đơn đặt vé liên kết.";
                }

                if (SuatDienDAL.KiemTraCoKhuVuc(maSuat) > 0)
                {
                    return "Không thể xóa! Suất diễn này đã thiết lập khu vực vé.";
                }

                return SuatDienDAL.Xoa(maSuat) ? "Success" : "Xóa suất diễn thất bại!";
            }
            catch (Exception ex)
            {
                return "Lỗi CSDL khi xóa suất diễn: " + ex.Message;
            }
        }
    }
}