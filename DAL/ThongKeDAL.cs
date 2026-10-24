using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLySuKienVaBanVe.DAL
{
    public class ThongKeDAL
    {
        public static (string DoanhThu, string SoVe, string SoSuKien, string SoDonHuy) LayThongKeCards()
        {
            string doanhThu = "0 đ";
            string soVe = "0 vé";
            string soSuKien = "0 sự kiện";
            string soDonHuy = "0 đơn";

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sqlDoanhThu = "SELECT SUM(TongTien) FROM DonVe WHERE TrangThai = N'DaThanhToan'";
                object dtObj = ExecuteScalar(conn, sqlDoanhThu);
                if (dtObj != null && dtObj != DBNull.Value)
                    doanhThu = Convert.ToDouble(dtObj).ToString("N0") + " đ";

                string sqlSoVe = "SELECT COUNT(*) FROM ChiTietVe WHERE TrangThai = N'HopLe'";
                object veObj = ExecuteScalar(conn, sqlSoVe);
                if (veObj != null && veObj != DBNull.Value)
                    soVe = veObj.ToString() + " vé";

                string sqlSuKien = "SELECT COUNT(*) FROM SuKien WHERE TrangThai = N'DangDien'";
                object skObj = ExecuteScalar(conn, sqlSuKien);
                if (skObj != null && skObj != DBNull.Value)
                    soSuKien = skObj.ToString() + " sự kiện";

                string sqlDonHuy = "SELECT COUNT(*) FROM DonVe WHERE TrangThai = N'DaHuy'";
                object dhObj = ExecuteScalar(conn, sqlDonHuy);
                if (dhObj != null && dhObj != DBNull.Value)
                    soDonHuy = dhObj.ToString() + " đơn";
            }

            return (doanhThu, soVe, soSuKien, soDonHuy);
        }

        public static DataTable LayDoanhThu7Ngay()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
                    SELECT CAST(NgayDat AS DATE) as Ngay, SUM(TongTien) as DoanhThu 
                    FROM DonVe 
                    WHERE TrangThai = N'DaThanhToan' AND NgayDat >= DATEADD(day, -7, GETDATE())
                    GROUP BY CAST(NgayDat AS DATE) ORDER BY Ngay ASC";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }
            return dt;
        }

        public static DataTable LayTyLeGhe()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT TrangThai, COUNT(*) as SoLuong FROM Ghe GROUP BY TrangThai";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }
            return dt;
        }

        public static DataTable LaySuKienSapDien(int top = 5)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = $@"
                    SELECT TOP {top} sk.TenSK AS [Tên sự kiện], sd.ThoiGianBatDau AS [Thời gian], sd.DiaDiem AS [Địa điểm], 
                           CASE 
                               WHEN sd.TrangThai = N'DaKetThuc' THEN N'Hết vé'
                               WHEN sd.ThoiGianBatDau <= GETDATE() THEN N'Đang diễn ra'
                               ELSE N'Còn vé'
                           END AS [Tình trạng]
                    FROM SuatDien sd
                    JOIN SuKien sk ON sd.MaSK = sk.MaSK
                    WHERE sd.ThoiGianBatDau > DATEADD(day, -1, GETDATE())
                    ORDER BY sd.ThoiGianBatDau ASC";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }
            return dt;
        }

        public static DataTable LayTopSuKienDoanhThu(int top = 5)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = $@"
                    SELECT TOP {top} sk.TenSK AS [Tên sự kiện], SUM(dv.TongTien) AS [Doanh thu]
                    FROM DonVe dv
                    JOIN SuatDien sd ON dv.MaSuat = sd.MaSuat
                    JOIN SuKien sk ON sd.MaSK = sk.MaSK
                    WHERE dv.TrangThai = N'DaThanhToan'
                    GROUP BY sk.TenSK
                    ORDER BY [Doanh thu] DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }
            return dt;
        }

        private static object ExecuteScalar(SqlConnection conn, string query)
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                return cmd.ExecuteScalar();
            }
        }
    }
}
