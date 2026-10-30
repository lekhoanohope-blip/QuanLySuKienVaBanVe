using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmThanhToan : Form
    {
        private readonly ThongTinDatVeDTO _booking;
        private DataTable _dtKhachHang;

        public FrmThanhToan(ThongTinDatVeDTO booking = null)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this._booking = booking ?? CreateDefaultBooking();

            this.Load += FrmThanhToan_Load;
            this.SizeChanged += FrmThanhToan_SizeChanged;
            pnlContainer.SizeChanged += (s, e) => ResponsiveLayout();

            cboKhachHang.SelectedIndexChanged += cboKhachHang_SelectedIndexChanged;
            btnHuyVe.Click += btnHuyVe_Click;

            // Đảm bảo nút Thanh toán luôn ở trạng thái KHÔNG ẤN ĐƯỢC theo đúng yêu cầu đề bài
            btnThanhToan.Enabled = false;
            btnThanhToan.Cursor = Cursors.No;

            ToolTip tt = new ToolTip();
           
        }

        private ThongTinDatVeDTO CreateDefaultBooking()
        {
            return new ThongTinDatVeDTO
            {
                MaSK = 1,
                TenSK = "Live Concert: HCMUE Melody 2026",
                MaSuat = 101,
                ThoiGianBatDau = DateTime.Now.AddDays(2),
                DiaDiem = "Hội trường A - HCMUE (280 An Dương Vương, Q.5)",
                TenKV = "VIP A",
                HangVe = "VIP",
                GiaVe = 500000m,
                SoLuong = 2,
                DanhSachGhe = new System.Collections.Generic.List<string> { "VIP A-5", "VIP A-6" }
            };
        }

        private void FrmThanhToan_Load(object sender, EventArgs e)
        {
            LoadThongTinDatVe();
            LoadDanhSachKhachHang();
            ResponsiveLayout();
        }

        private void FrmThanhToan_SizeChanged(object sender, EventArgs e)
        {
            ResponsiveLayout();
        }

        private void ResponsiveLayout()
        {
            if (pnlContainer.ClientSize.Width <= 0) return;

            int padding = 20;
            int gap = 20;
            int totalWidth = pnlContainer.ClientSize.Width - padding * 2;
            int bottomHeight = 85;
            int cardHeight = Math.Max(480, pnlContainer.ClientSize.Height - padding * 2 - bottomHeight);

            if (totalWidth <= 600) return;

            int leftWidth = (int)(totalWidth * 0.45f);
            int rightWidth = totalWidth - leftWidth - gap;

            pnlCustomerCard.SetBounds(padding, padding, leftWidth, cardHeight);
            pnlSummaryCard.SetBounds(padding + leftWidth + gap, padding, rightWidth, cardHeight);

            pnlBottomButtons.SetBounds(padding, padding + cardHeight + 10, totalWidth, bottomHeight);

            // Căn lề nút xác nhận thanh toán ở góc phải dưới
            btnThanhToan.Left = totalWidth - btnThanhToan.Width - 20;
            lblThanhToanNote.Left = btnThanhToan.Left;
        }

        #region 1. Hiển thị tóm tắt thông tin vé & Danh sách ghế phân bổ
        private void LoadThongTinDatVe()
        {
            if (_booking == null) return;

            lblSKVal.Text = _booking.TenSK;
            lblSuatVal.Text = $"{_booking.ThoiGianBatDau:dd/MM/yyyy HH:mm} - {_booking.DiaDiem}";
            lblKhuVucVal.Text = $"{_booking.TenKV} (Hạng {_booking.HangVe} - {_booking.GiaVe:N0} đ / vé)";

            Color catColor = GetCategoryColor(_booking.HangVe);
            lblKhuVucVal.ForeColor = catColor;

            lblSoLuongVal.Text = $"{_booking.SoLuong} Vé";
            lblTongTienVal.Text = $"{_booking.TongTien:N0} đ";

            // Hiển thị danh sách ghế cụ thể bằng Badge trực quan
            HienThiDanhSachGheBadge();
        }

        private void HienThiDanhSachGheBadge()
        {
            pnlGheBadges.Controls.Clear();

            if (_booking.DanhSachGhe == null || _booking.DanhSachGhe.Count == 0)
            {
                Label lblEmpty = new Label
                {
                    Text = "Đang phân bổ vị trí ghế...",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Italic),
                    ForeColor = Color.FromArgb(100, 116, 139)
                };
                pnlGheBadges.Controls.Add(lblEmpty);
                return;
            }

            Color badgeBg = GetCategoryColor(_booking.HangVe);

            foreach (string ghe in _booking.DanhSachGhe)
            {
                Guna2Button btnBadge = new Guna2Button
                {
                    Text = $"💺 {ghe}",
                    
                };
                pnlGheBadges.Controls.Add(btnBadge);
            }
        }

        private Color GetCategoryColor(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return Color.FromArgb(245, 158, 11);

            string cat = category.Trim();
            if (cat.IndexOf("VIP", StringComparison.OrdinalIgnoreCase) >= 0)
                return Color.FromArgb(147, 51, 234);
            if (cat.IndexOf("Phổ thông", StringComparison.OrdinalIgnoreCase) >= 0 ||
                cat.IndexOf("GA", StringComparison.OrdinalIgnoreCase) >= 0)
                return Color.FromArgb(56, 189, 248);

            return Color.FromArgb(245, 158, 11);
        }
        #endregion

        #region 2. Truy vấn và liên kết dữ liệu khách hàng
        private void LoadDanhSachKhachHang()
        {
            try
            {
                _dtKhachHang = MockDataService.LayDanhSachKhachHang();

                cboKhachHang.SelectedIndexChanged -= cboKhachHang_SelectedIndexChanged;
                cboKhachHang.DisplayMember = "TenKH";
                cboKhachHang.ValueMember = "MaKH";
                cboKhachHang.DataSource = _dtKhachHang;
                cboKhachHang.SelectedIndexChanged += cboKhachHang_SelectedIndexChanged;

                if (_dtKhachHang != null && _dtKhachHang.Rows.Count > 0)
                {
                    cboKhachHang.SelectedIndex = 0;
                    PopulateCustomerFields(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cboKhachHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboKhachHang.SelectedIndex >= 0)
            {
                PopulateCustomerFields(cboKhachHang.SelectedIndex);
            }
        }

        private void PopulateCustomerFields(int index)
        {
            if (_dtKhachHang == null || index < 0 || index >= _dtKhachHang.Rows.Count) return;

            DataRow r = _dtKhachHang.Rows[index];
            txtTenKH.Text = r["TenKH"]?.ToString() ?? "";
            txtSDT.Text = r["SDT"]?.ToString() ?? "";
            txtEmail.Text = r["Email"]?.ToString() ?? "";
        }
        #endregion

        #region 3. Các nút điều hướng chức năng
        private void btnHuyVe_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                "Bạn có chắc chắn muốn hủy giao dịch đặt vé này và quay trở lại sơ đồ bán vé không?",
                "Xác nhận hủy đặt vé",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                // Quay trở lại trang Bán vé
                if (this.ParentForm is FrmMain mainForm)
                {
                    mainForm.OpenChildForm(new FrmBanVe(), null);
                }
                else
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
        }
        #endregion

        private void lblHeaderSub_Click(object sender, EventArgs e)
        {

        }

        private void lblCustomerCardTitle_Click(object sender, EventArgs e)
        {

        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {

        }
    }
}
