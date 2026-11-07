using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;
using System;

namespace QuanLySuKienVaBanVe.UnitTests
{
    [TestClass]
    public class SuKienSuatDienTests
    {
        [TestInitialize]
        public void Setup()
        {
            PhienDangNhap.BatDau(new TaiKhoanDTO
            {
                MaTK = 1,
                TenDangNhap = "admin_test",
                VaiTro = VaiTro.QuanLy
            });
        }

        [TestCleanup]
        public void Cleanup()
        {
            PhienDangNhap.KetThuc();
        }

        [TestMethod]
        public void SuKien_ThemMoi_DoiTuongNull_TraVeLoi()
        {
            // Act
            string kq = SuKienBLL.Them(null);

            // Assert
            Assert.AreEqual("Dữ liệu sự kiện không hợp lệ!", kq);
        }

        [TestMethod]
        public void SuKien_ThemMoi_TenTrong_TraVeLoi()
        {
            // Arrange
            SuKienDTO sk = new SuKienDTO
            {
                TenSK = "   ",
                LoaiSK = "Ca nhạc"
            };

            // Act
            string kq = SuKienBLL.Them(sk);

            // Assert
            Assert.AreEqual("Tên sự kiện không được để trống!", kq);
        }

        [TestMethod]
        public void SuatDien_ThemMoi_DoiTuongNull_TraVeLoi()
        {
            // Act
            string kq = SuatDienBLL.Them(null);

            // Assert
            Assert.AreEqual("Dữ liệu suất diễn không hợp lệ!", kq);
        }

        [TestMethod]
        public void SuatDien_ThemMoi_ThieuSuKien_TraVeLoi()
        {
            // Arrange
            SuatDienDTO sd = new SuatDienDTO
            {
                MaSK = 0,
                DiaDiem = "Hội trường A"
            };

            // Act
            string kq = SuatDienBLL.Them(sd);

            // Assert
            Assert.AreEqual("Vui lòng chọn sự kiện!", kq);
        }

        [TestMethod]
        public void SuatDien_ThemMoi_ThieuDiaDiem_TraVeLoi()
        {
            // Arrange
            SuatDienDTO sd = new SuatDienDTO
            {
                MaSK = 1,
                DiaDiem = "  "
            };

            // Act
            string kq = SuatDienBLL.Them(sd);

            // Assert
            Assert.AreEqual("Địa điểm không được để trống!", kq);
        }

        [TestMethod]
        public void SuatDien_ThemMoi_ThoiGianKetThucNhoHonBatDau_TraVeLoi()
        {
            // Arrange
            SuatDienDTO sd = new SuatDienDTO
            {
                MaSK = 1,
                DiaDiem = "Hội trường B",
                ThoiGianBatDau = new DateTime(2026, 12, 1, 19, 0, 0),
                ThoiGianKetThuc = new DateTime(2026, 12, 1, 18, 0, 0)
            };

            // Act
            string kq = SuatDienBLL.Them(sd);

            // Assert
            Assert.AreEqual("Thời gian kết thúc phải lớn hơn thời gian bắt đầu!", kq);
        }

        [TestMethod]
        public void KhuVuc_ThemMoi_GiaVeAm_TraVeLoi()
        {
            // Arrange
            KhuVucDTO kv = new KhuVucDTO
            {
                MaSuat = 1,
                TenKV = "Khu Vực VIP",
                GiaVe = -100000m,
                TongSoGhe = 50
            };

            // Act
            string kq = KhuVucBLL.Them(kv);

            // Assert
            Assert.AreEqual("Giá vé phải lớn hơn hoặc bằng 0!", kq);
        }

        [TestMethod]
        public void KhuVuc_ThemMoi_SucChuaBangKhong_TraVeLoi()
        {
            // Arrange
            KhuVucDTO kv = new KhuVucDTO
            {
                MaSuat = 1,
                TenKV = "Khu Vực Phổ Thông",
                GiaVe = 200000m,
                TongSoGhe = 0
            };

            // Act
            string kq = KhuVucBLL.Them(kv);

            // Assert
            Assert.AreEqual("Sức chứa phải là số nguyên dương từ 1 đến 2.600 ghế!", kq);
        }

        [TestMethod]
        public void KhuVuc_ThemMoi_SucChuaVuotNguongToiDa_TraVeLoi()
        {
            // Arrange
            KhuVucDTO kv = new KhuVucDTO
            {
                MaSuat = 1,
                TenKV = "Khu Vực Khổng Lồ",
                GiaVe = 150000m,
                TongSoGhe = 3000
            };

            // Act
            string kq = KhuVucBLL.Them(kv);

            // Assert
            Assert.AreEqual("Sức chứa phải là số nguyên dương từ 1 đến 2.600 ghế!", kq);
        }
    }
}
