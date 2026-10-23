using System.Data;
using QuanLySuKienVaBanVe.DAL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.BLL
{
    public class SuKienBLL
    {
        public static DataTable LayDanhSach() => SuKienDAL.LayDanhSach();

        public static string Them(SuKienDTO sk)
        {
            if (string.IsNullOrWhiteSpace(sk.TenSK)) return "Tên sự kiện không được để trống!";
            return SuKienDAL.Them(sk) ? "Success" : "Thêm thất bại!";
        }

        public static string Sua(SuKienDTO sk)
        {
            if (string.IsNullOrWhiteSpace(sk.TenSK)) return "Tên sự kiện không được để trống!";
            return SuKienDAL.Sua(sk) ? "Success" : "Sửa thất bại!";
        }

        public static string Xoa(int maSK)
        {
            if (SuKienDAL.KiemTraCoSuatDien(maSK) > 0)
            {
                return "Không thể xóa! Sự kiện này đã có suất diễn bên trong.";
            }
            return SuKienDAL.Xoa(maSK) ? "Success" : "Xóa thất bại!";
        }
    }
}