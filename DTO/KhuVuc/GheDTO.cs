using System;
using System.Data;

namespace QuanLySuKienVaBanVe.DTO
{
    /// <summary>Các trạng thái ghế ngồi: Trong, DangGiu, DaBan.</summary>
    public static class TrangThaiGhe
    {
        public const string Trong = "Trong";
        public const string DangGiu = "DangGiu";
        public const string DaBan = "DaBan";
    }

    /// <summary>
    /// Đối tượng truyền dữ liệu Ghế ngồi (DTO)
    /// Ánh xạ bảng Ghe trong SQL Server.
    /// </summary>
    public class GheDTO
    {
        public int MaGhe { get; set; }
        public int MaKV { get; set; }
        public string Hang { get; set; }
        public int Cot { get; set; }
        public string MaGheHienThi { get; set; }
        public string TrangThai { get; set; }
        /// <summary>Thời điểm bắt đầu phiên giữ chỗ tạm thời (hết hạn sau 10 phút).</summary>
        public DateTime? ThoiGianGiu { get; set; }

        // Thông tin mở rộng từ KhuVuc
        public string TenKV { get; set; }
        public string HangVe { get; set; }
        public decimal GiaVe { get; set; }

        public GheDTO()
        {
            TrangThai = TrangThaiGhe.Trong;
        }

        public GheDTO(DataRow row)
        {
            if (row != null)
            {
                MaGhe = row.Table.Columns.Contains("MaGhe") && row["MaGhe"] != DBNull.Value
                    ? Convert.ToInt32(row["MaGhe"]) : 0;
                MaKV = row.Table.Columns.Contains("MaKV") && row["MaKV"] != DBNull.Value
                    ? Convert.ToInt32(row["MaKV"]) : 0;
                Hang = row.Table.Columns.Contains("Hang") ? row["Hang"]?.ToString() : "";
                Cot = row.Table.Columns.Contains("Cot") && row["Cot"] != DBNull.Value
                    ? Convert.ToInt32(row["Cot"]) : 0;
                MaGheHienThi = row.Table.Columns.Contains("MaGheHienThi")
                    ? row["MaGheHienThi"]?.ToString() : $"{Hang}{Cot}";
                TrangThai = row.Table.Columns.Contains("TrangThai")
                    ? row["TrangThai"]?.ToString() : TrangThaiGhe.Trong;
                ThoiGianGiu = row.Table.Columns.Contains("ThoiGianGiu") && row["ThoiGianGiu"] != DBNull.Value
                    ? Convert.ToDateTime(row["ThoiGianGiu"]) : (DateTime?)null;

                TenKV = row.Table.Columns.Contains("TenKV") ? row["TenKV"]?.ToString() : "";
                HangVe = row.Table.Columns.Contains("HangVe") ? row["HangVe"]?.ToString() : "";
                GiaVe = row.Table.Columns.Contains("GiaVe") && row["GiaVe"] != DBNull.Value
                    ? Convert.ToDecimal(row["GiaVe"]) : 0m;
            }
        }
    }
}
