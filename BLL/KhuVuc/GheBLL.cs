using System;
using System.Collections.Generic;
using System.Data;
using QuanLySuKienVaBanVe.DAL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.BLL
{
    /// <summary>Nghiệp vụ Ghế ngồi: phân bổ tự động, giữ ghế tạm thời và giải phóng phiên giữ.</summary>
    public class GheBLL
    {
        public static DataTable LayDanhSachTheoKhuVuc(int maKV)
        {
            if (maKV <= 0) return new DataTable();
            return GheDAL.LayDanhSachTheoKhuVuc(maKV);
        }

        public static bool GiuGheTamThoi(int maGhe, out Guid maPhienGiu, out string errorMessage)
        {
            if (maGhe <= 0)
            {
                maPhienGiu = Guid.Empty;
                errorMessage = "Mã vị trí ghế không hợp lệ.";
                return false;
            }
            return GheDAL.GiuGheTamThoi(maGhe, out maPhienGiu, out errorMessage);
        }

        /// <summary>
        /// Phân bổ và giữ ghế tự động theo quy tắc:
        /// Ưu tiên từ các hàng ghế đầu tiên (gần sân khấu nhất) theo thứ tự thời gian giao dịch.
        /// </summary>
        public static bool GiuGheTuDongTheoKhuVuc(int maKV, int soLuong, out List<GheDTO> dsGheGiu, out Guid maPhienGiu, out string errorMessage)
        {
            if (maKV <= 0)
            {
                dsGheGiu = new List<GheDTO>();
                maPhienGiu = Guid.Empty;
                errorMessage = "Vui lòng chọn khu vực hợp lệ.";
                return false;
            }
            if (soLuong <= 0)
            {
                dsGheGiu = new List<GheDTO>();
                maPhienGiu = Guid.Empty;
                errorMessage = "Số lượng vé phải lớn hơn 0.";
                return false;
            }
            return GheDAL.GiuGheTuDongTheoKhuVuc(maKV, soLuong, out dsGheGiu, out maPhienGiu, out errorMessage);
        }

        public static bool GiaiPhongGhe(int maGhe, Guid maPhienGiu)
        {
            if (maGhe <= 0 || maPhienGiu == Guid.Empty) return false;
            return GheDAL.GiaiPhongGhe(maGhe, maPhienGiu);
        }

        public static int LayGiayConLaiGiu(IEnumerable<int> dsMaGhe, Guid maPhienGiu)
        {
            if (dsMaGhe == null || maPhienGiu == Guid.Empty) return 0;
            return GheDAL.LayGiayConLaiGiu(dsMaGhe, maPhienGiu);
        }

        public static int GiaiPhongDanhSachGhe(IEnumerable<int> dsMaGhe, Guid maPhienGiu)
        {
            if (dsMaGhe == null || maPhienGiu == Guid.Empty) return 0;
            return GheDAL.GiaiPhongDanhSachGhe(dsMaGhe, maPhienGiu);
        }
    }
}
