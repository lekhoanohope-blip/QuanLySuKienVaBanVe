using System;
using System.Data;

namespace QuanLySuKienVaBanVe.DTO
{
    public static class TrangThaiDonVe
    {
        public const string DaThanhToan = "DaThanhToan";
        public const string DaHuy = "DaHuy";
        public const string DaDoi = "DaDoi";
    }

    /// <summary>
    /// Đối tượng truyền dữ liệu Đơn vé (DTO)
    /// Ánh xạ bảng DonVe trong SQL Server.
    /// </summary>
    public class DonVeDTO
    {
        public int MaDon { get; set; }
        public int MaKH { get; set; }
        public int MaSuat { get; set; }
        public int MaTK { get; set; }
        public int? MaDonGoc { get; set; }
        public DateTime NgayDat { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }

        public DonVeDTO()
        {
            NgayDat = DateTime.Now;
            TrangThai = TrangThaiDonVe.DaThanhToan;
        }

        public DonVeDTO(int maDon, int maKH, int maSuat, int maTK, int? maDonGoc, DateTime ngayDat, decimal tongTien, string trangThai)
        {
            MaDon = maDon;
            MaKH = maKH;
            MaSuat = maSuat;
            MaTK = maTK;
            MaDonGoc = maDonGoc;
            NgayDat = ngayDat;
            TongTien = tongTien;
            TrangThai = trangThai ?? TrangThaiDonVe.DaThanhToan;
        }

        public DonVeDTO(DataRow row)
        {
            if (row != null)
            {
                MaDon = row.Table.Columns.Contains("MaDon") && row["MaDon"] != DBNull.Value
                    ? Convert.ToInt32(row["MaDon"]) : 0;
                MaKH = row.Table.Columns.Contains("MaKH") && row["MaKH"] != DBNull.Value
                    ? Convert.ToInt32(row["MaKH"]) : 0;
                MaSuat = row.Table.Columns.Contains("MaSuat") && row["MaSuat"] != DBNull.Value
                    ? Convert.ToInt32(row["MaSuat"]) : 0;
                MaTK = row.Table.Columns.Contains("MaTK") && row["MaTK"] != DBNull.Value
                    ? Convert.ToInt32(row["MaTK"]) : 0;
                MaDonGoc = row.Table.Columns.Contains("MaDonGoc") && row["MaDonGoc"] != DBNull.Value
                    ? (int?)Convert.ToInt32(row["MaDonGoc"]) : null;
                NgayDat = row.Table.Columns.Contains("NgayDat") && row["NgayDat"] != DBNull.Value
                    ? Convert.ToDateTime(row["NgayDat"]) : DateTime.Now;
                TongTien = row.Table.Columns.Contains("TongTien") && row["TongTien"] != DBNull.Value
                    ? Convert.ToDecimal(row["TongTien"]) : 0m;
                TrangThai = row.Table.Columns.Contains("TrangThai") && row["TrangThai"] != DBNull.Value
                    ? row["TrangThai"].ToString() : TrangThaiDonVe.DaThanhToan;
            }
        }
    }
}
