using System;
using System.Data;
using System.Data.SqlClient;
using QuanLySuKienVaBanVe.DAL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.BLL
{
    public class KhuVucBLL
    {
        public static DataTable LayDanhSachTheoSuat(int maSuat)
        {
            if (maSuat <= 0) return new DataTable();
            return KhuVucDAL.LayDanhSachTheoSuat(maSuat);
        }

        public static KhuVucDTO LayTheoMa(int maKV)
        {
            if (maKV <= 0) return null;
            return KhuVucDAL.LayTheoMa(maKV);
        }

        public static string Them(KhuVucDTO kv)
        {
            // Kiểm tra phân quyền: Chỉ Quản lý mới được phép thêm khu vực
            if (!PhienDangNhap.LaQuanLy)
            {
                return "Bạn không có quyền thực hiện thao tác này! Chỉ Quản lý mới được phép thêm khu vực.";
            }

            if (kv == null)
            {
                return "Dữ liệu khu vực không hợp lệ!";
            }

            if (kv.MaSuat <= 0)
            {
                return "Vui lòng chọn Suất diễn hợp lệ!";
            }

            if (string.IsNullOrWhiteSpace(kv.TenKV))
            {
                return "Tên khu vực không được để trống!";
            }

            if (kv.GiaVe < 0)
            {
                return "Giá vé phải lớn hơn hoặc bằng 0!";
            }

            if (kv.TongSoGhe <= 0 || kv.TongSoGhe > 10000)
            {
                return "Sức chứa phải là số nguyên dương từ 1 đến 10.000!";
            }

            if (string.IsNullOrWhiteSpace(kv.HangVe))
            {
                kv.HangVe = "VIP";
            }

            try
            {
                int newMaKV = KhuVucDAL.Them(kv);
                if (newMaKV > 0)
                {
                    kv.MaKV = newMaKV;
                    return "Success";
                }
                return "Thêm khu vực thất bại!";
            }
            catch (SqlException ex)
            {
                return "Lỗi CSDL khi thêm khu vực: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Lỗi khi thêm khu vực: " + ex.Message;
            }
        }

        public static string Sua(KhuVucDTO kv)
        {
            // Kiểm tra phân quyền: Chỉ Quản lý mới được phép cập nhật khu vực
            if (!PhienDangNhap.LaQuanLy)
            {
                return "Bạn không có quyền thực hiện thao tác này! Chỉ Quản lý mới được phép cập nhật khu vực.";
            }

            if (kv == null || kv.MaKV <= 0)
            {
                return "Mã khu vực không hợp lệ!";
            }

            if (string.IsNullOrWhiteSpace(kv.TenKV))
            {
                return "Tên khu vực không được để trống!";
            }

            if (kv.GiaVe < 0)
            {
                return "Giá vé phải lớn hơn hoặc bằng 0!";
            }

            if (string.IsNullOrWhiteSpace(kv.HangVe))
            {
                kv.HangVe = "VIP";
            }

            try
            {
                return KhuVucDAL.Sua(kv) ? "Success" : "Cập nhật khu vực thất bại!";
            }
            catch (SqlException ex)
            {
                return "Lỗi CSDL khi cập nhật khu vực: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Lỗi khi cập nhật khu vực: " + ex.Message;
            }
        }

        public static string Xoa(int maKV)
        {
            // Kiểm tra phân quyền: Chỉ Quản lý mới được phép xóa khu vực
            if (!PhienDangNhap.LaQuanLy)
            {
                return "Bạn không có quyền thực hiện thao tác này! Chỉ Quản lý mới được phép xóa khu vực.";
            }

            if (maKV <= 0)
            {
                return "Mã khu vực không hợp lệ!";
            }

            try
            {
                // Kiểm tra trước: kiểm tra tham chiếu trong ChiTietVe (bao gồm cả vé đã bán, đã hủy)
                if (KhuVucDAL.DemSoVeThamChieu(maKV) > 0)
                {
                    return "Không thể xóa! Khu vực này đã có vé được phát hành (bao gồm cả vé đã hủy).";
                }

                return KhuVucDAL.Xoa(maKV) ? "Success" : "Xóa khu vực thất bại!";
            }
            catch (SqlException ex)
            {
                // Lớp bảo vệ cuối cùng: FK constraint violation trong trường hợp thao tác đồng thời
                if (ex.Number == 547 || ex.Message.Contains("FK_ChiTietVe_Ghe"))
                {
                    return "Không thể xóa! Khu vực này đã có vé liên kết với ghế trong hệ thống.";
                }
                return "Lỗi CSDL khi xóa khu vực: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Lỗi khi xóa khu vực: " + ex.Message;
            }
        }
    }
}
