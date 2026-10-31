using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;
using QuanLySuKienVaBanVe.DTO;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmNhanVien : Form
    {
        private const string Mask = "********";

        private readonly HashSet<int> revealed = new HashSet<int>();
        private int requestVersion;
        private bool loading;
        private bool deleting;

        public FrmNhanVien()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ManagementFormLayout.Attach(pnlHeader, txtTimKiem, btnThem, btnSua, btnXoa);
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
                revealed.Clear();
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

        private void UpdateActions()
        {
            bool hasSelection = !loading && !deleting && dgvNhanVien.CurrentRow != null;
            btnThem.Enabled = !loading && !deleting;
            btnSua.Enabled = hasSelection;
            btnXoa.Enabled = hasSelection;
            btnLamMoi.Enabled = !deleting;
            txtTimKiem.Enabled = !deleting;
            dgvNhanVien.Enabled = !loading && !deleting;
        }

        private TaiKhoanDTO SelectedAccount()
        {
            var row = dgvNhanVien.CurrentRow;
            if (row == null || loading || deleting) return null;
            DataRowView view = row.DataBoundItem as DataRowView;
            return new TaiKhoanDTO
            {
                MaTK = Convert.ToInt32(row.Cells["colMaTK"].Value),
                TenDangNhap = Convert.ToString(row.Cells["colTenTK"].Value),
                VaiTro = view == null ? VaiTro.NhanVien : Convert.ToString(view["VaiTro"])
            };
        }

        private int Scale(int pixels)
        {
            return (int)Math.Round(pixels * DeviceDpi / 96f);
        }

        private Rectangle EyeBounds(Rectangle cell)
        {
            int size = Scale(28);
            return new Rectangle(cell.Right - Scale(12) - size, cell.Top + (cell.Height - size) / 2, size, size);
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
            if (loading || deleting) return;
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

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            TaiKhoanDTO account = SelectedAccount();
            if (account == null) return;
            if (MessageBox.Show(this, string.Format("Xóa tài khoản “{0}”?\nTài khoản đã có đơn vé hoặc nhật ký sẽ không thể xóa.",
                account.TenDangNhap), "Xác nhận xóa tài khoản", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            deleting = true;
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
                        MessageBox.Show(this, "Tài khoản này đã phát sinh đơn vé hoặc nhật ký nên không thể xóa.", "Không thể xóa",
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
                deleting = false;
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
                    admin ? Color.FromArgb(237, 233, 254) : Color.FromArgb(209, 250, 229),
                    admin ? Color.FromArgb(91, 33, 182) : Color.FromArgb(6, 95, 70));
            }
            else if (column == colMatKhau)
                PaintPassword(e);
        }

        private void PaintPassword(DataGridViewCellPaintingEventArgs e)
        {
            e.Paint(e.ClipBounds, DataGridViewPaintParts.Background |
                DataGridViewPaintParts.SelectionBackground | DataGridViewPaintParts.Border);

            int maTK = Convert.ToInt32(dgvNhanVien.Rows[e.RowIndex].Cells["colMaTK"].Value);
            bool visible = revealed.Contains(maTK);
            string text = visible ? (Convert.ToString(e.Value) ?? "") : Mask;
            Rectangle eye = EyeBounds(e.CellBounds);
            Rectangle textBounds = new Rectangle(e.CellBounds.Left + Scale(16), e.CellBounds.Top,
                Math.Max(0, eye.Left - e.CellBounds.Left - Scale(24)), e.CellBounds.Height);
            bool selected = (e.State & DataGridViewElementStates.Selected) != 0;
            Color foreground = selected ? e.CellStyle.SelectionForeColor : e.CellStyle.ForeColor;

            GraphicsState state = e.Graphics.Save();
            try
            {
                e.Graphics.SetClip(Rectangle.Intersect(e.CellBounds, e.ClipBounds), CombineMode.Intersect);
                TextRenderer.DrawText(e.Graphics, text, e.CellStyle.Font, textBounds, foreground,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
                    TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Fx.IconEye(e.Graphics, new RectangleF(eye.X + 3, eye.Y + 3, eye.Width - 6, eye.Height - 6),
                    visible ? Fx.Blue : Fx.Muted, !visible);
            }
            finally
            {
                e.Graphics.Restore(state);
                e.Handled = true;
            }
        }

        private Rectangle CellOrigin(int columnIndex, int rowIndex)
        {
            return new Rectangle(Point.Empty, dgvNhanVien.GetCellDisplayRectangle(columnIndex, rowIndex, false).Size);
        }

        private void dgvNhanVien_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.Button != MouseButtons.Left || loading || deleting) return;
            DataGridViewColumn column = dgvNhanVien.Columns[e.ColumnIndex];
            Rectangle cell = CellOrigin(e.ColumnIndex, e.RowIndex);

            if (column == colMatKhau)
            {
                if (!EyeBounds(cell).Contains(e.X, e.Y)) return;
                int maTK = Convert.ToInt32(dgvNhanVien.Rows[e.RowIndex].Cells["colMaTK"].Value);
                if (!revealed.Remove(maTK)) revealed.Add(maTK);
                dgvNhanVien.InvalidateCell(e.ColumnIndex, e.RowIndex);
                return;
            }

            if (column != colThaoTac) return;
            dgvNhanVien.CurrentCell = dgvNhanVien.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (ManagementGridRenderer.GetActionBounds(dgvNhanVien, cell, false).Contains(e.X, e.Y))
                btnSua.PerformClick();
            else if (ManagementGridRenderer.GetActionBounds(dgvNhanVien, cell, true).Contains(e.X, e.Y))
                btnXoa.PerformClick();
        }

        private void dgvNhanVien_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            bool overEye = e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvNhanVien.Columns[e.ColumnIndex] == colMatKhau &&
                EyeBounds(CellOrigin(e.ColumnIndex, e.RowIndex)).Contains(e.X, e.Y);
            dgvNhanVien.Cursor = overEye ? Cursors.Hand : Cursors.Default;
        }

        private void dgvNhanVien_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dgvNhanVien.Cursor = Cursors.Default;
        }

        private void dgvNhanVien_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dgvNhanVien.Columns[e.ColumnIndex] != colThaoTac && dgvNhanVien.Columns[e.ColumnIndex] != colMatKhau)
                btnSua.PerformClick();
        }

        private void dgvNhanVien_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            DataGridViewColumn column = dgvNhanVien.Columns[e.ColumnIndex];
            if (column == colThaoTac)
                e.ToolTipText = "Đặt lại mật khẩu (bên trái) · Xóa tài khoản (bên phải)";
            else if (column == colMatKhau)
                e.ToolTipText = "Bấm biểu tượng mắt để hiện hoặc ẩn mật khẩu";
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
            if (deleting) e.Cancel = true;
            base.OnFormClosing(e);
        }
    }
}
