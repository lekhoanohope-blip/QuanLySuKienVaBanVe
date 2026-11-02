using System;
using System.Collections.Generic;
using System.Data;
using QuanLySuKienVaBanVe.DAL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.BLL
{
    public class GheBLL
    {
        public static DataTable LayDanhSachTheoKhuVuc(int maKV)
        {
            if (maKV <= 0) return new DataTable();
            return GheDAL.LayDanhSachTheoKhuVuc(maKV);
        }

        public static bool GiuGheTamThoi(int maGhe, out string errorMessage)
        {
            if (maGhe <= 0)
            {
                errorMessage = "Mã vị trí ghế không hợp lệ.";
                return false;
            }
            return GheDAL.GiuGheTamThoi(maGhe, out errorMessage);
        }

        /// <summary>
        /// Phân bổ và giữ ghế tự động theo quy tắc:
        /// "Thanh toán càng sớm, ghế càng gần sân khấu: Hệ thống sẽ tự động xếp chỗ ưu tiên
        /// từ các hàng ghế đầu tiên (gần sân khấu nhất) lùi dần về sau theo thứ tự thời gian giao dịch".
        /// </summary>
        public static bool GiuGheTuDongTheoKhuVuc(int maKV, int soLuong, out List<GheDTO> dsGheGiu, out string errorMessage)
        {
            if (maKV <= 0)
            {
                dsGheGiu = new List<GheDTO>();
                errorMessage = "Vui lòng chọn khu vực hợp lệ.";
                return false;
            }
            if (soLuong <= 0)
            {
                dsGheGiu = new List<GheDTO>();
                errorMessage = "Số lượng vé phải lớn hơn 0.";
                return false;
            }
            return GheDAL.GiuGheTuDongTheoKhuVuc(maKV, soLuong, out dsGheGiu, out errorMessage);
        }

        public static bool GiaiPhongGhe(int maGhe)
        {
            if (maGhe <= 0) return false;
            return GheDAL.GiaiPhongGhe(maGhe);
        }

        public static int GiaiPhongDanhSachGhe(IEnumerable<int> dsMaGhe)
        {
            if (dsMaGhe == null) return 0;
            return GheDAL.GiaiPhongDanhSachGhe(dsMaGhe);
        }
    }
}
