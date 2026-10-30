<<<<<<< HEAD
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
        private int requestVersion;
        private int detailRequestVersion;
        private bool loading;
        private bool loadingDetail;
        private DataTable dtDonVeGoc;
        private bool updatingLayout;
        private bool cancellingOrder;
        private static double preferredMasterRatio = 0.70;

        public FrmQuanLyDonVe()
        {
            InitializeComponent();
            DoubleBuffered = true;
            dgvDonVe.AutoGenerateColumns = false;
            dgvChiTietVe.AutoGenerateColumns = false;

            pnlHeader.SizeChanged += PnlHeader_SizeChanged;
            ClientSizeChanged += PnlHeader_SizeChanged;
            Shown += PnlHeader_SizeChanged;
            splitContainer.SizeChanged += PnlHeader_SizeChanged;
            pnlMasterHeader.SizeChanged += PnlHeader_SizeChanged;
            lblDonVeTomTat.TextChanged += PnlHeader_SizeChanged;
            lblDetailTitle.TextChanged += PnlHeader_SizeChanged;
            lblRuleBadge.TextChanged += PnlHeader_SizeChanged;
            splitContainer.SplitterMoved += (sender, args) =>
            {
                int available = splitContainer.Height - splitContainer.SplitterWidth;
                if (!updatingLayout && available > 0)
                    preferredMasterRatio = (double)splitContainer.SplitterDistance / available;
            };
            AutoScroll = true;
            txtTimKiem.AccessibleName = "Tìm đơn vé";
            cboLocTrangThai.AccessibleName = "Lọc trạng thái đơn vé";
            PnlHeader_SizeChanged(this, EventArgs.Empty);
        }

        private void PnlHeader_SizeChanged(object sender, EventArgs e)
        {
            ApplyResponsiveLayout(DeviceDpi / 96f);
        }

        private static int ScalePixels(int pixels, float scale)
        {
            return (int)Math.Round(pixels * scale);
        }

        private static int WrappedHeight(Label label, int width)
        {
            return TextRenderer.MeasureText(label.Text, label.Font,
                new Size(Math.Max(1, width), int.MaxValue),
                TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl |
                TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix).Height;
        }

        private void ApplyResponsiveLayout(float scale)
        {
            if (updatingLayout || IsDisposed) return;
            updatingLayout = true;
            try
            {
                // A vertical scrollbar can change the available width; settle both layouts.
                for (int pass = 0; pass < 2; pass++)
                {
                    LayoutToolbar(scale);
                    int padding = ScalePixels(14, scale);
                    int gap = ScalePixels(8, scale);
                    int summaryWidth = Math.Max(1, pnlMasterHeader.ClientSize.Width - padding * 2);
                    lblMasterTitle.Location = new Point(padding, gap);
                    lblDonVeTomTat.SetBounds(padding, lblMasterTitle.Bottom + gap,
                        summaryWidth, WrappedHeight(lblDonVeTomTat, summaryWidth));
                    pnlMasterHeader.Height = lblDonVeTomTat.Bottom + gap;

                    int detailWidth = pnlDetailHeader.ClientSize.Width;
                    int actionWidth = ScalePixels(146, scale);
                    int actionHeight = ScalePixels(36, scale);
                    int titleWidth = Math.Max(1, detailWidth - padding * 2 - actionWidth - gap);
                    bool stacked = TextRenderer.MeasureText(lblDetailTitle.Text,
                        lblDetailTitle.Font).Width > titleWidth;
                    if (stacked) titleWidth = Math.Max(1, detailWidth - padding * 2);
                    lblDetailTitle.SetBounds(padding, gap, titleWidth,
                        WrappedHeight(lblDetailTitle, titleWidth));
                    btnDoiVe.SetBounds(Math.Max(0, detailWidth - padding - actionWidth),
                        stacked ? lblDetailTitle.Bottom + gap : gap, actionWidth, actionHeight);
                    pnlDetailHeader.Height = Math.Max(lblDetailTitle.Bottom, btnDoiVe.Bottom) + gap;

                    int ruleWidth = Math.Max(1, pnlRuleBanner.ClientSize.Width - padding * 2);
                    lblRuleBadge.Padding = new Padding(padding, gap, padding, gap);
                    pnlRuleBanner.Height = WrappedHeight(lblRuleBadge, ruleWidth) + gap * 2;
                    lblSearchHint.Height = WrappedHeight(lblSearchHint,
                        Math.Max(1, lblSearchHint.Width)) + gap * 2;

                    int scrollHeight = SystemInformation.HorizontalScrollBarHeight;
                    int masterMinimum = pnlMasterHeader.Height + dgvDonVe.ColumnHeadersHeight +
                        dgvDonVe.RowTemplate.Height * 2 + scrollHeight + ScalePixels(4, scale);
                    int detailMinimum = pnlDetailHeader.Height + pnlRuleBanner.Height +
                        dgvChiTietVe.ColumnHeadersHeight + dgvChiTietVe.RowTemplate.Height +
                        scrollHeight + ScalePixels(4, scale);

                    // Keep full rows visible, including when long text needs extra lines.
                    splitContainer.Panel1MinSize = 0;
                    splitContainer.Panel2MinSize = 0;
                    int minimumContentHeight =
                        (pnlTitle.Visible ? pnlTitle.Height : 0) + pnlHeader.Height +
                        pnlFooter.Height + lblSearchHint.Height + masterMinimum +
                        detailMinimum + splitContainer.SplitterWidth;
                    pnlPageContent.Height = Math.Max(minimumContentHeight,
                        ClientSize.Height - Padding.Vertical);
                    PerformLayout();
                    pnlPageContent.PerformLayout();
                    int available = splitContainer.Height - splitContainer.SplitterWidth;
                    if (available >= masterMinimum + detailMinimum)
                    {
                        int desired = (int)Math.Round(available * preferredMasterRatio);
                        splitContainer.SplitterDistance = Math.Max(masterMinimum,
                            Math.Min(desired, available - detailMinimum));
                        splitContainer.Panel1MinSize = masterMinimum;
                        splitContainer.Panel2MinSize = detailMinimum;
                    }
                }
            }
            finally
            {
                updatingLayout = false;
            }
        }

        private void LayoutToolbar(float scale)
        {
            int width = pnlHeader.ClientSize.Width;
            int gap = ScalePixels(12, scale);
            int top = ScalePixels(8, scale);
            int labelHeight = Math.Max(lblTimKiem.PreferredHeight, lblLocTrangThai.PreferredHeight);
            int fieldTop = top + labelHeight + ScalePixels(6, scale);
            int height = Math.Max(ScalePixels(36, scale), txtTimKiem.Font.Height + gap);
            int actionWidth = ScalePixels(150, scale);
            int filterWidth = ScalePixels(180, scale);
            int searchMinimum = ScalePixels(220, scale);
            bool oneRow = width >= searchMinimum + filterWidth + actionWidth + gap * 2;
            bool fieldsTogether = width >= searchMinimum + filterWidth + gap;
            int searchWidth = oneRow
                ? Math.Min(ScalePixels(350, scale), width - filterWidth - actionWidth - gap * 2)
                : fieldsTogether ? width - filterWidth - gap : width;
            txtTimKiem.SetBounds(0, fieldTop, Math.Max(1, searchWidth), height);
            lblTimKiem.SetBounds(0, top, txtTimKiem.Width, labelHeight);
            int filterX = fieldsTogether ? txtTimKiem.Right + gap : 0;
            int filterTop = fieldsTogether ? fieldTop : txtTimKiem.Bottom + gap + labelHeight + ScalePixels(6, scale);
            cboLocTrangThai.ItemHeight = Math.Max(1, height - 6);
            cboLocTrangThai.SetBounds(filterX, filterTop,
                Math.Min(filterWidth, Math.Max(1, width)), height);
            lblLocTrangThai.SetBounds(filterX, filterTop - labelHeight - ScalePixels(6, scale),
                cboLocTrangThai.Width, labelHeight);
            int actionTop = oneRow ? fieldTop : Math.Max(txtTimKiem.Bottom, cboLocTrangThai.Bottom) + gap;
            btnHuyVe.SetBounds(Math.Max(0, width - actionWidth), actionTop,
                Math.Min(actionWidth, Math.Max(1, width)), height);
            pnlHeader.Height = Math.Max(cboLocTrangThai.Bottom, btnHuyVe.Bottom) + gap;
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

        private void KhoiTaoLocTrangThai()
        {
            cboLocTrangThai.Items.Clear();
            cboLocTrangThai.Items.Add("— Tất cả trạng thái —");
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
            lblCount.Text = "Đang tải dữ liệu đơn vé…";
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
                lblCount.Text = "Không thể tải dữ liệu đơn vé: " + ex.Message;
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

            // Xóa ngay danh sách chi tiết vé cũ để tránh chọn nhầm vé của đơn trước
            dgvChiTietVe.DataSource = null;
            dgvDonVe.DataSource = dtDisplay;

            // Định dạng cột
            if (dgvDonVe.Columns["colTongTien"] != null)
            {
                dgvDonVe.Columns["colTongTien"].DefaultCellStyle.Format = "N0";
                dgvDonVe.Columns["colTongTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDonVe.Columns["colTongTien"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dgvDonVe.Columns["colThoiGianBatDau"] != null)
            {
                dgvDonVe.Columns["colThoiGianBatDau"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }
            if (dgvDonVe.Columns["colNgayDat"] != null)
            {
                dgvDonVe.Columns["colNgayDat"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }

            lblCount.Text = string.Format("Tổng số: {0:N0} đơn vé", dtDisplay.Rows.Count);

            if (dtDisplay.Rows.Count > 0)
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

                dgvDonVe.ClearSelection();
                dgvDonVe.Rows[index].Selected = true;
                dgvDonVe.CurrentCell = dgvDonVe.Rows[index].Cells["colTenKH"];
            }
            else
            {
                dgvChiTietVe.DataSource = null;
                lblDetailTitle.Text = "CHI TIẾT VÉ TRONG ĐƠN: (Trống)";
                lblDonVeTomTat.Text = "Không có đơn vé nào phù hợp";
                lblRuleBadge.Text = "Không có đơn vé nào phù hợp điều kiện tìm kiếm.";
                lblRuleBadge.ForeColor = Color.FromArgb(100, 116, 139);
            }

            CapNhatTrangThaiNutThaoTac();
        }

        private DataGridViewRow LayDongDonVeDangChon()
        {
            if (dgvDonVe.SelectedRows.Count > 0)
            {
                return dgvDonVe.SelectedRows[0];
            }
            if (dgvDonVe.CurrentRow != null && dgvDonVe.CurrentRow.Selected)
            {
                return dgvDonVe.CurrentRow;
            }
            return dgvDonVe.CurrentRow;
        }

        private async void dgvDonVe_SelectionChanged(object sender, EventArgs e)
        {
            if (loading) return;
            await LoadChiTietVeAsync();
        }

        private async void dgvDonVe_CurrentCellChanged(object sender, EventArgs e)
        {
            if (loading) return;
            await LoadChiTietVeAsync();
        }

        private async Task LoadChiTietVeAsync()
        {
            var row = LayDongDonVeDangChon();
            if (row == null || row.Index < 0 || loading)
            {
                detailRequestVersion++;
                dgvChiTietVe.DataSource = null;
                lblDetailTitle.Text = "CHI TIẾT VÉ TRONG ĐƠN: (Trống)";
                lblDonVeTomTat.Text = "Chưa chọn đơn vé nào";
                lblRuleBadge.Text = "Vui lòng chọn một đơn vé trong danh sách để xem chi tiết.";
                lblRuleBadge.ForeColor = Color.FromArgb(100, 116, 139);
                CapNhatTrangThaiNutThaoTac();
                return;
            }

            int version = ++detailRequestVersion;
            loadingDetail = true;
            dgvChiTietVe.DataSource = null;
            CapNhatTrangThaiNutThaoTac();

            int maDon = Convert.ToInt32(row.Cells["colMaDon"].Value);
            string trangThai = row.Cells["colTrangThai"].Value?.ToString() ?? "";
            string tenKH = row.Cells["colTenKH"].Value?.ToString() ?? "";
            string sdt = row.Cells["colSDT"].Value?.ToString() ?? "";
            string tenSK = row.Cells["colTenSK"].Value?.ToString() ?? "";
            DateTime thoiGianBatDau = Convert.ToDateTime(row.Cells["colThoiGianBatDau"].Value);
            TimeSpan timeSpan = thoiGianBatDau - DateTime.Now;

            string trangThaiVN = trangThai == TrangThaiDonVe.DaThanhToan ? "Đã thanh toán"
                : (trangThai == TrangThaiDonVe.DaHuy ? "Đã hủy"
                : (trangThai == TrangThaiDonVe.DaDoi ? "Đã đổi" : trangThai));

            lblDetailTitle.Text = string.Format("CHI TIẾT VÉ TRONG ĐƠN: #{0} ({1})", maDon, trangThaiVN);
            lblDonVeTomTat.Text = string.Format("Khách hàng: {0} ({1})\nSự kiện: {2}", tenKH, sdt, tenSK);

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
                    lblRuleBadge.Text = string.Format("Suất diễn đã diễn ra lúc {0:dd/MM/yyyy HH:mm} · Không thể đổi vé · Không thể hủy đơn", thoiGianBatDau);
                }
                else
                {
                    string thoiGianStr = timeSpan.Days > 0
                        ? string.Format("Còn {0} ngày {1} giờ {2} phút", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes)
                        : string.Format("Còn {0} giờ {1} phút", (int)Math.Floor(timeSpan.TotalHours), timeSpan.Minutes);

                    if (timeSpan.TotalHours >= 24.0)
                    {
                        lblRuleBadge.ForeColor = Color.FromArgb(22, 101, 52);
                        lblRuleBadge.Text = string.Format("{0} · Có thể hủy đơn · Có thể đổi vé", thoiGianStr);
                    }
                    else if (timeSpan.TotalHours >= 12.0)
                    {
                        lblRuleBadge.ForeColor = Color.FromArgb(146, 64, 14);
                        lblRuleBadge.Text = string.Format("{0} · Có thể đổi vé · Không thể hủy đơn", thoiGianStr);
                    }
                    else
                    {
                        lblRuleBadge.ForeColor = Color.FromArgb(185, 28, 28);
                        lblRuleBadge.Text = string.Format("{0} · Không thể đổi vé · Không thể hủy đơn", thoiGianStr);
                    }
                }
            }

            try
            {
                DataTable dtCT = await Task.Run(() => DonVeBLL.LayChiTietVeTheoDon(maDon));
                if (IsDisposed || version != detailRequestVersion) return;

                // Kiểm tra xem dòng đang chọn hiện tại có còn là maDon này không
                var currentRow = LayDongDonVeDangChon();
                if (currentRow == null || currentRow.Index < 0 || Convert.ToInt32(currentRow.Cells["colMaDon"].Value) != maDon)
                {
                    // Dòng hiện tại đã đổi trong khi đang tải -> Kích hoạt tải đơn mới ngay
                    _ = LoadChiTietVeAsync();
                    return;
                }

                dgvChiTietVe.DataSource = dtCT;
                if (dgvChiTietVe.Columns["colGiaVe"] != null)
                {
                    dgvChiTietVe.Columns["colGiaVe"].DefaultCellStyle.Format = "N0";
                    dgvChiTietVe.Columns["colGiaVe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvChiTietVe.Columns["colGiaVe"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                }

                if (dtCT.Rows.Count > 0)
                {
                    dgvChiTietVe.ClearSelection();
                    dgvChiTietVe.Rows[0].Selected = true;
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

        private void CapNhatTrangThaiNutThaoTac()
        {
            var row = LayDongDonVeDangChon();
            bool coDonChon = !loading && !loadingDetail && !cancellingOrder && row != null && row.Index >= 0;
            if (!coDonChon)
            {
                btnHuyVe.Enabled = false;
                btnDoiVe.Enabled = false;
                return;
            }

            int maDon = Convert.ToInt32(row.Cells["colMaDon"].Value);
            string trangThai = row.Cells["colTrangThai"].Value?.ToString() ?? "";
            DateTime thoiGianBatDau = Convert.ToDateTime(row.Cells["colThoiGianBatDau"].Value);
            double gioConLai = (thoiGianBatDau - DateTime.Now).TotalHours;

            bool hopLeHuy = (trangThai == TrangThaiDonVe.DaThanhToan && gioConLai >= 24.0);

            bool hopLeDoi = false;
            if (trangThai == TrangThaiDonVe.DaThanhToan && gioConLai >= 12.0 && dgvChiTietVe.CurrentRow != null && dgvChiTietVe.CurrentRow.Index >= 0)
            {
                var drv = dgvChiTietVe.CurrentRow.DataBoundItem as DataRowView;
                if (drv != null && Convert.ToInt32(drv["MaDon"]) == maDon)
                {
                    string trangThaiVe = drv["TrangThai"]?.ToString() ?? "";
                    if (trangThaiVe == TrangThaiChiTietVe.HopLe)
                    {
                        hopLeDoi = true;
                    }
                }
            }

            btnHuyVe.Enabled = hopLeHuy;
            btnDoiVe.Enabled = hopLeDoi;
        }

        private void dgvChiTietVe_SelectionChanged(object sender, EventArgs e)
        {
            CapNhatTrangThaiNutThaoTac();
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

            if (dgvChiTietVe.CurrentRow == null || dgvChiTietVe.CurrentRow.Index < 0)
            {
                MessageBox.Show(this, "Vui lòng chọn 1 vé trong bảng Chi tiết vé bên dưới để thực hiện đổi vé!", "Chưa chọn vé cần đổi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            string colName = dgvDonVe.Columns[e.ColumnIndex].Name;

            if (colName == "colTrangThai")
            {
                string status = e.Value?.ToString() ?? "";
                Color bg = Color.FromArgb(241, 245, 249);
                Color fg = Color.FromArgb(71, 85, 105);
                string text = status;

                if (status == TrangThaiDonVe.DaThanhToan)
                {
                    bg = Color.FromArgb(209, 250, 229);
                    fg = Color.FromArgb(6, 95, 70);
                    text = "Đã thanh toán";
                }
                else if (status == TrangThaiDonVe.DaHuy)
                {
                    bg = Color.FromArgb(254, 226, 226);
                    fg = Color.FromArgb(153, 27, 27);
                    text = "Đã hủy";
                }
                else if (status == TrangThaiDonVe.DaDoi)
                {
                    bg = Color.FromArgb(219, 234, 254);
                    fg = Color.FromArgb(30, 64, 175);
                    text = "Đã đổi";
                }

                ManagementGridRenderer.PaintStatus(dgvDonVe, e, text, bg, fg);
            }
        }

        private void dgvDonVe_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            string colName = dgvDonVe.Columns[e.ColumnIndex].Name;

            if (colName == "colMaDon")
            {
                if (e.Value != null && e.Value != DBNull.Value)
                {
                    e.Value = "#" + e.Value.ToString();
                    e.FormattingApplied = true;
                }
            }
            else if (colName == "colMaDonGoc")
            {
                if (e.Value == null || e.Value == DBNull.Value || string.IsNullOrEmpty(e.Value.ToString()))
                {
                    e.Value = "—";
                    e.CellStyle.ForeColor = Color.FromArgb(156, 163, 175);
                    e.FormattingApplied = true;
                }
                else
                {
                    e.Value = "#" + e.Value.ToString();
                    e.CellStyle.ForeColor = Color.FromArgb(71, 85, 105);
                    e.FormattingApplied = true;
                }
            }
            else if (colName == "colTrangThai")
            {
                if (e.Value != null)
                {
                    string s = e.Value.ToString();
                    if (s == TrangThaiDonVe.DaThanhToan) e.Value = "Đã thanh toán";
                    else if (s == TrangThaiDonVe.DaHuy) e.Value = "Đã hủy";
                    else if (s == TrangThaiDonVe.DaDoi) e.Value = "Đã đổi";
                    e.FormattingApplied = true;
                }
            }
        }

        private void dgvChiTietVe_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            string colName = dgvChiTietVe.Columns[e.ColumnIndex].Name;

            if (colName == "colTrangThaiVe")
            {
                string status = e.Value?.ToString() ?? "";
                Color bg = Color.FromArgb(241, 245, 249);
                Color fg = Color.FromArgb(71, 85, 105);
                string text = status;

                if (status == TrangThaiChiTietVe.HopLe)
                {
                    bg = Color.FromArgb(209, 250, 229);
                    fg = Color.FromArgb(6, 95, 70);
                    text = "Hợp lệ";
                }
                else if (status == TrangThaiChiTietVe.DaHuy)
                {
                    bg = Color.FromArgb(254, 226, 226);
                    fg = Color.FromArgb(153, 27, 27);
                    text = "Đã hủy";
                }

                ManagementGridRenderer.PaintStatus(dgvChiTietVe, e, text, bg, fg);
            }
        }

        private void dgvChiTietVe_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            string colName = dgvChiTietVe.Columns[e.ColumnIndex].Name;

            if (colName == "colMaCT" && e.Value != null && e.Value != DBNull.Value)
            {
                e.Value = "#" + e.Value.ToString();
                e.FormattingApplied = true;
            }
            else if (colName == "colTrangThaiVe")
            {
                if (e.Value != null)
                {
                    string s = e.Value.ToString();
                    if (s == TrangThaiChiTietVe.HopLe) e.Value = "Hợp lệ";
                    else if (s == TrangThaiChiTietVe.DaHuy) e.Value = "Đã hủy";
                    e.FormattingApplied = true;
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
    }
}
=======
﻿using System.Windows.Forms;

namespace QuanLySuKienVaBanVe.GUI
{
    internal class FrmQuanLyDonVe : Form
    {
    }
}
>>>>>>> feature/area-management
