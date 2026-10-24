using System;
using System.Data;
using QuanLySuKienVaBanVe.DAL;

namespace QuanLySuKienVaBanVe.BLL
{
    public class ThongKeBLL
    {
        public static (string DoanhThu, string SoVe, string SoSuKien, string SoDonHuy) LayThongKeCards()
        {
            try
            {
                return ThongKeDAL.LayThongKeCards();
            }
            catch
            {
                return ("0 đ", "0 vé", "0 sự kiện", "0 đơn");
            }
        }

        public static DataTable LayDoanhThu7Ngay()
        {
            try
            {
                return ThongKeDAL.LayDoanhThu7Ngay();
            }
            catch
            {
                return new DataTable();
            }
        }

        public static DataTable LayTyLeGhe()
        {
            try
            {
                return ThongKeDAL.LayTyLeGhe();
            }
            catch
            {
                return new DataTable();
            }
        }

        public static DataTable LaySuKienSapDien(int top = 5)
        {
            try
            {
                return ThongKeDAL.LaySuKienSapDien(top);
            }
            catch
            {
                return new DataTable();
            }
        }

        public static DataTable LayTopSuKienDoanhThu(int top = 5)
        {
            try
            {
                return ThongKeDAL.LayTopSuKienDoanhThu(top);
            }
            catch
            {
                return new DataTable();
            }
        }
    }
}
