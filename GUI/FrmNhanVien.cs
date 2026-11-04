using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmNhanVien : Form
    {
        private int requestVersion;
        private bool loading;
        private bool busy;

        public FrmNhanVien()
        {
            InitializeComponent();
            HieuUng.GanChoForm(this);
            DoubleBuffered = true;
            foreach (Control control in new Control[] { txtTimKiem, btnThem, btnSua, btnKhoa, btnXoa })
                control.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            pnlHeader.SizeChanged += delegate { LayoutHeader(); };
            LayoutHeader();
        }

        private void LayoutHeader()
        {
            float scale = pnlHeader.DeviceDpi / 96f;
            Func<int, int> px = value => (int)Math.Round(value * scale);
            Control[] actions = { btnThem, btnSua, btnKhoa, btnXoa };
            int gap = px(10);
            int actionsWidth = actions.Sum(control => control.Width) + gap * (actions.Length - 1);
            int availableWidth = pnlHeader.ClientSize.Width;
            bool stacked = availableWidth < actionsWidth + px(190);
            int headerHeight = stacked ? px(112) : px(72);
            if (pnlHeader.Height != headerHeight) pnlHeader.Height = headerHeight;

            txtTimKiem.Location = new Point(0, stacked ? px(12) : px(20));
            txtTimKiem.Width = Math.Max(px(120), Math.Min(px(350),
                availableWidth - (stacked ? 0 : actionsWidth + px(20))));

            int x = Math.Max(0, availableWidth - actionsWidth);
            int y = stacked ? px(66) : px(20);
            foreach (Control control in actions)
            {
                control.Location = new Point(x, y);
                x += control.Width + gap;
            }
        }

        private async void FrmNhanVien_Load(object sender, EventArgs e)
        {
            pnlTitle.Visible = TopLevel;
            if (!TopLevel)
            {
                FormBorderStyle = FormBorderStyle.None;
                MinimumSize = Size.Empty;
            }
            await LoadDanhSachAsync();
        }

        private async Task LoadDanhSachAsync(string selectTen = null)
        {
            searchTimer.Stop();
            int version = ++requestVersion;
            string keyword = txtTimKiem.Text.Trim();
            loading = true;
            ShowListMessage("Đang tải tài khoản…", "Vui lòng chờ trong giây lát.");
            lblCount.Text = "Đang tải dữ liệu…";
            UpdateActions();
            try
            {
                DataTable data = await Task.Run(() => TaiKhoanBLL.LayDanhSachTaiKhoan(keyword));
                if (IsDisposed || version != requestVersion) return;
                dgvNhanVien.DataSource = data;
                lblCount.Text = string.Format(keyword.Length == 0
                    ? "Tổng: {0:N0} tài khoản" : "Kết quả: {0:N0} tài khoản", data.Rows.Count);
                if (data.Rows.Count == 0)
                    ShowListMessage(keyword.Length == 0 ? "Chưa có tài khoản" : "Không tìm thấy tài khoản",
                        keyword.Length == 0 ? "Chọn “Thêm mới” để tạo tài khoản đầu tiên."
                        : "Thử tên khác hoặc xóa từ khóa để xem tất cả.");
                else
                {
                    pnlMessage.Visible = false;
                    int index = 0;
                    if (!string.IsNullOrEmpty(selectTen))
                        for (int i = 0; i < dgvNhanVien.Rows.Count; i++)
                            if (string.Equals(Convert.ToString(dgvNhanVien.Rows[i].Cells["colTenTK"].Value), selectTen,
                                StringComparison.OrdinalIgnoreCase))
                            {
                                index = i;
                                break;
                            }
                    dgvNhanVien.CurrentCell = dgvNhanVien.Rows[index].Cells["colTenTK"];
                    dgvNhanVien.ClearSelection();
                    dgvNhanVien.Rows[index].Selected = true;
                }
            }
            catch (Exception)
            {
                if (IsDisposed || version != requestVersion) return;
                dgvNhanVien.DataSource = null;
                lblCount.Text = "Không thể tải dữ liệu";
                ShowListMessage("Không thể tải danh sách tài khoản",
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

        private static bool IsLocked(DataGridViewRow row)
        {
            DataRowView view = row == null ? null : row.DataBoundItem as DataRowView;
            return view != null && !Convert.ToBoolean(view["TrangThai"]);
        }

        private void UpdateActions()
        {
            bool hasSelection = !loading && !busy && dgvNhanVien.CurrentRow != null;
            btnThem.Enabled = !loading && !busy;
            btnSua.Enabled = hasSelection;
            btnKhoa.Enabled = hasSelection;
            btnXoa.Enabled = hasSelection;
            btnKhoa.Text = IsLocked(dgvNhanVien.CurrentRow) ? "Mở khóa" : "Khóa";
            btnLamMoi.Enabled = !busy;
            txtTimKiem.Enabled = !busy;
            dgvNhanVien.Enabled = !loading && !busy;
        }

        private TaiKhoanDTO SelectedAccount()
        {
            var row = dgvNhanVien.CurrentRow;
            if (row == null || loading || busy) return null;
            DataRowView view = row.DataBoundItem as DataRowView;
            return new TaiKhoanDTO
            {
                MaTK = Convert.ToInt32(row.Cells["colMaTK"].Value),
                TenDangNhap = Convert.ToString(row.Cells["colTenTK"].Value),
                VaiTro = view == null ? VaiTro.NhanVien : Convert.ToString(view["VaiTro"]),
                TrangThai = !IsLocked(row)
            };
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
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
            if (loading || busy) return;
            using (var dialog = new FrmTaiKhoan())
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                txtTimKiem.Clear();
                await LoadDanhSachAsync(dialog.TenDaLuu);
            }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            TaiKhoanDTO account = SelectedAccount();
            if (account == null) return;
            using (var dialog = new FrmTaiKhoan(account))
                if (dialog.ShowDialog(this) == DialogResult.OK)
                    await LoadDanhSachAsync(dialog.TenDaLuu);
        }

        private async void btnKhoa_Click(object sender, EventArgs e)
        {
            TaiKhoanDTO account = SelectedAccount();
            if (account == null) return;
            bool target = !account.TrangThai;
            string message = target
                ? string.Format("Mở khóa tài khoản “{0}”?\nNgười dùng sẽ đăng nhập lại được.", account.TenDangNhap)
                : string.Format("Khóa tài khoản “{0}”?\nNgười dùng sẽ không đăng nhập được cho đến khi được mở khóa.", account.TenDangNhap);
            if (MessageBox.Show(this, message, target ? "Xác nhận mở khóa" : "Xác nhận khóa tài khoản",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            busy = true;
            UpdateActions();
            try
            {
                KetQuaTaiKhoan result = await Task.Run(() => TaiKhoanBLL.DatTrangThai(account.MaTK, target));
                if (IsDisposed) return;
                switch (result)
                {
                    case KetQuaTaiKhoan.ThanhCong:
                        await LoadDanhSachAsync(account.TenDangNhap);
                        break;
                    case KetQuaTaiKhoan.TuKhoaMinh:
                        MessageBox.Show(this, "Bạn không thể khóa tài khoản đang đăng nhập.", "Không thể khóa",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case KetQuaTaiKhoan.KhongCoQuyen:
                        MessageBox.Show(this, "Chỉ Quản lý mới được khóa hoặc mở khóa tài khoản.", "Từ chối truy cập",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    default:
                        MessageBox.Show(this, "Không thể cập nhật trạng thái tài khoản. Vui lòng kiểm tra kết nối và thử lại.",
                            "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            catch (Exception)
            {
                if (!IsDisposed) MessageBox.Show(this, "Không thể cập nhật trạng thái tài khoản. Vui lòng kiểm tra kết nối và thử lại.",
                    "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                busy = false;
                if (!IsDisposed) UpdateActions();
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            TaiKhoanDTO account = SelectedAccount();
            if (account == null) return;
            if (MessageBox.Show(this, string.Format("Xóa tài khoản “{0}”?\nTài khoản đã có đơn vé hoặc nhật ký sẽ không xóa được, hãy dùng chức năng Khóa.",
                account.TenDangNhap), "Xác nhận xóa tài khoản", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            busy = true;
            UpdateActions();
            try
            {
                KetQuaTaiKhoan result = await Task.Run(() => TaiKhoanBLL.Xoa(account.MaTK));
                if (IsDisposed) return;
                switch (result)
                {
                    case KetQuaTaiKhoan.ThanhCong:
                        await LoadDanhSachAsync();
                        break;
                    case KetQuaTaiKhoan.TuXoaMinh:
                        MessageBox.Show(this, "Bạn không thể xóa tài khoản đang đăng nhập.", "Không thể xóa",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case KetQuaTaiKhoan.DaCoDuLieu:
                        MessageBox.Show(this, "Tài khoản này đã phát sinh đơn vé hoặc nhật ký nên không thể xóa.\nHãy chọn “Khóa” để vô hiệu hóa tài khoản.", "Không thể xóa",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case KetQuaTaiKhoan.KhongCoQuyen:
                        MessageBox.Show(this, "Chỉ Quản lý mới được xóa tài khoản.", "Từ chối truy cập",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    default:
                        MessageBox.Show(this, "Không thể xóa tài khoản. Vui lòng kiểm tra kết nối và thử lại.", "Lỗi kết nối",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            catch (Exception)
            {
                if (!IsDisposed) MessageBox.Show(this, "Không thể xóa tài khoản. Vui lòng kiểm tra kết nối và thử lại.",
                    "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                busy = false;
                if (!IsDisposed) UpdateActions();
            }
        }

        private void dgvNhanVien_SelectionChanged(object sender, EventArgs e) { UpdateActions(); }

        private void dgvNhanVien_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            DataGridViewColumn column = dgvNhanVien.Columns[e.ColumnIndex];
            if (column == colThaoTac)
                ManagementGridRenderer.PaintActions(dgvNhanVien, e);
            else if (column == colChucVu)
            {
                string role = Convert.ToString(e.Value) ?? "";
                bool admin = role == "Quản lý";
                ManagementGridRenderer.PaintStatus(dgvNhanVien, e, role,
                    admin ? Color.FromArgb(237, 233, 254) : Color.FromArgb(219, 234, 254),
                    admin ? Color.FromArgb(91, 33, 182) : Color.FromArgb(30, 64, 175));
            }
            else if (column == colTrangThai)
            {
                string status = Convert.ToString(e.Value) ?? "";
                bool active = status == "Hoạt động";
                ManagementGridRenderer.PaintStatus(dgvNhanVien, e, status,
                    active ? Color.FromArgb(209, 250, 229) : Color.FromArgb(254, 226, 226),
                    active ? Color.FromArgb(6, 95, 70) : Color.FromArgb(153, 27, 27));
            }
        }

        private void dgvNhanVien_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.Button != MouseButtons.Left ||
                dgvNhanVien.Columns[e.ColumnIndex] != colThaoTac || loading || busy) return;
            dgvNhanVien.CurrentCell = dgvNhanVien.Rows[e.RowIndex].Cells[e.ColumnIndex];
            Rectangle cell = new Rectangle(Point.Empty, dgvNhanVien.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Size);
            if (ManagementGridRenderer.GetActionBounds(dgvNhanVien, cell, false).Contains(e.X, e.Y))
                btnSua.PerformClick();
            else if (ManagementGridRenderer.GetActionBounds(dgvNhanVien, cell, true).Contains(e.X, e.Y))
                btnXoa.PerformClick();
        }

        private void dgvNhanVien_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvNhanVien.Columns[e.ColumnIndex] != colThaoTac)
                btnSua.PerformClick();
        }

        private void dgvNhanVien_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvNhanVien.Columns[e.ColumnIndex] == colThaoTac)
                e.ToolTipText = "Đặt lại mật khẩu (bên trái) · Xóa tài khoản (bên phải)";
        }

        private void FrmNhanVien_KeyDown(object sender, KeyEventArgs e)
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
            if (busy) e.Cancel = true;
            base.OnFormClosing(e);
        }
    }
}
