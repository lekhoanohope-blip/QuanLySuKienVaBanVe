using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;
using QuanLySuKienVaBanVe.GUI.Helpers;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmThanhToan : Form
    {
        private readonly ThongTinDatVeDTO _booking;
        private DataTable _dtKhachHang;
        private ErrorProvider _errorProvider;
        private Timer _countdownTimer;
        private int _remainingSeconds = 600;
        private bool _isPaidSuccess = false;

        public FrmThanhToan(ThongTinDatVeDTO booking = null)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this._booking = booking ?? CreateDefaultBooking();

            _errorProvider = new ErrorProvider { BlinkStyle = ErrorBlinkStyle.NeverBlink };

            this.Load += FrmThanhToan_Load;
            this.FormClosing += FrmThanhToan_FormClosing;
            this.SizeChanged += FrmThanhToan_SizeChanged;
            pnlContainer.SizeChanged += (s, e) => ResponsiveLayout();

            txtSDT.Leave += txtSDT_Leave;
            txtSDT.TextChanged += txtSDT_TextChanged;

            radChuyenKhoan.Click += (s, e) => SelectPaymentMethod(radChuyenKhoan);
            picLogoChuyenKhoan.Click += (s, e) => SelectPaymentMethod(radChuyenKhoan);
            lblTextChuyenKhoan.Click += (s, e) => SelectPaymentMethod(radChuyenKhoan);

            radTienMat.Click += (s, e) => SelectPaymentMethod(radTienMat);
            picLogoTienMat.Click += (s, e) => SelectPaymentMethod(radTienMat);
            lblTextTienMat.Click += (s, e) => SelectPaymentMethod(radTienMat);

            radTheNganHang.Click += (s, e) => SelectPaymentMethod(radTheNganHang);
            picLogoTheNganHang.Click += (s, e) => SelectPaymentMethod(radTheNganHang);
            lblTextTheNganHang.Click += (s, e) => SelectPaymentMethod(radTheNganHang);

            pnlDashedLine.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(203, 213, 225), 1.5f))
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    int y = pnlDashedLine.Height / 2;
                    e.Graphics.DrawLine(pen, 0, y, pnlDashedLine.Width, y);
                }
            };

            SelectPaymentMethod(radChuyenKhoan);
            InitCountdownTimer();
        }

        private ThongTinDatVeDTO CreateDefaultBooking()
        {
            return new ThongTinDatVeDTO
            {
                MaSK = 1,
                TenSK = "Live Concert: Cảm Ơn Vì Đã Từng - Vũ Trọng",
                MaSuat = 1,
                ThoiGianBatDau = DateTime.Now.AddDays(4),
                DiaDiem = "Hội trường A - HCMUE (280 An Dương Vương, Q.5, TP.HCM)",
                TenKV = "VIP A",
                HangVe = "VIP",
                GiaVe = 650000m,
                SoLuong = 1,
                DanhSachMaGhe = new List<int>(),
                SoGiayConLai = 600
            };
        }

        private void SelectPaymentMethod(Guna.UI2.WinForms.Guna2CustomRadioButton selected)
        {
            radChuyenKhoan.Checked = (selected == radChuyenKhoan);
            radTienMat.Checked = (selected == radTienMat);
            radTheNganHang.Checked = (selected == radTheNganHang);
        }

        private void InitCountdownTimer()
        {
            _remainingSeconds = _booking.SoGiayConLai > 0 ? _booking.SoGiayConLai : 600;

            _countdownTimer = new Timer { Interval = 1000 };
            _countdownTimer.Tick += CountdownTimer_Tick;
            UpdateCountdownDisplay();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if (_remainingSeconds > 0)
            {
                _remainingSeconds--;
                UpdateCountdownDisplay();
            }
            else
            {
                _countdownTimer.Stop();

                // Hết 10 phút giữ ghế
                if (_booking.DanhSachMaGhe != null && _booking.DanhSachMaGhe.Count > 0)
                {
                    GheBLL.GiaiPhongDanhSachGhe(_booking.DanhSachMaGhe);
                    _booking.DanhSachMaGhe.Clear();
                }

                MessageBox.Show("Thời gian giữ vé (10 phút) đã kết thúc!\n\nHệ thống đã tự động giải phóng ghế đang giữ về trạng thái trống.",
                    "Hết giờ giữ chỗ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void UpdateCountdownDisplay()
        {
            int mm = _remainingSeconds / 60;
            int ss = _remainingSeconds % 60;
            lblMinBox.Text = mm.ToString("D2");
            lblSecBox.Text = ss.ToString("D2");
        }

        private void FrmThanhToan_Load(object sender, EventArgs e)
        {
            LoadIcons();
            LoadBannerAndSummary();
            LoadDanhSachKhachHang();
            ResponsiveLayout();

            _countdownTimer.Start();
        }

        private void LoadIcons()
        {
            try
            {
                if (pbBannerTimeIcon != null) pbBannerTimeIcon.Image = IconHelper.GetIcon("icon_calendar.png");
                if (pbBannerLocIcon != null) pbBannerLocIcon.Image = IconHelper.GetIcon("icon_location.png");
                if (picLogoChuyenKhoan != null) picLogoChuyenKhoan.Image = IconHelper.GetIcon("payment_vietqr.png");
                if (picLogoTienMat != null) picLogoTienMat.Image = IconHelper.GetIcon("payment_cash.png");
                if (picLogoTheNganHang != null) picLogoTheNganHang.Image = IconHelper.GetIcon("payment_card.png");
            }
            catch { }
        }

        private void FrmThanhToan_SizeChanged(object sender, EventArgs e)
        {
            ResponsiveLayout();
        }

        private void ResponsiveLayout()
        {
            if (pnlContainer.ClientSize.Width <= 0) return;

            int padding = 24;
            int gap = 20;
            int totalW = pnlContainer.ClientSize.Width - (padding * 2);
            int totalH = pnlContainer.ClientSize.Height - (padding * 2);

            int rightW = 440;
            if (totalW < 1050) rightW = 410;
            if (totalW < 900) rightW = 370;
            int leftW = totalW - rightW - gap;

            if (leftW > 280)
            {
                pnlLeftColumn.SetBounds(padding, padding, leftW, totalH);
                pnlRightCard.SetBounds(padding + leftW + gap, padding, rightW, totalH);
            }
        }

        private void FrmThanhToan_FormClosing(object sender, FormClosingEventArgs e)
        {
            _countdownTimer?.Stop();

            // Nếu đóng form mà chưa thanh toán thành công, giải phóng ghế đang giữ
            if (!_isPaidSuccess && _booking.DanhSachMaGhe != null && _booking.DanhSachMaGhe.Count > 0)
            {
                GheBLL.GiaiPhongDanhSachGhe(_booking.DanhSachMaGhe);
                _booking.DanhSachMaGhe.Clear();
            }
        }

        #region 1. Hiển thị thông tin Banner & Tóm tắt đơn hàng (Ticketbox)
        private void LoadBannerAndSummary()
        {
            if (_booking == null) return;

            // Top Banner
            lblBannerTenSK.Text = _booking.TenSK;
            lblBannerTime.Text = $"{_booking.ThoiGianBatDau:HH:mm} - {_booking.ThoiGianBatDau.AddHours(2.5):HH:mm}, {_booking.ThoiGianBatDau:dd} Tháng {_booking.ThoiGianBatDau:MM, yyyy}";
            lblBannerLocation.Text = string.IsNullOrWhiteSpace(_booking.DiaDiem) ? "Địa điểm sự kiện" : _booking.DiaDiem;

            // Right Card
            lblItemNameVal.Text = _booking.TenKV;
            lblItemPriceVal.Text = $"{_booking.GiaVe:N0} đ";
            lblItemQtyVal.Text = $"{_booking.SoLuong:D2}";
            lblItemTotalVal.Text = $"{_booking.TongTien:N0} đ";

            lblSubTotalTitle.Text = $"Tạm tính {_booking.SoLuong} vé";
            lblSubTotalVal.Text = $"{_booking.TongTien:N0} đ";
            lblTongTienVal.Text = $"{_booking.TongTien:N0} đ";
        }
        #endregion

        #region 2. Tải danh sách khách hàng & Tự động khớp khi nhập SĐT
        private void LoadDanhSachKhachHang()
        {
            try
            {
                _dtKhachHang = KhachHangBLL.LayDanhSach();
            }
            catch
            {
                _dtKhachHang = null;
            }
        }

        private void txtSDT_TextChanged(object sender, EventArgs e)
        {
            AutoFillCustomerByPhone();
        }

        private void txtSDT_Leave(object sender, EventArgs e)
        {
            AutoFillCustomerByPhone();
        }

        private void AutoFillCustomerByPhone()
        {
            string sdt = txtSDT.Text.Trim();
            if (string.IsNullOrEmpty(sdt) || sdt.Length < 9 || _dtKhachHang == null) return;

            foreach (DataRow r in _dtKhachHang.Rows)
            {
                string dbPhone = r["SDT"]?.ToString()?.Trim() ?? "";
                if (dbPhone.Equals(sdt, StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrWhiteSpace(txtTenKH.Text))
                        txtTenKH.Text = r["TenKH"]?.ToString() ?? "";
                    if (string.IsNullOrWhiteSpace(txtEmail.Text))
                        txtEmail.Text = r["Email"]?.ToString() ?? "";

                    if (int.TryParse(r["MaKH"]?.ToString(), out int maKH))
                    {
                        _booking.MaKH = maKH;
                    }
                    return;
                }
            }
        }
        #endregion

        #region 3. Chọn lại vé (Hủy phiên giữ chỗ để chọn lại)
        private void lblChonLaiVe_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                "Bạn có chắc muốn chọn lại vé không?\n\nSố lượng vé đang giữ sẽ được giải phóng để bạn có thể chọn lại khu vực khác trên sơ đồ.",
                "Chọn lại vé",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                _countdownTimer?.Stop();

                if (_booking.DanhSachMaGhe != null && _booking.DanhSachMaGhe.Count > 0)
                {
                    GheBLL.GiaiPhongDanhSachGhe(_booking.DanhSachMaGhe);
                    _booking.DanhSachMaGhe.Clear();
                }

                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
        #endregion

        #region 4. Xác nhận thanh toán (SqlTransaction)
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            _errorProvider.Clear();

            string tenKH = txtTenKH.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(tenKH))
            {
                _errorProvider.SetError(txtTenKH, "Họ và tên khách hàng không được để trống.");
                txtTenKH.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(sdt))
            {
                _errorProvider.SetError(txtSDT, "Số điện thoại không được để trống.");
                txtSDT.Focus();
                return;
            }

            if (!Regex.IsMatch(sdt, @"^0[0-9]{9}$"))
            {
                _errorProvider.SetError(txtSDT, "Số điện thoại phải gồm đúng 10 chữ số bắt đầu bằng số 0.");
                txtSDT.Focus();
                return;
            }

            if (!chkAgreePolicy.Checked)
            {
                MessageBox.Show("Vui lòng tích chọn đồng ý điều khoản trước khi thanh toán!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Ghi nhận thông tin vào DTO
            _booking.TenKH = tenKH;
            _booking.SDT = sdt;
            _booking.Email = email;

            // Xác định phương thức thanh toán
            if (radChuyenKhoan.Checked)
                _booking.PhuongThucThanhToan = PhuongThucThanhToan.ChuyenKhoan;
            else if (radTheNganHang.Checked)
                _booking.PhuongThucThanhToan = PhuongThucThanhToan.TheNganHang;
            else
                _booking.PhuongThucThanhToan = PhuongThucThanhToan.TienMat;

            btnThanhToan.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                int maTK = PhienDangNhap.DaDangNhap ? PhienDangNhap.TaiKhoan.MaTK : 1;
                KetQuaThanhToan kq = BanVeBLL.XuLyThanhToan(_booking, maTK);

                if (kq.Success)
                {
                    _isPaidSuccess = true;
                    _countdownTimer?.Stop();

                    _booking.MaDonPhatHanh = kq.MaDon;
                    _booking.DanhSachMaVeCode = kq.DanhSachVePhatHanh.ConvertAll(v => v.MaVeCode);
                    _booking.DanhSachGhe = kq.DanhSachVePhatHanh.ConvertAll(v => v.MaGheHienThi);

                    string dsGhe = string.Join(", ", _booking.DanhSachGhe);
                    string thongBao = $"GIAO DỊCH BÁN VÉ THÀNH CÔNG!\n\n" +
                                      $"• Mã đơn hàng: #{kq.MaDon}\n" +
                                      $"• Khách hàng: {_booking.TenKH} ({_booking.SDT})\n" +
                                      $"• Khu vực: {_booking.TenKV}\n" +
                                      $"• Số lượng: {_booking.SoLuong} vé\n" +
                                      $"• Vị trí ghế được cấp: {dsGhe}\n" +
                                      $"• Tổng thanh toán: {kq.TongTien:N0} đ\n\n" +
                                      $"Bạn có muốn mở xem trước và in vé ngay bây giờ không?";

                    DialogResult drPrint = MessageBox.Show(thongBao, "Thanh toán thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (drPrint == DialogResult.Yes)
                    {
                        MessageBox.Show("Chức năng in vé đang được phát triển.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(kq.Message, "Lỗi xử lý giao dịch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnThanhToan.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra sự cố trong quá trình giao dịch: " + ex.Message, "Lỗi ngoại lệ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnThanhToan.Enabled = true;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion
    }
}
