using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;
using System;

namespace QuanLySuKienVaBanVe.UnitTests
{
    [TestClass]
    public class BaoCaoThongKeTests
    {
        [TestMethod]
        public void BaoCao_ChuanHoaNgay_TuNgayLonHonDenNgay_TuDongDaoNgay()
        {
            // Arrange
            DateTime tuNgay = new DateTime(2026, 12, 31);
            DateTime denNgay = new DateTime(2026, 1, 1);

            // Act
            BaoCaoBLL.ChuanHoaNgay(ref tuNgay, ref denNgay);

            // Assert
            Assert.AreEqual(new DateTime(2026, 1, 1), tuNgay, "Từ ngày phải được hoán vị về mốc thời gian nhỏ hơn");
            Assert.AreEqual(new DateTime(2026, 12, 31), denNgay, "Đến ngày phải được hoán vị về mốc thời gian lớn hơn");
        }

        [TestMethod]
        public void BaoCao_ChuanHoaNgay_HopLe_GiuNguyenThuTu()
        {
            // Arrange
            DateTime tuNgay = new DateTime(2026, 5, 1);
            DateTime denNgay = new DateTime(2026, 5, 31);

            // Act
            BaoCaoBLL.ChuanHoaNgay(ref tuNgay, ref denNgay);

            // Assert
            Assert.AreEqual(new DateTime(2026, 5, 1), tuNgay);
            Assert.AreEqual(new DateTime(2026, 5, 31), denNgay);
        }

        [TestMethod]
        public void BaoCao_TinhTangTruong_KyTruocBangKhong_TraVeNull()
        {
            // Act: Kỳ trước doanh thu = 0đ -> Không thể chia cho 0
            decimal? tangTruong = BaoCaoBLL.TinhTangTruong(5000000m, 0m);

            // Assert
            Assert.IsNull(tangTruong, "Kỳ trước bằng 0 tỷ lệ tăng trưởng phải là null để tránh chia cho 0");
        }

        [TestMethod]
        public void BaoCao_TinhTangTruong_DoanhThuTang_TraVePhanTramDuong()
        {
            // Act: Kỳ trước 10tr, kỳ này 15tr -> Tăng +50%
            decimal? tangTruong = BaoCaoBLL.TinhTangTruong(15000000m, 10000000m);

            // Assert
            Assert.IsNotNull(tangTruong);
            Assert.AreEqual(50m, tangTruong.Value);
        }

        [TestMethod]
        public void BaoCao_TinhTangTruong_DoanhThuGiam_TraVePhanTramAm()
        {
            // Act: Kỳ trước 10tr, kỳ này 8tr -> Giảm -20%
            decimal? tangTruong = BaoCaoBLL.TinhTangTruong(8000000m, 10000000m);

            // Assert
            Assert.IsNotNull(tangTruong);
            Assert.AreEqual(-20m, tangTruong.Value);
        }

        [TestMethod]
        public void BaoCao_KpiBaoCao_KhoiTaoDayDu_HopLe()
        {
            // Arrange & Act
            KpiBaoCao kpi = new KpiBaoCao
            {
                TongDoanhThu = 24500000m,
                TongVeDaBan = 48,
                TyLeLapDay = 88.5m,
                TangTruongDoanhThu = 15.2m,
                TangTruongVe = 10.0m,
                ChenhLechLapDay = 5.5m
            };

            // Assert
            Assert.AreEqual(24500000m, kpi.TongDoanhThu);
            Assert.AreEqual(48, kpi.TongVeDaBan);
            Assert.AreEqual(88.5m, kpi.TyLeLapDay);
            Assert.AreEqual(15.2m, kpi.TangTruongDoanhThu);
            Assert.AreEqual(10.0m, kpi.TangTruongVe);
            Assert.AreEqual(5.5m, kpi.ChenhLechLapDay);
        }
    }
}
