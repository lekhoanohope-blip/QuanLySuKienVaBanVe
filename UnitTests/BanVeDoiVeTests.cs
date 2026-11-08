using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;
using System.Collections.Generic;

namespace QuanLySuKienVaBanVe.UnitTests
{
    [TestClass]
    public class BanVeDoiVeTests
    {
        [TestMethod]
        public void DatVe_ThanhToan_DoiTuongNull_BaoLoi()
        {
            // Act
            KetQuaThanhToan kq = BanVeBLL.XuLyThanhToan(null);

            // Assert
            Assert.IsFalse(kq.Success);
            Assert.AreEqual("Danh sách ghế đặt đang trống. Vui lòng chọn ghế trước khi thanh toán.", kq.Message);
        }

        [TestMethod]
        public void DatVe_ThanhToan_KhongCoGheNao_BaoLoi()
        {
            // Arrange
            ThongTinDatVeDTO booking = new ThongTinDatVeDTO
            {
                MaSuat = 1,
                DanhSachMaGhe = new List<int>()
            };

            // Act
            KetQuaThanhToan kq = BanVeBLL.XuLyThanhToan(booking);

            // Assert
            Assert.IsFalse(kq.Success);
            Assert.AreEqual("Danh sách ghế đặt đang trống. Vui lòng chọn ghế trước khi thanh toán.", kq.Message);
        }

        [TestMethod]
        public void DonVeResult_KhoiTaoOk_DayDuThongTin()
        {
            // Act
            DonVeResult res = DonVeResult.Ok("Đổi vé thành công", 100, 101, "VE-HCMUE-99", 150000m);

            // Assert
            Assert.IsTrue(res.Success);
            Assert.AreEqual("Đổi vé thành công", res.Message);
            Assert.AreEqual(100, res.MaDon);
            Assert.AreEqual(101, res.MaDonMoi);
            Assert.AreEqual("VE-HCMUE-99", res.MaVeCodeMoi);
            Assert.AreEqual(150000m, res.ChenhLech);
        }

        [TestMethod]
        public void DonVeResult_KhoiTaoFail_ThongDiepLoiChinhXac()
        {
            // Act
            DonVeResult res = DonVeResult.Fail("Không tìm thấy đơn vé gốc hoặc đơn đã bị hủy.");

            // Assert
            Assert.IsFalse(res.Success);
            Assert.AreEqual("Không tìm thấy đơn vé gốc hoặc đơn đã bị hủy.", res.Message);
        }

        [TestMethod]
        public void DoiVe_NgangGia_ChenhLechBangKhong()
        {
            // Act
            decimal delta = DonVeBLL.TinhChenhLech(500000m, 500000m);

            // Assert
            Assert.AreEqual(0m, delta, "Đổi vé ngang giá chênh lệch phải đúng bằng 0");
        }

        [TestMethod]
        public void DoiVe_LenHangCaoHon_ChenhLechDuong_YeuCauThuThem()
        {
            // Act: Vé cũ 250.000đ đổi sang vé VIP 500.000đ -> Khách trả thêm 250.000đ
            decimal delta = DonVeBLL.TinhChenhLech(250000m, 500000m);

            // Assert
            Assert.AreEqual(250000m, delta);
            Assert.IsTrue(delta > 0m, "Đổi lên hạng cao hơn chênh lệch phải mang giá trị dương");
        }

        [TestMethod]
        public void DoiVe_XuongHangThapHon_ChenhLechAm_YeuCauHoanTien()
        {
            // Act: Vé cũ 500.000đ đổi sang vé Thường 350.000đ -> Hoàn tiền 150.000đ
            decimal delta = DonVeBLL.TinhChenhLech(500000m, 350000m);

            // Assert
            Assert.AreEqual(-150000m, delta);
            Assert.IsTrue(delta < 0m, "Đổi xuống hạng thấp hơn chênh lệch phải mang giá trị âm");
        }
    }
}
