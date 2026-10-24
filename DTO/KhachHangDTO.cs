using System;
using System.Data;

namespace QuanLySuKienVaBanVe.DTO
{
    /// <summary>
    /// Đối tượng truyền dữ liệu Khách hàng (DTO)
    /// Ánh xạ với bảng KhachHang trong SQL Server.
    /// </summary>
    public class KhachHangDTO
    {
        public int MaKH { get; set; }
        public string TenKH { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }

        public KhachHangDTO()
        {
            TenKH = string.Empty;
            SDT = string.Empty;
            Email = string.Empty;
        }

        public KhachHangDTO(int maKH, string tenKH, string sdt, string email)
        {
            MaKH = maKH;
            TenKH = tenKH ?? string.Empty;
            SDT = sdt ?? string.Empty;
            Email = email ?? string.Empty;
        }

        public KhachHangDTO(DataRow row)
        {
            if (row != null)
            {
                MaKH = row.Table.Columns.Contains("MaKH") && row["MaKH"] != DBNull.Value
                    ? Convert.ToInt32(row["MaKH"])
                    : 0;
                TenKH = row.Table.Columns.Contains("TenKH") && row["TenKH"] != DBNull.Value
                    ? row["TenKH"].ToString()
                    : string.Empty;
                SDT = row.Table.Columns.Contains("SDT") && row["SDT"] != DBNull.Value
                    ? row["SDT"].ToString()
                    : string.Empty;
                Email = row.Table.Columns.Contains("Email") && row["Email"] != DBNull.Value
                    ? row["Email"].ToString()
                    : string.Empty;
            }
        }
    }
}
