using System;
using System.Data;

namespace QuanLySuKienVaBanVe.DTO
{
    public static class PhuongThucThanhToan
    {
        public const string TienMat = "TienMat";
        public const string ChuyenKhoan = "ChuyenKhoan";
        public const string TheNganHang = "TheNganHang";
    }

    /// <summary>
    /// Đối tượng truyền dữ liệu Thanh toán (DTO)
    /// Ánh xạ bảng ThanhToan trong SQL Server.
    /// </summary>
    public class ThanhToanDTO
    {
        public int MaTT { get; set; }
        public int MaDon { get; set; }
        public DateTime NgayThanhToan { get; set; }
        public decimal SoTien { get; set; }
        public string PhuongThuc { get; set; }

        public ThanhToanDTO()
        {
            NgayThanhToan = DateTime.Now;
            PhuongThuc = PhuongThucThanhToan.TienMat;
        }

        public ThanhToanDTO(int maTT, int maDon, DateTime ngayThanhToan, decimal soTien, string phuongThuc)
        {
            MaTT = maTT;
            MaDon = maDon;
            NgayThanhToan = ngayThanhToan;
            SoTien = soTien;
            PhuongThuc = phuongThuc ?? PhuongThucThanhToan.TienMat;
        }

        public ThanhToanDTO(DataRow row) : this()
        {
            if (row != null)
            {
                MaTT = row.Table.Columns.Contains("MaTT") && row["MaTT"] != DBNull.Value
                    ? Convert.ToInt32(row["MaTT"]) : 0;
                MaDon = row.Table.Columns.Contains("MaDon") && row["MaDon"] != DBNull.Value
                    ? Convert.ToInt32(row["MaDon"]) : 0;
                NgayThanhToan = row.Table.Columns.Contains("NgayThanhToan") && row["NgayThanhToan"] != DBNull.Value
                    ? Convert.ToDateTime(row["NgayThanhToan"]) : DateTime.Now;
                SoTien = row.Table.Columns.Contains("SoTien") && row["SoTien"] != DBNull.Value
                    ? Convert.ToDecimal(row["SoTien"]) : 0m;
                PhuongThuc = row.Table.Columns.Contains("PhuongThuc") && row["PhuongThuc"] != DBNull.Value
                    ? row["PhuongThuc"].ToString() : PhuongThucThanhToan.TienMat;
            }
        }
    }
}
