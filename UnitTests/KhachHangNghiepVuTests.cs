using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;
using System;

namespace QuanLySuKienVaBanVe.UnitTests
{
    [TestClass]
    public class KhachHangNghiepVuTests
    {
        [TestMethod]
        public void KhachHang_ThemMoi_DoiTuongNull_TraVeThatBai()
        {
            // Act
            KhachHangResult res = KhachHangBLL.Them(null);

            // Assert
            Assert.IsFalse(res.Success);
            Assert.AreEqual("Dữ liệu khách hàng không được để trống.", res.Message);
        }

        [TestMethod]
        public void KhachHang_ThemMoi_ThieuTen_TraVeThatBai()
        {
            // Arrange
            KhachHangDTO kh = new KhachHangDTO
            {
                TenKH = "   ",
                SDT = "0912345678"
            };

            // Act
            KhachHangResult res = KhachHangBLL.Them(kh);

            // Assert
            Assert.IsFalse(res.Success);
            Assert.AreEqual("Họ và tên khách hàng không được để trống.", res.Message);
        }

        [TestMethod]
        public void KhachHang_ThemMoi_ThieuSDT_TraVeThatBai()
        {
            // Arrange
            KhachHangDTO kh = new KhachHangDTO
            {
                TenKH = "Lê Thị B",
                SDT = ""
            };

            // Act
            KhachHangResult res = KhachHangBLL.Them(kh);

            // Assert
            Assert.IsFalse(res.Success);
            Assert.AreEqual("Số điện thoại không được để trống.", res.Message);
        }

        [TestMethod]
        public void KhachHang_LoaiBoDauTiengViet_ChuoiCoDau_ChuyenThanhKhongDau()
        {
            // Arrange
            string chuoiGoc = "Nguyễn Văn Đạt - Vé Sự Kiện 2026";

            // Act
            string khongDau = KhachHangBLL.LoaiBoDauTiengViet(chuoiGoc);

            // Assert
            Assert.AreEqual("Nguyen Van Dat - Ve Su Kien 2026", khongDau);
        }

        [TestMethod]
        public void KhachHang_LoaiBoDauTiengViet_ChuoiRong_TraVeRong()
        {
            // Act & Assert
            Assert.AreEqual(string.Empty, KhachHangBLL.LoaiBoDauTiengViet(null));
            Assert.AreEqual(string.Empty, KhachHangBLL.LoaiBoDauTiengViet(""));
            Assert.AreEqual("abc", KhachHangBLL.LoaiBoDauTiengViet("abc"));
        }

        [TestMethod]
        public void KhachHang_CapNhat_MaKhachHangKhongHopLe_TraVeThatBai()
        {
            // Arrange
            KhachHangDTO kh = new KhachHangDTO
            {
                MaKH = 0,
                TenKH = "Trần C",
                SDT = "0987654321"
            };

            // Act
            KhachHangResult res = KhachHangBLL.Sua(kh);

            // Assert
            Assert.IsFalse(res.Success);
            Assert.AreEqual("Mã khách hàng không hợp lệ.", res.Message);
        }
    }
}
