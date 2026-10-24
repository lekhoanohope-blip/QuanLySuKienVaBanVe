using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using QuanLySuKienVaBanVe.BLL;

namespace QuanLySuKienVaBanVe.GUI
{
    public partial class FrmQuanLySuatDien : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int? MaSKCanLoc { get; set; }

        public FrmQuanLySuatDien()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            ManagementFormLayout.Attach(pnlHeader, txtTimKiem, btnThem, btnSua, btnXoa);
        }

        private void FrmQuanLySuatDien_Load(object sender, EventArgs e) { LoadData(); }

        private void LoadData()
        {
            try
            {
                DataTable dt = SuatDienBLL.LayDanhSach();

                dgvSuatDien.Rows.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    int idx = dgvSuatDien.Rows.Add();
                    dgvSuatDien.Rows[idx].Cells["colMaSuat"].Value = row["MaSuat"];
                    dgvSuatDien.Rows[idx].Cells["colTenSK"].Value = row["TenSK"];
                    dgvSuatDien.Rows[idx].Cells["colThoiGian"].Value = Convert.ToDateTime(row["ThoiGianBatDau"]).ToString("dd/MM/yyyy HH:mm");
                    dgvSuatDien.Rows[idx].Cells["colDiaDiem"].Value = row["DiaDiem"];
                    dgvSuatDien.Rows[idx].Cells["colTrangThai"].Value = row["TrangThai"];
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải danh sách suất diễn: " + ex.Message); }
        }

        // =================== THÊM / SỬA / XÓA ===================
        private void btnThem_Click(object sender, EventArgs e)
        {
            using (FrmChiTietSuatDien frm = new FrmChiTietSuatDien())
            {
                if (frm.ShowDialog() == DialogResult.OK) LoadData();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvSuatDien.CurrentRow == null) { MessageBox.Show("Vui lòng chọn suất diễn trên bảng!"); return; }
            int maSuat = Convert.ToInt32(dgvSuatDien.CurrentRow.Cells["colMaSuat"].Value);
            using (FrmChiTietSuatDien frm = new FrmChiTietSuatDien(maSuat))
            {
                if (frm.ShowDialog() == DialogResult.OK) LoadData();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvSuatDien.CurrentRow == null) { MessageBox.Show("Vui lòng chọn suất diễn trên bảng!"); return; }
            int maSuat = Convert.ToInt32(dgvSuatDien.CurrentRow.Cells["colMaSuat"].Value);

            if (MessageBox.Show("Bạn có chắc muốn xóa suất diễn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string result = SuatDienBLL.Xoa(maSuat);
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
        private void dgvSuatDien_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.Button != MouseButtons.Left) return;
            if (dgvSuatDien.Columns[e.ColumnIndex].Name != "colThaoTac") return;

            dgvSuatDien.CurrentCell = dgvSuatDien.Rows[e.RowIndex].Cells[e.ColumnIndex];
            Rectangle cell = new Rectangle(Point.Empty,
                dgvSuatDien.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Size);
            Point location = new Point(e.X, e.Y);
            if (ManagementGridRenderer.GetActionBounds(dgvSuatDien, cell, false).Contains(location))
                btnSua_Click(sender, EventArgs.Empty);
            else if (ManagementGridRenderer.GetActionBounds(dgvSuatDien, cell, true).Contains(location))
                btnXoa_Click(sender, EventArgs.Empty);
        }

        private void dgvSuatDien_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            string column = dgvSuatDien.Columns[e.ColumnIndex].Name;
            if (column == "colTrangThai")
            {
                string status = Convert.ToString(e.Value);
                Color badgeColor = Color.FromArgb(241, 245, 249);
                Color textColor = Color.FromArgb(71, 85, 105);
                string displayStatus = status;
                if (status == "MoCuaBanVe") { badgeColor = Color.FromArgb(209, 250, 229); textColor = Color.FromArgb(6, 95, 70); displayStatus = "Mở bán vé"; }
                else if (status == "DongBanVe") { badgeColor = Color.FromArgb(254, 243, 199); textColor = Color.FromArgb(146, 64, 14); displayStatus = "Đóng bán vé"; }
                else if (status == "DangDien") { badgeColor = Color.FromArgb(219, 234, 254); textColor = Color.FromArgb(30, 64, 175); displayStatus = "Đang diễn ra"; }
                else if (status == "DaKetThuc") { badgeColor = Color.FromArgb(241, 245, 249); textColor = Color.FromArgb(71, 85, 105); displayStatus = "Đã kết thúc"; }
                ManagementGridRenderer.PaintStatus(dgvSuatDien, e, displayStatus, badgeColor, textColor);
            }
            else if (column == "colThaoTac")
            {
                ManagementGridRenderer.PaintActions(dgvSuatDien, e);
            }
        }
    }
}
