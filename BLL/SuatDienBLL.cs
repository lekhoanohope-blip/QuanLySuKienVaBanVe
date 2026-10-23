using System.Data;
using QuanLySuKienVaBanVe.DAL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.BLL
{
    public class SuatDienBLL
    {
        public static DataTable LayDanhSach() => SuatDienDAL.LayDanhSach();

        public static string Them(SuatDienDTO sd)
        {
            if (sd.MaSK <= 0) return "Vui lòng chọn sự kiện!";
            if (string.IsNullOrWhiteSpace(sd.DiaDiem)) return "Địa điểm không được để trống!";
            if (sd.ThoiGianKetThuc.HasValue && sd.ThoiGianKetThuc <= sd.ThoiGianBatDau)
                return "Thời gian kết thúc phải lớn hơn thời gian bắt đầu!";

            return SuatDienDAL.Them(sd) ? "Success" : "Thêm thất bại!";
        }

        public static string Xoa(int maSuat)
        {
            return SuatDienDAL.Xoa(maSuat) ? "Success" : "Xóa thất bại!";
        }
    }
}