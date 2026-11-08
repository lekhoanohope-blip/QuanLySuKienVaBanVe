using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;
using System;

namespace QuanLySuKienVaBanVe.UnitTests
{
    [TestClass]
    public class DangNhapPhanQuyenTests
    {
        [TestInitialize]
        public void Setup()
        {
            PhienDangNhap.KetThuc();
        }

        [TestCleanup]
        public void Cleanup()
        {
            PhienDangNhap.KetThuc();
        }

        [TestMethod]
        public void DangNhap_ThieuTenDangNhap_TraVeLoiThieuThongTin()
        {
            // Act
            KetQuaDangNhap kq = TaiKhoanBLL.DangNhap("", "MatKhau123", out TaiKhoanDTO tk);

            // Assert
            Assert.AreEqual(KetQuaDangNhap.ThieuThongTin, kq);
            Assert.IsNull(tk);
        }

        [TestMethod]
        public void DangNhap_ThieuMatKhau_TraVeLoiThieuThongTin()
        {
            // Act
            KetQuaDangNhap kq = TaiKhoanBLL.DangNhap("admin", "", out TaiKhoanDTO tk);

            // Assert
            Assert.AreEqual(KetQuaDangNhap.ThieuThongTin, kq);
            Assert.IsNull(tk);
        }

        [TestMethod]
        public void DangNhap_TenDangNhapKhoangTrang_TraVeLoiThieuThongTin()
        {
            // Act
            KetQuaDangNhap kq = TaiKhoanBLL.DangNhap("   ", "MatKhau123", out TaiKhoanDTO tk);

            // Assert
            Assert.AreEqual(KetQuaDangNhap.ThieuThongTin, kq);
            Assert.IsNull(tk);
        }

        [TestMethod]
        public void MatKhauHelper_BamSha256_DungChuanKhongDoi()
        {
            // Arrange
            string matKhau = "Admin@2026!";

            // Act
            string hash1 = TaiKhoanBLL.BamMatKhau(matKhau);
            string hash2 = TaiKhoanBLL.BamMatKhau(matKhau);

            // Assert
            Assert.IsNotNull(hash1);
            Assert.AreEqual(64, hash1.Length, "SHA-256 chuỗi hex phải có độ dài đúng 64 ký tự");
            Assert.AreEqual(hash1, hash2, "Băm cùng một mật khẩu phải cho ra kết quả băm trùng khớp tuyệt đối");
        }

        [TestMethod]
        public void MatKhauHelper_MatKhauKhacNhau_SinhHashKhacNhau()
        {
            // Act
            string hashA = TaiKhoanBLL.BamMatKhau("MatKhauA");
            string hashB = TaiKhoanBLL.BamMatKhau("MatKhauB");

            // Assert
            Assert.AreNotEqual(hashA, hashB, "Hai mật khẩu khác nhau không được phép trùng giá trị băm");
        }

        [TestMethod]
        public void PhienDangNhap_QuanLy_CoDayDuQuyenQuanTri()
        {
            // Arrange
            TaiKhoanDTO tkQuanLy = new TaiKhoanDTO
            {
                MaTK = 1,
                TenDangNhap = "admin",
                VaiTro = VaiTro.QuanLy,
                TrangThai = true
            };

            // Act
            PhienDangNhap.BatDau(tkQuanLy);

            // Assert
            Assert.IsTrue(PhienDangNhap.DaDangNhap);
            Assert.IsTrue(PhienDangNhap.LaQuanLy);
            Assert.AreEqual("admin", PhienDangNhap.TaiKhoan.TenDangNhap);
            Assert.AreEqual(VaiTro.QuanLy, PhienDangNhap.TaiKhoan.VaiTro);
        }

        [TestMethod]
        public void PhienDangNhap_NhanVien_BiGioiHanChucNangQuanTri()
        {
            // Arrange
            TaiKhoanDTO tkNhanVien = new TaiKhoanDTO
            {
                MaTK = 2,
                TenDangNhap = "nhanvien1",
                VaiTro = VaiTro.NhanVien,
                TrangThai = true
            };

            // Act
            PhienDangNhap.BatDau(tkNhanVien);

            // Assert
            Assert.IsTrue(PhienDangNhap.DaDangNhap);
            Assert.IsFalse(PhienDangNhap.LaQuanLy);
            Assert.AreEqual(VaiTro.NhanVien, PhienDangNhap.TaiKhoan.VaiTro);
        }

        [TestMethod]
        public void PhienDangNhap_DangXuat_XoaSachThongTinPhien()
        {
            // Arrange
            TaiKhoanDTO tk = new TaiKhoanDTO { MaTK = 99, TenDangNhap = "test", VaiTro = VaiTro.QuanLy };
            PhienDangNhap.BatDau(tk);
            Assert.IsTrue(PhienDangNhap.DaDangNhap);

            // Act
            PhienDangNhap.KetThuc();

            // Assert
            Assert.IsFalse(PhienDangNhap.DaDangNhap);
            Assert.IsNull(PhienDangNhap.TaiKhoan);
            Assert.IsFalse(PhienDangNhap.LaQuanLy);
        }
    }
}
