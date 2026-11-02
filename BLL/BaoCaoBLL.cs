using System;
using System.Data;
using QuanLySuKienVaBanVe.DAL;

namespace QuanLySuKienVaBanVe.BLL
{
    /// <summary>Các chỉ số tổng quan hiển thị trên thẻ KPI của FrmBaoCao.</summary>
    public class KpiBaoCao
    {
        public decimal TongDoanhThu { get; set; }
        public int TongVeDaBan { get; set; }

        /// <summary>Tỷ lệ lấp đầy bình quân của các suất diễn bắt đầu trong kỳ (null = không có ghế).</summary>
        public decimal? TyLeLapDay { get; set; }

        /// <summary>% tăng/giảm so với kỳ trước có độ dài bằng kỳ hiện tại (null = kỳ trước bằng 0).</summary>
        public decimal? TangTruongDoanhThu { get; set; }
        public decimal? TangTruongVe { get; set; }

        /// <summary>Chênh lệch tỷ lệ lấp đầy so với kỳ trước, tính bằng điểm phần trăm (null = không tính được).</summary>
        public decimal? ChenhLechLapDay { get; set; }
    }

    /// <summary>
    /// Tầng nghiệp vụ báo cáo: chỉ chuẩn hóa đầu vào rồi gọi DAL, không chứa SQL.
    /// - Từ ngày &gt; đến ngày thì tự đảo lại, không bắt người dùng nhập lại.
    /// - Chế độ gom theo thời gian chỉ nhận Ngay/Thang/Nam, còn lại mặc định Ngay.
    /// - Kỳ trước = khoảng thời gian có cùng độ dài, nằm ngay trước kỳ hiện tại.
    /// </summary>
    public static class BaoCaoBLL
    {
        private static void ChuanHoaNgay(ref DateTime tuNgay, ref DateTime denNgay)
        {
            if (tuNgay.Date > denNgay.Date)
            {
                DateTime tam = tuNgay;
                tuNgay = denNgay;
                denNgay = tam;
            }
        }

        private static string ChuanHoaCheDo(string cheDo)
        {
            string c = (cheDo ?? "").Trim().ToLowerInvariant();
            if (c == "thang" || c == "nam") return c;
            return "ngay";
        }

        private static decimal? TinhTangTruong(decimal hienTai, decimal truoc)
        {
            if (truoc == 0m) return null;
            return (hienTai - truoc) / truoc * 100m;
        }

        public static KpiBaoCao LayKpi(int maSK, int maSuat, DateTime tuNgay, DateTime denNgay)
        {
            ChuanHoaNgay(ref tuNgay, ref denNgay);

            int soNgay = (denNgay.Date - tuNgay.Date).Days + 1;
            DateTime denTruoc = tuNgay.Date.AddDays(-1);
            DateTime tuTruoc = denTruoc.AddDays(-(soNgay - 1));

            decimal doanhThu = BaoCaoDAL.TongDoanhThu(maSK, maSuat, tuNgay, denNgay);
            int soVe = BaoCaoDAL.TongVeDaBan(maSK, maSuat, tuNgay, denNgay);
            decimal doanhThuTruoc = BaoCaoDAL.TongDoanhThu(maSK, maSuat, tuTruoc, denTruoc);
            int soVeTruoc = BaoCaoDAL.TongVeDaBan(maSK, maSuat, tuTruoc, denTruoc);
            decimal? lapDay = BaoCaoDAL.TyLeLapDayBinhQuan(maSK, maSuat, tuNgay, denNgay);
            decimal? lapDayTruoc = BaoCaoDAL.TyLeLapDayBinhQuan(maSK, maSuat, tuTruoc, denTruoc);

            return new KpiBaoCao
            {
                TongDoanhThu = doanhThu,
                TongVeDaBan = soVe,
                TyLeLapDay = lapDay,
                TangTruongDoanhThu = TinhTangTruong(doanhThu, doanhThuTruoc),
                TangTruongVe = TinhTangTruong((decimal)soVe, (decimal)soVeTruoc),
                ChenhLechLapDay = lapDay.HasValue && lapDayTruoc.HasValue
                    ? lapDay.Value - lapDayTruoc.Value
                    : (decimal?)null
            };
        }

        public static DataTable DoanhThuTheoSuKien(int maSK, int maSuat, DateTime tuNgay, DateTime denNgay)
        {
            ChuanHoaNgay(ref tuNgay, ref denNgay);
            return BaoCaoDAL.DoanhThuTheoSuKien(maSK, maSuat, tuNgay, denNgay);
        }

        public static DataTable DoanhThuTheoLoaiVe(int maSK, int maSuat, DateTime tuNgay, DateTime denNgay)
        {
            ChuanHoaNgay(ref tuNgay, ref denNgay);
            return BaoCaoDAL.DoanhThuTheoLoaiVe(maSK, maSuat, tuNgay, denNgay);
        }

        public static DataTable DoanhThuTheoThoiGian(int maSK, int maSuat, DateTime tuNgay, DateTime denNgay, string cheDo)
        {
            ChuanHoaNgay(ref tuNgay, ref denNgay);
            return BaoCaoDAL.DoanhThuTheoThoiGian(maSK, maSuat, tuNgay, denNgay, ChuanHoaCheDo(cheDo));
        }

        public static DataTable TyLeLapDayTheoKhuVuc(int maSK, int maSuat)
        {
            return BaoCaoDAL.TyLeLapDayTheoKhuVuc(maSK, maSuat);
        }

        public static DataTable DanhSachDonVe(int maSK, int maSuat, DateTime tuNgay, DateTime denNgay)
        {
            ChuanHoaNgay(ref tuNgay, ref denNgay);
            return BaoCaoDAL.DanhSachDonVe(maSK, maSuat, tuNgay, denNgay);
        }

        public static DataTable DanhSachKhachTheoSuat(int maSuat)
        {
            return BaoCaoDAL.DanhSachKhachTheoSuat(maSuat);
        }
    }
}
