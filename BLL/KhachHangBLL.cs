using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using QuanLySuKienVaBanVe.DAL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.BLL
{
    /// <summary>
    /// Đối tượng chứa kết quả trả về từ tầng nghiệp vụ BLL
    /// </summary>
    public class KhachHangResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string FieldName { get; set; }
        public int MaKH { get; set; }

        public static KhachHangResult Ok(string message = "", int maKH = 0)
        {
            return new KhachHangResult
            {
                Success = true,
                Message = message,
                MaKH = maKH,
                FieldName = string.Empty
            };
        }

        public static KhachHangResult Fail(string message, string fieldName = "", int maKH = 0)
        {
            return new KhachHangResult
            {
                Success = false,
                Message = message,
                FieldName = fieldName,
                MaKH = maKH
            };
        }
    }

    /// <summary>
    /// Lớp xử lý nghiệp vụ Khách hàng (BLL)
    /// Đảm bảo validation, ràng buộc UNIQUE, kiểm tra khóa ngoại trước khi gọi DAL.
    /// </summary>
    public class KhachHangBLL
    {
        private static readonly Regex PhoneRegex = new Regex(@"\A0[0-9]{9}\z", RegexOptions.Compiled);
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public static DataTable LayDanhSach()
        {
            return KhachHangDAL.LayDanhSach();
        }

        public static DataTable TimKiemTheoSDT(string tienTo)
        {
            if (string.IsNullOrWhiteSpace(tienTo))
            {
                return KhachHangDAL.LayDanhSach();
            }

            string cleanTienTo = tienTo.Trim();
            // Chỉ nhận tiền tố 1–10 chữ số ASCII, chặn ký tự chữ và wildcard %, _ trước khi xuống DAL
            if (!Regex.IsMatch(cleanTienTo, @"^[0-9]{1,10}$"))
            {
                // Trả về DataTable rỗng với cấu trúc chuẩn nếu input chứa chữ cái, wildcard %, _ hoặc quá 10 chữ số
                DataTable emptyTable = KhachHangDAL.LayDanhSach().Clone();
                return emptyTable;
            }

            return KhachHangDAL.TimKiemTheoSDT(cleanTienTo);
        }

        public static KhachHangDTO LayTheoMa(int maKH)
        {
            if (maKH <= 0) return null;
            return KhachHangDAL.LayTheoMa(maKH);
        }

        private static KhachHangResult ValidateKhachHang(KhachHangDTO kh, bool isEdit = false)
        {
            if (kh == null)
            {
                return KhachHangResult.Fail("Dữ liệu khách hàng không được để trống.");
            }

            if (isEdit && kh.MaKH <= 0)
            {
                return KhachHangResult.Fail("Mã khách hàng không hợp lệ.", "MaKH");
            }

            kh.TenKH = kh.TenKH?.Trim() ?? string.Empty;
            kh.SDT = kh.SDT?.Trim() ?? string.Empty;
            kh.Email = kh.Email?.Trim() ?? string.Empty;

            // 1. Validate Họ và tên
            if (string.IsNullOrWhiteSpace(kh.TenKH))
            {
                return KhachHangResult.Fail("Họ và tên khách hàng không được để trống.", "TenKH");
            }
            if (kh.TenKH.Length > 100)
            {
                return KhachHangResult.Fail("Họ và tên không được vượt quá 100 ký tự.", "TenKH");
            }

            // 2. Validate Số điện thoại (Quy ước: 10 chữ số ASCII bắt đầu bằng 0)
            if (string.IsNullOrWhiteSpace(kh.SDT))
            {
                return KhachHangResult.Fail("Số điện thoại không được để trống.", "SDT");
            }
            if (!PhoneRegex.IsMatch(kh.SDT))
            {
                return KhachHangResult.Fail("Số điện thoại không hợp lệ! Vui lòng nhập đúng 10 chữ số bắt đầu bằng số 0 (Ví dụ: 0901234567).", "SDT");
            }

            // 3. Validate Email (tùy chọn)
            if (!string.IsNullOrWhiteSpace(kh.Email))
            {
                if (kh.Email.Length > 100)
                {
                    return KhachHangResult.Fail("Địa chỉ email không được vượt quá 100 ký tự.", "Email");
                }
                if (!EmailRegex.IsMatch(kh.Email))
                {
                    return KhachHangResult.Fail("Địa chỉ email không đúng định dạng (Ví dụ: ten@domain.com).", "Email");
                }
            }

            // 4. Kiểm tra trùng SĐT tại tầng BLL trước khi ghi xuống DB
            int excludeId = isEdit ? kh.MaKH : 0;
            if (KhachHangDAL.KiemTraTrungSDT(kh.SDT, excludeId))
            {
                return KhachHangResult.Fail("Số điện thoại này đã được đăng ký bởi khách hàng khác trong hệ thống.", "SDT");
            }

            return KhachHangResult.Ok();
        }

        public static KhachHangResult Them(KhachHangDTO kh)
        {
            KhachHangResult validate = ValidateKhachHang(kh, isEdit: false);
            if (!validate.Success)
            {
                return validate;
            }

            try
            {
                int newId = KhachHangDAL.Them(kh);
                if (newId > 0)
                {
                    kh.MaKH = newId;
                    return KhachHangResult.Ok("Thêm mới khách hàng thành công!", newId);
                }
                return KhachHangResult.Fail("Thêm khách hàng thất bại do không nhận được mã định danh từ cơ sở dữ liệu.");
            }
            catch (SqlException ex)
            {
                // Bắt lỗi vi phạm ràng buộc UNIQUE phòng khi 2 người cùng submit đồng thời
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    return KhachHangResult.Fail("Số điện thoại này vừa được thêm bởi một phiên làm việc khác.", "SDT");
                }
                return KhachHangResult.Fail("Lỗi hệ thống khi lưu dữ liệu: " + ex.Message);
            }
            catch (Exception ex)
            {
                return KhachHangResult.Fail("Đã xảy ra lỗi không xác định: " + ex.Message);
            }
        }

        public static KhachHangResult Sua(KhachHangDTO kh)
        {
            KhachHangResult validate = ValidateKhachHang(kh, isEdit: true);
            if (!validate.Success)
            {
                return validate;
            }

            try
            {
                int rows = KhachHangDAL.Sua(kh);
                if (rows > 0)
                {
                    return KhachHangResult.Ok("Cập nhật thông tin khách hàng thành công!", kh.MaKH);
                }
                return KhachHangResult.Fail("Khách hàng không còn tồn tại trên hệ thống hoặc đã bị xóa bởi người dùng khác.");
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    return KhachHangResult.Fail("Số điện thoại này đã thuộc về khách hàng khác.", "SDT");
                }
                return KhachHangResult.Fail("Lỗi hệ thống khi cập nhật: " + ex.Message);
            }
            catch (Exception ex)
            {
                return KhachHangResult.Fail("Đã xảy ra lỗi không xác định: " + ex.Message);
            }
        }

        public static KhachHangResult Xoa(int maKH)
        {
            if (maKH <= 0)
            {
                return KhachHangResult.Fail("Mã khách hàng không hợp lệ.", "MaKH");
            }

            try
            {
                // 1. Kiểm tra ràng buộc trước khi xóa: Khách hàng đã phát sinh đơn vé thì không được xóa
                if (KhachHangDAL.KiemTraKhachHangCoDonVe(maKH))
                {
                    return KhachHangResult.Fail("Khách hàng này đã có lịch sử đặt vé trong hệ thống. Không được phép xóa để bảo toàn dữ liệu kiểm toán!", "MaKH", maKH);
                }

                int rows = KhachHangDAL.Xoa(maKH);
                if (rows > 0)
                {
                    return KhachHangResult.Ok("Xóa khách hàng thành công!");
                }
                return KhachHangResult.Fail("Khách hàng không còn tồn tại hoặc đã được xóa trước đó.");
            }
            catch (SqlException ex)
            {
                // Ràng buộc khóa ngoại phát sinh đồng thời
                if (ex.Number == 547)
                {
                    return KhachHangResult.Fail("Không thể xóa khách hàng do đang có dữ liệu liên quan trong hệ thống.", "MaKH");
                }
                return KhachHangResult.Fail("Lỗi hệ thống khi xóa khách hàng: " + ex.Message);
            }
            catch (Exception ex)
            {
                return KhachHangResult.Fail("Đã xảy ra lỗi không xác định: " + ex.Message);
            }
        }
    }
}
