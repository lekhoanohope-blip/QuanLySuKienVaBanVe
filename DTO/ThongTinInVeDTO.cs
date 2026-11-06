using System;
using System.Data;
using System.Globalization;

namespace QuanLySuKienVaBanVe.DTO
{
    public class ThongTinInVeDTO
    {
        public int MaDon { get; set; }
        public int MaCT { get; set; }
        public int MaGhe { get; set; }

        public string MaVeCode { get; set; }

        public string TenSuKien { get; set; }
        public string DiaDiem { get; set; }

        public string Zone { get; set; }

        public string Hang { get; set; }

        public int Cot { get; set; }

        public DateTime ThoiGianBatDau { get; set; }
        public decimal GiaVe { get; set; }
        public string TenKhachHang { get; set; }

        public ThongTinInVeDTO()
        {
            MaVeCode = string.Empty;
            TenSuKien = string.Empty;
            DiaDiem = string.Empty;
            Zone = string.Empty;
            Hang = string.Empty;
            TenKhachHang = string.Empty;
            ThoiGianBatDau = DateTime.MinValue;
        }

        public string SoGheIn
        {
            get { return Cot.ToString("D2", CultureInfo.InvariantCulture); }
        }

        public string HangIn
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Hang)) return "-";
                return Hang.Trim().ToUpperInvariant();
            }
        }

        public string ZoneIn
        {
            get { return string.IsNullOrWhiteSpace(Zone) ? "-" : Zone.Trim(); }
        }

        public string NgayIn
        {
            get
            {
                return ThoiGianBatDau == DateTime.MinValue
                    ? string.Empty
                    : ThoiGianBatDau.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            }
        }

        public string GioIn
        {
            get
            {
                return ThoiGianBatDau == DateTime.MinValue
                    ? string.Empty
                    : ThoiGianBatDau.ToString("HH:mm", CultureInfo.InvariantCulture);
            }
        }

        public string SoMaVe
        {
            get
            {
                if (MaCT > 0) return MaCT.ToString(CultureInfo.InvariantCulture);

                string tail = LayChuoiSoCuoi(MaVeCode);
                if (!string.IsNullOrEmpty(tail)) return tail;

                return MaDon > 0 ? MaDon.ToString(CultureInfo.InvariantCulture) : "0";
            }
        }

        public string MaBarcode
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(MaVeCode)) return MaVeCode.Trim();
                return MaCT > 0 ? MaCT.ToString(CultureInfo.InvariantCulture) : "0";
            }
        }

        private static string LayChuoiSoCuoi(string chuoi)
        {
            if (string.IsNullOrEmpty(chuoi)) return string.Empty;

            int i = chuoi.Length - 1;
            while (i >= 0 && !char.IsDigit(chuoi[i])) i--;
            if (i < 0) return string.Empty;

            int end = i;
            while (i >= 0 && char.IsDigit(chuoi[i])) i--;
            return chuoi.Substring(i + 1, end - i);
        }

        public static ThongTinInVeDTO TuDongVaChiTietVe(DataRow donVe, DataRow chiTietVe)
        {
            ThongTinInVeDTO ve = new ThongTinInVeDTO();

            if (chiTietVe != null)
            {
                ve.MaCT = LayInt(chiTietVe, "MaCT");
                ve.MaDon = LayInt(chiTietVe, "MaDon");
                ve.MaGhe = LayInt(chiTietVe, "MaGhe");
                ve.Cot = LayInt(chiTietVe, "Cot");
                ve.GiaVe = LayDecimal(chiTietVe, "GiaVe");
                ve.Hang = LayString(chiTietVe, "Hang");
                ve.Zone = LayString(chiTietVe, "TenKV");
                ve.MaVeCode = LayString(chiTietVe, "MaVeCode");

                if (string.IsNullOrWhiteSpace(ve.Hang))
                    ve.Hang = TachChu(DanhSachHangVe(chiTietVe));
            }

            if (donVe != null)
            {
                if (ve.MaDon == 0) ve.MaDon = LayInt(donVe, "MaDon");
                ve.TenSuKien = LayString(donVe, "TenSK");
                ve.DiaDiem = LayString(donVe, "DiaDiem");
                ve.TenKhachHang = LayString(donVe, "TenKH");

                DateTime gio;
                if (DateTime.TryParse(LayString(donVe, "ThoiGianBatDau"), out gio))
                    ve.ThoiGianBatDau = gio;
            }

            return ve;
        }

        private static string DanhSachHangVe(DataRow chiTietVe)
        {
            string hienThi = LayString(chiTietVe, "MaGheHienThi");
            return string.IsNullOrEmpty(hienThi) ? LayString(chiTietVe, "HangVe") : hienThi;
        }

        private static string TachChu(string chuoi)
        {
            if (string.IsNullOrEmpty(chuoi)) return string.Empty;
            int i = 0;
            while (i < chuoi.Length && !char.IsDigit(chuoi[i])) i++;
            return chuoi.Substring(0, i).Trim();
        }

        private static string LayString(DataRow row, string cot)
        {
            return row != null && row.Table.Columns.Contains(cot) && row[cot] != DBNull.Value
                ? row[cot].ToString()
                : string.Empty;
        }

        private static int LayInt(DataRow row, string cot)
        {
            string giaTri = LayString(row, cot);
            int kq;
            return int.TryParse(giaTri, NumberStyles.Integer, CultureInfo.InvariantCulture, out kq) ? kq : 0;
        }

        private static decimal LayDecimal(DataRow row, string cot)
        {
            string giaTri = LayString(row, cot);
            decimal kq;
            return decimal.TryParse(giaTri, NumberStyles.Number, CultureInfo.InvariantCulture, out kq) ? kq : 0m;
        }
    }
}
