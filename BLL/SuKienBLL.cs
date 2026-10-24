using System;
using System.Data;
using QuanLySuKienVaBanVe.DAL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.BLL
{
    public class SuKienBLL
    {
        public static DataTable LayDanhSach() => SuKienDAL.LayDanhSach();

        public static SuKienDTO LayTheoMa(int maSK) => SuKienDAL.LayTheoMa(maSK);

        public static string Them(SuKienDTO sk)
        {
            if (sk == null) return "Dữ liệu sự kiện không hợp lệ!";
            if (string.IsNullOrWhiteSpace(sk.TenSK)) return "Tên sự kiện không được để trống!";
            if (string.IsNullOrWhiteSpace(sk.TrangThai)) sk.TrangThai = "SapDien";

            try
            {
                return SuKienDAL.Them(sk) ? "Success" : "Thêm sự kiện thất bại!";
            }
            catch (Exception ex)
            {
                return "Lỗi CSDL khi thêm sự kiện: " + ex.Message;
            }
        }

        public static string Sua(SuKienDTO sk)
        {
            if (sk == null || sk.MaSK <= 0) return "Mã sự kiện không hợp lệ!";
            if (string.IsNullOrWhiteSpace(sk.TenSK)) return "Tên sự kiện không được để trống!";

            try
            {
                return SuKienDAL.Sua(sk) ? "Success" : "Sửa sự kiện thất bại!";
            }
            catch (Exception ex)
            {
                return "Lỗi CSDL khi cập nhật sự kiện: " + ex.Message;
            }
        }

        public static string Xoa(int maSK)
        {
            try
            {
                if (SuKienDAL.KiemTraCoSuatDien(maSK) > 0)
                {
                    return "Không thể xóa! Sự kiện này đang có các Suất diễn bên trong.";
                }
                return SuKienDAL.Xoa(maSK) ? "Success" : "Xóa sự kiện thất bại!";
            }
            catch (Exception ex)
            {
                return "Lỗi CSDL khi xóa sự kiện: " + ex.Message;
            }
        }
    }
}