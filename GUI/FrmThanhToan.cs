using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
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
        private DateTime _deadlineUtc;
        private int _remainingSeconds = 600;
        private bool _isPaidSuccess = false;
        private bool _isProcessing = false;

        // Quản lý liên kết khách hàng & chống đè dữ liệu người dùng đã sửa tay (P1)
        private bool _autoFilling = false;
        private bool _nameUserEdited = false;
        private bool _emailUserEdited = false;
        private string _linkedPhone = null;
        private string _dbCustomerName = null;

        public FrmThanhToan(ThongTinDatVeDTO booking = null)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this._booking = booking ?? CreateDefaultBooking();

            _errorProvider = new ErrorProvider { BlinkStyle = ErrorBlinkStyle.NeverBlink };

            this.Load += FrmThanhToan_Load;
            this.FormClosing += FrmThanhToan_FormClosing;
            this.SizeChanged += FrmThanhToan_SizeChanged;
            pnlContainer.SizeChanged += (s, e) => ResponsiveLayout();

            txtTenKH.TextChanged += txtTenKH_TextChanged;
            txtEmail.TextChanged += txtEmail_TextChanged;
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

            // Điều hướng bàn phím cho các phương thức thanh toán (P3)
            SetupPaymentRadioKeyNav(radChuyenKhoan);
            SetupPaymentRadioKeyNav(radTienMat);
            SetupPaymentRadioKeyNav(radTheNganHang);

            this.AcceptButton = btnThanhToan;
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape && !_isProcessing)
                {
                    lblChonLaiVe_Click(lblChonLaiVe, EventArgs.Empty);
                }
            };

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
                SoGiayConLai = 600,
                TongTienDaXacNhan = 650000m
            };
        }

        private void SetupPaymentRadioKeyNav(Guna2CustomRadioButton rad)
        {
            rad.TabStop = true;
            rad.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
                {
                    SelectPaymentMethod(rad);
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Right)
                {
                    if (rad == radChuyenKhoan) SelectPaymentMethod(radTienMat);
                    else if (rad == radTienMat) SelectPaymentMethod(radTheNganHang);
                    else SelectPaymentMethod(radChuyenKhoan);
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Left)
                {
                    if (rad == radTheNganHang) SelectPaymentMethod(radTienMat);
                    else if (rad == radTienMat) SelectPaymentMethod(radChuyenKhoan);
                    else SelectPaymentMethod(radTheNganHang);
                    e.Handled = true;
                }
            };
        }

        private void SelectPaymentMethod(Guna2CustomRadioButton selected)
        {
            radChuyenKhoan.Checked = (selected == radChuyenKhoan);
            radTienMat.Checked = (selected == radTienMat);
            radTheNganHang.Checked = (selected == radTheNganHang);
            selected.Focus();
        }

        private void InitCountdownTimer()
        {
            int initSec = _booking.SoGiayConLai > 0 ? _booking.SoGiayConLai : 600;
            _deadlineUtc = DateTime.UtcNow.AddSeconds(initSec);

            if (_countdownTimer == null)
            {
                _countdownTimer = new Timer { Interval = 1000 };
                _countdownTimer.Tick += CountdownTimer_Tick;
            }
            UpdateCountdownDisplay();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if (_isProcessing) return; // Nếu đang trong giao dịch thanh toán thì chờ hoàn tất

            TimeSpan diff = _deadlineUtc - DateTime.UtcNow;
            _remainingSeconds = (int)Math.Max(0, Math.Ceiling(diff.TotalSeconds));
            UpdateCountdownDisplay();

            if (_remainingSeconds <= 0)
            {
                _countdownTimer.Stop();

                // Hết 10 phút giữ ghế
                if (_booking.DanhSachMaGhe != null && _booking.DanhSachMaGhe.Count > 0)
                {
                    GheBLL.GiaiPhongDanhSachGhe(_booking.DanhSachMaGhe, _booking.MaPhienGiu);
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

        private async void FrmThanhToan_Load(object sender, EventArgs e)
        {
            // Điều chỉnh kích thước cửa sổ vừa vặn theo WorkingArea của màn hình
            try
            {
                Rectangle wa = Screen.FromControl(this).WorkingArea;
                int targetW = Math.Min(1200, (int)(wa.Width * 0.95));
                int targetH = Math.Min(800, (int)(wa.Height * 0.95));
                this.Size = new Size(targetW, targetH);
                this.CenterToScreen();
            }
            catch { }

            LoadIcons();
            LoadBannerAndSummary();
            ResponsiveLayout();
            InitCountdownTimer();
            _countdownTimer.Start();

            // Tải danh sách khách hàng bất đồng bộ
            await LoadDanhSachKhachHangAsync();
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

            int padding = 20;
            int gap = 16;
            int totalW = pnlContainer.ClientSize.Width - (padding * 2);
            int totalH = pnlContainer.ClientSize.Height - (padding * 2);

            // Bố cục 1 cột khi bề ngang hẹp (< 860px)
            if (totalW < 860)
            {
                pnlContainer.AutoScroll = true;
                int colW = Math.Max(320, totalW - 10);
                pnlLeftColumn.SetBounds(padding, padding, colW, 610);
                pnlRightCard.SetBounds(padding, padding + 610 + gap, colW, 580);
                return;
            }

            pnlContainer.AutoScroll = true;
            int rightW = 440;
            if (totalW < 1050) rightW = 400;
            if (totalW < 950) rightW = 360;
            int leftW = totalW - rightW - gap;

            pnlLeftColumn.SetBounds(padding, padding, leftW, totalH);
            pnlRightCard.SetBounds(padding + leftW + gap, padding, rightW, totalH);
        }

        private void FrmThanhToan_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isProcessing && !_isPaidSuccess)
            {
                e.Cancel = true;
                // Keep feedback on the form; repeated close attempts must not open modal warnings.
                lblCountdownTitle.Text = "Vui lòng chờ xác nhận";
                return;
            }

            _countdownTimer?.Stop();
            _countdownTimer?.Dispose();
            _errorProvider?.Dispose();

            // Nếu đóng form mà chưa thanh toán thành công, giải phóng ghế đang giữ
            if (!_isPaidSuccess && _booking.DanhSachMaGhe != null && _booking.DanhSachMaGhe.Count > 0)
            {
                GheBLL.GiaiPhongDanhSachGhe(_booking.DanhSachMaGhe, _booking.MaPhienGiu);
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

        #region 2. Tải danh sách khách hàng & Tự động khớp khi nhập SĐT (Xử lý P1)
        private async Task LoadDanhSachKhachHangAsync()
        {
            try
            {
                _dtKhachHang = await Task.Run(() => KhachHangBLL.LayDanhSach());
                if (!string.IsNullOrEmpty(txtSDT.Text))
                {
                    txtSDT_TextChanged(txtSDT, EventArgs.Empty);
                }
            }
            catch
            {
                _dtKhachHang = null;
            }
        }

        private void txtTenKH_TextChanged(object sender, EventArgs e)
        {
            if (!_autoFilling) _nameUserEdited = true;
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            if (!_autoFilling) _emailUserEdited = true;
        }

        private void txtSDT_TextChanged(object sender, EventArgs e)
        {
            string sdt = txtSDT.Text.Trim();

            // Nếu người dùng thay đổi SĐT so với SĐT đã liên kết trước đó
            if (sdt != _linkedPhone)
            {
                _booking.MaKH = null;
                _linkedPhone = null;
                _dbCustomerName = null;

                _autoFilling = true;
                if (!_nameUserEdited) txtTenKH.Clear();
                if (!_emailUserEdited) txtEmail.Clear();
                _autoFilling = false;
            }

            if (sdt.Length != 10 || !sdt.StartsWith("0") || _dtKhachHang == null) return;

            foreach (DataRow r in _dtKhachHang.Rows)
            {
                string dbPhone = r["SDT"]?.ToString()?.Trim() ?? "";
                if (dbPhone.Equals(sdt, StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(r["MaKH"]?.ToString(), out int maKH))
                    {
                        _booking.MaKH = maKH;
                        _linkedPhone = sdt;
                        _dbCustomerName = r["TenKH"]?.ToString() ?? "";
                    }

                    _autoFilling = true;
                    if (!_nameUserEdited)
                        txtTenKH.Text = _dbCustomerName;
                    if (!_emailUserEdited)
                        txtEmail.Text = r["Email"]?.ToString() ?? "";
                    _autoFilling = false;
                    return;
                }
            }
        }
        #endregion

        #region 3. Chọn lại vé (Hủy phiên giữ chỗ để chọn lại)
        private void lblChonLaiVe_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lblChonLaiVe_Click(sender, EventArgs.Empty);
        }

        private void lblChonLaiVe_Click(object sender, EventArgs e)
        {
            if (_isProcessing) return;

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
                    GheBLL.GiaiPhongDanhSachGhe(_booking.DanhSachMaGhe, _booking.MaPhienGiu);
                    _booking.DanhSachMaGhe.Clear();
                }

                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
        #endregion

        #region 4. Xác nhận thanh toán (SqlTransaction an toàn & Bất đồng bộ)
        private void SetProcessingState(bool processing)
        {
            _isProcessing = processing;
            btnThanhToan.Enabled = !processing;
            lblChonLaiVe.Enabled = !processing;
            txtTenKH.Enabled = !processing;
            txtSDT.Enabled = !processing;
            txtEmail.Enabled = !processing;
            chkAgreePolicy.Enabled = !processing;
            radChuyenKhoan.Enabled = !processing;
            radTienMat.Enabled = !processing;
            radTheNganHang.Enabled = !processing;

            if (processing)
            {
                btnThanhToan.Text = "Đang xác nhận thanh toán...";
                lblCountdownTitle.Text = "Vui lòng chờ xác nhận";
                Cursor.Current = Cursors.WaitCursor;
            }
            else
            {
                btnThanhToan.Text = "Thanh toán";
                lblCountdownTitle.Text = "Hoàn tất đặt vé trong";
                Cursor.Current = Cursors.Default;
            }
        }

        private async void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (_isProcessing || _isPaidSuccess) return;
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

            _booking.ChoPhepCapNhatTen = false;
            if (_booking.MaKH.HasValue && !string.IsNullOrWhiteSpace(_dbCustomerName) &&
                !string.Equals(_dbCustomerName.Trim(), tenKH.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                DialogResult drTen = MessageBox.Show(
                    $"Số điện thoại này thuộc về khách hàng '{_dbCustomerName}'.\nBạn có muốn cập nhật họ tên thành '{tenKH}' không?",
                    "Xác nhận cập nhật thông tin khách",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (drTen == DialogResult.Yes)
                {
                    _booking.ChoPhepCapNhatTen = true;
                }
            }

            // Xác định phương thức thanh toán
            if (radChuyenKhoan.Checked)
                _booking.PhuongThucThanhToan = PhuongThucThanhToan.ChuyenKhoan;
            else if (radTheNganHang.Checked)
                _booking.PhuongThucThanhToan = PhuongThucThanhToan.TheNganHang;
            else
                _booking.PhuongThucThanhToan = PhuongThucThanhToan.TienMat;

            // Lock input before the first await so a second click cannot start another checkout.
            SetProcessingState(true);
            try
            {
                int remainDbSeconds = await Task.Run(() => GheBLL.LayGiayConLaiGiu(_booking.DanhSachMaGhe, _booking.MaPhienGiu));
                if (remainDbSeconds <= 0)
                {
                    _isProcessing = false;
                    _countdownTimer?.Stop();
                    MessageBox.Show("Thời gian giữ vé đã hết hạn. Vui lòng chọn lại vé.",
                        "Hết hạn giữ vé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }

                int maTK = PhienDangNhap.DaDangNhap ? PhienDangNhap.TaiKhoan.MaTK : 1;
                KetQuaThanhToan kq = await Task.Run(() => BanVeBLL.XuLyThanhToan(_booking, maTK));

                if (kq.Success)
                {
                    _isPaidSuccess = true;
                    _isProcessing = false;
                    _countdownTimer?.Stop();
                    btnThanhToan.Text = "Đã thanh toán";
                    lblCountdownTitle.Text = "Thanh toán thành công";
                    Cursor.Current = Cursors.Default;

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
                                      $"• Tổng thanh toán: {kq.TongTien:N0} đ";

                    MessageBox.Show(thongBao, "Thanh toán thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else if (kq.CanXacNhanGia)
                {
                    DialogResult drGia = MessageBox.Show(
                        $"{kq.Message}\n\nBạn có muốn áp dụng mức giá mới ({kq.TongTienMoi:N0} đ) và tiếp tục thanh toán không?",
                        "Giá vé đã thay đổi",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (drGia == DialogResult.Yes)
                    {
                        _booking.TongTienDaXacNhan = kq.TongTienMoi;
                        if (_booking.SoLuong > 0)
                            _booking.GiaVe = kq.TongTienMoi / _booking.SoLuong;

                        LoadBannerAndSummary();
                    }
                    return;
                }
                else
                {
                    MessageBox.Show(kq.Message, "Lỗi xử lý giao dịch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra sự cố trong quá trình giao dịch: " + ex.Message, "Lỗi ngoại lệ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isProcessing = false;
                if (!IsDisposed && !Disposing && !_isPaidSuccess)
                    SetProcessingState(false);
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion
    }
}
