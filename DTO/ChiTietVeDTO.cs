using System;
using System.Data;

namespace QuanLySuKienVaBanVe.DTO
{
    public static class TrangThaiChiTietVe
    {
        public const string HopLe = "HopLe";
        public const string DaHuy = "DaHuy";
    }

    /// <summary>
    /// Đối tượng truyền dữ liệu Chi tiết vé (DTO)
    /// Ánh xạ bảng ChiTietVe trong SQL Server kèm thông tin bổ sung khi JOIN.
    /// </summary>
    public class ChiTietVeDTO
    {
        public int MaCT { get; set; }
        public int MaDon { get; set; }
        public int MaGhe { get; set; }
        public decimal GiaVe { get; set; }
        public string MaVeCode { get; set; }
        public string TrangThai { get; set; }

        // Thuộc tính mở rộng khi JOIN với bảng Ghe và KhuVuc
        public string MaGheHienThi { get; set; }
        public string TenKV { get; set; }
        public string HangVe { get; set; }

        public ChiTietVeDTO()
        {
            MaVeCode = string.Empty;
            TrangThai = TrangThaiChiTietVe.HopLe;
            MaGheHienThi = string.Empty;
            TenKV = string.Empty;
            HangVe = string.Empty;
        }

        public ChiTietVeDTO(int maCT, int maDon, int maGhe, decimal giaVe, string maVeCode, string trangThai)
            : this()
        {
            MaCT = maCT;
            MaDon = maDon;
            MaGhe = maGhe;
            GiaVe = giaVe;
            MaVeCode = maVeCode ?? string.Empty;
            TrangThai = trangThai ?? TrangThaiChiTietVe.HopLe;
        }

        public ChiTietVeDTO(DataRow row) : this()
        {
            if (row != null)
            {
                MaCT = row.Table.Columns.Contains("MaCT") && row["MaCT"] != DBNull.Value
                    ? Convert.ToInt32(row["MaCT"]) : 0;
                MaDon = row.Table.Columns.Contains("MaDon") && row["MaDon"] != DBNull.Value
                    ? Convert.ToInt32(row["MaDon"]) : 0;
                MaGhe = row.Table.Columns.Contains("MaGhe") && row["MaGhe"] != DBNull.Value
                    ? Convert.ToInt32(row["MaGhe"]) : 0;
                GiaVe = row.Table.Columns.Contains("GiaVe") && row["GiaVe"] != DBNull.Value
                    ? Convert.ToDecimal(row["GiaVe"]) : 0m;
                MaVeCode = row.Table.Columns.Contains("MaVeCode") && row["MaVeCode"] != DBNull.Value
                    ? row["MaVeCode"].ToString() : string.Empty;
                TrangThai = row.Table.Columns.Contains("TrangThai") && row["TrangThai"] != DBNull.Value
                    ? row["TrangThai"].ToString() : TrangThaiChiTietVe.HopLe;

                if (row.Table.Columns.Contains("MaGheHienThi") && row["MaGheHienThi"] != DBNull.Value)
                    MaGheHienThi = row["MaGheHienThi"].ToString();
                if (row.Table.Columns.Contains("TenKV") && row["TenKV"] != DBNull.Value)
                    TenKV = row["TenKV"].ToString();
                if (row.Table.Columns.Contains("HangVe") && row["HangVe"] != DBNull.Value)
                    HangVe = row["HangVe"].ToString();
            }
        }
    }
}
