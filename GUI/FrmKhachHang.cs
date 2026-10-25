using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmKhachHang : Form
    {
        private int requestVersion;
        private bool loading;
        private bool deleting;

        public FrmKhachHang()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ManagementFormLayout.Attach(pnlHeader, txtTimKiem, btnThem, btnSua, btnXoa);
        }

        private async void FrmKhachHang_Load(object sender, EventArgs e)
        {
            // FrmMain supplies the page title when hosting this form in pnlContent.
            pnlTitle.Visible = TopLevel;
            if (!TopLevel)
            {
                FormBorderStyle = FormBorderStyle.None;
                MinimumSize = Size.Empty;
            }
            await LoadDanhSachAsync();
        }

        public async void LoadDanhSachKhachHang(int selectMaKH = 0)
        {
            await LoadDanhSachAsync(selectMaKH);
        }

        private async Task LoadDanhSachAsync(int selectMaKH = 0)
        {
            searchTimer.Stop();
            int version = ++requestVersion;
            string keyword = txtTimKiem.Text.Trim();
            loading = true;
            ShowListMessage("Đang tải khách hàng…", "Vui lòng chờ trong giây lát.");
            lblCount.Text = "Đang tải dữ liệu…";
            UpdateActions();
            try
            {
                DataTable data = await Task.Run(() => KhachHangBLL.TimKiemTheoSDT(keyword));
                if (IsDisposed || version != requestVersion) return;
                dgvKhachHang.DataSource = data;
                lblCount.Text = string.Format(keyword.Length == 0
                    ? "Tổng: {0:N0} khách hàng" : "Kết quả: {0:N0} khách hàng", data.Rows.Count);
                if (data.Rows.Count == 0)
                    ShowListMessage(keyword.Length == 0 ? "Chưa có khách hàng" : "Không tìm thấy khách hàng",
                        keyword.Length == 0 ? "Chọn “Thêm mới” để tạo hồ sơ khách hàng đầu tiên."
                        : "Thử số điện thoại khác hoặc xóa từ khóa để xem tất cả.");
                else
                {
                    pnlMessage.Visible = false;
                    int index = 0;
                    for (int i = 0; i < dgvKhachHang.Rows.Count; i++)
                        if (Convert.ToInt32(dgvKhachHang.Rows[i].Cells["colMaKH"].Value) == selectMaKH)
                        {
                            index = i;
                            break;
                        }
                    dgvKhachHang.CurrentCell = dgvKhachHang.Rows[index].Cells["colTenKH"];
                    dgvKhachHang.ClearSelection();
                    dgvKhachHang.Rows[index].Selected = true;
                }
            }
            catch (Exception)
            {
                if (IsDisposed || version != requestVersion) return;
                dgvKhachHang.DataSource = null;
                lblCount.Text = "Không thể tải dữ liệu";
                ShowListMessage("Không thể tải danh sách khách hàng",
                    "Kiểm tra kết nối cơ sở dữ liệu, sau đó chọn “Làm mới” để thử lại.");
            }
            finally
            {
                if (!IsDisposed && version == requestVersion)
                {
                    loading = false;
                    UpdateActions();
                }
            }
        }

        private void ShowListMessage(string title, string description)
        {
            lblMessageTitle.Text = title;
            lblMessageDescription.Text = description;
            pnlMessage.Visible = true;
            pnlMessage.BringToFront();
        }

        private void UpdateActions()
        {
            bool hasSelection = !loading && !deleting && dgvKhachHang.CurrentRow != null;
            btnThem.Enabled = !loading && !deleting;
            btnSua.Enabled = hasSelection;
            btnXoa.Enabled = hasSelection;
            btnLamMoi.Enabled = !deleting;
            txtTimKiem.Enabled = !deleting;
            dgvKhachHang.Enabled = !loading && !deleting;
        }

        private KhachHangDTO SelectedCustomer()
        {
            var row = dgvKhachHang.CurrentRow;
            if (row == null || loading || deleting) return null;
            return new KhachHangDTO(Convert.ToInt32(row.Cells["colMaKH"].Value),
                Convert.ToString(row.Cells["colTenKH"].Value),
                Convert.ToString(row.Cells["colSDT"].Value),
                Convert.ToString(row.Cells["colEmail"].Value));
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            // Invalidate older results immediately, before the debounce expires.
            ++requestVersion;
            loading = true;
            UpdateActions();
            searchTimer.Stop();
            searchTimer.Start();
        }

        private async void searchTimer_Tick(object sender, EventArgs e) { await LoadDanhSachAsync(); }
        private async void btnLamMoi_Click(object sender, EventArgs e) { await LoadDanhSachAsync(); }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            if (loading || deleting) return;
            using (var dialog = new FrmChiTietKhachHang())
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                txtTimKiem.Clear();
                await LoadDanhSachAsync(dialog.MaKHDaLuu);
            }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            KhachHangDTO customer = SelectedCustomer();
            if (customer == null) return;
            using (var dialog = new FrmChiTietKhachHang(customer))
                if (dialog.ShowDialog(this) == DialogResult.OK)
                    await LoadDanhSachAsync(dialog.MaKHDaLuu);
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            KhachHangDTO customer = SelectedCustomer();
            if (customer == null) return;
            if (MessageBox.Show(this, string.Format("Xóa khách hàng “{0}” (SĐT: {1})?\nKhách hàng đã có lịch sử đặt vé sẽ được giữ lại.",
                customer.TenKH, customer.SDT), "Xác nhận xóa khách hàng", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            deleting = true;
            UpdateActions();
            try
            {
                KhachHangResult result = await Task.Run(() => KhachHangBLL.Xoa(customer.MaKH));
                if (IsDisposed) return;
                if (result.Success) await LoadDanhSachAsync();
                else MessageBox.Show(this, result.Message, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception)
            {
                if (!IsDisposed) MessageBox.Show(this, "Không thể xóa khách hàng. Vui lòng kiểm tra kết nối và thử lại.",
                    "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                deleting = false;
                if (!IsDisposed) UpdateActions();
            }
        }

        private void dgvKhachHang_SelectionChanged(object sender, EventArgs e) { UpdateActions(); }

        private void dgvKhachHang_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvKhachHang.Columns[e.ColumnIndex] == colThaoTac)
                ManagementGridRenderer.PaintActions(dgvKhachHang, e);
        }

        private void dgvKhachHang_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.Button != MouseButtons.Left ||
                dgvKhachHang.Columns[e.ColumnIndex] != colThaoTac || loading || deleting) return;
            dgvKhachHang.CurrentCell = dgvKhachHang.Rows[e.RowIndex].Cells[e.ColumnIndex];
            Rectangle cell = new Rectangle(Point.Empty, dgvKhachHang.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Size);
            if (ManagementGridRenderer.GetActionBounds(dgvKhachHang, cell, false).Contains(e.X, e.Y))
                btnSua.PerformClick();
            else if (ManagementGridRenderer.GetActionBounds(dgvKhachHang, cell, true).Contains(e.X, e.Y))
                btnXoa.PerformClick();
        }

        private void dgvKhachHang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvKhachHang.Columns[e.ColumnIndex] != colThaoTac)
                btnSua.PerformClick();
        }

        private void dgvKhachHang_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvKhachHang.Columns[e.ColumnIndex] == colThaoTac)
                e.ToolTipText = "Sửa khách hàng (bên trái) · Xóa khách hàng (bên phải)";
        }

        private void FrmKhachHang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.N) btnThem.PerformClick();
            else if (e.Control && e.KeyCode == Keys.E) btnSua.PerformClick();
            else if (e.Control && e.KeyCode == Keys.F) { txtTimKiem.Focus(); txtTimKiem.SelectAll(); }
            else if (e.KeyCode == Keys.F5) btnLamMoi.PerformClick();
            else if (e.KeyCode == Keys.Escape && txtTimKiem.Focused) txtTimKiem.Clear();
            else return;
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (deleting) e.Cancel = true;
            base.OnFormClosing(e);
        }
    }
}
