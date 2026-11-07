using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLySuKienVaBanVe.BLL;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmQuanLySuKien : Form
    {
        private DataTable _dtSuKienGoc;
        private readonly Action updateListState;

        public FrmQuanLySuKien()
        {
            InitializeComponent();
            HieuUng.GanChoForm(this);
            this.DoubleBuffered = true;
            ManagementFormLayout.Attach(pnlHeader, txtTimKiem, btnThem, btnSua, btnXoa);

            updateListState = ManagementUi.AttachListState(dgvSuKien, txtTimKiem, btnSua, btnXoa);
            txtTimKiem.TextChanged += (s, e) => ApDungBoLocSuKien();
        }

        private void FrmQuanLySuKien_Load(object sender, EventArgs e) { LoadData(); }

        private void LoadData()
        {
            try
            {
                _dtSuKienGoc = SuKienBLL.LayDanhSach();
                ApDungBoLocSuKien();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải danh sách sự kiện: " + ex.Message); }
        }

        private void ApDungBoLocSuKien()
        {
            if (_dtSuKienGoc == null) return;
            string keyword = txtTimKiem.Text.Trim();

            dgvSuKien.Rows.Clear();
            foreach (DataRow row in _dtSuKienGoc.Rows)
            {
                string tenSK = row["TenSK"]?.ToString() ?? "";
                string loaiSK = row["LoaiSK"]?.ToString() ?? "";
                string moTa = row["MoTa"]?.ToString() ?? "";
                string trangThai = row["TrangThai"]?.ToString() ?? "";

                if (!string.IsNullOrEmpty(keyword))
                {
                    bool match = ManagementUi.Matches(keyword, tenSK, loaiSK, moTa, trangThai, ManagementUi.StatusText(trangThai));
                    if (!match) continue;
                }

                int idx = dgvSuKien.Rows.Add();
                dgvSuKien.Rows[idx].Cells["colMaSK"].Value = row["MaSK"];
                dgvSuKien.Rows[idx].Cells["colTenSK"].Value = tenSK;
                dgvSuKien.Rows[idx].Cells["colLoaiSK"].Value = loaiSK;
                dgvSuKien.Rows[idx].Cells["colMoTa"].Value = moTa;
                dgvSuKien.Rows[idx].Cells["colTrangThai"].Value = trangThai;
            }
            updateListState();
        }

        // =================== THÊM / SỬA / XÓA ===================
        private void btnThem_Click(object sender, EventArgs e)
        {
            using (FrmChiTietSuKien frm = new FrmChiTietSuKien())
            {
                if (frm.ShowDialog() == DialogResult.OK) LoadData();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvSuKien.CurrentRow == null) { MessageBox.Show("Vui lòng chọn sự kiện trên bảng!"); return; }
            int maSK = Convert.ToInt32(dgvSuKien.CurrentRow.Cells["colMaSK"].Value);
            using (FrmChiTietSuKien frm = new FrmChiTietSuKien(maSK))
            {
                if (frm.ShowDialog() == DialogResult.OK) LoadData();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvSuKien.CurrentRow == null) { MessageBox.Show("Vui lòng chọn sự kiện trên bảng!"); return; }
            int maSK = Convert.ToInt32(dgvSuKien.CurrentRow.Cells["colMaSK"].Value);

            if (MessageBox.Show("Bạn có chắc muốn xóa sự kiện này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string result = SuKienBLL.Xoa(maSK);
                if (result == "Success")
                {
                    LoadData();
                }
                else
                {
                    MessageBox.Show(result, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // Hit-test the same rectangles used for painting, including the vertical position.
        private void dgvSuKien_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.Button != MouseButtons.Left) return;
            if (dgvSuKien.Columns[e.ColumnIndex].Name != "colThaoTac") return;

            dgvSuKien.CurrentCell = dgvSuKien.Rows[e.RowIndex].Cells[e.ColumnIndex];
            Rectangle cell = new Rectangle(Point.Empty,
                dgvSuKien.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Size);
            Point location = new Point(e.X, e.Y);
            if (ManagementGridRenderer.GetActionBounds(dgvSuKien, cell, false).Contains(location))
                btnSua_Click(sender, EventArgs.Empty);
            else if (ManagementGridRenderer.GetActionBounds(dgvSuKien, cell, true).Contains(location))
                btnXoa_Click(sender, EventArgs.Empty);
        }

        private void dgvSuKien_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            string column = dgvSuKien.Columns[e.ColumnIndex].Name;
            if (column == "colTrangThai")
            {
                string status = Convert.ToString(e.Value);
                Color badgeColor = Color.FromArgb(241, 245, 249);
                Color textColor = Color.FromArgb(71, 85, 105);
                string displayStatus = ManagementUi.StatusText(status);
                if (status == "DangDien") { badgeColor = Color.FromArgb(209, 250, 229); textColor = Color.FromArgb(6, 95, 70); }
                else if (status == "SapDien") { badgeColor = Color.FromArgb(219, 234, 254); textColor = Color.FromArgb(30, 64, 175); }
                else if (status == "DaKetThuc") { badgeColor = Color.FromArgb(241, 245, 249); textColor = Color.FromArgb(71, 85, 105); }
                else if (status == "DaHuy") { badgeColor = Color.FromArgb(254, 226, 226); textColor = Color.FromArgb(153, 27, 27); }
                ManagementGridRenderer.PaintStatus(dgvSuKien, e, displayStatus, badgeColor, textColor);
            }
            else if (column == "colThaoTac")
            {
                ManagementGridRenderer.PaintActions(dgvSuKien, e);
            }
        }
    }
}
