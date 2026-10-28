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

        public FrmQuanLyDonVe()
        {
            InitializeComponent();
            DoubleBuffered = true;
            dgvDonVe.AutoGenerateColumns = false;
            dgvChiTietVe.AutoGenerateColumns = false;
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
            DateTime thoiGianBatDau = Convert.ToDateTime(row.Cells["colThoiGianBatDau"].Value);
            double gioConLai = (thoiGianBatDau - DateTime.Now).TotalHours;

            lblDetailTitle.Text = string.Format("CHI TIẾT VÉ TRONG ĐƠN: #{0} ({1})", maDon, trangThai);

            // Cập nhật banner trạng thái quy định
            if (trangThai == TrangThaiDonVe.DaHuy)
            {
                lblRuleBadge.ForeColor = Color.FromArgb(239, 68, 68);
                lblRuleBadge.Text = "❌ Đơn vé này đã bị hủy toàn bộ.";
            }
            else if (trangThai == TrangThaiDonVe.DaDoi)
            {
                lblRuleBadge.ForeColor = Color.FromArgb(37, 99, 235);
                lblRuleBadge.Text = "🔄 Đơn vé này đã được đổi sang đơn mới.";
            }
            else
            {
                if (gioConLai <= 0)
                {
                    lblRuleBadge.ForeColor = Color.FromArgb(100, 116, 139);
                    lblRuleBadge.Text = string.Format("⏰ Suất diễn đã diễn ra lúc {0:dd/MM/yyyy HH:mm}. Không thể hủy hoặc đổi vé.", thoiGianBatDau);
                }
                else if (gioConLai >= 24.0)
                {
                    lblRuleBadge.ForeColor = Color.FromArgb(16, 185, 129);
                    lblRuleBadge.Text = string.Format("⏰ Giờ diễn: {0:dd/MM/yyyy HH:mm} (còn {1:F1}h) — ĐỦ điều kiện Hủy (≥24h) và Đổi (≥12h).", thoiGianBatDau, gioConLai);
                }
                else if (gioConLai >= 12.0)
                {
                    lblRuleBadge.ForeColor = Color.FromArgb(217, 119, 6);
                    lblRuleBadge.Text = string.Format("⏰ Giờ diễn: {0:dd/MM/yyyy HH:mm} (còn {1:F1}h) — CHẶN hủy (<24h), ĐỦ điều kiện Đổi vé (≥12h).", thoiGianBatDau, gioConLai);
                }
                else
                {
                    lblRuleBadge.ForeColor = Color.FromArgb(239, 68, 68);
                    lblRuleBadge.Text = string.Format("⏰ Giờ diễn: {0:dd/MM/yyyy HH:mm} (còn {1:F1}h) — CHẶN cả Hủy và Đổi (<12h).", thoiGianBatDau, gioConLai);
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
            bool coDonChon = !loading && !loadingDetail && row != null && row.Index >= 0;
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
            var row = LayDongDonVeDangChon();
            if (row == null || row.Index < 0) return;

            int maDon = Convert.ToInt32(row.Cells["colMaDon"].Value);
            string tenKH = row.Cells["colTenKH"].Value?.ToString() ?? "";
            string sdt = row.Cells["colSDT"].Value?.ToString() ?? "";
            decimal tongTien = Convert.ToDecimal(row.Cells["colTongTien"].Value);

            DonVeResult check = DonVeBLL.KiemTraDieuKienHuy(maDon);
            if (!check.Success)
            {
                MessageBox.Show(this, check.Message, "Không đủ điều kiện hủy vé", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string loiNhan = string.Format(
                "Xác nhận hủy đơn vé #{0}?\n\n• Khách hàng: {1} (SĐT: {2})\n• Tổng tiền: {3:N0} VNĐ\n• Quy tắc: Toàn bộ ghế trong đơn sẽ được trả về trạng thái Trống.",
                maDon, tenKH, sdt, tongTien);

            if (MessageBox.Show(this, loiNhan, "Xác nhận hủy đơn vé", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                UseWaitCursor = true;
                btnHuyVe.Enabled = false;

                DonVeResult result = await Task.Run(() => DonVeBLL.HuyVe(maDon));
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
                MessageBox.Show(this, "Lỗi trong quá trình hủy đơn vé: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UseWaitCursor = false;
                CapNhatTrangThaiNutThaoTac();
            }
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
                    e.CellStyle.ForeColor = Color.FromArgb(37, 99, 235);
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
