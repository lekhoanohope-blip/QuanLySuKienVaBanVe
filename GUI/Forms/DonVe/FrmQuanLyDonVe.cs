using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmQuanLyDonVe : Form
    {
        #region Fields & Properties
        private int requestVersion;
        private int detailRequestVersion;
        private bool loading;
        private bool loadingDetail;
        private DataTable dtDonVeGoc;
        private bool updatingLayout;
        private bool cancellingOrder;
        private static double preferredMasterRatio = 0.52;

        #endregion

        #region Constructors & Initialization
        public FrmQuanLyDonVe()
        {
            InitializeComponent();
            DoubleBuffered = true;
            KhoiTaoCotDgv();

            InitializeOrderSpacing();



            pnlHeader.SizeChanged += PnlHeader_SizeChanged;
            ClientSizeChanged += PnlHeader_SizeChanged;
            Shown += PnlHeader_SizeChanged;
            splitContainer.SizeChanged += PnlHeader_SizeChanged;
            splitContainer.SplitterMoved += (sender, args) =>
            {
                int available = splitContainer.Width - splitContainer.SplitterWidth;
                if (!updatingLayout && available > 0)
                    preferredMasterRatio = (double)splitContainer.SplitterDistance / available;
            };
            AutoScroll = false;
            txtTimKiem.AccessibleName = "Tìm đơn vé";
            cboLocTrangThai.AccessibleName = "Lọc trạng thái đơn vé";
            PnlHeader_SizeChanged(this, EventArgs.Empty);
        }

        #endregion

        #region UI Setup & Custom Styling (Guna2)
        private void KhoiTaoCotDgv()
        {
            dgvDonVe.AutoGenerateColumns = false;
            dgvDonVe.Columns.Clear();
            colMaDon = new DataGridViewTextBoxColumn
            {
                Name = "colMaDon",
                DataPropertyName = "MaDon",
                HeaderText = "Mã đơn",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                Visible = true
            };
            colMaDonGoc = new DataGridViewTextBoxColumn { Name = "colMaDonGoc", DataPropertyName = "MaDonGoc", HeaderText = "Đơn gốc", ReadOnly = true, Visible = false };
            colTenKH = new DataGridViewTextBoxColumn { Name = "colTenKH", DataPropertyName = "TenKH", HeaderText = "Khách hàng", ReadOnly = true, Visible = false };
            colSDT = new DataGridViewTextBoxColumn { Name = "colSDT", DataPropertyName = "SDT", HeaderText = "Số ĐT", ReadOnly = true, Visible = false };
            colEmail = new DataGridViewTextBoxColumn { Name = "colEmail", DataPropertyName = "Email", HeaderText = "Email", ReadOnly = true, Visible = false };
            colTenSK = new DataGridViewTextBoxColumn { Name = "colTenSK", DataPropertyName = "TenSK", HeaderText = "Sự kiện", ReadOnly = true, Visible = false };
            colThoiGianBatDau = new DataGridViewTextBoxColumn { Name = "colThoiGianBatDau", DataPropertyName = "ThoiGianBatDau", HeaderText = "Giờ diễn", ReadOnly = true, Visible = false };
            colNgayDat = new DataGridViewTextBoxColumn { Name = "colNgayDat", DataPropertyName = "NgayDat", HeaderText = "Ngày đặt", ReadOnly = true, Visible = false };
            colTongTien = new DataGridViewTextBoxColumn { Name = "colTongTien", DataPropertyName = "TongTien", HeaderText = "Tổng tiền", ReadOnly = true, Visible = false };
            colTrangThai = new DataGridViewTextBoxColumn { Name = "colTrangThai", DataPropertyName = "TrangThai", HeaderText = "Trạng thái", ReadOnly = true, Visible = false };
            colSoVeHopLe = new DataGridViewTextBoxColumn { Name = "colSoVeHopLe", DataPropertyName = "SoVeHopLe", HeaderText = "Số vé hợp lệ", ReadOnly = true, Visible = false };
            colTongSoVe = new DataGridViewTextBoxColumn { Name = "colTongSoVe", DataPropertyName = "TongSoVe", HeaderText = "Tổng số vé", ReadOnly = true, Visible = false };

            dgvDonVe.Columns.AddRange(new DataGridViewColumn[] {
                colMaDon, colMaDonGoc, colTenKH, colSDT, colEmail, colTenSK,
                colThoiGianBatDau, colNgayDat, colTongTien, colTrangThai, colSoVeHopLe, colTongSoVe
            });

            dgvChiTietVe.AutoGenerateColumns = false;
            dgvChiTietVe.Columns.Clear();
            colMaGheHienThi = new DataGridViewTextBoxColumn
            {
                Name = "colMaGheHienThi",
                DataPropertyName = "MaGheHienThi",
                HeaderText = "Ghế ngồi",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                Visible = true
            };
            colMaCT = new DataGridViewTextBoxColumn { Name = "colMaCT", DataPropertyName = "MaCT", HeaderText = "Mã vé", ReadOnly = true, Visible = false };
            colMaVeCode = new DataGridViewTextBoxColumn { Name = "colMaVeCode", DataPropertyName = "MaVeCode", HeaderText = "Mã tra cứu", ReadOnly = true, Visible = false };
            colTenKV = new DataGridViewTextBoxColumn { Name = "colTenKV", DataPropertyName = "TenKV", HeaderText = "Khu vực", ReadOnly = true, Visible = false };
            colHangVe = new DataGridViewTextBoxColumn { Name = "colHangVe", DataPropertyName = "HangVe", HeaderText = "Hạng vé", ReadOnly = true, Visible = false };
            colGiaVe = new DataGridViewTextBoxColumn { Name = "colGiaVe", DataPropertyName = "GiaVe", HeaderText = "Giá vé", ReadOnly = true, Visible = false };
            colTrangThaiVe = new DataGridViewTextBoxColumn { Name = "colTrangThaiVe", DataPropertyName = "TrangThai", HeaderText = "Trạng thái", ReadOnly = true, Visible = false };

            dgvChiTietVe.Columns.AddRange(new DataGridViewColumn[] {
                colMaGheHienThi, colMaCT, colMaVeCode, colTenKV, colHangVe, colGiaVe, colTrangThaiVe
            });
        }

        private void PnlHeader_SizeChanged(object sender, EventArgs e)
        {
            ApplyResponsiveLayout(DeviceDpi / 96f);
        }

        private static int ScalePixels(int pixels, float scale)
        {
            return (int)Math.Round(pixels * scale);
        }



        private async void FrmQuanLyDonVe_Load(object sender, EventArgs e)
        {
            pnlTitle.Visible = TopLevel;
            if (!TopLevel)
            {
                FormBorderStyle = FormBorderStyle.None;
                MinimumSize = Size.Empty;
            }

            KhoiTaoLocTrangThai();
            PnlHeader_SizeChanged(this, EventArgs.Empty);
            await LoadDanhSachDonVeAsync();
        }

        #endregion

        #region Data Binding & Loading
        private void KhoiTaoLocTrangThai()
        {
            cboLocTrangThai.Items.Clear();
            cboLocTrangThai.Items.Add("Tất cả trạng thái");
            cboLocTrangThai.Items.Add("Đã thanh toán");
            cboLocTrangThai.Items.Add("Đã hủy");
            cboLocTrangThai.Items.Add("Đã đổi");
            cboLocTrangThai.SelectedIndex = 0;
        }

        private async Task LoadDanhSachDonVeAsync(int selectMaDon = 0)
        {
            searchTimer.Stop();
            int version = ++requestVersion;
            string keyword = txtTimKiem.Text.Trim();
            loading = true;
            lblCount.Text = "Đang tải dữ liệu...";
            CapNhatTrangThaiNutThaoTac();

            try
            {
                DataTable data = await Task.Run(() => DonVeBLL.TimKiemDonVe(keyword));
                if (IsDisposed || version != requestVersion) return;

                dtDonVeGoc = data;
                ApDungBoLocVaHienThi(selectMaDon);
            }
            catch (Exception ex)
            {
                if (IsDisposed || version != requestVersion) return;
                dgvDonVe.DataSource = null;
                dgvChiTietVe.DataSource = null;
                lblCount.Text = "Lỗi tải: " + ex.Message;
            }
            finally
            {
                if (!IsDisposed && version == requestVersion)
                {
                    loading = false;
                    CapNhatTrangThaiNutThaoTac();
                }
            }

            if (!IsDisposed && version == requestVersion)
            {
                await LoadChiTietVeAsync();
            }
        }

        private void ApDungBoLocVaHienThi(int selectMaDon = 0)
        {
            if (dtDonVeGoc == null) return;

            string trangThaiLoc = cboLocTrangThai.SelectedIndex > 0
                ? (cboLocTrangThai.SelectedIndex == 1 ? TrangThaiDonVe.DaThanhToan
                    : cboLocTrangThai.SelectedIndex == 2 ? TrangThaiDonVe.DaHuy
                    : TrangThaiDonVe.DaDoi)
                : null;

            DataTable dtDisplay;
            if (string.IsNullOrEmpty(trangThaiLoc))
            {
                dtDisplay = dtDonVeGoc;
            }
            else
            {
                DataView dv = new DataView(dtDonVeGoc);
                dv.RowFilter = $"TrangThai = '{trangThaiLoc}'";
                dtDisplay = dv.ToTable();
            }

            dgvChiTietVe.DataSource = null;
            dgvDonVe.DataSource = dtDisplay;

            lblCount.Text = string.Format("{0:N0} đơn vé", dtDisplay.Rows.Count);

            if (dgvDonVe.Rows.Count > 0)
            {
                int index = 0;
                if (selectMaDon > 0)
                {
                    for (int i = 0; i < dgvDonVe.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(dgvDonVe.Rows[i].Cells["colMaDon"].Value) == selectMaDon)
                        {
                            index = i;
                            break;
                        }
                    }
                }

                if (index >= 0 && index < dgvDonVe.Rows.Count)
                {
                    dgvDonVe.ClearSelection();
                    dgvDonVe.Rows[index].Selected = true;
                    if (colMaDon != null && colMaDon.Visible)
                    {
                        dgvDonVe.CurrentCell = dgvDonVe.Rows[index].Cells["colMaDon"];
                    }
                }
            }
            else
            {
                dgvChiTietVe.DataSource = null;
                lblDetailTitle.Text = "Đơn vé: —";
                lblDetailBadge.Text = "";
                lblCustomerName.Text = "Chưa chọn đơn vé";
                lblCustomerPhone.Text = "—";
                lblCustomerEmail.Visible = false;
                lblDetailEventName.Text = "—";
                lblDetailEventTime.Text = "—";
                lblRuleBadge.Text = "Không có đơn vé nào phù hợp điều kiện tìm kiếm.";
                lblRuleBadge.ForeColor = Color.FromArgb(100, 116, 139);
                lblTicketsTitle.Text = "Vé trong đơn";
            }

            CapNhatTrangThaiNutThaoTac();
        }

        private DataGridViewRow LayDongDonVeDangChon()
        {
            if (dgvDonVe.SelectedRows.Count > 0)
            {
                var r = dgvDonVe.SelectedRows[0];
                if (r.Index >= 0 && r.Index < dgvDonVe.Rows.Count) return r;
            }
            if (dgvDonVe.CurrentRow != null && dgvDonVe.CurrentRow.Selected)
            {
                if (dgvDonVe.CurrentRow.Index >= 0 && dgvDonVe.CurrentRow.Index < dgvDonVe.Rows.Count)
                    return dgvDonVe.CurrentRow;
            }
            if (dgvDonVe.CurrentRow != null && dgvDonVe.CurrentRow.Index >= 0 && dgvDonVe.CurrentRow.Index < dgvDonVe.Rows.Count)
            {
                return dgvDonVe.CurrentRow;
            }
            return null;
        }

        private async void dgvDonVe_SelectionChanged(object sender, EventArgs e)
        {
            if (loading) return;
            await LoadChiTietVeAsync();
        }

        private async Task LoadChiTietVeAsync()
        {
            var row = LayDongDonVeDangChon();
            if (row == null || row.Index < 0 || row.Index >= dgvDonVe.Rows.Count || loading)
            {
                detailRequestVersion++;
                dgvChiTietVe.DataSource = null;
                lblDetailTitle.Text = "Đơn vé: —";
                lblDetailBadge.Text = "";
                lblCustomerName.Text = "Chưa chọn đơn vé";
                lblCustomerPhone.Text = "—";
                lblCustomerEmail.Visible = false;
                lblDetailEventName.Text = "—";
                lblDetailEventTime.Text = "—";
                lblRuleBadge.Text = (dgvDonVe.Rows.Count == 0)

                    ? "Không có đơn vé nào phù hợp điều kiện tìm kiếm."

                    : "Vui lòng chọn một đơn vé trong danh sách để xem chi tiết.";
                lblRuleBadge.ForeColor = Color.FromArgb(100, 116, 139);
                lblTicketsTitle.Text = "Vé trong đơn";
                CapNhatTrangThaiNutThaoTac();
                return;
            }

            int version = ++detailRequestVersion;
            loadingDetail = true;
            dgvChiTietVe.DataSource = null;
            CapNhatTrangThaiNutThaoTac();

            object maDonObj = row.Cells["colMaDon"]?.Value;
            if (maDonObj == null || maDonObj == DBNull.Value) return;
            int maDon = Convert.ToInt32(maDonObj);
            string trangThai = row.Cells["colTrangThai"]?.Value?.ToString() ?? "";
            string tenKH = row.Cells["colTenKH"]?.Value?.ToString() ?? "";
            string sdt = row.Cells["colSDT"]?.Value?.ToString() ?? "";
            string email = row.Cells["colEmail"]?.Value?.ToString()?.Trim() ?? "";
            string tenSK = row.Cells["colTenSK"]?.Value?.ToString() ?? "";
            DateTime thoiGianBatDau = DateTime.MinValue;
            if (row.Cells["colThoiGianBatDau"]?.Value != null && row.Cells["colThoiGianBatDau"]?.Value != DBNull.Value)
            {
                DateTime.TryParse(row.Cells["colThoiGianBatDau"].Value.ToString(), out thoiGianBatDau);
            }
            TimeSpan timeSpan = thoiGianBatDau - DateTime.Now;

            string trangThaiVN = trangThai == TrangThaiDonVe.DaThanhToan ? "Đã thanh toán"
                : (trangThai == TrangThaiDonVe.DaHuy ? "Đã hủy"
                : (trangThai == TrangThaiDonVe.DaDoi ? "Đã đổi" : trangThai));

            lblDetailTitle.Text = string.Format("Đơn #{0}", maDon);
            lblDetailBadge.Text = trangThaiVN;
            if (trangThai == TrangThaiDonVe.DaThanhToan)
            {
                lblDetailBadge.ForeColor = Color.FromArgb(6, 95, 70);
                lblDetailBadge.BackColor = Color.FromArgb(209, 250, 229);
            }
            else if (trangThai == TrangThaiDonVe.DaHuy)
            {
                lblDetailBadge.ForeColor = Color.FromArgb(153, 27, 27);
                lblDetailBadge.BackColor = Color.FromArgb(254, 226, 226);
            }
            else if (trangThai == TrangThaiDonVe.DaDoi)
            {
                lblDetailBadge.ForeColor = Color.FromArgb(30, 64, 175);
                lblDetailBadge.BackColor = Color.FromArgb(219, 234, 254);
            }
            else
            {
                lblDetailBadge.ForeColor = Color.FromArgb(71, 85, 105);
                lblDetailBadge.BackColor = Color.FromArgb(241, 245, 249);
            }

            lblCustomerName.Text = tenKH;
            lblCustomerPhone.Text = !string.IsNullOrWhiteSpace(sdt) ? sdt : "—";

            lblCustomerEmail.Text = email;
            lblCustomerEmail.Visible = !string.IsNullOrWhiteSpace(email);
            lblCustomerPhone.Visible = !string.IsNullOrWhiteSpace(sdt);

            lblDetailEventName.Text = tenSK;
            lblDetailEventTime.Text = thoiGianBatDau.ToString("dd/MM/yyyy · HH:mm");

            // Cập nhật banner trạng thái quy định
            if (trangThai == TrangThaiDonVe.DaHuy)
            {
                lblRuleBadge.ForeColor = Color.FromArgb(185, 28, 28);
                lblRuleBadge.Text = "Đơn vé này đã bị hủy toàn bộ.";
            }
            else if (trangThai == TrangThaiDonVe.DaDoi)
            {
                lblRuleBadge.ForeColor = Color.FromArgb(30, 64, 175);
                lblRuleBadge.Text = "Đơn vé này đã được đổi sang đơn mới.";
            }
            else
            {
                if (timeSpan.TotalMinutes <= 0)
                {
                    lblRuleBadge.ForeColor = Color.FromArgb(100, 116, 139);
                    lblRuleBadge.Text = string.Format("Suất diễn đã diễn ra ({0:dd/MM/yyyy HH:mm})", thoiGianBatDau);
                }
                else
                {
                    string thoiGianStr = timeSpan.Days > 0
                        ? string.Format("Còn {0} ngày {1} giờ", timeSpan.Days, timeSpan.Hours)
                        : string.Format("Còn {0} giờ {1} phút", (int)Math.Floor(timeSpan.TotalHours), timeSpan.Minutes);

                    if (timeSpan.TotalHours >= 24.0)
                    {
                        lblRuleBadge.ForeColor = Color.FromArgb(22, 101, 52);
                        lblRuleBadge.Text = string.Format("{0} · Có thể hủy hoặc đổi vé", thoiGianStr);
                    }
                    else if (timeSpan.TotalHours >= 12.0)
                    {
                        lblRuleBadge.ForeColor = Color.FromArgb(146, 64, 14);
                        lblRuleBadge.Text = string.Format("{0} · Có thể đổi vé (Hết hạn hủy)", thoiGianStr);
                    }
                    else
                    {
                        lblRuleBadge.ForeColor = Color.FromArgb(185, 28, 28);
                        lblRuleBadge.Text = string.Format("{0} · Không thể đổi vé hoặc hủy đơn", thoiGianStr);
                    }
                }
            }

            try
            {
                DataTable dtCT = await Task.Run(() => DonVeBLL.LayChiTietVeTheoDon(maDon));
                if (IsDisposed || version != detailRequestVersion) return;

                var currentRow = LayDongDonVeDangChon();
                if (currentRow == null || currentRow.Index < 0 || Convert.ToInt32(currentRow.Cells["colMaDon"].Value) != maDon)
                {
                    _ = LoadChiTietVeAsync();
                    return;
                }

                dgvChiTietVe.DataSource = dtCT;
                lblTicketsTitle.Text = string.Format("Vé trong đơn ({0})", dtCT.Rows.Count);

                if (dgvChiTietVe.Rows.Count > 0)
                {
                    dgvChiTietVe.ClearSelection();
                    dgvChiTietVe.Rows[0].Selected = true;
                    if (colMaGheHienThi != null && colMaGheHienThi.Visible)
                    {
                        dgvChiTietVe.CurrentCell = dgvChiTietVe.Rows[0].Cells["colMaGheHienThi"];
                    }
                }
            }
            catch (Exception ex)
            {
                if (IsDisposed || version != detailRequestVersion) return;
                dgvChiTietVe.DataSource = null;
                lblRuleBadge.Text = "Lỗi tải chi tiết vé: " + ex.Message;
            }
            finally
            {
                if (!IsDisposed && version == detailRequestVersion)
                {
                    loadingDetail = false;
                    CapNhatTrangThaiNutThaoTac();
                }
            }
        }

        #endregion

        #region Action Buttons & Helper Methods
        private void CapNhatTrangThaiNutThaoTac()
        {
            var row = LayDongDonVeDangChon();
            bool coDonChon = !loading && !loadingDetail && !cancellingOrder && row != null && row.Index >= 0 && row.Index < dgvDonVe.Rows.Count;
            if (!coDonChon)
            {
                btnHuyVe.Enabled = false;
                btnDoiVe.Enabled = false;
                if (btnInVe != null) btnInVe.Enabled = false;
                return;
            }

            int maDon = Convert.ToInt32(row.Cells["colMaDon"].Value);
            string trangThai = row.Cells["colTrangThai"].Value?.ToString() ?? "";
            DateTime thoiGianBatDau = Convert.ToDateTime(row.Cells["colThoiGianBatDau"].Value);
            double gioConLai = (thoiGianBatDau - DateTime.Now).TotalHours;

            int soVeHopLe = 0;
            if (row.Cells["colSoVeHopLe"]?.Value != null && row.Cells["colSoVeHopLe"].Value != DBNull.Value)
                int.TryParse(row.Cells["colSoVeHopLe"].Value.ToString(), out soVeHopLe);

            bool hopLeHuy = (trangThai == TrangThaiDonVe.DaThanhToan && gioConLai >= 24.0 && soVeHopLe > 0);

            bool hopLeDoi = false;
            if (trangThai == TrangThaiDonVe.DaThanhToan && gioConLai >= 12.0 && dgvChiTietVe.CurrentRow != null && dgvChiTietVe.CurrentRow.Index >= 0 && dgvChiTietVe.CurrentRow.Index < dgvChiTietVe.Rows.Count)
            {
                var drv = dgvChiTietVe.CurrentRow.DataBoundItem as DataRowView;
                if (drv != null && (!drv.Row.Table.Columns.Contains("MaDon") || Convert.ToInt32(drv["MaDon"]) == maDon))
                {
                    string trangThaiVe = drv["TrangThai"]?.ToString() ?? "";
                    if (trangThaiVe == TrangThaiChiTietVe.HopLe)
                    {
                        hopLeDoi = true;
                    }
                }
            }

            bool hopLeIn = (trangThai == TrangThaiDonVe.DaThanhToan);
            if (!hopLeIn && dgvChiTietVe.Rows.Count > 0)
            {
                foreach (DataGridViewRow r in dgvChiTietVe.Rows)
                {
                    if (r.Cells["colTrangThaiVe"].Value?.ToString() == TrangThaiChiTietVe.HopLe)
                    {
                        hopLeIn = true;
                        break;
                    }
                }
            }

            btnHuyVe.Enabled = hopLeHuy;
            btnDoiVe.Enabled = hopLeDoi;
            if (btnInVe != null) btnInVe.Enabled = hopLeIn;
        }

        #endregion

        #region Event Handlers
        private void dgvChiTietVe_SelectionChanged(object sender, EventArgs e)
        {
            CapNhatTrangThaiNutThaoTac();
        }

        private void btnCopyMaVe_Click(object sender, EventArgs e)
        {
            var ctRow = dgvChiTietVe.CurrentRow;
            if (ctRow == null || ctRow.Index < 0 || ctRow.Index >= dgvChiTietVe.Rows.Count)
            {
                MessageBox.Show(this, "Vui lòng chọn một vé trong danh sách để sao chép mã tra cứu.", "Chưa chọn vé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string maVeCode = ctRow.Cells["colMaVeCode"]?.Value?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(maVeCode))
            {
                MessageBox.Show(this, "Vé này chưa có mã tra cứu.", "Không có mã vé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                Clipboard.SetText(maVeCode);
                if (btnCopyMaVe != null)
                {
                    btnCopyMaVe.Text = "Đã chép mã!";
                    Timer t = new Timer { Interval = 1500 };
                    t.Tick += (s, args) =>
                    {
                        t.Stop();
                        t.Dispose();
                        if (!IsDisposed && btnCopyMaVe != null && !btnCopyMaVe.IsDisposed)
                            btnCopyMaVe.Text = "Sao chép mã";
                    };
                    t.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Không thể sao chép vào bộ nhớ tạm: " + ex.Message, "Lỗi sao chép", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btnInVe_Click(object sender, EventArgs e)
        {
            var row = LayDongDonVeDangChon();
            if (row == null || row.Index < 0) return;
            int maDon = Convert.ToInt32(row.Cells["colMaDon"].Value);
            FrmInVe.InDanhSachVe(maDon, this);
        }

        private async void btnHuyVe_Click(object sender, EventArgs e)
        {
            if (cancellingOrder) return;
            var row = LayDongDonVeDangChon();
            if (row == null || row.Index < 0) return;

            int maDon = Convert.ToInt32(row.Cells["colMaDon"].Value);
            string tenKH = row.Cells["colTenKH"].Value?.ToString() ?? "";
            string sdt = row.Cells["colSDT"].Value?.ToString() ?? "";
            string tenSK = row.Cells["colTenSK"].Value?.ToString() ?? "";
            try
            {
                cancellingOrder = true;
                UseWaitCursor = true;
                CapNhatTrangThaiNutThaoTac();
                DonVeResult check = await Task.Run(() => DonVeBLL.KiemTraDieuKienHuy(maDon));
                if (IsDisposed) return;
                if (!check.Success)
                {
                    MessageBox.Show(this, check.Message, "Không đủ điều kiện hủy vé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataTable chiTiet = await Task.Run(() => DonVeBLL.LayChiTietVeTheoDon(maDon));
                if (IsDisposed) return;
                string loiNhan = TaoNoiDungXacNhanHuy(maDon, tenKH, sdt, tenSK, chiTiet);
                if (string.IsNullOrEmpty(loiNhan))
                {
                    MessageBox.Show(this, "Đơn vé không còn vé hiệu lực để hủy. Vui lòng làm mới danh sách.",
                        "Dữ liệu đã thay đổi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                UseWaitCursor = false;
                if (MessageBox.Show(this, loiNhan, "Xác nhận hủy đơn vé", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                UseWaitCursor = true;
                DonVeResult result = await Task.Run(() => DonVeBLL.HuyVe(maDon));
                if (IsDisposed) return;
                if (result.Success)
                {
                    MessageBox.Show(this, result.Message, "Hủy đơn vé thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDanhSachDonVeAsync(maDon);
                }
                else
                {
                    MessageBox.Show(this, result.Message, "Hủy thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                if (!IsDisposed)
                    MessageBox.Show(this, "Lỗi trong quá trình hủy đơn vé: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cancellingOrder = false;
                if (!IsDisposed)
                {
                    UseWaitCursor = false;
                    CapNhatTrangThaiNutThaoTac();
                }
            }
        }

        private static string TaoNoiDungXacNhanHuy(int maDon, string tenKH, string sdt,
            string tenSK, DataTable chiTiet)
        {
            int soVeHieuLuc = 0;
            decimal giaTriVe = 0m;
            foreach (DataRow ve in chiTiet.Rows)
            {
                if (Convert.ToInt32(ve["MaDon"]) != maDon ||
                    ve["TrangThai"].ToString() != TrangThaiChiTietVe.HopLe) continue;
                soVeHieuLuc++;
                giaTriVe += Convert.ToDecimal(ve["GiaVe"]);
            }
            if (soVeHieuLuc == 0) return string.Empty;
            return string.Format(
                "Xác nhận hủy đơn vé #{0}?\n\n" +
                "• Khách hàng: {1} (SĐT: {2})\n" +
                "• Sự kiện: {3}\n" +
                "• Số vé hiệu lực sẽ hủy: {4} vé\n" +
                "• Giá trị vé bị hủy: {5:N0} VNĐ\n\n" +
                "{4} ghế tương ứng sẽ được trả về trạng thái Trống.\n" +
                "Việc hoàn tiền (nếu có) do nhân viên xử lý riêng theo chính sách sự kiện.",
                maDon, tenKH, sdt, tenSK, soVeHieuLuc, giaTriVe);
        }

        private async void btnDoiVe_Click(object sender, EventArgs e)
        {
            var row = LayDongDonVeDangChon();
            if (row == null || row.Index < 0) return;
            int maDon = Convert.ToInt32(row.Cells["colMaDon"].Value);

            DonVeResult check = DonVeBLL.KiemTraDieuKienDoi(maDon);
            if (!check.Success)
            {
                MessageBox.Show(this, check.Message, "Không đủ điều kiện đổi vé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvChiTietVe.CurrentRow == null || dgvChiTietVe.CurrentRow.Index < 0 || dgvChiTietVe.CurrentRow.Index >= dgvChiTietVe.Rows.Count)
            {
                MessageBox.Show(this, "Vui lòng chọn 1 vé trong danh sách Vé trong đơn để thực hiện đổi vé!", "Chưa chọn vé cần đổi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var ctRow = dgvChiTietVe.CurrentRow;
            var drv = ctRow.DataBoundItem as DataRowView;
            if (drv == null || Convert.ToInt32(drv["MaDon"]) != maDon)
            {
                MessageBox.Show(this, "Dữ liệu vé đang chọn chưa đồng bộ với đơn vé. Đang làm mới danh sách vé...", "Dữ liệu chưa đồng bộ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                await LoadChiTietVeAsync();
                return;
            }

            string trangThaiVe = drv["TrangThai"]?.ToString() ?? "";
            if (trangThaiVe != TrangThaiChiTietVe.HopLe)
            {
                MessageBox.Show(this, "Vé này đã bị hủy hoặc không hợp lệ, không thể tiếp tục đổi!", "Vé không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ChiTietVeDTO veCu = new ChiTietVeDTO
            {
                MaCT = Convert.ToInt32(drv["MaCT"]),
                MaDon = Convert.ToInt32(drv["MaDon"]),
                MaGheHienThi = drv["MaGheHienThi"]?.ToString() ?? "",
                TenKV = drv["TenKV"]?.ToString() ?? "",
                HangVe = drv["HangVe"]?.ToString() ?? "",
                GiaVe = Convert.ToDecimal(drv["GiaVe"]),
                MaVeCode = drv["MaVeCode"]?.ToString() ?? "",
                TrangThai = trangThaiVe
            };

            DataRow donVeFull = DonVeBLL.LayThongTinDonVeVaSuatDien(maDon);
            using (FrmDoiVe frm = new FrmDoiVe(donVeFull, veCu))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    int maDonMoi = frm.MaDonMoiDaTao;
                    await LoadDanhSachDonVeAsync(maDonMoi > 0 ? maDonMoi : maDon);
                    if (maDonMoi > 0)
                    {
                        DialogResult inNgay = MessageBox.Show(this,
                            string.Format("Đổi vé thành công! Đơn vé mới đã được tạo với mã #{0}.\nBạn có muốn in vé mới ngay bây giờ không?", maDonMoi),
                            "In vé đổi thành công",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);
                        if (inNgay == DialogResult.Yes)
                        {
                            FrmInVe.InDanhSachVe(maDonMoi, this);
                        }
                    }
                }
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            ++requestVersion;
            searchTimer.Stop();
            searchTimer.Start();
        }

        private async void searchTimer_Tick(object sender, EventArgs e)
        {
            await LoadDanhSachDonVeAsync();
        }

        private async void cboLocTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) return;
            try
            {
                loading = true;
                ApDungBoLocVaHienThi();
            }
            finally
            {
                loading = false;
            }
            await LoadChiTietVeAsync();
        }

        private async void btnLamMoi_Click(object sender, EventArgs e)
        {
            await LoadDanhSachDonVeAsync();
        }

        private void dgvDonVe_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvDonVe.Rows.Count || colMaDon == null || e.ColumnIndex != colMaDon.Index) return;

            float scale = DeviceDpi / 96f;
            e.Handled = true;
            Graphics g = e.Graphics;
            Rectangle bounds = e.CellBounds;
            bool isSelected = (e.State & DataGridViewElementStates.Selected) != 0;

            Color bg = isSelected ? Color.FromArgb(236, 253, 245) : Color.White;
            using (Brush b = new SolidBrush(bg))
            {
                g.FillRectangle(b, bounds);
            }

            using (Pen p = new Pen(Color.FromArgb(241, 245, 249)))
            {
                g.DrawLine(p, bounds.Left, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
            }

            if (isSelected)
            {
                using (Brush b = new SolidBrush(Color.FromArgb(16, 185, 129)))
                {
                    g.FillRectangle(b, bounds.Left, bounds.Top, ScalePixels(4, scale), bounds.Height);
                }
            }

            var row = dgvDonVe.Rows[e.RowIndex];
            string maDon = row.Cells["colMaDon"].Value?.ToString() ?? "";
            string tenKH = row.Cells["colTenKH"].Value?.ToString() ?? "";
            string tenSK = row.Cells["colTenSK"].Value?.ToString() ?? "";
            string trangThai = row.Cells["colTrangThai"].Value?.ToString() ?? "";
            object maDonGocObj = row.Cells["colMaDonGoc"].Value;
            string maDonGoc = (maDonGocObj != null && maDonGocObj != DBNull.Value) ? maDonGocObj.ToString() : "";
            int tongSoVe = 0;
            if (row.Cells["colTongSoVe"].Value != null && row.Cells["colTongSoVe"].Value != DBNull.Value)
                int.TryParse(row.Cells["colTongSoVe"].Value.ToString(), out tongSoVe);
            int soVeHopLe = 0;
            if (row.Cells["colSoVeHopLe"].Value != null && row.Cells["colSoVeHopLe"].Value != DBNull.Value)
                int.TryParse(row.Cells["colSoVeHopLe"].Value.ToString(), out soVeHopLe);
            decimal tongTien = 0m;
            if (row.Cells["colTongTien"].Value != null && row.Cells["colTongTien"].Value != DBNull.Value)
                decimal.TryParse(row.Cells["colTongTien"].Value.ToString(), out tongTien);

            int padLeft = bounds.Left + ScalePixels(20, scale);
            int padRight = bounds.Right - ScalePixels(20, scale);

            string statusText = trangThai == TrangThaiDonVe.DaThanhToan ? "Đã thanh toán"
                : (trangThai == TrangThaiDonVe.DaHuy ? "Đã hủy"
                : (trangThai == TrangThaiDonVe.DaDoi ? "Đã đổi" : trangThai));
            Color badgeBg = trangThai == TrangThaiDonVe.DaThanhToan ? Color.FromArgb(209, 250, 229)
                : (trangThai == TrangThaiDonVe.DaHuy ? Color.FromArgb(254, 226, 226)
                : Color.FromArgb(219, 234, 254));
            Color badgeFg = trangThai == TrangThaiDonVe.DaThanhToan ? Color.FromArgb(6, 95, 70)
                : (trangThai == TrangThaiDonVe.DaHuy ? Color.FromArgb(153, 27, 27)
                : Color.FromArgb(30, 64, 175));

            int badgeX = padRight;
            using (Font badgeFont = new Font("Segoe UI", 8.5f, FontStyle.Bold))
            {
                Size badgeSize = TextRenderer.MeasureText(statusText, badgeFont);
                int badgeW = badgeSize.Width + ScalePixels(24, scale);
                int badgeH = Math.Max(badgeSize.Height + ScalePixels(10, scale), ScalePixels(28, scale));
                badgeX = padRight - badgeW;
                int badgeY = bounds.Top + ScalePixels(14, scale);
                Rectangle badgeRect = new Rectangle(badgeX, badgeY, badgeW, badgeH);

                ManagementGridRenderer.FillRoundedRectangle(g, badgeRect, 4, badgeBg);
                TextRenderer.DrawText(g, statusText, badgeFont, badgeRect, badgeFg,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);

                using (Font orderFont = new Font("Segoe UI", 10f, FontStyle.Bold))
                using (Font nameFont = new Font("Segoe UI", 10f, FontStyle.Bold))
                {
                    string orderStr = "#" + maDon;
                    Size orderSize = TextRenderer.MeasureText(orderStr, orderFont);
                    Rectangle orderRect = new Rectangle(padLeft, bounds.Top + ScalePixels(16, scale), orderSize.Width, orderSize.Height);
                    TextRenderer.DrawText(g, orderStr, orderFont, orderRect, Color.FromArgb(15, 23, 42), TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

                    int nameLeft = orderRect.Right + ScalePixels(8, scale);
                    int maxNameW = Math.Max(10, badgeX - nameLeft - ScalePixels(8, scale));
                    Rectangle nameRect = new Rectangle(nameLeft, bounds.Top + ScalePixels(16, scale), maxNameW, orderSize.Height);
                    TextRenderer.DrawText(g, tenKH, nameFont, nameRect, Color.FromArgb(15, 23, 42), TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
                }
            }

            using (Font eventFont = new Font("Segoe UI", 9f, FontStyle.Regular))
            {
                int eventW = Math.Max(10, bounds.Width - ScalePixels(40, scale));
                Rectangle eventRect = new Rectangle(padLeft, bounds.Top + ScalePixels(46, scale), eventW, eventFont.Height + ScalePixels(4, scale));
                TextRenderer.DrawText(g, tenSK, eventFont, eventRect, Color.FromArgb(71, 85, 105), TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }

            using (Font summaryFont = new Font("Segoe UI", 9.5f, FontStyle.Bold))
            using (Font originFont = new Font("Segoe UI", 8.5f, FontStyle.Regular))
            {
                string ticketCountStr = (soVeHopLe > 0 && soVeHopLe < tongSoVe)
                    ? string.Format("{0} vé hiệu lực / {1} vé", soVeHopLe, tongSoVe)
                    : string.Format("{0} vé", tongSoVe);
                string sumStr = string.Format("{0}  ·  {1:N0}đ", ticketCountStr, tongTien);
                Size sumSize = TextRenderer.MeasureText(sumStr, summaryFont);
                Rectangle sumRect = new Rectangle(padLeft, bounds.Top + ScalePixels(76, scale), sumSize.Width, sumSize.Height);
                TextRenderer.DrawText(g, sumStr, summaryFont, sumRect, Color.FromArgb(6, 95, 70), TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

                if (!string.IsNullOrEmpty(maDonGoc))
                {
                    string originStr = string.Format(" · Đổi từ #{0}", maDonGoc);
                    int originLeft = sumRect.Right + 2;
                    int maxOriginW = Math.Max(10, padRight - originLeft);
                    Rectangle originRect = new Rectangle(originLeft, bounds.Top + ScalePixels(77, scale), maxOriginW, originFont.Height + ScalePixels(4, scale));
                    TextRenderer.DrawText(g, originStr, originFont, originRect, Color.FromArgb(71, 85, 105), TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
                }
            }
        }
        private void dgvChiTietVe_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvChiTietVe.Rows.Count || colMaGheHienThi == null || e.ColumnIndex != colMaGheHienThi.Index) return;

            float scale = DeviceDpi / 96f;
            e.Handled = true;
            Graphics g = e.Graphics;
            Rectangle bounds = e.CellBounds;
            bool isSelected = (e.State & DataGridViewElementStates.Selected) != 0;

            Color bg = isSelected ? Color.FromArgb(236, 253, 245) : Color.White;
            using (Brush b = new SolidBrush(bg))
            {
                g.FillRectangle(b, bounds);
            }

            using (Pen p = new Pen(Color.FromArgb(241, 245, 249)))
            {
                g.DrawLine(p, bounds.Left, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
            }

            if (isSelected)
            {
                using (Brush b = new SolidBrush(Color.FromArgb(16, 185, 129)))
                {
                    g.FillRectangle(b, bounds.Left, bounds.Top, ScalePixels(3, scale), bounds.Height);
                }
            }

            var row = dgvChiTietVe.Rows[e.RowIndex];
            string maGhe = row.Cells["colMaGheHienThi"].Value?.ToString() ?? "";
            string hangVe = row.Cells["colHangVe"].Value?.ToString() ?? "";
            string tenKV = row.Cells["colTenKV"].Value?.ToString() ?? "";
            string trangThai = row.Cells["colTrangThaiVe"].Value?.ToString() ?? "";
            string maVeCode = row.Cells["colMaVeCode"].Value?.ToString() ?? "";
            decimal giaVe = 0m;
            if (row.Cells["colGiaVe"].Value != null && row.Cells["colGiaVe"].Value != DBNull.Value)
                decimal.TryParse(row.Cells["colGiaVe"].Value.ToString(), out giaVe);

            int padLeft = bounds.Left + ScalePixels(20, scale);
            int padRight = bounds.Right - ScalePixels(20, scale);
            int cy = bounds.Top + bounds.Height / 2;

            int rightReserved = ScalePixels(115, scale);
            if (trangThai == TrangThaiChiTietVe.DaHuy)
            {
                using (Font badgeFont = new Font("Segoe UI", 8.5f, FontStyle.Bold))
                {
                    string badgeText = "Đã hủy";
                    Size badgeSize = TextRenderer.MeasureText(badgeText, badgeFont);
                    int bw = badgeSize.Width + ScalePixels(20, scale);
                    int bh = badgeSize.Height + ScalePixels(8, scale);
                    Rectangle br = new Rectangle(padRight - bw, cy - bh / 2, bw, bh);
                    ManagementGridRenderer.FillRoundedRectangle(g, br, 3, Color.FromArgb(254, 226, 226));
                    TextRenderer.DrawText(g, badgeText, badgeFont, br, Color.FromArgb(153, 27, 27),
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);
                    rightReserved = bw + ScalePixels(12, scale);
                }
            }
            else
            {
                using (Font priceFont = new Font("Segoe UI", 9.5f, FontStyle.Bold))
                {
                    string priceStr = string.Format("{0:N0}đ", giaVe);
                    Size priceSize = TextRenderer.MeasureText(priceStr, priceFont);
                    Rectangle pr = new Rectangle(padRight - priceSize.Width, cy - priceSize.Height / 2, priceSize.Width, priceSize.Height);
                    TextRenderer.DrawText(g, priceStr, priceFont, pr, Color.FromArgb(15, 23, 42), TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                    rightReserved = priceSize.Width + ScalePixels(12, scale);
                }
            }

            Color bulletColor = trangThai == TrangThaiChiTietVe.HopLe ? Color.FromArgb(16, 185, 129) : Color.FromArgb(239, 68, 68);
            using (Brush bb = new SolidBrush(bulletColor))
            {
                g.FillEllipse(bb, padLeft, cy - ScalePixels(4, scale), ScalePixels(8, scale), ScalePixels(8, scale));
            }

            int textLeft = padLeft + ScalePixels(18, scale);
            int availableTextWidth = Math.Max(10, (padRight - rightReserved) - textLeft);

            // Line 1: Ghế ngồi + Hạng vé (Khu vực)
            using (Font seatFont = new Font("Segoe UI", 9.5f, FontStyle.Bold))
            using (Font subFont = new Font("Segoe UI", 9f, FontStyle.Regular))
            {
                Size seatSize = TextRenderer.MeasureText(maGhe, seatFont);
                int line1Y = bounds.Top + ScalePixels(10, scale);
                Rectangle seatRect = new Rectangle(textLeft, line1Y, seatSize.Width, seatSize.Height);
                TextRenderer.DrawText(g, maGhe, seatFont, seatRect, Color.FromArgb(15, 23, 42), TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

                string infoStr = !string.IsNullOrEmpty(tenKV) ? string.Format(" · {0} ({1})", hangVe, tenKV) : string.Format(" · {0}", hangVe);
                int infoLeft = seatRect.Right;
                int maxInfoW = Math.Max(10, (padRight - rightReserved) - infoLeft);
                Rectangle infoRect = new Rectangle(infoLeft, line1Y, maxInfoW, seatSize.Height);
                TextRenderer.DrawText(g, infoStr, subFont, infoRect, Color.FromArgb(71, 85, 105), TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }

            // Line 2: Mã tra cứu vé (MaVeCode)
            if (!string.IsNullOrEmpty(maVeCode))
            {
                using (Font codeFont = new Font("Segoe UI", 8.5f, FontStyle.Regular))
                {
                    int line2Y = bounds.Top + ScalePixels(32, scale);
                    string codeStr = string.Format("Mã tra cứu: {0}", maVeCode);
                    Rectangle codeRect = new Rectangle(textLeft, line2Y, availableTextWidth, codeFont.Height + ScalePixels(2, scale));
                    TextRenderer.DrawText(g, codeStr, codeFont, codeRect, Color.FromArgb(71, 85, 105), TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
                }
            }
        }
        private async void FrmQuanLyDonVe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                await LoadDanhSachDonVeAsync();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                if (dgvChiTietVe.Focused && dgvChiTietVe.CurrentRow != null && btnCopyMaVe != null)
                {
                    btnCopyMaVe_Click(btnCopyMaVe, EventArgs.Empty);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
            else if (e.Control && e.KeyCode == Keys.P)
            {
                if (btnInVe != null && btnInVe.Enabled)
                {
                    btnInVe.PerformClick();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
            else if (e.Control && e.KeyCode == Keys.F)
            {
                txtTimKiem.Focus();
                txtTimKiem.SelectAll();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape && txtTimKiem.Focused)
            {
                txtTimKiem.Clear();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void dgvDonVe_Paint(object sender, PaintEventArgs e)
        {
            if (dgvDonVe.Rows.Count == 0 && !loading)
            {
                VeEmptyState(dgvDonVe, e.Graphics,
                    "Không tìm thấy đơn vé nào",
                    "Thử tra cứu mã đơn (#...), tên khách, SĐT hoặc chọn lại bộ lọc trạng thái");
            }
        }

        private void dgvChiTietVe_Paint(object sender, PaintEventArgs e)
        {
            if (dgvChiTietVe.Rows.Count == 0 && !loadingDetail)
            {
                VeEmptyState(dgvChiTietVe, e.Graphics,
                    "Không có vé nào trong đơn này",
                    "Chọn một đơn vé có vé hợp lệ trong danh sách để xem chi tiết");
            }
        }

        private static void VeEmptyState(DataGridView dgv, Graphics g, string mainText, string subText)
        {
            using (Font fontMain = new Font("Segoe UI", 10.5f, FontStyle.Bold))
            using (Font fontSub = new Font("Segoe UI", 9f, FontStyle.Regular))
            using (Brush brushMain = new SolidBrush(Color.FromArgb(100, 116, 139)))
            using (Brush brushSub = new SolidBrush(Color.FromArgb(148, 163, 184)))
            using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                int topOffset = dgv.ColumnHeadersVisible ? dgv.ColumnHeadersHeight : 0;
                Rectangle area = new Rectangle(0, topOffset, dgv.ClientSize.Width, Math.Max(0, dgv.ClientSize.Height - topOffset));
                if (area.Height < 40) return;

                Rectangle rectMain = new Rectangle(area.Left, area.Top + area.Height / 2 - 20, area.Width, 22);
                Rectangle rectSub = new Rectangle(area.Left, area.Top + area.Height / 2 + 4, area.Width, 20);

                g.DrawString(mainText, fontMain, brushMain, rectMain, sf);
                g.DrawString(subText, fontSub, brushSub, rectSub, sf);
            }
        }
        #endregion
    }
}